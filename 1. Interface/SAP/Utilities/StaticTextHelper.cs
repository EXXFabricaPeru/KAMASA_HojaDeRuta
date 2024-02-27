using System.Drawing;
using System.Linq;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class StaticTextHelper
    {
        public static StaticText SetForeColor(this StaticText staticText, Color color)
        {
            staticText.Item.ForeColor = color.R | (color.G << 8) | (color.B << 16);
            return staticText;
        }

        public static StaticText SetLeft(this StaticText staticText, int width)
        {
            SAPbouiCOM.Item item = null;
            try
            {
                item = staticText.Item;
                item.Left = width;
                return staticText;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(item);
            }
        }

        /// <summary>
        /// Bind picture box with a specific data source
        /// <returns>PictureBox binded</returns>
        /// </summary>
        public static PictureBox BindDataSource(this PictureBox pictureBox, string sourceName)
        {
            pictureBox.DataBind.SetBound(true, string.Empty, sourceName);
            return pictureBox;
        }

        public static PictureBox SetImagePath(this PictureBox pictureBox, string path)
        {
            pictureBox.Picture = path;
            return pictureBox;
        }

        /// <summary>
        /// Apply style into font
        /// </summary>
        /// <param name="staticText"></param>
        /// <param name="styles">Don't repeat style.</param>
        /// <returns></returns>
        public static StaticText ApplyFontStyle(this StaticText staticText, params FontStyle[] styles)
        {
            var fontStyle = styles.Sum(t => t.ToInt32());
            staticText.Item.TextStyle = fontStyle;
            return staticText;
        }

        
    }

    public enum FontStyle
    {
        Bold = 1,
        Underline = 4
    }
}