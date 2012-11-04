﻿// -----------------------------------------------------------------------
// <copyright file="IShardingStrategy.cs" company="Imran Saeed">
// Copyright (c) 2012 Imran Saeed
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core
{
    /// <summary>
    /// Sharding strategy.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IShardingStrategy<in TData>
    {
        /// <summary>
        /// Gets the target core.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ICore GetTargetCore(TData data);
    }
}
