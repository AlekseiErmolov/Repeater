using Repeater.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Interfaces
{
    interface ICards
    {
        List<Card> CardsCollection { get; set; }
    }
}
