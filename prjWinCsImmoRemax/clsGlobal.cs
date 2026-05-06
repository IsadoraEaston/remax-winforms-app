using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjWinCsImmoRemax
{
    public static class clsGlobal
    {
        public static DataSet setImmoRemaxDB;
        public static SqlDataAdapter adpEmployes, adpClients, adpMaisons;
    }
}
