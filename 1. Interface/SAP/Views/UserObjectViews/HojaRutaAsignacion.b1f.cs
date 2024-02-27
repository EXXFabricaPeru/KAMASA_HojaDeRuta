using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
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
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_4").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this._filtrarButton = ((SAPbouiCOM.Button)(this.GetItem("Item_6").Specific));
            this._filtrarButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this._filtrarButton_ClickBefore);
            this._filtrarButton.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this._filtrarButton_ClickAfter);
            this._guiaMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
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
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);

        }

        private SAPbouiCOM.Button _desprogramarButton;

        private void OnCustomInitialize()
        {
            initialize_choose_from_list();
            _guiaMatrix.AutoResizeColumns();
            _infrastructureDomain = FormHelper.GetDomain<InfrastructureDomain>();
            _liquidacionTarjetaDomain = FormHelper.GetDomain<LiquidacionTarjetasDomain>();
            _settingsDomain = FormHelper.GetDomain<SettingsDomain>();
            if (UIAPIRawForm.Mode == BoFormMode.fm_ADD_MODE)
            {
                _estadoComboBox.SelectByValue("O");
                _desdeEditText.Value = DateTime.Now.ToString("yyyyMMdd");
                _hastaEditText.Value = DateTime.Now.ToString("yyyyMMdd");
                _codigoHojaEditText.Value = _liquidacionTarjetaDomain.RetrieveCodigoGenerado();
                //_crearButton.Caption = "Programar";
                _enviarSunatButton.Item.Enabled = false;
            }
            _enviarSunatButton.Item.Enabled = false;
            //_crearButton.Caption = "Programar";
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
                    foreach (var item in hojadeRuta.Item2.DetalleZonas)
                    {
                        _zonaDespachoEditText.Value = item.ZonaDespacho + "-";
                    }

                    _placaEditText.Value = hojadeRuta.Item2.Placa;
                    _inicioTrasladoRutaEditText.Value = hojadeRuta.Item2.InicioTraslado.ToString("yyyyMMdd");
                    _finTrasladoRutaEditText.Value = hojadeRuta.Item2.FinTraslado.ToString("yyyyMMdd");

                }
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
        private SAPbouiCOM.CheckBox CheckBox0;
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

                _guiaMatrix.Clear();
                var combo = _programadosComboBox.Selected != null ? _programadosComboBox.Selected.Value : "";
                var listaGuias = _liquidacionTarjetaDomain.RetrieveGuiasHoja(_desdeEditText.Value, _hastaEditText.Value, combo, "Zona");
                //var listaGuias = _liquidacionTarjetaDomain.RetrieveGuiasHoja("", "", "", "Zona");

                int cont = 1;
                if (listaGuias.Item1)
                {
                    var totalcargado = 0.00;
                    var cantidadbultos = 0;
                    foreach (var item in listaGuias.Item2)
                    {
                        _guiaMatrix.AddRow();
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(2).Cells.Item(cont).Specific).Value = item.NumberAtCard;
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(3).Cells.Item(cont).Specific).Value = item.Peso;
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(4).Cells.Item(cont).Specific).Value = item.CantidadBultos.ToString();
                        ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(cont).Specific).SelectByValue(item.Programado);
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(6).Cells.Item(cont).Specific).Value = "Lima Centro";
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(7).Cells.Item(cont).Specific).Value = "Dirección prueba";
                        ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(8).Cells.Item(cont).Specific).Value = "Lima - Lima - Lima";
                        cont++;

                        totalcargado += item.Peso.ToDouble();
                        cantidadbultos += item.CantidadBultos;
                    }
                    _totalCargadoEditText.Value = totalcargado.ToString();
                    _cantidadBultosEditText.Value = cantidadbultos.ToString();


                }
            }
            catch (Exception ex)
            {

                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
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


        }

        private void _crearButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {


        }

        private Button _programarButton;

        private void _programarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(1).Cells.Item(i).Specific);
                    if (check.Checked)
                    {
                        ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("Y");
                        var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(2).Cells.Item(i).Specific).Value;
                        _liquidacionTarjetaDomain.ActualizarProgramado(numeracion, "Y");
                    }

                }
                if (_crearButton.Caption != "OK")
                    _crearButton.Item.Click();
            }
            catch (Exception)
            {
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizaron las guías de remisión");
            }


        }

        private void _desprogramarButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(1).Cells.Item(i).Specific);
                    if (check.Checked)
                    {
                        ((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("N");
                        var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(2).Cells.Item(i).Specific).Value;
                        _liquidacionTarjetaDomain.ActualizarProgramado(numeracion, "N");
                    }

                }
                if (_crearButton.Caption != "OK")
                    _crearButton.Item.Click();
            }
            catch (Exception)
            {
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizaron las guías de remisión");
            }
        }

        private SAPbouiCOM.DBDataSource _headerDataSource;
        //TERMINAR PROGRAMACION
        private void _terminarProButton_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            try
            {
                //SAPbouiCOM.DataSource dataSource = UIAPIRawForm.DataSources;
                //SAPbouiCOM.DBDataSources dbDataSources = dataSource.DBDataSources;
                //_headerDataSource = dbDataSources.Item(@"@EX_HR_OHGR");
                //GenericHelper.ReleaseCOMObjects(dbDataSources, dataSource);

                //_estadoComboBox.Select("T", BoSearchKey.psk_ByValue);
                //_headerDataSource.SetValue("U_EXK_EST", 0, "T");
                //_headerDataSource.upda
                _estadoComboBox.SelectByValue("T");

                _liquidacionTarjetaDomain.ActualizarEstadoHojaGuia("T", _codigoHojaEditText.Value);
                _enviarSunatButton.Item.Enabled = true;
                _terminarProButton.Item.Enabled = false;
                _programarButton.Item.Enabled = false;
                _desprogramarButton.Item.Enabled = false;

                if (_crearButton.Caption != "OK")
                    _crearButton.Item.Click();
            }
            catch (Exception)
            {

            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizó la hoja de ruta de asignación");
            }

        }

        private Button Button6;

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
                UIAPIRawForm.Freeze(true);
                for (int i = 1; i <= _guiaMatrix.RowCount; i++)
                {
                    var check = ((SAPbouiCOM.CheckBox)_guiaMatrix.Columns.Item(1).Cells.Item(i).Specific);

                    //((SAPbouiCOM.ComboBox)_guiaMatrix.Columns.Item(5).Cells.Item(i).Specific).SelectByValue("N");
                    var numeracion = ((SAPbouiCOM.EditText)_guiaMatrix.Columns.Item(2).Cells.Item(i).Specific).Value;
                    _liquidacionTarjetaDomain.ActualizarEnvioSunat(numeracion);


                }

            }
            catch (Exception)
            {


            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Se actualizó el estado de SUNAT de las guías de remisión");
            }

        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            _guiaMatrix.AutoResizeColumns();
            if (_estadoComboBox.Selected.Value == "T")
            {
                _enviarSunatButton.Item.Enabled = true;
                _terminarProButton.Item.Enabled = false;
                _programarButton.Item.Enabled = false;
                _desprogramarButton.Item.Enabled = false;

            }
            _filtrarButton.Item.Enabled = false;
        }
    }
}
