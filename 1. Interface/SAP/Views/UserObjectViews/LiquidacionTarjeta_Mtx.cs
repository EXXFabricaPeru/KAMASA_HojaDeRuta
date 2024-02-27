using ClosedXML.Excel;
using Newtonsoft.Json;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews
{
    public partial class LiquidacionTarjeta
    {
        private OPCG templatePasarela;
        private List<LiquidationLineSL> listDetailLiquidation;
        private void AnalizarArchivo()
        {
            SAPbouiCOM.DataRows dataRows = null;
            ApplicationInterfaceHelper.ShowStatusBarMessage("Analizando el archivo", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(true);

            try
            {
                if (string.IsNullOrEmpty(_fechaInicioVentasEditText.Value))
                    throw new Exception("Debe colocar la fecha de inicio de ventas");

                if (string.IsNullOrEmpty(_fechaFinVentasEditText.Value))
                    throw new Exception("Debe colocar la fecha de fin de ventas");


                templatePasarela = new OPCG();
                templatePasarela = _fileDomain.RetrieveTemplatePasarela(_pasarelaPagoEditText.Value);

                XLWorkbook work = new XLWorkbook(_archivoEditText.Value);
                validate_excel_workbook(work);
                generate_grid_rows(work);

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                ApplicationInterfaceHelper.ShowStatusBarMessage("Finalizando el proceso", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
                GenericHelper.ReleaseCOMObjects(dataRows);
                UIAPIRawForm.Freeze(false);
            }
        }

        private void validate_excel_workbook(XLWorkbook workBook)
        {
            const int firstRowIndex = 0;

            if (workBook.Worksheets.Count != 1)
                throw new Exception(@"[Validación] El archivo excel solo puede tener una hoja de trabajo.");

            IXLWorksheet workSheet = workBook.Worksheets.First();

            IList<PCG1> templateColumns = templatePasarela.ColumnDetail.ToList();

            bool firstRow2 = true;
            foreach (PCG1 column in templateColumns)
            {
                firstRow2 = true;
                var isValidColumn = false;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow2)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            //dt.Columns.Add(cell.Value.ToString());
                            var x = Tuple.Create(cell.Address.ToString(), cell.Value.ToString());
                            if (column.ExcelColumnName != cell.Value.ToString())
                                continue;

                            isValidColumn = true;
                            break;
                            //list.Add(x);
                        }
                        firstRow2 = false;
                    }
                    else
                    {
                    }
                    if (!isValidColumn)
                        throw new Exception($"[Validación] La columna '{column.ExcelColumnName}' no existe en el archivo excel a cargar.");
                    break;
                }

            }


        }
        private struct RequiredProperty
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public RequiredProperty(string name, PropertyInfo info, bool esFecha = false)
            {
                Name = name;
                Info = info;
                EsFecha = esFecha;
            }

            public string Name { get; }
            public PropertyInfo Info { get; }

            public bool EsFecha { get; }

        }
        private static IEnumerable<RequiredProperty> REQUIRED_PROPERTIES = new[]
        {
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.NroTarjeta).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.NroTarjeta)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.FechaCobro).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.FechaCobro),true),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.NroReferenciaCobro).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.NroReferenciaCobro)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.ImporteCobradoTarjeta).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.ImporteCobradoTarjeta)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.Comision).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.Comision)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.IGV).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.IGV)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.NetoParcial).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.NetoParcial)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.ComisionEmisor).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.ComisionEmisor)),
            new RequiredProperty(((VSVersionControl.FlagElements.Attributes.FieldNoRelated)DocumentDetail.RetrievePropertyInfo(t => t.ComisionMCPeru).GetCustomAttributeByName("FieldNoRelated")).ColumnName, DocumentDetail.RetrievePropertyInfo(t=> t.ComisionMCPeru)),
        };

        private void generate_grid_rows(XLWorkbook workBook)
        {

            const int firstRow = 1;
            string mainColumnLetter = Regex.Replace(templatePasarela.MainColumnAddress, "[^A-Za-z]", "");
            IList<DocumentDetail> result = new List<DocumentDetail>();
            IXLWorksheet workSheet = workBook.Worksheets.Single();
            IList<IXLRow> rowsToAnalyze = workSheet.RowsUsed().Where(t => t.RowNumber() > firstRow).ToList();

            var groupingRowsData = rowsToAnalyze
                .Select(t => new { RowNumber = t.RowNumber(), GroupingValue = t.Cell(mainColumnLetter).Value.ToString() })
                .GroupBy(t => t.GroupingValue);

            IList<PCG1> requiredColumns = templatePasarela.ColumnDetail.ToList();

            foreach (var groupingRow in groupingRowsData)
            {
                var lineCount = 1;
                foreach (var rowData in groupingRow)
                {
                    IXLRow row = rowsToAnalyze.RetrieveRow(rowData.RowNumber);
                    var record = new DocumentDetail();
                    //record.Identifier = rowData.GroupingValue;
                    //record.ItemLine = lineCount;
                    foreach (PCG1 requiredColumn in requiredColumns)
                    {
                        string columnLetter = Regex.Replace(requiredColumn.ExcelColumnAddress, "[^A-Za-z]", "");
                        var stringValue = row.Cell(columnLetter).Value.ToString();
                        var propiedad = REQUIRED_PROPERTIES.Single(t => t.Name == requiredColumn.SAPValue);
                        PropertyInfo property = propiedad.Info;
                        if (propiedad.EsFecha)
                        {
                            var objectValue = DateTime.ParseExact(stringValue, templatePasarela.DateFormat, CultureInfo.InvariantCulture);
                            property.SetValue(record, objectValue);
                        }
                        else
                        {
                            var objectValue = Convert.ChangeType(stringValue, property.PropertyType);
                            property.SetValue(record, objectValue);
                        }
                    }
                    record.IsSelected = true;
                    var fechainicio = DateTime.ParseExact(_fechaInicioVentasEditText.Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var fechaFin = DateTime.ParseExact(_fechaFinVentasEditText.Value, "yyyyMMdd", CultureInfo.InvariantCulture);

                    if (record.FechaCobro.Between(fechainicio, fechaFin.AddDays(1)))
                        result.Add(record);
                    lineCount++;

                }
            }


            seachDocuments(ref result);


            _linesMatrixMatrixBuilder.SyncData(result);
            //ColumnSetting columnSetting = _linesMatrixMatrixBuilder.ActualData.Item(15).ColumnSetting;
            //columnSetting.SumType = BoColumnSumType.bst_Auto;
            _linesMatrixMatrixBuilder.AutoResizeColumns();
            CargarTotales();


        }

        private void seachDocuments(ref IList<DocumentDetail> result)
        {
            try
            {
                foreach (var item in result)
                {
                    var document = _liquidacionTarjetaDomain.searchInfoDocument(item);

                    if (document != null)
                    {
                        item.CodigoCliente = document.CardCode;
                        if (document.ObjectType == 13)
                            item.TipoDocumento = "01";
                        item.NombreCliente = document.CardName;
                        item.NroTicket = document.NumberAtCard;
                        item.FechaDocumento = document.DocumentDate;
                        item.ImporteDocumento = document.TotalPrice.ToDouble();
                        item.Moneda = "SOL";//document.
                        //item.TipoCambio=document.
                        item.NroSAPCobro = document.NroSapCobro;
                        item.TransId = document.NroSapCobroTransId.ToString();
                    }
                    else
                    {
                        item.TipoDocumento = "99";
                        item.NroTicket = "Sin Documento en SAP";
                    }

                    item.CodigoTienda = _codigoTiendaComboBox.Value;
                    item.PasarelaPago = _pasarelaPagoEditText.Value;
                    item.CuentaTransitoria = _cuentaTransitoriaEditText.Value;

                }

                //result = result.Where(t => t.FechaCobro == templatePasarela.MainColumnAddress)
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
        }

        private void EjecutarLiquidacion()
        {

            try
            {
                listDetailLiquidation.Clear();
                listDetailLiquidation = new List<LiquidationLineSL>();
                var asiento = generarAsiento();

                var jsonSL = JsonConvert.SerializeObject(asiento);
                var respuesta = _liquidacionTarjetaDomain.generarAsiento(asiento);


                if (respuesta.Item1)
                {
                    var reconciliacion = generarReconciliacion(respuesta.Item3, respuesta.Item2);


                    if (!reconciliacion.Item1)
                    {
                        ApplicationInterfaceHelper.ShowErrorStatusBarMessage(reconciliacion.Item2);
                        cancelarAsiento(respuesta.Item2);
                        throw new Exception(reconciliacion.Item2);
                    }
                    else
                    {
                        //actualizarEstadoPagos();
                    }

                }
                else
                {
                    throw new Exception(respuesta.Item2);
                }

            }
            catch (Exception ex)
            {

                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                throw;
            }
        }

        private void actualizarEstadoPagos()
        {
            throw new NotImplementedException();
        }

        private void cancelarAsiento(string codAs)
        {
            try
            {
                var respuesta = _liquidacionTarjetaDomain.cancelarAsiento(codAs);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
        }

        private Tuple<bool, string> generarReconciliacion(JournalEntrySL asiento, string DocEntryAsiento)
        {
            try
            {
                ReconciliationSL reconciliation = new ReconciliationSL();

                reconciliation.CardOrAccount = "coaAccount";
                reconciliation.ReconDate = _fechaBonoEditText.GetDateTimeValue();

                List<ReconciliationLinesSL> lines = new List<ReconciliationLinesSL>();



                //_asiento.Selected = "tYes";
                //_asiento.SrcObjTyp =SAPbobsCOM.BoObjectTypes.oJournalEntries.ToString();
                //_asiento.SrcObjAbs = item2.ToInt32();
                //_asiento.CreditOrDebit = "codCredit";
                //_asiento.ReconcileAmount = 00;
                //_asiento.ShortName = _cuentaTransitoriaEditText.Value;



                int cont = 2;
                foreach (var item in _linesMatrixMatrixBuilder.ActualData.Where(t=>t.TipoDocumento=="01"|| t.TipoDocumento=="03"))
                {

                    ReconciliationLinesSL _asiento = new ReconciliationLinesSL();
                    _asiento.Selected = "tYES";
                    _asiento.SrcObjTyp = "30";
                    _asiento.SrcObjAbs = DocEntryAsiento.ToInt32();
                    _asiento.CreditOrDebit = "codCredit";
                    _asiento.ReconcileAmount = item.ImporteDocumento; ;
                    _asiento.ShortName = _cuentaTransitoriaEditText.Value;
                    _asiento.TransId = DocEntryAsiento.ToInt32();
                    _asiento.TransRowId = asiento.JournalEntryLines.Where(t => t.Reference2 == item.NroTicket).FirstOrDefault().Line;
                    lines.Add(_asiento);

                    ReconciliationLinesSL _pago = new ReconciliationLinesSL();

                    _pago.Selected = "tYES";
                    _pago.SrcObjTyp = "24";
                    _pago.SrcObjAbs = item.NroSAPCobro;
                    _pago.CreditOrDebit = "codDebit";
                    _pago.ReconcileAmount = item.ImporteDocumento;
                    _pago.ShortName = _cuentaTransitoriaEditText.Value;
                    _pago.TransId = item.TransId.ToInt32();
                    _pago.TransRowId = 0;

                    lines.Add(_pago);
                    cont++;
                }


                reconciliation.InternalReconciliationOpenTransRows = lines;

                string jsonString = JsonConvert.SerializeObject(reconciliation);
                var respuesta = _liquidacionTarjetaDomain.generarReconciliacion(reconciliation);
                return respuesta;
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
        }

        private JournalEntrySL generarAsiento()
        {
            JournalEntrySL entrySL = new JournalEntrySL();
            entrySL.ReferenceDate = _fechaBonoEditText.GetDateTimeValue();
            entrySL.DueDate = _fechaBonoEditText.GetDateTimeValue();
            entrySL.TaxDate = _fechaBonoEditText.GetDateTimeValue();
            entrySL.Memo = "Asiento Generado Por Liquidación de Tarjetas";
            entrySL.TransactionCode = "ASA";

            entrySL.JournalEntryLines = new List<JournalEntryLineSL>();

            //DETALLE 
            JournalEntryLineSL detalle104 = new JournalEntryLineSL();
            detalle104.AccountCode = _cuentaContableEditText.Value;
            detalle104.Debit = _importeDepositadoEditText.Value.ToDouble();
            detalle104.DueDate = _fechaBonoEditText.GetDateTimeValue();
            detalle104.Credit = 0;

            List<CashFlowAssignment> cashFlows = new List<CashFlowAssignment>();
            CashFlowAssignment cashFlow = new CashFlowAssignment();

            cashFlow.AmountLc = detalle104.Debit;
            cashFlow.CashFlowLineItemId = _flujoRelevanteComboBox.Selected.Value.ToInt32();


            cashFlows.Add(cashFlow);

            detalle104.CashFlowAssignments = cashFlows;
            entrySL.JournalEntryLines.Add(detalle104);
            ////
            //Cuenta comisiones 1691102//TODO PL20341198217

            JournalEntryLineSL detalleComisiones = new JournalEntryLineSL();
            detalleComisiones.AccountCode = _settingsDomain.AccountComision.Value;
            detalleComisiones.Debit = _sumaComisionesEditText.Value.ToDouble();
            detalleComisiones.DueDate = _fechaBonoEditText.GetDateTimeValue();
            detalleComisiones.Credit = 0;
            detalleComisiones.ShortName = _codSocioProveedor;
            entrySL.JournalEntryLines.Add(detalleComisiones);

            if (_importeAjusteEditText.Value.ToDecimal() != 0)
            {
                //CASO AJUSTE MAYOR
                JournalEntryLineSL detalleAJUSTE = new JournalEntryLineSL();
                var ajusteMayor = _linesMatrixMatrixBuilder.ActualData.Where(t => t.MotivoAjuste == "AJU_MAY");

                foreach (var _mot in ajusteMayor)
                {
                    detalleAJUSTE.AccountCode = _mot.CuentaAsignada;//_linesMatrixMatrixBuilder.ActualData.Where(t => t.ImporteAjuste > 0).FirstOrDefault().CuentaAsignada;
                    detalleAJUSTE.Debit = _mot.ImporteAjuste;//_importeAjusteEditText.Value.ToDouble();
                    detalleAJUSTE.DueDate = _fechaBonoEditText.GetDateTimeValue();
                    detalleAJUSTE.Credit = 0;
                    detalleAJUSTE.ShortName = _mot.SocioNegocioAsociado;
                    entrySL.JournalEntryLines.Add(detalleAJUSTE);
                }

                JournalEntryLineSL detalleRedondeo = new JournalEntryLineSL();
                var redondeo = _linesMatrixMatrixBuilder.ActualData.Where(t => t.MotivoAjuste.Contains("RED"));
                foreach (var _mot in redondeo)
                {
                    detalleRedondeo.AccountCode = _mot.CuentaAsignada;//_linesMatrixMatrixBuilder.ActualData.Where(t => t.ImporteAjuste > 0).FirstOrDefault().CuentaAsignada;
                    detalleRedondeo.Debit = _mot.ImporteAjuste;//_importeAjusteEditText.Value.ToDouble();
                    detalleRedondeo.DueDate = _fechaBonoEditText.GetDateTimeValue();
                    detalleRedondeo.Credit = 0;
                    //detalleAJUSTE.ShortName = _mot.SocioNegocioAsociado;
                    entrySL.JournalEntryLines.Add(detalleRedondeo);
                }

                JournalEntryLineSL detallePersonal = new JournalEntryLineSL();
                var personal = _linesMatrixMatrixBuilder.ActualData.Where(t => t.MotivoAjuste.Contains("DES_PER"));
                foreach (var _mot in personal)
                {
                    detallePersonal.AccountCode = _mot.CuentaAsignada;//_linesMatrixMatrixBuilder.ActualData.Where(t => t.ImporteAjuste > 0).FirstOrDefault().CuentaAsignada;
                    detallePersonal.Debit = _mot.ImporteAjuste;//_importeAjusteEditText.Value.ToDouble();
                    detallePersonal.DueDate = _fechaBonoEditText.GetDateTimeValue();
                    detallePersonal.Credit = 0;
                    detallePersonal.ShortName = _mot.SocioNegocioAsociado;
                    entrySL.JournalEntryLines.Add(detallePersonal);
                }


                JournalEntryLineSL detalleOtrosAjustes = new JournalEntryLineSL();
                var otrosAjustes = _linesMatrixMatrixBuilder.ActualData.Where(t => t.MotivoAjuste.Contains("OTR_AJU"));
                foreach (var _mot in otrosAjustes)
                {
                    detalleOtrosAjustes.AccountCode = _mot.CuentaAsignada;//_linesMatrixMatrixBuilder.ActualData.Where(t => t.ImporteAjuste > 0).FirstOrDefault().CuentaAsignada;
                    detalleOtrosAjustes.Debit = 0;//_importeAjusteEditText.Value.ToDouble();
                    detalleOtrosAjustes.DueDate = _fechaBonoEditText.GetDateTimeValue();
                    detalleOtrosAjustes.Credit = _mot.ImporteAjuste;//_importeAjusteEditText.Value.ToDouble();
                    detalleOtrosAjustes.ShortName = _mot.SocioNegocioAsociado;
                    entrySL.JournalEntryLines.Add(detalleOtrosAjustes);
                }

            }
            JournalEntryLineSL detalleNotaCredito = new JournalEntryLineSL();
            var notacredito = _linesMatrixMatrixBuilder.ActualData.Where(t => t.TipoDocumento == "07");

            foreach (var _mot in notacredito)
            {
                detalleNotaCredito.AccountCode = "1212101";//_mot.CuentaAsignada;//_linesMatrixMatrixBuilder.ActualData.Where(t => t.ImporteAjuste > 0).FirstOrDefault().CuentaAsignada;
                detalleNotaCredito.Debit = _mot.ImporteCobradoTarjeta * -1;//_importeAjusteEditText.Value.ToDouble();
                detalleNotaCredito.DueDate = _fechaBonoEditText.GetDateTimeValue();
                detalleNotaCredito.Credit = 0;
                detalleNotaCredito.ShortName = _mot.CodigoCliente;
                entrySL.JournalEntryLines.Add(detalleNotaCredito);
            }

            //DETALLE DOCUMENTOS
            foreach (var item in _linesMatrixMatrixBuilder.ActualData)
            {
                JournalEntryLineSL detalleDocumentos = new JournalEntryLineSL();
                if (item.TipoDocumento == "01" || item.TipoDocumento == "03")
                {
                    detalleDocumentos.AccountCode = _cuentaTransitoriaEditText.Value;
                    detalleDocumentos.Debit = 0;
                    detalleDocumentos.DueDate = _fechaBonoEditText.GetDateTimeValue();
                    detalleDocumentos.Credit = item.ImporteDocumento;
                    detalleDocumentos.Reference1 = item.CodigoCliente;
                    detalleDocumentos.Reference2 = item.NroTicket;
                    entrySL.JournalEntryLines.Add(detalleDocumentos);

                    actualizarDetalle(item);
                }
               
            }


            return entrySL;
        }


        private void actualizarDetalle(DocumentDetail item)
        {

            LiquidationLineSL lineSL = new LiquidationLineSL
            {
                CodigoTienda = item.CodigoTienda,
                Pasarela = item.PasarelaPago,
                CodigoCliente = item.CodigoCliente,
                CodigoDocumento = string.IsNullOrEmpty(item.CodigoDocumento) ? 0 : item.CodigoDocumento.ToInt32(),
                Comision = item.Comision,
                ComisionEmisor = item.ComisionEmisor,
                ComisionMCPeru = item.ComisionMCPeru,
                CuentaAsignada = item.CuentaAsignada,
                CuentaTransitoria = item.CuentaTransitoria,
                FechaCobrado = item.FechaCobro,
                FechaDocumento = item.FechaDocumento,
                IGV = item.IGV,
                ImporteAjuste = item.ImporteAjuste,
                ImporteDocumento = item.ImporteDocumento,
                ImporteTarjeta = item.ImporteCobradoTarjeta,
                Moneda = item.Moneda,
                MotivoAjuste = item.MotivoAjuste,
                NetoParcial = item.NetoParcial,
                NombreCliente = item.NombreCliente,
                NroOperacion = item.NroOperacion,
                NroReferencia = item.NroReferenciaCobro,
                NroTarjeta = item.NroTarjeta,
                NroSAPCobro = item.NroSAPCobro,
                NroTicket = item.NroTicket,
                OtroMedioPago = item.OtroMedioPago,
                SocioNegocioAsociado = item.SocioNegocioAsociado,
                TipoDocumento = item.TipoDocumento,
                Estado = "Liquidado"
            };



            listDetailLiquidation.Add(lineSL);

        }


        private List<DocumentDetail> ParseDetail(LiquidationSL respuesta)
        {
            List<DocumentDetail> detailList = new List<DocumentDetail>();

            foreach (var item in respuesta.VsLtLtr1Collection)
            {
                DocumentDetail newline = new DocumentDetail
                {
                    CodigoCliente = item.CodigoCliente,
                    NetoParcial = item.NetoParcial ?? 0,
                    CodigoDocumento = (item.CodigoDocumento ?? 0).ToString(),
                    CodigoTienda = item.CodigoTienda,
                    Comision = item.Comision ?? 0,
                    ComisionEmisor = item.ComisionEmisor ?? 0,
                    ComisionMCPeru = item.ComisionMCPeru ?? 0,
                    CuentaAsignada = item.CuentaAsignada,
                    CuentaTransitoria = item.CuentaTransitoria,
                    Estado = item.Estado,
                    IGV = item.IGV ?? 0,
                    FechaCobro = item.FechaCobrado ?? DateTime.Now,
                    ImporteAjuste = item.ImporteAjuste ?? 0,
                    ImporteCobradoTarjeta = item.ImporteTarjeta ?? 0,
                    FechaDocumento = item.FechaDocumento ?? DateTime.Now,
                    ImporteDocumento = item.ImporteDocumento ?? 0,
                    Moneda = item.Moneda,
                    NroReferenciaCobro = item.NroReferencia,
                    NroOperacion = item.NroOperacion,
                    NroSAPCobro = item.NroSAPCobro ?? 0,
                    NombreCliente = item.NombreCliente,
                    PasarelaPago = item.Pasarela,
                    MotivoAjuste = item.MotivoAjuste,
                    TipoCambio = item.TipoCambio ?? 0,
                    TipoDocumento = item.TipoDocumento,
                    SocioNegocioAsociado = item.SocioNegocioAsociado,
                    NroTarjeta = item.NroTarjeta,
                    OtroMedioPago = item.OtroMedioPago ?? 0,
                    NroTicket = item.NroTicket,
                    LineId = item.LineId,
                    IsSelected = true
                };
                detailList.Add(newline);
            }

            return detailList;
        }


        private void lineMatrix_validate_after(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                if (eventArgs.ColUID == "col23")
                {

                    var motivo = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col23").Cells.Item(eventArgs.Row).Specific;

                    var cuentaAsociada = _liquidacionTarjetaDomain.RetrieveCuentaxMotivo(motivo.Value);
                    if (cuentaAsociada.Item1)
                    {
                        var _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col24").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = cuentaAsociada.Item2;


                  
                        if (motivo.Value=="OTR_AJU")
                        {

                              var _netoParcial = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col22").Cells.Item(eventArgs.Row).Specific;
                            var _Comision = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col18").Cells.Item(eventArgs.Row).Specific;
                            var _IGV = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col21").Cells.Item(eventArgs.Row).Specific;

                            var Ajuste = _netoParcial.Value.ToDouble() + _Comision.Value.ToDouble() + _IGV.Value.ToDouble();



                            _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col26").Cells.Item(eventArgs.Row).Specific;
                            _item.Value = Ajuste.ToString();

                            CargarTotales();
                        }
                    }

                }

                if (eventArgs.ColUID == "col16")
                {

                    var codDoc = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col16").Cells.Item(eventArgs.Row).Specific;

                    var detalle = _liquidacionTarjetaDomain.RetrieveDataPago(codDoc.Value);
                    if (detalle.Item1)
                    {
                        var _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col6").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.CodigoCliente;

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col7").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.NombreCliente;

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col4").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.TipoDocumento;

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col5").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.NroTicket;

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col8").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.FechaDocumento.ToString("yyyyMMdd");

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col10").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.Moneda.ToString();

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col9").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.ImporteDocumento.ToString();

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col11").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.NroSAPCobro.ToString();


                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col28").Cells.Item(eventArgs.Row).Specific;
                        _item.Value = detalle.Item2.CodigoDocumento.ToString();


                        //_linesMatrixMatrixBuilder.ActualData.Where(t => t.CodigoDocumento == codDoc.Value)
                        //     .FirstOrDefault().TransId = detalle.Item2.CodigoDocumento.ToString();
                        var detail = _linesMatrixMatrixBuilder.ActualData.Where(t => t.CodigoDocumento == codDoc.Value)
                             .FirstOrDefault();

                        _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col15").Cells.Item(eventArgs.Row).Specific;
                        if (_item.Value.ToDouble() > 0)
                        {
                            var Ajuste = detalle.Item2.ImporteDocumento - (detail.NetoParcial + detail.Comision + detail.IGV);

                            _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col26").Cells.Item(eventArgs.Row).Specific;
                            _item.Value = Ajuste.ToString();

                        }

                        _linesMatrixMatrixBuilder.AutoResizeColumns();
                        CargarTotales();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);

            }
        }

        private void linesMatrix_choose_from_list_before(object sboObject, SBOItemEventArg eventArgs, out bool BubbleEvent)
        {
            BubbleEvent = true;


            try
            {
                //SAPbouiCOM.Columns matrixColumns = _linesMatrix.Columns;
                //SAPbouiCOM.Column matrixColumn = matrixColumns.Item("col23");
                //SAPbouiCOM.Cells columnCells = matrixColumn.Cells;
                //SAPbouiCOM.EditText editText = columnCells.Item(eventArgs.Row).Specific.To<SAPbouiCOM.EditText>();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                //throw;
            }
        }

        private void linesMatrix_choose_from_list_after(object sboObject, SBOItemEventArg eventArgs)
        {
            try
            {
                if (eventArgs.ColUID == "col25")
                {
                    SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
                    if (selectedObjects == null)
                        return;

                    object value = selectedObjects.GetValue("CardCode", 0);
                    var _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col25").Cells.Item(eventArgs.Row).Specific;
                    _item.Value = value.ToString();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
