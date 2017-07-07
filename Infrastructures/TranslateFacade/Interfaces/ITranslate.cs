using System.Collections.Generic;
using System.Threading.Tasks;
using Repeater.Interfaces;

namespace Repeater.Infrastructures.TranslateFacade.Interfaces
{
    public interface ITranslate
    {
        Task<bool> Translate(string key, List<ICard> text);
    }
}