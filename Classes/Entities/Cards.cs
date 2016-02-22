using Repeater.Interfaces;
using System;
using System.Collections.Generic;
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
