using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DigitalSignEditor.Helpers {

    public static class ImageHelper {
        private const int resizedWidth = 800;

        public static bool IsImageValid(byte[] byteArray, int minimumWidth, int minimumHeight, int aspectWidth, int aspectHeight) {
            var image = Image.Identify(byteArray);
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

        public static byte[] Resize(byte[] byteArray) {
            var image = Image.Load(byteArray, out var format);
            var newHeight = resizedWidth * image.Height / image.Width;
            image.Mutate(x => x.Resize(resizedWidth, newHeight));
            using var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }
    }
}