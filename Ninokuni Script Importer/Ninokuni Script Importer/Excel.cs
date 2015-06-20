using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninokuni_Script_Importer
{
    class Excel
    {
        private DataTable Table { get; set; }
        public int NumCol { get { return Table.Columns.Count; } }
        public int NumRow { get { return Table.Rows.Count; } }

        public Excel(Stream stream)
        {
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

        public int SearchCellInCol(string data, int row)
        {
            for (int i = 0; i < this.NumCol; i++)
                if (GetCell(i, row).ToString() == data)
                    return i;
            return -1;
        }

        public int SearchCellInRow(string data, int column)
        {
            for (int i = 0; i < this.NumRow; i++)
                if (GetCell(column, i).ToString() == data)
                    return i;
            return -1;
        }
    }
}
