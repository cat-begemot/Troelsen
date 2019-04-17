using System;

namespace Delegates
{
	public class Program
	{	
		public static void Main(string[] args)
		{
			Int32 counter = 0;
			var car = new Car() { PetName = "Alex" };

			car.AboutToBlow += (sender, e) =>
			{
				counter++;
				if (sender is Car c)
					Console.WriteLine($"Warning from {c.PetName}: {e.msg}");
			};

			car.Accelerate(10);

			car.Exploded += (sender, e) =>
			{
				counter++;
				if (sender is Car c)
					Console.WriteLine($"Critical from {c.PetName}: {e.msg}");
			};
			car.Accelerate(10);

			Console.WriteLine($"Events counter value: {counter}");
		}
	}
}
