using System.Drawing;

namespace LineSegmentDrawer
{
    public class Line(Color lineColor, Color backgroundColor, double x0, double y0, double x1, double y1)
    {
        public Bitmap CreateImage(int imageWidth, int imageHeight)
        {
            Bitmap bmp = new(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(bmp);

            //g.Clear(backgroundColor);

            DrawLineWu(g, x0, y0, x1, y1, lineColor);

            return bmp;
        }

        //line drawing with anti-aliasing using Xiaolin Wu method
        private static void DrawLineWu(Graphics g, double x0, double y0, double x1, double y1, Color color)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            double dx = x1 - x0;
            double dy = y1 - y0;

            double gradient;
            if (dx == 0.0f)
            {
                gradient = 1.0f;
            }
            else
            {
                gradient = dy / dx;
            }

            // first end point
            double xEnd = Math.Round(x0, 0, MidpointRounding.AwayFromZero);
            double yEnd = y0 + gradient * (xEnd - x0);

            double xGap = InverseFractionalPart(x0 + 0.5);

            double xPixel1 = xEnd;
            double yPixel1 = (int)yEnd;

            if (steep)
            {
                PlotPixel(g, (int)yPixel1, (int)xPixel1, InverseFractionalPart(yEnd) * xGap, color);
                PlotPixel(g, (int)yPixel1 + 1, (int)xPixel1, FractionalPart(yEnd) * xGap, color);
            }
            else
            {
                PlotPixel(g, (int)xPixel1, (int)yPixel1, InverseFractionalPart(yEnd) * xGap, color);
                PlotPixel(g, (int)xPixel1, (int)yPixel1 + 1, FractionalPart(yEnd) * xGap, color);
            }

            double intersectionY = yEnd + gradient;

            //second end point
            xEnd = Math.Round(x1, 0, MidpointRounding.AwayFromZero);
            yEnd = y1 + gradient * (xEnd - x1);

            xGap = FractionalPart(x1 + 0.5);

            double xPixel2 = xEnd;
            double yPixel2 = (int)yEnd;

            if (steep)
            {
                PlotPixel(g, (int)yPixel2, (int)xPixel2, InverseFractionalPart(yEnd) * xGap, color);
                PlotPixel(g, (int)yPixel2 + 1, (int)xPixel2, FractionalPart(yEnd) * xGap, color);
            }
            else
            {
                PlotPixel(g, (int)xPixel2, (int)yPixel2, InverseFractionalPart(yEnd) * xGap, color);
                PlotPixel(g, (int)xPixel2, (int)yPixel2 + 1, FractionalPart(yEnd) * xGap, color);
            }

            // main loop
            if (steep)
            {
                for (int x = (int)xPixel1 + 1; x < (int)xPixel2 - 1; x++)
                {
                    PlotPixel(g, (int)intersectionY, x, InverseFractionalPart(intersectionY), color);
                    PlotPixel(g, (int)intersectionY + 1, x, FractionalPart(intersectionY), color);
                    intersectionY += gradient;
                }
            }
            else
            {
                for (int x = (int)xPixel1 + 1; x < (int)xPixel2 - 1; x++)
                {
                    PlotPixel(g, x, (int)intersectionY, InverseFractionalPart(intersectionY), color);
                    PlotPixel(g, x, (int)intersectionY + 1, FractionalPart(intersectionY), color);
                    intersectionY += gradient;
                }
            }
        }

        static double FractionalPart(double x) => x - Math.Floor(x);
        static double InverseFractionalPart(double x) => 1 - FractionalPart(x);

        private static void Swap(ref double a, ref double b)
        {
            (b, a) = (a, b);
        }

        private static void PlotPixel(Graphics g, int x, int y, double brightness, Color pixelColor)
        {
            RGB2HSL(pixelColor, out float hue, out float sat, out float colorBrightness);
            colorBrightness *= (float)brightness;
            colorBrightness = 1 - colorBrightness;
            Color brightnessAdjusted = HSL2RGB(hue, sat, colorBrightness);

            DrawPixel(g, x, y, brightnessAdjusted);
        }

        private static void DrawPixel(Graphics g, int x, int y, Color color)
        {
            g.FillRectangle(new SolidBrush(color), x, y, 1, 1);
        }

        private static void RGB2HSL(Color color, out float hue, out float sat, out float brightness)
        {
            hue = color.GetHue();
            sat = color.GetSaturation();
            brightness = color.GetBrightness();
        }

        //based on https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;

                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Color.FromArgb(Convert.ToByte(r * 255.0f), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f)); ;
        }
    }
}
