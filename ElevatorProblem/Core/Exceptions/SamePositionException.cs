using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Core.Exceptions
{
    public class SamePositionException : Exception
    {
        public SamePositionException(string message) : base(message)
        { }
    }
}
