﻿using Athena.Models.Athena.Commands;

namespace Athena.Profiles.SmbSW.Models
{
    public interface ITcpMessenger
    {
        //Guid GetId(string source, double weight, int quantity);
        public event EventHandler<MessageReceivedArgs> MessageReceived;
        Task<string> GetMessage();
        bool ForwardMessage(string msg);
        //List<string> GetItems(Guid id, int[] vals);
    }

    //[Serializable]
    //public struct TestResponse
    //{
    //    public Guid Id { get; set; }
    //    public string Label { get; set; }
    //    public long Quantity { get; set; }
    //}
}
