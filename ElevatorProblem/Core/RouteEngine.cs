using ElevatorProblem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ElevatorProblem.Core
{
    public class RouteEngine
    {
        private readonly static RouteEngine _instance = new RouteEngine();

        public static RouteEngine GetInstance() => _instance;

        static RouteEngine()
        { }

        private RouteEngine()
        { }


        public class OrderedRoutes
        {
            public int Distance { get; set; }
            public Route Route { get; set; }
        }

        public int[] FindBetterRoute(ITransport transport, List<Route> routes)
        {
            var goingUp = routes.Where(r => r.Direction == Enums.Direction.Up);
            var goingDown = routes.Where(r => r.Direction == Enums.Direction.Down);

            var goingUpOrdered = CalculateDistance(transport, routes, goingUp);
            var goingDownOrdered = CalculateDistance(transport, routes, goingDown);

            goingUpOrdered.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            goingUpOrdered.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            //var result = new int[orders.Count + 1];
            //int index = 0, resultIndex = 0;
            //while (true)
            //{
            //    if (index >= orders.Count - 1)
            //    {
            //        result[resultIndex] = orders[index].Route.EndPositon;
            //        break;
            //    }
            //    else
            //    {
            //        result[resultIndex] = orders[index].Route.StartPositon;
            //        result[++resultIndex] = orders[index].Route.EndPositon;
            //    }

            //    index++;
            //    resultIndex++;
            //}

            return new int[] { };
        }

        #region Private Methods
        private List<OrderedRoutes> CalculateDistance(ITransport transport, Route[] routes, IEnumerable<Route> goingUp)
        {
            var orderedRoutes = new List<OrderedRoutes>();

            foreach (var route in routes)
            {
                var distance = Math.Abs(transport.CurrentPosition - route.StartPositon) + Math.Abs(route.StartPositon - route.EndPositon);
                var order = new OrderedRoutes() { Distance = distance, Route = route };

                orderedRoutes.Add(order);
            }

            return orderedRoutes;
        }
        #endregion
    }
}
