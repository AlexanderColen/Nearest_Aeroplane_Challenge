using Nearest_Aeroplane_Challenge.Models;
using System.Collections.Generic;

namespace Nearest_Aeroplane_Challenge
{
    public class StatesResponse
    {
        public long Time { get; set; }
        public List<object> States { get; set; }
        public List<State> ConvertedStates { get; set; }
    }
}