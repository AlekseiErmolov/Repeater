using System;
using System.Collections.Generic;
using Repeater.Interfaces;

namespace Repeater.Infrastructures.TranslateFacade.Classes
{
    public class TranslateEventArgs : EventArgs
    {
        public List<ICard> Cards { get; set; }
        public string Key { get; set; }
    }
}
