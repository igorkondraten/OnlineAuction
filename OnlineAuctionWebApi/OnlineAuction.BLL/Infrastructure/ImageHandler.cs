using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OnlineAuction.BLL.Infrastructure
{
    /// <summary>
    /// Class which handles received images.
    /// </summary>
    public static class ImageHandler
    {
        /// <summary>
        /// Writes byte array of image to file and returns saved image path for saving in the DB.
        /// </summary>
        /// <param name="arr">Byte array of image.</param>
        /// <returns>Short path where image is stored.</returns>
        /// <exception cref="ArgumentException">Thrown if image format is invalid.</exception>
        public static string WriteImageToFile(byte[] arr)
        {
            var filename = $"{Guid.NewGuid()}.";
            using (var img = Image.FromStream(new MemoryStream(arr)))
            {
                ImageFormat format;
                if (ImageFormat.Png.Equals(img.RawFormat))
                {
                    filename += "png";
                    format = ImageFormat.Png;
                }
                else if (ImageFormat.Gif.Equals(img.RawFormat))
                {
                    filename += "gif";
                    format = ImageFormat.Gif;
                }
                else if (ImageFormat.Jpeg.Equals(img.RawFormat))
                {
                    filename += "jpg";
                    format = ImageFormat.Jpeg;
                }
                else
                {
                    throw new ArgumentException("Invalid image format.");
                }
                
                var path = AppDomain.CurrentDomain.BaseDirectory + $@"Images\{filename}";
                img.Save(path, format);
            }
            return $"/Images/{filename}";
        }
    }
}
