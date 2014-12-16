
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW.Util;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW
{
    /// <summary>
    /// Manages a routes from a city to another city.
    /// </summary>
    public abstract class Routes : IRoutes
    {
        private static readonly TraceSource _logger = new TraceSource("Routes");

        //Kann abstract bleiben
        protected List<Link> _routes = new List<Link>();
        //Kann abstract bleiben
        protected Cities _cities;

        public bool ExecuteParallel { get; set; }

        //Kann abstract bleiben
        public int Count
        {
            get { return _routes.Count; }
        }

        //Kann abstract bleiben
        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public Routes(Cities cities)
        {
            this._cities = cities;
        }

        //Kann abstract bleiben
        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links where the cities exist.
        /// </summary>
        /// <param name="filename">name of links file</param>
        /// <returns>number of read route</returns>
        public int ReadRoutes(string filename)
        {
            _logger.TraceEvent(TraceEventType.Information, 1, "ReadRoutes started");
 
            var count = 0;
            using (TextReader reader = new StreamReader(filename))
            {
                reader.GetSplittedLines('\t').ToList().ForEach(s =>
                {
                    City c1 = _cities.FindCity(s[0]);
                    City c2 = _cities.FindCity(s[1]);
                    if (c1 != null && c2 != null)
                    {
                        _routes.Add(new Link(c1, c2, c1.Location.Distance(c2.Location), TransportModes.Rail));
                        count++;
                    }
                });
            }

            _logger.TraceEvent(TraceEventType.Information, 1, "ReadRoutes ended");

            return count;

        }

        //Neu Protected
        /// <summary>
        /// Find all cities between two cities
        /// </summary>
        /// <param name="fromCity">Start city as string</param>
        /// <param name="toCity">End city as string</param>
        /// <returns></returns>
        protected List<City> FindCitiesBetween(string fromCity, string toCity)
        {
            var fc = _cities.FindCity(fromCity);
            var tc = _cities.FindCity(toCity);
            return _cities.FindCitiesBetween(fc, tc);
        }

        public abstract List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode);
   }

    public class RouteRequestEventArgs : EventArgs
    {
        public City FromCity { get; private set; }
        public City ToCity { get; private set; }
        public TransportModes Mode { get; private set; }
        
        public RouteRequestEventArgs(City fromCity, City toCity, TransportModes mode)
        {
            this.FromCity = fromCity;
            this.ToCity = toCity;
            this.Mode = mode;
        }
    }
}
