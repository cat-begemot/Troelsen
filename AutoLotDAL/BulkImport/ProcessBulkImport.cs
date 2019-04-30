using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace AutoLotDAL.BulkImport
{
	public static class ProcessBulkImport
	{
		private static SqlConnection _sqlConnection = null;
		private static readonly String _connectionString =
			@"Data Source=DESKTOP-24IFPL5\SQLEXPRESS;Initial Catalog=AutoLot;Integrated Security=True";

		public static void OpenConnection()
		{
			_sqlConnection = new SqlConnection(_connectionString);
			_sqlConnection.Open();
		}

		public static void CloseConnection()
		{
			if (_sqlConnection.State != ConnectionState.Closed)
				_sqlConnection.Close();
		}

		public static void ExecuteBulkImport<T>(IEnumerable<T> records, String tableName)
		{
			OpenConnection();
			using (SqlConnection conn = _sqlConnection)
			{
				SqlBulkCopy bc = new SqlBulkCopy(conn)
				{
					DestinationTableName = tableName
				};
				var dataReader = new MyDataReader<T>()
				{
					Records = records.ToList()
				};
				try
				{
					bc.WriteToServer(dataReader);
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				finally
				{
					CloseConnection();
				}
			}
		}
	}
}
