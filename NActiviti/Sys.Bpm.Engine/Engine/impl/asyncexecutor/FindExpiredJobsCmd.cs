﻿using System.Collections.Generic;

/* Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Sys.Workflow.Engine.Impl.Asyncexecutor
{
    using Microsoft.Extensions.Logging;
    using Sys.Workflow.Engine.Impl.Interceptor;
    using Sys.Workflow.Engine.Impl.Persistence.Entity;
    using Sys.Workflow;
    using System;

    /// 
    public class FindExpiredJobsCmd : ICommand<IList<IJobEntity>>
    {
        private readonly ILogger<FindExpiredJobsCmd> logger = ProcessEngineServiceProvider.LoggerService<FindExpiredJobsCmd>();

        /// <summary>
        /// 
        /// </summary>
        protected internal int pageSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        public FindExpiredJobsCmd(int pageSize)
        {
            this.pageSize = pageSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        public virtual IList<IJobEntity> Execute(ICommandContext commandContext)
        {
            try
            {
                return commandContext.JobEntityManager.FindExpiredJobs(new Page(0, pageSize));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}