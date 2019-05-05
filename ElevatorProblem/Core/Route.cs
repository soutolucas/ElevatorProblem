using ElevatorProblem.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Core
{
    public class Route
    {
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }

        public Enums.Direction Direction { get; private set; }

        public Route(int startPosition, int endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;

            SetDirection();
        }

        private void SetDirection()
        {
            if (StartPosition < EndPosition)
                Direction = Enums.Direction.Up;
            else if (StartPosition > EndPosition)
                Direction = Enums.Direction.Down;
            else
                throw new SamePositionException("Start position is equal to the End position");
        }
    }
}
