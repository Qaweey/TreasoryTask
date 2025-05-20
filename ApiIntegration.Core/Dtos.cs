using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public class Data
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Dictionary allows flexibility with unknown/dynamic fields
        [JsonPropertyName("data")]
        public Dictionary<string, object> Datas { get; set; }
    }

    public class CreateProductRequestDto
    {
        public string Name { get; set; }
        public Dictionary<string, object> Data { get; set; }

    }
    public class GetProductResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Allows any key-value pairs or null
        [JsonPropertyName("data")]
        public Dictionary<string, object>? Data { get; set; }
    }

    public class DeleteProductResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class CreateProductResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
