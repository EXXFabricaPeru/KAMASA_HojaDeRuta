using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.Structs;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ConditionHelper
    {
        public static Conditions MakeConditions(params QueryCondition[] queryConditions)
        {
            var conditions = new Conditions();
            for (var index = 0; index < queryConditions.Length; index++)
            {
                QueryCondition queryCondition = queryConditions[index];
                Condition condition = conditions.Add();
                condition.Operation = queryCondition.Operation;
                condition.Alias = queryCondition.ColumnName;
                condition.CondVal = queryCondition.Value;
                if (index + 1 < queryConditions.Length && queryCondition.Relationship != null)
                    condition.Relationship = queryCondition.Relationship.Value;
            }

            return conditions;
        }
        
    }
}