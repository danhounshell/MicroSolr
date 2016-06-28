﻿// -----------------------------------------------------------------------
// <copyright file="JsonSerializer.cs" company="Imran Saeed">
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
using Newtonsoft.Json;

namespace MicroSolr.Core.Serializers
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class MultiFormatSerializer<TData> : IDataSerializer<TData>
    {
        public string Serialize(TData data, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    return JsonConvert.SerializeObject(data);
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
        }

        public string Serialize(IEnumerable<TData> data, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    return JsonConvert.SerializeObject(data);
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
        }

        public IEnumerable<TData> DeSerialize(string stream, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    var response = JsonConvert.DeserializeObject<JsonResponse>(stream);
                    return response.Response.Docs;
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
        }

        #region JSonresponsecontainer

        private class Response
        {
            [JsonProperty("numFound")]
            public long NumFound { get; set; }

            [JsonProperty("start")]
            public long Start { get; set; }

            [JsonProperty("docs")]
            public TData[] Docs { get; set; }
        }

        private class JsonResponse
        {
            [JsonProperty("response")]
            public Response Response { get; set; }
        }

        #endregion
    }
}