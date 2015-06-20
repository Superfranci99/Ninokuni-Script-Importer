using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ninokuni_Script_Importer
{
    class Converter
    {
        public Excel     ExcelData { get; set; }
        public XDocument XmlDoc    { get; set; }
        public string    XmlPath   { get; set; }

        public Converter(string excelPath, string xmlPath)
        {
            this.ExcelData = new Excel(new FileStream(excelPath, FileMode.Open));      
            this.XmlDoc = new XDocument();
            XmlDoc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            this.XmlPath = xmlPath;
        }

        public void StartProcess()
        {
            string dataType = this.ExcelData.GetCell(1, 0).ToString().Remove(0, 10);
            if (dataType != "script")
                throw new NotImplementedException("DataType conversion still not supported!");

            XElement root = new XElement(dataType);
        }
    }
}
