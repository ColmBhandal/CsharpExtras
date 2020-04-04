using CsharpExtras.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Api
{
    public class CsharpExtrasApi
    {
        public void SetLogger(ILogger logger)
        {
            StaticLogManager.Logger = logger;
        }

    }
}
