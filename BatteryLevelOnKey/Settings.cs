using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BatteryLevelOnKey
{
    class Settings
    {
        public Key HotKey { get; set; }
        public int FontColor { get; set; }
        public int BackgroundColor { get; set; }
        public double Opacity { get; set; }
    }
}
