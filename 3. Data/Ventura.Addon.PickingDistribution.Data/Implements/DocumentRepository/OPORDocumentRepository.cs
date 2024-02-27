using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{

    public class OPORDocumentRepository : BaseSAPDocumentRepository<OPOR, POR1>
    {
        public OPORDocumentRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OPOR> RetrieveDocuments(Expression<Func<OPOR, bool>> expression)
        {
            IEnumerable<OPOR> documents = base.RetrieveDocuments(expression)
                .ToList();
            var unitOfWork = new UnitOfWork(Company);
            foreach (OPOR document in documents)
            {
                POR1 firstLine = document.DocumentLines.First();
                OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
                //document.IsService = item.IsInventory == false;
                //document.IsItem = item.IsInventory;
            }

            return documents;
        }

        public override void UpdateSystemFieldsFromDocument(OPOR document)
        {
            var documents = Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders).To<Documents>();
            documents.GetByKey(document.DocumentEntry);
            foreach (var documentLine in document.DocumentLines)
            {
                documents.Lines.SetCurrentLine(documentLine.LineNumber);
                documents.Lines.Quantity = documentLine.Quantity.ToDouble();
            }

            documents.Update();
        }


    }
}
