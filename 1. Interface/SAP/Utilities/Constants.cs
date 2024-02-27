using System.Drawing;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class Constants
    {
        public static class Menu
        {
            public const string MAIN_APPLICATION = "3300";
            public const string MAIN_FLOW = "3303";
        }

        public static class Colors
        {
            public static Color DISABLE = Color.FromArgb(255, 231, 236, 240);
        }

        public static class PartialAtention
        {
            public const string PENDIENTE = "Si";
            public const string NO = "No";
            public const string ATENDIDO = "Atendido";
            public const string CERRADO = "Cerrado";
            public const string PENDIENTE_ATENDER= "PA";
            public const string NO_ATENDIDO = "No_Atendido";
        }
    }
}