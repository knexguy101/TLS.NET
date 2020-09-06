using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fleck;
using System.Diagnostics;

namespace TLS.NET
{    
    public class GoHandler
    {
        public int Port { get; set; }
        public WebSocketServer ws { get; set; }
        public IWebSocketConnection socket { get; set; }
        private Process proc { get; set; }

        public delegate void OnMessageDelegate(object sender, string e);
        public event OnMessageDelegate OnMessage;

        public delegate void OnErrorDelegate(object sender, Exception e);
        public event OnErrorDelegate OnError;

        public delegate void OnOpenDelegate(object sender, EventArgs e);
        public event OnOpenDelegate OnOpen;

        public GoHandler(int Port = 8011)
        {
            this.Port = Port;
            Listener();
        }

        public void Listener()
        {
            //spawn go instance
            var childProcess = new ProcessStartInfo()
            {
                FileName = "index.exe",
                CreateNoWindow = false,
                UseShellExecute = false
            };
            childProcess.Environment.Add("WS_PORT", this.Port.ToString());
            proc = Process.Start(childProcess);
            Task.Factory.StartNew(() => proc.WaitForExit());

            ws = new WebSocketServer($"ws://127.0.0.1:{this.Port}");

            bool connected = false;

            ws.Start(socket =>
            {
                this.socket = socket;
                socket.OnOpen = () => this.OnOpen?.Invoke(this, new EventArgs());
                socket.OnMessage = (message) => this.OnMessage(this, message);
                socket.OnError = (error) => this.OnError(this, error);
            });

            this.OnOpen += (sender, e) =>
            {
                connected = true;
            };

            Stopwatch stopwatch = new Stopwatch();
            while (!connected && stopwatch.Elapsed < TimeSpan.FromSeconds(10)) Thread.Sleep(100);

            if (!connected)
            {
                throw new Exception("Did not connect");
            }
        }

        public void Dispose()
        {
            ws.Dispose();
            proc.Close();
        }
    }
}
