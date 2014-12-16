using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib_JW.Util
{
    public class SimpleObjectReader
    {
        private System.IO.StringReader stream;

        public SimpleObjectReader(System.IO.StringReader stream)
        {
            // TODO: Complete member initialization
            this.stream = stream;
        }

        public Object Next()
        {
            int state = 0;
            Object retVal = null;
            string noName = String.Empty;
            Object no = null;
            string line;
            while ((line = stream.ReadLine()) != null)
            {
                switch (state)
                {
                    case 0:
                        if (line.Contains("Instance of "))
                        {
                            line = line.Remove(0, "Instance of ".Length);
                            retVal = CreatObject(line);
                            state = 1;
                        }
                        break;

                    case 1:

                        if (line.Contains("is a nested object..."))
                        {
                            noName = line.Substring(0, line.IndexOf(" "));
                            state = 2;
                        }
                        else if(line.Contains("End of instance"))
                        {
                            state = 0;
                            return retVal;
                        }
                        else
                        {
                            //Create to level member
                            Type t = retVal.GetType();

                            var index = line.IndexOf('=');
                            var name = line.Substring(0, index);

                            var pi = t.GetProperty(name);

                            if (null != pi)
                            {
                                if (pi.PropertyType.Name == "String")
                                {
                                    var value = line.Substring(index + 2, line.Length - index - 3);
                                    pi.SetValue(retVal, value);
                                }
                                else if (pi.PropertyType.Name == "Int32") 
                                {
                                    var value = line.Substring(index + 1, line.Length - index - 1);
                                    pi.SetValue(retVal, Int32.Parse(value));
                                }
                                else if (pi.PropertyType.Name == "Double")
                                {
                                    var value = line.Substring(index + 1, line.Length - index - 1);
                                    pi.SetValue(retVal, Double.Parse(value, CultureInfo.InvariantCulture));
                                }
                                
                            }
                        }

                        break;

                    case 2:
                        //Add nested Object
                        if (line.Contains("Instance of "))
                        {
                            line = line.Remove(0, "Instance of ".Length);
                            no = CreatObject(line);

                            if (null != no)
                            {
                                Type t = retVal.GetType();
                                var pi = t.GetProperty(noName);
                                if (null != pi)
                                {
                                    pi.SetValue(retVal, no);
                                    state = 3;
                                }

                            }
                        }

                        break;

                    case 3:
                        //Add nested Object properties
                        if (line.Contains("End of instance"))
                        {
                            //Continue with top level members
                            state = 1;
                        }
                        else
                        {
                            //Create to level member
                            Type t = no.GetType();

                            var index = line.IndexOf('=');
                            var name = line.Substring(0, index);

                            var pi = t.GetProperty(name);

                            if (null != pi)
                            {
                                if (pi.PropertyType.Name == "String")
                                {
                                    var value = line.Substring(index + 2, line.Length - index - 3);
                                    pi.SetValue(no, value);
                                }
                                else if (pi.PropertyType.Name == "Int32")
                                {
                                    var value = line.Substring(index + 1, line.Length - index - 1);
                                    pi.SetValue(no, Int32.Parse(value));
                                }
                                else if (pi.PropertyType.Name == "Double")
                                {
                                    var value = line.Substring(index + 1, line.Length - index - 1);
                                    pi.SetValue(no, Double.Parse(value, CultureInfo.InvariantCulture));
                                }

                            }

                        }
                        break;
                    default:
                        break;
                        
                }
            }


            /*
             * State Machine
             * Create Top Level
             * Create Top Level Attribute
             * Create Sub Level
             * Create Sub Level Attribute
             * */

            return retVal;
        }

        private Object CreatObject(string line)
        {
            Assembly ass = Assembly.Load("RoutePlannerLib_JW");
            object o = ass.CreateInstance(line);    
            return o;
        }
    }
}
