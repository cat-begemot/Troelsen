using System;
using System.Diagnostics;
using System.Linq;

namespace Processes
{
	internal enum SortOrder
	{
		SortByPID,
		SortByName
	}

	class Program
  {
    static void Main(string[] args)
    {
			EnumModsForPid(SortOrder.SortByPID);
		}


		/// <summary>
		/// Start a browser, open web-page and close process
		/// </summary>
		internal static void StartAndKillProcess()
		{
			Process targetProcess = null;
			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo("chrome.exe");
				startInfo.WindowStyle = ProcessWindowStyle.Maximized;
				targetProcess = Process.Start(startInfo);
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			if(targetProcess==null)
			{
				Console.WriteLine("Process didn't run!");
				return;
			}

			Console.WriteLine($"--> Hit any key to kill {targetProcess.Id}...");
			Console.ReadLine();

			try
			{
				targetProcess.Kill();
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}


		/// <summary>
		/// Show list of modules which uses defined process
		/// </summary>
		/// <param name="sortOrder"></param>
		internal static void EnumModsForPid(SortOrder sortOrder)
		{
			ListAllRunningProcesses(sortOrder);
			Console.Write("\nEnter process ID: ");
			Int32 processID;
			if(Int32.TryParse(Console.ReadLine(), out processID))
			{
				Process targetProcess = null;
				try
				{
					targetProcess = Process.GetProcessById(processID);
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
					return;
				}
				Console.WriteLine($"\nHere are the loaded modules for {targetProcess.ProcessName}");
				ProcessModuleCollection moduleCollection = targetProcess.Modules;
				foreach(ProcessModule module in moduleCollection)
				{
					String info = $"-> Module name: {module.ModuleName}";
					Console.WriteLine(info);
				}
				Console.WriteLine($"\nTotal modules: {moduleCollection.Count}");
			}
			else
			{
				Console.WriteLine("Process ID must be a number which exists in list above!");
			}
			Console.ReadLine();
		}

		/// <summary>
		/// Show details about each thread in defined process
		/// </summary>
		public static void ShowThreadsOfProcess(SortOrder sortOrder)
		{
			ListAllRunningProcesses(sortOrder);
			Console.Write("\nEnter process ID: ");
			Int32 processID;
			if (Int32.TryParse(Console.ReadLine(), out processID))
			{
				EnumThreadsForPid(processID);
			}
			else
			{
				Console.WriteLine("Process ID must be a number which exists in list above!");
			}
			Console.ReadLine();
		}

		/// <summary>
		/// Show all threads of defined process
		/// </summary>
		/// <param name="pID"></param>
		internal static void EnumThreadsForPid(Int32 pID)
		{
			Process targetProc = null;
			try
			{
				targetProc = Process.GetProcessById(pID);
			}
			catch(ArgumentException ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			Console.WriteLine($"Here are the threads used by: {targetProc.ProcessName} on machine: {targetProc.MachineName}");
			ProcessThreadCollection threadCollection = targetProc.Threads;
			foreach (ProcessThread pt in threadCollection)
			{
				String info = $"-> Thread ID: {pt.Id}\t Start time: {pt.StartTime}\t Priority: {pt.PriorityLevel}";
				Console.WriteLine(info);
			}
			Console.WriteLine($"\nTotal threads: {threadCollection.Count}");
		}


		/// <summary>
		/// Enumerating running processes
		/// </summary>
		internal static void ListAllRunningProcesses(SortOrder sortOrder)
		{
			IOrderedEnumerable<Process> runningProcesses = null;
			switch (sortOrder)
			{
				case SortOrder.SortByPID:
					runningProcesses = Process.GetProcesses(".").OrderBy(proc => proc.Id);
					break;
				case SortOrder.SortByName:
					runningProcesses = Process.GetProcesses(".").OrderBy(proc => proc.ProcessName);
					break;
			}

			foreach (var proc in runningProcesses)
			{
				String info = $"-> PID: {proc.Id}\t Name: {proc.ProcessName}";
				Console.WriteLine(info);
			}
		}
  }


}
