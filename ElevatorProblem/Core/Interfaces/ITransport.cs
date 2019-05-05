using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorProblem.Core.Interfaces
{
    public interface ITransport
    {
        int CurrentPosition { get;  }
        int MinPosition { get; }
        int MaxPosition { get; }

        Task RequestAsync(int startPosition, int endPosition);
    }
}
