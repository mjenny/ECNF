using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RoutesFactory {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        static public IRoutes Create(Cities cities)
        {
            return Create(cities, Properties.Settings.Default.RouteAlgorith);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cities"></param>
        /// <param name="algorithmClassName"></param>
        /// <returns></returns>
        static public IRoutes Create(Cities cities, string algorithmClassName)
        {
            Assembly asm = Assembly.LoadFrom(algorithmClassName);
            Type t = GetTypeFromAssembly(algorithmClassName, asm);
            return (t == null) ? null : Activator.CreateInstance(t, cities) as IRoutes;
        }

        public static Type GetTypeFromAssembly(String algorithmClassName, Assembly a)
        {
            foreach (var t in a.GetTypes)
            {
                if(t.FullName.Equals(algorithmClassName) && t.IsClass)
                    return t;
            }
            return null;
        }
    }
}
