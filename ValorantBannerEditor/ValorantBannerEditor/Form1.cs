using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ValorantBannerEditor.Helpers;

namespace ValorantBannerEditor
{
    public partial class Form1 : Form
    {
        public static Scene Scene;
        public static PipOverlayData pData;
        private const int biCoef = 30;


        public Form1()
        {
            InitializeComponent();
            Scene = new Scene()
            {
                Background = null,
                Layers = new LayerCollection()
            };
      
            Scene.Layers.Add("ring", new Layer() { Enabled=true, });
            Scene.Layers.Add("emblem", new Layer() { Enabled = true, });
            Scene.Layers.Add("pipm", new Layer() { Enabled=false });

            pData = new PipOverlayData()
            {
                oPen = new Pen(Color.Red, 2.5f),
                Radius = 60
            };

            canvas1.FrameRefreshRate = 50;
            canvas1.DrawNewFrame += DrawFrame;
        }
        public void DrawFrame(ref Graphics gfx)
        {
            if (checkBox1.Checked)
                gfx.DrawImage(Scene.Background, 0, 0);

            foreach (var layer in Scene.Layers)
            {
                if (!layer.Value.Enabled)
                    continue;

                if (layer.Value.Background != null)
                    if (layer.Value.Angle != 0)
                        gfx.DrawImage(RotateBitmap(layer.Value.Background,(float)layer.Value.Angle), layer.Value.Location == null ? new PointF(0, 0) : layer.Value.Location);
                    else
                        gfx.DrawImage(layer.Value.Background, layer.Value.Location == null ? new PointF(0, 0) : layer.Value.Location);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = GetImage(textBox1.Text);
            Bitmap bmp = (File.Exists(path) ? Image.FromFile(path) as Bitmap : null), bmp1 = null;

            if (bmp == null)
                return;

            bmp1 = new Bitmap(bmp.Width + biCoef, bmp.Height + biCoef);
            using (Graphics gfx = Graphics.FromImage(bmp1))
                gfx.DrawImage(bmp, biCoef/2, biCoef/2);

            textBox1.Text = path;
            Scene.Background = bmp1;

            canvas1.Size = Scene.Background.Size;
            canvas1.Location = new Point((panel1.Width/2)-(canvas1.Size.Width/2), (panel1.Size.Height/2)-(canvas1.Size.Height/2));
            canvas1.Start();

            Scene.Layers["pipm"].Location = new Point((Scene.Background.Width / 2) - pData.Radius, (Scene.Background.Height / 2) - pData.Radius);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = GetImage(textBox1.Text);
            var bmp = (File.Exists(path) ? Image.FromFile(path) as Bitmap : null);
            textBox2.Text = path;
            Scene.Layers["emblem"].Background = bmp;
            if (bmp != null)
                button4.Enabled = true;

            if (Scene.Layers["emblem"].Background != null)
            {
                Scene.Layers["emblem"].Location.X = (Form1.Scene.Background.Width / 2) - (Form1.Scene.Layers["emblem"].Background.Width / 2);
                Scene.Layers["emblem"].Location.Y = (Form1.Scene.Background.Height / 2) - (Form1.Scene.Layers["emblem"].Background.Height / 2);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Scene.Layers["emblem"].Enabled = checkBox2.Checked;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var path = GetImage(textBox3.Text);
            textBox3.Text = path;
            panel3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new LayerProperties("emblem")).Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new LayerProperties("pip", true)).Show();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Scene.Layers["pipm"].Enabled = checkBox4.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LayerProperties.RefreshPipOverlay();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(canvas1.Frame.Size.Width, canvas1.Frame.Size.Height);
                var gfx = Graphics.FromImage(bmp);
                DrawFrame(ref gfx);
                gfx.Dispose();
                bmp.Save(saveFileDialog1.FileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Scene.Layers["ring"].Enabled = checkBox5.Checked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            (new LayerProperties("ring", false)).Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var path = GetImage(textBox4.Text);
            var bmp = (File.Exists(path) ? Image.FromFile(path) as Bitmap : null);
            textBox4.Text = path;
            Scene.Layers["ring"].Background = bmp;
            if (bmp != null)
                button7.Enabled = true;

            if (Scene.Layers["ring"].Background != null)
            {
                Scene.Layers["ring"].Location.X = (Form1.Scene.Background.Width / 2) - (Form1.Scene.Layers["ring"].Background.Width / 2);
                Scene.Layers["ring"].Location.Y = (Form1.Scene.Background.Height / 2) - (Form1.Scene.Layers["ring"].Background.Height / 2);
            }
        }

        bool DontTriggerCheckState = false;
        private void button5_Click_1(object sender, EventArgs e)
        {
            string pipName = $"pip{checkedListBox1.Items.Count + 1}";
            DontTriggerCheckState = true;
            checkedListBox1.SetItemChecked(checkedListBox1.Items.Add(pipName), true);
            DontTriggerCheckState = false;
            var bmp = File.Exists(textBox3.Text) ? Image.FromFile(textBox3.Text) : null;
            if (bmp != null)
                Scene.Layers.Add(pipName, new Layer() { Enabled = true, Background = bmp as Bitmap });
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count == 0)
                return;

            var selected = checkedListBox1.SelectedItems[0];
            checkedListBox1.Items.Remove(selected);
            if (Scene.Layers.ContainsKey(selected.ToString()))
                Scene.Layers.Remove(selected.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItems.Count == 0)
                return;

            (new LayerProperties(checkedListBox1.SelectedItems[0].ToString(), true)).Show();
        }

