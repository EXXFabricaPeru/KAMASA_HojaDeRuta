using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class BPP_VEHICU : BaseSAPTable
    {
        public const string ID = "BPP_VEHICU";
        public const string DESCRIPTION = "Vehículos";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Code")]
        public string Code { get; set; }

 
        [EnhancedColumn(4), FieldNoRelated("U_BPP_VEMA", "Marca de Vehiculo", BoDbTypes.Alpha)]
        public string Marca { get; set; }

        [EnhancedColumn(17), ColumnProperty("Name", ColumnName = "Name")]
        public string Name { get; set; }

        [EnhancedColumn(18), FieldNoRelated("U_EXK_CPDC", "Capacidad de Carga", BoDbTypes.Price)]
        public double LoadCapacity { get; set; }

        [EnhancedColumn(19), FieldNoRelated("U_EXK_VLDC", "Volumen de Carga", BoDbTypes.Price)]
        public double VolumeCapacity { get; set; }

        [EnhancedColumn(20), FieldNoRelated("U_EXK_TPDS", "Tipo de Distribución", BoDbTypes.Alpha, Size = 3)]
        public string DistributionType { get; set; }

        [EnhancedColumn(21), FieldNoRelated("U_EXK_SRVH", "Posee Refrigeración", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string HasRefrigeration { get; set; }

        [EnhancedColumn(22), FieldNoRelated("U_EXK_TUCI", "Tarjeta Unica de Circulación", BoDbTypes.Alpha, Size = 100)]
        public string SingleCirculationCard { get; set; }
    }
}