using Repeater.Interfaces;

namespace Repeater.Classes.Themes
{
    public class ThemeGreen : ITheme
    {
        public string TitleColor
        {
            get
            {
                return "#81C784";
            }
        }

        public string MenuColor
        {
            get
            {
                return "#C8E6C9";
            }
        }

        public string ProgressColor
        {
            get
            {
                return "#66BB6A";
            }
        }

        public string ProgressStatusColor
        {
            get
            {
                return "#2E7D32";
            }
        }

        public string TextColor
        {
            get
            {
                return "#E8F5E9";
            }
        }
    }
}
