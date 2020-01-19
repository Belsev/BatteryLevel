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

        private Key _hotKey;
        private Color _fontColor;
        private Color _backgroundColor;
        private double _opacity;
        private OverlayPositionEnum _overlayPosition;

        public Key HotKey
        {
            get { return _hotKey; }
            set
            {
                _hotKey = value;
                mainForm.SetTextBoxText(HotKey.ToString());
                SaveSettings();
            }
        }
        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                batteryLevelForm.SetFontColor(FontColor);
                mainForm.SetFontColor(FontColor);
                SaveSettings();
            }
        }
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                batteryLevelForm.SetBackgroundColor(BackgroundColor);
                mainForm.SetBackgroundColor(BackgroundColor);
                SaveSettings();
            }
        }
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                batteryLevelForm.SetOpacity(Opacity);
                mainForm.SetOpacity(Opacity);
                SaveSettings();
            }
        }
        public OverlayPositionEnum OverlayPosition
        {
            get { return _overlayPosition; }
            set
            {
                _overlayPosition = value;
                batteryLevelForm.SetOverlayPosition(OverlayPosition);
                mainForm.SetOverlayPosition(OverlayPosition);
                SaveSettings();
            }
        }

        private bool SaveEnabled { get; set; } = false;

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
            OverlayPosition = settings.OverlayPosition;

            SaveEnabled = true;
        }

        public void SaveSettings()
        {
            if (!SaveEnabled)
            {
                return;
            }
            Settings settings = new Settings()
            {
                HotKey = HotKey,
                FontColor = FontColor.ToArgb(),
                BackgroundColor = BackgroundColor.ToArgb(),
                Opacity = Opacity,
                OverlayPosition = OverlayPosition
            };

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
    }
}
