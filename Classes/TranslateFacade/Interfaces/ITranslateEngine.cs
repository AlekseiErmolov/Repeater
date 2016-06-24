using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Classes.TranslateFacade
{
    public interface ITranslateEngine
    {
        string TranslateText(string key, string txtToTranslate, string from, string to);
        string GetKey();
    }
}
