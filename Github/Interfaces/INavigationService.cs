﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.Interfaces
{
    public interface INavigationService
    {        
        Task NavigateToLoginView();
        Task NavigateToMainView();

        //Other Views come here...
    }
}
