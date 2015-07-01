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

        public byte[] MagicID     { get; set; }
        public uint   BlockSize   { get; set; }
        public byte   BoxWidth    { get; set; }
        public byte   BoxHeight   { get; set; }
        public ushort BoxByteSize { get; set; }
        public byte   GlyphHeight { get; set; }
        public byte   GlyphWidth  { get; set; }
        public byte   Depth       { get; set; }
        public byte   Rotation    { get; set; }

        public override void Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            long pos = br.BaseStream.Position;

            this.MagicID     = br.ReadBytes(4);
            this.BlockSize   = br.ReadUInt32();
            this.BoxWidth    = br.ReadByte();
            this.BoxHeight   = br.ReadByte();
            this.BoxByteSize = br.ReadUInt16();
            this.GlyphHeight = br.ReadByte();
            this.GlyphWidth  = br.ReadByte();
            this.Depth       = br.ReadByte();
            this.Rotation    = br.ReadByte();

            // skip tile data
            br.BaseStream.Seek(pos + this.BlockSize, SeekOrigin.Begin);
        }
    }
}
