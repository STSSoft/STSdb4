﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace STSdb4.General.Communication
{
    public class ClientConnection
    {
        private long ID = 0;

        public TcpClient TcpClient { get; private set; }

        public BlockingCollection<Packet> PendingPackets;
        public ConcurrentDictionary<long, Packet> SentPackets;

        private CancellationTokenSource ShutdownTokenSource;

        private Thread SendWorker;
        private Thread RecieveWorker;

        public readonly string MachineName;
        public readonly int Port;

        public ClientConnection(string machineName = "localhost", int port = 7182)
        {
            MachineName = machineName;
            Port = port;
        }

        public void Send(Packet packet)
        {
            if (!IsWorking)
                throw new Exception("Client connection is not started.");

            packet.ID = Interlocked.Increment(ref ID);
            PendingPackets.Add(packet, ShutdownTokenSource.Token);
        }

        public void Start(int boundedCapacity = 64, int recieveTimeout = 0, int sendTimeout = 0)
        {
            if (IsWorking)
                throw new Exception("Client connection is already started.");

            PendingPackets = new BlockingCollection<Packet>(boundedCapacity);
            SentPackets = new ConcurrentDictionary<long, Packet>();
            ShutdownTokenSource = new CancellationTokenSource();

            TcpClient = new TcpClient();
            TcpClient.ReceiveTimeout = recieveTimeout;
            TcpClient.SendTimeout = sendTimeout;
            TcpClient.Connect(MachineName, Port);
            NetworkStream networkStream = TcpClient.GetStream();

            SendWorker = new Thread(new ParameterizedThreadStart(DoSend));
            RecieveWorker = new Thread(new ParameterizedThreadStart(DoRecieve));

            SendWorker.Start(networkStream);
            RecieveWorker.Start(networkStream);
        }

        public void Stop()
        {
            if (!IsWorking)
                return;

            ShutdownTokenSource.Cancel(false);

            Thread thread = RecieveWorker;
            if (thread != null)
            {
                if (thread.Join(2000))
                    thread.Abort();
            }

            thread = SendWorker;
            if (thread != null)
            {
                if (thread.Join(2000))
                    thread.Abort();
            }

            PendingPackets = null;
            SetException(new Exception("Client stopped"));
            ShutdownTokenSource = null;
        }

        public bool IsWorking
        {
            get { return SendWorker != null || RecieveWorker != null; }
        }

        private void DoSend(object state)
        {
            BinaryWriter writer = new BinaryWriter((NetworkStream)state);

            try
            {
                while (!Shutdown.IsCancellationRequested)
                {
                    Packet packet = PendingPackets.Take(Shutdown);

                    SentPackets.TryAdd(packet.ID, packet);
                    packet.Write(writer, packet.Request);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                SetException(e);
            }
            finally
            {
                SendWorker = null;
            }
        }

        private void DoRecieve(object state)
        {
            BinaryReader reader = new BinaryReader((NetworkStream)state);

            try
            {
                while (!Shutdown.IsCancellationRequested)
                {
                    long id = reader.ReadInt64();
                    int size = reader.ReadInt32();
                    MemoryStream response = new MemoryStream(reader.ReadBytes(size));

                    Packet packet = null;
                    if (SentPackets.TryRemove(id, out packet))
                    {
                        packet.Response = response;
                        packet.ResultEvent.Set();
                    }
                }
            }
            catch (Exception e)
            {
                SetException(e);
            }
            finally
            {
                RecieveWorker = null;
            }
        }

        private void SetException(Exception exception)
        {
            lock (SentPackets)
            {
                foreach (var packet in SentPackets.Values)
                {
                    packet.Exception = exception;
                    packet.ResultEvent.Set();
                }

                SentPackets.Clear();
            }
        }

        private CancellationToken Shutdown
        {
            get { return ShutdownTokenSource.Token; }
        }

        public int BoundedCapacity
        {
            get
            {
                if (!IsWorking)
                    throw new Exception("Client connection is not started.");

                return PendingPackets.BoundedCapacity;
            }
        }
    }
}