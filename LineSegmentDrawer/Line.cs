using System.Drawing;

namespace LineSegmentDrawer
{
    public class Line
    {
        private double x0, y0, x1, y1;
        private readonly Color foreColor;

        public Line(double x0, double y0, double x1, double y1, Color color)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.y1 = y1;
            this.x1 = x1;

            this.foreColor = color;
        }

        private void Plot(Bitmap bitmap, double x, double y, double c)
        {
            int alpha = (int)(c * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Color color = Color.FromArgb(alpha, foreColor);
            bitmap.SetPixel((int)x, (int)y, color);
        }

        int IntegerPart(double x) { return (int)x; }

        int Round(double x) { return IntegerPart(x + 0.5); }

        double FractionalPart(double x)
        {
            if (x < 0) return (1 - (x - Math.Floor(x)));
            return (x - Math.Floor(x));
        }

        double InverseFractionalPart(double x)
        {
            return 1 - FractionalPart(x);
        }


        public void Draw(Bitmap bitmap)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            double temp;
            if (steep)
            {
                temp = x0; x0 = y0; y0 = temp;
                temp = x1; x1 = y1; y1 = temp;
            }
            if (x0 > x1)
            {
                temp = x0; x0 = x1; x1 = temp;
                temp = y0; y0 = y1; y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = Round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = InverseFractionalPart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = IntegerPart(yEnd);

            if (steep)
            {
                Plot(bitmap, yPixel1, xPixel1, InverseFractionalPart(yEnd) * xGap);
                Plot(bitmap, yPixel1 + 1, xPixel1, FractionalPart(yEnd) * xGap);
            }
            else
            {
                Plot(bitmap, xPixel1, yPixel1, InverseFractionalPart(yEnd) * xGap);
                Plot(bitmap, xPixel1, yPixel1 + 1, FractionalPart(yEnd) * xGap);
            }
            double yIntersection = yEnd + gradient;

            xEnd = Round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = FractionalPart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = IntegerPart(yEnd);
            if (steep)
            {
                Plot(bitmap, yPixel2, xPixel2, InverseFractionalPart(yEnd) * xGap);
                Plot(bitmap, yPixel2 + 1, xPixel2, FractionalPart(yEnd) * xGap);
            }
            else
            {
                Plot(bitmap, xPixel2, yPixel2, InverseFractionalPart(yEnd) * xGap);
                Plot(bitmap, xPixel2, yPixel2 + 1, FractionalPart(yEnd) * xGap);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    Plot(bitmap, IntegerPart(yIntersection), x, InverseFractionalPart(yIntersection));
                    Plot(bitmap, IntegerPart(yIntersection) + 1, x, FractionalPart(yIntersection));
                    yIntersection += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    Plot(bitmap, x, IntegerPart(yIntersection), InverseFractionalPart(yIntersection));
                    Plot(bitmap, x, IntegerPart(yIntersection) + 1, FractionalPart(yIntersection));
                    yIntersection += gradient;
                }
            }
        }
    }
}
