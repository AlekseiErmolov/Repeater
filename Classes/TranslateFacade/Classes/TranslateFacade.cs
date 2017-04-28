using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using Repeater.Interfaces;

namespace Repeater.Classes.TranslateFacade
{
    internal class TranslateFacade : ITranslate
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="engine"></param>
        public TranslateFacade(IUnityContainer container)
        {
            _TranslateEngine = container.Resolve<ITranslateEngine>();
            ConfigureBackgroundWorker();
        }

        private BackgroundWorker _bw { get; set; }

        private List<ICard> _taskList { get; set; }

        private string SecretKey { get; set; }

        /// <summary>
        ///     Движок перевода
        /// </summary>
        private ITranslateEngine _TranslateEngine { get; }

        /// <summary>
        ///     Основное событие завершения перевода
        /// </summary>
        public event EventHandler GetTranslateResultEvent;

        /// <summary>
        ///     Делает перевод текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void Translate(string key, List<ICard> text)
        {
            _taskList = text;

            if (_bw.IsBusy != true)
                _bw.RunWorkerAsync(key);
        }

        /// <summary>
        ///     Возвращает ключ
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            var result = _TranslateEngine.GetKey();
            return result;
        }

        /// <summary>
        ///     Рабочий поток BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            SecretKey = e.Argument as string;

            if (string.IsNullOrEmpty(SecretKey))
                SecretKey = GetKey();

            if (worker != null && worker.CancellationPending)
                e.Cancel = true;
            else
            {
                // Perform a time consuming operation and report progress.
                foreach (var card in _taskList)
                {
                    if (string.IsNullOrEmpty(card.ForeignTask))
                        card.ForeignTask = _TranslateEngine.TranslateText(SecretKey, card.NativeTask, "ru", "en");
                    else if (string.IsNullOrEmpty(card.NativeTask))
                        card.NativeTask = _TranslateEngine.TranslateText(SecretKey, card.ForeignTask, "en", "ru");
                }
            }
        }

        /// <summary>
        ///     Событие изменения прогресса BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        ///     Событие завершения работы BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (GetTranslateResultEvent != null)
                GetTranslateResultEvent.Invoke(sender, new TranslateEventArgs { Cards = _taskList, Key = SecretKey });
        }

        /// <summary>
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
    }
}