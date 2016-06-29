// -----------------------------------------------------------------------
// <copyright file="SimpleOperations.cs" company="Imran Saeed">
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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MicroSolr.Core.Operations
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SimpleOperations : BaseOperations
    {
        public SimpleOperations(Uri baseUri, string coreName, IHttpHelper httpHelper = null)
            : base(baseUri, coreName, httpHelper)
        {
        }

        public override IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer,
            IResponseFormatter<string> formatter)
        {
            if (command.GetAll)
            {
                command.MaxRows = GetRowCountForResults(command);
            }

            if (command.MaxRows > 100000)
            {
                Debug.WriteLine("Too many rows. Try using concurrent library.");
            }

            var loadQS = MakeLoadQueryString(command);

            return ExecuteLoad(loadQS, command.ResponseFormat, serializer, formatter);
        }

        public override IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer,
            IResponseFormatter<string> formatter, out long start, out long numFound)
        {
            if (command.GetAll)
            {
                command.MaxRows = GetRowCountForResults(command);
            }

            if (command.MaxRows > 100000)
            {
                Debug.WriteLine("Too many rows. Try using concurrent library.");
            }

            var loadQS = MakeLoadQueryString(command);

            return ExecuteLoad(loadQS, command.ResponseFormat, serializer, formatter, out start, out numFound);
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer,
            bool commit = true, bool optimize = false)
        {
            return ExecuteSave(command.Data, serializer, commit, optimize);
        }

        public override IOperations Delete(string query, bool commit = true)
        {
            return ExecuteDelete(query, commit);
        }
    }
}