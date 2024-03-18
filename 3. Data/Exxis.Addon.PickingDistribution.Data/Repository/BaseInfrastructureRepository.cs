using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseInfrastructureRepository : Code.Repository
    {
        protected BaseInfrastructureRepository(Company company) : base(company)
        {
        }

  

        public abstract string RetrieveDescriptionOfValidValueCode(string field, string code);

        public abstract IEnumerable<Tuple<string, string>> RetrieveSaleChannels();

        public abstract IEnumerable<Tuple<string, string>> RetrieveValidValues<TEntity, TKProperty>(Expression<Func<TEntity, TKProperty>> propertyExpression)
            where TEntity : BaseUDO;

        /// <summary>
        /// Retrieve batches from current document
        /// </summary>
        /// <param name="documentEntry">Current document's DocEntry</param>
        /// <param name="lineNumber">Current document's LineNum</param>
        /// <param name="sapType">Current document's ObjType</param>
        /// <param name="warehouseCode">Current document's warehouse to is located batch</param>
        /// <returns>Reference batches</returns>
        public abstract IEnumerable<SAPSelectedBatch> RetrieveBatchesFromDocument(int documentEntry, int lineNumber, int sapType, string warehouseCode);

        /// <summary>
        /// Retrieve batches from current document
        /// </summary>
        /// <param name="documentEntry">Current document's DocEntry</param>
        /// <param name="lineNumber">Current document's LineNum</param>
        /// <param name="sapType">Current document's ObjType</param>
        /// <param name="warehouseCode">Current document's warehouse to is located batch</param>
        /// <returns>Reference batches</returns>
        public abstract IEnumerable<SAPSelectedBatch> RetrieveBatchesFromRequestDocument(int documentEntry, int lineNumber, int sapType, string warehouseCode);

        public abstract IEnumerable<Tuple<string, string>> RetrieveDocumentFields();

        public abstract IEnumerable<Tuple<string, string>> RetrieveLineDocumentFields();

     
        public abstract string RetrieveLastNumberBySerie(string serie, string tipo);

        public abstract List<Tuple<string, string>> RetriveMotiveTransferSoraya();

        public abstract List<Tuple<string, string>> RetriveMotiveTransferSunat();

        public abstract string RetrievePaymentGroupNumDescription(int code);

        public abstract Tuple<int, int> RetrievePaymentGroupNumDetail(int code);
        public abstract IEnumerable<Tuple<string, string>> RetrieveTipoFlujos();
    }
}