﻿using System;
using System.Collections.Generic;

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
namespace Sys.Workflow.Engine.Impl.Persistence.Entity.Data.Impl
{

    using Sys.Workflow.Engine.Impl.Cfg;
    using Sys.Workflow.Engine;

    /// 
    public class MybatisByteArrayDataManager : AbstractDataManager<IByteArrayEntity>, IByteArrayDataManager
    {

        public MybatisByteArrayDataManager(ProcessEngineConfigurationImpl processEngineConfiguration) : base(processEngineConfiguration)
        {
        }

        public override IByteArrayEntity Create()
        {
            return new ByteArrayEntityImpl();
        }

        public override Type ManagedEntityClass
        {
            get
            {
                return typeof(ByteArrayEntityImpl);
            }
        }

        public virtual IList<IByteArrayEntity> FindAll()
        {
            return DbSqlSession.SelectList<ByteArrayEntityImpl, IByteArrayEntity>("selectByteArrays");
        }

        public virtual void DeleteByteArrayNoRevisionCheck(string byteArrayEntityId)
        {
            DbSqlSession.Delete("deleteByteArrayNoRevisionCheck", new KeyValuePair<string, object>("id", byteArrayEntityId), typeof(ByteArrayEntityImpl));
        }

    }

}