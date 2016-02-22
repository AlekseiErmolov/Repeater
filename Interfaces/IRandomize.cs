using System.Collections.Generic;

namespace Repeater.Interfaces
{
    public interface IRandomize
    {
        ICard GetCardReduceList(ref List<ICard> lesson);
    }
}
