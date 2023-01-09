using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using template.Services;
using static template.Code.DataTypeConversionHelperClass;
namespace template.Code
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
