using System;
using System.Threading.Tasks;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.Batch.Process;

namespace Ventura.Addon.ComisionTarjetas.Batch
{
    public static class Runner
    {
        public static void Main(string[] arguments)
        {
            //string  cki = Console.ReadKey().ToString();
            // while (Console.ReadKey().Key != ConsoleKey.Enter) {
            //      cki = cki+Console.ReadKey().ToString();
            // }
            arguments = new String[1];
            //arguments[0] = Constants.ProcessConstant.RECEPTION_CLOSING_SALE_ORDER;
            //arguments[0] = Constants.ProcessConstant.OPEN_TRANSFER_ORDER;
            //arguments[0] = Constants.ProcessConstant.BUILD_TRANSFER_ORDER;
            //arguments[0] = Constants.ProcessConstant.POSPONED_TRANSFER_ORDER;
            //arguments[0] = Constants.ProcessConstant.CHANGE_DOCUMENT_CLOSING_STATE;
            //arguments[0] = Constants.ProcessConstant.UPDATE_LICENSE;



            ApplicationSettings settings = ApplicationSettings.MainSettings;
            //settings.Connection.Company = "BD_PRUEBA_PRODUCCION_20231204";
            var processes = new Task[arguments.Length];

            for (var index = 0; index < arguments.Length; index++)
            {
                string argument = arguments[index];
                processes[index] = execute_process(argument, settings);
            }

            Task.WaitAll(processes);
        }

        private static async Task execute_process(string argument, ApplicationSettings settings)
        {
            await Task.Run(() =>
            {
                BaseProcess baseProcess = null;
                switch (argument)
                {
                    case Constants.ProcessConstant.BUILD_TRANSFER_ORDER:
                        baseProcess = new BuildTransferOrderProcess(settings.Connection);
                        break;
                    case Constants.ProcessConstant.RECEPTION_CLOSING_SALE_ORDER:
                        baseProcess = new ClosingSaleOrderProcess(settings.Connection);
                        break;
                    case Constants.ProcessConstant.OPEN_TRANSFER_ORDER:
                        baseProcess = new OpeningTransferOrderProcess(settings.Connection);
                        break;
                    case Constants.ProcessConstant.POSPONED_TRANSFER_ORDER:
                        baseProcess = new PosponedTransferOrderProcess(settings.Connection);
                        break;
                    case Constants.ProcessConstant.CHANGE_DOCUMENT_CLOSING_STATE:
                        baseProcess = new ChangeDocumentClosingState(settings.Connection);
                        break;
                    case Constants.ProcessConstant.UPDATE_LICENSE:
                        baseProcess = new UpdateLicenseProcess(settings.Connection);
                        break;
                }

                if (baseProcess == null)
                    return;

                baseProcess.Build();
            });
        }
    }
}