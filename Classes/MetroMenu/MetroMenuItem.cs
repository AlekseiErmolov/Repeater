using System.Windows.Media;

namespace Repeater.Classes
{
    class MetroMenuItem
    {
        public string Title { get; set; }

        public Geometry Image { get; set; }

        public MetroMenuItem(string title, Geometry image)
        {
            Image = image;
            Title = title;
        }
    }
}
