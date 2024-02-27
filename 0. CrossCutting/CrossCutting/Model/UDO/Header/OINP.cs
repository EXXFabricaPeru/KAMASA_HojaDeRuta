using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OINP : BaseSAPTable
    {
        private int _type;

        public const string ID = "VS_PD_OINP";
        public const string DESCRIPTION = "Tabla de Indi. Pri.";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Código")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Descripción")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_VALR", "Valor", BoDbTypes.Price)]
        public decimal Value { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_TIPO", "Tipo", BoDbTypes.Alpha, Size = 2)]
        [Val("1", "Mayor valor con menor prioridad"), Val("-1", "Menor valor con menor prioridad")]
        public int Type => _type;

        public void SetType(string type)
        {
            _type = Convert.ToInt32(type);
        }
    }
}