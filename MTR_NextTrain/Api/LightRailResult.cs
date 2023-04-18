using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    public class LightRailRoute
    {
        // The Length of the train
        public int train_length { get; set; }
        // A : time act as remain arrival time for normal station
        // D : time act as remain departure time for terminal station
        public string arrival_departure { get; set; }
        // Destination of the route in English
        public string dest_en { get; set; }
        // Destination of the route in Chinese
        public string dest_ch { get; set; }
        // Arrival time/departure time in English
        // departing: the train is departing from terminus soon
        // arriving: the train is arriving soon
        // "-": the train is arriving / departing
        public string time_en { get; set; }
        // Arrival time/departure time in Chinese
        // "正在離開": the train is departing from terminus soon
        // "即將抵達": the train is arriving soon
        // "-": the train is arriving / departing
        public string time_ch { get; set; }
        // Route number
        public string route_no { get; set; }
        // Indicate the line is stopped
        public int stop { get; set; }

        public override string ToString()
        {
            return $"train_length: {train_length}, arrival_departure: {arrival_departure}, dest_en: {dest_en}, dest_ch: {dest_ch}, " + 
                $"time_en: {time_en}, time_ch: {time_ch}, route_no: {route_no} stop: {stop}";
        }
    }
    public class LightRailPlatform
    {
        // Platform id of each station
        public int platform_id { get; set; }
        public List<LightRailRoute> route_list { get; set; }
        public override string ToString()
        {
            StringBuilder routes = new StringBuilder();
            routes.Append('[');
            foreach (var route in route_list)
            {
                routes.Append(route.ToString());
            }
            routes.Append(']');
            return $"platform_id: {platform_id}, route_list: {routes}";
        }
    }

    public class LightRailResult
    {
        // "1" = normal, "0" = error or alert
        public byte status { get; set; }
        // System date and time
        public DateTime system_time { get; set; }
        // Array of platform list
        public List<LightRailPlatform> platform_list { get; set; }

        public override string ToString()
        {
            StringBuilder platforms = new StringBuilder();
            platforms.Append('[');
            foreach (var platform in platform_list)
            {
                platforms.Append(platform.ToString());
            }
            platforms.Append(']');
            return $"status: {status}, system_time: {system_time}, platform_list: {platforms}";
        }
    }
}
