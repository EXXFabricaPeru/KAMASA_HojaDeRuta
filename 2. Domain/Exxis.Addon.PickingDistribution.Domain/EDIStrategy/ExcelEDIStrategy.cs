using System;
using System.Collections.Generic;
using System.Linq;
using IronXL;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data;
using Exxis.Addon.HojadeRutaAGuia.Domain.EDIStrategy.Structures;
using SystemModels = Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System;
using UDOModels = Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.EDIStrategy
{
    public class ExcelEDIStrategy : BaseEDIStrategy
    {
        const int FIRST_ROW_EXCEL_INDEX = 0;
        const int SECOND_ROW_EXCEL_INDEX = 1;

        private WorkBook _excelWorkBook;
        private WorkSheet _mainWorkSheet;
        private RangeRow _headerRow;
        private RangeColumn[] _headerColumns;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ExcelEDIStrategy(string pathFile, UDOModels.Header.OFTP fileTemplate, SystemModels.Header.OCRD businessPartner, UnitOfWork unitOfWork)
            : base(pathFile, fileTemplate, businessPartner, unitOfWork)
        {
            _excelWorkBook = WorkBook.Load(PathFile);
        }

        #region Overrides of BaseEDIStrategy

        public override void ValidateFile()
        {
            if (_excelWorkBook.WorkSheets.Count != 1)
                throw new Exception(@"[Validación] El archivo excel solo puede tener una hoja de trabajo.");
            
            _mainWorkSheet = _excelWorkBook.WorkSheets.First();
            _headerRow = _mainWorkSheet.GetRow(FIRST_ROW_EXCEL_INDEX);
            _headerColumns = _headerRow.Columns;
            validate_header();
        }

        private void validate_header()
        {
            foreach (UDOModels.Detail.FTP1 column in PopulatedTemplateColumns)
            {
                var isValidColumn = false;
                for (var columnIndex = 0; columnIndex < _mainWorkSheet.ColumnCount; columnIndex++)
                {
                    RangeColumn rowColumn = _headerColumns[columnIndex];
                    if (column.ExcelColumnName != rowColumn.StringValue)
                        continue;

                    isValidColumn = true;
                    break;
                }

                if (!isValidColumn)
                    throw new Exception($"[Validación] La columna '{column.ExcelColumnName}' no existe en el archivo excel a cargar.");
            }
        }
        
        private int get_excel_column_header_index_by_sap_field(string sapFieldName)
        {
            string excelColumnHeaderName = PopulatedTemplateColumns
                .Single(t => t.SAPValue == sapFieldName)
                .ExcelColumnName;

            return get_excel_column_header_index_by_excel_address(excelColumnHeaderName);
        }

        private int get_excel_column_header_index_by_excel_address(string excelAddress)
        {
            for (var i = 0; i < _headerColumns.Length; i++)
            {
                RangeColumn headerColumn = _headerColumns[i];
                if (headerColumn.StringValue == excelAddress)
                    return i;
            }

            throw new Exception($"[Error] La columna '{excelAddress}' no existe en el archivo Excel.");
        }

        public override IEnumerable<EDIRecord> BuildEDIRecords()
        {
            int mainColumnIndex = get_excel_column_header_index_by_excel_address(FileTemplate.MainColumnAddress);
            int addressCodeColumnIndex = get_excel_column_header_index_by_sap_field("ShipToAdr");
            int itemCodeColumnIndex = get_excel_column_header_index_by_sap_field("ItemCode");
            int itemPriceColumnIndex = get_excel_column_header_index_by_sap_field("UnitPrice");
            int itemQuantityColumnIndex = get_excel_column_header_index_by_sap_field("Quantity");

            for (int rowIndex = SECOND_ROW_EXCEL_INDEX; rowIndex < _mainWorkSheet.RowCount; rowIndex++)
            {
                RangeRow currentRow = _mainWorkSheet.Rows[rowIndex];
                RangeColumn[] currentRowColumns = currentRow.Columns;
                
                var record = new EDIRecord
                {
                    EDIIdentifier = currentRowColumns[mainColumnIndex].StringValue,
                    EDIAddressCode = currentRowColumns[addressCodeColumnIndex].StringValue,
                    SAPAddressCode = null,
                    SAPAddressName = null,
                    SAPAddressStreet = null,
                    EDIItemCode = currentRowColumns[itemCodeColumnIndex].StringValue,
                    SAPItemCode = null,
                    SAPItemName = null,
                    SAPItemPrice = 0,
                    EDIItemPrice = currentRowColumns[itemPriceColumnIndex].DecimalValue,
                    EDIItemQuantity = currentRowColumns[itemQuantityColumnIndex].DecimalValue
                };

                SystemModels.Detail.CRD1 selectedAddress = BusinessPartner.Addresses
                    .TrySingle(t => t.DeliveryCode == record.EDIAddressCode,
                        exception => { throw new Exception($"El código EDI de dirección '{record.EDIAddressCode}' no esta configurado en el Maestro de Socio de Negocios (OCRD)."); });
                
                record.SAPAddressCode = selectedAddress.Code;
                record.SAPAddressName = selectedAddress.SecondAddress;
                record.SAPAddressStreet = selectedAddress.Street;

                SystemModels.Header.OITM item = UnitOfWork.ItemsRepository.FindByEDICode(record.EDIItemCode);

                record.SAPItemCode = item.ItemCode;
                record.SAPItemName = item.ItemDescription;
                //TODO: hacerlo en base a la lista de precios, configurado por addon LP. solo se replica lo del procedure para tener el territorio
                // record.SAPItemPrice = item.SaleUnit;
                // _contentDataTable.SetValue("Col_0", index, "N");
                // _contentDataTable.SafetySetValue("Col_1", index, record.Identifier);
                // _contentDataTable.SafetySetValue("Col_2", index, record.AddressCode);
                //
                // _contentDataTable.SafetySetValue("Col_3", index, selectedAddress.SecondAddress);
                // _contentDataTable.SafetySetValue("Col_4", index, selectedAddress.Street);
                //
                // OITM selectedItem = _itemDomain.RetrieveByEDICode(record.ItemEDICode);
                // _contentDataTable.SafetySetValue("Col_5", index, selectedItem.ItemCode);
                // _contentDataTable.SafetySetValue("Col_6", index, selectedItem.ItemDescription);
                // _contentDataTable.SafetySetValue("Col_7", index, record.ItemPrice);
                // _contentDataTable.SafetySetValue("Col_8", index, record.ItemQuantity);
                // _contentDataTable.SafetySetValue("Col_9", index, @"Pendiente");

                yield return record;
            }
        }

        private int retrieve_column_index_by_cell_value(RangeRow headerRow, string cellValue)
        {
            RangeColumn[] headerColumns = headerRow.Columns;
            for (var i = 0; i < headerColumns.Length; i++)
            {
                RangeColumn column = headerColumns[i];
                if (column.StringValue == cellValue)
                    return i;
            }

            throw new Exception($"[Error] La columna '{cellValue}' no existe en el archivo Excel.");
        }

        public override IEnumerable<SystemModels.Header.Document.ORDR> BuildSaleOrders()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<RegisterResume> RegisterSaleOrders()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}