using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski_server
{
    public static class Constants
    {
        public static string SERVER_LOCATION = "http://192.168.1.102:3000/";
        public static int USER_ID = -1;

        public static string getUserID()
        {
            USER_ID++;
            return USER_ID.ToString();
        }
        

    }
}
