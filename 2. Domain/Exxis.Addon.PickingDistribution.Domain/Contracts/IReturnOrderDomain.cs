using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IReturnOrderDomain
    {
        IEnumerable<ORRR> Retrieve(DateTime deliveryDate);

        ORRR RetrieveByEntry(int entry);

        IEnumerable<ORRR> ValidateDocuments(IEnumerable<ORRR> documents);
    }
}