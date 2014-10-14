using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectReader
    {
        private System.IO.StringReader stream;

        public SimpleObjectReader(System.IO.StringReader stream)
        {
            // TODO: Complete member initialization
            this.stream = stream;
        }

        public City Next()
        {
            throw new NotImplementedException();
        }
    }
}
