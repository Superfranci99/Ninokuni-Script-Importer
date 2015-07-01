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
        public FINF Finf { get; set; }
        public CGLP Cglp { get; set; }
        public CWDH Cwdh { get; set; }
        public CMAP Cmap { get; set; }

        public byte[] MagicId   { get; set; }
        public ushort Endianess { get; set; }
        public ushort Version   { get; set; }
        public uint   FileSize  { get; set; }
        public ushort BlockSize { get; set; }
        public ushort NumBlocks { get; set; }

        public override string Name { get { return "NFTR"; } }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);

            this.MagicId   = br.ReadBytes(4);
            this.Endianess = br.ReadUInt16();
            this.Version   = br.ReadUInt16();
            this.FileSize  = br.ReadUInt32();
            this.BlockSize = br.ReadUInt16();
            this.NumBlocks = br.ReadUInt16();

            this.Finf = new FINF();
            this.Finf.Read(stream);

            this.Cglp = new CGLP();
            this.Cglp.Read(stream);

            this.Cwdh = new CWDH();
            this.Cwdh.Read(stream);

            this.Cmap = new CMAP();
            this.Cmap.Read(stream);
        }

        public int GetTextLength(string data)
        {
            throw new NotImplementedException();
        }
    }
}
