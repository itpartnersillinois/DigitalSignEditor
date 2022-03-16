using System;
using System.Drawing;
using System.IO;

namespace DigitalSignEditor.Helpers {

    public static class ImageHelper {

        public static bool IsImageValid(byte[] byteArray, int minimumWidth, int minimumHeight, int aspectWidth, int aspectHeight) {
            var image = Image.FromStream(new MemoryStream(byteArray));
            if (image == null) {
                return false;
            }
            if (image.Width < minimumWidth || image.Height < minimumHeight) {
                return false;
            }
            double actualRatio = image.Width / image.Height;
            double expectedRatio = aspectWidth / aspectHeight;
            return Math.Round(actualRatio, 2) == Math.Round(expectedRatio, 2);
        }

        public static byte[] Resize(byte[] byteArray, int minimumWidth, int minimumHeight) {
            var image = Image.FromStream(new MemoryStream(byteArray));
            var resized = (Image) (new Bitmap(image, new Size(minimumWidth, minimumHeight)));
            using var ms = new MemoryStream();
            resized.Save(ms, resized.RawFormat);
            return ms.ToArray();
        }
    }
}