using System;
using System.Drawing;
using System.Windows.Forms;

namespace BatteryLevelOnKey
{
    public partial class BatteryLevelForm : Form
    {
        public BatteryLevelForm()
        {
            InitializeComponent();
        }

        public void SetText(string bat)
        {
            this.label1.Text = bat;
        }

        public void SetFontColor(Color color)
        {
            this.label1.ForeColor = color;
        }

        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        public void SetOpacity(double opacity)
        {
            this.Opacity = opacity;
        }

        public void SetOverlayPosition(OverlayPositionEnum overlayPosition)
        {
            Rectangle workingArea = Screen.GetBounds(this);
            (Top, Left) = overlayPosition switch
            {
                OverlayPositionEnum.TopLeft => (0, 0),
                OverlayPositionEnum.TopRight => (0, workingArea.Width - Width),
                OverlayPositionEnum.BottomRight => (workingArea.Height - Height, workingArea.Width - Width),
                OverlayPositionEnum.BottomLeft => (workingArea.Height - Height, 0),
                _ => throw new Exception()
            };
        }

        internal void SetFontSize(decimal fontSize)
        {
            this.label1.Font = new Font(this.label1.Font.FontFamily, (float)fontSize);
        }
    }
}
