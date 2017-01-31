using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker
{
    abstract class Managed
    {
        protected static Manager manager = null;

        public static void setManager(Manager m)
        {
            manager = m;
        }
    }
}
