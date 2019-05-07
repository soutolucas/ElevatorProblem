using ElevatorProblem.Core;
using ElevatorProblem.Core.Entities;
using ElevatorProblem.Core.Exceptions;
using ElevatorProblem.Core.Interfaces;
using ElevatorProblem.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorProblem.Test
{
    [TestClass]
    public class ElevatorTest
    {
        #region Scenarios Question 3
        [TestMethod]
        public async Task Request_10FloorsMultiplesRequestMoveBothDirections_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);
            var elevatorEndPosition = 10;

            //act
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            elevator.RequestAsync(8, 1);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await elevator.RequestAsync(1, elevatorEndPosition);

            //assert
            Assert.AreEqual(elevatorEndPosition, elevator.CurrentPosition);
        }

        [TestMethod]
        public async Task Request_20FloorsMultiplesRequestMoveBothDirections_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 3, minFloor: 1, maxFloor: 20);
            var elevatorEndPosition = 15;

            //act
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            elevator.RequestAsync(8, 1);
            elevator.RequestAsync(1, 7);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await elevator.RequestAsync(3, elevatorEndPosition);

            //assert
            Assert.AreEqual(elevatorEndPosition, elevator.CurrentPosition);
        }
        #endregion

        #region Others Scenarios
        [TestMethod]
        [ExpectedException(typeof(SamePositionException))]
        public async Task Request_TryMoveToSamePosition_SamePositionException()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);

            //act
            await elevator.RequestAsync(1, 1);
        }

        [TestMethod]
        public async Task Request_10FloorsSingleRequestMoveToUp_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);
            var elevatorNewPosition = 10;

            //act
            await elevator.RequestAsync(1, elevatorNewPosition);

            //assert
            Assert.AreEqual(elevatorNewPosition, elevator.CurrentPosition);
        }

        [TestMethod]
        public async Task Request_10FloorsSingleRequestMoveToDown_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);
            var elevatorNewPosition = 1;

            //act
            await elevator.RequestAsync(10, elevatorNewPosition);

            //assert
            Assert.AreEqual(elevatorNewPosition, elevator.CurrentPosition);
        }

        [TestMethod]
        public async Task Request_10FloorsMultiplesRequestMoveUp_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);
            var elevatorEndPosition = 8;

            //act
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            elevator.RequestAsync(1, 4);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await elevator.RequestAsync(7, elevatorEndPosition);

            //assert
            Assert.AreEqual(elevatorEndPosition, elevator.CurrentPosition);
        }

        [TestMethod]
        public async Task Request_10FloorsMultiplesRequestMoveDown_CurrentShouldBeTheEndSetPosition()
        {
            //arrange
            IVehicle elevator = ElevatorBuild.Build(currentFloor: 5, minFloor: 1, maxFloor: 10);
            var elevatorEndPosition = 2;

            //act
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            elevator.RequestAsync(7, 4);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await elevator.RequestAsync(6, elevatorEndPosition);

            //assert
            Assert.AreEqual(elevatorEndPosition, elevator.CurrentPosition);
        }
        #endregion
    }
}
