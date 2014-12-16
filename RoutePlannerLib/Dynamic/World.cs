using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW.Dynamic
{
    public class World : DynamicObject
    {
        private Cities _cities;
        public World(Cities cities)
        {
            _cities = cities;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Console.WriteLine("Tryed to invoke: {0}", binder.Name);
            var city = _cities.FindCity(binder.Name);
            if (city != null)
            {
                result = city;
            }
            else
            {
                result = "The city \"" + binder.Name + "\" does not exist!";
            }
            
            return true;
        }

    }
}
