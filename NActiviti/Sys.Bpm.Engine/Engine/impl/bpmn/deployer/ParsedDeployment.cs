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
namespace Sys.Workflow.Engine.Impl.Bpmn.Deployers
{

    using Sys.Workflow.Bpmn.Models;
    using Sys.Workflow.Engine.Impl.Bpmn.Parser;
    using Sys.Workflow.Engine.Impl.Persistence.Entity;

    /// <summary>
    /// An intermediate representation of a DeploymentEntity which keeps track of all of the entity's
    /// ProcessDefinitionEntities and resources, and BPMN parses, models, and processes associated
    /// with each ProcessDefinitionEntity - all produced by parsing the deployment.
    /// 
    /// The ProcessDefinitionEntities are expected to be "not fully set-up" - they may be inconsistent with the 
    /// DeploymentEntity and/or the persisted versions, and if the deployment is new, they will not yet be persisted.
    /// </summary>
    public class ParsedDeployment
    {

        protected internal IDeploymentEntity deploymentEntity;

        protected internal IList<IProcessDefinitionEntity> processDefinitions;
        protected internal IDictionary<IProcessDefinitionEntity, BpmnParse> mapProcessDefinitionsToParses;
        protected internal IDictionary<IProcessDefinitionEntity, IResourceEntity> mapProcessDefinitionsToResources;

        public ParsedDeployment(IDeploymentEntity entity, IList<IProcessDefinitionEntity> processDefinitions, IDictionary<IProcessDefinitionEntity, BpmnParse> mapProcessDefinitionsToParses, IDictionary<IProcessDefinitionEntity, IResourceEntity> mapProcessDefinitionsToResources)
        {
            this.deploymentEntity = entity;
            this.processDefinitions = processDefinitions;
            this.mapProcessDefinitionsToParses = mapProcessDefinitionsToParses;
            this.mapProcessDefinitionsToResources = mapProcessDefinitionsToResources;
        }


        public virtual IDeploymentEntity Deployment
        {
            get
            {
                return deploymentEntity;
            }
        }

        public virtual IList<IProcessDefinitionEntity> AllProcessDefinitions
        {
            get
            {
                return processDefinitions;
            }
        }

        public virtual IResourceEntity GetResourceForProcessDefinition(IProcessDefinitionEntity processDefinition)
        {
            mapProcessDefinitionsToResources.TryGetValue(processDefinition, out var res);

            return res;
        }

        public virtual BpmnParse GetBpmnParseForProcessDefinition(IProcessDefinitionEntity processDefinition)
        {
            mapProcessDefinitionsToParses.TryGetValue(processDefinition, out var parser);

            return parser;
        }

        public virtual BpmnModel GetBpmnModelForProcessDefinition(IProcessDefinitionEntity processDefinition)
        {
            BpmnParse parse = GetBpmnParseForProcessDefinition(processDefinition);

            return (parse?.BpmnModel);
        }

        public virtual Process GetProcessModelForProcessDefinition(IProcessDefinitionEntity processDefinition)
        {
            BpmnModel model = GetBpmnModelForProcessDefinition(processDefinition);

            return (model?.GetProcessById(processDefinition.Key));
        }
    }
}