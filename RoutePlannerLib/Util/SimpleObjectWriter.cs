using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        private System.IO.StringWriter stream;

        public SimpleObjectWriter(System.IO.StringWriter stream)
        {
            // TODO: Complete member initialization
            this.stream = stream;
        }

        public void Next(City c)
        {
            WriteType(c.GetType(), c);
        }

        private void WriteType(Type type, Object obj)
        {
            stream.WriteLine("Instance of {0}", type.FullName);

            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo item in type.GetProperties())
            {
                if ((item.PropertyType == typeof(System.String)))
                {
                    stream.WriteLine("{0}=\"{1}\"", item.Name, item.GetValue(obj));
                }
                else if ((item.PropertyType == typeof(System.Int16)) ||
                    (item.PropertyType == typeof(System.Int32)))
                {
                    stream.WriteLine("{0}={1}", item.Name, item.GetValue(obj));
       
                }
                else if ((item.PropertyType == typeof(System.Double)))
                {
                    stream.WriteLine("{0}={1}", item.Name, Convert.ToString(item.GetValue(obj), CultureInfo.InvariantCulture));
                }
                else if (item.PropertyType.BaseType == typeof(System.Object))
                {
                    stream.WriteLine("{0} is a nested object...", item.Name);
                    object propValue = item.GetValue(obj);
                    WriteType(Type.GetType(item.PropertyType.FullName), propValue);
                }
            }
            stream.WriteLine("End of instance");
        }
    }
}
