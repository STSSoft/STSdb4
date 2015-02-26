using STSdb4.General.Communication;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STSdb4.Server
{
    public class UsersAndExceptionHandler
    {
        private Thread Worker;

        private bool Disconnecting = false;
        private volatile bool HasNewExceptions = false;
        private bool ShutDown;
        private volatile bool Refreshed = false;

        private readonly ConcurrentDictionary<ServerConnection, string> clientsList = new ConcurrentDictionary<ServerConnection, string>();
        private readonly List<KeyValuePair<string, string>> ExceptionsList = new List<KeyValuePair<string, string>>(100);
        private readonly ConcurrentBag<string> ClientsForDisconnects = new ConcurrentBag<string>();

        public bool IsFinishRefresh { get { return Refreshed; } }
        public bool IsWorking { get { return Worker != null; } }
        public bool IsDisconnecting { get { return Disconnecting; } }

        public UsersAndExceptionHandler()
        {
        }

        public void Start()
        {
            Stop();
            ShutDown = false;

            Worker = new Thread(RefreshList);
            Worker.Start();
        }

        public void Stop()
        {
            if (!IsWorking)
                return;

            ShutDown = true;

            Thread thread = Worker;
            if (thread != null)
            {
                if (thread.Join(5000))
                    thread.Abort();
            }

            Worker = null;
        }

        public void Disconnect(ListView.SelectedListViewItemCollection clientsForDisconnect)
        {
            if (IsWorking)
            {
                foreach (var client in clientsForDisconnect)
                    ClientsForDisconnects.Add(((ListViewItem)client).Text);

                Disconnecting = true;
            }
        }

        private void DisconnectClient(string client)
        {
            lock (clientsList)
            {
                foreach (var item in clientsList)
                {
                    if (item.Value.Equals(client))
                        item.Key.Disconnect();
                }
            }
        }

        private void RefreshList()
        {
            while (!ShutDown)
            {
                lock (clientsList)
                {
                    clientsList.Clear();
                }

                int id = 0;
                foreach (var connection in Program.StorageEngineServer.TcpServer.ServerConnections)
                {
                    if (connection.Key.TcpClient.Connected)
                    {
                        string clientIp = IPAddress.Parse(((IPEndPoint)connection.Key.TcpClient.Client.RemoteEndPoint).Address.ToString()).ToString();
                        clientsList.TryAdd(connection.Key, (clientIp + " ID:" + id.ToString()));
                        id++;
                    }
                }

                Refreshed = true;

                KeyValuePair<DateTime, Exception> error;
                if (Program.StorageEngineServer.TcpServer.Errors.TryDequeue(out error))
                {
                    lock (ExceptionsList)
                    {
                        ExceptionsList.Insert(0, new KeyValuePair<string, string>(error.Key.ToString(), error.Value.Message));
                        HasNewExceptions = true;
                    }
                }

                if (IsDisconnecting)
                {
                    foreach (var client in ClientsForDisconnects)
                        DisconnectClient(client);
                    Disconnecting = false;
                }

                Thread.Sleep(300);
            }

            Worker = null;
        }

        public IEnumerable<string> GetClients()
        {
            Refreshed = false;
            foreach (var client in clientsList)
                yield return client.Value;
        }

        public IEnumerable<KeyValuePair<string, string>> GetExceptions()
        {
            if (HasNewExceptions)
            {
                lock (ExceptionsList)
                {
                    foreach (var excetion in ExceptionsList)
                        yield return excetion;

                    ExceptionsList.Clear();
                    HasNewExceptions = false;
                }
            }
        }
    }
}
