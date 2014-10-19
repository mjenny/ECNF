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

        //Default Constructor for serialization
        public WayPoint() { }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public double Distance(WayPoint target) {

            double radianToDegree = (Math.PI / 180);
            double tmp1 = Math.Sin(radianToDegree * Latitude) * Math.Sin(radianToDegree * target.Latitude);
            double tmp2 = Math.Cos(radianToDegree * Latitude) * Math.Cos(radianToDegree * target.Latitude);
            double tmp3 = Math.Cos(radianToDegree * (Longitude - target.Longitude));
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


        /// <summary>
        /// Summiert zwei Waypoints
        /// Die Operatoren sollen so funktionieren, 
        /// dass die Koordinaten addiert bzw. subtrahiert werden
        /// und der Name des ersten (linken) WayPoints erhalten bleibt. 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>new WayPoint</returns>
        public static WayPoint operator+ (WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude + rhs.Latitude, lhs.Longitude + rhs.Longitude);
        }

        /// <summary>
        /// Subtrahiert zwei Waypoints
        /// Die Operatoren sollen so funktionieren, 
        /// dass die Koordinaten addiert bzw. subtrahiert werden
        /// und der Name des ersten (linken) WayPoints erhalten bleibt. 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>new WayPoint</returns>
        public static WayPoint operator- (WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude - rhs.Latitude, lhs.Longitude - rhs.Longitude);
        }
    
    }
}
