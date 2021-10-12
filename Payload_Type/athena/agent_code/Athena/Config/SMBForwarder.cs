﻿using Athena.Models.Mythic.Response;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;


//This is for when an operator does not want to include an SMBForwarder into their agent.
namespace Athena.Config
{
    public class SMBForwarder
    {
        public bool connected { get; set; }
        public List<DelegateMessage> messageOut { get; set; }
        public ConcurrentQueue<string> queueIn { get; set; }

        public SMBForwarder()
        {
            this.connected = false;
            this.messageOut = new List<DelegateMessage>();
            this.queueIn = new ConcurrentQueue<string>();
        }

        //Link to the Athena SMB Agent
        public async Task<bool> Link(string host, string pipename)
        {
            return false;
        }
        public bool ForwardDelegateMessage(DelegateMessage dm)
        {
            return false;
        }
        public List<DelegateMessage> GetMessages()
        {
            return new List<DelegateMessage>();
        }

        //Unlink from the named pipe
        public void Unlink()
        {

        }
    }
}
