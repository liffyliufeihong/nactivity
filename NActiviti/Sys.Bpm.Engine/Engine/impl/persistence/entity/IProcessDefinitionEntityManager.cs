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
namespace Sys.Workflow.Engine.Impl.Persistence.Entity
{

    using Sys.Workflow.Engine.Repository;

    /// 
    public interface IProcessDefinitionEntityManager : IEntityManager<IProcessDefinitionEntity>
    {

        IProcessDefinitionEntity FindLatestProcessDefinitionByKey(string processDefinitionKey);

        IProcessDefinitionEntity FindLatestProcessDefinitionByKeyAndTenantId(string processDefinitionKey, string tenantId);

        IList<IProcessDefinition> FindProcessDefinitionsByQueryCriteria(IProcessDefinitionQuery processDefinitionQuery, Page page);

        long FindProcessDefinitionCountByQueryCriteria(IProcessDefinitionQuery processDefinitionQuery);

        IProcessDefinitionEntity FindProcessDefinitionByDeploymentAndKey(string deploymentId, string processDefinitionKey);

        IProcessDefinitionEntity FindProcessDefinitionByDeploymentAndKeyAndTenantId(string deploymentId, string processDefinitionKey, string tenantId);

        IProcessDefinition FindProcessDefinitionByKeyAndVersionAndTenantId(string processDefinitionKey, int? processDefinitionVersion, string tenantId);

        IList<IProcessDefinition> FindProcessDefinitionsByNativeQuery(IDictionary<string, object> parameterMap, int firstResult, int maxResults);

        long FindProcessDefinitionCountByNativeQuery(IDictionary<string, object> parameterMap);

        void UpdateProcessDefinitionTenantIdForDeployment(string deploymentId, string newTenantId);

        void DeleteProcessDefinitionsByDeploymentId(string deploymentId);
    }
}