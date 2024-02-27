using System;
using System.Drawing;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class MatrixHelper
    {
        /// <summary>
        /// Get specific casted cell from SAP Matrix
        /// </summary>
        /// <typeparam name="T">SAPbouiCOM type</typeparam>
        /// <param name="matrix"></param>
        /// <param name="columnIndex">id column or 0-based index</param>
        /// <param name="rowIndex">1-based index</param>
        /// <returns></returns>
        public static T GetSpecificCell<T>(this Matrix matrix, object columnIndex, int rowIndex)
        {
            Columns columns = null;
            Column column = null;
            Cells cells = null;
            Cell cell = null;
            try
            {

                columns = matrix.Columns;
                column = columns.Item(columnIndex);
                cells = column.Cells;
                cell = cells.Item(rowIndex);
                return cell.Specific.To<T>();
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(columns, column, cells, cell);
            }
        }

        /// <summary>
        /// Get specific casted cell from SAP Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="columnIndex">id column or 0-based index</param>
        /// <param name="rowIndex">1-based index</param>
        /// <returns></returns>
        public static object GetSpecificCell(this Matrix matrix, object columnIndex, int rowIndex)
        {
            return matrix.Columns.Item(columnIndex).Cells.Item(rowIndex).Specific;
        }

        /// <summary>
        /// Make default column with auto incremental identifier
        /// </summary>
        /// <returns>Column created</returns>
        public static Column MakeDefaultColumn(this Matrix matrix, string description, BoFormItemTypes type, bool editable)
        {
            var columnId = $"col{matrix.Columns.Count}";
            Column matrixColumn = matrix.Columns.Add(columnId, type);
            matrixColumn.Description = description;
            matrixColumn.TitleObject.Caption = description;
            matrixColumn.Editable = editable;
            matrixColumn.DisplayDesc = false;
            matrixColumn.TitleObject.Sortable = true;
            return matrixColumn;
        }

        /// <summary>
        /// Make default column with auto incremental identifier
        /// <returns>Column created</returns>
        /// </summary>
        public static Column MakeDefaultColumnEditable(this Matrix matrix, string description, BoFormItemTypes type)
            => MakeDefaultColumn(matrix, description, type, true);

        /// <summary>
        /// Make default column with auto incremental identifier
        /// <returns>Column created</returns>
        /// </summary>
        public static Column MakeDefaultColumnNonEditable(this Matrix matrix, string description, BoFormItemTypes type) 
            => MakeDefaultColumn(matrix,  description,  type, false);

        /// <summary>
        /// Bind column with a specific data source
        /// <returns>Column binded</returns>
        /// </summary>
        public static Column BindDataSource(this Column column, string sourceName, string columnName)
        {
            column.DataBind.SetBound(true, sourceName, columnName);
            return column;
        }

        /// <summary>
        /// Bind column with a specific data table
        /// <returns>Column binded</returns>
        /// </summary>
        public static Column BindDataTable(this Column column, string sourceName, string columnName)
        {
            column.DataBind.Bind(sourceName, columnName);
            return column;
        }

        public static Column ReferenceObjectType(this Column column, string sapObjectType)
        {
            column.ExtendedObject.To<LinkedButton>().LinkedObjectType = sapObjectType;
            return column;
        }

        /// <summary>
        /// Retrieve row index
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>1-based index</returns>
        public static int GetSelectedRow(this Matrix matrix)
        {
            var i = 0;
            do i++;
            while (!matrix.IsRowSelected(i) && matrix.RowCount > i);
            return i;
        }

        public static string GetItemId(this Matrix matrix)
        {
            Item item = matrix.Item;
            string matrixId = item.UniqueID;
            GenericHelper.ReleaseCOMObjects(item);
            return matrixId;
        }

        public static void SetRowBackGroundColor(this CommonSetting commonSetting, int rowNumber, Color color)
        {
            commonSetting.SetRowBackColor(rowNumber, color.R | (color.G << 8) | (color.B << 16));
        }

        public static void SetCellBackGroundColor(this CommonSetting commonSetting, int rowNumber, int columnNumber, Color color)
        {
            commonSetting.SetCellBackColor(rowNumber, columnNumber, color.R | (color.G << 8) | (color.B << 16));
        }
        
    }
}