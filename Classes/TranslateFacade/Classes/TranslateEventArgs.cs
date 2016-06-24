using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Classes.TranslateFacade
{
    public class TranslateEventArgs : EventArgs
    {
        public List<ICard> Cards { get; set; }
        public string Key { get; set; }
    }
}
