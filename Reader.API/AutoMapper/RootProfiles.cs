using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reader.API.AutoMapper
{
    public static class RootProfiles
    {
        public static Type[] Maps => new[]
        {
            typeof(TagProfiles)
        };
    }
}
