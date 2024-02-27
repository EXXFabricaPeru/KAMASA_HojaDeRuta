using SAPbouiCOM;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.Structs
{
    public struct QueryCondition
    {
        public string ColumnName { get; set; }
        public BoConditionOperation Operation { get; set; }
        public string Value { get; set; }
        public BoConditionRelationship? Relationship { get; set; }

        public QueryCondition(string columnName, BoConditionOperation operation, string value)
            : this(columnName, operation, value, null)
        {
            ColumnName = columnName;
            Operation = operation;
            Value = value;
        }

        public QueryCondition(string columnName, BoConditionOperation operation, string value, BoConditionRelationship? relationship)
        {
            ColumnName = columnName;
            Operation = operation;
            Value = value;
            Relationship = relationship;
        }
    }
}