using System;
using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class MessageRepository : BaseMessageRepository
    {
        private const string BUSINESS_PARTNER_FIND_QUERY = "select * from \"OCRD\" where \"CardCode\" = '{0}'";
        private const string ADDRESS_FIND_QUERY = "select * from \"CRD1\" where \"CardCode\" = '{0}'";

        public MessageRepository(Company company) : base(company)
        {
        }

        public override void SendMessage_Alert(string asunto,string message, List<string> listUser, BoMsgPriorities priority)
        {
            SAPbobsCOM.Messages msg = (SAPbobsCOM.Messages)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);
            var oCmpSrv = Company.GetCompanyService();
            var oMessageService = (SAPbobsCOM.MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);

            msg.Subject = asunto;
            msg.Priority = priority;
            msg.MessageText = message;

            for (int i = 0; i < listUser.Count; i++)
            {
                if (i > 0)
                    msg.Recipients.Add();
                msg.Recipients.SetCurrentLine(i);
                msg.Recipients.UserCode = listUser[i];
                msg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;

            }

            var result = msg.Add();


            if (result != 0) // Check the result
            {
                string error;
                string vm_GetLastErrorDescription_string = Company.GetLastErrorDescription();

                //Company.GetLastError(out res, out error);                 
            }
        }

        //public override OCRD FindByCode(string cardCode)
        //{
        //    var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
        //    recordset.DoQuery(string.Format(BUSINESS_PARTNER_FIND_QUERY, cardCode));
        //    var result = recordset.RetrieveBasicSAPEntity<OCRD>();

        //    if (RetrieveFormat != RetrieveFormat.Complete)
        //        return result;

        //    recordset.DoQuery(string.Format(ADDRESS_FIND_QUERY, cardCode));
        //    IList<CRD1> addresses = new List<CRD1>();
        //    while (!recordset.EoF)
        //    {
        //        var item = recordset.RetrieveBasicSAPEntity<CRD1>();
        //        addresses.Add(item);
        //        recordset.MoveNext();
        //    }
        //    result.Addresses = addresses;
        //    return result;
        //}
    }
}