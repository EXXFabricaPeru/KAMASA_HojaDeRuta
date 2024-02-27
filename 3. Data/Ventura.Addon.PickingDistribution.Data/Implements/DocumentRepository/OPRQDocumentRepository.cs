using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    public class OPRQDocumentRepository : BaseSAPDocumentRepository<OPRQ, PRQ1>
    {
        public OPRQDocumentRepository(Company company) : base(company)
        {

        }
    }
}
