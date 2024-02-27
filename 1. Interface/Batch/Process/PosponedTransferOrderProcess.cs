using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Constant;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Model.UDO.Header;
using Ventura.Addon.ComisionTarjetas.Domain;
using Z.Collections.Extensions;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class PosponedTransferOrderProcess : BaseProcess
    {
        public PosponedTransferOrderProcess(SAPConnection sapConnection) : base(sapConnection)
        {
        }


        #region Overrides of BaseProcess

        public override void Build()
        {
            SettingsDomain set = new SettingsDomain(SAPConnection);
            //ltoro 27-11-2023 se comenta - se separa close en otro batch CHANGE_DOCUMENT_CLOSING_STATE
            // MakeDomain<SettingsDomain>().ChangeClosingState(); 

            var domain = MakeDomain<TransferOrderDomain>();

            var transferOrders = domain.OpenTransferOrders();

            foreach (var item in transferOrders)
            {
                if (item.STAD == ORTR.Status.POSTPONED)
                    item.FEDS = DateTime.Now.AddDays(2);
                domain.UpdateOrder(item);
            }


        }

        #endregion
    }

}
