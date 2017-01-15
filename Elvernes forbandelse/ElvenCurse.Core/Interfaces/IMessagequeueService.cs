using System.Collections.Generic;
using ElvenCurse.Core.Engines.Messagequeue;

namespace ElvenCurse.Core.Interfaces
{
    public interface IMessagequeueService
    {
        List<Queueelement> GetMessagequeue();
        void Push(Queueelement element);
        void SetMessageAsDone(Queueelement msg, string errorMessage);
    }
}
