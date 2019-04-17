using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Reflection
{
	public class Program
  {
    public static void Main(string[] args)
    {
			ReflectOnAttributesUsingearlyBinding();
		}

		public static void ReflectOnAttributesUsingearlyBinding()
		{
			Type type = typeof(Winnebago);
			var customAtts = type.GetCustomAttributes(false);
			foreach(VehicleDescriptionAttribute v in customAtts)
				Console.WriteLine($"-> {v.Description}");
		}

		[Serializable]
		[VehicleDescription(Description = "My rocking Harley")]
		public class Motorcycle
		{
		}

		[Serializable]
		[Obsolete("Use another vehicle!")]
		[VehicleDescription("The old gray mare, she ain't what she used to be...")]
		public class HorseAndBuggy
		{
		}

		[VehicleDescription("A very long, slow, but feature-rich auto")]
		public class Winnebago
		{
		}


		public sealed class VehicleDescriptionAttribute : System.Attribute
		{
			public String Description { get; set; }

			public VehicleDescriptionAttribute(String vehicleDescription)
			{
				Description = vehicleDescription;
			}

			public VehicleDescriptionAttribute() { }
		}

		/*
		#region Old Examples
		public static void LateBindingApp()
		{
			Assembly asm = null;
			try
			{
				String asmPath = Path.Combine(@"C:\Users\cat-b\source\repos\Troelsen\CSharpAsync\bin\Debug\", "CSharpAsync.exe").ToString();
				asm = Assembly.LoadFrom(asmPath);
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			if (asm != null)
				CreateUsingLateBinding(asm);
		}

		public static void CreateUsingLateBinding(Assembly asm)
		{
			try
			{
				Type car = asm.GetType("CarLibrary.Car");
				Object obj = Activator.CreateInstance(car);
				Console.WriteLine($"Created {obj} using late binding!");
				MethodInfo TurnOnRadio = car.GetMethod("TurnOnRadio");
				TurnOnRadio.Invoke(obj, new object[] { true, 2 });

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		/// <summary>
		/// Helper method for showing types list in assembly
		/// </summary>
		/// <param name="asm">Loaded in memory reffernce to assembly</param>
		private static void DisplayTypesInAsm(Assembly asm)
		{
			Console.WriteLine($"\n***** Types in Assembly *****");
			Console.WriteLine($"-> {asm.FullName}");
			Type[] types = asm.GetTypes();
			foreach(Type type in types)
				Console.WriteLine($"-> {type}");
			Console.WriteLine();
		}

		/// <summary>
		/// Connect an assembly to the program and show its types
		/// </summary>
		public static void ExternalAssemblyReflector()
		{
			Console.WriteLine("***** External Assembly Viewer *****");
			String asmName = String.Empty;
			Assembly asm = null;
			AssemblyName asmIdentity = new AssemblyName();

			do
			{
				Console.WriteLine("\nEnter an assembly to evaluate");
				Console.Write("or enter Q to quit: ");
				asmName = Console.ReadLine();
				if (asmName.Equals("Q", StringComparison.OrdinalIgnoreCase))
					break;

				try
				{
					asm = Assembly.LoadFrom(Path.Combine(@"C:\Users\cat-b\source\repos\Troelsen\Reflection\bin\Debug\", asmName).ToString());
					DisplayTypesInAsm(asm);
				}
				catch
				{
					Console.WriteLine("Sorry, can't find assembly");
				}
			} while (true);

		}

		public static void TypeViewer()
		{
			ITypeViewer tv = new LinqTypeViewer();
			Type type = Type.GetType("System.Enum", false, true);

			tv.ListMethods();
			tv.ListFields();
			tv.ListProperties();
			tv.ListInterfaces();
			tv.ListVariousStats();
		}
		#endregion
		*/
  }
}
