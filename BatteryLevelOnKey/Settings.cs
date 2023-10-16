using System.Windows.Input;

namespace BatteryLevelOnKey
{
    public class Settings
    {
        public Key HotKey { get; set; }
        public int FontColor { get; set; }
        public int BackgroundColor { get; set; }
        public double Opacity { get; set; }
        public OverlayPositionEnum OverlayPosition { get; set; }
        public bool ShowTimeEnabled {  get; set; }
    }

    public enum OverlayPositionEnum
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }
}
