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
namespace Sys.Workflow.Engine.Impl.Bpmn.Listeners
{

    using Sys.Workflow.Bpmn.Models;

    /// 
    public class TransactionDependentExecutionListenerExecutionScope
    {

        protected internal readonly string processInstanceId;
        protected internal readonly string executionId;
        protected internal readonly FlowElement flowElement;
        protected internal readonly IDictionary<string, object> executionVariables;
        protected internal readonly IDictionary<string, object> customPropertiesMap;

        public TransactionDependentExecutionListenerExecutionScope(string processInstanceId, string executionId, FlowElement flowElement, IDictionary<string, object> executionVariables, IDictionary<string, object> customPropertiesMap)
        {
            this.processInstanceId = processInstanceId;
            this.executionId = executionId;
            this.flowElement = flowElement;
            this.executionVariables = executionVariables;
            this.customPropertiesMap = customPropertiesMap;
        }

        public virtual string ProcessInstanceId
        {
            get
            {
                return processInstanceId;
            }
        }

        public virtual string ExecutionId
        {
            get
            {
                return executionId;
            }
        }

        public virtual FlowElement FlowElement
        {
            get
            {
                return flowElement;
            }
        }

        public virtual IDictionary<string, object> ExecutionVariables
        {
            get
            {
                return executionVariables;
            }
        }

        public virtual IDictionary<string, object> CustomPropertiesMap
        {
            get
            {
                return customPropertiesMap;
            }
        }
    }
}