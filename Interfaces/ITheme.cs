using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Interfaces
{
    interface ITheme
    {
        string TitleColor { get; }
        string MenuColor { get; }
        string ProgressColor { get; }
        string ProgressStatusColor { get; }
        string TextColor { get; }
    }
}
