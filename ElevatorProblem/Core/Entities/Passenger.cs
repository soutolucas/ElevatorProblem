using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Core.Entities
{
    public class Passenger
    {
        public string Name { get; private set; }

        public Passenger(string name)
        {
            Name = name;
        }
    }
}
