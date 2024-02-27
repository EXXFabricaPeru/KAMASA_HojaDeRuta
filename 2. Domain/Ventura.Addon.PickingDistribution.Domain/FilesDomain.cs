using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class FilesDomain : BaseDomain, IFileDomain
    {
        public FilesDomain(Company company) : base(company)
        {
        }

        public Dictionary<string, string> RetrieveFields(string destino)
        {
            return UnitOfWork.FilesRepository.RetrieveFields(destino);
        }

        public OFTP Retrieve_template(string CardCode)
        {
            return UnitOfWork.FilesRepository.retrieve_template(t => t.Code == CardCode).First();
        }

        public IEnumerable<FTP1> Retrieve_template_columns(string CardCode)
        {
            return UnitOfWork.FilesRepository.retrieve_template_columns(t => t.Code == CardCode && t.TargetType != "" && t.SAPValue != "");
        }

        public void CreaterOrder(ref ORDR_ OrdenVenta, out int DocEntry, out string mensaje)
        {
            UnitOfWork.FileUploadRepository.CreaterOrder(ref OrdenVenta, out DocEntry, out mensaje);
        }

        public void LoadFile(int docentry, string archivo)
        {
            UnitOfWork.FileUploadRepository.LoadFile(docentry, archivo);
        }

        public OFUP RetrieveUploadedFilesByEntry(int documentEntry)
        {
            return UnitOfWork.FileUploadRepository
                .Retrieve(doc => doc.DocumentEntry == documentEntry)
                .Single();
        }

        public OFTP RetrieveTemplateByCode(string code)
        {
            OFTP template = UnitOfWork.FilesRepository.retrieve_template(t=>t.Code == code).Single();
            template.ColumnDetail = UnitOfWork.FilesRepository.retrieve_template_columns(t => t.Code == code).ToList();
            return template;
        }
        public OPCG RetrieveTemplatePasarela(string pasarelaCode)
        {
            OPCG template = UnitOfWork.FilesRepository.retrieve_template_pasarela(t => t.Pasarela == pasarelaCode).Single();
            template.ColumnDetail = UnitOfWork.FilesRepository.retrieve_template_pasarela_columns(t => t.Code == template.Code).ToList();
            return template;
        }
        public IEnumerable<ORDR> BuildSaleOrders()
        {
            ORDR result = new ORDR();
            return null;
        }

        public IEnumerable<Tuple<string, string>> RetrieveSaleOrderFields()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveDocumentFields();
        }

        public IEnumerable<Tuple<string, string>> RetrieveSaleOrderLineFields()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveLineDocumentFields();
        }

        public IEnumerable<Tuple<string, string>> RetrieveLinesLiquidationCardsFields()
        {
            return UnitOfWork.InfrastructureRepository.RetrieveLineLiquidationCardsFields();
        }

    }
}