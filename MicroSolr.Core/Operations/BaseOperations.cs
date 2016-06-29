// -----------------------------------------------------------------------
// <copyright file="BaseOperations.cs" company="Imran Saeed">
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
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MicroSolr.Core.Web;

namespace MicroSolr.Core.Operations
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public abstract class BaseOperations : IOperations
    {
        protected IHttpHelper _httpHelper;

        public BaseOperations(Uri baseUri, string coreName, IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper ?? new StatelessHttpHelper();
            CoreUri = new Uri(string.Format("{0}/{1}/", baseUri.ToString().TrimEnd('/'), coreName));
            SelectUri = new Uri(CoreUri + "select/");
            UpdateUri = new Uri(CoreUri + "update/");
        }

        protected Uri CoreUri { get; private set; }

        protected Uri SelectUri { get; private set; }

        protected Uri UpdateUri { get; private set; }

        public abstract IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer,
            IResponseFormatter<string> formatter);

        public abstract IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer,
            IResponseFormatter<string> formatter, out long start, out long numFound);

        public abstract IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer,
            bool commit = true, bool optimize = false);

        public abstract IOperations Delete(string query, bool commit = true);

        public virtual IOperations Commit()
        {
            var u = MakeUri(UpdateUri, "commit=true");
            _httpHelper.Get(u);
            return this;
        }

        public virtual IOperations Optimize()
        {
            var u = MakeUri(UpdateUri, "optimize=true");
            _httpHelper.Get(u);
            return this;
        }

        protected long GetRowCountForResults(ILoadCommand command)
        {
            var qs = MakeRowCountQueryString(command);
            var rowCountUri = MakeUri(SelectUri, qs);
            var results = _httpHelper.Get(rowCountUri);
            var resultsDoc = XDocument.Parse(results);
            var rootNode =
                (from r in resultsDoc.Descendants() where r.Attribute("name").Name == "response" select r).First();
            long rows = 0;
            var val = rootNode.Attribute("numFound").Value;
            long.TryParse(val, out rows);

            return rows;
        }

        protected IEnumerable<TOutput> ExecuteLoad<TOutput>(string loadQS, FormatType responseFormat,
            IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter)
        {
            long start = 0;
            long numFound = 0;
            var loadUri = MakeUri(SelectUri, loadQS);
            var response = _httpHelper.Get(loadUri);
            var formattedResponse = formatter != null ? formatter.Format(response) : response;
            return serializer.DeSerialize(formattedResponse, responseFormat, out start, out numFound);
        }

        protected IEnumerable<TOutput> ExecuteLoad<TOutput>(string loadQS, FormatType responseFormat,
            IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter, out long start, out long numFound)
        {
            var loadUri = MakeUri(SelectUri, loadQS);
            var response = _httpHelper.Get(loadUri);
            var formattedResponse = formatter != null ? formatter.Format(response) : response;
            return serializer.DeSerialize(formattedResponse, responseFormat, out start, out numFound);
        }

        protected IOperations ExecuteSave<TData>(IEnumerable<TData> data, IDataSerializer<TData> serializer, bool commit,
            bool optimize)
        {
            var saveData = serializer.Serialize(data, FormatType.JSON);
            _httpHelper.Post(UpdateUri, saveData, "application/json", Encoding.UTF8);
            if (commit) Commit();
            if (optimize) Optimize();
            return this;
        }

        protected IOperations ExecuteDelete(string query, bool commit)
        {
            query = "<delete><query>" + query + "</query></delete>";
            _httpHelper.Post(UpdateUri, query, "application/xml", Encoding.UTF8);
            if (commit) Commit();
            return this;
        }

        protected static Uri MakeUri(Uri baseUri, string queryString)
        {
            var builder = new UriBuilder(baseUri);
            builder.Query = queryString;
            return builder.Uri;
        }

        protected static string MakeLoadQueryString(ILoadCommand command)
        {
            IDictionary<string, string> qsParts = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(command.Query))
            {
                qsParts.Add("q", command.Query);
            }

            qsParts.Add("rows", command.MaxRows.ToString());
            qsParts.Add("start", command.StartIndex.ToString());

            qsParts.Add("wt", Enum.GetName(typeof (FormatType), command.ResponseFormat).ToLowerInvariant());
            return QueryStringFromDicionary(qsParts);
        }

        protected static string MakeRowCountQueryString(ILoadCommand command)
        {
            IDictionary<string, string> qsParts = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(command.Query))
            {
                qsParts.Add("q", command.Query);
            }
            qsParts.Add("rows", "0");
            qsParts.Add("wt", "XML");

            return QueryStringFromDicionary(qsParts);
        }

        private static string QueryStringFromDicionary(IDictionary<string, string> qsParts)
        {
            var qList = from k in qsParts.Keys select string.Format("{0}={1}", k, qsParts[k]);

            return string.Join("&", qList);
        }
    }
}