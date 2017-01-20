using Repeater.Interfaces;

namespace Repeater.Classes.Themes
{
    public class ThemeBlueGrey : ITheme
    {
        public string TitleColor
        {
            get
            {
                return "#CFD8DC";
            }
        }

        public string MenuColor
        {
            get
            {
                return "#B0BEC5";
            }
        }

        public string ProgressColor
        {
            get
            {
                return "#E8E8E8";
            }
        }

        public string ProgressStatusColor
        {
            get
            {
                return "#546E7A";
            }
        }

        public string TextColor
        {
            get
            {
                return "#ECEFF1";
            }
        }
    }
}
