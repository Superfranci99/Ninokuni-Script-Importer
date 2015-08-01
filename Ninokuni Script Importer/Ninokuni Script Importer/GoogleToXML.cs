using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ninokuni_Script_Importer.Font;
using System.Xml;

namespace Ninokuni_Script_Importer
{
    class GoogleToXML
    {
        int blocks = 3, counter = 0;
        int maxDepth = 0, depth = 0, tabs = 0;
        string path, converted = "";
        string[] nodesText;
        List<string[]> ssEntry = new List<string[]>(); //List of lines in arrays split at \t

        XmlTextWriter xmlWriter;
        public ExcelSheet Excel { get; set; }
        public NFTR Font { get; set; }
        public TextManip Manip { get; set; }
        public string FolderOut { get; set; }

        public GoogleToXML(Stream excelStream, NFTR font, int maxPixel, string folderOut)
        {
            this.Font = font;
            this.Excel = new ExcelSheet(excelStream);
            this.Manip = new TextManip(font, maxPixel);
            this.FolderOut = folderOut;
        }

        public void FromSpreadsheet()
        {
            int cols = this.Excel.NumCol;
            int rows = this.Excel.NumRow;
            string a = this.Excel.GetCell(1, 1).ToString();
            if (!this.Excel.GetCell(1, 1).ToString().Contains("Filename"))
            {
                Console.WriteLine("ERROR: Filename corrupted. Please reset the filename in the top-left column.");
                return;
            }

            Console.WriteLine("Making file " + this.Excel.GetCell(1, 1).ToString().Substring(9) + "...");
            xmlWriter = new XmlTextWriter(this.FolderOut + this.Excel.GetCell(1, 1).ToString().Substring(9) + ".xml",
                System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 2;
            xmlWriter.IndentChar = ' ';

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(this.Excel.GetCell(1, 2).ToString().Substring(10));
            if (this.Excel.GetCell(1, 2).ToString().Substring(10) == "Subtitle")
                xmlWriter.WriteAttributeString("Name", this.Excel.GetCell(1, 1).ToString().Substring(9));

            int translatedColumn = 0;
            for (int i = 1; i <= cols; i++)
            {
                if (this.Excel.GetCell(2, i).ToString() == "Translated Text")
                {
                    translatedColumn = i;
                    break;
                }
            }

            string[] split;
            int depth = 0;

            //first line to initiate loop
            for (int j = 1; j < translatedColumn - 1; j++)
            {
                if (!string.IsNullOrEmpty(this.Excel.GetCell(3, j).ToString()))
                {
                    depth++;
                    split = this.Excel.GetCell(3, j).ToString().Split(' ');
                    xmlWriter.WriteStartElement(split[0]);
                    if (split.Length > 1)
                    {
                        for (int c = 1; c < split.Length; c++)
                        {
                            var attribute = split[c];
                            while (attribute.Split('"').Count() % 2 == 0)
                            {
                                c++;
                                attribute += " " + split[c];
                            }
                            xmlWriter.WriteAttributeString(attribute.Split('=')[0],
                                attribute.Split('=')[1].Substring(1).TrimEnd('"'));
                        }
                    }
                }
            }

            string text = this.Excel.GetCell(3, translatedColumn).ToString();
            if (string.IsNullOrWhiteSpace(text))
                text = this.Excel.GetCell(3, translatedColumn - 1).ToString();
            text = this.Manip.FormatString(text);

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains("\n"))
                {
                    text = "\n" + text + "\n";

                    //////////////////////////////////////////////
                    text = text.Replace("\n", "\n" + new string(' ', (depth + 1) * 2));
                    text = text.Remove(text.Length - 2);
                    xmlWriter.WriteValue(text);
                }
                else
                {
                    xmlWriter.WriteValue(text);
                }
            }

            xmlWriter.WriteEndElement();
            depth--;

