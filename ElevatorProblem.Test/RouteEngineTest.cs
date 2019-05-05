using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorProblem.Core;
using ElevatorProblem.Core.Interfaces;
using System.Linq;
using ElevatorProblem.Test.Helpers;
using ElevatorProblem.Core.Entities;

namespace ElevatorProblem.Test
{
    [TestClass]
    public class RouteEngineTest
    {
        private RouteEngine _routeEngine;

        [TestInitialize]
        public void Setup()
        {
            _routeEngine = RouteEngine.GetInstance();

        }

        [TestMethod]
        public void FindBetterRoute_10FloorsOneRequestToUpAndOneRequestToDown_Return8To1To10()
        {
            //arrange
            ITransport elevator = new Elevator(currentPositon: Floor.GetPositonFromFloor(5),
                                               minPosition: Floor.GetPositonFromFloor(1),
                                               maxPosition: Floor.GetPositonFromFloor(10));

            Route[] routes =  { new Route(startPosition: Floor.GetPositonFromFloor(8), endPosition: Floor.GetPositonFromFloor(1)),
                                new Route(startPosition: Floor.GetPositonFromFloor(1), endPosition: Floor.GetPositonFromFloor(10))
                              };

            //act
            var result = _routeEngine.FindBetterRoute(elevator, routes);

            //assert
            var expectedResult = new int[] { Floor.GetPositonFromFloor(8), Floor.GetPositonFromFloor(1), Floor.GetPositonFromFloor(10) };
            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }

        [TestMethod]
        public void FindBetterRoute_20FloorsTwoRequestToUpAndOneRequestToDown_Return8To1To3To15()
        {
            //arrange
            ITransport elevator = new Elevator(currentPositon: Floor.GetPositonFromFloor(3),
                                               minPosition: Floor.GetPositonFromFloor(1),
                                               maxPosition: Floor.GetPositonFromFloor(20));

            Route[] routes =  { new Route(startPosition: Floor.GetPositonFromFloor(8), endPosition: Floor.GetPositonFromFloor(1)),
                                new Route(startPosition: Floor.GetPositonFromFloor(1), endPosition: Floor.GetPositonFromFloor(7)),
                                new Route(startPosition: Floor.GetPositonFromFloor(3), endPosition: Floor.GetPositonFromFloor(15))
                              };

            //act
            var result = _routeEngine.FindBetterRoute(elevator, routes);

            //assert
            var expectedResult = new int[] { Floor.GetPositonFromFloor(8), Floor.GetPositonFromFloor(1), Floor.GetPositonFromFloor(3), Floor.GetPositonFromFloor(15) };
            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }
    }
}
