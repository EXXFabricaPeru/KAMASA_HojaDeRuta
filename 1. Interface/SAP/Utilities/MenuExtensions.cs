using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class MenuExtensions
    {
        public static SAPMenu FindMenuId(this IList<BaseMenu> menus, string id)
        {
            foreach (BaseMenu baseMenu in menus)
            {
                if (baseMenu.RootSAPMenu.Id == id)
                    return baseMenu.RootSAPMenu;

                foreach (SAPMenu childMenu in baseMenu.ChildMenus)
                {
                    if (childMenu.Id == id)
                        return childMenu;
                }
            }

            throw new ArgumentException("Menu don't found", nameof(id));
        }
    }
}