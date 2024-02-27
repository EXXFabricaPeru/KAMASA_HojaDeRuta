using SAPbobsCOM;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseMessageRepository : Code.Repository
    {
        protected BaseMessageRepository(Company company) : base(company)
        {
        }

        public abstract void SendMessage_Alert(string asunto,string message,List<string> users, BoMsgPriorities priority);
    }
}