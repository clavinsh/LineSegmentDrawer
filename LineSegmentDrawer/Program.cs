using System.Drawing;
using System.Drawing.Imaging;
using LineSegmentDrawer;

Color lineColor = Color.White;
Color backgroundColor = Color.DarkSlateBlue;

Point start = new Point(173, 10);
Point end = new Point(10, 50);

//Line line = new Line(lineColor, backgroundColor, start.X, start.Y, end.X, end.Y);


Line line = new Line(start.X, start.Y, end.X, end.Y, lineColor);

Console.WriteLine("Creating the image based on the supplied line parameters...");

var image = new Bitmap(200, 200);
Graphics g = Graphics.FromImage(image);

g.Clear(backgroundColor);

line.DrawTo(image);
image.Save("output.png", ImageFormat.Png);

Console.WriteLine("Image created and saved!");
