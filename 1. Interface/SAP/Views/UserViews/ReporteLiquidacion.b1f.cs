using ClosedXML.Excel;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews
{
    [FormAttribute("Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews.ReporteLiquidacion", "Views/UserViews/ReporteLiquidacion.b1f")]
    class ReporteLiquidacion : UserFormBase
    {
        public static ReporteLiquidacion INSTANCE { get; private set; }
        public const string ID = "FRM_REP_LIQ";
        public const string DESCRIPTION = "Reporte de Liquidación";
        private ISettingsDomain _settingsDomain;
        private MatrixBuilder<DocumentDetail> _linesMatrixMatrixBuilder;
        private IInfrastructureDomain _infrastructureDomain;
        private ILiquidacionTarjetasDomain _liquidacionTarjetaDomain;

        public ReporteLiquidacion()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this._detailMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("Item_0").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this._consultarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_10").Specific));
            this._consultarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._consultarButton_ClickAfter);
            this._consultarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._consultarButton_ClickBefore);
            this._exportarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_11").Specific));
            this._exportarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._exportarButton_ClickBefore);
            this._exportarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._exportarButton_ClickAfter);
            this._pasarelaEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_13").Specific));
            this._fechaInicioEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_14").Specific));
            this._fechaFinEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_15").Specific));
            this._nombreTiendaEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_16").Specific));
            this._monedaEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_17").Specific));
            this._bancoEditText = ((SAPbouiCOM.EditText)(this.GetItem("Item_18").Specific));
            this._nroOperacion = ((SAPbouiCOM.EditText)(this.GetItem("Item_19").Specific));
            this._codigoTiendaComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_20").Specific));
            this._linesMatrixDataTable = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private SAPbouiCOM.Matrix _detailMatrix;

        private void OnCustomInitialize()
        {
            _infrastructureDomain = FormHelper.GetDomain<InfrastructureDomain>();
            _liquidacionTarjetaDomain = FormHelper.GetDomain<LiquidacionTarjetasDomain>();
            _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
            try
            {
                UIAPIRawForm.Freeze(true);

                initialize_choose_from_list();
                Inicializar();
                InicializarMatrix();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Inicializar()
        {

            SAPbouiCOM.ValidValues validValues = null;
            validValues = _codigoTiendaComboBox.ValidValues;
            validValues.RemoveValidValues();
            validValues.Add(string.Empty, string.Empty);

            _liquidacionTarjetaDomain.RetrieveTiendas()
                .ForEach(t => validValues.Add(t.Item1, t.Item2));
        }

        private void initialize_choose_from_list()
        {
            SAPbouiCOM.ChooseFromList
                bancoEditTextChooseFromList,
                pasarelaPagoEditTextChooseFromList,
                monedaEditTextChooseFromList;

            make_choose_from_lists(
                out bancoEditTextChooseFromList,
         out pasarelaPagoEditTextChooseFromList,
         out monedaEditTextChooseFromList);

            append_choose_from_list_bank(_bancoEditText, bancoEditTextChooseFromList, bancoEditText_ChooseFromListAfter,
             bancoEditText_ChooseFromListBefore);

            append_choose_from_list_pasarela(_pasarelaEditText, pasarelaPagoEditTextChooseFromList, pasarelaPagoEditText_ChooseFromListAfter,
               pasarelaPagoText_ChooseFromListBefore);

            append_choose_from_list_moneda(_monedaEditText, monedaEditTextChooseFromList, monedaEditText_ChooseFromListAfter,
               monedaEditText_ChooseFromListBefore);

            GenericHelper.ReleaseCOMObjects(bancoEditTextChooseFromList, pasarelaPagoEditTextChooseFromList);

        }

        private void monedaEditText_ChooseFromListAfter()
        {
        }

        private bool monedaEditText_ChooseFromListBefore()
        {
            return true;
        }

        private void append_choose_from_list_moneda(EditText editText, ChooseFromList chooseFromList, Action afterAction, Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
            .SetAlias("CurrCode")
            //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
            //.ReferenceConditions()
            .AppendAfterAction(afterAction)
            .AppendBeforeFunction(beforeFunction);
        }

        private bool pasarelaPagoText_ChooseFromListBefore()
        {
            return true;
        }
        private void pasarelaPagoEditText_ChooseFromListAfter()
        {

        }

        private void append_choose_from_list_pasarela(EditText editText, ChooseFromList chooseFromList, Action afterAction, Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
            .SetAlias("Code")
            //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
            //.ReferenceConditions()
            .AppendAfterAction(afterAction)
            .AppendBeforeFunction(beforeFunction);
        }


        private bool bancoEditText_ChooseFromListBefore()
        {
            return true;
        }

        private void bancoEditText_ChooseFromListAfter()
        {
        }

        private void append_choose_from_list_bank(SAPbouiCOM.EditText editText, SAPbouiCOM.ChooseFromList chooseFromList, Action afterAction,
           Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
                .SetAlias("BankCode")
                //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
                //.ReferenceConditions()
                .AppendAfterAction(afterAction)
                .AppendBeforeFunction(beforeFunction);
        }

        private void make_choose_from_lists(out ChooseFromList bancoEditTextChooseFromList, out ChooseFromList pasarelaPagoEditTextChooseFromList, out ChooseFromList monedaEditTextChooseFromList)
        {
            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = UIAPIRawForm.ChooseFromLists;
            bancoEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_BN", 3);
            pasarelaPagoEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_PP", OPAP.ID);
            monedaEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_MN", 37);
            GenericHelper.ReleaseCOMObjects(chooseFromListCollection);
        }

        public class DocumentDetail : LTR1
        {
            public bool sel { get; set; }
        }
        private void InicializarMatrix()
        {
            SAPbouiCOM.ChooseFromListCollection chooseListCollection = null;
            _linesMatrixMatrixBuilder = new MatrixBuilder<DocumentDetail>(_detailMatrix, _linesMatrixDataTable);

            _detailMatrix.SelectionMode = BoMatrixSelect.ms_Single;
            try
            {
                _linesMatrixMatrixBuilder
                .AddNonEditableColumn("#", BoFormItemTypes.it_CHECK_BOX, t => t.sel)
                .AddNonEditableEditTextColumn("Código Tienda", t => t.CodigoTienda)//1
                .AddNonEditableEditTextColumn("Pasarela de Pago", t => t.PasarelaPago)//2
                .AddNonEditableEditTextColumn("Cuenta Transitoria", t => t.CuentaTransitoria)//3
                .AddNonEditableEditTextColumn("Tipo Doc.", t => t.TipoDocumento)//4
                .AddNonEditableEditTextColumn("Nro de Ticket", t => t.NroTicket)//5
                .AddNonEditableEditTextColumn("Codigo Cliente", t => t.CodigoCliente)//6
                .AddNonEditableEditTextColumn("Nombre Cliente", t => t.NombreCliente)//7
                .AddNonEditableEditTextColumn("Fecha Documento", t => t.FechaDocumento)//8
                .AddNonEditableEditTextColumn("Importe Documento", t => t.ImporteDocumento)//9
                .AddNonEditableEditTextColumn("Moneda", t => t.Moneda)//10
                .AddNonEditableLinkedColumn("Nro SAP Cobro", t => t.NroSAPCobro, "24")//11
                .AddNonEditableEditTextColumn("Nro Referencia Cobro", t => t.NroReferenciaCobro)//12
                .AddNonEditableEditTextColumn("Fecha Cobro", t => t.FechaCobro)//13
                .AddNonEditableEditTextColumn("Tipo Cambio", t => t.TipoCambio)//14
                .AddNonEditableEditTextColumn("Importe Cobrado Tarjeta", t => t.ImporteCobradoTarjeta)//15
                .AddNonEditableEditTextColumn("Codigo Documento", t => t.CodigoDocumento)//16
                .AddNonEditableEditTextColumn("Otro Medio Pago", t => t.OtroMedioPago)//17
                .AddNonEditableEditTextColumn("Comision", t => t.Comision)//18
                .AddNonEditableEditTextColumn("Comision Emisor", t => t.ComisionEmisor)//19
                .AddNonEditableEditTextColumn("Comision MCPeru", t => t.ComisionMCPeru)//20
                .AddNonEditableEditTextColumn("IGV", t => t.IGV)//21
                .AddNonEditableEditTextColumn("Neto Parcial", t => t.NetoParcial)//22
                .AddNonEditableEditTextColumn("Motivo Ajuste", t => t.MotivoAjuste)//23
                .AddNonEditableEditTextColumn("Cuenta Asignada", t => t.CuentaAsignada)//24
                .AddNonEditableEditTextColumn("Socio Negocio Asociado", t => t.SocioNegocioAsociado)//25
                .AddNonEditableEditTextColumn("Importe Ajuste", t => t.ImporteAjuste)//26
                .AddNonEditableEditTextColumn("Estado", t => t.Estado);//27


                //ColumnSetting columnSetting = _linesMatrix.Columns.Item(15).ColumnSetting;
                //columnSetting.SumType = BoColumnSumType.bst_Auto;
            }
            catch (Exception ex)
            {
                GenericHelper.ReleaseCOMObjects(chooseListCollection);
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

            _linesMatrixMatrixBuilder.AutoResizeColumns();

        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.Button _consultarButton;
        private SAPbouiCOM.Button _exportarButton;
        private SAPbouiCOM.EditText _pasarelaEditText;
        private SAPbouiCOM.EditText _fechaInicioEditText;
        private SAPbouiCOM.EditText _fechaFinEditText;
        private SAPbouiCOM.EditText _nombreTiendaEditText;
        private SAPbouiCOM.EditText _monedaEditText;
        private SAPbouiCOM.EditText _bancoEditText;
        private SAPbouiCOM.EditText _nroOperacion;
        private SAPbouiCOM.ComboBox _codigoTiendaComboBox;
        private DataTable _linesMatrixDataTable;

        private void _consultarButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            _linesMatrixMatrixBuilder.ClearData();

        }

        private void _consultarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                bool isEmpty = true;
                List<Tuple<string, string>> lista = new List<Tuple<string, string>>();

                if (!string.IsNullOrEmpty(_codigoTiendaComboBox.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_CDTD", _codigoTiendaComboBox.Value));
                }


                if (!string.IsNullOrEmpty(_pasarelaEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_PPAY", _pasarelaEditText.Value));
                }


                if (!string.IsNullOrEmpty(_fechaInicioEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_FEIV", _fechaInicioEditText.Value));
                }


                if (!string.IsNullOrEmpty(_fechaFinEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_FEFV", _fechaFinEditText.Value));
                }


                if (!string.IsNullOrEmpty(_nombreTiendaEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_DSTD", _nombreTiendaEditText.Value));
                }


                if (!string.IsNullOrEmpty(_monedaEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_MONE", _monedaEditText.Value));
                }


                if (!string.IsNullOrEmpty(_bancoEditText.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_BANK", _bancoEditText.Value));
                }


                if (!string.IsNullOrEmpty(_nroOperacion.Value))
                {
                    isEmpty = false;
                    lista.Add(new Tuple<string, string>("U_VS_LT_NROP", _nroOperacion.Value));
                }


                if (isEmpty)
                {
                    ApplicationInterfaceHelper.ShowMessageBox("Debe ingresar por lo menos un parámetro de consulta");
                }
                else
                {
                    //TODO PROCESO




                    var _linesDocuments = _liquidacionTarjetaDomain.RetrieveDetalleLiquidacion(lista);
                    if (_linesDocuments != null)
                    {
                        if (_linesDocuments.Count > 0)
                        {
                            var detail = ParseDetail(_linesDocuments);
                            _linesMatrixMatrixBuilder.SyncData(detail);
                        }
                        else
                            ApplicationInterfaceHelper.ShowErrorStatusBarMessage("No se encontraron resultados");
                    }
                    else
                        ApplicationInterfaceHelper.ShowErrorStatusBarMessage("No se encontraron resultados");


                    _linesMatrixMatrixBuilder.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private List<DocumentDetail> ParseDetail(List<LiquidationLineSL> linesDocuments)
        {
            List<DocumentDetail> list = new List<DocumentDetail>();
            //foreach (var SL in linesDocuments)
            //{
            foreach (var item in linesDocuments)
            {
                DocumentDetail line = new DocumentDetail
                {
                    sel = true,
                    CodigoCliente = item.CodigoCliente,
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
                    FechaDocumento = item.FechaDocumento ?? DateTime.Now,
                    ImporteAjuste = item.ImporteAjuste ?? 0,
                    ImporteCobradoTarjeta = item.ImporteTarjeta ?? 0,
                    ImporteDocumento = item.ImporteDocumento ?? 0,
                    Moneda = item.Moneda,
                    NetoParcial = item.NetoParcial ?? 0,
                    MotivoAjuste = item.MotivoAjuste,
                    NombreCliente = item.NombreCliente,
                    NroOperacion = item.NroOperacion,
                    NroReferenciaCobro = item.NroReferencia,
                    NroTarjeta = item.NroTarjeta,
                    NroTicket = item.NroTicket,
                    NroSAPCobro = item.NroSAPCobro ?? 0,
                    OtroMedioPago = item.OtroMedioPago ?? 0,
                    PasarelaPago = item.Pasarela,
                    SocioNegocioAsociado = item.SocioNegocioAsociado,
                    TipoCambio = item.TipoCambio ?? 0,
                    TipoDocumento = item.TipoDocumento


                };
                list.Add(line);
            }
            //}


            return list;
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {

            try
            {
                if (_detailMatrix != null)
                    _detailMatrix.AutoResizeColumns();

            }
            catch (Exception)
            {
            }
        }

        private void _exportarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (_linesMatrixMatrixBuilder.SyncedData.Count() > 0)
                {

                    var success = generate_excel_report();

                    if (success)
                        ApplicationInterfaceHelper.ShowSuccessStatusBarMessage($"Se han procesado un total de ");
                    else
                        throw new Exception("Error al generar el reporte");

                }

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void _exportarButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;


        }

        private bool generate_excel_report()
        {
            //IList<PickingSheet> report = _reportDomain
            //    .RetrievePickingExcel(entryRoute, isRoute, true)
            //    .ToList();
            var saveFileDialog = new FileDialog();
            string filePath = saveFileDialog.SaveFile(@"Excel File|*.xls;*.xlsx");

            while (string.IsNullOrEmpty(filePath) && string.IsNullOrEmpty(saveFileDialog.ruta))
            {
            }

            filePath = string.IsNullOrEmpty(filePath) ? saveFileDialog.ruta : filePath;

            if (filePath == "xxx")
            {
                return false;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(@"Reporte");

                worksheet.Column("C").Width = 25;
                worksheet.Column("D").Width = 25;
                worksheet.Column("E").Width = 25;
                worksheet.Column("F").Width = 25;
                worksheet.Column("G").Width = 25;
                worksheet.Column("H").Width = 25;
                worksheet.Column("I").Width = 10;

                //worksheet.Cell("A1").Value = @"FECHA PEDIDO";
                //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //worksheet.Range("A1:B1").Merge();
                //worksheet.Cell("C1").Style.Fill.BackgroundColor = XLColor.LightGray;
                //worksheet.Cell("C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //worksheet.Cell("C1").Value = "xxxx";
                //worksheet.Cell("D1").Value = @"OPERADOR";
                //worksheet.Cell("D1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //worksheet.Cell("D1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //worksheet.Range("D1:E1").Merge();
                //worksheet.Cell("F1").Style.Fill.BackgroundColor = XLColor.LightGray;
                //worksheet.Cell("G1").Value = @"RUTA";
                //worksheet.Cell("G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //worksheet.Cell("G1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //worksheet.Range("G1:H1").Merge();
                //worksheet.Range("I1:J1").Style.Fill.BackgroundColor = XLColor.LightGray;
                //worksheet.Cell("I1").Value = "ruta cdrf";
                //worksheet.Cell("K1").Value = @"PRIORIDAD";
                //worksheet.Cell("K1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //worksheet.Cell("K1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //worksheet.Range("K1:O1").Merge();
                //worksheet.Range("P1:R1").Style.Fill.BackgroundColor = XLColor.LightGray;
                //worksheet.Cell("P1").Value = "ruta num";

                ////
                worksheet.Cell("A1").Value = @"Código Tienda";
                worksheet.Cell("A1").Style.Font.Bold = true;
                //worksheet.Cell("A1").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                worksheet.Cell("B1").Value = @"Pasarela de Pago";
                worksheet.Cell("B1").Style.Font.Bold = true;
                worksheet.Cell("C1").Value = @"Cuenta Transitoria";
                worksheet.Cell("C1").Style.Font.Bold = true;
                worksheet.Cell("D1").Value = @"Tipo Doc.";
                worksheet.Cell("D1").Style.Font.Bold = true;
                //worksheet.Range("C3:E3").Merge();
                worksheet.Cell("E1").Value = @"Nro de Ticket";
                worksheet.Cell("E1").Style.Font.Bold = true;
                worksheet.Cell("F1").Value = @"Codigo Cliente";
                worksheet.Cell("F1").Style.Font.Bold = true;
                worksheet.Cell("G1").Value = @"Nombre Cliente";
                worksheet.Cell("G1").Style.Font.Bold = true;
                //worksheet.Range("F3:H3").Merge();
                worksheet.Cell("H1").Value = @"Fecha Documento";
                worksheet.Cell("H1").Style.Font.Bold = true;
                worksheet.Cell("I1").Value = @"Importe Documento.";
                worksheet.Cell("I1").Style.Font.Bold = true;
                worksheet.Cell("J1").Value = @"Moneda";
                worksheet.Cell("J1").Style.Font.Bold = true;
                worksheet.Cell("K1").Value = @"Nro Sap Cobro";
                worksheet.Cell("K1").Style.Font.Bold = true;
                worksheet.Cell("L1").Value = @"Nro Referencia Cobro";
                worksheet.Cell("L1").Style.Font.Bold = true;
                worksheet.Cell("M1").Value = @"Fecha Cobro";
                worksheet.Cell("M1").Style.Font.Bold = true;
                worksheet.Cell("N1").Value = @"Tipo Cambio";
                worksheet.Cell("N1").Style.Font.Bold = true;
                worksheet.Cell("O1").Value = @"Importe Cobrado Tarjeta";
                worksheet.Cell("O1").Style.Font.Bold = true;
                worksheet.Cell("P1").Value = @"Codigo Documento";
                worksheet.Cell("P1").Style.Font.Bold = true;
                worksheet.Cell("Q1").Value = @"Otro Medio Pago";
                worksheet.Cell("Q1").Style.Font.Bold = true;
                worksheet.Cell("R1").Value = @"Comisión";
                worksheet.Cell("R1").Style.Font.Bold = true;
                worksheet.Cell("S1").Value = @"Comisión Emisor";
                worksheet.Cell("S1").Style.Font.Bold = true;
                worksheet.Cell("T1").Value = @"Comisión MCPeru";
                worksheet.Cell("T1").Style.Font.Bold = true;
                worksheet.Cell("U1").Value = @"IGV";
                worksheet.Cell("U1").Style.Font.Bold = true;
                worksheet.Cell("V1").Value = @"Neto Parcial";
                worksheet.Cell("V1").Style.Font.Bold = true;
                worksheet.Cell("W1").Value = @"Motivo Ajuste";
                worksheet.Cell("W1").Style.Font.Bold = true;
                worksheet.Cell("X1").Value = @"Cuenta Asignada";
                worksheet.Cell("X1").Style.Font.Bold = true;
                worksheet.Cell("Y1").Value = @"Socio Negocio Asociado";
                worksheet.Cell("Y1").Style.Font.Bold = true;
                worksheet.Cell("Z1").Value = @"Importe Ajuste";
                worksheet.Cell("Z1").Style.Font.Bold = true;
                worksheet.Cell("AA1").Value = @"Estado";
                worksheet.Cell("AA1").Style.Font.Bold = true;

                var rowNumber = 1;
                foreach (var item in _linesMatrixMatrixBuilder.SyncedData)
                {
                    rowNumber++;
                    worksheet.Cell($"A{rowNumber}").Value = item.CodigoTienda;
                    worksheet.Cell($"A{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"B{rowNumber}").Value = item.PasarelaPago;
                    worksheet.Cell($"B{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"C{rowNumber}").Value = item.CuentaTransitoria;
                    worksheet.Cell($"C{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"D{rowNumber}").Value = item.TipoDocumento;
                    worksheet.Cell($"D{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"D{rowNumber}").Style.Alignment.WrapText = true;
                    //worksheet.Range($"C{rowNumber}:E{rowNumber}").Merge();
                    worksheet.Cell($"E{rowNumber}").Value = "'" + item.NroTicket;
                    worksheet.Cell($"E{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //worksheet.Cell($"E{rowNumber}").Style.NumberFormat.Format = "@";
                    worksheet.Cell($"F{rowNumber}").Value = item.CodigoCliente;
                    worksheet.Cell($"F{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"G{rowNumber}").Value = item.NombreCliente;
                    worksheet.Cell($"G{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"G{rowNumber}").Style.Alignment.WrapText = true;
                    //worksheet.Range($"F{rowNumber}:H{rowNumber}").Merge();

                    worksheet.Cell($"H{rowNumber}").Value = item.FechaDocumento;
                    worksheet.Cell($"H{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"I{rowNumber}").Value = item.ImporteDocumento;
                    worksheet.Cell($"I{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"J{rowNumber}").Value = item.Moneda;
                    worksheet.Cell($"J{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"K{rowNumber}").Value = item.NroSAPCobro;
                    worksheet.Cell($"K{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"L{rowNumber}").Value = item.NroReferenciaCobro;
                    worksheet.Cell($"L{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"M{rowNumber}").Value = item.FechaCobro;
                    worksheet.Cell($"M{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"N{rowNumber}").Value = item.TipoCambio;
                    worksheet.Cell($"N{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"O{rowNumber}").Value = item.ImporteCobradoTarjeta;
                    worksheet.Cell($"O{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"P{rowNumber}").Value = item.CodigoDocumento;
                    worksheet.Cell($"P{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"Q{rowNumber}").Value = item.OtroMedioPago;
                    worksheet.Cell($"Q{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"R{rowNumber}").Value = item.Comision;
                    worksheet.Cell($"R{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"S{rowNumber}").Value = item.ComisionEmisor;
                    worksheet.Cell($"S{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"T{rowNumber}").Value = item.ComisionMCPeru;
                    worksheet.Cell($"T{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"U{rowNumber}").Value = item.IGV;
                    worksheet.Cell($"U{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"V{rowNumber}").Value = item.NetoParcial;
                    worksheet.Cell($"V{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"W{rowNumber}").Value = item.MotivoAjuste;
                    worksheet.Cell($"W{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"X{rowNumber}").Value = item.CuentaAsignada;
                    worksheet.Cell($"X{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"Y{rowNumber}").Value = item.SocioNegocioAsociado;
                    worksheet.Cell($"Y{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"Z{rowNumber}").Value = item.ImporteAjuste;
                    worksheet.Cell($"Z{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell($"AA{rowNumber}").Value = item.Estado;
                    worksheet.Cell($"AA{rowNumber}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Row(rowNumber).Height = 60;
                }

                worksheet.Column("A").AdjustToContents();
                worksheet.Column("B").AdjustToContents();

                worksheet.Column("P").AdjustToContents();

                workbook.SaveAs(filePath);
                return true;
            }
        }

    }
}
