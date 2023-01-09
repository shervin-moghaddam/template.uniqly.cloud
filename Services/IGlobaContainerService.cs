using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using template.Code;

namespace template.Services
{
    interface IGlobaContainerService
    {
        Dictionary<string,GlobalContainerClass> dicGlobalContainer { get; set; }
    }
}
