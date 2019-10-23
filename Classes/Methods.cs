using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace WebApplication6.Classes
{
    public static class Methods
    {
        public static Image CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static bool IsBlank(System.Drawing.Image imageFileName)
        {
            double stdDev = GetStdDev(imageFileName);
            return stdDev < 100000;
        }

        /// <summary>
        /// Get the standard deviation of pixel values.
        /// </summary>
        /// <param name="imageFileName">Name of the image file.</param>
        /// <returns>Standard deviation.</returns>
        public static double GetStdDev(System.Drawing.Image imageFileName)
        {
            double total = 0, totalVariance = 0;
            int count = 0;
            double stdDev = 0;

            // First get all the bytes
            using (Bitmap b = new Bitmap(imageFileName))
            {
                BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);
                int stride = bmData.Stride;
                IntPtr Scan0 = bmData.Scan0;
                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - b.Width * 3;
                    for (int y = 0; y < b.Height; ++y)
                    {
                        for (int x = 0; x < b.Width; ++x)
                        {
                            count++;

                            byte blue = p[0];
                            byte green = p[1];
                            byte red = p[2];

                            int pixelValue = Color.FromArgb(0, red, green, blue).ToArgb();
                            total += pixelValue;
                            double avg = total / count;
                            totalVariance += Math.Pow(pixelValue - avg, 2);
                            stdDev = Math.Sqrt(totalVariance / count);

                            p += 3;
                        }
                        p += nOffset;
                    }
                }

                b.UnlockBits(bmData);
            }

            return stdDev;
        }
    }
}