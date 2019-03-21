﻿using Microsoft.Extensions.Logging;
using org.activiti.cloud.services.api.events;
using org.activiti.cloud.services.api.model;
using org.activiti.engine.@delegate.@event;
using org.activiti.engine.impl.persistence.entity;
using org.activiti.engine.task;
using Sys;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright 2018 Alfresco, Inc. and/or its affiliates.
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

namespace org.activiti.cloud.services.events.converter
{
    /// <summary>
    /// 
    /// </summary>
    public class EventConverterContext
    {
        private static readonly ILogger LOGGER = ProcessEngineServiceProvider.LoggerService<EventConverterContext>();

        /// <summary>
        /// ProcessInstance:
        /// </summary>
        public const string PROCESS_EVENT_PREFIX = "ProcessInstance:";
        /// <summary>
        /// Task:
        /// </summary>
        public const string TASK_EVENT_PREFIX = "Task:";
        /// <summary>
        /// TaskCandidateUser:
        /// </summary>
        public const string TASK_CANDIDATE_USER_EVENT_PREFIX = "TaskCandidateUser:";
        /// <summary>
        /// TaskCandidateGroup:
        /// </summary>
        public const string TASK_CANDIDATE_GROUP_EVENT_PREFIX = "TaskCandidateGroup:";
        /// <summary>
        /// ""
        /// </summary>
        public const string EVENT_PREFIX = "";

        private IDictionary<string, IEventConverter> convertersMap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convertersMap"></param>
        public EventConverterContext(IDictionary<string, IEventConverter> convertersMap)
        {
            this.convertersMap = convertersMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converters"></param>
        public EventConverterContext(ISet<IEventConverter> converters)
        {
            this.convertersMap = converters.ToDictionary(x => x.GetType().FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual IDictionary<string, IEventConverter> ConvertersMap
        {
            get
            {
                return convertersMap;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        public virtual IProcessEngineEvent from(IActivitiEvent activitiEvent)
        {
            IEventConverter converter = convertersMap[getPrefix(activitiEvent) + activitiEvent.Type];

            IProcessEngineEvent newEvent = null;
            if (converter != null)
            {
                newEvent = converter.from(activitiEvent);
            }
            else
            {
                LOGGER.LogDebug(">> Ommited Event Type: " + activitiEvent.GetType().FullName);
            }
            return newEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        public static string getPrefix(IActivitiEvent activitiEvent)
        {
            if (isProcessEvent(activitiEvent))
            {
                return PROCESS_EVENT_PREFIX;
            }
            else if (isTaskEvent(activitiEvent))
            {
                return TASK_EVENT_PREFIX;
            }
            else if (isIdentityLinkEntityEvent(activitiEvent))
            {
                IIdentityLink identityLinkEntity = (IIdentityLink)((IActivitiEntityEvent)activitiEvent).Entity;
                if (isCandidateUserEntity(identityLinkEntity))
                {
                    return TASK_CANDIDATE_USER_EVENT_PREFIX;
                }
                else if (isCandidateGroupEntity(identityLinkEntity))
                {
                    return TASK_CANDIDATE_GROUP_EVENT_PREFIX;
                }
            }

            return EVENT_PREFIX;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        private static bool isProcessEvent(IActivitiEvent activitiEvent)
        {
            bool isProcessEvent = false;
            if (activitiEvent is IActivitiEntityEvent)
            {
                object entity = ((IActivitiEntityEvent)activitiEvent).Entity;
                if (entity != null && entity.GetType().IsAssignableFrom(typeof(ProcessInstance)))
                {
                    isProcessEvent = !isExecutionEntityEvent(activitiEvent) || ((IExecutionEntity)entity).ProcessInstanceType;
                }
            }
            else if (activitiEvent.Type == ActivitiEventType.PROCESS_CANCELLED)
            {
                isProcessEvent = true;
            }

            return isProcessEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        private static bool isExecutionEntityEvent(IActivitiEvent activitiEvent)
        {
            return activitiEvent.Type == ActivitiEventType.ENTITY_SUSPENDED || activitiEvent.Type == ActivitiEventType.ENTITY_ACTIVATED || activitiEvent.Type == ActivitiEventType.ENTITY_CREATED;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        private static bool isTaskEvent(IActivitiEvent activitiEvent)
        {
            return activitiEvent is IActivitiEntityEvent && ((IActivitiEntityEvent)activitiEvent).Entity is TaskModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activitiEvent"></param>
        /// <returns></returns>
        private static bool isIdentityLinkEntityEvent(IActivitiEvent activitiEvent)
        {
            return activitiEvent is IActivitiEntityEvent && ((IActivitiEntityEvent)activitiEvent).Entity is IIdentityLink;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityLinkEntity"></param>
        /// <returns></returns>
        private static bool isCandidateUserEntity(IIdentityLink identityLinkEntity)
        {
            return string.Compare(IdentityLinkType.CANDIDATE, identityLinkEntity.Type, true) == 0 && identityLinkEntity.UserId != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityLinkEntity"></param>
        /// <returns></returns>
        private static bool isCandidateGroupEntity(IIdentityLink identityLinkEntity)
        {
            return string.Compare(IdentityLinkType.CANDIDATE, identityLinkEntity.Type, true) == 0 && identityLinkEntity.GroupId != null;
        }
    }

}