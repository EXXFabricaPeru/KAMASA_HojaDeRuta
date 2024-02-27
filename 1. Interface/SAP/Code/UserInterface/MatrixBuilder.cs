using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface
{
    public class MatrixBuilder<T> where T : class
    {
        public struct ColumnSettings
        {
            public bool IsDefault { get; }
            public Color ForeColor { get; }
            public int FontStyle { get; }

            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public ColumnSettings(Color foreColor, params Utilities.FontStyle[] styles)
                : this(false)
            {
                ForeColor = foreColor;
                FontStyle = styles.Sum(t => t.ToInt32());
            }

            private ColumnSettings(bool @default)
            {
                ForeColor = default(Color);
                FontStyle = 0;
                IsDefault = @default;
            }

            public static ColumnSettings Default => new ColumnSettings(true);
        }

        public delegate void ChooseFromListBefore(object column, SBOItemEventArg eventArg, out bool handleEvent);

        private struct RelationValue
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public BoFormItemTypes ItemType { get; }
            public string PropertyName { get; }

            public RelationValue(BoFormItemTypes itemType, string propertyName)
            {
                ItemType = itemType;
                PropertyName = propertyName;
            }
        }

        private const string AUTOINCREMENT_COLUMN_ID = "autoincrement";

        private readonly Matrix _matrix;

        private DataTable _dataTable;

        private IDictionary<string, RelationValue> _relationColumns;

        private IEnumerable<T> _syncedData;

        public IEnumerable<T> SyncedData
        {
            get { return _syncedData ?? Enumerable.Empty<T>(); }
            private set { _syncedData = value; }
        }
        private IEnumerable<T> _filterData;

        public IEnumerable<T> FilterData
        {
            get { return _filterData ?? Enumerable.Empty<T>(); }
            private set { _filterData = value; }
        }
        public int FilterCount { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<T> ActualData
        {
            get
            {
                IEnumerable<PropertyInfo> itemProperties = typeof(T).GetProperties();
                IList<T> result = new List<T>();
                for (var i = 1; i <= _matrix.RowCount; i++)
                {
                    var item = Activator.CreateInstance<T>();
                    foreach (var valuePair in _relationColumns)
                    {
                        var property = itemProperties.SingleOrDefault(t => t.Name == valuePair.Value.PropertyName);
                        if (property == null)
                            continue;

                        var specificCell = _matrix.GetSpecificCell(valuePair.Key, i);
                        object value = null;
                        if (valuePair.Value.ItemType == BoFormItemTypes.it_EDIT)
                        {
                            string cellValue = specificCell.To<IEditText>().Value;
                            if (property.PropertyType == typeof(bool))
                                value = cellValue == "Si";
                            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            {
                                if (property.PropertyType == typeof(DateTime) && string.IsNullOrEmpty(cellValue))
                                    value = default(DateTime);
                                else if (!string.IsNullOrEmpty(cellValue))
                                    value = DateTime.ParseExact(cellValue, "yyyyMMdd", CultureInfo.InvariantCulture);
                            }
                            else
                                value = Convert.ChangeType(cellValue, property.PropertyType);
                        }
                        else if (valuePair.Value.ItemType == BoFormItemTypes.it_LINKED_BUTTON)
                        {
                            string cellValue = specificCell.To<IEditText>().Value;
                            Type innerType = Nullable.GetUnderlyingType(property.PropertyType);
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                value = Convert.ChangeType(cellValue, innerType ?? property.PropertyType);
                            }
                            else
                            {
                                if (innerType == null && property.PropertyType.IsValueType)
                                    value = Activator.CreateInstance(property.PropertyType);
                            }
                        }
                        else if (valuePair.Value.ItemType == BoFormItemTypes.it_CHECK_BOX)
                        {
                            value = specificCell.To<CheckBox>().Checked;
                        }
                        else if (valuePair.Value.ItemType == BoFormItemTypes.it_COMBO_BOX)
                        {
                            var cellValue = specificCell.To<ComboBox>().Value;
                            value = Convert.ChangeType(cellValue, property.PropertyType);
                        }

                        property.SetValue(item, value);
                    }

                    result.Add(item);
                }

                return result;
            }
        }

        public MatrixBuilder(Matrix matrix, DataTable dataTable)
        {
            _matrix = matrix;
            _dataTable = dataTable;
            _relationColumns = new Dictionary<string, RelationValue>();
        }

        public MatrixBuilder<T> ApplySingleRowMode()
        {
            _matrix.SelectionMode = BoMatrixSelect.ms_Single;
            return this;
        }

        public MatrixBuilder<T> ApplyNoSelectionMode()
        {
            _matrix.SelectionMode = BoMatrixSelect.ms_None;
            return this;
        }

        public MatrixBuilder<T> Clear()
        {
            _matrix.Clear();
            _dataTable.Clear();
            _relationColumns = new Dictionary<string, RelationValue>();

            Columns matrixColumns = _matrix.Columns;
            while (matrixColumns.Count > 0)
                matrixColumns.Remove(0);

            return this;
        }

        public MatrixBuilder<T> AutoResizeColumns()
        {
            if (_matrix.Columns.Count > 0)
                _matrix.AutoResizeColumns();
            return this;
        }

        public MatrixBuilder<T> AddAutoincrementColumn()
        {
            _dataTable.Columns.Add(AUTOINCREMENT_COLUMN_ID, BoFieldsType.ft_Integer);
            Column column = _matrix.MakeDefaultColumnNonEditable("#", BoFormItemTypes.it_EDIT)
                .BindDataTable(_dataTable.UniqueID, AUTOINCREMENT_COLUMN_ID);

            _relationColumns.Add(column.UniqueID, new RelationValue(BoFormItemTypes.it_EDIT, AUTOINCREMENT_COLUMN_ID));
            return this;
        }

        public MatrixBuilder<T> AddEditableColumn<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);

            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddEditableColumnWithEvent<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            _IColumnEvents_KeyDownAfterEventHandler eventHandler)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);

            column.KeyDownAfter += new _IColumnEvents_KeyDownAfterEventHandler(eventHandler);
            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddEditableColumnFromList<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            ChooseFromList chooseFromList, string sapColumnName, Action<PropertyInfo, ChooseFromList> mappingAction = null,
            ChooseFromListBefore beforeAction = null, Action<object, SBOItemEventArg> afterAction = null)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);

            column.ChooseFromListUID = chooseFromList.UniqueID;
            column.ChooseFromListAlias = sapColumnName;

            if (beforeAction != null)
                column.ChooseFromListBefore += new _IColumnEvents_ChooseFromListBeforeEventHandler(beforeAction);

            if (afterAction != null)
                column.ChooseFromListAfter += new _IColumnEvents_ChooseFromListAfterEventHandler(afterAction);

            mappingAction?.Invoke(property, chooseFromList);

            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddEditableEditTextColumn<TK>(string description, Expression<Func<T, TK>> mapping)
        {
            return AddEditableColumn(description, BoFormItemTypes.it_EDIT, mapping);
        }

        public MatrixBuilder<T> AddNonEditableEditTextColumn<TK>(string description, Expression<Func<T, TK>> mapping)
        {
            return AddNonEditableColumn(description, BoFormItemTypes.it_EDIT, mapping);
        }
        public MatrixBuilder<T> AddNonEditableEditTextColumnBlack<TK>(string description, Expression<Func<T, TK>> mapping)
        {
            return AddNonEditableColumn(description, BoFormItemTypes.it_EDIT, mapping);
        }

        public MatrixBuilder<T> AddNonEditableEditTextExtendColumn<TK>(string description, Expression<Func<T, TK>> mapping)
        {
            return AddNonEditableColumn(description, BoFormItemTypes.it_EXTEDIT, mapping);
        }

        public MatrixBuilder<T> AddNonEditableEditTextColumn<TK>(string description, Expression<Func<T, TK>> mapping, ColumnSettings columnSettings)
        {
            return AddNonEditableColumn(description, BoFormItemTypes.it_EDIT, mapping, columnSettings);
        }

        public MatrixBuilder<T> AddInvisibleColumn<TK>(Expression<Func<T, TK>> mapping)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable("invi", BoFormItemTypes.it_EDIT);
            column.Visible = false;
            column.BindDataTable(_dataTable.UniqueID, property.Name);

            _relationColumns.Add(column.UniqueID, new RelationValue(BoFormItemTypes.it_EDIT, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableColumn<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);
            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }
        public MatrixBuilder<T> AddNonEditableColumnBlack<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);
            
            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableColumn<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            ColumnSettings columnSettings)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name);

            column.ForeColor = columnSettings.ForeColor.R | (columnSettings.ForeColor.G << 8) | (columnSettings.ForeColor.B << 16);
            column.TextStyle = columnSettings.FontStyle;


            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddEditableLinkedColumn<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            int sapObjectType)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(sapObjectType.ToString());

            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableLinkedColumn<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            int sapObjectType)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(sapObjectType.ToString());

            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableLinkedColumn<TK>(string description, Expression<Func<T, TK>> mapping, int sapObjectType)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, BoFormItemTypes.it_LINKED_BUTTON)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(sapObjectType.ToString());

            _relationColumns.Add(column.UniqueID, new RelationValue(BoFormItemTypes.it_LINKED_BUTTON, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableLinkedColumn<TK>(string description, Expression<Func<T, TK>> mapping, string sapObjectType)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, BoFormItemTypes.it_LINKED_BUTTON)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(sapObjectType);

            _relationColumns.Add(column.UniqueID, new RelationValue(BoFormItemTypes.it_LINKED_BUTTON, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableLinkedColumnUDO<TK>(string description, BoFormItemTypes type, Expression<Func<T, TK>> mapping,
            string udoName)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, type)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(udoName);

            _relationColumns.Add(column.UniqueID, new RelationValue(type, property.Name));

            return this;
        }

        public MatrixBuilder<T> AddNonEditableLinkedColumnUWithEventLink<TK>(string description, Expression<Func<T, TK>> mapping,
            _IColumnEvents_LinkPressedBeforeEventHandler eventHandler, int sapObjectType)
        {
            PropertyInfo property = get_property_name_for_expression(mapping);

            _dataTable.Columns.Add(property.Name, get_field_type(property));

            Column column = _matrix.MakeDefaultColumnNonEditable(description, BoFormItemTypes.it_LINKED_BUTTON)
                .BindDataTable(_dataTable.UniqueID, property.Name)
                .ReferenceObjectType(sapObjectType.ToString());

            column.LinkPressedBefore += new _IColumnEvents_LinkPressedBeforeEventHandler(eventHandler);
            _relationColumns.Add(column.UniqueID, new RelationValue(BoFormItemTypes.it_LINKED_BUTTON, property.Name));

            return this;
        }
        
        private PropertyInfo get_property_name_for_expression<TK>(Expression<Func<T, TK>> mapping)
        {
            var memberExpression = mapping.Body as MemberExpression;
            var propertyInfo = memberExpression?.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new Exception(ErrorMessages.BadMatchMatrixBuilder);
            return propertyInfo;
        }

        private BoFieldsType get_field_type(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(bool))
                return BoFieldsType.ft_AlphaNumeric;

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                return BoFieldsType.ft_Integer;

            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                return BoFieldsType.ft_Date;

            if (property.PropertyType == typeof(decimal))
                return BoFieldsType.ft_Price;

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(float))
                return BoFieldsType.ft_Float;

            throw new Exception(string.Format(ErrorMessages.NoImplemented, property.PropertyType.Name));
        }

        private object get_value(T item, PropertyInfo property)
        {
            object value = property.GetValue(item);
            if (value == null && (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) ||
                                  property.PropertyType == typeof(DateTime?)))
                return string.Empty;

            if (value is decimal || value is float)
                return value.ToDouble();

            var booleanValue = value as bool?;
            if (booleanValue != null)
                return booleanValue.Value ? "Si" : "No";

            return value;
        }

        private object get_values(T item, PropertyInfo property)
        {
            object value = property.GetValue(item);
            if (value == null && (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) ||
                                  property.PropertyType == typeof(DateTime?)))
                return string.Empty;

            if (value is decimal || value is float)
                return value.ToDouble();

            var booleanValue = value as bool?;
            if (booleanValue != null)
                return booleanValue.Value ? "Si" : "No";

            return value;
        }

        public void SetValidValuesToComboBox(object columnId, IEnumerable<SelectItem> validValues)
        {
            var column = _matrix.Columns.Item(columnId);
            column.DisplayDesc = true;
            var columnValidValues = column.ValidValues;
            validValues.ForEach(item => columnValidValues.Add(item.Code, item.Description));
        }

        public void SyncData(IEnumerable<T> data)
        {
            T[] actualData = data as T[] ?? data.ToArray();
            SyncedData = actualData;
            TotalCount = actualData.Length;
            populate_matrix(actualData);
        }

        public delegate bool RemoveDataComparer(T sourceItem, T removeItem);

        /// <summary>
        /// Remove all items in specific matrix.
        /// </summary>
        public void ClearData()
        {
            _dataTable.Rows.Clear();
            _matrix.Clear();
        }

        /// <summary>
        /// Remove items in specific matrix (if found this).
        /// </summary>
        /// <param name="data">Items to remove</param>
        /// <param name="comparer">Works only non editable column values</param>
        public void RemoveData(IEnumerable<T> data, RemoveDataComparer comparer)
        {
            IList<T> result = new List<T>();
            foreach (var item in SyncedData)
            {
                if (data.Any(t => comparer(item, t)))
                    continue;
                result.Add(item);
            }

            SyncData(result);
        }

        public void CustomFilter(Func<T, bool> filterExpression)
        {
            T[] filteredData = SyncedData.Where(filterExpression).ToArray();
            FilterCount = TotalCount - filteredData.Length;
            FilterData = filteredData;
            populate_matrix(filteredData);
        }

        private void populate_matrix(T[] data)
        {
            _dataTable.Rows.Clear();
            _matrix.Clear();
            for (var index = 0; index < data.Length; index++)
            {
                T item = data[index];
                _dataTable.Rows.Add();
                foreach (KeyValuePair<string, RelationValue> valuePair in _relationColumns)
                {
                    RelationValue relationValue = valuePair.Value;
                    object value;
                    if (relationValue.PropertyName == AUTOINCREMENT_COLUMN_ID)
                        value = index + 1;
                    else
                    {
                        PropertyInfo property = item.GetProperty(relationValue.PropertyName);
                        value = get_value(item, property);
                        if (relationValue.ItemType == BoFormItemTypes.it_CHECK_BOX)
                            value = ReferenceEquals(value, "Si") ? "Y" : "N";
                        else if (relationValue.ItemType == BoFormItemTypes.it_LINKED_BUTTON && value == null)
                            continue;
                    }

                    _dataTable.SetValue(relationValue.PropertyName, index, value);
                }
            }

            _matrix.LoadFromDataSourceEx();
            _matrix.AutoResizeColumns();
        }

        /// <summary>
        /// Set background color to specific row
        /// </summary>
        /// <param name="color">16-bit based color</param>
        /// <param name="index">specific row 0-index based</param>
        public void SetRowBackgroundColor(Color color, int index)
        {
            _matrix.CommonSetting.SetRowBackColor(index + 1, color.R | (color.G << 8) | (color.B << 16));
        }

        public void SetRowFontStyle(BoFontStyle style, int index)
        {
            _matrix.CommonSetting.SetRowFontStyle(index + 1, style);
        }

        public void SetRowEditable(bool val, int index)
        {
            _matrix.CommonSetting.SetRowEditable(index + 1, val);
        }

        public void SetRowFontColor(Color color, int index)
        {
            _matrix.CommonSetting.SetRowFontColor(index + 1, color.R | (color.G << 8) | (color.B << 16));

        }
        /// <summary>
        /// Set background color to specific column
        /// </summary>
        /// <param name="color">16-bit based color</param>
        /// <param name="index">specific column 0-index based</param>
        public void SetColumnBackgroundColor(Color color, int index)
        {
            for (var i = 1; i <= _matrix.RowCount; i++)
                _matrix.CommonSetting.SetCellBackColor(i, index, color.R | (color.G << 8) | (color.B << 16));
        }

        /// <summary>
        /// Select a specific row
        /// </summary>
        /// <param name="index">specific row 0-index based</param>
        public void SelectOnlyRow(int index)
        {
            _matrix.SelectRow(index + 1, true, false);
        }

        public T RetrieveRecordByMatrixRowNumber(int rowNumber)
        {
            return _syncedData.ElementAt(rowNumber - 1).DeepClone();
        }

        public void UnSelectRows()
        {
            for (int i = 1; i <= _matrix.RowCount; i++)
            {
                _matrix.SelectRow(i, false, false);
            }
        }

        public void RemoveSelectRow()
        {
            int index = _matrix.GetSelectedRow() - 1;
            IEnumerable<T> removedRecords = ActualData
                .Select((t, i) => index == i ? null : t)
                .Where(t => t != null).ToList();
            SyncData(removedRecords);
        }
    }
}