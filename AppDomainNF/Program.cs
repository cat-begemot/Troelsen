using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace AppDomainNF
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CustomAppDomain();
			Console.ReadLine();
		}

		internal static void CustomAppDomain()
		{
			AppDomain defaultAD = AppDomain.CurrentDomain;
			defaultAD.ProcessExit += (e, s) =>
				{
					Console.WriteLine("Default AppDomain has been unloaded!");
				};
			MakeNewAppDomain();
		}

		private static void MakeNewAppDomain()
		{
			AppDomain newAD = AppDomain.CreateDomain("SecondAppDomain");
			newAD.DomainUnload += (o, s) =>
			{
				Console.WriteLine("The second AppDomain has been unloaded!");
			};

			try
			{
				newAD.ExecuteAssembly("Program.exe");
			}
			catch(FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
			}
			ListAllAssembliesInAppDomain(newAD);
			AppDomain.Unload(newAD);
		}

		/// <summary>
		/// Subscribes on AssemblyLoad event
		/// </summary>
		internal static void InitDAD()
		{
			AppDomain defaultAD = AppDomain.CurrentDomain;
			defaultAD.AssemblyLoad += (o, s) =>
			{
				Console.WriteLine($"{s.LoadedAssembly.GetName().Name} has been loaded!");
			};
		}

		/// <summary>
		/// Shows list of all assemblies which were loaded in current default AppDomain
		/// </summary>
		internal static void ListAllAssembliesInAppDomain(AppDomain appDomain)
		{
			IOrderedEnumerable<Assembly> loadedAssemblies = appDomain.GetAssemblies().OrderBy(a => a.GetName().Name);
			Console.WriteLine($"Here are the assemblies loaded in {appDomain.FriendlyName}");
			foreach (var assembly in loadedAssemblies)
			{
				Console.WriteLine($"--> Name: {assembly.GetName().Name}");
				Console.WriteLine($"    Version: { assembly.GetName().Version}\n");
			}
		}

		/// <summary>
		/// Displays some useful info about current AppDomain
		/// </summary>
		private static void DisplayDADStats()
		{
			AppDomain defaultAppDomain = AppDomain.CurrentDomain;

			Console.WriteLine($"Name of the current domain: {defaultAppDomain.FriendlyName}");
			Console.WriteLine($"ID of domain in this process: {defaultAppDomain.Id}");
			Console.WriteLine($"Is this default domain? {defaultAppDomain.IsDefaultAppDomain()}");
			Console.WriteLine($"Base directory of this domain: {defaultAppDomain.BaseDirectory}");
		}
	}
}
