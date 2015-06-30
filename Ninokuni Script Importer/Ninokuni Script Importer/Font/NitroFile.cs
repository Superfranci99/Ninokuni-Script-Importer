using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ninokuni_Script_Importer.Font
{
    abstract class NitroFile
    {
        public abstract string Name { get; }

        public abstract void Read(Stream stream);
    }
}
