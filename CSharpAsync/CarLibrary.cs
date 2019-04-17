using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLibrary
{
	// Represents the state of the engine.
	public enum EngineState
	{ engineAlive, engineDead }

	// Which type of music player does this car have?
	public enum MusicMedia
	{
		musicCd,
		musicTape,
		musicRadio,
		musicMp3
	}

	// The abstract base class in the hierarchy.
	public class Car
	{
		public string PetName { get; set; }
		public int CurrentSpeed { get; set; }
		public int MaxSpeed { get; set; }

		protected EngineState egnState = EngineState.engineAlive;
		public EngineState EngineState => egnState;

		public Car() => Console.WriteLine("CarLibrary Version 2.0!");
		public Car(string name, int maxSp, int currSp)
		{
			Console.WriteLine("CarLibrary Version 2.0!");
			PetName = name; MaxSpeed = maxSp; CurrentSpeed = currSp;
		}

		public void TurboBoost()
		{
			this.egnState = EngineState.engineDead;
			Console.WriteLine("The engine is dead!!!");
		}

		public void TurnOnRadio(bool musicOn, MusicMedia mm)
				=> Console.WriteLine(musicOn ? $"Jamming {mm}" : "Quiet time...");

	}
}