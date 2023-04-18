using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    public class MetroUpDown
    {
        // Dummy
        public object ttnt { get; set; }
        // Dummy
        public object valid { get; set; }
        // Platform numbers for the departure / arrival train
        public string plat { get; set; }
        // Estimated arrival time (or departure time) of the train
        public DateTime time { get; set; }
        // Dummy
        public object source { get; set; }
        // Strings, 3 characters 
        public string dest { get; set; }
        // The sequence of the 4 upcoming trains.
        // Value: 1,2,3,4
        public string seq { get; set; }
        // optional Special field apply for EAL only.
        // Indicate the train is arrive or depart from the station.
        // "A" = Arrival
        // "D" = Departure
        public string timetype { get; set; }
        // Special field apply for EAL only
        // Indicate the train to destination via Racecourse station instead of Fo Tan station.
        // "" = Normal
        // "RAC" = Via Racecourse station
        public string route { get; set; }
    }
    public class MetroData
    {
        // Current date and time
        public DateTime curr_time { get; set; }
        // System date and time
        public DateTime sys_time { get; set; }
        public List<MetroUpDown> UP { get; set; }
        public List<MetroUpDown> DOWN { get; set; }
    }
    public class MetroResult
    {
        // System date and time
        public DateTime sys_time { get; set; }
        // Current date and time
        public DateTime curr_time { get; set; }
        // "1" = normal
        // "0" = error or alert
        public byte status { get; set; }
        // Alert message
        public string message { get; set; }
        // "Y" = train is delayed
        // "N" = train is running on time
        public string isdelay { get; set; }
        // optional URL for Special Train Services Arrangement case.
        public string url { get; set; }
        public Dictionary<string, MetroData> data { get; set; }
    }
}
