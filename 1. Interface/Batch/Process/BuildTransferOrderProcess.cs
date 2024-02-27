using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Code.Models;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Model.System.Header.Document;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Model.UDO.Header;
using Ventura.Addon.ComisionTarjetas.Domain;
using Ventura.Addon.ComisionTarjetas.Domain.Contracts;
using Z.Core.Extensions;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class BuildTransferOrderProcess : BaseProcess
    {
        public BuildTransferOrderProcess(SAPConnection sapConnection) : base(sapConnection)
        {
           
        }
       
        IInfrastructureDomain _infrastructureDomain;
        IMarketingDocumentDomain _marketingDocumentDomain;
        ITransferOrderDomain _domain;
        public override void Build()
        {
            _domain = new TransferOrderDomain(SAPConnection);
            _infrastructureDomain = new InfrastructureDomain(SAPConnection);
            _marketingDocumentDomain = new MarketingDocumentDomain(SAPConnection);
            var fecha = DateTime.UtcNow.AddDays(1).Date;

            //var emissionDate = DateTime.ParseExact("20220604", "yyyyMMdd", CultureInfo.InvariantCulture);
            var transferOrders = _domain.BuildTransferOrder(fecha, "0").ToList();

            var totalGenerateOrders = 0;
            for (var i = 0; i < transferOrders.Count; i++)
            {
                var currentOrder = transferOrders[i];

                if (currentOrder.STAD == ORTR.Status.APPROVED || currentOrder.STAD == "MA")
                {
                    currentOrder.STDS = currentOrder.ReasonDescription;
                  
                    var registerOrder = _domain.RegisterOrder(currentOrder);
                    if (!registerOrder.Item1)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    currentOrder.DocumentNumber = registerOrder.Item2.ToInt32();
                    currentOrder.DocumentEntry = registerOrder.Item3.ToInt32();
                }

                var response = Tuple.Create(false, @"[Internal Error] No se puede procesar este tipo de documento");

                if (currentOrder.OBJT == SAPConstanst.ObjectType.SALE_ORDER)
                    response = update_related_documents<ORDR>(currentOrder);
                else if (currentOrder.OBJT == SAPConstanst.ObjectType.INVENTORY_TRANSFER)
                    response = update_related_documents<OWTQ>(currentOrder);
                else if (currentOrder.OBJT == SAPConstanst.ObjectType.RETURN_ORDER)
                    response = update_related_documents<ORRR>(currentOrder);
                else if (currentOrder.OBJT == SAPConstanst.ObjectType.PURCHASE_ORDER)
                    response = update_related_documents<OPOR>(currentOrder);

                if (!response.Item1)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                totalGenerateOrders++;
            }
        }

        private Tuple<bool, string> update_related_documents<T>(ORTR currentOrder)
           where T : SAPDocument, new()
        {
            foreach (var referenceDocument in currentOrder.RelatedSAPDocuments)
            {
                OMTT reason = null;
                var reasonName = "";
                if (currentOrder.STMT != null)
                {
                    reason = _infrastructureDomain.RetrieveDisapprovalReasonByCode(currentOrder.STMT);
                    reasonName = $"{reason.Name} - {reason.Name}";
                }

                var sapDocument = new T
                {
                    DocumentEntry = referenceDocument.DCEN,
                    TransferOrderReference = currentOrder.DocumentNumber.ToString(),
                    TransferOrderValidationPreview = currentOrder.STAD,
                    TransferOrderValidationReason = reasonName,
                    TransferOrderValidationComments = currentOrder.ReasonDescription
                };

                var updateResponse = _marketingDocumentDomain.UpdateMarketingDocumentCustomFields(sapDocument);

                if (updateResponse.Item1)
                    continue;

                var errorMessage = !_domain.RemoveOrder(currentOrder.DocumentEntry).Item1
                    ? "No se pudo revertir la generación de la orden de traslado"
                    : $"Error al actualizar el documento de marketing '{referenceDocument.DCNM}' - {updateResponse.Item2}";

                return Tuple.Create(false, errorMessage);
            }

            return Tuple.Create(true, string.Empty);
        }


    }
}