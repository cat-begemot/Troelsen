using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
	class Manager : Employee
	{
		public Int32 StockOptions { get; set; }

		public override sealed void GiveBonus(float amount)
		{
			base.GiveBonus(amount);
			Random r = new Random();
			StockOptions += r.Next(500);
		}

		public override void DisplayStats()
		{
			base.DisplayStats();
			Console.WriteLine("Stock options number: {0}", StockOptions);
		}
	}
}
