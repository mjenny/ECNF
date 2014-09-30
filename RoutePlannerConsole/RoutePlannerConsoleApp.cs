using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RoutePlanner (" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")");
            Console.WriteLine();

            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);

            var wpBern = new WayPoint("Bern", 46.9479222, 7.4446085);
            var wpTripolis = new WayPoint("Tripolis", 32.8084124, 13.1509672);

            Console.WriteLine(wayPoint.ToString());

            Console.WriteLine(wpBern.ToString());
            Console.WriteLine(wpTripolis.ToString());

            Console.WriteLine("Distance: {0}" , wpBern.Distance(wpTripolis));

            var citiesLab2 = new Cities();
            citiesLab2.ReadCities("data\\citiesTestDataLab2.txt");

            var bern = new WayPoint("Bern", 46.95, 7.44);
            var tripolis = new WayPoint("Tripolis", 32.876174, 13.187507);
            const double expected = 1638.74410788167;
            double actual = bern.Distance(tripolis);
            
            Console.WriteLine();
            var findCity = citiesLab2.FindCity("Bern");

            if (findCity != null)
                Console.WriteLine("City {0} was found.", findCity.Name);
            else
                Console.WriteLine("City not found.");

            findCity = citiesLab2.FindCity("Dilli");

            if (findCity != null)
                Console.WriteLine("City {0} was found.", findCity.Name);
            else
                Console.WriteLine("City not found.");

            Console.WriteLine();
            Console.WriteLine("Test Routes");
            var citiesLab3 = new Cities();
            citiesLab3.ReadCities("data\\citiesTestDataLab3.txt");
            var reqWatcher = new RouteRequestWatcher();

            var routes = new Routes(citiesLab3);
            routes.RouteRequestEvent += reqWatcher.LogRouteRequests;
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Bus);
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Bus);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
