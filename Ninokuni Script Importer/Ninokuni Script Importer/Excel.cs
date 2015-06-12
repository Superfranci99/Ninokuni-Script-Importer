using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel;
using System.Data;

namespace Ninokuni_Script_Importer
{
    class Excel
    {
        private DataTable Table { get; set; }
        public int NumCol { get { return Table.Columns.Count; } }
        public int NumRow { get { return Table.Rows.Count; } }

        public Excel(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            IExcelDataReader excelReader2007 = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader2007.AsDataSet();
            this.Table = result.Tables[0];
        }

        public object GetCell(int x, int y)
        {
            if (((x < 0) || (x > NumCol - 1)) || ((y < 0) || (y > NumRow - 1)))
                return null;
            return this.Table.Rows[y].ItemArray[x];
        }

    }
}
