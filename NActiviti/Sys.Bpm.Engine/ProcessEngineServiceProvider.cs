﻿using DatabaseSchemaReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using org.activiti.engine;
using org.activiti.engine.impl;
using org.activiti.engine.impl.agenda;
using org.activiti.engine.impl.asyncexecutor;
using org.activiti.engine.impl.cfg;
using org.activiti.engine.impl.db;
using org.activiti.engine.impl.util;
using org.activiti.validation;
using SmartSql;
using SmartSql.Abstractions;
using SmartSql.DbSession;
using Spring.Core;
using Spring.Core.TypeResolution;
using Sys.Bpm;
using Sys.Bpm.Engine.impl;
using Sys.Data;
using Sys.Net.Http;
using Sys.Workflow.Engine.Bpmn.Rules;
using System;
using System.Data.Common;
using System.IO;
using System.Net.Http;

namespace Sys
{
    public static class ProcessEngineServiceProvider
    {
        public static IServiceCollection AddProcessEngine(this IServiceCollection services)
        {
            TypeRegistry.RegisterType(typeof(CollectionUtil));

            services.AddHttpClient<HttpClient>()
                .ConfigureHttpMessageHandlerBuilder(builder =>
                {
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.MaxRequestContentBufferSize = int.MaxValue;
                    handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                    handler.UseDefaultCredentials = true;
                    handler.ServerCertificateCustomValidationCallback += (s, arg1, arg2, arg3) => true;

                    builder.PrimaryHandler = handler;
                    builder.Build();
                });

            IGetBookmarkRuleProvider getBookmarkRuleProvider = new GetBookmarkRuleProvider();

            services.AddSingleton<IGetBookmarkRuleProvider>(getBookmarkRuleProvider);

            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            DbSqlSessionVersion.InitVersion(configuration);

            CreateProcessValidator(services);

            services.Configure<ProcessEngineOption>(configuration);

            services.Configure<DataSourceOption>(configuration.GetSection("SysDataSource"));

            Action<IDataSource> updateSmartSqlDbSession = (dataSource) =>
            {
                ISmartSqlMapper ssm = serviceProvider.GetService<ISmartSqlMapper>();

                ssm.SmartSqlOptions.DbSessionStore = new DbConnectionSessionStore(serviceProvider.GetService<ILoggerFactory>(), dataSource.DbProviderFactory);

                ssm.SmartSqlOptions.SmartSqlContext.Database.WriteDataSource.ConnectionString = dataSource.ConnectionString;
                foreach (var ds in ssm.SmartSqlOptions.SmartSqlContext.Database.ReadDataSources)
                {
                    ds.ConnectionString = dataSource.ConnectionString;
                }
            };

            services.AddSingleton<IDataSource>(sp =>
            {
                return new Data.DataSource(sp.GetService<IOptionsMonitor<DataSourceOption>>(), updateSmartSqlDbSession);
            });

            services.AddTransient<IDatabaseReader>(sp =>
             {
                 var ds = sp.GetService<IDataSource>();

                 DatabaseReader reader = new DatabaseReader(ds.Connection as DbConnection);

                 switch (reader.DatabaseSchema.Provider)
                 {
                     case "MySql.Data.MySqlClient":
                         reader.Owner = ds.Connection.Database;
                         break;
                 }

                 return reader;
             });

            services.AddSingleton<ISmartSqlMapper>(sp =>
            {
                var codebase = Path.GetDirectoryName(typeof(ProcessEngineConfiguration).Assembly.Location);

                IDataSource dataSource = sp.GetService<IDataSource>();

                var dbSessionStore = new DbConnectionSessionStore(sp.GetService<ILoggerFactory>(), dataSource.DbProviderFactory);

                SmartSqlOptions options = new SmartSqlOptions
                {
                    ConfigPath = Path.Combine(codebase, DEFAULT_MYBATIS_MAPPING_FILE),
                    DbSessionStore = dbSessionStore
                };

                SmartSqlMapper ssm = new SmartSqlMapper(options);

                options.SmartSqlContext.Database.WriteDataSource.ConnectionString = dataSource.ConnectionString;
                foreach (var ds in options.SmartSqlContext.Database.ReadDataSources)
                {
                    ds.ConnectionString = dataSource.ConnectionString;
                }

                return ssm;
            });

            services.AddTransient<IActivitiEngineAgendaFactory>(sp => new DefaultActivitiEngineAgendaFactory());

            services.AddSingleton<IIdGenerator>(sp => new GuidGenerator());

            services.AddSingleton<IBpmnParseFactory, DefaultBpmnParseFactory>();

            services.AddTransient<IAsyncExecutor>(sp =>
            {
                IConfigurationSection dajw = sp.GetService<IConfiguration>().GetSection("defaultAsyncJobAcquireWaitTimeInMillis");
                IConfigurationSection dtjw = sp.GetService<IConfiguration>().GetSection("defaultTimerJobAcquireWaitTimeInMillis");

                if (int.TryParse(dajw?.Value, out int iDajw) == false)
                {
                    iDajw = 10000;
                }
                if (int.TryParse(dtjw?.Value, out int iDtjw) == false)
                {
                    iDtjw = 10000;
                }

                return new DefaultAsyncJobExecutor()
                {
                    DefaultAsyncJobAcquireWaitTimeInMillis = iDajw,
                    DefaultTimerJobAcquireWaitTimeInMillis = iDtjw,
                };
            });

            services.AddSingleton<ProcessEngineConfiguration>(sp =>
            {
                return new StandaloneProcessEngineConfiguration(
                    new HistoryServiceImpl(),
                    new TaskServiceImpl(),
                    new DynamicBpmnServiceImpl(),
                    new RepositoryServiceImpl(),
                    new RuntimeServiceImpl(),
                    new ManagementServiceImpl(),
                    sp.GetService<IAsyncExecutor>(),
                    sp.GetService<IConfiguration>()
                );
            });

            services.AddSingleton<ProcessEngineFactory>(sp =>
            {
                return ProcessEngineFactory.Instance;
            });

            services.AddSingleton<IAccessTokenProvider>(sp => AccessTokenProvider.Create(sp.GetService<ILoggerFactory>()));

            services.AddTransient<IHttpClientProxy>(sp =>
            {
                return new HttpClientProxy(sp.GetService<IHttpClientFactory>().CreateClient(),
                    sp.GetService<IAccessTokenProvider>(),
                    sp.GetService<IHttpContextAccessor>().HttpContext);
            });

            services.AddSingleton<IUserServiceProxy>(sp =>
            {
                var cfg = sp.GetService<IConfiguration>();

                return new UserServiceProxy(cfg["userServiceProxyBaseUrl"], sp.GetService<IHttpClientProxy>());
            });

            services.AddTransient<IRepositoryService>(sp => sp.GetRequiredService<IProcessEngine>().RepositoryService);

            services.AddTransient<IRuntimeService>(sp => sp.GetRequiredService<IProcessEngine>().RuntimeService);

            services.AddTransient<IManagementService>(sp => sp.GetRequiredService<IProcessEngine>().ManagementService);

            services.AddTransient<IHistoryService>(sp => sp.GetRequiredService<IProcessEngine>().HistoryService);

            services.AddTransient<ITaskService>(sp => sp.GetRequiredService<IProcessEngine>().TaskService);

            services.AddTransient<IDynamicBpmnService>(sp => sp.GetRequiredService<IProcessEngine>().DynamicBpmnService);

            services.AddTransient<IProcessEngine>(sp =>
            {
                return sp.GetService<ProcessEngineFactory>().DefaultProcessEngine;
            });

            ServiceProvider servicePprovider = services.BuildServiceProvider();
            servicePprovider.EnsureProcessEngineInit();

            services.AddSpringCoreService(servicePprovider.GetService<ILoggerFactory>());
            services.AddBpmModelServiceProvider();

            servicePprovider.GetService<IOptionsMonitor<ProcessEngineOption>>()
                .OnChange((opts, prop) =>
                {
                    if (ProcessEngineOption.HasChanged())
                    {
                        ProcessEngineFactory.Instance.destroy();
                        servicePprovider.EnsureProcessEngineInit();
                    }
                });

            return services;
        }

