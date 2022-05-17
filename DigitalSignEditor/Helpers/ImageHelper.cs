using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DigitalSignEditor.Helpers {

    public static class ImageHelper {
        private const int resizedWidth = 800;

        public static string IsImageValid(byte[] byteArray, int size, int minimumWidth, int minimumHeight, int aspectWidth, int aspectHeight) {
            if (size > 0 && byteArray.Length / 1024 > size) {
                return "Error: Image is larger than maximum size";
            }
            var image = Image.Identify(byteArray);
            if (image == null) {
                return "Error: Image cannot be identified";
            }
            if (image.Width < minimumWidth) {
                return "Error: Image does not meet minimum width";
            }
            if (image.Height < minimumHeight) {
                return "Error: Image does not meet minimum height";
            }
            return GetRatio(image.Width, image.Height) == GetRatio(aspectWidth, aspectHeight) ? "" : "Error: Image is not correct dimensions";
        }

        public static byte[] Resize(byte[] byteArray) {
            var image = Image.Load(byteArray, out var format);
            var newHeight = resizedWidth * image.Height / image.Width;
            image.Mutate(x => x.Resize(resizedWidth, newHeight));
            using var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }

        private static decimal GetRatio(int width, int height) => Math.Round(decimal.Divide(width, height), 2);
    }
}