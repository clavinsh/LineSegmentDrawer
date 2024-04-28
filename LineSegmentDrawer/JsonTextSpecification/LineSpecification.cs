using System.Drawing;

namespace LineSegmentDrawer.JsonTextSpecification
{
    public class LineSpecification
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
        public required SimpleColor Color { get; set; }
    }
}
