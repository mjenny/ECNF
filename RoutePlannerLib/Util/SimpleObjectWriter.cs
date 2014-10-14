using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        private System.IO.StringWriter stream;

        public SimpleObjectWriter(System.IO.StringWriter stream)
        {
            // TODO: Complete member initialization
            this.stream = stream;
        }

        public void Next(City c)
        {
            throw new NotImplementedException();
        }
    }
}
