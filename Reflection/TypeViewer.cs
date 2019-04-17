using System;
using System.Reflection;
using System.Linq;
using System.Text;

namespace Reflection
{
	public interface ITypeViewer
	{
		Type InspectedType { get; set; }
		void ListMethods();
		void ListFields();
		void ListProperties();
		void ListInterfaces();
		void ListVariousStats();
	}

	public abstract class TypeViewer
	{
		public Type InspectedType { get; set; }
		public ConsoleColor StatisticsNameColor { get; set; } = ConsoleColor.Blue;
		public ConsoleColor ReturnTypeColor { get; set; } = ConsoleColor.Yellow;
		public ConsoleColor ParametersColor { get; set; } = ConsoleColor.Yellow;

		public TypeViewer()
		{
			this.InspectedType = this.GetType();
		}

		public TypeViewer(Type inspectedType)
		{
			this.InspectedType = inspectedType;
		}

		public void ShowConsoleColor()
		{
			foreach(var color in Type.GetType("System.ConsoleColor").GetFields())
			{
				Console.WriteLine(color.ToString());
			}
		}

		public void ListVariousStats()
		{
			this.WriteColorText("***** Various Statistics *****", this.StatisticsNameColor);
			Console.WriteLine($"Base class is: {this.InspectedType.BaseType}");
			Console.WriteLine($"Is type abstract? {this.InspectedType.IsAbstract}");
			Console.WriteLine($"Is type sealed? {this.InspectedType.IsSealed}");
			Console.WriteLine($"Is type generic? {this.InspectedType.IsGenericTypeDefinition}");
			Console.WriteLine($"Is type a class type? {this.InspectedType.IsClass}");
			Console.WriteLine();
		}

		public void ListMethods()
		{
			this.WriteColorText($"***** Methods of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			MethodInfo[] mi = this.InspectedType.GetMethods();
			Boolean hasParams;
			if (mi.Count() == 0)
				Console.WriteLine("-> No elements");
			else
			{
				foreach (MethodInfo method in mi)
				{
					this.WriteColorText(method.ReturnType.Name, this.ReturnTypeColor, false);
					Console.Write(" " + method.Name + "(");
					hasParams = false;
					foreach (var param in method.GetParameters())
					{
						if (hasParams)
							Console.Write(", ");
						else
							hasParams = true;
						this.WriteColorText(param.ParameterType.Name.ToString(), this.ParametersColor, false);
						Console.Write(" " + param.Name);
					}
					Console.WriteLine(")");
				}
			}
			Console.WriteLine();
		}

		public void WriteColorText(String message, ConsoleColor textColor, Boolean addNewLine=true)
		{
			var tempTextColor = Console.ForegroundColor;
			Console.ForegroundColor = textColor;
			if(addNewLine)
				Console.WriteLine(message);
			else
				Console.Write(message);
			Console.ForegroundColor = tempTextColor;
		}
	}

	public class MyTypeViewer : TypeViewer, ITypeViewer
	{
		public MyTypeViewer() {	}
		public MyTypeViewer(Type inspectedType) : base(inspectedType) { }

		public void ListFields()
		{
			this.WriteColorText($"***** Fields of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			FieldInfo[] fi = this.InspectedType.GetFields();
			if(fi.Count()==0)
				Console.WriteLine("-> No elements");
			else
				foreach (FieldInfo field in fi)
					Console.WriteLine($"-> {field.Name}");
			Console.WriteLine();
		}

		public void ListInterfaces()
		{
			this.WriteColorText($"***** Interfaces of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			Type[] ii = this.InspectedType.GetInterfaces();
			if (ii.Count() == 0)
				Console.WriteLine("-> No elements");
			else
				foreach (Type i in ii)
					Console.WriteLine($"-> {i.Name}");
			Console.WriteLine();
		}

		public void ListProperties()
		{
			this.WriteColorText($"***** Properties of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			PropertyInfo[] pi = this.InspectedType.GetProperties();
			if(pi.Count()==0)
				Console.WriteLine("-> No elements");
			else
				foreach(var property in pi)
					Console.WriteLine($"-> {property.Name}");
			Console.WriteLine();
		}
	}

	public class LinqTypeViewer : TypeViewer, ITypeViewer
	{
		public LinqTypeViewer() { }
		public LinqTypeViewer(Type inspectedType) : base(inspectedType) { }

		public void ListFields()
		{
			this.WriteColorText($"***** Fields of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			var fieldNames = from field in this.InspectedType.GetFields() select field.Name;
			if(fieldNames.Count()==0)
				Console.WriteLine("-> No elements");
			else
				foreach (var name in fieldNames)
					Console.WriteLine($"-> {name}");
			Console.WriteLine();
		}

		public void ListProperties()
		{
			this.WriteColorText($"***** Properties of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			var propertyNames = from property in this.InspectedType.GetProperties() select property.Name;
			if (propertyNames.Count() == 0)
				Console.WriteLine("-> No elements");
			else
				foreach (var name in propertyNames)
					Console.WriteLine($"-> {name}");
			Console.WriteLine();
		}

		public void ListInterfaces()
		{
			this.WriteColorText($"***** Interfaces of {this.InspectedType.Name} *****", this.StatisticsNameColor);
			var interfaces = from i in this.InspectedType.GetInterfaces() select i;
			if(interfaces.Count()==0)
				Console.WriteLine("-> No elements");
			else
				foreach(Type i in interfaces)
					Console.WriteLine($"-> {i.Name}");
			Console.WriteLine();
		}
	}
}
