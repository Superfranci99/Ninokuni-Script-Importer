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
            if (IsFormatted(data))
                return data;

            string[] lines = data.Replace("\r\n", " ").Replace("\n", " ").Split(' ');

            int counter = 0;
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                if ((Font.GetStringLength(line) + counter) > this.MaxLength)
                {
                    sb.AppendLine();
                    counter = 0;
                }

                counter += Font.GetStringLength(line);
                sb.Append(line);
            }

            return sb.ToString();
        }

        public bool IsFormatted(string data)
        {
            if (Font.GetStringLength(data) > this.MaxLength)
                return true;

            string[] lines = data.Replace("\r\n", "\r").Split('r');
            foreach (string line in lines)
                if (Font.GetStringLength(line) > this.MaxLength)
                    return false;

            return true;
        }
    }
}
