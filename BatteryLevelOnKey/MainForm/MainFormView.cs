﻿using Microsoft.Win32;
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
        private readonly string startupRegistryKeyName = "Battery Level";
        private readonly string startupRegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private Key _hotKey;
        private Color _fontColor;
        private Color _backgroundColor;
        private double _opacity;
        private OverlayPositionEnum _overlayPosition;
        private bool _startupEnabled;
        private bool _showTimeEnabled;
        private decimal _fontSize;

        public Key HotKey
        {
            get { return _hotKey; }
            set
            {
                if (_hotKey != value)
                {
                    _hotKey = value;
                    mainForm.SetTextBoxText(HotKey.ToString());
                    SaveSettings();
                }
            }
        }
        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                if (_fontColor != value)
                {
                    _fontColor = value;
                    batteryLevelForm.SetFontColor(FontColor);
                    mainForm.SetFontColor(FontColor);
                    SaveSettings();
                }
            }
        }
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    batteryLevelForm.SetBackgroundColor(BackgroundColor);
                    mainForm.SetBackgroundColor(BackgroundColor);
                    SaveSettings();
                }
            }
        }
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (_opacity != value)
                {
                    _opacity = value;
                    batteryLevelForm.SetOpacity(Opacity);
                    mainForm.SetOpacity(Opacity);
                    SaveSettings();
                }
            }
        }
        public OverlayPositionEnum OverlayPosition
        {
            get { return _overlayPosition; }
            set
            {
                if (_overlayPosition != value)
                {
                    _overlayPosition = value;
                    batteryLevelForm.SetOverlayPosition(OverlayPosition);
                    mainForm.SetOverlayPosition(OverlayPosition);
                    SaveSettings();
                }
            }
        }
        public bool StartupEnabled
        {
            get { return _startupEnabled; }
            set
            {
                if (_startupEnabled != value)
                {
                    _startupEnabled = value;
                    mainForm.SetStartupBtnText(_startupEnabled ? "Remove" : "Add");
                }
            }
        }
        public bool ShowTimeEnabled
        {
            get { return _showTimeEnabled; }
            set
            {
                if (_showTimeEnabled != value)
                {
                    _showTimeEnabled = value;
                    mainForm.ToggleShowTime(_showTimeEnabled);
                    SaveSettings();
                }
            }
        }
        public decimal FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    mainForm.SetFontSize(_fontSize);
                    batteryLevelForm.SetFontSize(_fontSize);
                    SaveSettings();
                }
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
            string settingsStr = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json"));
            Settings settings = JsonConvert.DeserializeObject<Settings>(settingsStr);

            HotKey = settings.HotKey;
            FontColor = Color.FromArgb(settings.FontColor);
            BackgroundColor = Color.FromArgb(settings.BackgroundColor);
            Opacity = settings.Opacity;
            OverlayPosition = settings.OverlayPosition;
            ShowTimeEnabled = settings.ShowTimeEnabled;
            FontSize = settings.FontSize;

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
                OverlayPosition = OverlayPosition,
                ShowTimeEnabled = ShowTimeEnabled,
                FontSize = FontSize,
            };

            string settingsStr = JsonConvert.SerializeObject(settings);
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json"), settingsStr);
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
                    batteryLevelForm.SetOverlayPosition(OverlayPosition);
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
            batteryLevelForm.SetText(ShowTimeEnabled ? $"{batteryLevel}% {DateTime.Now:HH:mm}" : $"{batteryLevel}%");
        }

        public void ToggleStartup()
        {
            if (StartupValueExists())
            {
                RemoveFromStartup();
                StartupEnabled = false;
            }
            else
            {
                AddToStartup();
                StartupEnabled = true;
            }
        }

        public bool StartupValueExists()
        {
            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(startupRegistryKeyPath, false))
            {
                var value = registryKeyStartup.GetValue(startupRegistryKeyName);
                if (value == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void AddToStartup()
        {
            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(startupRegistryKeyPath, true))
            {
                registryKeyStartup.SetValue(startupRegistryKeyName, string.Format("\"{0}\" -silent", System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
        }

        private void RemoveFromStartup()
        {
            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(startupRegistryKeyPath, true))
            {
                registryKeyStartup.DeleteValue(startupRegistryKeyName, false);
            }
        }

        public void ToggleShowTimeEnabled()
        {
            ShowTimeEnabled = !ShowTimeEnabled;
        }

        public void SetShowTimeEnabled(bool value)
        {
            ShowTimeEnabled = value;
        }

        public void SetFontSize(decimal value)
        {
            FontSize = value;
        }
    }
}
