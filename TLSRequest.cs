using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.NET
{
    public class TLSRequest
    {

        public class RequestArgs
        {
            public string RequestId { get; set; }
            public Dictionary<string, string> Headers = new Dictionary<string, string>();
            public string Url { get; set; }
            public string Method { get; set; }
            public string Body { get; set; }
            public string JA3 { get; set; }
            public string Proxy { get; set; }

            public RequestArgs()
            {
                this.RequestId = Guid.NewGuid().ToString();
                this.JA3 = "771,255-49195-49199-49196-49200-49171-49172-156-157-47-53,0-10-11-13,23-24,0";
                this.Proxy = "";
                this.Body = "";
                this.Url = "";
                this.Method = "get";
            }

            public string GetRequestBody()
            {
                var headers = new JObject();
                foreach(var header in this.Headers)
                {
                    headers.Add(header.Key, header.Value);
                }
                var options = new JObject()
                {
                    {"headers", headers },
                    {"body", this.Body },
                    {"ja3", this.JA3 },
                    {"proxy", this.Proxy },
                    {"url", Url },
                    {"method", Method }
                };
                return new JObject()
                {
                    {"requestId", this.RequestId },
                    {"options", options }
                }.ToString();
            }
        }

        public class ResponseArgs
        {
            public int StatusCode { get; set; }
            public string Body { get; set; }
            public Exception ErrorMessage { get; set; }
            public bool Error { get; set; }
            public Dictionary<string, string> Headers = new Dictionary<string, string>();
        }
    }
}
