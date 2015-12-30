using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Interfaces
{
    public interface IRandomize
    {
        ICard GetCardReduceList(ref List<ICard> lesson);
    }
}
