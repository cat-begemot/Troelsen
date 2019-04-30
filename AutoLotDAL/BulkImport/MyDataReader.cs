using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AutoLotDAL.Models;

namespace AutoLotDAL.BulkImport
{
	public class MyDataReader<T> : IMyDataReader<T>
	{
		private Int32 _currentIndex = -1;
		private readonly PropertyInfo[] _propertyInfos;
		private readonly Dictionary<String, Int32> _nameDictionary;

		public MyDataReader()
		{
			this._propertyInfos = typeof(T).GetProperties();
			this._nameDictionary = this._propertyInfos
				.Select((x, index) => new { x.Name, index })
				.ToDictionary(pair => pair.Name, pair => pair.index);
		}

		/// <summary>
		/// Get the next record.
		/// </summary>
		/// <returns>Returns true if there is another record or returns false if at the end of the list.</returns>
		public bool Read()
		{
			if ((this._currentIndex + 1) >= this.Records.Count)
				return false;
			this._currentIndex++;
			return true;
		}

		/// <summary>
		/// Get the value of a field based on the original position
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public object GetValue(int i)
		{
			return this._propertyInfos[i].GetValue(this.Records[this._currentIndex]);
		}

		public int FieldCount => this._propertyInfos.Length;

		public string GetName(int i)
		{
			return i >= 0 && i < this.FieldCount ? this._propertyInfos[i].Name : String.Empty;
		}

		public int GetOrdinal(string name)
		{
			return this._nameDictionary.ContainsKey(name) ? this._nameDictionary[name] : -1;
		}




		public object this[int i] => throw new NotImplementedException();

		public object this[string name] => throw new NotImplementedException();

		public List<T> Records { get; set; }

		public int Depth => throw new NotImplementedException();

		public bool IsClosed => throw new NotImplementedException();

		public int RecordsAffected => throw new NotImplementedException();

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public bool GetBoolean(int i)
		{
			throw new NotImplementedException();
		}

		public byte GetByte(int i)
		{
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i)
		{
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

		public Type GetFieldType(int i)
		{
			throw new NotImplementedException();
		}

		public float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			throw new NotImplementedException();
		}

		public short GetInt16(int i)
		{
			throw new NotImplementedException();
		}

		public int GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public long GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public string GetString(int i)
		{
			throw new NotImplementedException();
		}


		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i)
		{
			throw new NotImplementedException();
		}

		public bool NextResult()
		{
			throw new NotImplementedException();
		}
	}
}
