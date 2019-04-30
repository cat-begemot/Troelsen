using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AutoLotDAL.DataOperations;
using AutoLotDAL.Models;
using AutoLotDAL.BulkImport;


namespace ADOConsole
{
	public class Program
	{
		public static void Main(string[] args)
		{
			DoBulkCopy();
		}



		#region AutoLotClient
		public static void DoBulkCopy()
		{
			var cars = new List<Car>()
			{
				new Car(){Color="Blue", Make="Honda", PetName="MyCar1"},
				new Car(){Color="Red", Make="Volvo", PetName="MyCar2"},
				new Car(){Color="White", Make="VW", PetName="MyCar3"},
				new Car(){Color="Yellow", Make="Toyota", PetName="MyCar4"},
			};
			ProcessBulkImport.ExecuteBulkImport(cars, "Inventory");
			var dal = new InventoryDAL();
			var list = dal.GetAllInventory();
			Console.WriteLine("CarId\tMake\tColor\tPet Name");
			foreach (var car in list)
				Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");
		}

		public static void MoveCustomer()
		{
			Boolean throwEx = true;
			Console.Write("Do you want to throw an exception (Y or N): ");
			var userAnswer = Console.ReadLine();
			if (userAnswer?.ToLower() == "n")
				throwEx = false;

			var dal = new InventoryDAL();
			dal.ProcessCreditRisk(throwEx, 1);
		}

		public static void AutoLotClient()
		{
			var dal = new InventoryDAL();
			var list = dal.GetAllInventory();
	
			Console.WriteLine("***** Show All Cars *****\n");
			Console.WriteLine("CarId\tMake\tColor\tPet Name");
			foreach(var car in list)
				Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");
			Console.WriteLine();

			Console.WriteLine("***** First Car By Color *****\n");
			var firstCarByColor = dal.GetCar(list.OrderBy(c => c.Color).Select(c => c.CarId).First());
			Console.WriteLine("CarId\tMake\tColor\tPet Name");
			Console.WriteLine($"{firstCarByColor.CarId}\t{firstCarByColor.Make}\t" +
				$"{firstCarByColor.Color}\t{firstCarByColor.PetName}");

			try
			{
				dal.DeleteCar(5);
				Console.WriteLine("Car deleted.");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"An exception occured: {ex.Message}");
			}

			dal.InsertAuto(new Car() { Color = "Blue", Make = "Pilot", PetName = "TowMonster" });
			list = dal.GetAllInventory();
			var newCar = list.First(c => c.PetName == "TowMonster");
			Console.WriteLine("***** New Car *****\n");
			Console.WriteLine("CarId\tMake\tColor\tPet Name");
			Console.WriteLine($"{newCar.CarId}\t{newCar.Make}\t" +
				$"{newCar.Color}\t{newCar.PetName}");

			var petName = dal.LookUpPetName(firstCarByColor.CarId);
			Console.WriteLine($"Car pet name (using SP): {petName}");
		}
		#endregion

		#region AutoLotDataReader
		public static void AutoLotDataReader()
		{
			using (var connection = new SqlConnection())
			{
				// Configure connection string
				String connectionString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
				var conStrBuilder = new SqlConnectionStringBuilder(connectionString);
				conStrBuilder.ConnectTimeout = 10;

				// Configure connection
				connection.ConnectionString = conStrBuilder.ConnectionString;
				Console.WriteLine($"Connection state: {connection.State}");
				connection.Open();
				Console.WriteLine($"Connection state: {connection.State}");

				// Configure command
				String sql = "select * from dbo.Inventory";
				var myCommnad = new SqlCommand(sql, connection);
				myCommnad.CommandType = CommandType.Text;

				using (var dataReader = myCommnad.ExecuteReader())
				{
					while(dataReader.Read())
					{
						Console.WriteLine($"-> Make: {dataReader["Make"]}, " +
							$"PetName: {dataReader["PetName"]}, " +
							$"Color: {dataReader["Color"]}");
					}
				}
			}
		}
		#endregion

		#region Data Provider Factory
		public static void DataProviderFactory()
		{
			String dataProvider = ConfigurationManager.AppSettings["provider"];
			String connectionString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;

			DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);

			using (DbConnection connection = factory.CreateConnection())
			{
				if(connectionString==null)
				{
					ShowError("Connection");
					return;
				}
				Console.WriteLine($"Your connection is a: {connection.GetType().Name}");
				connection.ConnectionString = connectionString;
				connection.Open();

				// Casting connection to derived type.
				var sqlConnection = connection as SqlConnection;
				if(sqlConnection!=null)
					Console.WriteLine($"SQL Server version is: {sqlConnection.ServerVersion}");

				// Make command object.
				DbCommand command = factory.CreateCommand();
				if(command==null)
				{
					ShowError("Command");
					return;
				}
				Console.WriteLine($"Your command object is a: {command.GetType().Name}");
				command.Connection = connection;
				command.CommandText = "SELECT * FROM dbo.Inventory";
				using (DbDataReader dataReader = command.ExecuteReader())
				{
					Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
					Console.WriteLine("\n***** Current Inventory *****");
					while(dataReader.Read())
						Console.WriteLine($"-> Car #{dataReader["CarId"]} is a {dataReader["Make"]}.");
				}
			}
		}

		private static void ShowError(String objectName)
		{
			Console.WriteLine($"There was an issue creating the {objectName}");
		}
		#endregion

		#region Very Simple Connection Factory
		public enum DataProvider
		{
			SqlServer, OleDb, Odbc, None
		}

		public static void ConnectionFactoryMain()
		{
			Console.WriteLine("***** Very Simple Connection Factory *****");
			String dataProviderString = ConfigurationManager.AppSettings["provider"];
			var dataProvider = DataProvider.None;
			if(Enum.IsDefined(typeof(DataProvider), dataProviderString))
				dataProvider=(DataProvider)Enum.Parse(typeof(DataProvider), dataProviderString);
			else
			{
				Console.WriteLine("Sorry, no provider exists!");
				return;
			}
			IDbConnection connection = GetConnection(dataProvider);
			Console.WriteLine($"Your connection is a {connection.GetType().Name ?? "unrecognized type"}");

			Console.ReadLine();
		}

		public static IDbConnection GetConnection(DataProvider dataProvider)
		{
			IDbConnection connection = null;
			switch(dataProvider)
			{
				case DataProvider.SqlServer:
					connection = new SqlConnection();
					break;
				case DataProvider.OleDb:
					connection = new OleDbConnection();
					break;
				case DataProvider.Odbc:
					connection = new OdbcConnection();
					break;
			}

			return connection;
		}
		#endregion
	}
}
