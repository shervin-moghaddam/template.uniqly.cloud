﻿using robotmanden.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace robotmanden.Services
{
    interface IGlobaContainerService
    {
        Dictionary<string,GlobalContainerClass> dicGlobalContainer { get; set; }
    }
}
