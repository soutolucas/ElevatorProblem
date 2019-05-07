using ElevatorProblem.Core.Entities;
using ElevatorProblem.Core.Exceptions;
using System;
using static System.Console;

namespace ElevatorProblem
{
    class Program
    {
        private static Elevator _elevator;

        static void Main(string[] args)
        {
            //Uncomment the lines below to view the result of the scenarios.
            //TenFloorsElevatorOn5FloorRequestsFrom8To1From1To10Floor();
            //TwentyFloorsElevatorOn3FloorRequestsFrom8To1From1To7From3To15Floor();

            Setup();

            WriteLine("\n========================= Elevador configurado. Agora podemos iniciar a viagem! =========================");
            WriteLine("\nQual seu nome?");
            var nome = ReadLine();
            var passenger = new Passenger(nome);

            WriteLine("\nDigite seu andar atual:");
            if (!int.TryParse(ReadLine(), out int currentFloor))
                WriteLine("Andar atual inválido!");

            WriteLine("\nDigite para qual andar deseja ir:");
            if (!int.TryParse(ReadLine(), out int goToFloor))
                WriteLine("Andar de destino inválido!");

            bool hasError = true;
            passenger.DoAction(async () =>
            {
                try
                {
                    await _elevator.RequestAsync(currentFloor, goToFloor);
                    hasError = false;
                }
                catch (SamePositionException)
                {
                    WriteLine("\nO andar de partida precisa ser diferente do andar de destino!");
                }
                catch (RouteOutOfRangeException)
                {
                    WriteLine("\nO andar de partida é menor que o andar mínimo ou o destino é maior que o andar máximo!");
                }
            });

            if (!hasError)
                WriteLine("\nPor favor, espere pelo resultado.");

            ReadKey();
        }

        private static void TenFloorsElevatorOn5FloorRequestsFrom8To1From1To10Floor()
        {
            _elevator = new Elevator(currentPosition: 5, minPosition: 1, maxPosition: 10);
            _elevator.StopPositionChangedEvent += StopPositionChangedHandler;
#pragma warning disable CS4014
            _elevator.RequestAsync(8, 1);
#pragma warning restore CS4014
            _elevator.RequestAsync(1, 10).Wait();
        }

        private static void TwentyFloorsElevatorOn3FloorRequestsFrom8To1From1To7From3To15Floor()
        {
            _elevator = new Elevator(currentPosition: 3, minPosition: 1, maxPosition: 20);
            _elevator.StopPositionChangedEvent += StopPositionChangedHandler;
#pragma warning disable CS4014
            _elevator.RequestAsync(8, 1);
            _elevator.RequestAsync(1, 7);
#pragma warning restore CS4014
            _elevator.RequestAsync(3, 15).Wait();
        }

        private static void Setup()
        {
            WriteLine("Bem vindo ao Elevador! Precisamos de algumas informações para começar:");

            WriteLine("\nQual a altura do prédio?");
            if (!int.TryParse(ReadLine(), out int heightBuilding))
                WriteLine("Altura inválida!");

            WriteLine("\nQual o andar atual do elevador?");
            if (!int.TryParse(ReadLine(), out int currentFloor))
                WriteLine("Andar atual do elevador inválido!");

            WriteLine("\nQual o andar mínimo que o elevador pode chegar?");
            if (!int.TryParse(ReadLine(), out int minFloor))
                WriteLine("Andar mínimo que o elevador inválido!");

            _elevator = new Elevator(currentFloor, minFloor, heightBuilding);

            _elevator.CurrentPositionChangedEvent += CurrentPositionChangedHandler;
            _elevator.StopPositionChangedEvent += StopPositionChangedHandler;
        }

        private static void CurrentPositionChangedHandler(object sender, EventArgs e)
        {
            // WriteLine($"{_elevator.CurrentPosition}.."); //TODO: Show the current position. No at the moment maybe later
        }

        private static void StopPositionChangedHandler(object sender, EventArgs e)
        {
            WriteLine($"\nParou no {_elevator.StopPosition} andar.");
        }
    }
}
