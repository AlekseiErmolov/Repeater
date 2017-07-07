using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Repeater.Classes.TranslateFacade.Interfaces;
using Repeater.Interfaces;

namespace Repeater.Classes.TranslateFacade.Classes
{
    internal class TranslateFacade : ITranslate
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public TranslateFacade(IUnityContainer container)
        {
            TranslateEngine = container.Resolve<ITranslateEngine>();
        }

        private List<ICard> TaskList { get; set; }

        private string SecretKey { get; set; }

        /// <summary>
        ///     Движок перевода
        /// </summary>
        private ITranslateEngine TranslateEngine { get; }

        /// <summary>
        ///     Делает перевод текста
        /// </summary>
        public async Task<bool> Translate(string key, List<ICard> text)
        {
            TaskList = text;

            if (string.IsNullOrEmpty(SecretKey))
                SecretKey = await GetKey();

            // Perform a time consuming operation and report progress.
            foreach (var card in TaskList)
            {
                if (string.IsNullOrEmpty(card.ForeignTask))
                {
                    card.ForeignTask = await TranslateEngine.TranslateText(SecretKey, card.NativeTask, "ru", "en");
                    card.ForeignTask = card.ForeignTask.Trim().ToLowerInvariant();
                }
                else if (string.IsNullOrEmpty(card.NativeTask))
                {
                    card.NativeTask = await TranslateEngine.TranslateText(SecretKey, card.ForeignTask, "en", "ru");
                    card.NativeTask = card.NativeTask.Trim().ToLowerInvariant();
                }
            }

            return true;
        }

        /// <summary>
        ///     Возвращает ключ
        /// </summary>
        public async Task<string> GetKey()
        {
            var result = await TranslateEngine.GetKey();
            return result;
        }
    }
}