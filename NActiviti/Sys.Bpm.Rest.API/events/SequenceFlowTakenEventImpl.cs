﻿/*
 * Copyright 2018 Alfresco and/or its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
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
 *
 */

namespace Sys.Workflow.Cloud.Services.Events
{

    /// <summary>
    /// 
    /// </summary>
    public class SequenceFlowTakenEventImpl : AbstractProcessEngineEvent, ISequenceFlowTakenEvent
    {
        private readonly string sequenceFlowId;
        private readonly string sourceActivityId;
        private readonly string sourceActivityName;
        private readonly string sourceActivityType;
        private readonly string targetActivityId;
        private readonly string targetActivityName;
        private readonly string targetActivityType;


        /// <summary>
        /// 
        /// </summary>
        public SequenceFlowTakenEventImpl(string appName, string appVersion, string serviceName, string serviceFullName, string serviceType, string serviceVersion, string executionId, string processDefinitionId, string processInstanceId, string sequenceFlowId, string sourceActivityId, string sourceActivityName, string sourceActivityType, string targetActivityId, string targetActivityName, string targetActivityType) : base(appName, appVersion, serviceName, serviceFullName, serviceType, serviceVersion, executionId, processDefinitionId, processInstanceId)
        {
            this.sequenceFlowId = sequenceFlowId;
            this.sourceActivityId = sourceActivityId;
            this.sourceActivityName = sourceActivityName;
            this.sourceActivityType = sourceActivityType;
            this.targetActivityId = targetActivityId;
            this.targetActivityName = targetActivityName;
            this.targetActivityType = targetActivityType;
        }


        /// <summary>
        /// 
        /// </summary>
        public override string EventType
        {
            get
            {
                return "SequenceFlowTakenEvent";
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string SequenceFlowId
        {
            get
            {
                return sequenceFlowId;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string SourceActivityId
        {
            get
            {
                return sourceActivityId;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string SourceActivityName
        {
            get
            {
                return sourceActivityName;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string SourceActivityType
        {
            get
            {
                return sourceActivityType;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual string TargetActivityId
        {
            get
            {
                return targetActivityId;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string TargetActivityName
        {
            get
            {
                return targetActivityName;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual string TargetActivityType
        {
            get
            {
                return targetActivityType;
            }
        }
    }

}