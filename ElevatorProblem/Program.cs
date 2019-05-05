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
            Setup();

            while (true)
            {
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

                passenger.DoAction(async () =>
                {
                    try
                    {
                        await _elevator.RequestAsync(currentFloor, goToFloor);
                    }
                    catch (SamePositionException)
                    {
                        WriteLine("\nO andar de partida precisa ser diferente do andar de destino!");
                    }
                });
            }
        }

        private static void Setup()
        {
            WriteLine("Bem vindo ao Smart-Elevador! Precisamos de algumas informações para começar:");

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

            _elevator.CurrentPositionChangedEvent += CurrentPositionChangedEvent;
        }

        private static void CurrentPositionChangedEvent(object sender, EventArgs e)
        {
            Write($"{_elevator.CurrentPosition}..");
        }
    }
}
