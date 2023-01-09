using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using template.Models;
using template.SQL;
using static template.Code.DataTypeConversionHelperClass;
namespace template.Services
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