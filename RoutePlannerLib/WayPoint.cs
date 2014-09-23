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
        const int RADIUS = 6371;

        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public double Distance(WayPoint target) {

            double tmp1 = Math.Sin(Latitude) * Math.Sin(target.Latitude);
            double tmp2 = Math.Cos(Latitude) * Math.Cos(target.Latitude);
            double tmp3 = Math.Cos(target.Longitude - Longitude);
            double tmp4 = tmp2 * tmp3;

            double d = 0;
            d = RADIUS * Math.Acos(tmp1 + tmp4);
            return d;
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
