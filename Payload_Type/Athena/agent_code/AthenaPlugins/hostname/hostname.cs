﻿using System;
using System.Collections.Generic;
using System.Net;
namespace Athena
{
    public static class Plugin
    {

        public static string Execute(Dictionary<string, object> args)
        {
            return Dns.GetHostName();
        }
    }
}
