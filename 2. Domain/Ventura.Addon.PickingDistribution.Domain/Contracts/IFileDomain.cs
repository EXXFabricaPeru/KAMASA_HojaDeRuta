using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IFileDomain : IBaseDomain
    {
        Dictionary<string, string> RetrieveFields(string destino);
        OFTP Retrieve_template(string CardCode);
        IEnumerable<FTP1> Retrieve_template_columns(string CardCode);
        void CreaterOrder(ref ORDR_ OrdenVenta, out int DocEntry, out string mensaje);
        void LoadFile(int docentry, string archivo);

        OFUP RetrieveUploadedFilesByEntry(int documentEntry);
        OFTP RetrieveTemplateByCode(string code);

        IEnumerable<ORDR> BuildSaleOrders();
        IEnumerable<Tuple<string, string>> RetrieveSaleOrderFields();
        IEnumerable<Tuple<string, string>> RetrieveSaleOrderLineFields();

        IEnumerable<Tuple<string, string>> RetrieveLinesLiquidationCardsFields();


        OPCG RetrieveTemplatePasarela(string pasarelaCode);

        //IEnumerable<FTP1> Retrieve_template_columns(string CardCode);
    }
}
