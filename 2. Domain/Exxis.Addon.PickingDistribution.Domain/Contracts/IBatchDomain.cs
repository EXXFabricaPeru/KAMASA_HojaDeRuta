using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IBatchDomain
    {
        IEnumerable<OIBT> RetrieveBatchesPerItemCode(string itemCode);
        IEnumerable<OIBT> RetrieveBatchesPerItemCodeWO(string itemCode);
        decimal RetrieveActualStock(string itemCode, string warehouse, string batchNumber);
        decimal RetrieveActualStock(string itemCode, string warehouse);
    }
}