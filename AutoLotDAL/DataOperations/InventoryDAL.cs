using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AutoLotDAL.Models;

namespace AutoLotDAL.DataOperations
{
	public class InventoryDAL
	{
		#region Private Fields
		private readonly String _connectionString;
		private SqlConnection _sqlConnection = null;
		#endregion

		#region Constructors
		public InventoryDAL() : this(@"Data Source=DESKTOP-24IFPL5\SQLEXPRESS;Initial Catalog=AutoLot;Integrated Security=True")
		{ }

		public InventoryDAL(String connectionString)
		{
			this._connectionString = connectionString;
		}
		#endregion

		#region Methods
		public void OpenConnection()
		{
			this._sqlConnection = new SqlConnection(this._connectionString);
			this._sqlConnection.Open();
		}

		public void CloseConnection()
		{
			if (this._sqlConnection.State != ConnectionState.Closed)
				this._sqlConnection.Close();
		}

		/// <summary>
		/// Get all inventory from the database.
		/// </summary>
		/// <returns>List of Car objects.</returns>
		public List<Car> GetAllInventory()
		{
			this.OpenConnection();

			var inventoryList = new List<Car>();
			var sql = "select * from dbo.Inventory;";

			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.CommandType = CommandType.Text;
				var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
				while (dataReader.Read())
					inventoryList.Add(new Car()
					{
						CarId = (Int32)dataReader["CarId"],
						Make = (String)dataReader["Make"],
						Color = (String)dataReader["Color"],
						PetName = (String)dataReader["PetName"]
					});
				dataReader.Close();
			}

			return inventoryList;
		}

		/// <summary>
		/// Get Car object by its Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Car GetCar(Int32 id)
		{
			this.OpenConnection();
			Car car = null;
			String sql = $"select * from dbo.Inventory where CarId={id};";
			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.CommandType = CommandType.Text;
				var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
				if (dataReader.Read())
					car = new Car()
					{
						CarId = (Int32)dataReader["CarId"],
						Make = (String)dataReader["Make"],
						Color = (String)dataReader["Color"],
						PetName = (String)dataReader["PetName"]
					};
				dataReader.Close();
			}

			return car;
		}

		public void InsertAuto(String color, String make, String petName)
		{
			this.OpenConnection();
			String sql = $"insert into dbo.Inventory (Make, Color, PetName) " +
				$"values ('{make}', '{color}', '{petName}');";

			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.CommandType = CommandType.Text;
				command.ExecuteNonQuery();
			}
			this.CloseConnection();
		}

		public void InsertAuto(Car car)
		{
			this.OpenConnection();
			String sql = $"insert into dbo.Inventory (Make, Color, PetName) " +
				$"values (@Make, @Color, @PetName);";

			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.Parameters.Add(new SqlParameter()
				{
					ParameterName="@Make",
					Value=car.Make,
					SqlDbType=SqlDbType.Char,
					Size=10
				});

				command.Parameters.Add(new SqlParameter()
				{
					ParameterName="@Color",
					Value=car.Color,
					SqlDbType=SqlDbType.Char,
					Size=10
				});

				command.Parameters.Add(new SqlParameter()
				{
					ParameterName = "@PetName",
					Value = car.PetName,
					SqlDbType = SqlDbType.Char,
					Size = 10
				});

				command.CommandType = CommandType.Text;
				command.ExecuteNonQuery();
			}
			this.CloseConnection();
		}

		/// <summary>
		/// Delete Car object from the DB by car id.
		/// </summary>
		/// <param name="id"></param>
		public void DeleteCar(Int32 id)
		{
			this.OpenConnection();
			String sql = $"delete from dbo.Inventory where CarId={id};";
			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.CommandType = CommandType.Text;
				try
				{
					command.ExecuteNonQuery();
				}
				catch(SqlException ex)
				{
					Exception error = new Exception("Sorry! That car is on order", ex);
					throw error;
				}
				finally
				{
					this.CloseConnection();
				}
			}			
		}

		public void UpdateCarPetName(Int32 id, String newPetName)
		{
			this.OpenConnection();
			String sql = $"update dbo.Inventory set PetName={newPetName} where CarId={id};";

			using (var command = new SqlCommand(sql, this._sqlConnection))
			{
				command.ExecuteNonQuery();
			}

			this.CloseConnection();
		}

		public String LookUpPetName(Int32 carId)
		{
			this.OpenConnection();
			String carPetName;

			using (var command = new SqlCommand("GetPetName", this._sqlConnection))
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter()
				{
					ParameterName="@carId",
					Value = carId,
					SqlDbType =SqlDbType.Int,
					Direction=ParameterDirection.Input					
				});
				command.Parameters.Add(new SqlParameter()
				{
					ParameterName="@petName",
					SqlDbType=SqlDbType.Char,
					Size=10,
					Direction=ParameterDirection.Output
				});
				command.ExecuteNonQuery();

				carPetName = (String)command.Parameters["@petName"].Value;
			}
			this.CloseConnection();

			return carPetName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="throwEx"></param>
		/// <param name="custId"></param>
		public void ProcessCreditRisk(Boolean throwEx, Int32 custId)
		{
			this.OpenConnection();
			String fName;
			String lName;
			var cmdSelect = new SqlCommand($"select * from dbo.Customers where CustId={custId};", this._sqlConnection);
			using (var dataReader = cmdSelect.ExecuteReader())
			{
				if(dataReader.HasRows)
				{
					dataReader.Read();
					fName = (String)dataReader["FirstName"];
					lName = (String)dataReader["LastName"];
				}
				else
				{
					this.CloseConnection();
					return;
				}
			}

			var cmdRemove = new SqlCommand($"delete from dbo.Customers where CustId={custId};", 
				this._sqlConnection);
			var cmdInsert = new SqlCommand($"insert into dbo.CreditRisk(FirstName, LastName) values ('{fName}', '{lName}');",
				this._sqlConnection);

			SqlTransaction tx = null;

			try
			{
				tx = this._sqlConnection.BeginTransaction();
				cmdInsert.Transaction = tx;
				cmdRemove.Transaction = tx;

				cmdInsert.ExecuteNonQuery();
				cmdRemove.ExecuteNonQuery();

				if (throwEx)
					throw new Exception("Sorry! Database error! Tx failed...");

				tx.Commit();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				tx?.Rollback();
			}
			finally
			{
				this.CloseConnection();
			}
		}
		#endregion
	}
}
