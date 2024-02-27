using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Code.Models;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Model.System.Header.Document;
using Ventura.Addon.ComisionTarjetas.Domain;
using Z.Collections.Extensions;
using Z.Core.Extensions;
using static Ventura.Addon.ComisionTarjetas.CrossCutting.Code.Models.SAPDocument;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class ClosingSaleOrderProcess : BaseProcess
    {
        public ClosingSaleOrderProcess(SAPConnection sapConnection) : base(sapConnection)
        {
        }

        #region Overrides of BaseProcess

        public override void Build()
        {
            var saleOrderDomain = MakeDomain<SaleOrderDomain>();
            var marketingDocumentDomain = MakeDomain<MarketingDocumentDomain>();
            SettingsDomain set = new SettingsDomain(SAPConnection);
            //ltoro 27-11-2023 se comenta - se separa close en otro batch CHANGE_DOCUMENT_CLOSING_STATE
            //MakeDomain<SettingsDomain>().ChangeClosingState();
            //List<ORDR> documents3 = saleOrderDomain
            //    .Retrieve(DateTime.Now.AddDays(1)).ToList();

            IEnumerable<ORDR> documents = saleOrderDomain
                .Retrieve(DateTime.Now.AddDays(1))
                //.Where(
                ////item => item.TransferOrderReference.IsDefault()
                ////&& item.IsExtemporaneousApproved()
                ////&& item.IsCashPaymentApproved() //ltoro 
                ////&& !item.HasValidationPrevious()
                //)
                .OrderBy(item => item.DistributionPriority, SAPDocument.SortByDistributionPriority);

            documents = documents.Where(
                item => 
                item.TransferOrderReference==""
                && (item.Extemporal == "" || item.Extemporal == "N" || item.Extemporal == "A")
                && (item.isCashPayment == "" || item.isCashPayment == "N" || item.isCashPayment == "A" )//ltoro 
                && !(item.DistributionValidationCode == DistributionValidationStatus.MANUAL_APPROVED || item.DistributionValidationCode == DistributionValidationStatus.MANUAL_DISAPPROVED)
                );

                //saleOrderDomain.ValidateStock(documents)
                //    .Select(item => new ORDR
                //    {
                //        DocumentEntry = item.DocumentEntry,
                //        DistributionValidationCode = item.DistributionValidationCode,
                //        DistributionValidationReason = item.DistributionValidationReason,
                //        DistributionValidationMessage = item.DistributionValidationMessage,
                //    })
                //    .ForEach(item => marketingDocumentDomain.UpdateMarketingDocumentCustomFields(item));
            var orders = saleOrderDomain.ValidateStock(documents);

            orders.Select(item => new ORDR
                 {
                     DocumentEntry = item.DocumentEntry,
                     DistributionValidationCode = item.DistributionValidationCode,
                     DistributionValidationReason = item.DistributionValidationReason,
                     DistributionValidationMessage = item.DistributionValidationMessage,
                 })
                  .ForEach(item => marketingDocumentDomain.UpdateMarketingDocumentCustomFields(item));


            foreach (var item in orders)
            {
                if (item.DistributionValidationCode == "DI")
                {
                    SendMessage(item);
                }
            }



        }

        private void SendMessage(ORDR order)
        {
            SAPbobsCOM.Messages msg = (SAPbobsCOM.Messages)SAPConnection.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);

            var recordSet = SAPConnection
              .GetBusinessObject(BoObjectTypes.BoRecordsetEx)
              .To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery($@"select ""USER_CODE"" from ""OUSR"" where ""USERID"" = {order.UserSign} ");
            var userCode = "";
            List<string> listUser = new List<string>();

            while (!recordSet.EoF)
            {
                userCode = recordSet.GetColumnValue("USER_CODE").ToString();
                recordSet.MoveNext();
            }

            msg.Priority = BoMsgPriorities.pr_High;


            msg.Subject = ": Alerta de Desaprobación de Ordenes de Venta";
            var mensaje = $"Se ha desaprobado automáticamente la OV de venta {order.DocumentNumber}";

            msg.MessageText = mensaje;
            msg.Recipients.UserCode = userCode;
            msg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;

            var result = msg.Add();
            if (result != 0) // Check the result
            {
                string error;
                string vm_GetLastErrorDescription_string = SAPConnection.GetLastErrorDescription();

                //Company.GetLastError(out res, out error);                 
            }
        }
        #endregion
    }
}