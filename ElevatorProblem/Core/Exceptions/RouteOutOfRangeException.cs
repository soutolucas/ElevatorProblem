using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Core.Exceptions
{
    public class RouteOutOfRangeException : Exception
    {
        public RouteOutOfRangeException(string message) : base(message)
        { }
    }
}
