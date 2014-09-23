using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    class City
    {
        public string name { get; private set; }
        public string country { get; private set; }
        public int population { get; private set; }
        public WayPoint location { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">Name of the city</param>
        /// <param name="Country">Country of the city</param>
        /// <param name="Population">Population of the city</param>
        /// <param name="Latitude">Latitude of the city</param>
        /// <param name="Longitude">Longitude of the city</param>
        public City(string Name, string Country, int Population, double Latitude, double Longitude)
        {
            this.name = Name;
            this.country = Country;
            this.population = Population;
            location = new WayPoint(Name, Latitude, Longitude);
        }
    }
}
