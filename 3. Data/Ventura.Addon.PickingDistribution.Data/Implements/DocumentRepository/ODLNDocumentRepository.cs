// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    public class ODLNDocumentRepository : BaseSAPDocumentRepository<ODLN, DLN1>
    {
        public ODLNDocumentRepository(Company company) : base(company)
        {
        }
    }
}