using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
	class SalesPerson : Employee
	{
		public Int32 SalesNumbers { get; set; }

		public override void GiveBonus(float amount)
		{
			Int32 salesBonus = 0;
			if (SalesNumbers >= 0 && SalesNumbers <= 100)
				salesBonus = 10;
			else
			{
				if (SalesNumbers >= 101 && SalesNumbers <= 200)
					salesBonus = 15;
				else
					salesBonus = 20;
			}
			base.GiveBonus(amount * salesBonus);
		}

		public override void DisplayStats()
		{
			base.DisplayStats();
			Console.WriteLine("Sales numbers: {0}", SalesNumbers);
		}
	}
}
