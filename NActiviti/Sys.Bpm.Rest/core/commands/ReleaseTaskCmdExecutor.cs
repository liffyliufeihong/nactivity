﻿using Sys.Workflow.Cloud.Services.Api.Commands;
using Sys.Workflow.Cloud.Services.Api.Commands.Results;
using Sys.Workflow.Messaging;
using Sys.Workflow.Messaging.Support;
using System;

namespace Sys.Workflow.Cloud.Services.Core.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseTaskCmdExecutor : ICommandExecutor<ReleaseTaskCmd>
    {

        private readonly ProcessEngineWrapper processEngine;
        private readonly IMessageChannel<ReleaseTaskResults> commandResults;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processEngine"></param>
        /// <param name="commandResults"></param>
        public ReleaseTaskCmdExecutor(ProcessEngineWrapper processEngine, IMessageChannel<ReleaseTaskResults> commandResults)
        {
            this.processEngine = processEngine;
            this.commandResults = commandResults;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type HandledType
        {
            get
            {
                return typeof(ReleaseTaskCmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public virtual void Execute(ReleaseTaskCmd cmd)
        {
            processEngine.ReleaseTask(cmd);
            ReleaseTaskResults cmdResult = new ReleaseTaskResults(cmd.Id);
            commandResults.Send(MessageBuilder<ReleaseTaskResults>.WithPayload(cmdResult).Build());
        }
    }

}