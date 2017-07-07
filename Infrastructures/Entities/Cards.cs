using System;
using System.Collections.Generic;
using Repeater.Interfaces;
using Repeater.Model;

namespace Repeater.Infrastructures.Entities
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
