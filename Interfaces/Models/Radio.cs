using System;

namespace Interfaces
{
	internal class Radio
	{
		public void TurnOn(bool on)
		{
			Console.WriteLine(on ? "Jamming..." : "Quiet time...");
		}
	}
}
