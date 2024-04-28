using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection.Metadata.Ecma335;

namespace LineSegmentDrawer
{
    public class Line(double x0, double y0, double x1, double y1, Color color)
    {
        private double x0 = x0,
            y0 = y0,
            x1 = x1,
            y1 = y1;
        private readonly Color foreColor = color;

        //based on Xiaolin Wu's line algorithm
        public void DrawTo(Bitmap bitmap)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
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
                PlotPixel(bitmap, yPixel1, xPixel1, InverseFractionalPart(yEnd) * xGap);
                PlotPixel(bitmap, yPixel1 + 1, xPixel1, FractionalPart(yEnd) * xGap);
            }
            else
            {
                PlotPixel(bitmap, xPixel1, yPixel1, InverseFractionalPart(yEnd) * xGap);
                PlotPixel(bitmap, xPixel1, yPixel1 + 1, FractionalPart(yEnd) * xGap);
            }
            double yIntersection = yEnd + gradient;

            xEnd = Round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = FractionalPart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = IntegerPart(yEnd);
            if (steep)
            {
                PlotPixel(bitmap, yPixel2, xPixel2, InverseFractionalPart(yEnd) * xGap);
                PlotPixel(bitmap, yPixel2 + 1, xPixel2, FractionalPart(yEnd) * xGap);
            }
            else
            {
                PlotPixel(bitmap, xPixel2, yPixel2, InverseFractionalPart(yEnd) * xGap);
                PlotPixel(bitmap, xPixel2, yPixel2 + 1, FractionalPart(yEnd) * xGap);
            }

            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    PlotPixel(
                        bitmap,
                        IntegerPart(yIntersection),
                        x,
                        InverseFractionalPart(yIntersection)
                    );
                    PlotPixel(
                        bitmap,
                        IntegerPart(yIntersection) + 1,
                        x,
                        FractionalPart(yIntersection)
                    );
                    yIntersection += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    PlotPixel(
                        bitmap,
                        x,
                        IntegerPart(yIntersection),
                        InverseFractionalPart(yIntersection)
                    );
                    PlotPixel(
                        bitmap,
                        x,
                        IntegerPart(yIntersection) + 1,
                        FractionalPart(yIntersection)
                    );
                    yIntersection += gradient;
                }
            }
        }

        private void PlotPixel(Bitmap bitmap, double x, double y, double c)
        {
            int xCoordinate = (int)x;
            int yCoordinate = (int)y;

            if (!WithinBounds(xCoordinate, yCoordinate, bitmap.Width, bitmap.Height))
            {
                return;
            }

            int alpha = Math.Clamp((int)(c * 255), 0, 255);

            Color linePixel = Color.FromArgb(alpha, foreColor);
            Color background = bitmap.GetPixel(xCoordinate, yCoordinate);

            Color blended = BlendColors(linePixel, background);

            bitmap.SetPixel(xCoordinate, yCoordinate, blended);
        }

        // from https://en.wikipedia.org/wiki/Alpha_compositing
        private static Color BlendColors(Color foreground, Color background)
        {
            float foregroundAlpha = foreground.A / 255f;
            float backgroundAlpha = background.A / 255f;

            float blendAlpha = foregroundAlpha + backgroundAlpha * (1 - foregroundAlpha);

            int blendRed = (int)(
                (
                    foreground.R * foregroundAlpha
                    + background.R * backgroundAlpha * (1 - foregroundAlpha)
                ) / blendAlpha
            );
            int blendGreen = (int)(
                (
                    foreground.G * foregroundAlpha
                    + background.G * backgroundAlpha * (1 - foregroundAlpha)
                ) / blendAlpha
            );
            int blendBlue = (int)(
                (
                    foreground.B * foregroundAlpha
                    + background.B * backgroundAlpha * (1 - foregroundAlpha)
                ) / blendAlpha
            );

            return Color.FromArgb((int)(blendAlpha * 255f), blendRed, blendGreen, blendBlue);
        }

        private static bool WithinBounds(int x, int y, int width, int height)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        private static int IntegerPart(double x)
        {
            return (int)x;
        }

        private static int Round(double x)
        {
            return (int)Math.Round(x, 0, MidpointRounding.AwayFromZero);
        }

        private static double FractionalPart(double x)
        {
            return Math.Abs(x - Math.Floor(x));
        }

        private static double InverseFractionalPart(double x)
        {
            return 1 - FractionalPart(x);
        }
    }
}
