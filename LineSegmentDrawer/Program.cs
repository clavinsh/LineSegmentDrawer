using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using LineSegmentDrawer;
using LineSegmentDrawer.JsonTextSpecification;

const string inputFileName = "input.txt";
const string outputFileName = "output.png";

string currentDirectory = Directory.GetCurrentDirectory();
string inputPath = Path.Combine(currentDirectory, inputFileName);

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine(
        $"No input file {inputFileName} found in the current directory: {currentDirectory}"
    );
    return -1;
}

ImageSpecification? specification;

try
{
    string inputJson = File.ReadAllText(inputPath);
    specification = JsonSerializer.Deserialize(
        inputJson,
        SourceGenerationContext.Default.ImageSpecification
    );
}
catch (JsonException ex)
{
    Console.Error.WriteLine("Error parsing JSON input: " + ex.Message);
    return -1;
}

if (specification == null)
{
    Console.Error.WriteLine("Failed to parse the input or the input was empty.");
    return -1;
}

if (specification.ImageWidth <= 0 || specification.ImageHeight <= 0)
{
    Console.Error.WriteLine("Invalid image dimensions specified in the input.");
    return -1;
}

if (specification.Lines == null || specification.Lines.Length == 0)
{
    Console.Error.WriteLine("No lines specified in the input for drawing.");
    return -1;
}

Console.WriteLine("Input text parsed...");

try
{
    Console.WriteLine("Drawing image...");
    using var image = new Bitmap(specification.ImageWidth, specification.ImageHeight);
    using (Graphics g = Graphics.FromImage(image))
    {
        g.Clear(specification.BackgroundColor.ToColor());
    }

    foreach (var lineSpec in specification.Lines)
    {
        var line = new Line(
            lineSpec.x0,
            lineSpec.y0,
            lineSpec.x1,
            lineSpec.y1,
            lineSpec.Color.ToColor()
        );
        line.DrawTo(image);
    }

    image.Save(outputFileName, ImageFormat.Png);
    Console.WriteLine($"Image created and saved to {Path.GetFullPath(outputFileName)}!");
}
catch (Exception ex)
{
    Console.Error.WriteLine("An error occurred while creating the image: " + ex.Message);
    return -1;
}

return 0;
