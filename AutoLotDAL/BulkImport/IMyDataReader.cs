using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AutoLotDAL.BulkImport
{
	public interface IMyDataReader<T> : IDataReader
	{
		List<T> Records { get; set; }
	}
}
