using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Classes.TranslateFacade
{
    class TranslateFacade : ITranslate
    {
        /// <summary>
        /// Основное событие завершения перевода
        /// </summary>
        public event EventHandler GetTranslateResultEvent;

        BackgroundWorker _bw { get; set; }
        List<ICard> _taskList { get; set; }

        private string SecretKey { get; set; }
        /// <summary>
        /// Движок перевода
        /// </summary>
        ITranslateEngine _TranslateEngine { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="engine"></param>
        public TranslateFacade(ITranslateEngine engine)
        {
            _TranslateEngine = engine;
            ConfigureBackgroundWorker();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigureBackgroundWorker()
        {
            _bw = new BackgroundWorker();
            _bw.WorkerSupportsCancellation = true;
            _bw.WorkerReportsProgress = true;

            _bw.DoWork += _bw_DoWork;
            _bw.ProgressChanged += _bw_ProgressChanged;
            _bw.RunWorkerCompleted += _bw_RunWorkerCompleted;
        }


        /// <summary>
        /// Событие завершения работы BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (GetTranslateResultEvent != null)
                GetTranslateResultEvent.Invoke(sender, new TranslateEventArgs { Cards = _taskList, Key = SecretKey });
        }


        /// <summary>
        /// Событие изменения прогресса BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Рабочий поток BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            SecretKey = e.Argument as string;

            if(string.IsNullOrEmpty(SecretKey))
            {
                SecretKey = GetKey();
            }

            if (!string.IsNullOrEmpty(SecretKey))
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    foreach (var card in _taskList)
                    {
                        if (string.IsNullOrEmpty(card.ForeignTask))
                        {
                            card.ForeignTask = _TranslateEngine.TranslateText(SecretKey, card.NativeTask, "ru", "en");
                        }
                        else if (string.IsNullOrEmpty(card.NativeTask))
                        {
                            card.NativeTask = _TranslateEngine.TranslateText(SecretKey, card.ForeignTask, "en", "ru");
                        }
                    }

                    //worker.ReportProgress((i * 10));
                }
            }
        }

        /// <summary>
        /// Делает перевод текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void Translate(string key, List<ICard> text)
        {
            _taskList = text;

            if (_bw.IsBusy != true)
            {
                _bw.RunWorkerAsync(key);
            }
        }

        /// <summary>
        /// Возвращает ключ
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            var result = _TranslateEngine.GetKey();
            return result;
        }
    }
}
