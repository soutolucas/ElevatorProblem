using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorProblem.Core.Interfaces;

namespace ElevatorProblem.Core.Entities
{
    public class Elevator : ITransport
    {
        #region Properties
        public int CurrentPosition { get; private set; }
        public int MinPosition { get; private set; }
        public int MaxPosition { get; private set; }
        #endregion

        #region Attributes
        private List<Route> _routesToProcess = new List<Route>();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Task threadPool;

        private RouteEngine _routeEngine = RouteEngine.GetInstance();
        #endregion


        public Elevator(int currentPositon, int minPosition, int maxPosition)
        {
            CurrentPosition = currentPositon;
            MaxPosition = maxPosition;
            MinPosition = minPosition;
        }

        public async Task RequestAsync(int startPosition, int endPosition)
        {
            try
            {
                var route = new Route(startPosition, endPosition);

                await _semaphore.WaitAsync();
                _routesToProcess.Add(route);

                WaitingByOthersCloseRequest();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #region Private Methods

        private void WaitingByOthersCloseRequest()
        {
            _routeEngine.FindBetterRoute(this, _routesToProcess);
        }
        #endregion

        private void MoveUp()
        {
            
        }

        private void MoveDown()
        {

        }
    }
}
