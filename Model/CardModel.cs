using Repeater.Classes.Entities;
using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
