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
        public TranslateFacade(IUnityContainer container)
        {
            TranslateEngine = container.Resolve<ITranslateEngine>();
            ConfigureBackgroundWorker();
        }

        private BackgroundWorker BackWorker { get; set; }

        private List<ICard> TaskList { get; set; }

        private string SecretKey { get; set; }

        /// <summary>
        ///     Движок перевода
        /// </summary>
        private ITranslateEngine TranslateEngine { get; }

        /// <summary>
        ///     Основное событие завершения перевода
        /// </summary>
        public event EventHandler GetTranslateResultEvent;

        /// <summary>
        ///     Делает перевод текста
        /// </summary>
        public void Translate(string key, List<ICard> text)
        {
            TaskList = text;

            if (BackWorker.IsBusy != true)
                BackWorker.RunWorkerAsync(key);
        }

        /// <summary>
        ///     Возвращает ключ
        /// </summary>
        public string GetKey()
        {
            var result = TranslateEngine.GetKey();
            return result;
        }

        /// <summary>
        ///     Рабочий поток BackgroundWorker
        /// </summary>
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
                foreach (var card in TaskList)
                {
                    if (string.IsNullOrEmpty(card.ForeignTask))
                        card.ForeignTask = TranslateEngine.TranslateText(SecretKey, card.NativeTask, "ru", "en")
                            .Trim()
                            .ToLowerInvariant();
                    else if (string.IsNullOrEmpty(card.NativeTask))
                        card.NativeTask = TranslateEngine.TranslateText(SecretKey, card.ForeignTask, "en", "ru")
                            .Trim()
                            .ToLowerInvariant();
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
                GetTranslateResultEvent.Invoke(sender, new TranslateEventArgs { Cards = TaskList, Key = SecretKey });
        }

        /// <summary>
        /// </summary>
        private void ConfigureBackgroundWorker()
        {
            BackWorker = new BackgroundWorker();
            BackWorker.WorkerSupportsCancellation = true;
            BackWorker.WorkerReportsProgress = true;

            BackWorker.DoWork += _bw_DoWork;
            BackWorker.ProgressChanged += _bw_ProgressChanged;
            BackWorker.RunWorkerCompleted += _bw_RunWorkerCompleted;
        }
    }
}