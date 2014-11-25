using System;
using System.Collections.Generic;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public enum TransportModes { Ship, Rail, Flight, Car, Bus, Tram };

    /// <summary>
    /// Represents a link between two cities with its distance
    /// </summary>
    public class Link : IComparable
    {
        private City fromCity;
        private City toCity;
        double distance;

        TransportModes transportMode = TransportModes.Car;

        public TransportModes TransportMode
        {
            get { return transportMode; }
        }

        public City FromCity
        {
            get { return fromCity; }
        }

        public City ToCity
        {
            get { return toCity; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fromCity">Links departing city</param>
        /// <param name="toCity">Links arriving city</param>
        /// <param name="distance">distance between cities</param>
        public Link(City fromCity, City toCity, double distance)
        {
            this.fromCity = fromCity;
            this.toCity = toCity;
            this.distance = distance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fromCity">Links departing city</param>
        /// <param name="toCity">Links arriving city</param>
        /// <param name="distance">Distance between cities</param>
        /// <param name="transportMode">How link is served</param>
        public Link(City fromCity, City toCity, double distance, TransportModes transportMode)
            : this(fromCity, toCity, distance)
        {
            this.transportMode = transportMode;
        }

        public double Distance
        {
            get { return distance; }
        }

        /// <summary>
        /// Specifies "distance" as default compare criteria 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(object o)
        {
            var other = (Link)o;
            return (int)(1000 * (distance - other.distance));
        }

        internal bool IsIncluded(List<City> cities)
        {
            var foundFrom = false;
            var foundTo = false;
            foreach (var c in cities)
            {
                if (!foundFrom && c.Name == FromCity.Name)
                {
                    foundFrom = true;
                }
                if (!foundTo && c.Name == ToCity.Name)
                {
                    foundTo = true;
                }
                if (foundTo && foundFrom)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
