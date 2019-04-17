using System;

namespace Delegates
{
	public class CarEventArg : EventArgs
	{
		public readonly String msg;
		public CarEventArg(String message)
		{
			msg = message;
		}
	}

	public class Car
	{
		public delegate void CarEngineHandle(Object sender, CarEventArg arg);

		public Int32 CurrentSpeed { get; set; }
		public Int32 MaxSpeed { get; set; }
		public String PetName { get; set; }

		private Boolean carIdDead=false;
		//public CarEngineHandle listOfHandlers;

		public event EventHandler<CarEventArg> Exploded;
		public event EventHandler<CarEventArg> AboutToBlow;

		public Car()
		{

		}

		public Car(String name, Int32 maxSpeed, Int32 currentSpeed)
		{
			PetName = name;
			MaxSpeed = maxSpeed;
			CurrentSpeed = currentSpeed;
		}
		/*
		public void RegisterWithCarEngine(CarEngineHandle methodToCall)
		{
			listOfHandlers += methodToCall;
		}

		public void UnRegisterWithCarEngine(CarEngineHandle methodToCall)
		{
			listOfHandlers -= methodToCall;
		}
		*/
		public void Accelerate(Int32 delta)
		{
			if (carIdDead)
			{
				Exploded?.Invoke(this, new CarEventArg("Sorry, this car is dead..."));
			}
			else
			{
				CurrentSpeed += delta;
				if (MaxSpeed - CurrentSpeed <= 10)
					AboutToBlow?.Invoke(this, new CarEventArg("Careful buddy! Gonna blow!"));
				if (CurrentSpeed > MaxSpeed)
					this.carIdDead = true;
				else
					Console.WriteLine($"CurrentSpeed = {this.CurrentSpeed}");
			}
		}
	}
}
