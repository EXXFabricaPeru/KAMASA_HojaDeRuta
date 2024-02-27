using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu
{
    public abstract class BaseMenu : IEnumerable
    {
        public SAPMenu RootSAPMenu { get; }

        public int order { set; get; }

        public SAPMenu[] ChildMenus { get; }

        protected BaseMenu(SAPMenu rootSAPMenu)
        {
            RootSAPMenu = rootSAPMenu;
            IList<SAPMenu> child = new List<SAPMenu>();
            BuildChildOptions(child);
            ChildMenus = child.ToArray();
        }

        public abstract void BuildChildOptions(IList<SAPMenu> childMenuReference);

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < ChildMenus.Length; i++)
            {
                yield return ChildMenus[i];
            }
        }
    }
}