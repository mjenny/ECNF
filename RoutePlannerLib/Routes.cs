
using System;
using System.IO;
using System.Collections.Generic;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    /// <summary>
    /// Manages a routes from a city to another city.
    /// </summary>
    public class Routes
    {
        List<Link> routes = new List<Link>();
        Cities cities;

        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        public event RouteRequestHandler RouteRequestEvent;

        public int Count
        {
            get { return routes.Count; }
        }

        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public Routes(Cities cities)
        {
            this.cities = cities;
        }

        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links where the cities exist.
        /// </summary>
        /// <param name="filename">name of links file</param>
        /// <returns>number of read route</returns>
        public int ReadRoutes(string filename)
        {
            var count = 0;
            using (TextReader reader = new StreamReader(filename))
            {
                foreach (var item in reader.GetSplittedLines('\t'))
                {
                    City city1 = cities.FindCity(item[0]);
                    City city2 = cities.FindCity(item[1]);

                    // only add links, where the cities are found 
                    if ((city1 != null) && (city2 != null))
                    {
                        routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location),
                                                   TransportModes.Rail));
                        count++;
                    }
                }
            }

            return count;

        }

        /// <summary>
        /// Find all cities between two cities
        /// </summary>
        /// <param name="fromCity">Start city as string</param>
        /// <param name="toCity">End city as string</param>
        /// <returns></returns>
        public List<City> FindCitiesBetween(string fromCity, string toCity)
        {
            var fc = cities.FindCity(fromCity);
            var tc = cities.FindCity(toCity);
            return cities.FindCitiesBetween(fc, tc);
        }

        public Link FindLink(City c1, City c2, TransportModes mode)
        {
            return routes.Find(delegate(Link lnk)
            {
                return (lnk.TransportMode == mode && ((lnk.FromCity == c1 && lnk.ToCity == c2) || (lnk.FromCity == c2 && lnk.ToCity == c1)));
            });
        }

        public List<Link> FindPath(List<City> citiesOnRoute, TransportModes mode)
        {
            var ret = new List<Link>();
            for (int i = 0; i < citiesOnRoute.Count - 1; i++)
            {
                var city1 = citiesOnRoute[i];
                var city2 = citiesOnRoute[i+1];
                ret.Add(new Link(city1, city2, city1.Location.Distance(city2.Location),mode));
            }
            return ret;
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode)
        {
            if (RouteRequestEvent != null)
                RouteRequestEvent(this, new RouteRequestEventArgs(new City(fromCity, "", 0, 0.0, 0.0), new City(toCity, "", 0, 0.0, 0.0), mode));
            var citiesBetween = FindCitiesBetween(fromCity, toCity);
            if (citiesBetween == null || citiesBetween.Count < 1 || routes == null || routes.Count < 1)
                return null;

            var source = citiesBetween[0];
            var target = citiesBetween[citiesBetween.Count - 1];

            Dictionary<City, double> dist;
            Dictionary<City, City> previous;
            var q = FillListOfNodes(citiesBetween, out dist, out previous);
            dist[source] = 0.0;

            // the actual algorithm
            previous = SearchShortestPath(mode, q, dist, previous);

            // create a list with all cities on the route
            var citiesOnRoute = GetCitiesOnRoute(source, target, previous);

            // prepare final list if links
            return FindPath(citiesOnRoute, mode);
        }

        private static List<City> FillListOfNodes(List<City> cities, out Dictionary<City, double> dist, out Dictionary<City, City> previous)
        {
            var q = new List<City>(); // the set of all nodes (cities) in Graph ;
            dist = new Dictionary<City, double>();
            previous = new Dictionary<City, City>();
            foreach (var v in cities)
            {
                dist[v] = double.MaxValue;
                previous[v] = null;
                q.Add(v);
            }

            return q;
        }

        /// <summary>
        /// Searches the shortest path for cities and the given links
        /// </summary>
        /// <param name="mode">transportation mode</param>
        /// <param name="q"></param>
        /// <param name="dist"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        private Dictionary<City, City> SearchShortestPath(TransportModes mode, List<City> q, Dictionary<City, double> dist, Dictionary<City, City> previous)
        {
            while (q.Count > 0)
            {
                City u = null;
                var minDist = double.MaxValue;
                // find city u with smallest dist
                foreach (var c in q)
                    if (dist[c] < minDist)
                    {
                        u = c;
                        minDist = dist[c];
                    }

                if (u != null)
                {
                    q.Remove(u);
                    foreach (var n in FindNeighbours(u, mode))
                    {
                        var l = FindLink(u, n, mode);
                        var d = dist[u];
                        if (l != null)
                            d += l.Distance;
                        else
                            d += double.MaxValue;

                        if (dist.ContainsKey(n) && d < dist[n])
                        {
                            dist[n] = d;
                            previous[n] = u;
                        }
                    }
                }
                else
                    break;

            }

            return previous;
        }


        /// <summary>
        /// Finds all neighbor cities of a city. 
        /// </summary>
        /// <param name="city">source city</param>
        /// <param name="mode">transportation mode</param>
        /// <returns>list of neighbor cities</returns>
        private List<City> FindNeighbours(City city, TransportModes mode)
        {
            var neighbors = new List<City>();
            foreach (var r in routes)
                if (mode.Equals(r.TransportMode))
                {
                    if (city.Equals(r.FromCity))
                        neighbors.Add(r.ToCity);
                    else if (city.Equals(r.ToCity))
                        neighbors.Add(r.FromCity);
                }

            return neighbors;
        }

        private List<City> GetCitiesOnRoute(City source, City target, Dictionary<City, City> previous)
        {
            var citiesOnRoute = new List<City>();
            var cr = target;
            while (previous[cr] != null)
            {
                citiesOnRoute.Add(cr);
                cr = previous[cr];
            }
            citiesOnRoute.Add(source);

            citiesOnRoute.Reverse();
            return citiesOnRoute;
        }
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
