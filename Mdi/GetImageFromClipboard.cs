using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GetImageFromClipboard
{
    static class GetImageFromClipboard
    {
        static private string imageSource;
        static public Image ImageFromClipboard{get;private set;}
        static public string ImageSource
        {
            get
            {
                return ImageSource;
            }
            set
            {
                if (!System.IO.File.Exists(value))
                    throw new System.IO.FileNotFoundException("The specified file was not found, specify a new location and try again.");
                else
                imageSource=value;
            }
        }

        public static Image GetImage()
        {
            try
            {
                if (System.Windows.Forms.Clipboard.ContainsImage())
                    ImageFromClipboard = Clipboard.GetImage();
                return ImageFromClipboard;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

    }
}
