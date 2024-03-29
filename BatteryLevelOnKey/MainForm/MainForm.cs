﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BatteryLevelOnKey
{
    public partial class MainForm : Form
    {
        private readonly MainFormView mainFormView;

        public MainForm()
        {
            mainFormView = new MainFormView(this);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void Start()
        {
            var PositionList = new List<OverlayPositionEnum>() {
                OverlayPositionEnum.TopLeft,
                OverlayPositionEnum.TopRight,
                OverlayPositionEnum.BottomRight,
                OverlayPositionEnum.BottomLeft
            };
            comboBox1.DataSource = PositionList;

            mainFormView.LoadSettings();
            mainFormView.StartupEnabled = mainFormView.StartupValueExists();
            _ = mainFormView.CheckKeyboardStateAsync();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void closeForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showForm_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            showForm_Click(sender, e);
        }

        public void SetFontColor(Color color)
        {
            this.panel1.BackColor = color;
        }

        public void SetFontSize(decimal size)
        {
            this.numericUpDown1.Value = size;
        }

        public void SetBackgroundColor(Color color)
        {
            this.panel2.BackColor = color;
        }

        public void SetTextBoxText(string text)
        {
            this.textBox1.Text = text;
        }

        public void SetOpacity(double opacity)
        {
            this.opacityTrackBar.Value = Convert.ToInt32(opacity * 100);
        }

        public void SetOverlayPosition(OverlayPositionEnum overlayPosition)
        {
            this.comboBox1.SelectedItem = overlayPosition;
        }

        public void SetStartupBtnText(string text)
        {
            this.ToggleStartupBtn.Text = text;
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                mainFormView.FontColor = colorDialog.Color;
            }
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                mainFormView.BackgroundColor = colorDialog.Color;
            }
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var key = e.KeyValue switch
            {
                // Ctrl
                16 => Keyboard.IsKeyDown(Key.RightShift) ? Key.RightShift : Key.LeftShift,
                // Ctrl
                17 => Keyboard.IsKeyDown(Key.RightCtrl) ? Key.RightCtrl : Key.LeftCtrl,
                // Alt
                18 => Keyboard.IsKeyDown(Key.RightAlt) ? Key.RightAlt : Key.LeftAlt,
                _ => KeyInterop.KeyFromVirtualKey(e.KeyValue)
            };
            mainFormView.HotKey = key;
            e.SuppressKeyPress = true;
        }

        private void opacityTrackBar_Scroll(object sender, EventArgs e)
        {
            mainFormView.Opacity = Convert.ToDouble(this.opacityTrackBar.Value) / 100;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainFormView.OverlayPosition = (OverlayPositionEnum)this.comboBox1.SelectedItem;
        }

        private void ToggleStartupBtn_Click(object sender, EventArgs e)
        {
            mainFormView.ToggleStartup();
        }

        public void ToggleShowTime(bool showTimeEnabled)
        {
            showTimeCheckBox.Checked = showTimeEnabled;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var nud = sender as NumericUpDown;
            mainFormView.SetFontSize(nud.Value);
        }

        private void showTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            mainFormView.SetShowTimeEnabled(cb.Checked);
        }
    }
}
