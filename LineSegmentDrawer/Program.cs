using LineSegmentDrawer;
using System.Drawing;
using System.Drawing.Imaging;

Color lineColor = Color.White;
Point start = new Point(0, 0);
Point end = new Point(100, 100);

Line line = new Line(lineColor, start, end);

Console.WriteLine("Creating the image based on the supplied line parameters...");

var img = line.CreateImage(200,200);
img.Save("output.png", ImageFormat.Png);

Console.WriteLine("Image created and saved!");
