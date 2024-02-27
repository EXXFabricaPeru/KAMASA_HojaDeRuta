using System;
using System.Collections.Generic;
using System.Linq;
using Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup
{
    public class VersionCollection : List<Versioner>
    {
        public string GetLastVersion()
        {
            Versioner lastVersion = this.LastOrDefault();
            if(lastVersion == null)
                throw new Exception("The application doesn't have versions to implement.");

            return lastVersion.CurrentVersion;
        }
    }
}