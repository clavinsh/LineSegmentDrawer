using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LineSegmentDrawer.JsonTextSpecification;

namespace LineSegmentDrawer
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(ImageSpecification))]
    internal partial class SourceGenerationContext : JsonSerializerContext { }
}
