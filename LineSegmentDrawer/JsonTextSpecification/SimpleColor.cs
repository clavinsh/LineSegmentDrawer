using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSegmentDrawer.JsonTextSpecification
{
    public class SimpleColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public SimpleColor()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        public SimpleColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public Color ToColor()
        {
            return Color.FromArgb(R, G, B);
        }
    }
}
