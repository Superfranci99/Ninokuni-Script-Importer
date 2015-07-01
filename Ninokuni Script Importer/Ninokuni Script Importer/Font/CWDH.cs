using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    class CWDH : NitroFile
    {
        public override string Name { get { return "CWDH"; } }

        public byte[]       MagicID    { get; set; }
        public uint         BlockSize  { get; set; }
        public ushort       FirstChar  { get; set; }
        public ushort       LastChar   { get; set; }
        public uint         NextRegion { get; set; }
        public GlyphWidth[] Widths     { get; set; }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            long pos = br.BaseStream.Position;

            this.MagicID    = br.ReadBytes(4);
            this.BlockSize  = br.ReadUInt32();
            this.FirstChar  = br.ReadUInt16();
            this.LastChar   = br.ReadUInt16();
            this.NextRegion = br.ReadUInt32();

            if (this.NextRegion != 0)
                throw new NotImplementedException("Multiple CWDH regions are not supported!");

            int numWidths = this.LastChar - this.FirstChar + 1;
            this.Widths = new GlyphWidth[numWidths];

            for (int i = 0; i < numWidths; i++)
                this.Widths[i] = GlyphWidth.FromStream(stream);

            br.BaseStream.Seek(pos + this.BlockSize, SeekOrigin.Begin);
        }
    }
}
