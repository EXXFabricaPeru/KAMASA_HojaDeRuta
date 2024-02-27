using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Ventura.Addon.PickingDistribution.Interface.Views.UserObjectViews
{
    [FormAttribute("UDO_FT_VS_PD_OFTP")]
    class UDOForm1 : UDOFormBase
    {
        public UDOForm1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("1_U_G").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Matrix Matrix0;

        private void OnCustomInitialize()
        {

        }
    }
}
