using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.Domain;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class ChangeDocumentClosingState : BaseProcess
    {
        public ChangeDocumentClosingState(SAPConnection sapConnection) : base(sapConnection)
        {

        }

        #region Overrides of BaseProcess

        public override void Build()
        {
            MakeDomain<SettingsDomain>().ChangeClosingState();
        }

        #endregion
    }
}