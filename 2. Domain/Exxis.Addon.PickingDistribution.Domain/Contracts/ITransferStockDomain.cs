using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ITransferStockDomain : IBaseDomain
    {
        OWTR RetrieveTransfeStockByEntry(int documentEntry);

        Tuple<bool, string> UpdateEntity(OWTR document);
        Tuple<bool, string> UpdateEntityRequest(OWTR document);

        Tuple<bool, string> UpdateFolio(OWTR document);
        Tuple<bool, string> UpdateFolioRequest(OWTR document);

        OWTR RetrieveTransfeStockByOT(int transferOrderDocEntry);

        OWTR RetrieveRequestTransfeStockByOT(int transferOrderDocEntry);
    }
}
