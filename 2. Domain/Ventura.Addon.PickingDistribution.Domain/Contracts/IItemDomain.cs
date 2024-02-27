using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IItemDomain
    {
        decimal RetrieveSaleWeight(string itemCode);

        decimal RetrieveInventoryWeight(string itemCode);

        OITM Retrieve(string itemCode);

        OITM RetrieveByEDICode(string ediCode);

        decimal RetrieveAvaiblebyWHS(string itemCode, DateTime fecha, string warehouse);

        decimal RetrieveAvaiblebyWHS_Wo(string itemCode, DateTime fecha, string warehouse);

        decimal RetrieveAvaiblebyWHS_Wo_byBatch(string itemCode, DateTime fecha, string warehouse, string batch);

        decimal RetrieveAvaiblebyWHS_Wo_byBatch_WO_Date(string itemCode, DateTime fecha, string warehouse, string batch);

    }
}