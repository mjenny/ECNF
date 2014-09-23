using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public string Name { get; private set; }
        public string Country { get; private set; }
        public int Population { get; private set; }
        public WayPoint Location { get; private set; }

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
            this.Name = Name;
            this.Country = Country;
            this.Population = Population;
            Location = new WayPoint(Name, Latitude, Longitude);
        }
    }
}
