using System;
using SAPbouiCOM;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface
{
    public class ChooseFromListBuilder
    {
        private EditText _editText;
        private ChooseFromList _chooseFromList;

        private Conditions _chooseFromListConditions;
        private Action _afterAction;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private ChooseFromListBuilder(EditText editText, ChooseFromList chooseFromList)
        {
            _chooseFromList = chooseFromList;
            _editText = editText;
            _editText.ChooseFromListUID = chooseFromList.UniqueID;
            _editText.ChooseFromListAfter += editText_ChooseFromListAfter;
        }

        private void editText_ChooseFromListAfter(object sapObject, SBOItemEventArg eventArg)
        {
            DataTable selectedObjects = eventArg.To<ISBOChooseFromListEventArg>().SelectedObjects;
            if (selectedObjects == null)
                return;

            object value = selectedObjects.GetValue(_editText.ChooseFromListAlias, 0);
            _editText.Value = value.ToString();
            _afterAction?.Invoke();
        }

        public ChooseFromListBuilder SetAlias(string alias)
        {
            _editText.ChooseFromListAlias = alias;
            return this;
        }

        public ChooseFromListBuilder AppendAfterAction(Action action)
        {
            _afterAction += action;
            return this;
        }

        public ChooseFromListBuilder AppendBeforeFunction(Func<bool> function)
        {
            _editText.ChooseFromListBefore += delegate(object sboObject, SBOItemEventArg eventArg, out bool handle) { handle = function(); };

            return this;
        }


        public ChooseFromListBuilder AppendCondition(string columnAlias, BoConditionOperation operation, string columnValue,
            BoConditionRelationship? relationship = null)
        {
            if (_chooseFromListConditions == null)
                _chooseFromListConditions = _chooseFromList.GetConditions();

            Condition condition = _chooseFromListConditions.Add();
            condition.Alias = columnAlias;
            condition.Operation = operation;
            condition.CondVal = columnValue;

            if (relationship != null)
                condition.Relationship = relationship.Value;

            return this;
        }

        public ChooseFromListBuilder ReferenceConditions()
        {
            if (_chooseFromListConditions == null)
                throw new Exception(@"[Internal Error] Not condition appened");

            _chooseFromList.SetConditions(_chooseFromListConditions);
            return this;
        }

        public static ChooseFromListBuilder Reference(EditText editText, ChooseFromList chooseFromList)
        {
            return new ChooseFromListBuilder(editText, chooseFromList);
        }
    }
}