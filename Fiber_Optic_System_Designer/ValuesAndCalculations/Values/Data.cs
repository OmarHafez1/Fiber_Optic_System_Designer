using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiber_Optic_System_Designer.ValuesAndCalculations
{
    internal class Data
    {
        String name, value;
        public Data(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public override String ToString()
        {
            return name + ": " + value;
        }
    }
}
