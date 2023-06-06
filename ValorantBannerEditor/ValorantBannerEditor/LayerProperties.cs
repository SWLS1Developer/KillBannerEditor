using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValorantBannerEditor
{
    public partial class LayerProperties : Form
    {
        string Tag;
        bool ShowPip;

        public LayerProperties(string layerTag, bool ShowPipModif = false)
        {
            InitializeComponent();
            Tag = layerTag;
            ShowPip = ShowPipModif;

            this.Size =  ShowPipModif ? new Size(674, 487) : new Size(674, 331);
            this.groupBox1.Visible = ShowPipModif;
        }
        private void LayerProperties_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Application.OpenForms[0].Location.X + Application.OpenForms[0].Width + 5, Application.OpenForms[0].Location.Y);
            
            flowLayoutPanel1.Controls.Clear();
            foreach (int degree in (new int[] { 0, 45, 60, 90, 120, 180, 210, 270 }))
            {
                var btn = new Button()
                {
                    BackColor = Color.FromArgb(12, 12, 12),
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(68, 28),
                    Text = (degree == 0 ? "0°/360°" : degree.ToString() + "°"),
                    ForeColor = Color.White,
                    Tag = degree
                };
                btn.Click += (_S, _E) =>
                {
                    var deg = (int)((Button)_S).Tag;
                    Form1.Scene.Layers[Tag].Angle = deg;
                    trackBar1.Value = deg;
                };
                flowLayoutPanel1.Controls.Add(btn);
            }

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 360;

            numericUpDown1.Value = (int)Form1.Scene.Layers[Tag].Location.X;
            numericUpDown2.Value = (int)Form1.Scene.Layers[Tag].Location.Y;

            wNumeric.Value = (int)Form1.Scene.Layers[Tag].Background.Width;
            hNumeric.Value = (int)Form1.Scene.Layers[Tag].Background.Height;

            if (ShowPip)
            {
                numericUpDown4.Value = (int)(Form1.Scene.Layers[Tag].Angle - 90);
                numericUpDown7.Value = Form1.pData.Radius;
                numericUpDown5.Value = (decimal)Form1.pData.oPen.Width;
                panel2.BackColor = Form1.pData.oPen.Color;

                numericUpDown6.Value = (int)Form1.Scene.Layers["pipm"].Location.X;
                numericUpDown3.Value = (int)Form1.Scene.Layers["pipm"].Location.Y;
            }

            this.Text = $"Layer properties for '{Tag}'";
        }

        #region Angle
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.label1.Text = $"Angle ({trackBar1.Value}°)";
            Form1.Scene.Layers[Tag].Angle = trackBar1.Value;
        }
        #endregion
        #region Location
        private void RefreshLocationInformations(bool RefreshNumerics = false)
        {
            int x = (int)Form1.Scene.Layers[Tag].Location.X, y = (int)Form1.Scene.Layers[Tag].Location.Y;
            this.label2.Text = $"Location ({x}, {y})";
            if (RefreshNumerics)
            {
                numericUpDown1.Value = x;
                numericUpDown2.Value = y;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Form1.Scene.Layers[Tag].Location.X = (float)numericUpDown1.Value;
            RefreshLocationInformations();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Form1.Scene.Layers[Tag].Location.Y = (float)numericUpDown2.Value;
            RefreshLocationInformations();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Top Left
            Form1.Scene.Layers[Tag].Location.X = 0;
            Form1.Scene.Layers[Tag].Location.Y = 0;
            RefreshLocationInformations(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Top Middle
            Form1.Scene.Layers[Tag].Location.X = (Form1.Scene.Background.Width / 2) - (Form1.Scene.Layers[Tag].Background.Width / 2);
            Form1.Scene.Layers[Tag].Location.Y = 0;
            RefreshLocationInformations(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Top Left
            Form1.Scene.Layers[Tag].Location.X = Form1.Scene.Background.Width - Form1.Scene.Layers[Tag].Background.Width;
            Form1.Scene.Layers[Tag].Location.Y = 0;
            RefreshLocationInformations(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Middle Left
            Form1.Scene.Layers[Tag].Location.X = 0;
            Form1.Scene.Layers[Tag].Location.Y = (Form1.Scene.Background.Height / 2) - (Form1.Scene.Layers[Tag].Background.Height / 2);
            RefreshLocationInformations(true);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Middle Center
            Form1.Scene.Layers[Tag].Location.X = (Form1.Scene.Background.Width / 2) - (Form1.Scene.Layers[Tag].Background.Width / 2);
            Form1.Scene.Layers[Tag].Location.Y = (Form1.Scene.Background.Height / 2) - (Form1.Scene.Layers[Tag].Background.Height / 2);
            RefreshLocationInformations(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Middle Right
            Form1.Scene.Layers[Tag].Location.X = Form1.Scene.Background.Width - Form1.Scene.Layers[Tag].Background.Width;
            Form1.Scene.Layers[Tag].Location.Y = (Form1.Scene.Background.Height / 2) - (Form1.Scene.Layers[Tag].Background.Height / 2);
            RefreshLocationInformations(true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Bottom Left
            Form1.Scene.Layers[Tag].Location.X = 0;
            Form1.Scene.Layers[Tag].Location.Y = Form1.Scene.Background.Height - Form1.Scene.Layers[Tag].Background.Height;
            RefreshLocationInformations(true);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Bottom Middle
            Form1.Scene.Layers[Tag].Location.X = (Form1.Scene.Background.Width / 2) - (Form1.Scene.Layers[Tag].Background.Width / 2);
            Form1.Scene.Layers[Tag].Location.Y = Form1.Scene.Background.Height - Form1.Scene.Layers[Tag].Background.Height;
            RefreshLocationInformations(true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Bottom Right
            Form1.Scene.Layers[Tag].Location.X = Form1.Scene.Background.Width - Form1.Scene.Layers[Tag].Background.Width;
            Form1.Scene.Layers[Tag].Location.Y = Form1.Scene.Background.Height - Form1.Scene.Layers[Tag].Background.Height;
            RefreshLocationInformations(true);
        }
        #endregion
        #region PipModf
        private void panel2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel2.BackColor = colorDialog1.Color;
                Form1.pData.oPen.Color = colorDialog1.Color;
                RefreshPipOverlay();
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            Form1.pData.oPen.Width = (float)numericUpDown5.Value;
            RefreshPipOverlay();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            Form1.pData.Radius = (int)numericUpDown7.Value;
            if (checkBox2.Checked)
                button10_Click(null, null);
            else
                RefreshPipOverlay();

            if (!checkBox1.Checked)
                UpdatePip(Tag, (double)numericUpDown4.Value);
            else
            {
                foreach (string t in (checkBox1.Checked ? Form1.Scene.Layers.Where(t => t.Key.StartsWith("pip")).Select(x => x.Key).ToArray() : new string[] { Tag }))
                    UpdatePip(t, Form1.Scene.Layers[t].Angle - 90);
            }
        }
        public static void RefreshPipOverlay()
        {
            var bmp = new Bitmap(Form1.pData.Radius * 2 + 1, Form1.pData.Radius * 2 + 1);
            var gfx = Graphics.FromImage(bmp);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.DrawEllipse(Form1.pData.oPen, 0, 0, Form1.pData.Radius * 2, Form1.pData.Radius * 2);

            Form1.Scene.Layers["pipm"].Background = bmp;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            Form1.Scene.Layers["pipm"].Location.X = (float)numericUpDown6.Value;
            RefreshPipOverlay();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            Form1.Scene.Layers["pipm"].Location.Y = (float)numericUpDown3.Value;
            RefreshPipOverlay();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            numericUpDown6.Value = (Form1.Scene.Background.Width / 2) - Form1.pData.Radius;
            numericUpDown3.Value = (Form1.Scene.Background.Height / 2) - Form1.pData.Radius;

            RefreshPipOverlay();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            UpdatePip(Tag, (double)numericUpDown4.Value);  
        }

        public static void UpdatePip(string t, double a)
        {
                if (Form1.Scene.Layers[t].Background == null || Form1.Scene.Layers[t].Location == null)
                    return;

                double rad = (Math.PI / 180) * (a);
                double cx = Form1.Scene.Layers["pipm"].Location.X + Form1.pData.Radius,
                       cy = Form1.Scene.Layers["pipm"].Location.Y + Form1.pData.Radius;

                Form1.Scene.Layers[t].Location.X = (float)(cx + (Form1.pData.Radius * Math.Cos(rad)) - Form1.Scene.Layers[t].Background.Width / 2);
                Form1.Scene.Layers[t].Location.Y = (float)(cy + (Form1.pData.Radius * Math.Sin(rad)) - Form1.Scene.Layers[t].Background.Height / 2);
                Form1.Scene.Layers[t].Angle = (int)(a) + 90;
        }
        #endregion

        private void button11_Click(object sender, EventArgs e)
        {
            Form1.Scene.Layers[Tag].Background = new Bitmap(Form1.Scene.Layers[Tag].Background, new Size((int)wNumeric.Value, (int)hNumeric.Value));
        }
    }
}
