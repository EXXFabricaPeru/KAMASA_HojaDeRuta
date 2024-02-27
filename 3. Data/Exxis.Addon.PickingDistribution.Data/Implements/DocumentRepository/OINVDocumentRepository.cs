// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    public class OINVDocumentRepository : BaseSAPDocumentRepository<OINV, INV1>
    {
        public OINVDocumentRepository(Company company) : base(company)
        {
        }
    }
}