// ReSharper disable StaticMemberInGenericType

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using VSVersionControl.FlagElements.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Newtonsoft.Json;
using DisposableSAPBO.RuntimeMapper.Attributes;
using System.Collections;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;


namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseSAPDocumentRepository<T, TK> : Code.Repository<T>
        where T : SAPDocument<TK>, new() where TK : SAPDocumentLine, new()
    {
        private const string SELECTED_BATCHES_QUERY =
            "select * from \"IBT1\" where \"BaseLinNum\" = {1} and \"BaseType\" = {2} and \"BaseEntry\" = {0}";

        private static readonly string DOCUMENT_QUERY = $"select * from \"{typeof(T).Name}\" {{0}}";

        private static readonly string DOCUMENT_QUERY_DRAFT = $"select * from \"ODRF\" {{0}}";

        private static readonly string DOCUMENT_LINE_QUERY = $"select * from \"{typeof(TK).Name}\" where \"DocEntry\" = {{0}} order by \"LineNum\"";

        private static readonly string DOCUMENT_LINE_QUERY_DRAFT = $"select * from \"DRF1\" where \"DocEntry\" = {{0}} order by \"LineNum\"";

        protected BaseSAPDocumentRepository(Company company)
            : base(company)
        {
        }

        BaseOITMRepository bs;

     
        public Tuple<bool, string> RetrieveAnexoDocument(int referenceDocumentEntryInvoice)
        {
            try
            {
                var query = $@"select A.* from ""ATC1"" A Join OINV O on A.""AbsEntry""=O.""AtcEntry"" where  O.""DocEntry"" = '{referenceDocumentEntryInvoice}'; ";

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                recordSet.DoQuery(query);
                var url = "";
                while (!recordSet.EoF)
                {
                    url = recordSet.GetColumnValue("srcPath").ToString();
                    recordSet.MoveNext();
                }

                return Tuple.Create(true, url);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

        }

        public Tuple<bool, string> RetrieveAnexoDocumentDelivery(int referenceDocumentEntry)
        {
            try
            {
                var query = $@"select A.* from ""ATC1"" A Join ODLN O on A.""AbsEntry""=O.""AtcEntry"" where  O.""DocEntry"" = '{referenceDocumentEntry}'; ";

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                recordSet.DoQuery(query);
                var url = "";
                while (!recordSet.EoF)
                {
                    url = recordSet.GetColumnValue("srcPath").ToString();
                    recordSet.MoveNext();
                }

                return Tuple.Create(true, url);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

        }

        private void UpdateNumLegal(int DocEntry, string SerieControl, string correlativo)
        {
            var query = $@"SELECT IFNULL(max(""LineId""),0) + 1   FROM ""@BPVSN_NROLEGL"" WHERE ""Code"" = '{SerieControl}'; ";
            var LineNum = 0;
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                LineNum = recordSet.GetColumnValue("Line").ToString().ToInt32();
                recordSet.MoveNext();
            }

            query = $@"INSERT INTO ""@BPVSN_NROLEGL""( 
                    ""Code"", ""LineId"", ""U_BP_OBJSAP"", ""U_BP_OBJDES"", ""U_BP_DE_SAP"", ""U_BP_NROLEG"",  ""Object"",)
		            VALUES(
		            '{SerieControl}',
		            {LineNum},
		            15,
                    'Entrega' ,
                    {DocEntry},
                    {correlativo},
                    'BPVSN_NROLEG'";

            recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            query = $@"DO
                                BEGIN
	                                DECLARE CORREL NVARCHAR(20);
                                CALL ""bpvs_AX_ObtenerSiguienteCorrelativo""('{SerieControl}',CORREL);	
                                select CORREL as ""Correlativo"" from dummy;
                                                    END; ";
            var nuevoNum = "";
            recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);
            while (!recordSet.EoF)
            {
                nuevoNum = recordSet.GetColumnValue("Correlativo").ToString();
                recordSet.MoveNext();
            }

            query = $@"UPDATE ""@BPVSN_NROLEGC""
                            SET ""U_BP_NROACT"" = '{nuevoNum}'
                            WHERE ""Code"" = '{SerieControl}'";

            recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);
        }

        public virtual Tuple<bool, string, string> RegisterDocumentCreditNote(T document, DocumentObjectTypeEnum obj, string FlowSource)
        {
            string DocEntry = "", numeracion = "";
            //try
            //{
            //    Items s = (SAPbobsCOM.Items)Company.GetBusinessObject(BoObjectTypes.oItems);
            //    Documents documentValid;

            //    documentValid = (SAPbobsCOM.Documents)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);


            //    documentValid.DocDate = document.DocumentDate;
            //    documentValid.DocDueDate = document.DocumentDeliveryDate;
            //    documentValid.TaxDate = document.DocumentDate;
            //    documentValid.CardCode = document.CardCode;
            //    documentValid.JournalMemo = documentValid.CardName = document.CardName;
            //    int cont = 0;
            //    //documentValid.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_SalesOrder;
            //    //documentValid.DocumentReferences.ReferencedDocEntry = document.DocumentLines.FirstOrDefault().BaseEntry;
            //    //documentValid.DocumentReferences.Add();
            //    //var lines= document.DocumentLines.GroupBy(t => t.ItemCode).Select;

            //    int docentryRef = 0;
            //    BaseOPDSRepository _settings = new UnitOfWork(Company).SettingsRepository;

            //    foreach (var item in document.DocumentLines)
            //    {
            //        s.GetByKey(item.ItemCode);

            //        documentValid.Lines.ItemCode = item.ItemCode;
            //        //documentValid.Lines.AccountCode = "7011101";
            //        documentValid.Lines.BaseEntry = docentryRef = item.BaseEntry;
            //        documentValid.Lines.BaseLine = item.BaseLine;
            //        documentValid.Lines.BaseType = 234000031; //SAPbobsCOM.BoObjectTypes.oReturnRequest.g;
            //        documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);

            //        documentValid.Lines.WarehouseCode = _settings.Setting(OPDS.Codes.TEMPORAL_WAREHOUSE).Value;
            //        documentValid.Lines.UnitPrice = Decimal.ToDouble(item.UnitPrice);
            //        documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "01";
            //        documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "10";

            //        int contB = 0;

            //        foreach (var batch in item.SelectedBatches)
            //        {
            //            documentValid.Lines.BatchNumbers.BatchNumber = batch.BatchNumber;
            //            //documentValid.Lines.BatchNumbers.ItemCode = item.ItemCode;
            //            documentValid.Lines.BatchNumbers.Quantity = Decimal.ToDouble(batch.Quantity); // * s.SalesItemsPerUnit;
            //            //documentValid.Lines.BatchNumbers.BaseLineNumber = cont;
            //            //documentValid.Lines.BatchNumbers.SetCurrentLine(contB);
            //            documentValid.Lines.BatchNumbers.Add();
            //        }

            //        contB++;

            //        cont++;
            //        documentValid.Lines.Add();
            //    }
            //    var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //    string query = $@"select top 1 TO_VARCHAR(T1.""DocDate"",'YYYYMMDD') as ""DocDate"" , T1.""DocEntry"",T1.""DocSubType"",T1.""U_BPP_MDSD"",T1.""U_BPV_NCON2"" ,
            //                    case when ""Currency"" ='USD' then T1.""DocTotalFC"" else T1.""DocTotal"" end as ""DocTotal""
            //                    from RRR1 T0 JOIN OINV T1 on T1.""DocEntry"" = T0.""BaseEntry""
            //                    where T0.""DocEntry""={docentryRef}";
            //    recordSet.DoQuery(query);

            //    string tipo = "";
            //    string corrRef = "";
            //    string DocTotalRef = "";
            //    string SerieRef = "";
            //    string FechaRef = "";
            //    while (!recordSet.EoF)
            //    {
            //        FechaRef = recordSet.Fields.Item("DocDate").Value.ToString();
            //        tipo = recordSet.Fields.Item("DocSubType").Value.ToString();
            //        corrRef = recordSet.Fields.Item("U_BPV_NCON2").Value.ToString();
            //        DocTotalRef = recordSet.Fields.Item("DocTotal").Value.ToString();
            //        SerieRef = recordSet.Fields.Item("U_BPP_MDSD").Value.ToString();
            //        docentryRef = recordSet.Fields.Item("DocEntry").Value.ToInt32();
            //        recordSet.MoveNext();
            //    }
            //    ///
            //    if (tipo == "--")
            //    {
            //        documentValid.UserFields.Fields.Item("U_VS_TDOCORG").Value = "13";
            //        documentValid.UserFields.Fields.Item("U_BPP_MDTO").Value = "01";

            //    }
            //    else
            //    {
            //        documentValid.UserFields.Fields.Item("U_BPP_MDTO").Value = "03";
            //    }
            //    documentValid.UserFields.Fields.Item("U_VS_DOCORG").Value = docentryRef;
            //    documentValid.UserFields.Fields.Item("U_BPP_MDSO").Value = SerieRef;
            //    documentValid.UserFields.Fields.Item("U_BPP_MDCO").Value = corrRef;
            //    documentValid.UserFields.Fields.Item("U_VS_SDOCTOTAL").Value = DocTotalRef;
            //    documentValid.UserFields.Fields.Item("U_BPP_SDOCDATE").Value = FechaRef;

            //    documentValid.UserFields.Fields.Item("U_VS_MOTEMI").Value = "07";
            //    documentValid.UserFields.Fields.Item("U_VS_SUSTNT").Value = "Devolución por ítem";

            //    //
            //    //if (document.CardCode.Length == 10)
            //    //{

            //    recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //    query = $"CALL \"bpvs_LC_BF_OP_VENTAS\"('14','39','N','{document.SerieControl}','P','','','')";
            //    recordSet.DoQuery(query);

            //    string correlativo = "";
            //    while (!recordSet.EoF)
            //    {
            //        correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
            //        documentValid.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
            //        //documents.Add(document);
            //        recordSet.MoveNext();
            //    }

            //    documentValid.UserFields.Fields.Item("U_BPV_SERI").Value = document.SerieControl;

            //    //asdas
            //    //documentValid.NumAtCard = numeracion = "09GD01-" + correlativo;
            //    var splitserie = document.SerieControl.Split("-");
            //    documentValid.NumAtCard = numeracion = splitserie[1] + splitserie[0] + "-" + correlativo;

            //    documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "N";
            //    documentValid.UserFields.Fields.Item("U_BPP_MDTD").Value = "07";

            //    int res = documentValid.Add();
            //    if (res != 0) // Check the result
            //    {
            //        string error;
            //        string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
            //        return Tuple.Create(false, vm_GetLastErrorDescription_string, "");
            //        //Company.GetLastError(out res, out error);                 
            //    }
            //    else
            //    {
            //        DocEntry = Company.GetNewObjectKey();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Tuple.Create(false, ex.Message, "");
            //}

            return Tuple.Create(true, DocEntry, numeracion);
        }

        public virtual Tuple<bool, string, string> RegisterDocumentReturn(T document, DocumentObjectTypeEnum obj, string FlowSource)
        {
            string DocEntry = "", numeracion = "";
            try
            {
                Items s = (SAPbobsCOM.Items)Company.GetBusinessObject(BoObjectTypes.oItems);
                Documents documentValid;

                documentValid = (SAPbobsCOM.Documents)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns);


                documentValid.DocDate = document.DocumentDate;
                documentValid.DocDueDate = document.DocumentDeliveryDate;
                documentValid.TaxDate = document.DocumentDate;
                documentValid.CardCode = document.CardCode;
                documentValid.JournalMemo = documentValid.CardName = document.CardName;
                int cont = 0;
                //documentValid.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_SalesOrder;
                //documentValid.DocumentReferences.ReferencedDocEntry = document.DocumentLines.FirstOrDefault().BaseEntry;
                //documentValid.DocumentReferences.Add();
                //var lines= document.DocumentLines.GroupBy(t => t.ItemCode).Select;
                BaseOPDSRepository _settings = new UnitOfWork(Company).SettingsRepository;

                foreach (var item in document.DocumentLines)
                {
                    s.GetByKey(item.ItemCode);

                    documentValid.Lines.ItemCode = item.ItemCode;
                    //documentValid.Lines.AccountCode = "7011101";
                    documentValid.Lines.BaseEntry = item.BaseEntry;
                    documentValid.Lines.BaseLine = item.BaseLine;
                    documentValid.Lines.BaseType = 234000031; //SAPbobsCOM.BoObjectTypes.oReturnRequest.g;
                    documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);

                    documentValid.Lines.WarehouseCode = _settings.Setting(OPDS.Codes.TEMPORAL_WAREHOUSE).Value;
                    documentValid.Lines.UnitPrice = Decimal.ToDouble(item.UnitPrice);
                    documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "01";
                    documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "10";

                    int contB = 0;

                    foreach (var batch in item.SelectedBatches)
                    {
                        documentValid.Lines.BatchNumbers.BatchNumber = batch.BatchNumber;
                        //documentValid.Lines.BatchNumbers.ItemCode = item.ItemCode;
                        documentValid.Lines.BatchNumbers.Quantity = Decimal.ToDouble(batch.Quantity) * s.SalesItemsPerUnit;
                        //documentValid.Lines.BatchNumbers.BaseLineNumber = cont;
                        //documentValid.Lines.BatchNumbers.SetCurrentLine(contB);
                        documentValid.Lines.BatchNumbers.Add();
                    }

                    contB++;

                    cont++;
                    documentValid.Lines.Add();
                }


                ///


                //
                //if (document.CardCode.Length == 10)
                //{

                //var recordSet = ((SAPbobsCOM.Recordset) (Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                //string query = $"CALL \"bpvs_LC_BF_OP_VENTAS\"('16','16','N','{document.SerieControl}','P','','','')";
                //recordSet.DoQuery(query);

                //string correlativo = "";
                //while (!recordSet.EoF)
                //{
                //    correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
                //    //documentValid.Lines.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
                //    //documents.Add(document);
                //    recordSet.MoveNext();
                //}
                var _serie = get_document_series();
                var correlativo = get_correlative(_serie);
                documentValid.UserFields.Fields.Item("U_BPV_SERI").Value = _serie;//document.SerieControl;
                documentValid.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
                documentValid.UserFields.Fields.Item("U_BPP_MDTD").Value = "09";
                var splitserie = _serie.Split("-");
                documentValid.NumAtCard = "09" + splitserie[0] + "-" + correlativo;
                //asdas
                //documentValid.NumAtCard = numeracion = "09GD01-" + correlativo;
                //var splitserie = document.SerieControl.Split("-");
                //documentValid.NumAtCard = numeracion = splitserie[1] + splitserie[0] + "-" + correlativo;

                documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "N";


                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    return Tuple.Create(false, vm_GetLastErrorDescription_string, "");
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message, "");
            }

            return Tuple.Create(true, DocEntry, numeracion);
        }
        private string get_document_series()
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery("select nlc.\"Code\" from \"@BPVSN_NROLEGC\" as nlc left join \"@BPVSN_NROLEGO\" as nlo on nlo.\"Code\" = nlc.\"Code\" where nlo.\"U_BP_FRMSAP\" = '180'");
            return recordSet.GetColumnValue(0).ToString();
        }

        private string get_correlative(string series)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery($"select T0.\"U_BP_NROACT\" from \"@BPVSN_NROLEGC\" as T0 left join \"@BPVSN_NROLEGO\" as T1 on T1.\"Code\" = T0.\"Code\" where T1.\"U_BP_FRMSAP\" = '180' and T0.\"Code\" = '{series}'");
            return recordSet.GetColumnValue(0).ToString();

        }

        public virtual object GetDocument(string docentry)
        {
            ODLN x = new ODLN();

            return x;
        }

        public virtual Tuple<bool, string> RegisterDocument(T document)
        {
            return Tuple.Create(true, "");
        }

        public virtual Tuple<bool, string> RegisterDocumentDraft(T document)
        {
            return Tuple.Create(true, "");
        }

        public virtual Tuple<bool, string> RegisterDocumentPurchaseOrder(T document)
        {
            string DocEntry = "", numeracion = "";
            try
            {
                Documents documentValid;
                documentValid = (SAPbobsCOM.Documents)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

                documentValid.DocDate = document.DocumentDate;
                documentValid.DocDueDate = document.DocumentDeliveryDate;
                documentValid.TaxDate = document.DocumentDate;
                documentValid.CardCode = document.CardCode;
                documentValid.JournalMemo = document.CardName;
                documentValid.UserFields.Fields.Item("U_VS_AFEDET").Value = "N";
                int cont = 0;
                documentValid.DocType = BoDocumentTypes.dDocument_Service;
                foreach (var item in document.DocumentLines)
                {
                    //documentValid.Lines.ItemCode = item.ItemCode;
                    documentValid.Lines.ItemDescription = item.ItemDescription;
                    //documentValid.Lines.BaseEntry = deliveryEntry;
                    //documentValid.Lines.BaseLine = cont;
                    documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);
                    //documentValid.Lines.BaseType = 15;
                    documentValid.Lines.UnitPrice = Decimal.ToDouble(item.UnitPrice);
                    documentValid.Lines.LineTotal = Decimal.ToDouble(item.TotalPrice);
                    documentValid.Lines.UserFields.Fields.Item("U_BPP_OPER").Value = "E";
                    documentValid.Lines.AccountCode = "6311101";
                    documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "99";
                    documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "10";
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_ROUT").Value = item.routeCode;
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_PENL").Value = decimal.ToDouble(item.ajust);
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_AJUT").Value = decimal.ToDouble(item.penalidad);
                    documentValid.Lines.TaxCode = item.TaxCode;
                    documentValid.Lines.Add();
                    cont++;
                }

                documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "N";

                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    return Tuple.Create(false, vm_GetLastErrorDescription_string);
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

            return Tuple.Create(true, "");
        }

        public virtual Tuple<bool, string> SimpleRegisterDocumentRequestPurchaseOrder(T document)
        {
            Documents purchaseRequest = null;
            Document_Lines documentLines = null;
            UserFields userFields = null;
            Fields fields = null;
            Field field = null;

            try
            {
                purchaseRequest = Company.GetBusinessObject(BoObjectTypes.oPurchaseRequest)
                    .To<Documents>();

                purchaseRequest.DocType = BoDocumentTypes.dDocument_Items;
                purchaseRequest.DocDate = document.DocumentDate;
                purchaseRequest.TaxDate = document.DocumentDate;
                purchaseRequest.RequriedDate = document.DocumentDate;
                purchaseRequest.DocDueDate = document.DocumentDeliveryDate;
                purchaseRequest.Comments = document.Comments;
                userFields = purchaseRequest.UserFields;
                fields = userFields.Fields;
                field = fields.Item("U_VS_AFEDET");
                field.Value = "N";
                field = fields.Item("U_VS_USRSV");
                field.Value = "N";

                documentLines = purchaseRequest.Lines;
                foreach (var line in document.DocumentLines)
                {
                    PRQ1 item = (PRQ1)(object)line;
                    documentLines.ItemCode = item.ItemCode;
                    documentLines.LineVendor = item.ProviderCode;
                    documentLines.RequiredDate = document.DocumentDate;
                    documentLines.Quantity = decimal.ToDouble(item.Quantity);
                    documentLines.LineTotal = decimal.ToDouble(item.TotalPrice);

                    userFields = documentLines.UserFields;
                    fields = userFields.Fields;
                    field = fields.Item("U_BPP_OPER");
                    field.Value = "E";
                    field = fields.Item("U_tipoOpT12");
                    field.Value = "99";
                    field = fields.Item("U_VS_TIPAFE");
                    field.Value = "10";
                    documentLines.Add();
                }

                if (purchaseRequest.Add() != 0)
                    return Tuple.Create(false, Company.GetLastErrorDescription());

                string entry = Company.GetNewObjectKey();
                return Tuple.Create(true, entry);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(field, fields, userFields, documentLines, purchaseRequest);
            }
        }

        public virtual Tuple<bool, string> RegisterDocumentRequestPurchaseOrder(T document)
        {
            string DocEntry = "", numeracion = "";
            try
            {
                Documents documentValid;
                documentValid = (SAPbobsCOM.Documents)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);

                documentValid.DocDate = document.DocumentDate;
                documentValid.DocDueDate = document.DocumentDeliveryDate;
                documentValid.TaxDate = document.DocumentDate;
                documentValid.RequriedDate = document.DocumentDate;
                //documentValid.CardCode = document.CardCode;
                documentValid.JournalMemo = document.CardName;
                documentValid.UserFields.Fields.Item("U_VS_AFEDET").Value = "N";
                int cont = 0;
                documentValid.DocType = BoDocumentTypes.dDocument_Items;
                foreach (var item in document.DocumentLines)
                {
                    documentValid.Lines.ItemCode = item.ItemCode;
                    //documentValid.Lines.ItemDescription = item.ItemDescription;
                    //documentValid.Lines.BaseEntry = deliveryEntry;
                    //documentValid.Lines.BaseLine = cont;
                    documentValid.Lines.LineVendor = document.CardCode;
                    documentValid.Lines.RequiredDate = document.DocumentDate;
                    documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);
                    //documentValid.Lines.BaseType = 15;
                    documentValid.Lines.UnitPrice = Decimal.ToDouble(item.UnitPrice);
                    documentValid.Lines.LineTotal = Decimal.ToDouble(item.TotalPrice);
                    documentValid.Lines.UserFields.Fields.Item("U_BPP_OPER").Value = "E";
                    //documentValid.Lines.AccountCode = "6311101";
                    documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = "99";
                    documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "10";
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_ROUT").Value = item.routeCode;
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_PENL").Value = decimal.ToDouble(item.ajust);
                    //documentValid.Lines.UserFields.Fields.Item("U_EXK_AJUT").Value = decimal.ToDouble(item.penalidad);
                    documentValid.Lines.TaxCode = item.TaxCode;
                    documentValid.Lines.Add();
                    cont++;
                }

                documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "N";

                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    return Tuple.Create(false, vm_GetLastErrorDescription_string);
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

            return Tuple.Create(true, "");
        }

        public class _Anticipo
        {
            public int DocEntry { get; set; }
            public double Monto { get; set; }
            public int Order { get; set; }
        }

        public virtual Tuple<bool, string, string> RegisterDocumentInvoice(T document, int deliveryEntry, bool serieFija = false)
        {
            string DocEntry = "", numeracion = "";
            //try
            //{
            //    var documentValid = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices).To<SAPbobsCOM.Documents>();
            //    documentValid.DocDate = document.DocumentDate;
            //    //documentValid.DocDueDate = document.DocumentDeliveryDate;
            //    documentValid.TaxDate = document.TaxDate;
            //    documentValid.CardCode = document.CardCode;
            //    documentValid.JournalMemo = document.CardName;
            //    documentValid.CardName = document.CardName;

            //    Fields userFields = documentValid.UserFields.Fields;
            //    userFields.Item(@"U_VS_AFEDET").Value = "N";
            //    userFields.Item(@"U_VS_USRSV").Value = "N";

            //    userFields.Item(@"U_VS_NRO_GR").Value = document.NumberAtCard.Substring(2);

            //    documentValid.UserFields.Fields.Item("U_CL_CANAL").Value = document.SaleChannel;
            //    documentValid.UserFields.Fields.Item("U_CL_TERRIT").Value = document.Region;

            //    BaseOPDSRepository _settings = new UnitOfWork(Company).SettingsRepository;
            //    documentValid.UserFields.Fields.Item("U_VS_FESTAT").Value = _settings.Setting(OPDS.Codes.SEND_SUNAT).Value;

            //    //documentValid.UserFields.Fields.Item("U_VS_FESTAT").Value = "I";


            //    BaseSAPDocumentRepository<ODLN, DLN1> _guiaRepo = new UnitOfWork(Company).DeliveryRepository;
            //    var _guia = _guiaRepo.RetrieveDocuments(t => t.DocumentEntry == deliveryEntry).FirstOrDefault();


            //    documentValid.UserFields.Fields.Item("U_VS_GRATEN").Value = _guia.TituloGratuito;
            //    int cont = 0;
            //    foreach (var item in document.DocumentLines)
            //    {
            //        documentValid.Lines.ItemCode = item.ItemCode;
            //        documentValid.Lines.BaseEntry = deliveryEntry;
            //        documentValid.Lines.BaseLine = cont;
            //        documentValid.Lines.Quantity = Decimal.ToDouble(item.Quantity);
            //        documentValid.Lines.BaseType = 15;

            //        var lineGuia = _guia.DocumentLines.Where(t => t.ItemCode == item.ItemCode).FirstOrDefault();
            //        documentValid.Lines.UserFields.Fields.Item("U_tipoOpT12").Value = lineGuia.TipoOperacion;// "01";
            //        documentValid.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = lineGuia.TipoAfectacion;//"10";
            //        documentValid.Lines.UserFields.Fields.Item("U_BPP_OPER").Value = lineGuia.TipoOperacionExoINA;// "10";
            //        documentValid.Lines.UserFields.Fields.Item("U_VS_ONEROSO").Value = lineGuia.Oneroso;// "10";
            //        documentValid.Lines.TaxOnly = lineGuia.SoloImpuesto == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

            //        documentValid.Lines.Add();
            //        cont++;
            //    }

            //    var recordSetDown = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //    //string queryD =
            //    //    $"select \"DocEntry\",\"DocTotal\" -\"VatSum\" as \"Total\" , \"DocTotal\" -\"VatSum\" -\"DpmAppl\"  as \"Saldo\" from ODPI where \"CardCode\"='{document.CardCode}' and \"DocStatus\"='C' " +
            //    //    $"and \"DocTotal\" -\"VatSum\" -\"DpmAppl\" >0 order by \"DocDate\" asc ";

            //    string queryD = $@" select Distinct
            //    OD.""DocEntry"",
            //    OD.""DocTotal"" - OD.""VatSum"" as ""Total"" , 
            //    OD.""DocTotal"" - OD.""VatSum"" - OD.""DpmAppl"" as ""Saldo""
            //    from ""DLN21"" D21            
            //    JOIN ""RDR1"" V1 ON D21.""RefDocEntr"" = V1.""DocEntry""
            //    JOIN ""OQUT"" OQ ON OQ.""DocEntry"" = V1.""BaseEntry""
            //    JOIN ""DPI1"" D1 ON OQ.""DocEntry"" = D1.""BaseEntry""
            //    JOIN ""ODPI"" OD ON OD.""DocEntry"" = D1.""DocEntry""
            //    WHERE D21.""DocEntry"" = {deliveryEntry}
            //    and OD.""DocStatus"" = 'C' 
            //    and OD.""DocTotal"" - OD.""VatSum"" - OD.""DpmAppl"" > 0 
            //    ";
            //    recordSetDown.DoQuery(queryD);

            //    List<_Anticipo> anticipos = new List<_Anticipo>();
            //    int contAnt = 1;
            //    while (!recordSetDown.EoF)
            //    {
            //        _Anticipo ant = new _Anticipo();
            //        ant.DocEntry = int.Parse(recordSetDown.Fields.Item("DocEntry").Value.ToString());
            //        ant.Monto = double.Parse(recordSetDown.Fields.Item("Saldo").Value
            //            .ToString()); //  Double.Parse(recordSetDown.Fields.Item("Total").Value.ToString());
            //        ant.Order = contAnt;

            //        anticipos.Add(ant);
            //        contAnt++;
            //        recordSetDown.MoveNext();
            //    }

            //    if (anticipos.Count == 0)
            //    {

            //        queryD = $@" select Distinct
            //            OD.""DocEntry"",
            //            OD.""DocTotal"" - OD.""VatSum"" as ""Total"" , 
            //            OD.""DocTotal"" - OD.""VatSum"" - OD.""DpmAppl"" as ""Saldo""
            //            from ""DLN21"" D21            
            //            JOIN ""ORDR"" V1 ON D21.""RefDocEntr"" = V1.""DocEntry""
            //            JOIN ""DPI1"" D1 ON V1.""DocEntry"" = D1.""BaseEntry""
            //            JOIN ""ODPI"" OD ON OD.""DocEntry"" = D1.""DocEntry""
            //            WHERE D21.""DocEntry"" = {deliveryEntry}
            //            and OD.""DocStatus"" = 'C' 
            //            and OD.""DocTotal"" - OD.""VatSum"" - OD.""DpmAppl"" > 0 
            //            ";
            //        recordSetDown.DoQuery(queryD);



            //        while (!recordSetDown.EoF)
            //        {
            //            _Anticipo ant = new _Anticipo();
            //            ant.DocEntry = int.Parse(recordSetDown.Fields.Item("DocEntry").Value.ToString());
            //            ant.Monto = double.Parse(recordSetDown.Fields.Item("Saldo").Value
            //                .ToString()); //  Double.Parse(recordSetDown.Fields.Item("Total").Value.ToString());
            //            ant.Order = contAnt;

            //            anticipos.Add(ant);
            //            contAnt++;
            //            recordSetDown.MoveNext();
            //        }
            //    }

            //    if (anticipos.Count > 0)
            //    {
            //        anticipos = anticipos.OrderBy(t => t.Order).ToList();
            //        var _totalDocument = document.TotalPrice - document.Impuesto;

            //        if (anticipos.Sum(t => t.Monto) > 0)
            //        {
            //            if (_totalDocument >= anticipos.Sum(t => t.Monto).ToDecimal())
            //            {
            //                foreach (var item in anticipos)
            //                {
            //                    documentValid.DownPaymentsToDraw.DocEntry = item.DocEntry;
            //                    documentValid.DownPaymentsToDraw.AmountToDraw = item.Monto;
            //                    //if (anticipos.Count > 1)
            //                    documentValid.DownPaymentsToDraw.Add();
            //                }
            //            }
            //            else
            //            {
            //                foreach (var item in anticipos)
            //                {
            //                    if (_totalDocument < item.Monto.ToDecimal())
            //                    {
            //                        documentValid.DownPaymentsToDraw.DocEntry = item.DocEntry;
            //                        documentValid.DownPaymentsToDraw.AmountToDraw = _totalDocument.ToDouble();
            //                        //if (anticipos.Count > 1)
            //                        documentValid.DownPaymentsToDraw.Add();
            //                        break;
            //                    }
            //                    else
            //                    {
            //                        documentValid.DownPaymentsToDraw.DocEntry = item.DocEntry;
            //                        documentValid.DownPaymentsToDraw.AmountToDraw = item.Monto;
            //                        //if (anticipos.Count > 1)
            //                        documentValid.DownPaymentsToDraw.Add();
            //                        _totalDocument = _totalDocument - item.Monto.ToDecimal();
            //                    }
            //                }
            //            }
            //        }
            //    }



            //    //documentValid.DownPaymentsToDraw.DocEntry = int.Parse(recordSetDown.Fields.Item("DocEntry").Value.ToString());
            //    //documentValid.DownPaymentsToDraw.AmountToDraw = decimal.ToDouble(document.TotalPrice - document.Impuesto);  //  Double.Parse(recordSetDown.Fields.Item("Total").Value.ToString());
            //    //documentValid.DownPaymentsToDraw.Add();

            //    #region set number card
            //    if (serieFija)
            //    {
            //        documentValid.UserFields.Fields.Item("U_VS_USRSV").Value = "Y";
            //        documentValid.NumAtCard = numeracion = document.SerieControl;
            //        var splitserie = document.SerieControl.Split("-");
            //        var serie_ = splitserie[0].Substring(2, 4) + "-01";
            //        documentValid.UserFields.Fields.Item("U_BPV_SERI").Value = serie_;
            //        documentValid.UserFields.Fields.Item("U_BPV_NCON2").Value = splitserie[1];
            //    }
            //    else
            //    {
            //        if (document.CardCode.Length == 10)
            //        {
            //            //documentValid.DocumentSubType = BoDocumentSubType.bod_Bill;
            //            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //            string query = $"CALL \"bpvs_LC_BF_OP_VENTAS\"('13','25','N','{document.SerieControl}','P','','','')";
            //            recordSet.DoQuery(query);

            //            userFields.Item("U_BPV_SERI").Value = document.SerieControl; // document.SerieVenta+"-03";
            //            string correlativo = "";
            //            while (!recordSet.EoF)
            //            {
            //                correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
            //                userFields.Item("U_BPV_NCON2").Value = correlativo;
            //                //documents.Add(document);
            //                recordSet.MoveNext();
            //            }

            //            var splitserie = document.SerieControl.Split("-");
            //            documentValid.NumAtCard = numeracion = splitserie[1] + splitserie[0] + "-" + correlativo;
            //            //documentValid.NumAtCard = numeracion = "03"+ document.SerieVenta+" - " + correlativo;
            //        }
            //        else
            //        {
            //            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //            string query = $"CALL \"bpvs_LC_BF_OP_VENTAS\"('13','25','N','{document.SerieControl}','P','','','')";
            //            recordSet.DoQuery(query);

            //            userFields.Item("U_BPV_SERI").Value = document.SerieControl; // --document.SerieVenta+"-01";
            //            string correlativo = "";
            //            while (!recordSet.EoF)
            //            {
            //                correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
            //                userFields.Item("U_BPV_NCON2").Value = correlativo;
            //                //documents.Add(document);
            //                recordSet.MoveNext();
            //            }

            //            var splitserie = document.SerieControl.Split("-");
            //            documentValid.NumAtCard = numeracion = splitserie[1] + splitserie[0] + "-" + correlativo;
            //            //documentValid.NumAtCard = numeracion = "01"+ document.SerieVenta+"-" + correlativo;
            //        }

            //    }

            //    #endregion

            //    bool valRetencion = false;


            //    #region retencion

            //    BaseInfrastructureRepository _infra = new UnitOfWork(Company).InfrastructureRepository;
            //    BaseBusinessPartnerRepository _bPRepository = new UnitOfWork(Company).BusinessPartnerRepository;
            //    var _condicionPago = _infra.RetrievePaymentGroupNumDescription(document.CondicionPago).ToString();
            //    var _extraFechas = _infra.RetrievePaymentGroupNumDetail(document.CondicionPago);

            //    if (_condicionPago.Contains("RETENCION") || _condicionPago.Contains("Retención") || _condicionPago.Contains("retencion") || _condicionPago.Contains("RETENCIÓN"))
            //    {
            //        documentValid.UserFields.Fields.Item("U_VS_FESTAT").Value = "I";
            //        valRetencion = true;
            //    }
            //    // documentValid.Installments.Add()

            //    if (!valRetencion)
            //    {
            //        if (document.TotalPrice >= 700)
            //        {
            //            _bPRepository.SetRetrieveFormat(Code.RetrieveFormat.Complete);
            //            var _bp = _bPRepository.FindByCode(document.CardCode);
            //            if (_bp.SujetoRetencion == "Y")
            //            {
            //                for (int i = 0; i < 2; i++)
            //                {
            //                    if (i == 0)
            //                    {
            //                        documentValid.Installments.Percentage = 3;
            //                        documentValid.Installments.UserFields.Fields.Item("U_VS_TIPOCUOTA").Value = "R";
            //                        documentValid.Installments.DueDate = DateTime.Now.AddDays(_extraFechas.Item2).AddMonths(_extraFechas.Item1);
            //                        documentValid.Installments.Add();
            //                    }
            //                    else
            //                    {
            //                        documentValid.Installments.Percentage = 97;
            //                        documentValid.Installments.DueDate = DateTime.Now.AddDays(_extraFechas.Item2).AddMonths(_extraFechas.Item1);
            //                        documentValid.Installments.Add();
            //                    }

            //                }
            //            }


            //        }
            //    }



            //    #endregion

            //    int res = documentValid.Add();
            //    if (res != 0) // Check the result
            //    {
            //        string error;
            //        string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
            //        return Tuple.Create(false, vm_GetLastErrorDescription_string, "");
            //        //Company.GetLastError(out res, out error);                 
            //    }
            //    else
            //    {
            //        DocEntry = Company.GetNewObjectKey();

            //        if (valRetencion)
            //        {
            //            if (_condicionPago.Contains("RETENCION") || _condicionPago.Contains("Retención") || _condicionPago.Contains("retencion") || _condicionPago.Contains("RETENCIÓN"))
            //            {
            //                //BaseSAPDocumentRepository<OINV, INV1> _oinv= new UnitOfWork(Company).InvoiceRepository;
            //                //var _invoice=_oinv.RetrieveDocuments(t => t.DocumentEntry == DocEntry.ToInt32());
            //                var _FacturaRetencion = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices).To<SAPbobsCOM.Documents>();
            //                int entry = DocEntry.ToInt32();
            //                _FacturaRetencion.GetByKey(entry);
            //                if (_FacturaRetencion.DocTotal >= 700)//TODO dejar configurable
            //                {
            //                    var plazos = _FacturaRetencion.Installments;
            //                    for (int i = 0; i < plazos.Count; i++)
            //                    {
            //                        _FacturaRetencion.Installments.SetCurrentLine(i);
            //                        if (_FacturaRetencion.Installments.Percentage == 3)//TODO dejar configurable
            //                        {
            //                            _FacturaRetencion.Installments.UserFields.Fields.Item("U_VS_TIPOCUOTA").Value = "R"; //TODO dejar configurable
            //                        }
            //                    }
            //                }

            //                documentValid.UserFields.Fields.Item("U_VS_FESTAT").Value = _settings.Setting(OPDS.Codes.SEND_SUNAT).Value;

            //                int _res = _FacturaRetencion.Update();

            //                if (_res != 0)
            //                {
            //                    string error;
            //                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
            //                    return Tuple.Create(false, vm_GetLastErrorDescription_string, "");
            //                }

            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Tuple.Create(false, ex.Message, "");
            //}

            return Tuple.Create(true, DocEntry, numeracion);
        }


        public virtual IEnumerable<T> RetrieveDocuments(Expression<Func<T, bool>> expression)
        {
            string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            return documents_from_query(string.Format(DOCUMENT_QUERY, $"where {whereStatement}"));
        }

        public virtual IEnumerable<T> RetrieveDocumentsDraft(Expression<Func<T, bool>> expression)
        {
            string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            return documents_from_query_draft(string.Format(DOCUMENT_QUERY_DRAFT, $"where {whereStatement}"));
        }

        public virtual IEnumerable<RDR1> RetrieveDocumentsDetailByRoute(int codRoute)
        {
            string whereStatement =
                $"SELECT A1.\"DocEntry\" as \"DocEntryRuta\", R1.\"DocEntry\" as \"DocEntryOT\", OR.\"DocEntry\" as \"DocEntryOrder\",  "
                + $"RR1.* from \"@VS_PD_ARD1\" A1 join \"@VS_PD_RTR1\" R1 on R1.\"DocEntry\" = A1.\"U_EXK_CDTR\"  " +
                $"join ORDR OR on OR.\"DocEntry\" = R1.\"U_EXK_DCEN\" "
                + $"join RDR1 RR1 on RR1.\"DocEntry\" = OR.\"DocEntry\" "
                + $"where A1.\"DocEntry\" ={codRoute} "; //QueryHelper.ParseToHANAQuery("");
            return documents_from_query_detail(string.Format(whereStatement));
        }

        public  IEnumerable<T> documents_from_query(string query)
        {
            IList<T> documents = new List<T>();
            Type entityType = typeof(T);

            IEnumerable<PropertyInfo> entityProperties = entityType.GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null)
                .OrderBy(order_attributes)
                .ToList();
            //entityProperties = entityType.GetProperties()
            //   .Where(item => item.Name!= null)
            //   .ToList();
            if (!entityProperties.Any())
                throw new Exception("[ERROR] The SAP document is not setter correctly");

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                var document = (T)Activator.CreateInstance(entityType);
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.DocumentLines))
                    {
                        document.DocumentLines = retrieve_lines(document.DocumentEntry, document.DocumentNumber,
                            document.ObjectType);
                        continue;
                    }

                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        private IEnumerable<T> documents_from_query_draft(string query)
        {
            IList<T> documents = new List<T>();
            Type entityType = typeof(T);

            IEnumerable<PropertyInfo> entityProperties = entityType.GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null)
                .OrderBy(order_attributes)
                .ToList();
            //entityProperties = entityType.GetProperties()
            //   .Where(item => item.Name!= null)
            //   .ToList();
            if (!entityProperties.Any())
                throw new Exception("[ERROR] The SAP document is not setter correctly");

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                var document = (T)Activator.CreateInstance(entityType);
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.DocumentLines))
                    {
                        document.DocumentLines = retrieve_lines_draft(document.DocumentEntry, document.DocumentNumber,
                            document.ObjectType);
                        continue;
                    }

                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }
        private int order_attributes(PropertyInfo propertyInfo)
        {
            var sapColumnAttribute = propertyInfo.GetCustomAttribute<SAPColumnAttribute>();

            if (sapColumnAttribute.SystemField && !sapColumnAttribute.DetailLine)
                return 1;
            if (sapColumnAttribute.SystemField && sapColumnAttribute.DetailLine)
                return 2;
            if (!sapColumnAttribute.SystemField)
                return 3;
            throw new Exception($"[ERROR] The property '{propertyInfo.Name}' don't have set a 'SAPColumnAttribute'");
        }

        private string get_sap_column_name(MemberInfo propertyInfo)
        {
            var sapColumnAttribute = propertyInfo.GetCustomAttribute<SAPColumnAttribute>();
            var fieldNoRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
            return fieldNoRelatedAttribute == null ? sapColumnAttribute.Name : fieldNoRelatedAttribute.ColumnName;
        }

        private object try_get_sap_value(RecordsetEx recordSet, string sapColumnName, PropertyInfo propertyInfo)
        {
            try
            {
                object originalValue = recordSet.GetColumnValue(sapColumnName) ??
                                       propertyInfo.PropertyType.GetDefaultValue();
                return Convert.ChangeType(originalValue, propertyInfo.PropertyType);
            }
            catch
            {
                return propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
            }
        }

        private IEnumerable<RDR1> documents_from_query_detail(string query)
        {
            BaseInfrastructureRepository infrastructureRepository = new UnitOfWork(Company).InfrastructureRepository;
            IList<RDR1> documents = new List<RDR1>();
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(query);
            Type entityType = typeof(RDR1);
            IEnumerable<PropertyInfo> entityProperties = entityType.GetProperties()
                .OrderByDescending(item => item.GetCustomAttribute<SAPColumnAttribute>() != null ? 1 : 0)
                .ToList();
            while (!recordSet.EoF)
            {
                var document = (RDR1)Activator.CreateInstance(entityType);
                foreach (PropertyInfo property in entityProperties)
                {
                    var columnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
                    //if (columnAttribute == null && property.Name != nameof(document.DocumentLines))
                    //    continue;

                    //if (property.Name == nameof(document.DocumentLines))
                    //{
                    //    document.DocumentLines = retrieve_lines(document.DocumentEntry, document.DocumentNumber,
                    //        document.ObjectType);
                    //    continue;
                    //}

                    object parsedValue = null;

                    if (columnAttribute != null)
                    {
                        object originalValue = recordSet.GetColumnValue(columnAttribute.Name) ??
                                               property.PropertyType.GetDefaultValue();
                        parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                    }

                    var validValueDescription = property.GetCustomAttribute<SAPValidValueDescription>();
                    if (validValueDescription != null)
                    {
                        string sapField = validValueDescription.SAPField;
                        string originalValue = recordSet.GetColumnValue(sapField)?.ToString() ?? string.Empty;
                        parsedValue = originalValue.IfNotNullOrEmpty(t =>
                            infrastructureRepository.RetrieveDescriptionOfValidValueCode(sapField.SubstringStartAt(2),
                                t));
                    }

                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        private IEnumerable<TK> retrieve_lines(int id, int number, int type)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(DOCUMENT_LINE_QUERY, id));
            IList<TK> lines = new List<TK>();
            while (!recordSet.EoF)
            {
                var sapDocument = new TK();
                sapDocument.LineNumber = recordSet.GetColumnValue("LineNum").ToInt32();
                sapDocument.VisOrder = recordSet.GetColumnValue("VisOrder").ToInt32();
                sapDocument.ItemCode = recordSet.GetStringValue("ItemCode");
                sapDocument.ItemDescription = recordSet.GetStringValue("Dscription");
                sapDocument.Quantity = recordSet.GetColumnValue("Quantity").ToDecimal();
                //sapDocument.UsefulLifeTime = recordSet.GetColumnValue("U_EXK_TVUM")?.ToInt32() ?? 0;
                sapDocument.UnitPrice = recordSet.GetColumnValue("Price").ToDecimal();
                sapDocument.TotalAmount = recordSet.GetColumnValue("LineTotal").ToDecimal();
                sapDocument.TotalPrice = recordSet.GetColumnValue("GTotal").ToDecimal();
                sapDocument.TotalSaleVolume = recordSet.GetColumnValue("Volume").ToDecimal();
                sapDocument.TotalSaleWeight = recordSet.GetColumnValue("Weight1").ToDecimal();
                sapDocument.TaxCode = recordSet.GetStringValue("TaxCode");
                sapDocument.WarehouseCode = recordSet.GetStringValue("WhsCode");
                //sapDocument.DirOrigen = recordSet.GetStringValue("U_VS_DIRORG");
                if (recordSet.GetColumnValue("BaseEntry") != null)
                {
                    sapDocument.BaseEntry = recordSet.GetColumnValue("BaseEntry").ToInt32();
                    sapDocument.BaseLine = recordSet.GetColumnValue("BaseLine").ToInt32();
                    sapDocument.BaseType = recordSet.GetColumnValue("BaseType").ToInt32();
                }
                sapDocument.TargetEntry = recordSet.GetColumnValue("TrgetEntry").ToInt32();
                if (recordSet.GetColumnValue("TrgetEntry") != null)
                {
                    sapDocument.TargetType = recordSet.GetColumnValue("TargetType").ToInt32();
                }


                //sapDocument.EstadoLineaFR = recordSet.GetStringValue("U_EXK_STLN");
                //sapDocument.DistributionValidationCode = recordSet.GetStringValue("U_EXK_VSDS");
                //sapDocument.DistributionValidationReason = recordSet.GetStringValue("U_EXK_VRDS");
                //sapDocument.DistributionValidationMessage = recordSet.GetStringValue("U_EXK_VMDS");
                //sapDocument.TransferOrderReference = recordSet.GetStringValue("U_EXK_OTRF");
                //sapDocument.DistributionStatus = recordSet.GetStringValue("U_EXK_STDS");
                //sapDocument.TransferOrderValidationPreview = recordSet.GetStringValue("U_EXK_VPTO");
                //sapDocument.TransferOrderValidationReason = recordSet.GetStringValue("U_EXK_VPTR");
                //sapDocument.TransferOrderValidationComments = recordSet.GetStringValue("U_EXK_VPTC");

                //sapDocument.Cantidad_a_Procesar = recordSet.GetColumnValue("U_EXK_CPFR").ToDecimal();
                //sapDocument.CantidadProcesada = recordSet.GetColumnValue("U_EXK_CTFR").ToDecimal();

                //sapDocument.EstadoLineaFR = recordSet.GetStringValue("U_EXK_STLN");

                //sapDocument.TipoOperacion = recordSet.GetStringValue("U_tipoOpT12");
                //sapDocument.TipoAfectacion = recordSet.GetStringValue("U_VS_TIPAFE");
                //sapDocument.TipoOperacionExoINA = recordSet.GetStringValue("U_BPP_OPER");
                //sapDocument.Oneroso = recordSet.GetStringValue("U_VS_ONEROSO");

                sapDocument.CentroCosto = recordSet.GetStringValue("OcrCode");
                sapDocument.CentroCosto2 = recordSet.GetStringValue("OcrCode2");
                sapDocument.CentroCosto3 = recordSet.GetStringValue("OcrCode3");
                sapDocument.CentroCosto4 = recordSet.GetStringValue("OcrCode4");
                sapDocument.COGSCostingCode = recordSet.GetStringValue("CogsOcrCod");
                sapDocument.COGSCostingCode2 = recordSet.GetStringValue("CogsOcrCo2");
                sapDocument.COGSCostingCode3 = recordSet.GetStringValue("CogsOcrCo3");
                sapDocument.COGSCostingCode4 = recordSet.GetStringValue("CogsOcrCo4");

                sapDocument.SoloImpuesto = recordSet.GetStringValue("TaxOnly");

                sapDocument.SelectedBatches = RetrieveBatchesFromDocument(id, sapDocument.LineNumber, type);
                lines.Add(sapDocument);
                recordSet.MoveNext();
            }

            return lines;
        }

        private IEnumerable<TK> retrieve_lines_draft(int id, int number, int type)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(DOCUMENT_LINE_QUERY_DRAFT, id));
            IList<TK> lines = new List<TK>();
            while (!recordSet.EoF)
            {
                var sapDocument = new TK();
                sapDocument.LineNumber = recordSet.GetColumnValue("LineNum").ToInt32();
                sapDocument.VisOrder = recordSet.GetColumnValue("VisOrder").ToInt32();
                sapDocument.ItemCode = recordSet.GetStringValue("ItemCode");
                sapDocument.ItemDescription = recordSet.GetStringValue("Dscription");
                sapDocument.Quantity = recordSet.GetColumnValue("Quantity").ToDecimal();
                //sapDocument.UsefulLifeTime = recordSet.GetColumnValue("U_EXK_TVUM")?.ToInt32() ?? 0;
                sapDocument.UnitPrice = recordSet.GetColumnValue("Price").ToDecimal();
                sapDocument.TotalAmount = recordSet.GetColumnValue("LineTotal").ToDecimal();
                sapDocument.TotalPrice = recordSet.GetColumnValue("GTotal").ToDecimal();
                sapDocument.TotalSaleVolume = recordSet.GetColumnValue("Volume").ToDecimal();
                sapDocument.TotalSaleWeight = recordSet.GetColumnValue("Weight1").ToDecimal();
                sapDocument.TaxCode = recordSet.GetStringValue("TaxCode");
                sapDocument.WarehouseCode = recordSet.GetStringValue("WhsCode");
                //sapDocument.DirOrigen = recordSet.GetStringValue("U_VS_DIRORG");
                if (recordSet.GetColumnValue("BaseEntry") != null)
                {
                    sapDocument.BaseEntry = recordSet.GetColumnValue("BaseEntry").ToInt32();
                    sapDocument.BaseLine = recordSet.GetColumnValue("BaseLine").ToInt32();
                    sapDocument.BaseType = recordSet.GetColumnValue("BaseType").ToInt32();
                }
                sapDocument.TargetEntry = recordSet.GetColumnValue("TrgetEntry").ToInt32();
                if (recordSet.GetColumnValue("TrgetEntry") != null)
                {
                    sapDocument.TargetType = recordSet.GetColumnValue("TargetType").ToInt32();
                }


                //sapDocument.EstadoLineaFR = recordSet.GetStringValue("U_EXK_STLN");
                //sapDocument.DistributionValidationCode = recordSet.GetStringValue("U_EXK_VSDS");
                //sapDocument.DistributionValidationReason = recordSet.GetStringValue("U_EXK_VRDS");
                //sapDocument.DistributionValidationMessage = recordSet.GetStringValue("U_EXK_VMDS");
                //sapDocument.TransferOrderReference = recordSet.GetStringValue("U_EXK_OTRF");
                //sapDocument.DistributionStatus = recordSet.GetStringValue("U_EXK_STDS");
                //sapDocument.TransferOrderValidationPreview = recordSet.GetStringValue("U_EXK_VPTO");
                //sapDocument.TransferOrderValidationReason = recordSet.GetStringValue("U_EXK_VPTR");
                //sapDocument.TransferOrderValidationComments = recordSet.GetStringValue("U_EXK_VPTC");

                //sapDocument.Cantidad_a_Procesar = recordSet.GetColumnValue("U_EXK_CPFR").ToDecimal();
                //sapDocument.CantidadProcesada = recordSet.GetColumnValue("U_EXK_CTFR").ToDecimal();

                //sapDocument.EstadoLineaFR = recordSet.GetStringValue("U_EXK_STLN");

                //sapDocument.TipoOperacion = recordSet.GetStringValue("U_tipoOpT12");
                //sapDocument.TipoAfectacion = recordSet.GetStringValue("U_VS_TIPAFE");
                //sapDocument.TipoOperacionExoINA = recordSet.GetStringValue("U_BPP_OPER");
                //sapDocument.Oneroso = recordSet.GetStringValue("U_VS_ONEROSO");

                sapDocument.CentroCosto = recordSet.GetStringValue("OcrCode");
                sapDocument.CentroCosto2 = recordSet.GetStringValue("OcrCode2");
                sapDocument.CentroCosto3 = recordSet.GetStringValue("OcrCode3");
                sapDocument.CentroCosto4 = recordSet.GetStringValue("OcrCode4");
                sapDocument.COGSCostingCode = recordSet.GetStringValue("CogsOcrCod");
                sapDocument.COGSCostingCode2 = recordSet.GetStringValue("CogsOcrCo2");
                sapDocument.COGSCostingCode3 = recordSet.GetStringValue("CogsOcrCo3");
                sapDocument.COGSCostingCode4 = recordSet.GetStringValue("CogsOcrCo4");

                sapDocument.SoloImpuesto = recordSet.GetStringValue("TaxOnly");

                sapDocument.SelectedBatches = RetrieveBatchesFromDocument(id, sapDocument.LineNumber, type);
                lines.Add(sapDocument);
                recordSet.MoveNext();
            }

            return lines;
        }

        /// <summary>
        /// Retrieve batches from current document
        /// </summary>
        /// <param name="documentEntry">Current document's DocEntry</param>
        /// <param name="lineNumber">Current document's LineNum</param>
        /// <param name="sapType">Current document's ObjType</param>
        /// <returns>Reference batches</returns>
        protected virtual IEnumerable<SAPSelectedBatch> RetrieveBatchesFromDocument(int documentEntry, int lineNumber, int sapType)
        {
            IList<SAPSelectedBatch> result = new List<SAPSelectedBatch>();
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(SELECTED_BATCHES_QUERY, documentEntry, lineNumber, sapType));
            while (!recordSet.EoF)
            {
                var batch = new SAPSelectedBatch();
                batch.ItemCode = recordSet.GetColumnValue("ItemCode").ToString();
                batch.BatchNumber = recordSet.GetColumnValue("BatchNum").ToString();
                batch.WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString();
                batch.Quantity = recordSet.GetColumnValue("Quantity").ToDecimal();
                result.Add(batch);
                recordSet.MoveNext();
            }

            return result;
        }

        private bool is_only_custom_fields_with_values(object document, PropertyInfo property)
        {
            var sapColumnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
            if (sapColumnAttribute == null || sapColumnAttribute.SystemField)
                return false;

            object propertyValue = property.GetValue(document);
            return !(propertyValue?.Equals(property.PropertyType.GetDefaultValue()) ?? true);
        }

        public virtual Tuple<bool, string> UpdateCustomFieldsFromDocument(T document)
        {
            Type documentType = document.GetType();
            var sapObject = documentType.GetCustomAttribute<SAPObjectAttribute>();
            var saleOrder = Company.GetBusinessObject(sapObject.SapTypes).To<Documents>();
            //Documents saleOrder = (Documents)Company.GetBusinessObject(BoObjectTypes.oOrders);

            saleOrder.GetByKey(document.DocumentEntry);

            Fields userFields = saleOrder.UserFields.Fields;
            IEnumerable<PropertyInfo> documentProperties = documentType.GetProperties()
                .Where(property => is_only_custom_fields_with_values(document, property));

            foreach (PropertyInfo property in documentProperties)
            {
                object propertyValue = property.GetValue(document);
                string columnName = get_sap_column_name(property);
                Field userField = userFields.Item(columnName);
                object value = get_valid_value(userField.ValidValues, propertyValue);
                userField.Value = value;
            }

            if (saleOrder.Update().IsDefault())
                return Tuple.Create(true, string.Empty);

            var description = Company.GetLastErrorDescription();
            return Tuple.Create(false, description);
        }

        public virtual Tuple<bool, string> UpdateCustomFieldsFromDocumentFR(ORDR document, List<RDR1> lines)
        {
            Type documentType = document.GetType();
            var sapObject = documentType.GetCustomAttribute<SAPObjectAttribute>();
            var saleOrder = Company.GetBusinessObject(sapObject.SapTypes).To<Documents>();
            saleOrder.GetByKey(document.DocumentEntry);


            for (int i = 0; i < saleOrder.Lines.Count; i++)
            {
                saleOrder.Lines.SetCurrentLine(i);
                if (lines.Where(t => t.ItemCode == saleOrder.Lines.ItemCode).FirstOrDefault() != null)
                {
                    var line = document.DocumentLines.Where(t => t.ItemCode == saleOrder.Lines.ItemCode).FirstOrDefault();
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VSDS").Value = line.DistributionValidationCode;
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VRDS").Value = line.DistributionValidationReason;
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VMDS").Value = line.DistributionValidationMessage;

                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_OTRF").Value = line.TransferOrderReference;
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VPTO").Value = line.TransferOrderValidationPreview;
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VPTR").Value = line.TransferOrderValidationReason ?? "";
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_VPTC").Value = line.TransferOrderValidationComments ?? "";

                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_CTFR").Value = line.CantidadProcesada.ToString();
                    //saleOrder.Lines.UserFields.Fields.Item("U_EXK_STLN").Value = line.EstadoLineaFR.ToString();



                }
            }



            if (saleOrder.Update().IsDefault())
                return Tuple.Create(true, string.Empty);

            var description = Company.GetLastErrorDescription();
            return Tuple.Create(false, description);
        }

        public virtual Tuple<bool, string> UpdateCustomFieldsClearTransferOrderreference(T document)
        {
            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            string query = $"Update ORDR set \"U_EXK_OTRF\" =null where \"DocEntry\" ={document.DocumentEntry}";
            recordSet.DoQuery(query);




            //if (saleOrder.Update().IsDefault())
            return Tuple.Create(true, string.Empty);

            //var description = Company.GetLastErrorDescription();
            //return Tuple.Create(false, description);
        }
        public virtual Tuple<bool, string> UpdateCustomFieldsComments(T document)
        {
            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            string query = $"Update ORDR set \"Comments\" ='Cerrado por cancelación OT N°{document.DocumentNumber}' where \"DocEntry\" ={document.DocumentEntry}";
            recordSet.DoQuery(query);


            //if (saleOrder.Update().IsDefault())
            return Tuple.Create(true, string.Empty);

            //var description = Company.GetLastErrorDescription();
            //return Tuple.Create(false, description);
        }

        private object get_valid_value(IValidValues validValues, object value)
        {
            for (var i = 0; i < validValues.Count; i++)
            {
                ValidValue item = validValues.Item(i);
                if (item.Description == value.ToString())
                    return item.Value;
            }

            return value;
        }

        public virtual void UpdateSystemFieldsFromDocument(T document)
        {
        }

        public virtual void CloseDocument(int documentEntry)
        {
            Type documentType = typeof(T);
            var sapObject = documentType.GetCustomAttribute<SAPObjectAttribute>();
            var sapDocument = Company.GetBusinessObject(sapObject.SapTypes).To<Documents>();
            sapDocument.GetByKey(documentEntry);
            var result = sapDocument.Close();
            if (result != 0)
                throw new Exception($"[ERROR] ({result}): {Company.GetLastErrorDescription()}");
        }

        public virtual void CancelDocument(int documentEntry)
        {
            var sapObject = typeof(T).GetCustomAttribute<SAPObjectAttribute>();
            var sapDocument = Company.GetBusinessObject(sapObject.SapTypes).To<Documents>();
            sapDocument.GetByKey(documentEntry);

            Documents cancellationDocument = sapDocument.CreateCancellationDocument();
            cancellationDocument.DocDate = sapDocument.DocDate;
            //cancellationDocument.DocDueDate = sapDocument.DocDueDate;
            cancellationDocument.TaxDate = sapDocument.TaxDate;
            cancellationDocument.Comments = $"[Generación automatica] {cancellationDocument.Comments}";

            if (cancellationDocument.Add() == 0)
            {
                var docentry = Company.GetNewObjectKey();
                SendMessage_AlertAsync(sapDocument.NumAtCard, docentry);
                return;
            }
            int errorCode;
            string errorMessage;
            Company.GetLastError(out errorCode, out errorMessage);
            throw new Exception($"({errorCode})-{errorMessage}");
        }
        public virtual void UpdateDocument(int documentEntry)
        {
            var sapObject = typeof(T).GetCustomAttribute<SAPObjectAttribute>();
            var sapDocument = Company.GetBusinessObject(sapObject.SapTypes).To<Documents>();
            sapDocument.GetByKey(documentEntry);

            sapDocument.UserFields.Fields.Item("U_VS_FESTAT").Value = "I";

            if (sapDocument.Update() == 0)
            {
                var docentry = Company.GetNewObjectKey();
                return;
            }
            int errorCode;
            string errorMessage;
            Company.GetLastError(out errorCode, out errorMessage);
            throw new Exception($"({errorCode})-{errorMessage}");
        }
        public void SendMessage_AlertAsync(string oMessage, string docentry)
        {
            SendMessage_Alert(oMessage, docentry);
        }
        private void SendMessage_Alert(string oMessage, string docentry)
        {
            try
            {
                SAPbobsCOM.Messages msg = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages).To<Messages>();
                var oCmpSrv = Company.GetCompanyService();
                var oMessageService = (SAPbobsCOM.MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);


                var recordSet = Company
                  .GetBusinessObject(BoObjectTypes.BoRecordsetEx)
                  .To<SAPbobsCOM.RecordsetEx>();
                recordSet.DoQuery($@"select ""U_EXK_CVAL"" from ""@VS_PD_OPDS"" where ""Code"" = 'DEV_AL' ");
                var userCode = "";
                List<string> listUser = new List<string>();

                while (!recordSet.EoF)
                {
                    userCode = recordSet.GetColumnValue("U_EXK_CVAL").ToString();
                    recordSet.MoveNext();
                }

                foreach (var item in userCode.Split(","))
                {
                    listUser.Add(item);
                }

                //if (oMessage.Priority.HasValue)
                msg.Priority = BoMsgPriorities.pr_High;


                msg.Subject = "Alerta de cancelación de documento ";
                var mensaje = $"Se ha cancelado el documento N° {oMessage} ";



                msg.MessageText = mensaje;

                for (int i = 0; i < listUser.Count; i++)
                {
                    if (i > 0)
                        msg.Recipients.Add();
                    msg.Recipients.SetCurrentLine(i);
                    msg.Recipients.UserCode = listUser[i];
                    msg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;

                }

                //msg.Recipients.Add();


                var result = msg.Add();


                if (result != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();

                    //Company.GetLastError(out res, out error);                 
                }
            }
            catch (Exception)
            {

            }

        }

        public void SendMessage_AlerBilling(string message)
        {
            try
            {
                SAPbobsCOM.Messages msg = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages).To<Messages>();
                var oCmpSrv = Company.GetCompanyService();
                var oMessageService = (SAPbobsCOM.MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);


                var recordSet = Company
                  .GetBusinessObject(BoObjectTypes.BoRecordsetEx)
                  .To<SAPbobsCOM.RecordsetEx>();
                recordSet.DoQuery($@"select ""U_EXK_CVAL"" from ""@VS_PD_OPDS"" where ""Code"" = 'REP_AL' ");
                var userCode = "";
                List<string> listUser = new List<string>();

                while (!recordSet.EoF)
                {
                    userCode = recordSet.GetColumnValue("U_EXK_CVAL").ToString();
                    recordSet.MoveNext();
                }

                foreach (var item in userCode.Split(","))
                {
                    listUser.Add(item);
                }

                //if (oMessage.Priority.HasValue)
                msg.Priority = BoMsgPriorities.pr_High;


                msg.Subject = "Alerta de reprogramación, cancelación de factura ";
                var mensaje = message;



                msg.MessageText = mensaje;

                for (int i = 0; i < listUser.Count; i++)
                {
                    if (i > 0)
                        msg.Recipients.Add();
                    msg.Recipients.SetCurrentLine(i);
                    msg.Recipients.UserCode = listUser[i];
                    msg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;

                }

                //msg.Recipients.Add();


                var result = msg.Add();


                if (result != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();

                    //Company.GetLastError(out res, out error);                 
                }
            }
            catch (Exception)
            {

            }

        }

        public virtual void UpdateCustomBatchesSaleOrder(T document)
        {
            try
            {
                var order = (SAPbobsCOM.Documents)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                Items s = (SAPbobsCOM.Items)Company.GetBusinessObject(BoObjectTypes.oItems);
                //ORDR or = (ORDR)document;
                order.GetByKey(document.DocumentEntry);

                var lines = document.DocumentLines;
                s.GetByKey(lines.FirstOrDefault().ItemCode);

                for (int i = 0; i < order.Lines.Count; i++)
                {
                    order.Lines.SetCurrentLine(i);
                    if (order.Lines.ItemCode == lines.FirstOrDefault().ItemCode)
                    {
                        order.Lines.BatchNumbers.Add();
                        order.Lines.BatchNumbers.BatchNumber = lines.FirstOrDefault().SelectedBatches.FirstOrDefault().BatchNumber;
                        order.Lines.BatchNumbers.Quantity = double.Parse(lines.FirstOrDefault().Quantity.ToString()) * s.SalesItemsPerUnit;
                    }
                }

                int res = order.Update();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    //return Tuple.Create(false, vm_GetLastErrorDescription_string, "");
                    //Company.GetLastError(out res, out error);                 
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Tuple<bool, string> RegisterByPicking(List<CrossCutting.Model.Local.Item> items, DateTime fecha)
        {
            string DocEntry = "";
            try
            {
                //Items s = (SAPbobsCOM.Items) Company.GetBusinessObject(BoObjectTypes.oItems);
                StockTransfer documentValid = (SAPbobsCOM.StockTransfer)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                //documentValid.DocDate = DateTime.Now.AddHours(-2);
                documentValid.DocDate = DateTime.Now.AddHours(-2);
                documentValid.TaxDate = DateTime.Now.AddHours(-2);

                documentValid.FromWarehouse = items.FirstOrDefault().Batches.FirstOrDefault().WareHouseId; // "0101";
                                                                                                           //TODO: agregar el campo configurable - Referenciado para Ruben Gamarra
                BaseOPDSRepository _settings = new UnitOfWork(Company).SettingsRepository;
                BaseOWHSRepository _whs = new UnitOfWork(Company).WarehouseRepository;


                documentValid.ToWarehouse = _settings.Setting(OPDS.Codes.TEMPORAL_WAREHOUSE).Value;
                documentValid.Comments = "Creado por el addon de Picking y Distribución " + items.FirstOrDefault().RouteId;
                //


                string _json = JsonConvert.SerializeObject(items);

                int contB = 0;
                foreach (var item in items)
                {


                    documentValid.Lines.ItemCode = item.ItemCode;
                    documentValid.Lines.FromWarehouseCode = item.Batches.FirstOrDefault().WareHouseId; // "0101";
                    documentValid.Lines.WarehouseCode = _whs.RetrieveWarehouse(documentValid.Lines.FromWarehouseCode).CodigoAlmacenTemporal; //_settings.Setting(OPDS.Codes.TEMPORAL_WAREHOUSE).Value;

                    documentValid.Lines.Quantity =
                        item.Batches.Sum(t => t.Quantity)
                            .ToDouble(); // / s.SalesItemsPerUnit;//Decimal.ToDouble(item.Quantity) * s.SalesItemsPerUnit;

                    foreach (var batch in item.Batches)
                    {
                        documentValid.Lines.BatchNumbers.BatchNumber = batch.BatchId;
                        documentValid.Lines.BatchNumbers.ItemCode = item.ItemCode;
                        documentValid.Lines.BatchNumbers.Quantity = Decimal.ToDouble(batch.Quantity); //* s.SalesItemsPerUnit;
                        documentValid.Lines.BatchNumbers.BaseLineNumber = contB;
                        //documentValid.Lines.BatchNumbers.SetCurrentLine(contB);
                        documentValid.Lines.BatchNumbers.Add();
                    }

                    contB++;
                    documentValid.Lines.Add();
                }

                int res = documentValid.Add();
                if (res != 0) // Check the result
                {
                    string error;
                    string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();
                    return Tuple.Create(false, vm_GetLastErrorDescription_string);
                    //Company.GetLastError(out res, out error);                 
                }
                else
                {
                    DocEntry = Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

            return Tuple.Create(true, DocEntry);
        }


        public Tuple<bool, string> RetriveCobroTransId(string id)
        {
            try
            {
                var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
                var query = "select * from \"ORCT\" where \"DocEntry\" ={0} ";
                recordSet.DoQuery(string.Format(query, id));
                while (!recordSet.EoF)
                {

                    return Tuple.Create(true,recordSet.GetColumnValue("TransId").ToString());
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, "Error RetriveCobroTransId ");

            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Error RetriveCobroTransId ");
            }
        }
    }
}