using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CalculateDuration
{
    public class VideoInfo
    {
        [JsonPropertyName("cid")]
        public int Cid { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

    }
}
