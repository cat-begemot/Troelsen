using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			Car[] myAutos = new Car[5];
			myAutos[0] = new Car("Rusty", 80, 1);
			myAutos[1] = new Car("Mary", 80, 234);
			myAutos[2] = new Car("Viper", 80, 34);
			myAutos[3] = new Car("Mel", 80, 4);
			myAutos[4] = new Car("Chucky", 80, 5);

			Array.Sort<Car>(myAutos, Car.SortByPetName);

			foreach(var car in myAutos)
				Console.WriteLine($"{car.PetName} - {car.CarID}");
		}
	}
}
