using System;
using MediatR;

namespace BaseProject.Core.Messages
{
    public class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
