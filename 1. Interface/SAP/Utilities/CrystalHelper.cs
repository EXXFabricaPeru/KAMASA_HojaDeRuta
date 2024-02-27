using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class CrystalHelper
    {
        private static System.Data.DataTable CreateTableDev<T>()
        {
            Type entityType = typeof(T);
            System.Data.DataTable table = new System.Data.DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, System.Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }

        public static System.Data.DataTable ConvertToDev<T>(List<T> list)
        {
            System.Data.DataTable table = CreateTableDev<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch (Exception)
                    {

                    }

                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
