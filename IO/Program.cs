using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace IO
{
	[Serializable]
	[XmlType]
	public class Radio
	{
		public Boolean hasTweeters;
		public bool hasSubWoofers;
		public Double[] stationPresets;

		[NonSerialized]
		[XmlIgnore]
		public String radioID = "XF-552RR6";
	}

	[Serializable]
	public class Car
	{
		public Radio theRadio = new Radio();
		public Boolean isHatchBack;
	}

	[Serializable]
	public class JamesBondCar : Car
	{
		public JamesBondCar() { }
		public JamesBondCar(Boolean skyWorthy, Boolean seaWorthy)
		{
			this.canFly = skyWorthy;
			this.canSubmerge = seaWorthy;
			this.description = "empty";
		}

		[XmlAttribute]
		public Boolean canFly;
		[XmlAttribute]
		public Boolean canSubmerge;
		public String description;

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			this.description = this.description.ToUpper();
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			SaveListOfCars();
			//SimpleSerialize();
			//SimpleDeserialize();

			Console.WriteLine("Task is finished!");

			Console.ReadLine();
		}

		public static void SaveListOfCars()
		{
			List<JamesBondCar> cars = new List<JamesBondCar>();
			cars.Add(new JamesBondCar(true, true));
			cars.Add(new JamesBondCar(true, false));
			cars.Add(new JamesBondCar(false, true));
			cars.Add(new JamesBondCar(false, false));

			var xmlFormat = new XmlSerializer(typeof(List<JamesBondCar>));

			using (Stream fStream = new FileStream("CarData.dat", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				xmlFormat.Serialize(fStream, cars);
			}
		}

		public static void SimpleDeserialize()
		{
			JamesBondCar jbc = null;
			var soapFormat = new SoapFormatter();
			using (Stream fStream = new FileStream("CarData.dat", FileMode.Open, FileAccess.Read, FileShare.None))
			{
				jbc = soapFormat.Deserialize(fStream) as JamesBondCar;
			}

			if(jbc!=null)
			{
				foreach(var wave in jbc.theRadio.stationPresets)
					Console.WriteLine($"Wave: {wave:f2}");
			}
		}

		public static void SimpleSerialize()
		{
			var jbc = new JamesBondCar();
			jbc.canFly = true;
			jbc.canSubmerge = false;
			jbc.theRadio.stationPresets = new double[] { 89.3, 105.1, 97.1 };
			jbc.theRadio.hasTweeters = true;

			var soamFormat = new SoapFormatter();
			using (Stream fStream = new FileStream("CarData.dat", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				soamFormat.Serialize(fStream, jbc);
			}
		}

		#region FS classes
		public static void BinaryWriter()
		{
			FileInfo file = new FileInfo("BinFile.dat");
			using (BinaryWriter bw = new BinaryWriter(file.OpenWrite()))
			{
				Console.WriteLine($"Base stream is: {bw.BaseStream}");
			}
		}

		public static void DirectoryWatcher()
		{
			var watcher = new FileSystemWatcher();
			try
			{
				watcher.Path = "MyFolder";
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			watcher.NotifyFilter = NotifyFilters.LastAccess
				| NotifyFilters.LastWrite
				| NotifyFilters.FileName
				| NotifyFilters.DirectoryName;

			watcher.Filter = "*.txt";
			watcher.Changed += Watcher_Changed;
			watcher.Created += new FileSystemEventHandler(Watcher_Changed);
			watcher.Deleted += new FileSystemEventHandler(Watcher_Changed);
			watcher.Renamed += new RenamedEventHandler(OnRenamed);

			watcher.EnableRaisingEvents = true;

		}

		private static void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($"File: {e.FullPath} {e.ChangeType}!");
		}

		private static void OnRenamed(Object source, RenamedEventArgs e)
		{
			Console.WriteLine($"File {e.OldFullPath} renamed to {e.FullPath}");
		}

		public static void StreamReader()
		{
			String path = "remiders.txt";
			if (File.Exists(path))
				using (StreamReader reader = File.OpenText(path))
				{
					String input = null;
					while ((input = reader.ReadLine()) != null)
						Console.WriteLine(input);
				}
			else
				Console.WriteLine($"File '{path}' doesn't exist!");
		}

		public static void StreamWriter()
		{
			using (StreamWriter writer = File.CreateText("remiders.txt"))
			{
				writer.WriteLine("Don't forget Mother's Day this year...");
				writer.WriteLine("Don't forget Father's Day this year...");
				writer.WriteLine("Don't forget these numbers:");
				for (Int32 i = 0; i < 10; i++)
					writer.Write(i + " ");
				writer.Write(writer.NewLine);
			}

			Console.WriteLine("Created file and wrote some thoughts...");
		}

		public static void ReadWriteText()
		{
			using (FileStream fStream = File.Open(@"myMessage.dat", FileMode.Create))
			{
				string msg = "Hello!";
				Byte[] msgAsByteArray = Encoding.Default.GetBytes(msg);
				fStream.Write(msgAsByteArray, 0, msgAsByteArray.Length);

				fStream.Position = 0;
				Byte[] bytesFromFile = new Byte[msgAsByteArray.Length];
				for (Int32 ind = 0; ind < msgAsByteArray.Length; ind++)
				{
					bytesFromFile[ind] = (Byte)fStream.ReadByte();
					Console.Write(bytesFromFile[ind]);
				}
				Console.WriteLine();
				Console.WriteLine(Encoding.Default.GetString(bytesFromFile));
			}
		}

		public static void DrivesInfo()
		{
			var drives = DriveInfo.GetDrives();
			foreach (var drive in drives)
			{
				Console.WriteLine($"Name: {drive.Name}");
				Console.WriteLine($"Type: {drive.DriveType}");

				if (drive.IsReady)
				{
					Console.WriteLine($"Free space: {drive.TotalFreeSpace / 1024 / 1024 / 1024:f2} Gb");
					Console.WriteLine($"Format: {drive.DriveFormat}");
					Console.WriteLine($"Label: {drive.VolumeLabel}");
				}
			}
		}

		public static void DisplayImageFiles()
		{
			var dir = new DirectoryInfo(@"C:\Windows\Web\Wallpaper");
			if (dir.Exists)
			{
				var imageFiles = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
				Console.WriteLine($"Found {imageFiles.Count()} file(s)");
				foreach (var imgFile in imageFiles)
				{
					Console.Write($"Name: {imgFile.Name}, ");
					Console.Write($"Length: {imgFile.Length}, ");
					Console.Write($"{imgFile.CreationTime}, ");
					Console.Write($"{imgFile.Attributes}\n");
				}
			}
		}

		public static void ShowWindowsDirectoryInfo()
		{
			DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");
			if (dir.Exists)
			{
				Console.WriteLine($"Full Name: {dir.FullName}");
				Console.WriteLine($"Name: {dir.Name}");
				Console.WriteLine($"Parent: {dir.Parent}");
				Console.WriteLine($"Creation: {dir.CreationTime}");
				Console.WriteLine($"Attributes: {dir.Attributes}");
				Console.WriteLine($"Root: {dir.Root}");
			}
		}
		#endregion
	}
}
