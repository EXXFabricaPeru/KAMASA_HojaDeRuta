using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IronXL;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain.Extractor;
using Z.Core.Extensions;
using ClosedXML.Excel;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.EDIProcessor
{
    public class ExcelEDIProcessor
    {
        private const int EXCEL_FIRST_ROW = 0;
        private const int EXCEL_SECOND_ROW = 1;
        private readonly OFTP _chargingTemplate;
        private readonly IEnumerable<OEIT> _intermediaryMappingValues;
        private readonly IEnumerable<string> _identifierFilter;

        private readonly WorkBook _workBook;
        private readonly XLWorkbook _xlworkBook;
        private readonly WorkSheet _workSheet;
        private readonly IXLWorksheet _xlworkSheet;
        private readonly SAPbobsCOM.Company _company;
        private RangeRow _headerRow;
        private IXLRow _firstRow;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ExcelEDIProcessor(WorkBook workBook, OFTP chargingTemplate, IEnumerable<OEIT> intermediaryMappingValues, SAPbobsCOM.Company company,
            IEnumerable<string> identifierFilter)
        {
            _workBook = workBook;
            _workSheet = _workBook.WorkSheets.Single();
            _chargingTemplate = chargingTemplate;
            _intermediaryMappingValues = intermediaryMappingValues;
            _company = company;
            _identifierFilter = identifierFilter;
        }
        public ExcelEDIProcessor(XLWorkbook workBook, OFTP chargingTemplate, IEnumerable<OEIT> intermediaryMappingValues, SAPbobsCOM.Company company,
            IEnumerable<string> identifierFilter)
        {
            _xlworkBook = workBook;
            _xlworkSheet = _xlworkBook.Worksheets.Single();
            _chargingTemplate = chargingTemplate;
            _intermediaryMappingValues = intermediaryMappingValues;
            _company = company;
            _identifierFilter = identifierFilter;
        }

        private void validate_price_list_process(ref SAPbobsCOM.Documents documents, ref SAPbobsCOM.Document_Lines documentLines)
        {
            SAPbobsCOM.Recordset recordset = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;
            SAPbobsCOM.UserFields userFields = null;
            try
            {
                var query = string.Empty;
                var priceListCategory = default(string);
                var saleChannel = default(string);
                var territory = default(string);
                var priceListValue = default(double);
                //query =
                //    $"select \"U_VS_CATEGORIALP\" from OITM where \"ItemCode\" = '{documentLines.ItemCode}' and ifnull(\"U_VS_CATEGORIALP\",'') <> ''";
                //recordset = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset).To<SAPbobsCOM.Recordset>();
                //recordset.DoQuery(query);
                //if (!recordset.EoF)
                //{
                //    fields = recordset.Fields;
                //    field = fields.Item(0);
                //    priceListCategory = field.Value?.ToString();
                //}

                //if (string.IsNullOrEmpty(priceListCategory))
                //    throw new Exception($"No se ha definido la categoria del producto '{documentLines.ItemCode}'");

                userFields = documents.UserFields;
                fields = userFields.Fields;
                field = fields.Item(@"U_CL_CANAL");
                saleChannel = field.Value.ToString();
                fields = userFields.Fields;
                field = fields.Item(@"U_CL_TERRIT");
                territory = field.Value.ToString();

                //if("PTFV03VC.0190"== documentLines.ItemCode)
                //{
                //    var x = 11;
                //}
                query =
                    $"CALL\"VS_SP_ObtenerPrecioUnitario\"('{documents.CardCode}','{saleChannel}','{territory}','{priceListCategory}','{documentLines.ItemCode}')";
                recordset = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset).To<SAPbobsCOM.Recordset>();
                recordset.DoQuery(query);

                fields = recordset.Fields;
                field = fields.Item(0);
                priceListValue = field.Value is double ? Convert.ToDouble(field.Value) : default(double);

                if (Math.Abs(documentLines.UnitPrice - priceListValue) > 0)
                    throw new Exception(
                        $" Diferencia en precios: Excel: '{documentLines.UnitPrice}'; SAP: '{priceListValue}'");
                    //throw new Exception(
                    //    $"El precio definido en el Excel es '{documentLines.UnitPrice}'; distinto al calculado en SAP que es '{priceListValue}'");


               
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(recordset, fields, field, userFields);
            }
        }

        public IEnumerable<GenerateResponse> GenerateSaleOrders(string businessPartnerCardCode, DateTime deliveryDate)
        {
            //_headerRow = _workSheet.GetRow(EXCEL_FIRST_ROW);
            _firstRow = _xlworkSheet.Rows().First();

            IList<GenerateResponse> result = new List<GenerateResponse>();
            SAPbobsCOM.Documents documents = null;
            SAPbobsCOM.Document_Lines documentLines = null;

            IEnumerable<GroupingRangeRowExcel> groupingRows_ = grouping_rows_excel(_chargingTemplate.MainColumnAddress);


            //IEnumerable<GroupingRangeRow> groupingRows = grouping_rows(_chargingTemplate.MainColumnAddress);

            IEnumerable<FTP1> headerTemplateMapping = _chargingTemplate.ColumnDetail
                .Where(t => t.SAPStructure == FTP1.SAPStructures.HEADER_CODE)
                .ToList();

            IEnumerable<FTP1> lineTemplateMapping = _chargingTemplate.ColumnDetail
                .Where(t => t.SAPStructure == FTP1.SAPStructures.LINE_CODE)
                .ToList();

            var cont = 0;
            foreach (GroupingRangeRowExcel groupingRow in groupingRows_)
            {
                var response = new GenerateResponse
                {
                    WorkBookIdentifierReference = groupingRow.Key
                };

                try
                {
                    documents = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders).To<SAPbobsCOM.Documents>();
                    documents.CardCode = businessPartnerCardCode;
                    var stringValue = "";
                    using (SafeRecordSet recordSet = _company.MakeSafeRecordSet())
                    {
                        recordSet.ExecuteQuery(string.Format("Select \"CardName\" from OCRD where \"CardCode\"='{0}'", businessPartnerCardCode));
                        recordSet.ReadSingleRecord(x => stringValue = x.GetString(0));
                    }
                    documents.JournalMemo = stringValue;
                    documents.DocDueDate = deliveryDate;
                    Tuple<int, IXLRow> tupleRow = groupingRow.Rows.First();
                    IXLRow groupRow = tupleRow.Item2;
                    process_header(ref documents, groupRow, headerTemplateMapping);
                    documentLines = documents.Lines;

                    for (var index = 0; index < groupingRow.Rows.Count; index++)
                    {
                        tupleRow = groupingRow.Rows[index];
                        try
                        {
                            IXLRow row = tupleRow.Item2;
                            process_detail(ref documentLines, row, lineTemplateMapping); //TODO REVISAR EDI
                            validate_price_list_process(ref documents, ref documentLines);
                            if (index + 1 < groupingRow.Rows.Count)
                                documentLines.Add();
                        }
                        catch (Exception exception)
                        {
                            GenerateResponse lineResponse = response.DeepClone();
                            lineResponse.Success = false;
                            lineResponse.LineIndex = tupleRow.Item1;// tupleRow.Item2.RangeAddress.FirstAddress.RowNumber;
                            lineResponse.ErrorMessage = exception.Message;// --+ documentLines.ItemCode + "-" + index + "-"+ tupleRow.Item1 + "cont-"+ cont;
                            lineResponse.Index = cont;
                            result.Add(lineResponse);
                        }
                        cont++;
                    } 

                    if (result.Count(t => t.WorkBookIdentifierReference == response.WorkBookIdentifierReference && !t.Success) > 0)
                        continue;
                    //quitar comentarios
                    if (documents.Add() != default(int))
                    {
                        response.Success = false;
                        response.LineIndex = -1;
                        response.ErrorMessage = _company.GetLastErrorDescription();
                    }
                    else
                    {
                        response.Success = true;
                        response.DocumentEntry = _company.GetNewObjectKey().ToInt32();
                        documents.GetByKey(response.DocumentEntry);
                        response.DocumentNumber = documents.DocNum;
                    }

                    //Tuple<int, RangeRow> tupleRow = groupingRow.Rows.First();
                    //RangeRow groupRow = tupleRow.Item2;
                    //process_header(ref documents, groupRow, headerTemplateMapping);
                    //documentLines = documents.Lines;

                    //for (var index = 0; index < groupingRow.Rows.Count; index++)
                    //{
                    //    tupleRow = groupingRow.Rows[index];
                    //    try
                    //    {
                    //        RangeRow row = tupleRow.Item2;
                    //        process_detail(ref documentLines, row, lineTemplateMapping);
                    //        validate_price_list_process(ref documents, ref documentLines);
                    //        if (index + 1 < groupingRow.Rows.Count)
                    //            documentLines.Add();
                    //    }
                    //    catch (Exception exception)
                    //    {
                    //        GenerateResponse lineResponse = response.DeepClone();
                    //        lineResponse.Success = false;
                    //        lineResponse.LineIndex = tupleRow.Item1;
                    //        lineResponse.ErrorMessage = exception.Message;
                    //        result.Add(lineResponse);
                    //    }
                    //}

                    //if (result.Count(t => t.WorkBookIdentifierReference == response.WorkBookIdentifierReference && !t.Success) > 0)
                    //    continue;

                    //if (documents.Add() != default(int))
                    //{
                    //    response.Success = false;
                    //    response.LineIndex = -1;
                    //    response.ErrorMessage = _company.GetLastErrorDescription();
                    //}
                    //else
                    //{
                    //    response.Success = true;
                    //    response.DocumentEntry = _company.GetNewObjectKey().ToInt32();
                    //    documents.GetByKey(response.DocumentEntry);
                    //    response.DocumentNumber = documents.DocNum;
                    //}
                }
                catch (Exception exception)
                {
                    response.Success = false;
                    response.LineIndex = -1;
                    response.ErrorMessage = exception.Message;
                }
                finally
                {
                    
                }

                result.Add(response);

            }

            GenericHelper.ReleaseCOMObjects(documents, documentLines);
            return result;
        }
        private IEnumerable<GroupingRangeRowExcel> grouping_rows_excel(string mainColumnAddress)
        {
            IList<GroupingRangeRowExcel> result = new List<GroupingRangeRowExcel>();
            int mainColumnIndex = retrieve_column_index(mainColumnAddress, false);
            bool first = true;
            int i = 0;
            foreach (IXLRow row in _xlworkSheet.Rows())
            {

                if (first)
                {
                    first = false;
                    i++;
                }
                else
                {
                    if (!_identifierFilter.Contains(row.Cell(mainColumnIndex).Value.ToString()))
                        continue;
                    i++;
                    int? indexExists = retrieve_grouped_exist_list(result, row.Cell(mainColumnIndex).Value.ToString());

                    if (indexExists == null)
                    {
                        var grouping = new GroupingRangeRowExcel(row.Cell(mainColumnIndex).Value.ToString());
                        grouping.Rows.Add(Tuple.Create(i - EXCEL_SECOND_ROW, row));
                        result.Add(grouping);
                    }
                    else
                    {
                        GroupingRangeRowExcel grouping = result[indexExists.Value];
                        grouping.Rows.Add(Tuple.Create(i - EXCEL_SECOND_ROW, row));
                    }


                }
            }

            return result;
        }

        private IEnumerable<GroupingRangeRow> grouping_rows(string mainColumnAddress)
        {
            IList<GroupingRangeRow> result = new List<GroupingRangeRow>();
            int mainColumnIndex = retrieve_column_index(mainColumnAddress, false);
           

            for (int i = EXCEL_SECOND_ROW; i < _workSheet.RowCount; i++)
            {
                RangeRow currentRow = _workSheet.GetRow(i);
                RangeColumn currentRowColumn = currentRow.Columns[mainColumnIndex];

                if (_identifierFilter.Contains(currentRowColumn.StringValue))
                    continue;

                int? indexExists = retrieve_grouped_exist_list(result, currentRowColumn.StringValue);
                if (indexExists == null)
                {
                    var grouping = new GroupingRangeRow(currentRowColumn.StringValue);
                    grouping.Rows.Add(Tuple.Create(i - EXCEL_SECOND_ROW, currentRow));
                    result.Add(grouping);
                }
                else
                {
                    GroupingRangeRow grouping = result[indexExists.Value];
                    grouping.Rows.Add(Tuple.Create(i - EXCEL_SECOND_ROW, currentRow));
                }
            }

            return result;
        }

        private int? retrieve_grouped_exist_list(IList<GroupingRangeRow> collections, string comparedValue)
        {
            for (var i = 0; i < collections.Count; i++)
            {
                GroupingRangeRow collection = collections[i];
                if (collection.Key == comparedValue)
                    return i;
            }

            return null;
        }

        private int? retrieve_grouped_exist_list(IList<GroupingRangeRowExcel> collections, string comparedValue)
        {
            for (var i = 0; i < collections.Count; i++)
            {
                GroupingRangeRowExcel collection = collections[i];
                if (collection.Key == comparedValue)
                    return i;
            }

            return null;
        }


        private object get_process_value(FTP1 propertyMapping, RangeColumn[] rowColumns, object basicObject, Type basicObjectType,
            Type gotPropertyInfoType)
        {
            switch (propertyMapping.TargetType)
            {
                case FTP1.TargetTypes.EXCEL_VALUE_CODE:
                    {
                        int index = retrieve_column_index(propertyMapping.ExcelColumnAddress);
                        string stringValue = rowColumns[index].StringValue;
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.DEFAULT_VALUE_CODE:
                    {
                        string stringValue = propertyMapping.DefaultValue;
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.QUERY_VALUE_CODE:
                    {
                        var queryExtractor = new QueryExtractor(propertyMapping.QueryValue);
                        string extract = queryExtractor.Extract();
                        var stringValue = string.Empty;

                        object[] parametersValue = queryExtractor.Parameters
                            .Select(t =>
                            {
                                if (t.Type == QueryParameter.Types.Field)
                                {
                                    PropertyInfo info = basicObjectType.GetProperty(t.Name, BindingFlags.Instance | BindingFlags.Public);
                                    return info.GetValue(basicObject)?.ToString() ?? string.Empty;
                                }

                                int columnIndex = retrieve_column_index(t.Name);
                                return rowColumns[columnIndex].StringValue;
                            })
                            .ToArray();

                        using (SafeRecordSet recordSet = _company.MakeSafeRecordSet())
                        {
                            recordSet.ExecuteQuery(string.Format(extract, parametersValue));
                            recordSet.ReadSingleRecord(x => stringValue = x.GetString(0));
                        }

                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.EXCEL_INTERMEDIATE_VALUE_CODE:
                    {
                        int index = retrieve_column_index(propertyMapping.ExcelColumnAddress);
                        string stringValue = _intermediaryMappingValues
                            .Single(t => t.SAPProperty == propertyMapping.SAPValue && t.EDIValue == rowColumns[index].StringValue)
                            .SAPValue;
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                default:
                    throw new Exception();
            }
        }

        private object get_process_value(FTP1 propertyMapping, IXLRow rowColumns, object basicObject, Type basicObjectType,
            Type gotPropertyInfoType)
        {
            switch (propertyMapping.TargetType)
            {
                case FTP1.TargetTypes.EXCEL_VALUE_CODE:
                    {
                        int index = retrieve_column_index(propertyMapping.ExcelColumnAddress,true);
                        string stringValue = rowColumns.Cell(index).Value.ToString();
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.DEFAULT_VALUE_CODE:
                    {
                        string stringValue = propertyMapping.DefaultValue;
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.QUERY_VALUE_CODE:
                    {
                        var queryExtractor = new QueryExtractor(propertyMapping.QueryValue);
                        string extract = queryExtractor.Extract();
                        var stringValue = string.Empty;

                        object[] parametersValue = queryExtractor.Parameters
                            .Select(t =>
                            {
                                if (t.Type == QueryParameter.Types.Field)
                                {
                                    PropertyInfo info = basicObjectType.GetProperty(t.Name, BindingFlags.Instance | BindingFlags.Public);
                                    return info.GetValue(basicObject)?.ToString() ?? string.Empty;
                                }

                                int columnIndex = retrieve_column_index(t.Name, true);
                                return rowColumns.Cell(columnIndex).Value.ToString();
                            })
                            .ToArray();

                        using (SafeRecordSet recordSet = _company.MakeSafeRecordSet())
                        {
                            recordSet.ExecuteQuery(string.Format(extract, parametersValue));
                            recordSet.ReadSingleRecord(x =>
                            {
                                try
                                {
                                    stringValue = x.GetString(0);
                                }
                                catch (SafeRecordSet.NotRecordFound)
                                {
                                    throw;
                                }
                                catch
                                {
                                    throw new Exception($"No se pudo procesar el query '{x.ExecutedQuery}'.");
                                }
                            });
                        }

                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                case FTP1.TargetTypes.EXCEL_INTERMEDIATE_VALUE_CODE:
                    {
                        int index = retrieve_column_index(propertyMapping.ExcelColumnAddress,true);
                        string stringValue = _intermediaryMappingValues
                            .Single(t => t.SAPProperty == propertyMapping.SAPValue && t.EDIValue == rowColumns.Cell(index).Value.ToString())
                            .SAPValue;
                        return Convert.ChangeType(stringValue, gotPropertyInfoType);
                    }
                default:
                    throw new Exception();
            }
        }
        private Type get_type_from_user_field(ref SAPbobsCOM.Field field)
        {
            switch (field.Type)
            {
                case SAPbobsCOM.BoFieldTypes.db_Alpha:
                    return typeof(string);
                case SAPbobsCOM.BoFieldTypes.db_Numeric:
                    return typeof(int);
                case SAPbobsCOM.BoFieldTypes.db_Float:
                    return typeof(double);
                default:
                    throw new Exception();
            }
        }

        private void process_header(ref SAPbobsCOM.Documents documents, RangeRow row, IEnumerable<FTP1> propertyMappings)
        {
            SAPbobsCOM.UserFields userFields = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;
            RangeColumn[] rowColumns = row.Columns;
            Type documentType = typeof(SAPbobsCOM.IDocuments);
            IOrderedEnumerable<FTP1> orderedMappings = propertyMappings
                .OrderBy(t =>
                {
                    if (!t.SAPValue.StartsWith("U_"))
                        return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 1 : 0;

                    return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 3 : 2;
                });

            foreach (FTP1 propertyMapping in orderedMappings)
            {
                PropertyInfo propertyInfo = documentType.GetProperty(propertyMapping.SAPValue, BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo == null)
                {
                    if (propertyMapping.SAPValue.StartsWith("U_"))
                    {
                        userFields = documents.UserFields;
                        fields = userFields.Fields;
                        field = fields.Item(propertyMapping.SAPValue);

                        Type userFieldType = get_type_from_user_field(ref field);
                        object objectValue = get_process_value(propertyMapping, rowColumns, documents, documentType, userFieldType);
                        field.Value = objectValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    object objectValue = get_process_value(propertyMapping, rowColumns, documents, documentType, propertyInfo.PropertyType);
                    propertyInfo.SetValue(documents, objectValue);
                }
            }

            GenericHelper.ReleaseCOMObjects(userFields, field, fields);
        }

        private void process_header(ref SAPbobsCOM.Documents documents, IXLRow row, IEnumerable<FTP1> propertyMappings)
        {
            SAPbobsCOM.UserFields userFields = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;
            //RangeColumn[] rowColumns = row.Columns;
            Type documentType = typeof(SAPbobsCOM.IDocuments);
            IOrderedEnumerable<FTP1> orderedMappings = propertyMappings
                .OrderBy(t =>
                {
                    if (!t.SAPValue.StartsWith("U_"))
                        return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 1 : 0;

                    return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 3 : 2;
                });

            foreach (FTP1 propertyMapping in orderedMappings)
            {
                PropertyInfo propertyInfo = documentType.GetProperty(propertyMapping.SAPValue, BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo == null)
                {
                    if (propertyMapping.SAPValue.StartsWith("U_"))
                    {
                        userFields = documents.UserFields;
                        fields = userFields.Fields;
                        field = fields.Item(propertyMapping.SAPValue);

                        Type userFieldType = get_type_from_user_field(ref field);
                        object objectValue = get_process_value(propertyMapping, row, documents, documentType, userFieldType);
                        field.Value = objectValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    try
                    {
                        object objectValue = get_process_value(propertyMapping, row, documents, documentType, propertyInfo.PropertyType);
                        propertyInfo.SetValue(documents, objectValue);
                    }
                    catch (Exception exception)
                    {
                        var exceptionMessage = $"[Error] Tipo: 'Cabecera', Tipo: '{propertyMapping.RetrieveTargetTypeDescription()}', Valor SAP: '{propertyMapping.SAPValue}', Columna: {propertyMapping.ExcelColumnAddress}-{propertyMapping.ExcelColumnName}. Msj: {exception.Message}";
                        if (exceptionMessage.Length > 254)
                        {
                            exceptionMessage = exceptionMessage.Substring(0, 250) + "...";
                        }
                        throw new Exception(exceptionMessage);
                    }
                }
            }

            GenericHelper.ReleaseCOMObjects(userFields, field, fields);
        }

        /*
         * [Val(TargetTypes.EXCEL_VALUE_CODE, TargetTypes.EXCEL_VALUE_DESCRIPTION)]
        [Val(TargetTypes.DEFAULT_VALUE_CODE, TargetTypes.DEFAULT_VALUE_DESCRIPTION)]
        [Val(TargetTypes.QUERY_VALUE_CODE, TargetTypes.QUERY_VALUE_DESCRIPTION)]
        [Val(TargetTypes.EXCEL_INTERMEDIATE_VALUE_CODE, TargetTypes.EXCEL_INTERMEDIATE_VALUE_DESCRIPTION)]
         *
         */

        private void process_detail(ref SAPbobsCOM.Document_Lines documentLines, RangeRow row, IEnumerable<FTP1> propertyMappings)
        {
            SAPbobsCOM.UserFields userFields = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;
            RangeColumn[] rowColumns = row.Columns;
            Type documentType = typeof(SAPbobsCOM.IDocument_Lines);
            IOrderedEnumerable<FTP1> orderedMappings = propertyMappings
                .OrderBy(t =>
                {
                    if (!t.SAPValue.StartsWith("U_"))
                        return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 1 : 0;

                    return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 3 : 2;
                });

            foreach (FTP1 propertyMapping in orderedMappings)
            {
                PropertyInfo propertyInfo = documentType.GetProperty(propertyMapping.SAPValue, BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo == null)
                {
                    if (propertyMapping.SAPValue.StartsWith("U_"))
                    {
                        userFields = documentLines.UserFields;
                        fields = userFields.Fields;
                        field = fields.Item(propertyMapping.SAPValue);

                        Type userFieldType = get_type_from_user_field(ref field);
                        object objectValue = get_process_value(propertyMapping, rowColumns, documentLines, documentType, userFieldType);
                        field.Value = objectValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    object objectValue = get_process_value(propertyMapping, rowColumns, documentLines, documentType,
                        propertyInfo.PropertyType);
                    propertyInfo.SetValue(documentLines, objectValue);
                }
            }

            GenericHelper.ReleaseCOMObjects(userFields, field, fields);
        }

        private void process_detail(ref SAPbobsCOM.Document_Lines documentLines, IXLRow row, IEnumerable<FTP1> propertyMappings)
        {
            SAPbobsCOM.UserFields userFields = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;
            //RangeColumn[] rowColumns = row.Columns;
            Type documentType = typeof(SAPbobsCOM.IDocument_Lines);
            IOrderedEnumerable<FTP1> orderedMappings = propertyMappings
                .OrderBy(t =>
                {
                    if (!t.SAPValue.StartsWith("U_"))
                        return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 1 : 0;

                    return t.TargetType == FTP1.TargetTypes.QUERY_VALUE_CODE ? 3 : 2;
                });

            foreach (FTP1 propertyMapping in orderedMappings)
            {
                PropertyInfo propertyInfo = documentType.GetProperty(propertyMapping.SAPValue, BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo == null)
                {
                    if (propertyMapping.SAPValue.StartsWith("U_"))
                    {
                        userFields = documentLines.UserFields;
                        fields = userFields.Fields;
                        field = fields.Item(propertyMapping.SAPValue);

                        Type userFieldType = get_type_from_user_field(ref field);
                        object objectValue = get_process_value(propertyMapping, row, documentLines, documentType, userFieldType);
                        field.Value = objectValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    object objectValue = get_process_value(propertyMapping, row, documentLines, documentType,
                        propertyInfo.PropertyType);
                    propertyInfo.SetValue(documentLines, objectValue);
                }
            }

            GenericHelper.ReleaseCOMObjects(userFields, field, fields);
        }

        private int retrieve_column_index(string columnAddress)
        {
            RangeColumn[] headerColumns = _headerRow.Columns;
            for (var i = 0; i < headerColumns.Length; i++)
            {
                RangeColumn headerColumn = headerColumns[i];
                if (headerColumn.RangeAddressAsString == columnAddress)
                    return i;
            }

            throw new Exception($"El código de columna '{columnAddress}' referenciado no existe.");
        }
        private int retrieve_column_index(string columnAddress, bool ce)
        {
            //RangeColumn[] headerColumns = _headerRow.Columns;
            //for (var i = 0; i < headerColumns.Length; i++)
            //{
            //    RangeColumn headerColumn = headerColumns[i];
            //    if (headerColumn.RangeAddressAsString == columnAddress)
            //        return i;
            //}
            int column = 0;

            foreach (IXLCell cell in _firstRow.Cells())
            {
                var x = Tuple.Create(cell.Address.ToString(), cell.Value.ToString());
                if (cell.Address.ToString() == columnAddress)
                {
                    column++;
                    return column;
                }
                column++;
            }


            throw new Exception($"El código de columna '{columnAddress}' referenciado no existe.");
        }

        [Serializable]
        public class GenerateResponse
        {
            public string WorkBookIdentifierReference { get; set; }
            public int LineIndex { get; set; }
            public bool Success { get; set; }
            public int DocumentEntry { get; set; }
            public int DocumentNumber { get; set; }
            public string ErrorMessage { get; set; }
            public string ArtCod { get; set; }
            public string Agrupador { get; set; }
            public int Index { get; set; }
        }

        private class GroupingRangeRow
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public GroupingRangeRow(string key)
            {
                Key = key;
                Rows = new List<Tuple<int, RangeRow>>();
            }

            public string Key { get; }
            public IList<Tuple<int, RangeRow>> Rows { get; }
        }

        private class GroupingRangeRowExcel
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public GroupingRangeRowExcel(string key)
            {
                Key = key;
                Rows = new List<Tuple<int, IXLRow>>();
            }

            public string Key { get; }
            public IList<Tuple<int, IXLRow>> Rows { get; }
        }
    }
}