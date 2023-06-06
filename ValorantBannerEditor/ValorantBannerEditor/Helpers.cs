using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValorantBannerEditor
{
    public static class Helpers
    {
        public static void RemoveKeysMatching<TKey, TValue>(Dictionary<TKey, TValue> dictionary, Regex rgx)
        {
            List<TKey> keysToRemove = new List<TKey>();

            foreach (var key in dictionary.Keys)
                if (rgx.IsMatch(key.ToString()))
                    keysToRemove.Add(key);

            foreach (var key in keysToRemove)
                dictionary.Remove(key);
        }

        public static string GetImage(string path = "", string filter = "Image Files (*.jpg, *.png)|*.jpg;*.png")
        {
            var ofd = new OpenFileDialog()
            {
                Filter = filter,
                Title = "Select an image",
                FileName = ""
            };

            if (path != "")
                ofd.InitialDirectory = path;

            return (ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null);
        }
        public static Bitmap RotateBitmap(Bitmap originalBitmap, float angleDegrees)
        {
            Bitmap rotatedBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);
            using (Graphics graphics = Graphics.FromImage(rotatedBitmap))
            {
                Matrix mx = graphics.Transform;
                graphics.TranslateTransform(originalBitmap.Width / 2, originalBitmap.Height / 2);
                graphics.RotateTransform(angleDegrees);
                graphics.TranslateTransform(-originalBitmap.Width / 2, -originalBitmap.Height / 2);
                graphics.DrawImage(originalBitmap, new Point(0, 0));
                graphics.Transform = mx;
            }

            return rotatedBitmap;
        }
        public static Bitmap ResizeBitmap(Bitmap originalBitmap, int size) 
        {
            int w  = (originalBitmap.Width + size), h = (originalBitmap.Height + size);
            if (w > 1 && h > 1)
                return new Bitmap(originalBitmap, new Size(w, h));
            else
                return originalBitmap;
        }
        
}
    public class LayerCollection : Dictionary<string,Layer> 
    { 
    
    }
    public class Scene
    {
        public Bitmap Background;
        public LayerCollection Layers;
    }
    public class Layer
    {
        public Bitmap Background;
        public float Angle;
        public PointF Location;
        public bool Enabled;
        public float PipAngle;
    }
    public class PipOverlayData
    {
        public Pen oPen;
        public int Radius;
    }
}
