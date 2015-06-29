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
        public int NumCol { get { return Table.Columns.Count; } }  // number of columns in the sheet
        public int NumRow { get { return Table.Rows.Count; } }  // number of rows in the sheet
        public int ColTranslated { get { return SearchCellInCol("Translated Text", 1); } }  // get the translated column

        public Excel(Stream stream)
        {
            // initializing excel sheet
            IExcelDataReader excelReader2007 = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader2007.AsDataSet();
            this.Table = result.Tables[0];
        }

        /// <summary>
        /// Get a cell content from its cordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Cell content</returns>
        public object GetCell(int x, int y)
        {
            if (((x < 0) || (x > NumCol - 1)) || ((y < 0) || (y > NumRow - 1)))
                return null;
            return this.Table.Rows[y].ItemArray[x];
        }

        /// <summary>
        /// Search a cell inside a row
        /// </summary>
        /// <param name="data">Content to search</param>
        /// <param name="row">Raw where to search</param>
        /// <returns></returns>
        public int SearchCellInCol(string data, int row)
        {
            for (int i = 0; i < this.NumCol; i++)
                if (GetCell(i, row).ToString() == data)
                    return i;
            return -1;
        }

        /// <summary>
        /// Search a cell inside a column
        /// </summary>
        /// <param name="data">Content to search</param>
        /// <param name="column">Column where to search</param>
        /// <returns></returns>
        public int SearchCellInRow(string data, int column)
        {
            for (int i = 0; i < this.NumRow; i++)
                if (GetCell(column, i).ToString() == data)
                    return i;
            return -1;
        }
    }
}