            for (int i = 4; i <= rows; i++)
            {
                for (int j = 1; j < translatedColumn - 1; j++)
                {
                    if (!string.IsNullOrEmpty(this.Excel.GetCell(i, j).ToString()))
                    {
                        depth++;
                        while (depth > j)
                        {
                            xmlWriter.WriteEndElement();
                            depth--;
                        }
                        split = this.Excel.GetCell(i, j).ToString().Split(' ');
                        xmlWriter.WriteStartElement(split[0]);
                        if (split.Length > 1)
                        {
                            for (int c = 1; c < split.Length; c++)
                            {
                                if (split[c].Split('\"').Length == 2)
                                {
                                    xmlWriter.WriteAttributeString(split[c].Split('=')[0],
                                        (split[c].Split('=')[1] + split[c + 1]).Substring(1).TrimEnd('"'));
                                    c++;
                                }
                                else
                                {
                                    xmlWriter.WriteAttributeString(split[c].Split('=')[0],
                                        split[c].Split('=')[1].Substring(1).TrimEnd('"'));
                                }
                            }
                        }
                    }
                }

                text = this.Excel.GetCell(i, translatedColumn).ToString();
                if (string.IsNullOrWhiteSpace(text))
                    text = this.Excel.GetCell(i, translatedColumn - 1).ToString();
                text = this.Manip.FormatString(text);

                if (!string.IsNullOrEmpty(text) && text.Contains("\n"))
                {
                    text = "\n" + text + "\n";           
                    /////////////////////////////////////////////////////////
                    text = text.Replace("\n", "\n" + new string(' ', (depth + 1) * 2));
                    text = text.Remove(text.Length - 2);
                    xmlWriter.WriteValue(text);
                }
                else
                {
                    xmlWriter.WriteValue(text);
                }

                xmlWriter.WriteEndElement();
                depth--;
            }
            while (depth > 0)
            {
                xmlWriter.WriteEndElement();
                depth--;
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public void Reset()
        {
            tabs = 0;
            blocks = 3;
            maxDepth = 0;
            depth = 0;
            counter = 0;
            converted = "";
            ssEntry = new List<string[]>();
        }

        public string AddAttributes(XElement xel)
        {
            string content = "\t";
            XAttribute xat;
            if (xel.HasAttributes)
            {
                content = " ";
                xat = xel.FirstAttribute;
                content += xat.ToString();
                while (xat.NextAttribute != null)
                {
                    content += " ";
                    xat = xat.NextAttribute;
                    content += xat.ToString();
                }
                content += "\t";
            }
            tabs++;
            return content;
        }

        public string CheckDepth(XElement xel)
        {
            string returnTabs = "";
            int checkDepth = 0;
            while (xel.FirstNode != null && xel.FirstNode.NodeType != XmlNodeType.Text)
            {
                xel = (XElement)xel.FirstNode;
                checkDepth++;
            }
            for (int num = maxDepth - (depth + checkDepth); num > 0; num--)
            {
                returnTabs += "\t";
                tabs++;
            }
            return returnTabs;
        }

        public string FixValue(XElement xel)
        {
            string value = "", addTabs = "", preTabs = "";
            string[] valueFix;

            tabs++;
            while (tabs <= blocks - 2)
            {
                preTabs += "\t";
                tabs++;
            }

            if (xel.Value != String.Empty)
            {
                valueFix = xel.Value.Split('\n');
                if (valueFix.Length > 1)
                {
                    for (int num = 1; num < valueFix.Length - 1; num++)
                    {
                        if (num > 1)
                            value += "\n";
                        string fixit = valueFix[num];
                        while (fixit[0] == '\t')
                            fixit.Replace('\t', ' ');
                        string[] str = fixit.Split(' ');
                        for (int i = 0; i < str.Length; i++)
                            if (str[i] != string.Empty)
                                value += str[i];
                    }
                    for (int num = tabs; num < blocks; num++)
                        addTabs += "\t";
                    tabs = 0;
                    return preTabs + value + addTabs + "\t\t";
                }
                else
                {
                    for (int num = tabs; num < blocks; num++)
                        addTabs += "\t";
                    tabs = 0;
                    return preTabs + xel.Value + addTabs + "\t\t";
                }
            }
            else
            {
                for (int num = tabs; num < blocks; num++)
                    addTabs += "\t";
                tabs = 0;
                return preTabs + xel.Value + addTabs + "\t\t";
            }
        }

        public void Convert(XElement xel, bool called = false)
        {
            counter++;
            bool found = false;
            int offset = 0;
            string content = "", line = "";

            line += xel.Name + AddAttributes(xel);
            if (!called)
            {
                while (!found)
                {
                    offset = 0;
                    while (nodesText[counter][offset] == ' ' && nodesText[counter][offset + 1] == ' ')
                        offset += 2;
                    if (xel.Name.ToString().Length > nodesText[counter].Length)
                        counter++;
                    else
                        if (offset + 1 + xel.Name.ToString().Length < nodesText[counter].Length)
                            if (nodesText[counter].Substring(offset + 1, xel.Name.ToString().Length) != xel.Name.ToString())
                                counter++;
                            else
                                found = true;
                        else
                            counter++;
                }

                for (int num = 2; nodesText[counter][num] == ' ' && nodesText[counter][num + 1] == ' '; num += 2)
                {
                    line = "\t" + line;
                    tabs++;
                }
            }

            content += line;
            line = "";
            if (xel.FirstNode != null && xel.FirstNode.NodeType != XmlNodeType.Text)
            {
                xel = (XElement)xel.FirstNode;
                depth++;
                if (converted == "" || called)
                    converted += content;
                else
                    converted += "\v" + content;
                    converted += content;
                Convert(xel, true);

                if (xel.NextNode != null)
                {
                    XElement xel2 = (XElement)xel.NextNode;
                    Convert(xel2);
                    while (xel2.NextNode != null)
                    {
                        xel2 = (XElement)xel2.NextNode;
                        Convert(xel2, false);
                    }
                }

                xel = xel.Parent;
                depth--;
            }
            else
            {
                content += FixValue(xel);
                content += (xel);
                if (converted == "" || called)
                    converted += content;
                else
                    converted += "\v" + content;
                    converted += content;
                content = "";
            }
        }
    }
}