using System.Collections.Generic;

namespace Repeater.Interfaces
{
    public interface ICardService
    {
        ICard GetCardReduceList(List<ICard> lesson);
    }
}
