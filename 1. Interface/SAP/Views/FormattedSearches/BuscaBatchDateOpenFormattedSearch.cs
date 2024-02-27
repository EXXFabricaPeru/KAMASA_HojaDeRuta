using SAPbouiCOM.Framework;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.FormattedSearches
{
    [Form(SAP_TYPE)]
    public class BuscaBatchDateOpenFormattedSearch : SystemFormBase
    {
        public const string SAP_TYPE = "2000671";
        private const int ITEM_NAME_COLUMN_INDEX = 2;


        private SAPbouiCOM.Button _applyButton;
        private SAPbouiCOM.Matrix _contentMatrix;

        //HINT: these is executed after form's load events
        public BuscaBatchDateOpenFormattedSearch()
        {
            Instance = this;
            SelectedItemName = string.Empty;
        }

        public static BuscaBatchDateOpenFormattedSearch Instance { get; private set; }

        public string SelectedItemName { get; private set; }

        public override void OnInitializeComponent()
        {
            _applyButton = GetItem("1").RetrieveSpecificButton();
            _applyButton.ClickBefore += applyButton_clickBefore;
            _applyButton.ClickAfter += applyButton_clickAfter;
            _contentMatrix = GetItem("4").RetrieveSpecificMatrix();
            _contentMatrix.DoubleClickBefore += contentMatrix_doubleClickBefore;
        }

        private void contentMatrix_doubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg eventArg, out bool eventHandler)
        {
            eventHandler = true;
            set_item_name_from_selected_row(eventArg.Row);
        }

        private void applyButton_clickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg eventArg, out bool eventHandler)
        {
            eventHandler = true;
            if (_contentMatrix.GetNextSelectedRow() != -1)
                return;

            eventHandler = false;
            ApplicationInterfaceHelper.ShowErrorStatusBarMessage(@"[validación] Se debe seleccionar un artículo a filtrar.");
        }

        private void applyButton_clickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg eventArg)
        {
            int rowIndex = _contentMatrix.GetNextSelectedRow();
            set_item_name_from_selected_row(rowIndex);
        }

        private void set_item_name_from_selected_row(int rowIndex)
        {
            var itemNameEdit = _contentMatrix
                .GetCellSpecific(ITEM_NAME_COLUMN_INDEX, rowIndex)
                .To<SAPbouiCOM.EditText>();

            SelectedItemName = itemNameEdit.Value;

            GenericHelper.ReleaseCOMObjects(itemNameEdit);
        }

        //HINT: these is executed after catch events
        public override void OnInitializeFormEvents()
        {
            CloseAfter += close_after;
        }

        private void close_after(SAPbouiCOM.SBOItemEventArg pVal)
        {
            Instance = null;
        }
    }
}