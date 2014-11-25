using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {

        private static readonly TraceSource _logger = new TraceSource("Cities");

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
            _logger.TraceEvent(TraceEventType.Information, 1, "ReadCities started");
            var readCounter = 0;
            try
            {
                using (TextReader tr = new StreamReader(filename))
                {

                    tr.GetSplittedLines('\t').ToList().ForEach(c => {
                        _cities.Add(new City(c[0].Trim(),
                                             c[1].Trim(),
                                             int.Parse(c[2]),
                                             double.Parse(c[3], CultureInfo.InvariantCulture),
                                             Double.Parse(c[4], CultureInfo.InvariantCulture)));
                        readCounter++;
                    
                    });
                }
                return readCounter;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file \"" + filename + "\" could not be read!");
                Console.WriteLine(e.Message);
                _logger.TraceEvent(TraceEventType.Critical, 1, e.StackTrace);
                return -1;
            }
            finally
            {
                _logger.TraceEvent(TraceEventType.Information, 1, "ReadCities ended");
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
            return (_cities
                .Where(c => location.Distance(c.Location) < distance)
                .OrderBy(d => location.Distance(d.Location)).ToList<City>());
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

        /// <summary>
        /// Find a city object with specified name
        /// </summary>
        /// <param name="cityName">Name of the city to find</param>
        /// <returns></returns>
        public City FindCity(string cityName)
        {
            return _cities.Find(c => c.Name.ToLower().Equals(cityName.ToLower(), StringComparison.InvariantCultureIgnoreCase));
        }

        #region Lab04: FindShortestPath helper function
        /// <summary>
        /// Find all cities between 2 cities 
        /// </summary>
        /// <param name="from">source city</param>
        /// <param name="to">target city</param>
        /// <returns>list of cities</returns>
        public List<City> FindCitiesBetween(City from, City to)
        {
            var foundCities = new List<City>();
            if (from == null || to == null)
                return foundCities;

            foundCities.Add(from);

            var minLat = Math.Min(from.Location.Latitude, to.Location.Latitude);
            var maxLat = Math.Max(from.Location.Latitude, to.Location.Latitude);
            var minLon = Math.Min(from.Location.Longitude, to.Location.Longitude);
            var maxLon = Math.Max(from.Location.Longitude, to.Location.Longitude);

            foundCities.AddRange(_cities.FindAll(c =>
                c.Location.Latitude > minLat && c.Location.Latitude < maxLat
                        && c.Location.Longitude > minLon && c.Location.Longitude < maxLon));

            foundCities.Add(to);
            return foundCities;
        }
        #endregion
    
        private List<City> InitIndexForAlgorithm(List<City> foundCities)
        {
            for(int index = 0; index < foundCities.Count; index++)
            {
                foundCities[index].Index = index;
            }
            return foundCities;
        }

    
    }
}
