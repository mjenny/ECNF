

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
        /// <param name="name">name of the city</param>
        /// <param name="country">Country of the city</param>
        /// <param name="population">Population of the city</param>
        /// <param name="latitude">Latitude of the city</param>
        /// <param name="longitude">Longitude of the city</param>
        public City(string name, string country, int population, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(name, latitude, longitude);
        }
    }
}
