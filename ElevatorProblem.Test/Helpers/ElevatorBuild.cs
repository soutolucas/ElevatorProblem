using ElevatorProblem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Test.Helpers
{
    public static class ElevatorBuild
    {
        public static Elevator Build(int currentFloor, int minFloor, int maxFloor)
        {
            return new Elevator(currentPosition: currentFloor,
                                minPosition: minFloor,
                                maxPosition: maxFloor);
        }
    }
}
