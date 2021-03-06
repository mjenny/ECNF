﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public class Lab5Test
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";

        private const string LinksTestFile = "linksTestDataLab4.txt";

        [TestMethod]
        public void TestLoadDynamicValid()
        {
            var cities = new Cities();
            // just test for correct dynamic creation of valid routed class from config
            IRoutes routes = RoutesFactory.Create(cities);
            Assert.IsInstanceOfType(routes, typeof(IRoutes));
            
            // now test for correct dynamic creation of valid routed class passed as string
            routes = RoutesFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW.RoutesDijkstra");
            Assert.IsInstanceOfType(routes, typeof(IRoutes));
             
        }
        
        [TestMethod]
        public void TestLoadDynamicInValid()
        {
            var cities = new Cities();
            // pass a name of a class that does not exist
            IRoutes routes = RoutesFactory.Create(cities, "Class.Does.Not.Exits");
            Assert.IsNull(routes);

            // pass a name of a class that exists, but does not implement the inetrface
            routes = RoutesFactory.Create(cities, "Cities");
            Assert.IsNull(routes);

        }
        
        [TestMethod]
        public void TestLoadAndRunDynamic()
        {

            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            IRoutes routes = RoutesFactory.Create(cities);

            routes.ReadRoutes(LinksTestFile);

            Assert.AreEqual(11, cities.Count);

            // test available cities
            List<Link> links = routes.FindShortestRouteBetween("Zürich", "Basel", TransportModes.Rail);

            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                                       new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Assert.IsNotNull(links);
            Assert.AreEqual(expectedLinks.Count, links.Count);

            for (int i = 0; i < links.Count; i++)
            {
                Assert.AreEqual(expectedLinks[i].FromCity.Name, links[i].FromCity.Name);
                Assert.AreEqual(expectedLinks[i].ToCity.Name, links[i].ToCity.Name);
            }

            links = routes.FindShortestRouteBetween("doesNotExist", "either", TransportModes.Rail);
            Assert.IsNull(links);
        }



    }
    
}

