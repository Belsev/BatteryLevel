using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace BatteryLevelOnKey
{
    class MainFormView
    {
        private readonly MainForm mainForm;
        private readonly BatteryLevelForm batteryLevelForm;
        private Key HotKey { get; set; } = Key.RightAlt;
        private Color FontColor { get; set; }
        private Color BackgroundColor { get; set; }
        private double Opacity { get; set; }
        public MainFormView(MainForm mainForm)
        {
            this.mainForm = mainForm;
            batteryLevelForm = new BatteryLevelForm();
        }

        public void LoadSettings()
        {
            string settingsStr = File.ReadAllText("./Settings.json");
            Settings settings = JsonConvert.DeserializeObject<Settings>(settingsStr);
            HotKey = settings.HotKey;

            FontColor = Color.FromArgb(settings.FontColor);
            BackgroundColor = Color.FromArgb(settings.BackgroundColor);
            Opacity = settings.Opacity;

            SetFontColor();
            SetBackgroundColor();
            SetHotKey();
            SetOpacity();
        }

        public void SaveSettings()
        {
            Settings settings = new Settings();
            settings.HotKey = HotKey;
            settings.FontColor = FontColor.ToArgb();
            settings.BackgroundColor = BackgroundColor.ToArgb();
            settings.Opacity = Opacity;

            string settingsStr = JsonConvert.SerializeObject(settings);
            File.WriteAllText("./Settings.json", settingsStr);
        }

        public async Task CheckKeyboardStateAsync()
        {
            bool keyDown;
            bool prevKeyDown = false;

            while (true)
            {
                keyDown = (Keyboard.GetKeyStates(HotKey) & KeyStates.Down) > 0;

                if (keyDown && !prevKeyDown)
                {
                    SetCurrentBatteryLevel();
                    batteryLevelForm.Show();
                }
                if (!keyDown && prevKeyDown)
                {
                    batteryLevelForm.Hide();
                }
                prevKeyDown = keyDown;
                await Task.Delay(100);
            }
        }

        private void SetCurrentBatteryLevel()
        {
            var batteryLevel = Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifePercent * 100);
            batteryLevelForm.SetBatteryLevel($"{batteryLevel}%");
        }

        internal void ChangeOpacity(int value)
        {
            this.Opacity = Convert.ToDouble(value) / 100;
            SetOpacity();
        }

        private void SetFontColor()
        {
            batteryLevelForm.SetFontColor(FontColor);
            mainForm.SetFontColor(FontColor);
        }

        private void SetBackgroundColor()
        {
            batteryLevelForm.SetBackgroundColor(BackgroundColor);
            mainForm.SetBackgroundColor(BackgroundColor);
        }

        private void SetHotKey()
        {
            mainForm.SetTextBoxText(HotKey.ToString());
        }

        private void SetOpacity()
        {
            batteryLevelForm.SetOpacity(Opacity);
            mainForm.SetOpacity(Opacity);
            SaveSettings();
        }

        public void ChangeFontColor()
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                FontColor = colorDialog.Color;
                SetFontColor();
                SaveSettings();
            }
        }

        public void ChangeBackgroundColor()
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                BackgroundColor = colorDialog.Color;
                SetBackgroundColor();
                SaveSettings();
            }
        }

        public void ChangeHotKey(Key key)
        {
            HotKey = key;
            SetHotKey();
            SaveSettings();
        }
    }
}
