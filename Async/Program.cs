using System;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace Async
{
	public delegate Int32 BinaryOp(Int32 x, Int32 y);

	internal class Program
	{
		private static Boolean isDone = false;

		static void PrintString(String str)
		{
			str += "World";
			Console.Write(str);
		}

		static void PrintSB (StringBuilder sb)
		{
			sb.Append("World");
			Console.Write(sb);
		}


		static void Main(string[] args)
		{
			String str = "Hello";
			PrintString(str);
			Console.Write(str);

			Console.WriteLine();

			StringBuilder sb = new StringBuilder("Hello");
			PrintSB(sb);
			Console.Write(sb);

			Console.WriteLine();
		}

		#region PoolOfThreads
		internal static void PoolOfThreads()
		{
			Console.WriteLine($"Main thread started. ThreadID: {Thread.CurrentThread.ManagedThreadId}");
			Printer p = new Printer();
			WaitCallback workItem = new WaitCallback((Object state) =>
				{
					Printer task = (Printer)state;
					task.PrintNumbers();
				});

			for (Int32 i = 0; i < 10; i++)
				ThreadPool.QueueUserWorkItem(workItem, p);
			Console.WriteLine("All tasks queued");
			Console.ReadLine();
		}
		#endregion

		#region TimerApp
		internal static void TimerApp()
		{
			TimerCallback cb = new TimerCallback(PrintTime);
			var t = new System.Threading.Timer(cb, "Hello from Main()!", 0, 1000);

			Console.WriteLine("Hit any key to terminate...");
			Console.ReadLine();
		}

		internal static void PrintTime(Object state)
		{
			Console.WriteLine($"{(String)state}: {DateTime.Now.ToString()}");
			
		}
		#endregion

		#region Concurrency
		internal static void MultiThreadedPrinting()
		{
			Printer p = new Printer();
			Thread[] threads = new Thread[10];
			for(Int32 i=0; i<10;i++)
			{
				threads[i] = new Thread(new ThreadStart(p.PrintNumbers))
				{
					Name = $"Worker thread #{i}"
				};
			}

			foreach (var t in threads)
				t.Start();

			Console.ReadLine();
		}
		#endregion

		#region SimpleMultiThreadApp
		internal static void SimpleMultiThreadApp()
		{
			Console.WriteLine("Do you want [1] or [2] threads?");
			String threadCount = Console.ReadLine();

			Thread primaryThread = Thread.CurrentThread;
			primaryThread.Name = "Primary";
			Console.WriteLine($"-> {Thread.CurrentThread.Name} is executing Main()");

			Printer p = new Printer();

			switch(threadCount)
			{
				case "2":
					Thread backgroundThread = new Thread(new ThreadStart(p.PrintNumbers));
					backgroundThread.Name = "Secondary";
					backgroundThread.Start();
					break;
				case "1":
					p.PrintNumbers();
					break;
				default:
					Console.WriteLine("I don't know what you want.. you get 1 thread.");
					goto case "1";
			}
			MessageBox.Show("I'm busy!", "Work on main thread...");
			Console.ReadLine();
		}

		[Synchronization]
		internal class Printer : ContextBoundObject
		{
			public void PrintNumbers()
			{
				Console.WriteLine($"-> {Thread.CurrentThread.Name} is executing PrintNumbers()");
				Console.WriteLine("Your numbers:");
				for (Int32 i = 0; i < 10; i++)
				{
					Random r = new Random();
					Thread.Sleep(100 * r.Next(5));
					Console.WriteLine($"{i}");
				}
				Console.WriteLine();				
			}
		}
		#endregion

		#region AddWithThreads
		private static AutoResetEvent waitHandle = new AutoResetEvent(false);

		internal static void AddWiththreads()
		{
			Console.WriteLine($"ID of thread in Main(): {Thread.CurrentThread.ManagedThreadId}");
			AddParams ap = new AddParams(10, 15);
			Thread t = new Thread(new ParameterizedThreadStart(Add));
			t.Start(ap);

			waitHandle.WaitOne();
			Console.WriteLine("Other thread is done!");
		}

		internal class AddParams
		{
			public Int32 a, b;
			public AddParams(Int32 num1, Int32 num2)
			{
				a = num1; b = num2;
			}
		}

		internal static void Add(Object data)
		{
			if (data is AddParams)
			{
				Console.WriteLine($"ID of thread in Add(): {Thread.CurrentThread.ManagedThreadId}");
				AddParams ap = (AddParams)data;
				Thread.Sleep(5000);
				Console.WriteLine($"{ap.a} + {ap.b} = {ap.a + ap.b}");
				waitHandle.Set();
			}
		}
		#endregion

		#region Delegate
		internal static void ThreadStat()
		{
			Thread primaryThread = Thread.CurrentThread;
			primaryThread.Name = "ThePrimaryThread";

			Console.WriteLine($"Name of the current AppDomain: {Thread.GetDomain().FriendlyName}");
			Console.WriteLine($"ID of current Context: {Thread.CurrentContext.ContextID}");

			Console.WriteLine($"Thread name: {primaryThread.Name}");
			Console.WriteLine($"Has thread started: {primaryThread.IsAlive}");
			Console.WriteLine($"Priority level: {primaryThread.Priority}");
			Console.WriteLine($"Thread state: {primaryThread.ThreadState}");

			Console.ReadLine();
		}

		internal static void AsyncWithCallBack()
		{
			Console.WriteLine($"Main() invoked in thread: {Thread.CurrentThread.ManagedThreadId}");

			BinaryOp b = new BinaryOp(Add);
			IAsyncResult ar = b.BeginInvoke(15, 10, new AsyncCallback(AddComplete), "Main() thanks you for adding these numbers!");

			while (!isDone)
			{
				Console.WriteLine("Working...");
				Thread.Sleep(1000);
			}

			Console.ReadLine();
		}

		internal static void AddComplete(IAsyncResult iar)
		{
			Console.WriteLine($"AddComplete() invoked in thread: {Thread.CurrentThread.ManagedThreadId}");
			Console.WriteLine("Your additional is complite");

			// Get the result
			AsyncResult ar = (AsyncResult)iar;
			BinaryOp b = (BinaryOp)ar.AsyncDelegate;
			Console.WriteLine($"10+15={b.EndInvoke(iar)}");

			// Additional object from BeginInvoke() method
			String str = (String)iar.AsyncState;
			Console.WriteLine($"Message: {str}");

			isDone = true;
		}

		internal static void AsyncCall()
		{
			Console.WriteLine($"Main() invoked on thread: {Thread.CurrentThread.ManagedThreadId}");

			BinaryOp b = new BinaryOp(Add);
			IAsyncResult ar = b.BeginInvoke(10, 15, null, null);

			while (!ar.AsyncWaitHandle.WaitOne(1000, true))
				Console.WriteLine("Doing more work in Main()!");

			Int32 answer = b.EndInvoke(ar);
			Console.WriteLine($"10+15={answer}");
			Console.ReadLine();
		}

		internal static void SyncCall()
		{
			Console.WriteLine($"Main() invoked on thread: {Thread.CurrentThread.ManagedThreadId}");
			BinaryOp b = new BinaryOp(Add);

			Int32 answer = b(10, 15);

			Console.WriteLine("Doing more work in Main()!");
			Console.WriteLine($"10+15={answer}");
			Console.ReadLine();
		}

		internal static Int32 Add(Int32 x, Int32 y)
		{
			Console.WriteLine($"Add() invoked on thread: {Thread.CurrentThread.ManagedThreadId}");
			Thread.Sleep(5000);
			return x + y;
		}
		#endregion
	}
}
