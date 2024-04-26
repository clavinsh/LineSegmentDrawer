using LineSegmentDrawer;
using System.Drawing;
using System.Drawing.Imaging;

Color lineColor = Color.White;
Color backgroundColor = Color.DarkSlateBlue;
Point start = new Point(10, 10);
Point end = new Point(50, 173);

Line line = new Line(lineColor, backgroundColor, start.X, start.Y, end.X, end.Y);

Console.WriteLine("Creating the image based on the supplied line parameters...");

var img = line.CreateImage(200, 200);
img.Save("output.png", ImageFormat.Png);

Console.WriteLine("Image created and saved!");
