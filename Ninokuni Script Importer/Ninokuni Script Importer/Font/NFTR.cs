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
        public List<CMAP> Cmaps { get; set; }

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

            int i = 0;
            this.Cmaps = new List<CMAP>();
            while (stream.Position < stream.Length)
            {
                this.Cmaps.Add(new CMAP());
                this.Cmaps[i++].Read(stream);
            }       
        }

        public int GetStringLength(string data)
        {
            int result = 0;
            foreach (char c in data)
                result += GetCharLength(c);
            return result;
        }

        public int GetCharLength(char data)
        {
            foreach (CMAP cmap in this.Cmaps)
            {
                if(cmap.Contains(data))
                    return this.Cwdh.Widths[cmap.Map[cmap.GetIndexCode(data), 1]].Advance;
            }
            return 12;
        }

    }
}
