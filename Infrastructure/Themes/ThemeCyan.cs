using Repeater.Interfaces;

namespace Repeater.Infrastructure.Themes
{
    public class ThemeCyan : ITheme
    {
        public string TitleColor
        {
            get
            {
                return "#B2EBF2";
            }
        }

        public string MenuColor
        {
            get
            {
                return "#00BCD4";
            }
        }

        public string ProgressColor
        {
            get
            {
                return "#B2EBF2";
            }
        }

        public string ProgressStatusColor
        {
            get
            {
                return "#00838F";
            }
        }

        public string TextColor
        {
            get
            {
                return "#E0F7FA";
            }
        }
    }
}
