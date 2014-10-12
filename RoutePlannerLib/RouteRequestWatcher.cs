using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        readonly Dictionary<string, int> _requests;

        public RouteRequestWatcher()
        {
            _requests = new Dictionary<string, int>();
        }

        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            if (_requests.ContainsKey(e.ToCity.Name))
                _requests[e.ToCity.Name] += 1;
            else
                _requests[e.ToCity.Name] = 1;
            Console.WriteLine("ToCity: {0} has been requested {1} times", e.ToCity.Name, _requests[e.ToCity.Name]);
        }

        public int GetCityRequests(string cityName)
        {
            return (_requests.ContainsKey(cityName)) ? _requests[cityName] : 0;
        }
    }
}
