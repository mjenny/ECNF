using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW
{
    public class RoutesDijkstra : Routes
    {

        //Kann abstract bleiben
        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        //Kann abstract bleiben
        public event RouteRequestHandler RouteRequestEvent;


        public RoutesDijkstra(Cities cities)
            : base(cities)
        {
           
        }

        public Task<List<Link>> GoFindShortestRouteBetween(string fromCity, string toCity, TransportModes mode, IProgress<String> progress = null)
        {
     
            
            return Task.Run(() => FindShortestRouteBetween(fromCity,toCity,mode, progress));
        }

        public override List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode)
        {
            return FindShortestRouteBetween(fromCity, toCity, mode, null);
        }
 
        private List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode, IProgress<String> progress = null)
        {
            if (progress != null)progress.Report("Start done");
            if (RouteRequestEvent != null)
                RouteRequestEvent(this, new RouteRequestEventArgs(new City(fromCity, "", 0, 0.0, 0.0), new City(toCity, "", 0, 0.0, 0.0), mode));
            var citiesBetween = FindCitiesBetween(fromCity, toCity);
            if (citiesBetween == null || citiesBetween.Count < 1 || _routes == null || _routes.Count < 1)
                return null;

            if (progress != null) progress.Report("Set origin done");
            var source = citiesBetween[0];
            if (progress != null) progress.Report("Set destination done");
            var target = citiesBetween[citiesBetween.Count - 1];

            if (progress != null) progress.Report("Fill list of nodes done");
            Dictionary<City, double> dist;
            Dictionary<City, City> previous;
            var q = FillListOfNodes(citiesBetween, out dist, out previous);
            dist[source] = 0.0;

            // the actual algorithm
            if (progress != null) progress.Report("Search shortest path done");
            previous = SearchShortestPath(mode, q, dist, previous);

            // create a list with all cities on the route
            if (progress != null) progress.Report("Get cities on route done");
            var citiesOnRoute = GetCitiesOnRoute(source, target, previous);

            // prepare final list if links
            if (progress != null) progress.Report("FindShortestRouteBetween done");

            return FindPath(citiesOnRoute, mode);
        }

        private Link FindLink(City c1, City c2, TransportModes mode)
        {
            return _routes.Find(delegate(Link lnk)
            {
                return (lnk.TransportMode == mode && ((lnk.FromCity == c1 && lnk.ToCity == c2) || (lnk.FromCity == c2 && lnk.ToCity == c1)));
            });
        }
        private List<Link> FindPath(List<City> citiesOnRoute, TransportModes mode)
        {
            var ret = new List<Link>();
            for (int i = 0; i < citiesOnRoute.Count - 1; i++)
            {
                var city1 = citiesOnRoute[i];
                var city2 = citiesOnRoute[i + 1];
                ret.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), mode));
            }
            return ret;
        }

        private List<City> FillListOfNodes(List<City> cities, out Dictionary<City, double> dist, out Dictionary<City, City> previous)
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
}
