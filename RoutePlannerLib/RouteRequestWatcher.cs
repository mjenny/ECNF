using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW
{
    public class RouteRequestWatcher
    {
        readonly Dictionary<string, int> _requests;

        /// <summary>
        /// Constructor
        /// </summary>
        public RouteRequestWatcher()
        {
            _requests = new Dictionary<string, int>();
        }

        /// <summary>
        /// Event Handler for the Route Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            if (_requests.ContainsKey(e.ToCity.Name))
                _requests[e.ToCity.Name] += 1;
            else
                _requests[e.ToCity.Name] = 1;
            Console.WriteLine("ToCity: {0} has been requested {1} times", e.ToCity.Name, _requests[e.ToCity.Name]);
        }

        /// <summary>
        /// Returns the count of requests for a given city.
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public int GetCityRequests(string cityName)
        {
            return (_requests.ContainsKey(cityName)) ? _requests[cityName] : 0;
        }
    }
}
