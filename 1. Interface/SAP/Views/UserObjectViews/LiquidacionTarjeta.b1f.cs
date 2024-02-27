using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews
{
    [FormAttribute("UDO_FT_VS_LT_OLTR")]
    public partial class LiquidacionTarjeta : UDOFormBase
    {
        private IInfrastructureDomain _infrastructureDomain;
        private ILiquidacionTarjetasDomain _liquidacionTarjetaDomain;
        private IFileDomain _fileDomain;
        private MatrixBuilder<DocumentDetail> _linesMatrixMatrixBuilder;
        private SAPbouiCOM.ChooseFromList _ajustesChooseFromList;
        private SAPbouiCOM.ChooseFromList _socioNegocioAsociadoChooseFromList;
        private ISettingsDomain _settingsDomain;
        private string _codSocioProveedor;
        public LiquidacionTarjeta()
        {
        }
       
        public class DocumentDetail : LTR1
        {
            public bool IsSelected { get; set; }
            public string TransId { get; set; }
            public static PropertyInfo RetrievePropertyInfo<T>(Expression<Func<LTR1, T>> propertyLambda)
            {
                var member = propertyLambda.Body as MemberExpression;
                if (member == null)
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

                var propInfo = member.Member as PropertyInfo;
                if (propInfo == null)
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

                Type type = typeof(LTR1);
                if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                    throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

                return propInfo;
            }
        }
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this._DocEntryEditText = ((SAPbouiCOM.EditText)(this.GetItem("0_U_E").Specific)); 
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.btnSeleccionar = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.btnSeleccionar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSeleccionar_ClickBefore);
            this.btnSeleccionar.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnSeleccionar_ClickAfter);
            this.btnCargaArchivo = ((SAPbouiCOM.Button)(this.GetItem("Item_6").Specific));
            this.btnCargaArchivo.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCargaArchivo_ClickAfter);
            this.btnCargaArchivo.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCargaArchivo_ClickBefore);
            this._archivoEditText = ((SAPbouiCOM.EditText)(this.GetItem("27_U_E").Specific));
            this._tipoCambioEditText = ((SAPbouiCOM.EditText)(this.GetItem("32_U_E").Specific));
            this._fechaBonoEditText = ((SAPbouiCOM.EditText)(this.GetItem("28_U_E").Specific));
            this._fechaBonoEditText.LostFocusAfter += new SAPbouiCOM._IEditTextEvents_LostFocusAfterEventHandler(this._fechaBonoEditText_LostFocusAfter);
            this._bancoEditText = ((SAPbouiCOM.EditText)(this.GetItem("29_U_E").Specific));
            this._cuentaContableEditText = ((SAPbouiCOM.EditText)(this.GetItem("33_U_E").Specific));
            this._nombreTiendaEditText = ((SAPbouiCOM.EditText)(this.GetItem("21_U_E").Specific));
            this._fechaInicioVentasEditText = ((SAPbouiCOM.EditText)(this.GetItem("24_U_E").Specific));
            this._fechaFinVentasEditText = ((SAPbouiCOM.EditText)(this.GetItem("25_U_E").Specific));
            this._cuentaTransitoriaEditText = ((SAPbouiCOM.EditText)(this.GetItem("23_U_E").Specific));
            this._monedaEditText = ((SAPbouiCOM.EditText)(this.GetItem("26_U_E").Specific));
            this._codigoTiendaComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_1").Specific));
            this._codigoTiendaComboBox.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this._codigoTiendaComboBox_ComboSelectAfter);
            this._pasarelaPagoEditText = ((SAPbouiCOM.EditText)(this.GetItem("22_U_E").Specific));
            this._linesMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
            this._linesMatrix.DatasourceLoadAfter += new SAPbouiCOM._IMatrixEvents_DatasourceLoadAfterEventHandler(this._linesMatrix_DatasourceLoadAfter);
            this._linesMatrixDataTable = this.UIAPIRawForm.DataSources.DataTables.Item("DT_LM");
            this._importeCobradoEditText = ((SAPbouiCOM.EditText)(this.GetItem("34_U_E").Specific));
            this._otroMedioPagoEditText = ((SAPbouiCOM.EditText)(this.GetItem("35_U_E").Specific));
            this._comisionEmisorEditText = ((SAPbouiCOM.EditText)(this.GetItem("37_U_E").Specific));
            this._comisionEditText = ((SAPbouiCOM.EditText)(this.GetItem("36_U_E").Specific));
            this._comisionMCPERUEditText = ((SAPbouiCOM.EditText)(this.GetItem("38_U_E").Specific));
            this._netoParcialEditText = ((SAPbouiCOM.EditText)(this.GetItem("40_U_E").Specific));
            this._igvEditText = ((SAPbouiCOM.EditText)(this.GetItem("39_U_E").Specific));
            this._sumaComisionesEditText = ((SAPbouiCOM.EditText)(this.GetItem("41_U_E").Specific));
            this._importeAjusteEditText = ((SAPbouiCOM.EditText)(this.GetItem("43_U_E").Specific));
            this._importeDepositadoEditText = ((SAPbouiCOM.EditText)(this.GetItem("42_U_E").Specific));
            this._totalGeneralEditText = ((SAPbouiCOM.EditText)(this.GetItem("44_U_E").Specific));
            this._flujoRelevanteComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_2").Specific));
            this._ejecutarButton = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this._ejecutarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._ejecutarButton_ClickBefore);
            this._ejecutarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._ejecutarButton_ClickAfter);
            this._nroOperacionEditText = ((SAPbouiCOM.EditText)(this.GetItem("31_U_E").Specific));
            this._linesMatrix.ChooseFromListAfter += this.linesMatrix_choose_from_list_after;
            this._linesMatrix.ChooseFromListBefore += this.linesMatrix_choose_from_list_before;
            this._linesMatrix.ValidateAfter += this.lineMatrix_validate_after; 
            this.OnCustomInitialize();

        }


        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.DataAddAfter += new DataAddAfterHandler(this.Form_DataAddAfter);

        }

        private EditText _DocEntryEditText;
        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            UIAPIRawForm.Freeze(true);
            try
            {
                _infrastructureDomain = FormHelper.GetDomain<InfrastructureDomain>();
                _liquidacionTarjetaDomain = FormHelper.GetDomain<LiquidacionTarjetasDomain>();
                _fileDomain = FormHelper.GetDomain<FilesDomain>();
                _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
                if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    initialize_choose_from_list();
                    Inicializar();

                    InicializarMatrix();
                    listDetailLiquidation = new List<LiquidationLineSL>();
                    //PRUEBAS
                    _codigoTiendaComboBox.SelectByValue(25);
                    _pasarelaPagoEditText.Value = "NIUBIZ";
                    _archivoEditText.Value = "C:\\Proyectos Ventura\\Azaleia\\Comisión de Tarjetas\\Proyecto\\NIUBIZ Caso2.xlsx";
                    _fechaBonoEditText.Value = DateTime.Now.ToString("yyyyMMdd");
                    _bancoEditText.Value = "BCP";
                    //_fechaFinVentasEditText.Value = "20240106";
                    //_fechaInicioVentasEditText.Value = "20240105";
                    _cuentaContableEditText.Value = "1041102";
                    _codSocioProveedor = "PL20341198217";
                }
                initialize_data_source();

                

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
        private void InicializarMatrix()
        {
            try
            {
                int cont = _linesMatrix.Columns.Count;

                for (int i = 0; i < cont; i++)
                {
                    _linesMatrix.Columns.Remove(0);
                }
            }
            catch (Exception ex)
            {

            }


            SAPbouiCOM.ChooseFromListCollection chooseListCollection = null;

            _linesMatrixMatrixBuilder = new MatrixBuilder<DocumentDetail>(_linesMatrix, _linesMatrixDataTable);

            //chooseListCollection = UIAPIRawForm.ChooseFromLists;
            //_ajustesChooseFromList = chooseListCollection.Add(ApplicationInterfaceHelper.MakeChooseListParams("AJU_CFL", "@"+OMAJ.ID, false));

            //SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = UIAPIRawForm.ChooseFromLists;
            //_ajustesChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"AJU_CFL", OMAJ.ID);

            //append_choose_from_list_Ajuste(_bancoEditText, _ajustesChooseFromList, bancoEditText_ChooseFromListAfter,
            //    bancoEditText_ChooseFromListBefore);


            //ChooseFromListCreationParams chooseListParams =
            //    ApplicationInterfaceHelper.MakeChooseListParams("AJU_CFL", OMAJ.ID, false);
            //_ajustesChooseFromList = UIAPIRawForm.ChooseFromLists.Add(chooseListParams);

            ChooseFromListCreationParams chooseListParams =
           ApplicationInterfaceHelper.MakeChooseListParams("SNA_CFL", SAPConstanst.ObjectType.BUSINESS_PARTNER, false);
            _socioNegocioAsociadoChooseFromList = UIAPIRawForm.ChooseFromLists.Add(chooseListParams);

            _linesMatrix.SelectionMode = BoMatrixSelect.ms_Single;
            try
            {
                _linesMatrixMatrixBuilder
                .AddEditableColumn("Seleccionar", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX, t => t.IsSelected)//0
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
                .AddEditableEditTextColumn("Codigo Documento", t => t.CodigoDocumento)//16
                .AddNonEditableEditTextColumn("Otro Medio Pago", t => t.OtroMedioPago)//17
                .AddNonEditableEditTextColumn("Comision", t => t.Comision)//18
                .AddNonEditableEditTextColumn("Comision Emisor", t => t.ComisionEmisor)//19
                .AddNonEditableEditTextColumn("Comision MCPeru", t => t.ComisionMCPeru)//20
                .AddNonEditableEditTextColumn("IGV", t => t.IGV)//21
                .AddNonEditableEditTextColumn("Neto Parcial", t => t.NetoParcial)//22
                //.AddEditableColumnFromList("Motivo Ajuste", SAPbouiCOM.BoFormItemTypes.it_EDIT, t => t.MotivoAjuste, _ajustesChooseFromList, "CODE", beforeAction: MotivoAjusteEditText_ChooseFromListBefore,afterAction:MotivoAjusteEditText_ChooseFromListAfter)//23
                .AddEditableEditTextColumn("MotivoAjuste", t => t.MotivoAjuste)//23
                .AddNonEditableEditTextColumn("CuentaAsignada", t => t.CuentaAsignada)//24
                .AddEditableColumnFromList("Socio Negocio Asociado", SAPbouiCOM.BoFormItemTypes.it_EDIT, t => t.SocioNegocioAsociado, _socioNegocioAsociadoChooseFromList, "CardCode", beforeAction: socioNefocioAsociadoEditText_ChooseFromListBefore, afterAction: socioNefocioAsociadoEditText_ChooseFromListAfter)//23
                //.AddEditableEditTextColumn("SocioNegocioAsociado", t => t.SocioNegocioAsociado)//25
                .AddNonEditableEditTextColumn("ImporteAjuste", t => t.ImporteAjuste)//26
                .AddNonEditableEditTextColumn("Estado", t => t.Estado)//27
                .AddInvisibleColumn(t => t.TransId);

                //ColumnSetting columnSetting = _linesMatrix.Columns.Item(15).ColumnSetting;
                //columnSetting.SumType = BoColumnSumType.bst_Auto;
            }
            catch (Exception ex)
            {
                GenericHelper.ReleaseCOMObjects(chooseListCollection);
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }



            _linesMatrixMatrixBuilder.AutoResizeColumns();

            _linesMatrix.Columns.Item("col15").ColumnSetting.SumType = BoColumnSumType.bst_Auto;
            _linesMatrix.Columns.Item("col17").ColumnSetting.SumType = BoColumnSumType.bst_Auto;

        }

        private void socioNefocioAsociadoEditText_ChooseFromListAfter(object sboObject, SBOItemEventArg eventArgs)
        {

            SAPbouiCOM.DataTable selectedObjects = eventArgs.To<SAPbouiCOM.ISBOChooseFromListEventArg>().SelectedObjects;
            if (selectedObjects == null)
                return;

            object value = selectedObjects.GetValue("CardCode", 0);
            var _item = (SAPbouiCOM.EditText)_linesMatrix.Columns.Item("col23").Cells.Item(eventArgs.Row).Specific;
            _item.Value = value.ToString();

        }

        private void socioNefocioAsociadoEditText_ChooseFromListBefore(object column, SBOItemEventArg eventArg, out bool handleEvent)
        {
            handleEvent = true;
        }

        private void MotivoAjusteEditText_ChooseFromListAfter(object arg1, SBOItemEventArg arg2)
        {
            //throw new NotImplementedException();
        }

        private void MotivoAjusteEditText_ChooseFromListBefore(object column, SBOItemEventArg eventArg, out bool handleEvent)
        {
            handleEvent = true;
        }



        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.Button btnSeleccionar;
        private SAPbouiCOM.Button btnCargaArchivo;
        private EditText _archivoEditText;
        private EditText _tipoCambioEditText;

        private void btnSeleccionar_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            try
            {

                var _openFileDialog = new FileDialog();

                string filePath = _openFileDialog.FindFile(@"Excel File|*.xls;*.xlsx");

                while (string.IsNullOrEmpty(filePath))
                {
                }

                filePath = string.IsNullOrEmpty(filePath) ? _openFileDialog.ruta : filePath;

                if (string.IsNullOrEmpty(filePath))
                    throw new Exception("[Sistema] Porfavor, volver a cargar el archivo.");

                _archivoEditText.Value = filePath;


            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {

            }
        }

        private void btnSeleccionar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                BubbleEvent = false;
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(@"[Validación] Solo se puede cargar un archivo en modo 'Nuevo'.");
            }

        }

        private void btnCargaArchivo_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (_archivoEditText.Value == "")
                {
                    BubbleEvent = false;
                    ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Debe seleccionar un archivo");
                }

            }
            catch (Exception ex)
            {
            }

        }


        private void btnCargaArchivo_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            AnalizarArchivo();

        }

        private void Inicializar()
        {
            _cuentaContableEditText.Item.Enabled = false;

            SAPbouiCOM.ValidValues validValues = null;
            validValues = _codigoTiendaComboBox.ValidValues;
            validValues.RemoveValidValues();
            validValues.Add(string.Empty, string.Empty);

            _liquidacionTarjetaDomain.RetrieveTiendas()
                .ForEach(t => validValues.Add(t.Item1, t.Item2));


            validValues = null;
            validValues = _flujoRelevanteComboBox.ValidValues;
            validValues.RemoveValidValues();
            validValues.Add(string.Empty, string.Empty);
            _infrastructureDomain.RetrieveTipoFlujos()
                  .ForEach(t => validValues.Add(t.Item1, t.Item2));


            if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                _ejecutarButton.Caption = "Ejecutar";
            }
        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            _fechaBonoEditText.Item.Enabled = false;
            _cuentaContableEditText.Item.Enabled = false;
            _fechaFinVentasEditText.Item.Enabled = false;
            _fechaInicioVentasEditText.Item.Enabled = false;
            _codigoTiendaComboBox.Item.Enabled = false;
            _pasarelaPagoEditText.Item.Enabled = false;
            _bancoEditText.Item.Enabled = false;
            _cuentaContableEditText.Item.Enabled = false;
            _flujoRelevanteComboBox.Item.Enabled = false;
            _nroOperacionEditText.Item.Enabled = false;

            _linesMatrix.Item.Enabled = false;

            if (UIAPIRawForm.Mode == BoFormMode.fm_OK_MODE)
            {
                
                //TODO crear campos nuevamente
                var respuesta = _liquidacionTarjetaDomain.RetrieveDetalleLiquidacion(_DocEntryEditText.Value.ToString());

                if (respuesta != null)
                {
                    List<DocumentDetail> list = new List<DocumentDetail>();
                    list = ParseDetail(respuesta);
                    _linesMatrixMatrixBuilder.SyncData(list);
                }
                else
                {
                    _linesMatrixMatrixBuilder.ClearData();
                }
                
            }

        }

    

        private void Form_LoadAfter(SBOItemEventArg pVal)
        {

        }

        private EditText _fechaBonoEditText;
        private EditText _bancoEditText;
        private EditText _cuentaContableEditText;
        private EditText _nombreTiendaEditText;
        private EditText _fechaInicioVentasEditText;
        private EditText _fechaFinVentasEditText;
        private EditText _cuentaTransitoriaEditText;
        private EditText _monedaEditText;

        private void _fechaBonoEditText_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(_fechaBonoEditText.Value))
                {
                    _tipoCambioEditText.Value = _infrastructureDomain.obtenerTipoCambio(DateTime.ParseExact(_fechaBonoEditText.Value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)).ToString();
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void initialize_choose_from_list()
        {
            SAPbouiCOM.ChooseFromList
                bancoEditTextChooseFromList,
                cuentaContableEditTextChooseFromList,
                pasarelaPagoEditTextChooseFromList,
                tiendaEditTextChooseFromList;

            make_choose_from_lists(
                out bancoEditTextChooseFromList,
         out cuentaContableEditTextChooseFromList,
         out pasarelaPagoEditTextChooseFromList,
         out tiendaEditTextChooseFromList);

            append_choose_from_list_bank(_bancoEditText, bancoEditTextChooseFromList, bancoEditText_ChooseFromListAfter,
                bancoEditText_ChooseFromListBefore);

            append_choose_from_list_bank_detail(_cuentaContableEditText, cuentaContableEditTextChooseFromList, cuentaEditText_ChooseFromListAfter,
               cuentaEditText_ChooseFromListBefore);

            append_choose_from_list_pasarela(_pasarelaPagoEditText, pasarelaPagoEditTextChooseFromList, pasarelaPagoEditText_ChooseFromListAfter,
               pasarelaPagoText_ChooseFromListBefore);

            //append_choose_from_list_tienda(_codigoTiendaEditText, tiendaEditTextChooseFromList, codigoTiendaEditText_ChooseFromListAfter,
            //   codigoTiendaText_ChooseFromListBefore);

            GenericHelper.ReleaseCOMObjects(bancoEditTextChooseFromList, cuentaContableEditTextChooseFromList, pasarelaPagoEditTextChooseFromList, tiendaEditTextChooseFromList);
        }

        private bool codigoTiendaText_ChooseFromListBefore()
        {
            return true;
        }

        private void codigoTiendaEditText_ChooseFromListAfter()
        {
        }

        private void append_choose_from_list_tienda(EditText editText, ChooseFromList chooseFromList, Action afterAction, Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
             .SetAlias("Code")
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
            try
            {
                if (!string.IsNullOrEmpty(_codigoTiendaComboBox.Value))
                {

                    if (_codigoTiendaComboBox.Value == "-1")
                    {
                        _cuentaTransitoriaEditText.Value = "";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_pasarelaPagoEditText.Value))
                        {
                            _cuentaTransitoriaEditText.Value = _liquidacionTarjetaDomain.RetrieveCuentaTransitoria(_pasarelaPagoEditText.Value, _codigoTiendaComboBox.Value).Item1;
                            var socio = _liquidacionTarjetaDomain.RetrieveSocioNegocioPasarela(_pasarelaPagoEditText.Value);
                            if (socio.Item1)
                            {
                                _codSocioProveedor = socio.Item2;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(_pasarelaPagoEditText.Value))
                {
                    _monedaEditText.Value = _liquidacionTarjetaDomain.RetrieveMonedaPasarela(_pasarelaPagoEditText.Value);
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
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

        private bool cuentaEditText_ChooseFromListBefore()
        {
            if (string.IsNullOrEmpty(_bancoEditText.Value))
                return false;

            SAPbouiCOM.ChooseFromList cuentaEditTextChooseFromList = null;
            SAPbouiCOM.Conditions conditions = null;
            SAPbouiCOM.Condition condition = null;



            try
            {
                conditions = ApplicationInterfaceHelper.ApplicationInstance
                    .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    .To<SAPbouiCOM.Conditions>();

                condition = conditions.Add();
                condition.Alias = @"BankCode";
                condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                condition.CondVal = _bancoEditText.Value;


                cuentaEditTextChooseFromList = UIAPIRawForm.ChooseFromLists.Item(@"CFL_BD");
                cuentaEditTextChooseFromList.SetConditions(conditions);

                return true;
            }
            catch (Exception exception)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage($"[Error] {exception.Message}");
                return false;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(cuentaEditTextChooseFromList, condition, conditions);
            }
        }

        private void cuentaEditText_ChooseFromListAfter()
        {

        }

        private void append_choose_from_list_bank_detail(EditText editText, ChooseFromList chooseFromList, Action afterAction, Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
                 .SetAlias("GLAccount")
                 //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
                 //.ReferenceConditions()
                 .AppendAfterAction(afterAction)
                 .AppendBeforeFunction(beforeFunction);
        }

        private bool bancoEditText_ChooseFromListBefore()
        {
            //throw new NotImplementedException();
            return true;
        }

        private void bancoEditText_ChooseFromListAfter()
        {
            if (!string.IsNullOrEmpty(_bancoEditText.Value))
            {
                _cuentaContableEditText.Item.Enabled = true;
            }
        }

        private void make_choose_from_lists(
            out SAPbouiCOM.ChooseFromList bancoEditTextChooseFromList,
            out ChooseFromList cuentaContableEditTextChooseFromList,
            out ChooseFromList pasarelaPagoEditTextChooseFromList,
            out ChooseFromList tiendaEditTextChooseFromList)
        {
            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = UIAPIRawForm.ChooseFromLists;
            bancoEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_BN", 3);
            cuentaContableEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_BD", 231);
            pasarelaPagoEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_PP", OPAP.ID);
            tiendaEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_TI", OCEM.ID);
            GenericHelper.ReleaseCOMObjects(chooseFromListCollection);
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

        //private void append_choose_from_list_Ajuste(SAPbouiCOM.EditText editText, SAPbouiCOM.ChooseFromList chooseFromList, Action afterAction,
        //   Func<bool> beforeFunction)
        //{
        //    ChooseFromListBuilder.Reference(editText, chooseFromList)
        //        .SetAlias("Code")
        //        //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
        //        //.ReferenceConditions()
        //        .AppendAfterAction(afterAction)
        //        .AppendBeforeFunction(beforeFunction);
        //}

        private ComboBox _codigoTiendaComboBox;
        private EditText _pasarelaPagoEditText;
        private Matrix _linesMatrix;
        private DataTable _linesMatrixDataTable;
        private EditText _importeCobradoEditText;
        private EditText _otroMedioPagoEditText;
        private EditText _comisionEmisorEditText;
        private EditText _comisionEditText;
        private EditText _comisionMCPERUEditText;
        private EditText _netoParcialEditText;
        private EditText _igvEditText;
        private EditText _sumaComisionesEditText;
        private EditText _importeAjusteEditText;
        private EditText _importeDepositadoEditText;
        private EditText _totalGeneralEditText;

        private void _codigoTiendaComboBox_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!string.IsNullOrEmpty(_codigoTiendaComboBox.Value))
                {
                    _nombreTiendaEditText.Value = _codigoTiendaComboBox.Selected.Description;

                    if (_codigoTiendaComboBox.Value == "-1")
                    {
                        _cuentaTransitoriaEditText.Value = "";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_pasarelaPagoEditText.Value))
                        {
                            _cuentaTransitoriaEditText.Value = _liquidacionTarjetaDomain.RetrieveCuentaTransitoria(_pasarelaPagoEditText.Value, _codigoTiendaComboBox.Value).Item1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void _linesMatrix_DatasourceLoadAfter(object sboObject, SBOItemEventArg pVal)
        {
            CargarTotales();
        }

        private void CargarTotales()
        {
            try
            {
                _importeCobradoEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.ImporteCobradoTarjeta).ToString();
                _otroMedioPagoEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.OtroMedioPago).ToString();
                _comisionEmisorEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.ComisionEmisor).ToString();
                _comisionEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.Comision).ToString();
                _comisionMCPERUEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.ComisionMCPeru).ToString();
                _netoParcialEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.NetoParcial).ToString();
                _igvEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.IGV).ToString();

                _sumaComisionesEditText.Value = (_comisionEmisorEditText.Value.ToDecimal() + _comisionMCPERUEditText.Value.ToDecimal() + _igvEditText.Value.ToDecimal()).ToString();
                _importeDepositadoEditText.Value = _netoParcialEditText.Value;
                _importeAjusteEditText.Value = _linesMatrixMatrixBuilder.ActualData.Sum(t => t.ImporteAjuste).ToString();
                _totalGeneralEditText.Value = (_sumaComisionesEditText.Value.ToDecimal() + _importeDepositadoEditText.Value.ToDecimal() + _importeAjusteEditText.Value.ToDecimal()).ToString();
            }
            catch (Exception)
            {
            }
        }

        private ComboBox _flujoRelevanteComboBox;

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            try
            {
                if (_linesMatrixMatrixBuilder != null)
                    _linesMatrixMatrixBuilder.AutoResizeColumns();

            }
            catch (Exception)
            {
            }

        }

        private Button _ejecutarButton;
        private EditText _nroOperacionEditText;

        private void _ejecutarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {


        }

        private void _ejecutarButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    EjecutarLiquidacion();

                //BubbleEvent = false;
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);

            }

        }
        private SAPbouiCOM.DBDataSource _headerDataSource;

        private void initialize_data_source()
        {
            SAPbouiCOM.DataSource dataSource = UIAPIRawForm.DataSources;
            SAPbouiCOM.DBDataSources dbDataSources = dataSource.DBDataSources;
            _headerDataSource = dbDataSources.Item(@"@VS_LT_OLTR");
            GenericHelper.ReleaseCOMObjects(dbDataSources, dataSource);
        }
        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {
            //TODO grabar detalle
            try
            {

                var documentEntry = _headerDataSource.GetValue(@"DocEntry", 0).ToInt32();

                var respuestaPagos = _liquidacionTarjetaDomain.actualizarPagosSap(_linesMatrixMatrixBuilder.ActualData.Select(t => t.NroSAPCobro).ToList());
                var respuesta = _liquidacionTarjetaDomain.agregarDetalleLiquidacion(documentEntry.ToString(), listDetailLiquidation);

                listDetailLiquidation = new List<LiquidationLineSL>();
                _linesMatrixMatrixBuilder.ClearData();
            }
            catch (Exception ex)
            {

                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
        }

      
    }
}
