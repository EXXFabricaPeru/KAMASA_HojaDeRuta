using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable("OHEM", FormType = "dummy")]
    public class OHEM : BaseSAPTable
    {
        [SAPColumn("Code")]
        public string Code { get; set; }

        [SAPColumn("firstName")]
        public string FirstName { get; set; }

        [SAPColumn("lastName")]
        public string LastName { get; set; }
    }
}