        private void ItemCheckChanged(object sender, ItemCheckEventArgs e)
        {
            string t = checkedListBox1.Items[e.Index].ToString();
            if (!DontTriggerCheckState && Scene.Layers.ContainsKey(t))
            Scene.Layers[t].Enabled = e.NewValue == CheckState.Checked;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddPips(int count)
        {
            var bmp = File.Exists(textBox3.Text) ? Image.FromFile(textBox3.Text) : null;
            if (bmp != null)
            {
                checkedListBox1.Items.Clear();
                RemoveKeysMatching(Scene.Layers, new Regex(@"^\w+\d+$"));
                var array = pip2d[count-1];
                for (int i = 0; i < array.Length; ++i)
                {
                    if (array[i] == 0.1)
                        break;

                    string pipName = $"pip{checkedListBox1.Items.Count + 1}";
                    DontTriggerCheckState = true;
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.Add(pipName), true);
                    DontTriggerCheckState = false;
                    Scene.Layers.Add(pipName, new Layer() { Enabled = true, Background = bmp as Bitmap });

                    LayerProperties.UpdatePip(pipName, array[i]);
                }
            }
        }

        double[][] pip2d = new double[][]
            {
                new double[] { -90 , 0.1, 0.1, 0.1, 0.1, 0.1},
                new double[] { -180, 0, 0.1 , 0.1 , 0.1 , 0.1},
                new double[] { -90, -220, 40 , 0.1 , 0.1 , 0.1},
                new double[] { 0, -90, -180, 90, 0.1 , 0.1},
                new double[] { -20, -90, -160, 130 , 40 , 0.1},
                new double[] { -90, -25, -155, 90, 25, -206}
            };


        private void button11_Click(object sender, EventArgs e)
        {
            AddPips(1);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            AddPips(2);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AddPips(3);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            AddPips(5);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            AddPips(4);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            AddPips(6);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            RemoveKeysMatching(Scene.Layers, new Regex(@"^\w+\d+$"));
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            for (int i = 1; i <= 6; ++i)
            {
                AddPips(i);
                var bmp = new Bitmap(canvas1.Frame.Size.Width, canvas1.Frame.Size.Height);
                var gfx = Graphics.FromImage(bmp);
                DrawFrame(ref gfx);
                gfx.Dispose();
                bmp.Save(Path.GetFullPath(saveFileDialog1.FileName).Replace(Path.GetFileName(saveFileDialog1.FileName), string.Empty) + $"{textBox5.Text}{i}.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button19, new Point(button19.Width / 2, button19.Height / 2));
        }
    }
}
