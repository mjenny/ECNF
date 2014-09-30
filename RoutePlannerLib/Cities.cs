using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        readonly List<City> _cities = new List<City>();
        public int Count { get { return _cities.Count; } }

        /// <summary>
        /// Read cities from text file
        /// One entry per line, seperated with a tab (name, country, population, latitude, longitude)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int ReadCities(string filename)
        {
            var readCounter = 0;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = line.Split('\t');
                        _cities.Add(new City(data[0], data[1], Int32.Parse(data[2]), Double.Parse(data[3], CultureInfo.InvariantCulture), Double.Parse(data[4], CultureInfo.InvariantCulture)));
                        ++readCounter;
                    }
                }
                return readCounter;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file \"" + filename + "\" could not be read!");
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Find all neighbours of a city within the defined distance.
        /// The list is sorted by distance 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            var neighbours = new SortedDictionary<double, City>();
            foreach (var c in _cities)
            {
                var currentDistance = location.Distance(c.Location);
                if (currentDistance <= distance)
                {
                    neighbours.Add(currentDistance, c);
                }
            }
            return neighbours.Values.ToList();
        }

        /// <summary>
        /// Iterator for read cities
        /// </summary>
        /// <param name="index">Index of the city to select</param>
        /// <returns>City with defined index</returns>
        public City this[int index]
        {
            get { return (index < _cities.Count) ? _cities[index] : null; }
        }

    }
}
