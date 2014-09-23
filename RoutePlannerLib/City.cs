using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    class City
    {
        string _name;
        string _country;
        int _population;
        WayPoint _location;
        public City(string Name, string Country, int Population, double Latitude, double Longitude)
        {
            _name = Name;
            _country = Country;
            _population = Population;
            _location = new WayPoint(_name, Latitude, Longitude);
        }
    }
}
