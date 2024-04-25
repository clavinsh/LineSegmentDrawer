using System.Drawing;

namespace LineSegmentDrawer
{
    public class Line(Color lineColor, Point start, Point end)
    {
        public Bitmap CreateImage(int imageWidth, int imageHeight)
        {
            Bitmap bmp = new(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(bmp);

            DrawLine(g, start.X, start.Y, end.X, end.Y, lineColor);

            return bmp;
        }

        private static void DrawLine(Graphics g, int x0, int y0, int x1, int y1, Color color)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;

            float gradient = dy / dx;

            float yStep = y0 + gradient;

            for(int x = x0; x < x1; x++)
            {
                g.FillRectangle(new SolidBrush(color), x, (int)yStep, 1, 1);

                yStep += gradient;
            }
        }
    }
}
