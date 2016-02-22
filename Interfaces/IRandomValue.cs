using System.Collections.Generic;

namespace Repeater.Interfaces
{
    public interface IRandomValue
    {
        ICard Card { get; set; }
        IEnumerable<ICard> Cards { get; set; }
    }
}
