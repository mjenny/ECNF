
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    /// <summary>
    /// Manages a routes from a city to another city.
    /// </summary>
    public class Routes : IRoutes
    {
        List<Link> _routes = new List<Link>();
        Cities _cities;

        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        public event RouteRequestHandler RouteRequestEvent;

        public int Count
        {
            get { return _routes.Count; }
        }

        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public Routes(Cities cities)
        {
            this._cities = cities;
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
            var fc = _cities.FindCity(fromCity);
            var tc = _cities.FindCity(toCity);
            return _cities.FindCitiesBetween(fc, tc);
        }

        public Link FindLink(City c1, City c2, TransportModes mode)
        {
            return _routes.Find(delegate(Link lnk)
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
            if (citiesBetween == null || citiesBetween.Count < 1 || _routes == null || _routes.Count < 1)
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
            foreach (var r in _routes)
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

        public City[] FindCities(TransportModes transportModes)
        {
            var links = _routes.Where(p => p.TransportMode == transportModes).ToList();
            var fromCities = links.Select(c => c.FromCity).Distinct().ToList();
            var toCities = links.Select(d => d.ToCity).Distinct().ToList();
            fromCities.AddRange(toCities);
            return fromCities.Distinct().ToArray();
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
