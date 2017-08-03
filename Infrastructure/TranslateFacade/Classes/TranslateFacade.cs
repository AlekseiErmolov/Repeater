using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeater.Infrastructure.TranslateFacade.Interfaces;
using Repeater.Interfaces;

namespace Repeater.Infrastructure.TranslateFacade.Classes
{
    internal class TranslateFacade : ITranslate
    {
        private string SecretKey { get; set; }
        public ITranslateEngine TranslateEngine { get; set; }
        public ILoggerWrap Logger { get; set; }

        /// <summary>
        ///     Translate all cards.
        /// </summary>
        public async Task<bool> Translate(string key, List<ICard> cards)
        {
            try
            {
                if (string.IsNullOrEmpty(SecretKey))
                    SecretKey = await GetKey();

                // Perform a time consuming operation and report progress.
                foreach (var card in cards)
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
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex, ex.Message);
            }

            return true;
        }

        /// <summary>
        ///     Get the auth key.
        /// </summary>
        public async Task<string> GetKey()
        {
            var result = await TranslateEngine.GetKey();
            return result;
        }
    }
}