using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Repeater.Model;

namespace Repeater.Classes.Entities
{
    [Serializable]
    public class Cards: ICards
    {
        public Cards()
        {
            CardsCollection = new List<Card>();
        }

        public List<Card> CardsCollection { get; set; }
    }
}
