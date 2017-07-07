using Repeater.Interfaces;

namespace Repeater.Infrastructures.Themes
{
    public class ThemeBlue : ITheme
    {
        public string TitleColor
        {
            get { return "#64B5F6"; }
        }

        public string MenuColor
        {
            get { return "#BBDEFB"; }
        }

        public string ProgressColor
        {
            get { return "#BBDEFB"; }
        }

        public string ProgressStatusColor
        {
            get { return "#1565C0"; }
        }

        public string TextColor
        {
            get { return "#E3F2FD"; }
        }
    }
}