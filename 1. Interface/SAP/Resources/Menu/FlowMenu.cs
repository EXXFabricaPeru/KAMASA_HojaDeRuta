using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu
{
    public class FlowMenu : BaseMenu
    {
        //public FlowMenu() : base(new SAPMenu {Id = Constants.Menu.MAIN_FLOW, Description = "Hoja de Ruta Asignación",TargetUDOClass =  typeof(OHRG) })
        //{
        //    order = 1;
        //}

        public FlowMenu() : base(new SAPMenu { Id = $"HA.{OHRG.ID}", Description = "Asignación de Hoja de Ruta", TargetUDOClass = typeof(OHRG) })
        {
            order = 1;
        }
        public static int order=1;
        
        public override void BuildChildOptions(IList<SAPMenu> childMenuReference)
        {
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{OHRG.ID}", Description = OHRG.DESCRIPTION, TargetUDOClass = typeof(OHRG) });
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{Form1.ID}", Description = Form1.DESCRIPTION, TargetFormClass = typeof(Form1) });
            //childMenuReference.Add(new SAPMenu { Id = $"PD.{ReporteLiquidacion.ID}", Description = ReporteLiquidacion.DESCRIPTION, TargetFormClass = typeof(ReporteLiquidacion) });

        }
    }
}
