using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews
{
    [FormAttribute("Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews.Motivos_de_Ajustes", "Views/UserObjectViews/Motivos de Ajustes.b1f")]
    class Motivos_de_Ajustes : UserFormBase
    {
        public Motivos_de_Ajustes()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_4").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.StaticText StaticText2;
    }
}
