using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    internal class LightRail
    {
        public static Dictionary<int, StationName> StationIds { get; set; }
        public static async Task<LightRailResult> Get(int stationId)
        {
            // 200 Success
            // 429 Too Many Requests
            // 500 Internal Server Error
            HttpClient client = new HttpClient();
            HttpResponseMessage result = await client.GetAsync($"https://rt.data.gov.hk/v1/transport/mtr/lrt/getSchedule?station_id={stationId}");
            string content = await result.Content.ReadAsStringAsync();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
            return JsonSerializer.Deserialize<LightRailResult>(content, options);
        }
    }
}
