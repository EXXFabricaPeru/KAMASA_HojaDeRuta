using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document
{
    
    public class OCTG
    {
        [SAPColumn("ExtraMonth")]
        public int months { get; set; }

        [SAPColumn("ExtraDays")]
        public int days { get; set; }



    }
}
