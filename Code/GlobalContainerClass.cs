using robotmanden.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static robotmanden.Code.DataTypeConversionHelperClass;
namespace robotmanden.Code
{
    public class GlobalContainerClass
    {
        public DateTime CreatedDateStamp { get; set; }
        public DateTime LastUsedDateStamp { get; set; }
        public GlobalValuesService GlobalValues { get; set; }
     

        /// <summary>
        /// On construct, prepare all variables. This will be reset each time 
        /// the user logs in, by GlobalContainerService.
        /// </summary>
        public GlobalContainerClass()
        {
            // Storing dates
            CreatedDateStamp = DateTime.UtcNow;
            LastUsedDateStamp = DateTime.UtcNow;

            // Creating instances of each service class
            GlobalValues = new GlobalValuesService();
        }

     
    }
}
