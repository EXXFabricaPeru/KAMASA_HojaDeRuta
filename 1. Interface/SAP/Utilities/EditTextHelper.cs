using System;
using System.Drawing;
using System.Globalization;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class EditTextHelper
    {
        private const string SAP_DATE_STRING_FORMAT = "yyyyMMdd";

        public static EditText BindUserDataSource(this EditText editText, string userDataSourceId)
        {
            DataBind dataBind = editText.DataBind;
            dataBind.SetBound(true, string.Empty, userDataSourceId);
            GenericHelper.ReleaseCOMObjects(dataBind);
            return editText;
        }

        public static SAPbouiCOM.EditText BindUserDataSource(this SAPbouiCOM.EditText editText, SAPbouiCOM.UserDataSource userDataSource)
        {
            return BindUserDataSource(editText, userDataSource.UID);
        }

        public static EditText BindChooseFromList(this EditText editText, string chooseFromListId, string columnAlias)
        {
            editText.ChooseFromListUID = chooseFromListId;
            editText.ChooseFromListAlias = columnAlias;
            return editText;
        }

        public static EditText SetBackGroundColor(this EditText editText, Color color)
        {
            editText.BackColor = color.R | (color.G << 8) | (color.B << 16);
            return editText;
        }

        public static EditText SetDateTimeValue(this EditText editText, DateTime dateTime)
        {
            editText.Value = dateTime.ToString(SAP_DATE_STRING_FORMAT);
            return editText;
        }

        public static EditText ClearValue(this EditText editText)
        {
            editText.Value = null;
            return editText;
        }

        public static EditText SetValue(this EditText editText, string value)
        {
            editText.Value = value;
            return editText;
        }

        public static EditText SetEnableStatus(this EditText editText, bool status)
        {
            if (status == false && editText.IsActive())
                editText.Active = false;

            Item item = editText.Item;
            item.Enabled = status;

            GenericHelper.ReleaseCOMObjects(item);
            return editText;
        }
        
        public static EditText SetDecimalValue(this EditText editText, decimal value)
        {
            editText.Value = value.ToString("0.00");
            return editText;
        }

        public static EditText Enable(this EditText editText)
        {
            return SetEnableStatus(editText, true);
        }

        public static EditText Select(this EditText editText)
        {
            editText.Active = true;
            return editText;
        }

        public static EditText Disable(this EditText editText)
        {
            return SetEnableStatus(editText, false);
        }

        public static EditText Hide(this EditText editText)
        {
            if (editText.IsActive()) editText.Active = false;
            Item item = editText.Item;
            item.Visible = false;
            GenericHelper.ReleaseCOMObjects(item);
            return editText;
        }

        public static bool IsEnable(this EditText editText)
        {
            Item item = editText.Item;
            bool status = item.Enabled;
            GenericHelper.ReleaseCOMObjects(item);
            return status;
        }

        public static bool IsActive(this EditText editText)
        {
            try
            {
                return editText.Active;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime GetDateTimeValue(this EditText editText)
        {
            var value = editText.Value;
            return DateTime.ParseExact(value, SAP_DATE_STRING_FORMAT, CultureInfo.InvariantCulture);
        }

        public static int GetInt32Value(this EditText editText)
        {
            string value = editText.Value;
            if (string.IsNullOrEmpty(value))
                return default(int);
            return value.ToInt32();
        }


        public static int? TryGetInt32Value(this EditText editText)
        {
            string value = editText.Value;
            if (string.IsNullOrEmpty(value))
                return null;
            return value.ToInt32();
        }

        public static decimal? TryGetDecimalValue(this EditText editText)
        {
            string value = editText.Value;
            if (string.IsNullOrEmpty(value))
                return default(decimal);
            return value.ToDecimal();
        }

        public static decimal GetDecimalValue(this EditText editText)
        {
            return editText.Value.ToFloat().ToDecimal();
        }

        public static TimeSpan GetTimeSpan(this EditText editText)
        {
            TimeSpan result;
            var value = editText.Value;
            if (is_strict_time(value, out result) || is_integer_time(value, out result))
                return result;

            return TimeSpan.Zero;
        }

        private static bool is_strict_time(string value, out TimeSpan time)
        {
            return TimeSpan.TryParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture, out time);
        }

        private static bool is_integer_time(string value, out TimeSpan time)
        {
            int integer;
            if (!int.TryParse(value, out integer))
            {
                time = new TimeSpan(0, 0, 0);
                return false;
            }

            var minutes = integer % 100;
            if (minutes > 59)
            {
                time = new TimeSpan(0, 0, 0);
                return false;
            }

            var hours = integer / 100;
            if (hours > 23)
            {
                time = new TimeSpan(0, 0, 0);
                return false;
            }

            time = new TimeSpan(hours, minutes, 0);
            return true;
        }
    }
}