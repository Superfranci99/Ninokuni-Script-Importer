using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    struct GlyphWidth
    {
        public byte BearingX { get; set; }
        public byte Width    { get; set; }
        public byte Advance  { get; set; }

        public static GlyphWidth FromStream(Stream stream)
        {
            GlyphWidth gw = new GlyphWidth();
            gw.BearingX = (byte)stream.ReadByte();
            gw.Width = (byte)stream.ReadByte();
            gw.Advance = (byte)stream.ReadByte();
            return gw;
        }
    }
}
