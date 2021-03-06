﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorProblem.Core.Exceptions;
using ElevatorProblem.Core.Interfaces;

namespace ElevatorProblem.Core.Entities
{
    public class Elevator : IVehicle
    {
        #region Constants
        private const int TIMEOUT_FOR_NEW_REQUESTS = 500;
        #endregion

        #region Events
        public event EventHandler CurrentPositionChangedEvent;
        public event EventHandler StopPositionChangedEvent;
        #endregion

        #region Properties
        private int _currentPosition;
        public int CurrentPosition
        {
            get => _currentPosition;
            private set
            {
                _currentPosition = value;
                CheckIfNeedStop?.Invoke();
                CurrentPositionChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        private int _stopPosition;
        public int StopPosition
        {
            get => _stopPosition;
            private set
            {
                _stopPosition = value;
                StopPositionChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public int MinPosition { get; private set; }
        public int MaxPosition { get; private set; }
        #endregion

        #region Attributes
        private List<Route> _routesToProcess = new List<Route>();
        private List<Route> _runningRoutes = new List<Route>();
        
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private SemaphoreSlim _semaphoreRoutes = new SemaphoreSlim(1, 1);

        private delegate void CheckIfNeedStopDelegate();
        private CheckIfNeedStopDelegate CheckIfNeedStop;
        #endregion

        #region Constructor
        public Elevator(int currentPosition, int minPosition, int maxPosition)
        {
            CurrentPosition = currentPosition;
            MaxPosition = maxPosition;
            MinPosition = minPosition;
        }
        #endregion

        #region Public Methods
        public async Task RequestAsync(int startPosition, int endPosition)
        {
            if (startPosition < MinPosition || endPosition > MaxPosition)
                throw new RouteOutOfRangeException($"The {nameof(startPosition)} is less than {nameof(MinPosition)} or {nameof(endPosition)} is bigger than {nameof(MaxPosition)}");

            var route = new Route(startPosition, endPosition);
            await AddRoute(route);

            await Task.Delay(TIMEOUT_FOR_NEW_REQUESTS);

            try
            {
                await _semaphore.WaitAsync();
                await StartMoving();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        #endregion

        #region Private Methods
        private async Task AddRoute(Route route)
        {
            try
            {
                await _semaphoreRoutes.WaitAsync();
                _routesToProcess.Add(route);

            }
            finally
            {
                _semaphoreRoutes.Release();
            }
        }

        private async Task StartMoving()
        {
            await Task.Run(() =>
            {
                if (_routesToProcess.Any())
                {
                    MoveRoutesToRunning();

                    var routesToUp = _runningRoutes.Where(r => r.Direction == Enums.Direction.Up);
                    var routesToDown = _runningRoutes.Where(r => r.Direction == Enums.Direction.Down);

                    bool isToUpOnly = routesToUp.Any() && !routesToDown.Any();
                    bool isToDownOnly = !routesToUp.Any() && routesToDown.Any();

                    if (isToUpOnly)
                        MoveToUpOnlyBehavior(routesToUp);
                    else if (isToDownOnly)
                        MoveToDownOnlyBehavior(routesToDown);
                    else
                        MoveToBothDirectionsBehavior(routesToUp, routesToDown);
                }
            });
        }

        private void MoveRoutesToRunning()
        {
            try
            {
                _semaphoreRoutes.WaitAsync();
                _runningRoutes = _routesToProcess.ToList();
                _routesToProcess.Clear();
            }
            finally
            {
                _semaphoreRoutes.Release();
            }
        }

        private void MoveToBothDirectionsBehavior(IEnumerable<Route> goingUp, IEnumerable<Route> goingDown)
        {
            int totalDistanceUp = CalculateTotalDistance(goingUp);
            int totalDistanceDown = CalculateTotalDistance(goingDown);

            if (totalDistanceUp >= totalDistanceDown)
            {
                MoveToDownOnlyBehavior(goingDown);
                MoveToUpOnlyBehavior(goingUp);
            }
            else
            {
                MoveToUpOnlyBehavior(goingUp);
                MoveToDownOnlyBehavior(goingDown);
            }
        }

        private void MoveToUpOnlyBehavior(IEnumerable<Route> routeToMove)
        {
            int minStartPosition = routeToMove.Min(r => r.StartPosition);
            int maxEndPosition = routeToMove.Max(r => r.EndPosition);

            CheckIfNeedStop = CheckIfNeedStopMoveToUpOnly;

            MoveDown(minStartPosition);
            MoveUp(maxEndPosition);
        }

        private void MoveToDownOnlyBehavior(IEnumerable<Route> routeToMove)
        {
            int maxStartPosition = routeToMove.Max(r => r.StartPosition);
            int minEndPosition = routeToMove.Min(r => r.EndPosition);

            CheckIfNeedStop = CheckIfNeedStopMoveToDownOnly;

            MoveUp(maxStartPosition);
            MoveDown(minEndPosition);
        }

        private void MoveUp(int position)
        {
            while (CurrentPosition < position)
                CurrentPosition++;
        }

        private void MoveDown(int position)
        {
            while (CurrentPosition > position)
                CurrentPosition--;
        }

        private void CheckIfNeedStopMoveToUpOnly()
        {
            var isStop = IsStop(Enums.Direction.Up);

            if (isStop)
                StopPosition = CurrentPosition;
        }

        private void CheckIfNeedStopMoveToDownOnly()
        {
            var isStop = IsStop(Enums.Direction.Down);

            if (isStop)
                StopPosition = CurrentPosition;
        }
        private bool IsStop(Enums.Direction direction)
        {
            return _runningRoutes.Any(r => (r.StartPosition == CurrentPosition || r.EndPosition == CurrentPosition) && r.Direction == direction);
        }

        private int CalculateTotalDistance(IEnumerable<Route> goingUp)
        {
            int totalDistance = 0;
            foreach (var route in goingUp)
                 totalDistance += Math.Abs(CurrentPosition - route.StartPosition) + Math.Abs(route.StartPosition - route.EndPosition);
            
            return totalDistance;
        }
        #endregion
    }
}