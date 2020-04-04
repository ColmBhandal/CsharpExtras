using CsharpExtras.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Api
{
    public class QueryApi
    {
        public void SetLogger(ILogger logger)
        {
            StaticLogManager.Logger = logger;
        }

    }
}
