﻿/*
 * Copyright 2017 Alfresco, Inc. and/or its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Sys.Workflow.Cloud.Services.Events.Integration
{




    /// <summary>
    /// 
    /// </summary>
    public class IntegrationResultReceivedEventImpl : BaseIntegrationEventImpl, IIntegrationResultReceivedEvent
	{


        /// <summary>
        /// 
        /// </summary>
        //used for json deserialization
        public IntegrationResultReceivedEventImpl()
		{
        }

        /// <summary>
        /// 
        /// </summary>

        public IntegrationResultReceivedEventImpl(string appName, string appVersion, string serviceName, string serviceFullName, string serviceType, string serviceVersion, string executionId, string processDefinitionId, string processInstanceId, string integrationContextId, string flowNodeId) : base(appName,appVersion,serviceName,serviceFullName,serviceType,serviceVersion, executionId, processDefinitionId, processInstanceId, integrationContextId, flowNodeId)
		{
		}


        /// <summary>
        /// 
        /// </summary>
        public override string EventType
		{
			get
			{
				return "IntegrationResultReceivedEvent";
			}
		}
	}

}