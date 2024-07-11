using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.Util;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews
{
    [FormAttribute("UDO_FT_EX_HR_OHGR")]
    public class HojaRutaAsignacion : UDOFormBase
    {
        private IInfrastructureDomain _infrastructureDomain;
        private ILiquidacionTarjetasDomain _liquidacionTarjetaDomain;
        private ISettingsDomain _settingsDomain;
        public HojaRutaAsignacion()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this._desprogramarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this._desprogramarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._desprogramarButton_ClickAfter);
            this._terminarProButton = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this._terminarProButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._terminarProButton_ClickAfter);
            this._enviarSunatButton = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this._enviarSunatButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._enviarSunatButton_ClickAfter);
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.Button3.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button3_ClickAfter);
            this._zonaRutaCheck = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_4").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this._filtrarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_6").Specific));
            this._filtrarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._filtrarButton_ClickBefore);
            this._filtrarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._filtrarButton_ClickAfter);
            this._guiaMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
            this._guiaMatrix.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this._guiaMatrix_ClickAfter);
            this._guiaMatrix.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this._guiaMatrix_ValidateAfter);
            this._hojaRutaEditText = ((SAPbouiCOM.EditText)(this.GetItem("13_U_E").Specific));
            this._transportistaEditText = ((SAPbouiCOM.EditText)(this.GetItem("14_U_E").Specific));
            this._choferEditText = ((SAPbouiCOM.EditText)(this.GetItem("15_U_E").Specific));
            this._auxiliar1EditText = ((SAPbouiCOM.EditText)(this.GetItem("16_U_E").Specific));
            this._auxiliar2EditText = ((SAPbouiCOM.EditText)(this.GetItem("17_U_E").Specific));
            this._auxiliar3EditText = ((SAPbouiCOM.EditText)(this.GetItem("18_U_E").Specific));
            this._zonaDespachoEditText = ((SAPbouiCOM.EditText)(this.GetItem("22_U_E").Specific));
            this._placaEditText = ((SAPbouiCOM.EditText)(this.GetItem("19_U_E").Specific));
            this._inicioTrasladoRutaEditText = ((SAPbouiCOM.EditText)(this.GetItem("20_U_E").Specific));
            this._finTrasladoRutaEditText = ((SAPbouiCOM.EditText)(this.GetItem("21_U_E").Specific));
            this._estadoComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("23_U_Cb").Specific));
            this._desdeEditText = ((SAPbouiCOM.EditText)(this.GetItem("26_U_E").Specific));
            this._hastaEditText = ((SAPbouiCOM.EditText)(this.GetItem("27_U_E").Specific));
            this._programadosComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("25_U_Cb").Specific));
            this._totalCargadoEditText = ((SAPbouiCOM.EditText)(this.GetItem("29_U_E").Specific));
            this._cantidadBultosEditText = ((SAPbouiCOM.EditText)(this.GetItem("31_U_E").Specific));
            this._crearButton = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this._crearButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._crearButton_ClickAfter);
            this._crearButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._crearButton_ClickBefore);
            this._codigoHojaEditText = ((SAPbouiCOM.EditText)(this.GetItem("0_U_E").Specific));
            this._programarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_7").Specific));
            this._programarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._programarButton_ClickAfter);
            this.Button6 = ((SAPbouiCOM.Button)(this.GetItem("Item_8").Specific));
            this.Button6.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button6_ClickAfter);
            this._zonaRuta = ((SAPbouiCOM.EditText)(this.GetItem("24_U_E").Specific));
            this._cargaUtil = ((SAPbouiCOM.EditText)(this.GetItem("28_U_E").Specific));
            this._diferenciaEditText = ((SAPbouiCOM.EditText)(this.GetItem("30_U_E").Specific));
            this.linkedButonHojaRuta = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_9").Specific));
            this._impresionComboButton = ((SAPbouiCOM.ButtonCombo)(this.GetItem("Item_10").Specific));
            this._impresionComboButton.ClickAfter += new SAPbouiCOM._IButtonComboEvents_ClickAfterEventHandler(this._impresionComboButton_ClickAfter);
            this._impresionComboButton.ComboSelectAfter += new SAPbouiCOM._IButtonComboEvents_ComboSelectAfterEventHandler(this._impresionComboButton_ComboSelectAfter);
            this._cancelarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_11").Specific));
            this._cancelarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._cancelarButton_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataAddBefore += new DataAddBeforeHandler(this.Form_DataAddBefore);

        }

        private SAPbouiCOM.Button _desprogramarButton;

        private void OnCustomInitialize()
        {
            initialize_choose_from_list();
            _guiaMatrix.AutoResizeColumns();
            _infrastructureDomain = FormHelper.GetDomain<InfrastructureDomain>();
            _liquidacionTarjetaDomain = FormHelper.GetDomain<LiquidacionTarjetasDomain>();
            _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
            _settingsDomain.ValidData();
            _crearButton.Caption = "Programar";
            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE)
            {
                _zonaRutaCheck.Check();
                _estadoComboBox.SelectByValue("O");
                _desdeEditText.Value = DateTime.Now.ToString("yyyyMMdd");
                _hastaEditText.Value = DateTime.Now.ToString("yyyyMMdd");
                _codigoHojaEditText.Value = _liquidacionTarjetaDomain.RetrieveCodigoGenerado();
                //_crearButton.Caption = "Programar";
                _enviarSunatButton.Item.Enabled = false;
                _programadosComboBox.Item.Enabled = false;
            }
            _enviarSunatButton.Item.Enabled = false;
            //_crearButton.Caption = "Programar";

            //_impresionComboButton.ValidValues.Add("Imprimir", "");
            //_impresionComboButton.ValidValues.Add("Imprimir2", "");
            _impresionComboButton.ValidValues.Add("1", "Imprimir GR");
            _impresionComboButton.ValidValues.Add("2", "Imprimir HR");
            _impresionComboButton.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
        }

        private void initialize_choose_from_list()
        {
            SAPbouiCOM.ChooseFromList
                hojaRutaEditTextChooseFromList;

            make_choose_from_lists(
                out hojaRutaEditTextChooseFromList);


            append_choose_from_list_hojaruta(_hojaRutaEditText, hojaRutaEditTextChooseFromList, hojaRutaEditText_ChooseFromListAfter,
               hojaRutaText_ChooseFromListBefore);


            GenericHelper.ReleaseCOMObjects(hojaRutaEditTextChooseFromList);
        }

        private bool hojaRutaText_ChooseFromListBefore()
        {
            return true;
        }

        private void hojaRutaEditText_ChooseFromListAfter()
        {
            if (!string.IsNullOrEmpty(_hojaRutaEditText.Value))
            {
                var hojadeRuta = _liquidacionTarjetaDomain.RetrieveHojaRuta(_hojaRutaEditText.Value);

                if (hojadeRuta.Item1)
                {
                    _transportistaEditText.Value = hojadeRuta.Item2.Transportista;
                    _choferEditText.Value = hojadeRuta.Item2.Chofer;
                    _auxiliar1EditText.Value = hojadeRuta.Item2.Auxiliar1;
                    _auxiliar2EditText.Value = hojadeRuta.Item2.Auxiliar2;
                    _auxiliar3EditText.Value = hojadeRuta.Item2.Auxiliar3;
                    _zonaDespachoEditText.Value = "";
                    int cont = 1;
                    foreach (var item in hojadeRuta.Item2.DetalleZonas)
                    {
                        if (cont == 1)
                        {
                            _zonaDespachoEditText.Value = item.ZonaDespacho;
                            cont++;
                        }
                        else
                        {
                            _zonaDespachoEditText.Value = _zonaDespachoEditText.Value + "-" + item.ZonaDespacho;

                        }

                    }

                    _placaEditText.Value = hojadeRuta.Item2.Placa;
                    var cargaUtil = _liquidacionTarjetaDomain.GetCargaUtilByPlaca(hojadeRuta.Item2.Placa);
                    if (cargaUtil.Item1)
                        _cargaUtil.Value = cargaUtil.Item2;
                    else
                        ApplicationInterfaceHelper.ShowErrorStatusBarMessage("No se encontró la carga util con la placa seleccionada");
                    _inicioTrasladoRutaEditText.Value = hojadeRuta.Item2.InicioTraslado.ToString("yyyyMMdd");
                    _finTrasladoRutaEditText.Value = hojadeRuta.Item2.FinTraslado.ToString("yyyyMMdd");

                }
                else
                {
                    ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Seleccione una Hoja de ruta valida");
                }
            }
            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE)
            {

                _codigoHojaEditText.Value = _liquidacionTarjetaDomain.RetrieveCodigoGenerado();

            }
        }

        private void make_choose_from_lists(
         out SAPbouiCOM.ChooseFromList hojaRutaEditTextChooseFromList)
        {
            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = UIAPIRawForm.ChooseFromLists;
            hojaRutaEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_HR", OHOJ.ID);

            GenericHelper.ReleaseCOMObjects(chooseFromListCollection);
        }

        private void append_choose_from_list_hojaruta(EditText editText, ChooseFromList chooseFromList, Action afterAction, Func<bool> beforeFunction)
        {
            ChooseFromListBuilder.Reference(editText, chooseFromList)
            .SetAlias("Code")
            //.AppendCondition(@"U_EXK_STAD", SAPbouiCOM.BoConditionOperation.co_EQUAL, OARD.Status.IN_PROGRESS)
            //.ReferenceConditions()
            .AppendAfterAction(afterAction)
            .AppendBeforeFunction(beforeFunction);
        }

        private SAPbouiCOM.Button _terminarProButton;
        private SAPbouiCOM.Button _enviarSunatButton;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.CheckBox _zonaRutaCheck;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.Button _filtrarButton;
        private Matrix _guiaMatrix;
        private EditText _hojaRutaEditText;
        private EditText _transportistaEditText;
        private EditText _choferEditText;
        private EditText _auxiliar1EditText;
        private EditText _auxiliar2EditText;
        private EditText _auxiliar3EditText;
        private EditText _zonaDespachoEditText;
        private EditText _placaEditText;
        private EditText _inicioTrasladoRutaEditText;
        private EditText _finTrasladoRutaEditText;
        private ComboBox _estadoComboBox;
        private EditText _desdeEditText;
        private EditText _hastaEditText;
        private ComboBox _programadosComboBox;
        private EditText _totalCargadoEditText;
        private EditText _cantidadBultosEditText;

        private void _filtrarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                if (_filtrarButton.IsEnabled())
                {
                    if (!string.IsNullOrEmpty(_hojaRutaEditText.Value))
                    {
                        try
                        {

                            _guiaMatrix.Clear();
                            var combo = "N";// _programadosComboBox.Selected != null ? _programadosComboBox.Selected.Value : "";
                            var listaGuias = _liquidacionTarjetaDomain.RetrieveGuiasHoja(_desdeEditText.Value, _hastaEditText.Value, combo, "Zona");
                            //var listaGuias = _liquidacionTarjetaDomain.RetrieveGuiasHoja("", "", "", "Zona");



                            int cont = 1;
                            if (listaGuias.Item1)
                            {
                                var lista = listaGuias.Item2;
                                string[] palabras = _zonaDespachoEditText.Value.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                if (_zonaRutaCheck.Checked)
                                {
                                    lista = lista.Where(t => palabras.Any(palabra => t.Zona.Contains(palabra))).ToList();
                                }

                                var totalcargado = 0.00;
                                var cantidadbultos = 0.00;
                                foreach (var item in lista)
                                {
                                    _guiaMatrix.AddRow();
                                    ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(cont).Specific).Check();// = item.NumberAtCard;
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(cont).Specific).Value = item.DocumentEntry.ToString();
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(cont).Specific).Value = item.NumberAtCard;
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(cont).Specific).Value = item.Peso;
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(cont).Specific).Value = item.CantidadBultos.ToString();
                                    ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(cont).Specific).SelectByValue(item.Programado);
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(cont).Specific).Value = item.Zona;
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(cont).Specific).Value = item.DireccionDespacho;
                                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(cont).Specific).Value = item.DepProvZona;
                                    cont++;

                                    totalcargado += item.Peso.ToDouble();
                                    cantidadbultos += item.CantidadBultos;
                                }
                                _totalCargadoEditText.Value = totalcargado.ToString();
                                _cantidadBultosEditText.Value = cantidadbultos.ToString();
                                _diferenciaEditText.Value = (_cargaUtil.Value.ToDouble() - totalcargado).ToString();

                            }
                        }
                        catch (Exception ex)
                        {

                            ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            _guiaMatrix.AutoResizeColumns();
                        }
                    }
                    else
                    {
                        ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Debe seleccionar primero una hoja de ruta");
                    }
                }
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

        private void _filtrarButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;


        }

        private Button _crearButton;
        private EditText _codigoHojaEditText;

        private void _crearButton_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            int cont = _guiaMatrix.RowCount;
            //if (UIAPIRawForm.IsAddMode())
            //{
            validarMatrix(ref BubbleEvent);
            programarGuias(ref BubbleEvent);
            //}

        }

        private void _crearButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {


        }

        private void validarMatrix(ref bool bubbleEvent)
        {
            try
            {

                List<detailGrilla> list = new List<detailGrilla>();

                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    detailGrilla line = new detailGrilla();

                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);
                    var programado = ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).Selected.Value;
                    if (check.Checked)
                    {
                        line.seleccionar = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific).Checked;
                        line.docEntry = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(i).Specific).Value;
                        line.numeroGuia = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                        line.peso = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(i).Specific).Value;
                        line.cantidadBultos = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(i).Specific).Value;
                        line.programado = ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).Selected.Value;
                        line.zona = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(i).Specific).Value;
                        line.direccion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(i).Specific).Value;
                        line.departamento = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(i).Specific).Value;
                        list.Add(line);
                    }
                    else if (programado == "Y")
                    {
                        line.seleccionar = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific).Checked;
                        line.docEntry = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(i).Specific).Value;
                        line.numeroGuia = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                        line.peso = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(i).Specific).Value;
                        line.cantidadBultos = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(i).Specific).Value;
                        line.programado = ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).Selected.Value;
                        line.zona = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(i).Specific).Value;
                        line.direccion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(i).Specific).Value;
                        line.departamento = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(i).Specific).Value;
                        list.Add(line);
                    }
                }
                UIAPIRawForm.Freeze(true);
                _guiaMatrix.Clear();

                int cont = 1;
                var totalcargado = 0.00;
                var cantidadbultos = 0.00;

                foreach (var item in list)
                {
                    _guiaMatrix.AddRow();
                    ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(cont).Specific).Check();
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(cont).Specific).Value = item.docEntry;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(cont).Specific).Value = item.numeroGuia;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(cont).Specific).Value = item.peso;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(cont).Specific).Value = item.cantidadBultos.ToString();
                    ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(cont).Specific).SelectByValue(item.programado);
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(cont).Specific).Value = item.zona;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(cont).Specific).Value = item.direccion;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(cont).Specific).Value = item.departamento;
                    cont++;

                    totalcargado += item.peso.ToDouble();
                    cantidadbultos += item.cantidadBultos.ToDouble();
                }

                _totalCargadoEditText.Value = totalcargado.ToString();
                _cantidadBultosEditText.Value = cantidadbultos.ToString();
                _diferenciaEditText.Value = (_cargaUtil.Value.ToDouble() - totalcargado).ToString();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                bubbleEvent = false;
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }


        }

        public class detailGrilla
        {
            public bool seleccionar { get; set; }
            public string docEntry { get; set; }
            public string numeroGuia { get; set; }
            public string peso { get; set; }
            public string cantidadBultos { get; set; }
            public string programado { get; set; }
            public string zona { get; set; }
            public string direccion { get; set; }
            public string departamento { get; set; }
        }

        private static string ColumnaSeleccionar = "Col_0";
        private static string ColumnaDocEntry = "Col_1";
        private static string ColumnaNumeroGuia = "C_0_1";
        private static string ColumnaPeso = "C_0_2";
        private static string ColumnaCantBultos = "C_0_3";
        private static string ColumnaProgramado = "C_0_4";
        private static string ColumnaZona = "C_0_5";
        private static string ColumnaDireccion = "C_0_6";
        private static string ColumnaDepartamento = "C_0_7";

        private Button _programarButton;

        private void _programarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {

        }

        private void programarGuias(ref bool bubbleEvent)
        {



            try
            {

                UIAPIRawForm.Freeze(true);
                ODLN datosguia = new ODLN();
                datosguia.CodigoTransportista = _transportistaEditText.Value;
                var transportista = _infrastructureDomain.RetrieveBusinessPartner(_transportistaEditText.Value);
                if (transportista == null)
                {
                    throw new Exception("No existe el transportista registrado");
                }
                datosguia.NombreTransportista = transportista.CardName;
                datosguia.RucTransportista = transportista.LicTradNum;

                if (transportista .Addresses==null)
                {
                    throw new Exception("No existe direcciones registradas en el transportista");
                }
                if (transportista.Addresses.Count==0)
                {
                    throw new Exception("No existe direcciones registradas en el transportista");
                }
                datosguia.DireccionTransportista = transportista.Addresses.FirstOrDefault().Street;
                datosguia.TipoOperacion = "01";
                datosguia.MotivoTraslado = "01";
                if (transportista.Contact == null)
                {
                    throw new Exception("No existe choferes(contacto) registrados en el transportista");
                }
                if (transportista.Contact.Count == 0)
                {
                    throw new Exception("No existe choferes(contacto) registrados en el transportista");
                }

                datosguia.LicenciaConductor = transportista.Contact.Where(t => t.ID == _choferEditText.Value).FirstOrDefault().Licencia;
                datosguia.NombreConductor = _choferEditText.Value;
                datosguia.FechaInicioTraslado = _inicioTrasladoRutaEditText.GetDateTimeValue();
                datosguia.FEXModalidadTraslado = "02";
                datosguia.PlacaVehiculo = _placaEditText.Value;
                datosguia.HojaRuta = _hojaRutaEditText.Value;
                datosguia.EstadoEnvioSunat = "N";


                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);
                    var cantBultos = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(i).Specific);
                    var numguia = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific);
                    if (check.Checked)
                    {
                        if (cantBultos.Value.ToDouble() > 0)
                        {
                            ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).SelectByValue("Y");

                            datosguia.CantidadBultos = cantBultos.Value.ToDouble();
                            _liquidacionTarjetaDomain.ActualizarProgramado(numguia.Value, "Y", datosguia);
                        }
                        else
                        {

                            throw new Exception("No se puede programar la guía " + numguia.Value + " con cantidad de bultos 0");
                        }


                    }

                }
                //if (_crearButton.Caption != "OK")
                //    _crearButton.Item.Click();
            }
            catch (Exception ex)
            {
                bubbleEvent = false;
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizaron las guías de remisión");
            }



        }

        private void _desprogramarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_desprogramarButton.IsEnabled())
            {
                if (UIAPIRawForm.IsAddMode())
                {
                    //ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Primero debe crear la hoja de asignación");
                }
                else
                {
                    desprogramar();
                }

            }

        }

        private void desprogramar()
        {
            try
            {
                ODLN datosguia = new ODLN();
                UIAPIRawForm.Freeze(true);
                datosguia.CodigoTransportista = "";
                var transportista = _infrastructureDomain.RetrieveBusinessPartner(_transportistaEditText.Value);
                datosguia.NombreTransportista = "";
                datosguia.RucTransportista = "";
                datosguia.DireccionTransportista = "";
                datosguia.TipoOperacion = "";
                datosguia.MotivoTraslado = "";
                datosguia.LicenciaConductor = "";
                datosguia.NombreConductor = "";
                datosguia.FechaInicioTraslado = _inicioTrasladoRutaEditText.GetDateTimeValue();
                datosguia.FEXModalidadTraslado = "02";
                datosguia.PlacaVehiculo = "";
                datosguia.HojaRuta = "";
                datosguia.EstadoEnvioSunat = "N";

                List<detailGrilla> list = new List<detailGrilla>();

                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    detailGrilla line = new detailGrilla();
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);
                    var programado = ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).Selected.Value;
                    if (check.Checked)
                    {
                        ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).SelectByValue("N");
                        var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                        _liquidacionTarjetaDomain.ActualizarProgramado(numeracion, "N", datosguia);
                    }
                    else
                    {
                        line.seleccionar = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific).Checked;
                        line.docEntry = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(i).Specific).Value;
                        line.numeroGuia = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                        line.peso = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(i).Specific).Value;
                        line.cantidadBultos = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(i).Specific).Value;
                        line.programado = ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(i).Specific).Selected.Value;
                        line.zona = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(i).Specific).Value;
                        line.direccion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(i).Specific).Value;
                        line.departamento = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(i).Specific).Value;
                        list.Add(line);
                    }
                }

                UIAPIRawForm.Freeze(true);
                _guiaMatrix.Clear();

                int cont = 1;
                var totalcargado = 0.00;
                var cantidadbultos = 0.00;

                foreach (var item in list)
                {
                    _guiaMatrix.AddRow();
                    ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(cont).Specific).Check();
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDocEntry).Cells.Item(cont).Specific).Value = item.docEntry;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(cont).Specific).Value = item.numeroGuia;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(cont).Specific).Value = item.peso;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(cont).Specific).Value = item.cantidadBultos.ToString();
                    ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(ColumnaProgramado).Cells.Item(cont).Specific).SelectByValue(item.programado);
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaZona).Cells.Item(cont).Specific).Value = item.zona;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDireccion).Cells.Item(cont).Specific).Value = item.direccion;
                    ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaDepartamento).Cells.Item(cont).Specific).Value = item.departamento;
                    cont++;

                    totalcargado += item.peso.ToDouble();
                    cantidadbultos += item.cantidadBultos.ToDouble();
                }

                _totalCargadoEditText.Value = totalcargado.ToString();
                _cantidadBultosEditText.Value = cantidadbultos.ToString();
                _diferenciaEditText.Value = (_cargaUtil.Value.ToDouble() - totalcargado).ToString();

                _crearButton.Item.Click();
                //if (_crearButton.Caption != "OK")
                //    _crearButton.Item.Click();



                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizaron las guías de remisión");
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

        private SAPbouiCOM.DBDataSource _headerDataSource;
        //TERMINAR PROGRAMACION
        private void _terminarProButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_terminarProButton.IsEnabled())
            {
                try
                {
                    ApplicationInterfaceHelper.ShowDialogMessageBox("¿Está seguro de terminar la programación?",
                            () =>
                            {
                                UIAPIRawForm.Freeze(true);
                                _estadoComboBox.SelectByValue("T");

                                _liquidacionTarjetaDomain.ActualizarEstadoHojaGuia("T", _codigoHojaEditText.Value);
                                _enviarSunatButton.Item.Enabled = true;
                                _terminarProButton.Item.Enabled = false;
                                _programarButton.Item.Enabled = false;
                                _desprogramarButton.Item.Enabled = false;

                                //if (_crearButton.Caption != "OK")
                                //    _crearButton.Item.Click();
                                UIAPIRawForm.Refresh();

                                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizó la hoja de ruta de asignación");
                            },
                             null);



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

        }

        private Button Button6;
        private EditText _zonaRuta;
        private EditText _cargaUtil;
        private EditText _diferenciaEditText;

        private void Button6_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                _estadoComboBox.SelectByValue("O");
                _liquidacionTarjetaDomain.ActualizarEstadoHojaGuia("O", _codigoHojaEditText.Value);
                _enviarSunatButton.Item.Enabled = false;
                _terminarProButton.Item.Enabled = true;
                _programarButton.Item.Enabled = true;
                _desprogramarButton.Item.Enabled = true;
                //_crearButton.Item.Click();
            }
            catch (Exception)
            {

            }

        }

        private void _enviarSunatButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (_enviarSunatButton.IsEnabled())
                {
                    ApplicationInterfaceHelper.ShowDialogMessageBox("¿Está seguro de enviar las guías a SUNAT?",
                    () =>
                    {
                        UIAPIRawForm.Freeze(true);
                        for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                        {
                            var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);

                            //((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("N");
                            var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                            _liquidacionTarjetaDomain.ActualizarEnvioSunat(numeracion);

                        }
                        ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizó el estado de SUNAT de las guías de remisión");
                    },
                    null);
                }




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

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            _crearButton.Caption = "Programar";
            _guiaMatrix.AutoResizeColumns();
            //if (_estadoComboBox.Selected.Value == "T")
            //{
            //    _enviarSunatButton.Item.Enabled = true;
            //    _terminarProButton.Item.Enabled = false;
            //    _programarButton.Item.Enabled = false;
            //    _desprogramarButton.Item.Enabled = false;
            //    _filtrarButton.Item.Enabled = false;
            //    _programadosComboBox.Item.Enabled = false;
            //}
            //else
            //{

            //}
            bloquearCampos();
        }

        static byte[] DecodePdfText(string pdfText)
        {
            // Convertir el texto PDF a bytes
            byte[] pdfBytes = System.Text.Encoding.UTF8.GetBytes(pdfText);

            // Devolver los bytes del PDF decodificado
            return pdfBytes;
        }

        private void Button3_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {


        }

        private LinkedButton linkedButonHojaRuta;

        private void Form_LoadAfter(SBOItemEventArg pVal)
        {
            try
            {

            }
            catch (Exception)
            {

            }

        }

        private ButtonCombo _impresionComboButton;


        bool eventButtonCombo = false;
        private void _impresionComboButton_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                bloquearCampos();

                eventButtonCombo = true;
                var x = pVal;
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void bloquearCampos()
        {
            if (_estadoComboBox.Selected.Value == "T")
            {
                _enviarSunatButton.Item.Enabled = true;
                _terminarProButton.Item.Enabled = false;
                _programarButton.Item.Enabled = false;
                _desprogramarButton.Item.Enabled = false;
                _filtrarButton.Item.Enabled = false;
                _programadosComboBox.Item.Enabled = false;

            }
            else if (_estadoComboBox.Selected.Value == "O")
            {
                _enviarSunatButton.Item.Enabled = false;
                _terminarProButton.Item.Enabled = true;
                _programarButton.Item.Enabled = true;
                _desprogramarButton.Item.Enabled = true;
                _filtrarButton.Item.Enabled = true;
                _programadosComboBox.Item.Enabled = true;
            }


        }

        private void _impresionComboButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {

                if (eventButtonCombo)
                    eventButtonCombo = false;
                else
                {
                    if (_impresionComboButton.Selected.Value == "1")
                        imprimirGR();
                    else
                        imprimirHR();
                }

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void imprimirHR()
        {
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();

            // Crear una instancia del informe de Crystal Reports
            ReportDocument reportDocument = new ReportDocument();

            try
            {
                PrinterSettings printerSettings = new PrinterSettings();

                // Verifica si hay una impresora predeterminada configurada
                var impresoraDefecto = "";
                if (printerSettings.IsDefaultPrinter)
                {
                    impresoraDefecto = printerSettings.PrinterName;
                }
                else
                {
                    throw new Exception("No hay una impresora predeterminada configurada.");
                }

                var sPath = System.Windows.Forms.Application.StartupPath;

                var rutaCrystal = _settingsDomain.RutaCompartida.Value;

                // Cargar el archivo del informe de Crystal Reports
                try
                {

                    reportDocument.Load($@"{rutaCrystal}\ReporteAsignacionRuta.rpt");
                    //reportDocument.Load(@"C:\Reporte\ReporteAsignacionRuta.rpt"); // Reemplaza "ruta\al\informe.rpt" con la ruta real de tu informe

                }
                catch (Exception)
                {
                    var bin = sPath + "\\Reports\\ReporteAsignacionRuta.rpt";
                    //reportDocument.Load(@"\\192.168.1.215\Instaladores\ReporteAddon.rpt");
                    reportDocument.Load(bin);
                }
                reportDocument.SetDatabaseLogon(_settingsDomain.UserDB.Value, _settingsDomain.PassDB.Value);
                reportDocument.SetParameterValue(0, _codigoHojaEditText.Value);


                //var ServerName = "192.168.1.215:30015"; // Change to your new IP address
                //var DatabaseName = "ZSBO_KAMASA_DESARROLLO"; // Change to your new database name
                //var UserID = _settingsDomain.UserDB.Value; // Change to your database username
                //var Password = _settingsDomain.PassDB.Value; // Change to your database password

                //reportDocument.SetDatabaseLogon(_settingsDomain.UserDB.Value, _settingsDomain.PassDB.Value,ServerName,DatabaseName);

                //reportDocument.SetParameterValue(0, _codigoHojaEditText.Value);
                //TableLogOnInfo logOnInfo = reportDocument.Database.Tables[0].LogOnInfo;
                //ConnectionInfo connectionInfo = logOnInfo.ConnectionInfo;
                //connectionInfo.ServerName = ServerName; // Change to your new IP address
                //connectionInfo.DatabaseName = DatabaseName; // Change to your new database name
                //connectionInfo.UserID = _settingsDomain.UserDB.Value; // Change to your database username
                //connectionInfo.Password = _settingsDomain.PassDB.Value; // Change to your database password
                //connectionInfo.Type = ConnectionInfoType.CRQE;

                //foreach (Table table in reportDocument.Database.Tables)
                //{
                //    table.LogOnInfo.ConnectionInfo = connectionInfo;
                //    table.ApplyLogOnInfo(logOnInfo);

                //}
                //string strConnection = string.Format("DRIVER={0};UID={1};PWD={2};SERVERNODE={3};CS={4};", "{HDBODBC}",
                //    UserID, Password, ServerName, DatabaseName);//HDBODBC32 HDBODBC

                //NameValuePairs2 logonProps2 = reportDocument.DataSourceConnections[0].LogonProperties;

                //logonProps2.Set("Provider", "HDBODBC");
                //logonProps2.Set("Server Type", "HDBODBC");
                //logonProps2.Set("Connection String", strConnection);
                //reportDocument.DataSourceConnections[0].SetLogonProperties(logonProps2);

                ////reportDocument.Refresh();
                //reportDocument.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                ////reportDocument.SetParameterValue(0, _codigoHojaEditText.Value);

                //ParameterField parameterField = new ParameterField();
                //parameterField.Name = "Code"; // Replace with your parameter name
                //ParameterDiscreteValue parameterValue = new ParameterDiscreteValue();
                //parameterValue.Value = _codigoHojaEditText.Value; // Replace with your parameter value
                //parameterField.CurrentValues.Add(parameterValue);
                //reportDocument.DataDefinition.ParameterFields["Code"].ApplyCurrentValues(parameterField.CurrentValues);


                // Asignar el informe al visor de informes
                //crystalReportViewer.ReportSource = reportDocument;
                //reportDocument.SetDatabaseLogon(_settingsDomain.UserDB.Value, _settingsDomain.PassDB.Value);
                reportDocument.PrintOptions.PrinterName = impresoraDefecto;
                reportDocument.PrintToPrinter(1, true, 1, 1);


            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                Console.WriteLine($"Error al abrir el informe: {ex.Message}");
            }
            finally
            {
                // Liberar recursos
                reportDocument.Close();
                reportDocument.Dispose();
            }
            ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Finalizó la impresión");
        }

        private void imprimirGR()
        {
            try
            {
                PrinterSettings printerSettings = new PrinterSettings();

                // Verifica si hay una impresora predeterminada configurada
                var impresoraDefecto = "";
                if (printerSettings.IsDefaultPrinter)
                {
                    impresoraDefecto = printerSettings.PrinterName;
                }
                else
                {
                    throw new Exception("No hay una impresora predeterminada configurada.");
                }


                UIAPIRawForm.Freeze(true);

                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);

                    if (check.Checked)
                    {
                        //((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("N");
                        var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                        var odfstream = _liquidacionTarjetaDomain.ObtenerPDF(numeracion);

                        if (odfstream.Item1)
                        {
                            //// Guardar el PDF decodificado en un archivo temporal
                            string tempFilePath = Path.GetTempFileName() + ".pdf";
                            try
                            {
                                //File.WriteAllBytes(tempFilePath, odfstream.Item2);

                                Spire.Pdf.PdfDocument pdfdocument = new Spire.Pdf.PdfDocument();
                                pdfdocument.LoadFromBytes(odfstream.Item2);
                                //pdf.PrinterName = impresoraDefecto;//printerName;
                                //pdf.Copies = 2;
                                //pdfdocument.Print(pdf);

                                pdfdocument.PrintSettings.PrinterName = impresoraDefecto;//printerName;              
                                pdfdocument.PrintSettings.Copies = 1;
                                pdfdocument.Print();
                                pdfdocument.Dispose();

                                //// Abrir el archivo PDF en el navegador predeterminado
                                //Process.Start(tempFilePath);
                            }
                            catch (Exception ex)
                            {
                                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Error al obtener PDF del documento: " + numeracion);
                            }

                        }
                        else
                        {
                            ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Error al obtener PDF del documento: " + numeracion);
                        }

                    }


                }


            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se terminó el proces");
            }
        }

        private void _guiaMatrix_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            bloquearCampos();
            actualizarTotales();

        }

        private void _guiaMatrix_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            bloquearCampos();
            actualizarTotales();
            _crearButton.Caption = "Programar";
        }

        private void actualizarTotales()
        {
            try
            {
                var totalcargado = 0.00;
                var cantidadbultos = 0.00;
                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(ColumnaSeleccionar).Cells.Item(i).Specific);
                    if (check.Checked)
                    {
                        var peso = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaPeso).Cells.Item(i).Specific).Value;
                        var cantidadBultos = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaCantBultos).Cells.Item(i).Specific).Value;

                        totalcargado += peso.ToDouble();
                        cantidadbultos += cantidadBultos.ToDouble();
                    }

                }

                _totalCargadoEditText.Value = totalcargado.ToString();
                _cantidadBultosEditText.Value = cantidadbultos.ToString();
                _diferenciaEditText.Value = (_cargaUtil.Value.ToDouble() - totalcargado).ToString();
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage("Error al actualizar totales : " + ex.Message);
            }

        }

        private void Form_DataAddAfter(ref BusinessObjectInfo pVal)
        {


        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;


        }

        private Button _cancelarButton;

        private void _cancelarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_cancelarButton.IsEnabled())
            {
                try
                {
                    if (_estadoComboBox.Selected.Value == "T")
                    {
                        for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                        {

                            //((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("N");
                            var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(ColumnaNumeroGuia).Cells.Item(i).Specific).Value;
                            var validacion = _liquidacionTarjetaDomain.ValidarSunat(numeracion);
                            if (validacion.Item1)
                            {
                                throw new Exception("No se puede cancelar si ya hay guías validadas por SUNAT");
                            }

                        }
                    }
                    ApplicationInterfaceHelper.ShowDialogMessageBox("¿Está seguro de cancelar la programación?",
                            () =>
                            {
                                UIAPIRawForm.Freeze(true);
                                _estadoComboBox.SelectByValue("C");

                                _liquidacionTarjetaDomain.ActualizarEstadoHojaGuia("C", _codigoHojaEditText.Value);
                                _enviarSunatButton.Item.Enabled = false;
                                _terminarProButton.Item.Enabled = false;
                                _programarButton.Item.Enabled = false;
                                _desprogramarButton.Item.Enabled = false;

                                UIAPIRawForm.Refresh();
                                //if (_crearButton.Caption != "OK")
                                //    _crearButton.Item.Click();

                                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizó la hoja de ruta de asignación");
                            },
                             null);



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

        }
    }
}