        private static IServiceCollection CreateProcessValidator(IServiceCollection services)
        {
            IProcessValidator processValidator = new ProcessValidatorFactory().createProcessValidator();

            services.AddSingleton<IProcessValidator>(processValidator);

            return services;
        }

        private static IServiceProvider serviceProvider;

        private static void EnsureProcessEngineInit(this IServiceProvider serviceProvider)
        {
            ProcessEngineServiceProvider.serviceProvider = serviceProvider;

            var engine = serviceProvider.GetService<IProcessEngine>();
            if (engine == null)
            {
                throw new InitProcessEngineFaliedException();
            }
        }

        public static T Resolve<T>()
        {
            return serviceProvider.GetService<T>();
        }

        private static string DEFAULT_MYBATIS_MAPPING_FILE = "resources/db/mapping/mappings.xml";

        public static ILogger<T> LoggerService<T>()
        {
            ILoggerFactory logFac = serviceProvider.GetService<ILoggerFactory>();
            if (logFac != null)
            {
                return logFac.CreateLogger<T>();
            }

            return new NoneLogger<T>();
        }
    }

    public class NoneLogger<T> : ILogger<T>, IDisposable
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

        }
    }

    [System.Serializable]
    public sealed class InitProcessEngineFaliedException : Exception
    {
        public InitProcessEngineFaliedException()
        {
        }

        public InitProcessEngineFaliedException(string message) : base(message)
        {
        }

        public InitProcessEngineFaliedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
