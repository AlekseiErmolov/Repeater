using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Interfaces
{
    public interface IRandomValue
    {
        ICard Card { get; set; }
        IEnumerable<ICard> Cards { get; set; }
    }
}
