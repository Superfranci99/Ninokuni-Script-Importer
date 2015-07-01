using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    class CMAP : NitroFile
    {
        public byte[] MagicID { get; set; }
        public int   Size    { get; set; }

        private ushort firstChar;
		private ushort lastChar;
		private uint   type;

		#region Properties

		public int Id {
			get;
			set;
		}

		public ushort FirstChar {
			get { return this.firstChar; }
			set { 
				this.firstChar = value;
				if (this.Type == 1) {
					this.Size = 8 + 0xC + 2 * (this.lastChar - this.firstChar + 1);
				}
			}
		}

		public ushort LastChar {
			get { return this.lastChar; }
			set {
				this.lastChar = value;
				if (this.Type == 1) {
					this.Size = 8 + 0xC + 2 * (this.lastChar - this.firstChar + 1);
				}
			}
		}

		public uint Type {
			get { return this.type; }
			set { 
				this.type = value;
				if (this.Type == 1) {
					this.Size = 8 + 0xC + 2 * (this.lastChar - this.firstChar + 1);
				}
			}
		}

		public uint NextCmap {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the map.
		/// The first column contains the char codes and in the second
		/// the glyph index.
		/// </summary>
		/// <value>The map.</value>
		public int[,] Map {
			get;
			set;
		}

		#endregion

		#region implemented abstract members of NitroBlock

		public override void Read(Stream strIn)
		{
			BinaryReader br = new BinaryReader(strIn);

            this.MagicID = br.ReadBytes(4);
            this.Size = br.ReadInt32();
			this.FirstChar = br.ReadUInt16();
			this.LastChar = br.ReadUInt16();
			this.Type = br.ReadUInt32();
			this.NextCmap = br.ReadUInt32();

			// Read map
			int numEntries = this.LastChar - this.FirstChar + 1;
			this.Map = new int[numEntries, 2];

			switch (this.Type) {
			case 0:
				ushort firstGlyph = br.ReadUInt16();
				for (int i = 0; i < numEntries; i++) {
					this.Map[i, 0] = this.FirstChar + i;
					this.Map[i, 1] = firstGlyph + i;
				}
				break;

			case 1:
				List<Tuple<int, int>> map = new List<Tuple<int, int>>();
				for (int i = 0; i < numEntries; i++) {
					ushort gidx = br.ReadUInt16();
					if (gidx != 0xFFFF)
						map.Add(Tuple.Create<int, int>(this.FirstChar + i, gidx));
				}

				this.Map = new int[map.Count, 2];
				for (int i = 0; i < this.Map.GetLength(0); i++) {
					this.Map[i, 0] = map[i].Item1;
					this.Map[i, 1] = map[i].Item2;
				}

				break;

			case 2:
				numEntries = br.ReadUInt16();
				this.Map = new int[numEntries, 2];
				for (int i = 0; i < numEntries; i++) {
					this.Map[i, 0] = br.ReadUInt16();
					this.Map[i, 1] = br.ReadUInt16();
				}
				break;
			}

			br = null;
		}

		public override string Name {
			get { return "CMAP"; }
		}

		#endregion

		public bool Contains(int imgIndex)
		{
			for (int i = 0; i < this.Map.GetLength(0); i++) {
				if (this.Map[i, 1] == imgIndex)
					return true;
			}

			return false;
		}

		public bool Contains(ushort charCode)
		{
			if (charCode < this.FirstChar || charCode > this.LastChar)
				return false;

			for (int i = 0; i < this.Map.GetLength(0); i++) {
				if (this.Map[i, 0] == charCode)
					return true;
			}

			return false;
		}

		public int GetCharCode(int imgIndex)
		{
			for (int i = 0; i < this.Map.GetLength(0); i++) {
				if (this.Map[i, 1] == imgIndex)
					return this.Map[i, 0];
			}

			return -1;
		}



    }
}
