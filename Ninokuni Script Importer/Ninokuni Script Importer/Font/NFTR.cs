using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    class NFTR : NitroFile
    {
        public byte[] MagicId   { get; set; }
        public ushort Endianess { get; set; }
        public ushort Version   { get; set; }
        public uint   FileSize  { get; set; }
        public ushort NumBlocks { get; set; }

        public NFTR()
        {
            // nothing to do...
        }

        public override string Name { get { return "NFTR"; } }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            this.MagicId   = br.ReadBytes(4);
            this.Endianess = br.ReadUInt16();
            this.Version   = br.ReadUInt16();
            this.FileSize  = br.ReadUInt32();
            this.NumBlocks = br.ReadUInt16();
        }

        public int GetTextLength(string data)
        {
            throw new NotImplementedException();
        }
    }
}
