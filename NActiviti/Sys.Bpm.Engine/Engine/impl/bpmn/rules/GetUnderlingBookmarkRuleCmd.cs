///////////////////////////////////////////////////////////
//  GetUnderlingsBookmarkRuleCmd.cs
//  Implementation of the Class GetUnderlingsBookmarkRuleCmd
//  Generated by Enterprise Architect
//  Created on:      30-1月-2019 8:32:00
//  Original author: 张楠
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using org.activiti.engine.impl.interceptor;

namespace Sys.Workflow.Engine.Bpmn.Rules
{
    /// <summary>
    /// 获取用户的所有下属及子下属
    /// </summary>
    [GetBookmarkDescriptor("GetUnderling")]
	public class GetUnderlingBookmarkRuleCmd : IGetBookmarkRule
    {

        public GetUnderlingBookmarkRuleCmd()
        {

        }

        public QueryBookmark Condition { get; set; }

        public IList<IUserInfo> execute(ICommandContext commandContext)
        {
            IUserServiceProxy proxy = ProcessEngineServiceProvider.Resolve<IUserServiceProxy>();
            return proxy.GetUsers(Condition).Result;
        }
    }
}