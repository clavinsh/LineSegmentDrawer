using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LineSegmentDrawer.JsonTextSpecification
{
    public class ImageSpecification
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public required SimpleColor BackgroundColor { get; set; }
        public required LineSpecification[] Lines { get; set; }
    }
}
