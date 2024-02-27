using System;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class DataTablesHelper
    {
        public static T TryGetSingleValue<T>(this SAPbouiCOM.DataTable dataTable, string columnAlias)
        {
            try
            {
                object value = dataTable.GetValue(columnAlias, 0);
                return (T) Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static SAPbouiCOM.DataTable SafetySetValue(this SAPbouiCOM.DataTable dataTable, string columnAlias, int recordIndex, string value)
        {
            try
            {
                dataTable.SetValue(columnAlias, recordIndex, value ?? string.Empty);
                return dataTable;
            }
            catch (Exception)
            {
                throw new Exception($"No se puede registrar el '{value}'. Columna: '{columnAlias}' Fila:'{recordIndex + 1}'");
            }
        }

        public static SAPbouiCOM.DataTable SafetySetValue(this SAPbouiCOM.DataTable dataTable, string columnAlias, int recordIndex, int value)
        {
            try
            {
                dataTable.SetValue(columnAlias, recordIndex, value);
                return dataTable;
            }
            catch (Exception)
            {
                throw new Exception($"No se puede registrar el '{value}'. Columna: '{columnAlias}' Fila:'{recordIndex + 1}'");
            }
        }

        public static SAPbouiCOM.DataTable SafetySetValue(this SAPbouiCOM.DataTable dataTable, string columnAlias, int recordIndex, decimal value)
        {
            try
            {
                dataTable.SetValue(columnAlias, recordIndex, value.ToDouble());
                return dataTable;
            }
            catch (Exception)
            {
                throw new Exception($"No se puede registrar el '{value}'. Columna: '{columnAlias}' Fila:'{recordIndex + 1}'");
            }
        }
    }
}