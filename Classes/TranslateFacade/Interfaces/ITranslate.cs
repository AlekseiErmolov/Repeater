using System.Collections.Generic;
using System.Threading.Tasks;
using Repeater.Interfaces;

namespace Repeater.Classes.TranslateFacade.Interfaces
{
    public interface ITranslate
    {
        Task<bool> Translate(string key, List<ICard> text);
    }
}
