using template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using template.Code;

namespace template.Services
{
    public class GlobalContainerService : IGlobaContainerService
    {
        // Prepare an instance on load
        public GlobalContainerService()
        {
            dicGlobalContainer = new Dictionary<string, GlobalContainerClass>();
            dicUserIDList = new Dictionary<string, int>();
        }

        public Dictionary<string, GlobalContainerClass> dicGlobalContainer { get; set; } 
        public Dictionary<string, int> dicUserIDList { get; set; }

        // Whenever a new user logs in, create or reset instance of GlobalContainerClass
        public GlobalContainerClass StoreUser(string UserName)
        {
            if (dicGlobalContainer.ContainsKey(UserName))
            {
                dicGlobalContainer[UserName].GlobalValues.ClearUserData();
                dicGlobalContainer.Remove(UserName);
            }
            
            GlobalContainerClass iGlobalContainer = new GlobalContainerClass();
            dicGlobalContainer.Add(UserName, iGlobalContainer);
            return iGlobalContainer;
        }

        /// <summary>
        /// Shortcuts to get the correct service class
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public GlobalValuesService GetGlobalValues(string UserName)
        {
            if (dicGlobalContainer == null) dicGlobalContainer = new Dictionary<string, GlobalContainerClass>();
            if (dicGlobalContainer.TryGetValue(UserName, out GlobalContainerClass iGlobalContainer))
            {
                iGlobalContainer.LastUsedDateStamp = DateTime.UtcNow;
                return iGlobalContainer.GlobalValues;
            }
            else
            {
                // If the user's global container doesn't exist, create it
                return StoreUser(UserName).GlobalValues;
            }
        }
    }
}
