using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using static TLS.NET.TLSRequest;

namespace TLS.NET
{
    public class TLSHttpClient
    {
        public CookieContainer CookieJar = new CookieContainer();
        public GoHandler GoHandler { get; set; }
        public TimeSpan Timeout = new TimeSpan(0, 2, 0);

        public TLSHttpClient(GoHandler Handler)
        {
            this.GoHandler = Handler;
        }

        public TLSHttpClient(int Port)
        {
            this.GoHandler = new GoHandler(Port);
        }

        private ResponseArgs ParseRawMessage(JObject msg)
        {
            Dictionary<string, string> headers = (msg["Response"]["Headers"] as JObject).ToObject<Dictionary<string, string>>();
            return new ResponseArgs()
            {
                Headers = headers,
                StatusCode = msg["Response"]["Status"].Value<int>(),
                Body = msg["Response"]["Body"].Value<string>()
            };
        }

        public ResponseArgs Send(RequestArgs Args)
        {
            JObject obj = null;
            Exception ex = null;
            GoHandler.OnMessage += (sender, e) =>
            {
                var tempobj = JObject.Parse(e);
                if(tempobj["RequestID"].Value<string>() == Args.RequestId)
                {
                    obj = tempobj;
                }
            };
            GoHandler.OnError += (sender, e) =>
            {
                ex = e;
            };

            GoHandler.socket.Send(Args.GetRequestBody());

            Stopwatch T = new Stopwatch();
            T.Start();
            while (T.Elapsed < this.Timeout && obj == null && ex == null) Thread.Sleep(100);

            if(obj == null)
            {
                return new ResponseArgs()
                {
                    Error = true,
                    ErrorMessage = ex
                };
            }

            return ParseRawMessage(obj);
        }
    }
}
