using B1SLayer;
using ClosedXML.Excel;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews
{
    [FormAttribute("UDO_FT_VS_LT_OPCG")]
    class PlantillaCarga : UDOFormBase
    {

        private const string SAP_COLUMN_UID = "C_0_1";
        private const string EXCEL_ADDRESS_COLUMN_UID = "C_0_2";
        private const string EXCEL_NAME_COLUMN_UID = "C_0_3";

        private SAPbouiCOM.DBDataSource _metadataDataSource;


        private const int NEST_NOTE_INDEX_LEFT = 5;
        private const int NOTE_RECTANGLE_WIDTH = 400;
        private const int NOTE_RECTANGLE_LEFT = 419;
        private const int RECTANGLE_HEIGHT = 35;
        private IFileDomain _fileDomain;
        public PlantillaCarga()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.FolderGeneral = ((SAPbouiCOM.Folder)(this.GetItem("Item_1").Specific));
            this.btnSeleccionar = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this.btnSeleccionar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSeleccionar_ClickBefore);
            this.btnSeleccionar.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnSeleccionar_ClickAfter);
            this.btnCargaArchivo = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.btnCargaArchivo.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCargaArchivo_ClickAfter);
            this.btnCargaArchivo.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCargaArchivo_ClickBefore);
            this._mainColumnComboBox = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_4").Specific));
            this._mainColumnComboBox.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this._mainColumnComboBox_ComboSelectAfter);
            this._archivoEditText = ((SAPbouiCOM.EditText)(this.GetItem("14_U_E").Specific));
            this._mainColumnTitleEdit = ((SAPbouiCOM.EditText)(this.GetItem("17_U_E").Specific));
            this._mappingMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("0_U_G").Specific));
            this._mappingMatrix.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this._mappingMatrix_ValidateBefore);
            this._mappingMatrix.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this._mappingMatrix_ComboSelectAfter);
            this._mappingMatrix.ComboSelectBefore += new SAPbouiCOM._IMatrixEvents_ComboSelectBeforeEventHandler(this._mappingMatrix_ComboSelectBefore);
            this._metadataMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("1_U_G").Specific));
            this.FolderMetadata = ((SAPbouiCOM.Folder)(this.GetItem("1_U_FD").Specific));
            this.FolderMapping = ((SAPbouiCOM.Folder)(this.GetItem("0_U_FD").Specific));
            this.Folder1 = ((SAPbouiCOM.Folder)(this.GetItem("Item_5").Specific));
            this._pasarelaPagoEditText = ((SAPbouiCOM.EditText)(this.GetItem("13_U_E").Specific));
            this.btnOk = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btnOk.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnOk_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.DataAddBefore += new DataAddBeforeHandler(this.Form_DataAddBefore);

        }

        private SAPbouiCOM.Folder FolderGeneral;

        private void OnCustomInitialize()
        {
            _fileDomain = FormHelper.GetDomain<FilesDomain>();
            SAPbouiCOM.DataSource dataSource = UIAPIRawForm.DataSources;
            SAPbouiCOM.DBDataSources dataSources = dataSource.DBDataSources;
            //_mappingDataSource = dataSources.Item("@VS_PD_FTP1");
            _metadataDataSource = dataSources.Item("@VS_LT_PCG2");
            GenericHelper.ReleaseCOMObjects(dataSource, dataSources);
            _mappingMatrix.AutoResizeColumns();
            Folder1.Hide();
            FolderMetadata.Hide();
            FolderMapping.SetWidth(150);
            loadSapFields(null);
            initialize_choose_from_list();

        }


        private SAPbouiCOM.Button btnSeleccionar;
        private SAPbouiCOM.Button btnCargaArchivo;
        private SAPbouiCOM.ComboBox _mainColumnComboBox;
        private EditText _archivoEditText;
        private EditText _mainColumnTitleEdit;
        private Matrix _mappingMatrix;

        private void initialize_choose_from_list()
        {
            SAPbouiCOM.ChooseFromList
                bancoEditTextChooseFromList,
                cuentaContableEditTextChooseFromList,
                pasarelaPagoEditTextChooseFromList,
                tiendaEditTextChooseFromList;

            make_choose_from_lists(
         out pasarelaPagoEditTextChooseFromList);

            append_choose_from_list_pasarela(_pasarelaPagoEditText, pasarelaPagoEditTextChooseFromList, pasarelaPagoEditText_ChooseFromListAfter,
               pasarelaPagoText_ChooseFromListBefore);

            //append_choose_from_list_tienda(_codigoTiendaEditText, tiendaEditTextChooseFromList, codigoTiendaEditText_ChooseFromListAfter,
            //   codigoTiendaText_ChooseFromListBefore);

            GenericHelper.ReleaseCOMObjects(pasarelaPagoEditTextChooseFromList);
        }
        private bool pasarelaPagoText_ChooseFromListBefore()
        {
            return true;
        }

        private void pasarelaPagoEditText_ChooseFromListAfter()
        {
            try
            {

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

        private void make_choose_from_lists(
    out ChooseFromList pasarelaPagoEditTextChooseFromList)
        {
            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = UIAPIRawForm.ChooseFromLists;
            pasarelaPagoEditTextChooseFromList = chooseFromListCollection.MakeChooseFromListIfNotExist(@"CFL_PP", OPAP.ID);

            GenericHelper.ReleaseCOMObjects(chooseFromListCollection);
        }
        private void btnSeleccionar_ClickAfter(object sboObject, SBOItemEventArg pVal)
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

        private void btnSeleccionar_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                BubbleEvent = false;
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(@"[Validación] Solo se puede cargar un archivo en modo 'Nuevo'.");
            }

        }

        //private async Task bpList(SLConnection login)
        //{

        //    //Pruebas


        //    //ServiceLayerHelper serviceLayerHelper = new ServiceLayerHelper();

        //    //var company = "B1H_AZALEIA_PRUEBAS";
        //    //var user = "DESARROLLO3";
        //    //var pass = "B1Admin$$";
        //    //var login = serviceLayerHelper.Login(company, user, pass);
        //    //bpList(login);

        //    var bpList = await login.Request("BusinessPartners")
        //       .Filter("startswith(CardCode, 'c')")
        //       .Select("CardCode, CardName")
        //       .OrderBy("CardName")
        //       .WithPageSize(50)
        //       .WithCaseInsensitive()
        //       .GetAsync<List<OCRD>>();
        //}
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
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void btnCargaArchivo_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            SAPbouiCOM.ValidValues comboBoxValidValues = _mainColumnComboBox.ValidValues;
            SAPbouiCOM.Columns columns = _mappingMatrix.Columns;
            SAPbouiCOM.Column column = columns.Item(EXCEL_ADDRESS_COLUMN_UID);
            SAPbouiCOM.ValidValues columnValidValues = column.ValidValues;
            try
            {

                var work = new XLWorkbook(_archivoEditText.Value);
                validate_and_process_workbook(work)
                    .ForEach(delegate (Tuple<string, string> item, int index, bool lastIteration)
                    {
                        columnValidValues.Add(item.Item1, item.Item2);
                        comboBoxValidValues.Add(item.Item1, item.Item2);

                        _metadataDataSource.SetValue(@"U_EXK_CLAR", index, item.Item1);
                        _metadataDataSource.SetValue(@"U_EXK_COLU", index, item.Item2);
                        if (!lastIteration)
                            _metadataDataSource.InsertRecord(index + 1);
                    }
                    );
                _metadataMatrix.LoadFromDataSourceEx();
                _mappingMatrix.AutoResizeColumns();

                ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("Terminó el proceso de carga de datos de plantilla cargados");
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private IEnumerable<Tuple<string, string>> validate_and_process_workbook(XLWorkbook workBook)
        {
            var list = new List<Tuple<string, string>>();
            if (workBook.Worksheets.Count != 1)
                throw new Exception(@"[Validación] El archivo excel solo puede tener una hoja de trabajo.");

            IXLWorksheet workSheet = workBook.Worksheets.First();

            //Create a new DataTable.
            var dataTable = new System.Data.DataTable();
            DataColumnCollection dataTableColumns = dataTable.Columns;

            //Loop through the Worksheet rows.
            var firstRow = true;
            foreach (IXLRow row in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (!firstRow)
                    continue;

                foreach (IXLCell cell in row.Cells())
                {
                    var columnName = cell.Value.ToString();
                    if (dataTableColumns.Contains(columnName))
                        throw new Exception($@"[Validación] El archivo excel tiene como duplicado el titulo '{columnName}'.");

                    dataTableColumns.Add(columnName);
                    Tuple<string, string> column = Tuple.Create(cell.Address.ToString(), columnName);
                    list.Add(column);
                }

                firstRow = false;
            }


            return list;
        }

        private void _mainColumnComboBox_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            SAPbouiCOM.ValidValue validValue = null;

            try
            {
                validValue = _mainColumnComboBox.Selected;
                _mainColumnTitleEdit.Value = validValue.Description;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(validValue);
            }

        }

        private Matrix _metadataMatrix;

        public Folder FolderMetadata;
        private Folder FolderMapping;

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            _mappingMatrix?.AutoResizeColumns();

        }

        private Folder Folder1;
        private EditText _pasarelaPagoEditText;
        private Button btnOk;

        private void _mappingMatrix_ComboSelectBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.ComboBox comboBox = null;
            SAPbouiCOM.ValidValues validValues = null;

            try
            {
                UIAPIRawForm.Freeze(true);
                if (pVal.ColUID == SAP_COLUMN_UID)
                {
                    loadSapFields(pVal);
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

        private void _mappingMatrix_ComboSelectAfter(object sboObject, SBOItemEventArg eventArg)
        {
            SAPbouiCOM.EditText editText = null;
            SAPbouiCOM.ComboBox comboBox = null;
            SAPbouiCOM.ValidValue validValue = null;
            UIAPIRawForm.Freeze(true);
            try
            {
                if (eventArg.ColUID == EXCEL_ADDRESS_COLUMN_UID)
                {
                    comboBox = _mappingMatrix.GetCellSpecific(EXCEL_ADDRESS_COLUMN_UID, eventArg.Row).To<SAPbouiCOM.ComboBox>();
                    validValue = comboBox.Selected;

                    editText = _mappingMatrix.GetCellSpecific(EXCEL_NAME_COLUMN_UID, eventArg.Row).To<SAPbouiCOM.EditText>();
                    editText.Value = validValue.Description;
                }
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                GenericHelper.ReleaseCOMObjects(editText, comboBox, validValue);
            }

        }

        private void _mappingMatrix_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.ComboBox comboBox = null;
            SAPbouiCOM.ValidValues validValues = null;
            try
            {
                UIAPIRawForm.Freeze(true);
                if (pVal.ColUID == SAP_COLUMN_UID)
                {
                    loadSapFields(pVal);
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


        private void loadSapFields(SBOItemEventArg pVal)
        {
            SAPbouiCOM.ComboBox comboBox = null;
            SAPbouiCOM.ValidValues validValues = null;
            int row = 0;
            if (pVal == null)
            {
                row = 1;
            }
            else
            {
                row = pVal.Row;
            }
            comboBox = _mappingMatrix.GetCellSpecific(SAP_COLUMN_UID, row).To<SAPbouiCOM.ComboBox>();
            validValues = comboBox.ValidValues;
            validValues.RemoveValidValues();
            validValues.Add(string.Empty, string.Empty);

            _fileDomain.RetrieveLinesLiquidationCardsFields()
                .ForEach(t => validValues.Add(t.Item1, t.Item2));
        }

        private void btnOk_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
         
        }

        private void Form_DataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            List<int> val = new List<int>();
            //List<string> val = new List<int>();

            try
            {

                for (int i = 1; i <= _mappingMatrix.RowCount; i++)
                {
                    var valorSAP = _mappingMatrix.GetCellSpecific("C_0_1", i).To<SAPbouiCOM.ComboBox>();

                    if ("U_VS_LT_NREF"== valorSAP.Selected.Value)
                    {
                        val.Add(1);
                    }

                    if ("U_VS_LT_FECO" == valorSAP.Selected.Value)
                    {
                        val.Add(2);
                    }

                    if ("U_VS_LT_NTAR" == valorSAP.Selected.Value)
                    {
                        val.Add(3);
                    }

                    if ("U_VS_LT_IMTA" == valorSAP.Selected.Value)
                    {
                        val.Add(4);
                    }

                    if ("U_VS_LT_COMI" == valorSAP.Selected.Value)
                    {
                        val.Add(5);
                    }

                    if ("U_VS_LT_NTPA" == valorSAP.Selected.Value)
                    {
                        val.Add(6);
                    }
                }

                if (val.Count < 6)
                {
                    BubbleEvent = false;
                    var message = "La plantilla debe tener por lo menos los siguientes campos" +
                        "U_VS_LT_NREF - Nro. Referencia de Cobro"+
                        "U_VS_LT_FECO - Fecha Cobro"+
                        "U_VS_LT_NTAR - Nro.  de Tarjeta"+
                        "U_VS_LT_IMTA - Importe Cobrado Tarjeta"+
                        "U_VS_LT_COMI - Comisión"+
                        "U_VS_LT_NTPA - Neto Parcial ";
                    ApplicationInterfaceHelper.ShowDialogMessageBox(message);
                }
                else
                {

                }

            }
            catch (Exception)
            {
                BubbleEvent = false;
            }
         

        }

        //    public static List<(string, string)> staticTupleList = new List<(string, string)>
        //{
        //    ("U_VS_LT_NREF", "Nro. Referencia de Cobro"),
        //    ("U_VS_LT_FECO", "Fecha Cobro"),
        //    ("U_VS_LT_NTAR", "Nro.  de Tarjeta"),
        //    ("U_VS_LT_IMTA", "Importe Cobrado Tarjeta"),
        //    ("U_VS_LT_COMI", "Comisión"),
        //    ("U_VS_LT_NTPA", "Neto Parcial")
        //    // Puedes agregar más elementos según sea necesario
        //};
    }
}
