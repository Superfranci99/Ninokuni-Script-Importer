using System;
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

        public override void Read(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
