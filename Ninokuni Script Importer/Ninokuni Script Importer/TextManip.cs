using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninokuni_Script_Importer.Font;

namespace Ninokuni_Script_Importer
{
    class TextManip
    {
        public NFTR Font { get; set; }
        public readonly int MaxLength;

        public TextManip(NFTR font, int maxLength)
        {
            this.Font = font;
            this.MaxLength = maxLength;
        }

        public string FormatString(string data)
        {
            data = FixAccents(data);

            if (IsFormatted(data))
                return data;

            data = data.Replace("\r\n", " ").Replace("\n", " ");
            string[] lines = data.Split(' ');

            int counter = 0;
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                if ((Font.GetStringLength(line.Replace("{3:4}", "").Replace("{1:2}", "")) + counter) > this.MaxLength)
                {
                    sb.Append("\r\n");
                    counter = 0;
                }

                counter += Font.GetStringLength(line + " ");
                sb.Append(line + " ");
            }

            return sb.ToString();
        }

        public bool IsFormatted(string data)
        {
            if (Font.GetStringLength(data.Replace("\r\n", "").Replace("\n", "").Replace("{3:4}", "").Replace("{1:2}","")) <= this.MaxLength)
                return true;

            string[] lines = FixAccents(data.Replace("\r\n", "\n")).Split('\n');
            foreach (string line in lines)
                if (Font.GetStringLength(line.Replace("{3:4}", "").Replace("{1:2}", "")) > this.MaxLength)
                    return false;

            return true;
        }

        public static string FixAccents(string data)
        {
            string result = "";

            foreach (char c in data)
            {
                string newChar = c.ToString();
                switch (c)
                {
                    case 'à': newChar = ((char)166).ToString(); break;
                    case 'è': newChar = ((char)167).ToString(); break;
                    case 'é': newChar = ((char)168).ToString(); break;
                    case 'ì': newChar = ((char)169).ToString(); break;
                    case 'ò': newChar = ((char)170).ToString(); break;
                    case 'ù': newChar = ((char)171).ToString(); break;
                    case 'È': newChar = "E'"; break;
                    case '…': newChar = "..."; break;
                }
                result += newChar;
            }

            return result;
        }
    }
}
