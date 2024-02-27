using System;
using System.Collections.Generic;
using IronXL;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.EDIProcessor;
using ClosedXML.Excel;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IEDIDomain
    {
        IEnumerable<ExcelEDIProcessor.GenerateResponse> GenerateSaleOrders(WorkBook workBook, OFTP selectedTemplate,
            IEnumerable<OEIT> intermediaryMappingValues, string businessPartnerCardCode, DateTime deliveryDate, 
            IEnumerable<string> identifierFilters);

        IEnumerable<ExcelEDIProcessor.GenerateResponse> GenerateSaleOrders(XLWorkbook workBook, OFTP selectedTemplate,
            IEnumerable<OEIT> intermediaryMappingValues, string businessPartnerCardCode, DateTime deliveryDate,
            IEnumerable<string> identifierFilters);

        Tuple<bool, string, int, int> RegisterEDIFile(OFUP ediFile);

        Tuple<bool, string> InsertLineEDIFile(int entry, IEnumerable<FUP1> lines);

        Tuple<bool, string> UpdateStatusEDIFile(int entry, string status);

        string CopyFileToServerPath(string sourceFilePath);
    }
}