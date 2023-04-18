using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    internal class StationName
    {
        public string En { get; set; }
        public string Cn { get; set; }
        public override string ToString()
        {
            return $"{Cn}({En})";
        }
    }
}
