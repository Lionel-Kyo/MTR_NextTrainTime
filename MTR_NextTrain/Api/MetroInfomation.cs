using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    public class MetroInfomation
    {
        /// <summary>
        /// Abbreviation, Full form
        /// </summary>
        public Dictionary<string, string> Lines { get; set; }
        /// <summary>
        /// Abbreviation, Full form
        /// </summary>
        public Dictionary<string, string> Stations { get; set; }
        /// <summary>
        /// Line abbreviation, Station abbreviations
        /// </summary>
        public Dictionary<string, List<string>> LineStations { get; set; }
    }
}
