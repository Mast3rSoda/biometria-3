using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace biometria_3
{
    public static class Algorithm
    {
        public static Bitmap Bernsen(Bitmap bmp, int range, int limit)
        {
            byte[,] data = ImageTo2DByteArray(bmp);
            byte[,] data2 = new byte[bmp.Height, bmp.Width];
            byte[,] data3 = new byte[bmp.Height, bmp.Width];

            for (int y = 0; y < bmp.Height; ++y)
                for (int x = 0; x < bmp.Width; ++x)
                {
                    int min = 255, max = 0;
                    for (int z = y - range; z <= y + range; ++z)
                    {
                        if (z >= 0 && z < bmp.Height)
                            for (int i = x - range; i <= x + range; ++i)
                            {
                                if (i >= 0 && i < bmp.Width)
                                {
                                    if(data[z,i] > max)
                                        max = data[z,i];
                                    if(data[z,i] < min)
                                        min = data[z,i];
                                }
                            }
                    }
                    data2[y, x] = (byte)((max + min) / 2);
                    //liczymy contrast measure, ale na chuj???
                    data3[y, x] = (byte)((max - min));
                }






            //maybe will work on that later as it's not working
            //unsafe
            //{
            //    BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            //    int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            //    int heightInPixels = bitmapData.Height;
            //    int widthInBytes = bitmapData.Width * bytesPerPixel;
            //    byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

            //    Parallel.For(0, heightInPixels, y =>
            //    {
            //        byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
            //        for (int x = 0; x < widthInBytes; x += bytesPerPixel)
            //        {
            //            currentLine[x] = 
            //            currentLine[x + 1] = 
            //            currentLine[x + 2] = data[y,x];
            //        }
            //    });
            //    bmp.UnlockBits(bitmapData);
            //}

            //no idea how to use it
            for (int y = 0; y < bmp.Height; ++y)
                for (int x = 0; x < bmp.Width; ++x)
                {
                    if(data3[y,x] < limit)
                        data2[y, x] = 0;
                    else
                        data2[y, x] = 255;
                    bmp.SetPixel(x, y, Color.FromArgb(data2[y, x], data2[y, x], data2[y, x]));
                }
            return bmp;
        }

        public static byte[,] ImageTo2DByteArray(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            byte[] bytes = new byte[height * data.Stride];
            try
            {
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            byte[,] result = new byte[height, width];
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    int offset = y * data.Stride + x * 3;
                    result[y, x] = (byte)((bytes[offset + 0] + bytes[offset + 1] + bytes[offset + 2]) / 3);
                }
            return result;
        }
    }
}
