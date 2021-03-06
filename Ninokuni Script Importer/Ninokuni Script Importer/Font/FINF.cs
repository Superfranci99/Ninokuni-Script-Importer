﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    class FINF : NitroFile
    {
        public override string Name { get { return "FINF"; } }

        public byte[]     MagicID          { get; set; }
        public uint       BlockSize        { get; set; }
        public byte       Unknown          { get; set; }
        public byte       LineGap          { get; set; }
        public ushort     ErrorCharIndex   { get; set; }
        public GlyphWidth DefaultWidth     { get; set; }
        public byte       Encoding         { get; set; }
        public uint       OffsetCglp       { get; set; }
        public uint       OffsetCwdh       { get; set; }
        public uint       OffsetCmap       { get; set; }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            long pos = br.BaseStream.Position;

            this.MagicID        = br.ReadBytes(4);
            this.BlockSize      = br.ReadUInt32();
            this.Unknown        = br.ReadByte();
            this.LineGap        = br.ReadByte();
            this.ErrorCharIndex = br.ReadUInt16();
            this.DefaultWidth   = GlyphWidth.FromStream(stream);
            this.Encoding       = br.ReadByte();
            this.OffsetCglp     = br.ReadUInt32();
            this.OffsetCwdh     = br.ReadUInt32();
            this.OffsetCmap     = br.ReadUInt32();

            br.BaseStream.Seek(pos + this.BlockSize, SeekOrigin.Begin);
        }
    }
}
