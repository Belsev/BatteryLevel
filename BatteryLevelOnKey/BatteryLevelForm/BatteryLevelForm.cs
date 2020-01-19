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

        public void SetBatteryLevel(string bat)
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

        private void BatteryLevelForm_Load(object sender, System.EventArgs e)
        {
            this.Width = 86;
            this.Height = 37;
            //Rectangle workingArea = Screen.GetWorkingArea(this);
            Rectangle workingArea = Screen.GetBounds(this);
            this.Top = workingArea.Height - this.Height;
            this.Left = workingArea.Width - this.Width;
        }
    }
}
