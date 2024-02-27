using SAPbouiCOM;
using System.Collections.Generic;
using System.Linq;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface
{
    public class ComboBoxBuilder
    {
        private readonly DataTable _dataTable;
        private readonly ComboBox _comboBox;

        public IEnumerable<SelectItem> ActualData { get; private set; }

        public struct SelectItem
        {
            public string Code { get; }
            public string Description { get; }

            public SelectItem(string code, string description)
            {
                Code = code;
                Description = description;
            }
        }

        public static ComboBoxBuilder.SelectItem MakeItem(string code, string description)
        {
            return new ComboBoxBuilder.SelectItem(code, description);
        }

        public static ComboBoxBuilder.SelectItem MakeItem(int code, string description)
        {
            return new ComboBoxBuilder.SelectItem(code.ToString(), description);
        }

        public static ComboBoxBuilder Reference(ComboBox comboBox, DataTable dataTable)
        {
            return new ComboBoxBuilder(comboBox, dataTable);
        }

        public ComboBoxBuilder(ComboBox comboBox, DataTable dataTable)
        {
            _comboBox = comboBox;
            _dataTable = dataTable;
            initialize();
        }

        private void initialize()
        {
            _dataTable.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric);
            _dataTable.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric);
            _comboBox.DataBind.Bind(_dataTable.UniqueID, "Code");
        }

        public ComboBoxBuilder DisplayDescription()
        {
            _comboBox.Item.DisplayDesc = true;
            return this;
        }

        public ComboBoxBuilder Disable()
        {
            _comboBox.Item.Enabled = false;
            return this;
        }

        public void SyncData(IEnumerable<SelectItem> data)
        {
            _dataTable.Rows.Clear();
            _comboBox.RemoveValidValues();
            SelectItem[] actualData = data as SelectItem[] ?? data.ToArray();
            ActualData = actualData;
            for (var i = 0; i < actualData.Length; i++)
            {
                SelectItem item = actualData[i];
                _dataTable.Rows.Add();                
                _dataTable.SetValue("Code", i, item.Code);
                _dataTable.SetValue("Name", i, item.Description);
                _comboBox.ValidValues.Add(item.Code, item.Description);
            }
        }
    }
}
