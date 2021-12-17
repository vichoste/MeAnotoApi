using System.Text.Json.Serialization;

namespace MeAnotoApi.Information
{
    /// <summary>
    /// Institution response
    /// </summary>
    public class InstitutionResponse
    {
        /// <summary>
        /// Institution ID
        /// </summary>
        [JsonPropertyName(JsonPropertyNames.Id)]
        public int Id { get; set; }
        /// <summary>
        /// Institution Name
        /// </summary>
        [JsonPropertyName(JsonPropertyNames.Name)]
        public string Name { get; set; }
    }
}
