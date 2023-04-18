using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    internal class Metro
    {
        public static MetroInfomation MetroInfo { get; set; }

        public async static Task<MetroResult> Get(string line, string station, ApiLanguage language=ApiLanguage.English)
        {
            string lang = "EN";
            switch (language)
            {
                case ApiLanguage.English:
                    lang = "EN";
                    break;
                case ApiLanguage.TraditionalChinese:
                case ApiLanguage.SimplifiedChinese:
                    lang = "TC";
                    break;
            }
            // 200 Success
            // 429 Too Many Requests
            // 500 Internal Server Error
            HttpClient client = new HttpClient();
            HttpResponseMessage result = await client.GetAsync($"https://rt.data.gov.hk/v1/transport/mtr/getSchedule.php?line={line}&sta={station}&lang={lang}");
            string content = await result.Content.ReadAsStringAsync();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
            return JsonSerializer.Deserialize<MetroResult>(content, options);
        }
    }
}
