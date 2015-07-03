using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ninokuni_Script_Importer.Font;

namespace Ninokuni_Script_Importer
{
    class Converter
    {
        public Excel     ExcelData { get; set; }
        public XDocument XmlDoc    { get; set; }
        public string    XmlPath   { get; set; }
        public TextManip Manip     { get; set; }

        private const int START_ROW_DATA = 2;

        public Converter(string excelPath, string xmlPath, string fontPath, int maxLength)
        {
            this.ExcelData = new Excel(new FileStream(excelPath, FileMode.Open));  // Excel
            
            // XML
            this.XmlDoc = new XDocument();
            XmlDoc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            this.XmlPath = xmlPath;

            // Font
            NFTR font = new NFTR();
            Stream fs = new FileStream(fontPath, FileMode.Open);
            font.Read(fs);
            this.Manip = new TextManip(font, maxLength);
        }

        /// <summary>
        /// Convert the Excel file in a Xml file
        /// </summary>
        public void StartProcess()
        {
            string dataType = this.ExcelData.GetCell(1, 0).ToString().Remove(0, 10);
            if (dataType != "Script")
                throw new NotImplementedException("DataType conversion still not supported!");

            XElement root = new XElement(dataType);
            XElement block = null;

            for (int y = START_ROW_DATA; y < ExcelData.NumRow; y++)
            {
                // get the block which contains the element
                if (!string.IsNullOrWhiteSpace(ExcelData.GetCell(0, y).ToString()))
                {
                    block = SetAttributes(ExcelData.GetCell(0, y).ToString());                
                    root.Add(block);
                }

                // check if it's a blank row
                if (string.IsNullOrWhiteSpace(ExcelData.GetCell(1, y).ToString())
                    && string.IsNullOrWhiteSpace(ExcelData.GetCell(ExcelData.ColTranslated, y).ToString()))
                    continue;

                // add all elements and attributes of this row
                XElement el = SetAttributes(ExcelData.GetCell(1, y).ToString());
                for (int i = 2; i < ExcelData.ColTranslated - 1; i++)
                {
                    if (string.IsNullOrWhiteSpace(ExcelData.GetCell(i, y).ToString()))
                        continue;
                    el.Add(new XElement(ExcelData.GetCell(i, y).ToString()));
                    el = (XElement)el.LastNode;
                }

                string aaa = ExcelData.GetCell(ExcelData.ColTranslated, y).ToString();
                el.Value = Manip.FormatString(aaa);
                block.Add(el); // save the element read
            }

            SaveFile(root); //save file
        }

        /// <summary>
        /// Parse a string and create a new XElement
        /// </summary>
        /// <param name="xmlAttributes">string to parse with XElement informations</param>
        /// <returns>XElement with tha informations of xmlAttributes</returns>
        public XElement SetAttributes(string xmlAttributes)
        {
            List<string> attributes = xmlAttributes.Split(' ').ToList<string>();
            XElement element = new XElement(attributes[0]);
            attributes.RemoveAt(0);
            foreach (string attr in attributes)
            {
                string[] val = attr.Replace("\"", "").Split('=');
                element.Add(new XAttribute(val[0], val[1]));
            }
            return element;
        }

        /// <summary>
        /// Save a XElement structure
        /// </summary>
        /// <param name="data">XML data</param>
        private void SaveFile(XElement data)
        {
            this.XmlDoc.Add(data);
            this.XmlDoc.Save(this.XmlPath);
        }
    }
}
