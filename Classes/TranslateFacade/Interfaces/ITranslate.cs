using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Classes.TranslateFacade
{
    public interface ITranslate
    {
        void Translate(string key, List<ICard> text);

        event EventHandler GetTranslateResultEvent;
    }
}
