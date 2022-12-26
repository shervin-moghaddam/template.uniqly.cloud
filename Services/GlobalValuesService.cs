using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using robotmanden.Models;
using robotmanden.SQL;
using static robotmanden.Code.DataTypeConversionHelperClass;
namespace robotmanden.Services
{
    public class GlobalValuesService : IGlobalValues
    {

        public void ClearUserData()
        {
            // UserGUID = Guid.Empty;
            // Id = 0;
            // UserConn = null;
            // SQLConnectionString = null;
            // WebDarkmode = 0;

        }
    }
}