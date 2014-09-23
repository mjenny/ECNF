using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {

        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public int Distance(WayPoint target) {

            return 0;
        }

        public override string ToString()
        {
            string s = "WayPoint: ";
            if (Name != null)
                s += Name + " ";
            s += Latitude.ToString("F") + "/" + Longitude.ToString("F");
            return s; 
        }
    }
}
