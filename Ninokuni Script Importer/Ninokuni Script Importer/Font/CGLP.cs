using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    class CGLP : NitroFile 
    {
        public override string Name { get { return "CGLP"; } }

        public byte[] MagicID   { get; set; }
        public uint   BlockSize { get; set; }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);

            this.MagicID   = br.ReadBytes(4);
            this.BlockSize = br.ReadUInt32();

            // this block contains character glyph, this tool doesn't need to read this data...
            br.BaseStream.Seek(this.BlockSize - 4, SeekOrigin.Current);
        }
    }
}
