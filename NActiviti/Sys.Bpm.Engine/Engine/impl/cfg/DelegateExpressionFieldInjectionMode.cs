﻿/* Licensed under the Apache License, Version 2.0 (the "License");
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
using Sys.Workflow.Engine.Delegate;

namespace Sys.Workflow.Engine.Impl.Cfg
{
    /// <summary>
    /// 
    /// </summary>
    public enum DelegateExpressionFieldInjectionMode
    {

        /// <summary>
        /// This is the pre version 5.21 mode: field expressions are allowed and the
        /// only way to inject values.
        /// 
        /// Using the
        /// <seealso cref="DelegateHelper"/>
        /// method is not possible when using this mode, unless the Expressions are still defined
        /// as members of the delegate (otherwise an exception will be thrown). In that case, they
        /// should not be used, but rather the DelegateHelper methods should be used.
        /// </summary>
        COMPATIBILITY,

        /// <summary>
        /// Allows injection when using delegateExpressions but will not throw an exception
        /// when the fields are not defined on the delegate. This allows for mixed behaviours
        /// where some delegates have injection (for example because they are not singletons)
        /// and some don't.
        /// </summary>
        MIXED,

        /// <summary>
        /// (Advised mode, as it is the safest) 
        /// 
        /// Disables field injection when using delegateExpressions, no field injection will happen.
        /// </summary>
        DISABLED
    }
}