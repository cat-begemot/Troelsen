using System;
using System.Reflection;
using System.Linq;

namespace AppDomainTest
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
			ListAllAssembliesInAppDomain(defaultAD);
			Console.WriteLine();
			MakeNewAppDomain();
		}

		private static void MakeNewAppDomain()
		{
			AppDomain newAD = AppDomain.CreateDomain("SecondAppDomain");
			ListAllAssembliesInAppDomain(newAD);
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
			foreach(var assembly in loadedAssemblies)
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
