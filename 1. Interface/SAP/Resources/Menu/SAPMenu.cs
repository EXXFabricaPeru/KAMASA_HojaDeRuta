using System;
using SAPbouiCOM.Framework;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.Forms;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu
{
    public struct SAPMenu
    {
        private Type _targetFormClass;
        private Type _targetUDOClass;

        public string Id { get; set; }
         
        public string Description { get; set; }

        public bool IsFormMenu 
            => _targetFormClass != null;

        public Type TargetUDOClass
        {
            set
            {
                if (value.BaseType != typeof(BaseUDO))
                    throw new Exception(string.Format(ErrorMessages.MissmatchType, value.FullName, nameof(BaseUDO)));

                _targetUDOClass = value;
            }
        }

        public Type TargetFormClass
        {
            set
            {
                if (value.BaseType != typeof(UserFormBase) && value.BaseType != typeof(BaseForm))
                    throw new Exception(string.Format(ErrorMessages.MissmatchType, value.FullName, nameof(UserFormBase)));

                _targetFormClass = value;
            }
        }

        public string TargetUDOId
        {
            get
            {
                var attribute = _targetUDOClass.GetCustomAttribute<Udo>();
                if (attribute == null)
                    throw new Exception(string.Format(ErrorMessages.RequiredAttributeDontFound, _targetUDOClass.FullName, nameof(Udo)));

                return attribute.UdoName;
            }
        }

        public UserFormBase TargetForm
            => (UserFormBase) Activator.CreateInstance(_targetFormClass);
    }
}