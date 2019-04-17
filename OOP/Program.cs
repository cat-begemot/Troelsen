using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
	class Program
	{
		static void Main(string[] args)
		{
			Object circle = new Manager();
			Shape hexagon = new Hexagon("Krolche");
			Shape threeDCircle = new ThreeDCircle();

			(circle as Shape)?.Draw();

			/*
			circle.Draw();
			hexagon.Draw();
			threeDCircle.Draw();
			*/
		}
	}

	public abstract class Shape
	{
		public Shape(String name="NoName")
		{
			PetName = name;
		}

		public String PetName { get; set; }

		public abstract void Draw();
	}

	public class Circle : Shape
	{
		public Circle()
		{

		}

		public Circle(String name) : base(name)
		{
		
		}

		public override void Draw()
		{
			Console.WriteLine($"Drawing {PetName} the Circle");
		}
	}

	public class Hexagon : Shape
	{
		public Hexagon()
		{

		}

		public Hexagon(String name) : base(name)
		{

		}

		public override void Draw()
		{
			Console.WriteLine($"Drawing {PetName} the Hexagon");
		}
	}

	public class ThreeDCircle : Circle
	{
		public override void Draw()
		{
			Console.WriteLine("Drawing a 3D Circle");
		}
	}
}
