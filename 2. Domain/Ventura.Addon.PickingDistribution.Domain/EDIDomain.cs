using System;
using System.Collections.Generic;
using System.IO;
using IronXL;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Domain.EDIProcessor;
using ClosedXML.Excel;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class EDIDomain : BaseDomain, IEDIDomain
    {
        public EDIDomain(SAPbobsCOM.Company company) : base(company)
        {
        }

        public IEnumerable<ExcelEDIProcessor.GenerateResponse> GenerateSaleOrders(WorkBook workBook, OFTP selectedTemplate, IEnumerable<OEIT> intermediaryMappingValues, string businessPartnerCardCode, DateTime deliveryDate, IEnumerable<string> identifierFilters)
        {
            var ediProcessor = new ExcelEDIProcessor(workBook, selectedTemplate, intermediaryMappingValues, Company, identifierFilters);
            return ediProcessor.GenerateSaleOrders(businessPartnerCardCode, deliveryDate);
        }
        public IEnumerable<ExcelEDIProcessor.GenerateResponse> GenerateSaleOrders(XLWorkbook workBook, OFTP selectedTemplate, IEnumerable<OEIT> intermediaryMappingValues, string businessPartnerCardCode, DateTime deliveryDate, IEnumerable<string> identifierFilters)
        {
            var ediProcessor = new ExcelEDIProcessor(workBook, selectedTemplate, intermediaryMappingValues, Company, identifierFilters);
            return ediProcessor.GenerateSaleOrders(businessPartnerCardCode, deliveryDate);
        }

        public Tuple<bool, string, int, int> RegisterEDIFile(OFUP ediFile)
        {
            return UnitOfWork.FileUploadRepository.Register(ediFile);
        }

        public Tuple<bool, string> UpdateStatusEDIFile(int entry, string status)
        {
            return UnitOfWork.FileUploadRepository.UpdateStatusEDIFile(entry, status);
        }

        public Tuple<bool, string> InsertLineEDIFile(int entry, IEnumerable<FUP1> lines)
        {
            return UnitOfWork.FileUploadRepository.InsertLineEDIFile(entry, lines);
        }

        public string CopyFileToServerPath(string sourceFilePath)
        {
            SAPbobsCOM.CompanyService companyService = null;
            SAPbobsCOM.PathAdmin pathAdmin = null;
            try
            {
                companyService = Company.GetCompanyService();
                pathAdmin = companyService.GetPathAdmin();
                string basePath = pathAdmin.AttachmentsFolderPath + "EDI_files";
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);
                var fileName = DateTime.Now.ToString("yyyy_MM_dd");
                string[] existingFiles = Directory.GetFiles(basePath, $"{fileName}*");
                var destinyFilePath = $"{basePath}\\{fileName}_{existingFiles.Length}{Path.GetExtension(sourceFilePath)}";
                File.Copy(sourceFilePath, destinyFilePath);
                return destinyFilePath;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(companyService, pathAdmin);
            }
        }
    }
}