using ElevatorProblem.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Core
{
    public class Route
    {
        public int StartPositon { get; private set; }
        public int EndPositon { get; private set; }

        public Enums.Direction Direction { get; private set; } = Enums.Direction.None;

        public Route(int startPosition, int endPosition)
        {
            StartPositon = startPosition;
            EndPositon = endPosition;

            SetDirection();
        }

        private void SetDirection()
        {
            if (StartPositon < EndPositon)
                Direction = Enums.Direction.Up;
            else if (StartPositon > EndPositon)
                Direction = Enums.Direction.Down;
            else
                throw new SamePositionException("Start position is equal to the End position");
        }
    }
}
