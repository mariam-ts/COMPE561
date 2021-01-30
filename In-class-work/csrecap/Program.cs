using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csrecap
{
    class Program
    {
        static void Main(string[] args)
        {
           /* Account tbc_atm = new Account(100, "Helen", 123456789);
            tbc_atm.Menu();*/

            VipAccount vip = new VipAccount(10, "Mariam Tsirekidze", 823460489, 10, 10, 10);
            vip.Menu();
            BusinessAccount business = new BusinessAccount(100, "Mariam Tsirekidze", 823460489, 10, 10, 10);
        }
    }
}
