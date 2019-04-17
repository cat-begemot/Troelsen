using System;
using System.Collections.Generic;

namespace Interfaces
{
	internal class Car : IComparable<Car>
	{
		public Int32 CarID { get; set; }

		public static IComparer<Car> SortByPetName { get => new PetNameComparer(); }

		// Constant for maximum speed.
		public const int MaxSpeed = 100;
		// Car properties.
		public int CurrentSpeed { get; set; } = 0;
		public string PetName { get; set; } = "";
		// Is the car still operational?
		private bool carIsDead;
		// A car has-a radio.
		private Radio theMusicBox = new Radio();
		
		
		// Constructors.
		public Car() { }

		public Car(string name, int speed, Int32 id)
		{
			CurrentSpeed = speed;
			PetName = name;
			CarID = id;
		}
		public void CrankTunes(bool state)
		{
			// Delegate request to inner object.
			theMusicBox.TurnOn(state);
		}
		// See if Car has overheated.
		public void Accelerate(int delta)
		{
			if (carIsDead)
				Console.WriteLine("{0} is out of order...", PetName);
			else
			{
				CurrentSpeed += delta;
				if (CurrentSpeed > MaxSpeed)
				{
					Console.WriteLine("{0} has overheated!", PetName);
					CurrentSpeed = 0;
					carIsDead = true;
				}
				else
					Console.WriteLine("=> CurrentSpeed = {0}", CurrentSpeed);
			}
		}

		public Int32 CompareTo(Car other)
		{
			return this.CarID.CompareTo(other.CarID);
		}
	}

	public class PetNameComparer : IComparer<Car>
	{
		int IComparer<Car>.Compare(Car x, Car y)
		{
			return String.Compare(x.PetName, y.PetName);
		}
	}
}
