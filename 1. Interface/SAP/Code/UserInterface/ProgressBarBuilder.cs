using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.UserInterface
{
    public class ProgressBarBuilder : IDisposable
    {
        private ProgressBar _progressBar;
        private bool _isStopped;
        
        public ProgressBarBuilder(Application application, string title, int count)
        {
            _progressBar = application.StatusBar.CreateProgressBar(title, count, false);
        }

        public ProgressBarBuilder(string title, int count) 
            : this(ApplicationInterfaceHelper.ApplicationInstance, title, count)
        {
        }

        public void TickProgress(string message, int count)
        {
            _progressBar.Value = count;
            _progressBar.Text = message;
            Thread.Sleep(500);
        }

        public void AddProgressWithDelay(string message, int millisecondsDelay)
        {
            _progressBar.Value += 1;
            _progressBar.Text = message;
            Task.Delay(millisecondsDelay);
        }

        public void AddProgress(string message)
        {
            _progressBar.Value += 1;
            _progressBar.Text = message;
            Thread.Sleep(500);
        }

        public void Stop()
        {
            _isStopped = true;
            _progressBar.Stop();
        }

        public void Dispose()
        {
            if (!_isStopped)
                _progressBar.Stop();

            Marshal.ReleaseComObject(_progressBar);
        }

        public static void Execute(string title, int count, Action<ProgressBarBuilder> action)
        {
            using (var progressBar = new ProgressBarBuilder(title, count))
            {
                try
                {
                    action(progressBar);
                }
                catch (Exception)
                {
                    progressBar.Stop();
                    throw;
                }
            }
        }
    }
}