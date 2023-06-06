using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValorantBannerEditor
{
    public partial class BannerResizer : Form
    {
        List<Image> images = new List<Image>();
        public BannerResizer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sfd = new OpenFileDialog()
            {
                Title = "Banner secin"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var array = sfd.FileNames.Select(x => Path.GetFileName(x)).ToList();
                textBox1.Text = string.Join(", ", array);
                for (int i = 0; i < array.Count; i++)
                    listBox1.SetSelected(listBox1.Items.Add(array[i]), true);
                foreach (var path in sfd.FileNames)
                    images.Add(Image.FromFile(path));
            }
            
        }
    }
}
