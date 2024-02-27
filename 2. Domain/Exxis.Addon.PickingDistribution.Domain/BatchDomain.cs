using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class BatchDomain : BaseDomain, IBatchDomain
    {
        public BatchDomain(Company company) : base(company)
        {
        }

        public IEnumerable<OIBT> RetrieveBatchesPerItemCode(string itemCode)
        {
            return UnitOfWork.BatchRepository.GetBatches(itemCode);
        }

        public IEnumerable<OIBT> RetrieveBatchesPerItemCodeWO(string itemCode)
        {
            return UnitOfWork.BatchRepository.GetBatchesWithoutCNRV(itemCode);
        }

        public decimal RetrieveActualStock(string itemCode, string warehouse, string batchNumber)
        {
            return UnitOfWork.BatchRepository.RetrieveActualStock(itemCode,  warehouse, batchNumber);
        }

        public decimal RetrieveActualStock(string itemCode, string warehouse)
        {
            return UnitOfWork.BatchRepository.RetrieveActualStock(itemCode, warehouse);
        }
    }
}