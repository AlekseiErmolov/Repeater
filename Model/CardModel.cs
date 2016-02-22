using Repeater.Interfaces;
using System;

namespace Repeater.Model
{
    [Serializable]
    public class Card : ICard
    {
        public string Comment { get; set; }
        public string ForeignTask { get; set; }
        public string NativeTask { get; set; }
        public string UserAnswer { get; set; }

    }
}
