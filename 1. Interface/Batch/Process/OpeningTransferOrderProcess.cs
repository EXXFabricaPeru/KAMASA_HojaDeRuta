using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.Domain;
using Z.Collections.Extensions;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class OpeningTransferOrderProcess : BaseProcess
    {
        public OpeningTransferOrderProcess(SAPConnection sapConnection) : base(sapConnection)
        {
        }

        #region Overrides of BaseProcess

        public override void Build()
        {
            var domain = MakeDomain<TransferOrderDomain>();

            domain.OpenTransferOrders()
                .ForEach(order => 
                domain.UpdateOrder(order));
        }

        #endregion
    }
}