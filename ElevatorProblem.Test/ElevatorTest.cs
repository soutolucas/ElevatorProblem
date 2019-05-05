using ElevatorProblem.Core;
using ElevatorProblem.Core.Entities;
using ElevatorProblem.Core.Exceptions;
using ElevatorProblem.Core.Interfaces;
using ElevatorProblem.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProblem.Test
{
    [TestClass]
    public class ElevatorTest
    {
        private ITransport _elevator;

        [TestInitialize]
        public void Setup()
        {

        }

        [ExpectedException(typeof(SamePositionException))]
        [TestMethod]
        public void Request_TryMoveToSamePosition_SamePositionException()
        {
            //arrange
            ITransport elevator = new Elevator(currentPositon: Floor.GetPositonFromFloor(5),
                                               minPosition: Floor.GetPositonFromFloor(1),
                                               maxPosition: Floor.GetPositonFromFloor(10));
            //act
            elevator.Request(Floor.GetPositonFromFloor(1), Floor.GetPositonFromFloor(1));
        }

        [TestMethod]
        public void Request_MoveToUp_CurrentShouldBeTheSetPosition()
        {
            //arrange
            ITransport elevator = new Elevator(currentPositon: Floor.GetPositonFromFloor(5),
                                               minPosition: Floor.GetPositonFromFloor(1),
                                               maxPosition: Floor.GetPositonFromFloor(10));
            var elevatorNewPosition = Floor.GetPositonFromFloor(10);
            //act
            elevator.Request(Floor.GetPositonFromFloor(1), Floor.GetPositonFromFloor(10));

            //assert
            Assert.AreEqual(elevatorNewPosition, elevator.CurrentPosition);
        }
    }
}
