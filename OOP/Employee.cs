using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
	public abstract class Employee
	{
		// Field data.
		protected string empName;
		protected int empID;
		protected float currPay;
		protected int empAge;
		protected string empSSN = "";

		public virtual void GiveBonus(float amount)
		{
			Pay += amount;
		}

		public virtual void DisplayStats()
		{
			Console.WriteLine("Name: {0}", Name);
			Console.WriteLine("ID: {0}", ID);
			Console.WriteLine("Age: {0}", Age);
			Console.WriteLine("Pay: {0}", Pay);
			Console.WriteLine("SSN: {0}", SocialSecurityNumber);
		}

		#region Ctors
		// Note use of constructor chaining.
		public Employee() { }
		public Employee(string name, int id, float pay)
			: this(name, 0, id, pay)
		{ }
		public Employee(string name, int age, int id, float pay)
		{
			Name = name;
			Age = age;
			ID = id;
			Pay = pay;
		}
		#endregion

		#region Properties 
		// Properties!
		public string Name
		{
			get { return empName; }
			set
			{
				if (value.Length > 15)
					Console.WriteLine("Error!  Name length exceeds 15 characters");
				else
					empName = value;
			}
		}

		// We could add additional business rules to the sets of these properties,
		// however there is no need to do so for this example.
		public int ID
		{
			get { return empID; }
			set { empID = value; }
		}
		public float Pay
		{
			get { return currPay; }
			set { currPay = value; }
		}
		public int Age
		{
			get { return empAge; }
			set { empAge = value; }
		}


		/// <summary>
		/// Read-only property that saves SSN number
		/// </summary>
		public string SocialSecurityNumber
		{
			get { return empSSN; }
		}
		#endregion
	}
}
