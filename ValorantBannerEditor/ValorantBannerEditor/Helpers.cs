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
        public static Bitmap RotateBitmap(Bitmap image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI / 2.0;
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;
            
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;

            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight;

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                Point[] points;
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] {
                                             new Point( (int) oppositeBottom, 0 ),
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( 0, (int) adjacentBottom )
                                         };

                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] {
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( (int) adjacentTop, nHeight ),
                                             new Point( (int) oppositeBottom, 0 )
                                         };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] {
                                             new Point( (int) adjacentTop, nHeight ),
                                             new Point( 0, (int) adjacentBottom ),
                                             new Point( nWidth, (int) oppositeTop )
                                         };
                }
                else
                {
                    points = new Point[] {
                                             new Point( 0, (int) adjacentBottom ),
                                             new Point( (int) oppositeBottom, 0 ),
                                             new Point( (int) adjacentTop, nHeight )
                                         };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
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
