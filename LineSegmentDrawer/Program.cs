using System.Drawing;
using System.Drawing.Imaging;
using LineSegmentDrawer;

Color lineColor = Color.Green;
Color backgroundColor = Color.DarkSlateBlue;
Point start = new Point(10, 10);
Point end = new Point(50, 173);

//Line line = new Line(lineColor, backgroundColor, start.X, start.Y, end.X, end.Y);


Line line = new Line(start.X, start.Y, end.X, end.Y, lineColor);

Console.WriteLine("Creating the image based on the supplied line parameters...");

var image = new Bitmap(200, 200);
Graphics g = Graphics.FromImage(image);

g.Clear(backgroundColor);

line.Draw(image);
image.Save("output.png", ImageFormat.Png);

Console.WriteLine("Image created and saved!");
