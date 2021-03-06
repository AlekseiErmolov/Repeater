﻿using System.Threading.Tasks;

namespace Repeater.Infrastructure.TranslateFacade.Interfaces
{
    public interface ITranslateEngine
    {
        Task<string> TranslateText(string key, string txtToTranslate, string from, string to);
        Task<string> GetKey();
    }
}