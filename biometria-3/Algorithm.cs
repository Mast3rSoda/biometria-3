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
            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    int min = (int)allValues.Min();
                    int max = (int)allValues.Max();
                    int mean = (max + min) / 2;
                    int contrast = max - min;
                    var current = (int)grayImage[i, j];
                    if (contrast < limit)
                    {
                        //if (mean >= 128)
                            //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        //else
                            //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));

                        if (mean >= 128)
                            vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                            //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                            //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        if (current >= mean)
                            vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                            //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        else
                            vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                            //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

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

        public static Bitmap Niblack(Bitmap bmp, double k = 0.1)
        {

            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    int min = (int)allValues.Min();
                    int max = (int)allValues.Max();
                    int mean = (max + min) / 2;
                    double standardDeviation = Math.Sqrt((Math.Pow((double)(grayImage[i, j] - mean), 2) + Math.Pow((double)(min - mean), 2) + Math.Pow((double)(max - mean), 2)) / 2);
                    var current = (int)grayImage[i, j];
                    if (current < mean - k * standardDeviation)
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    else
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

            return bmp;
        }

        public static Bitmap MidGrey(Bitmap bmp)
        {

            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    int min = (int)allValues.Min();
                    int max = (int)allValues.Max();
                    int mean = (max + min) / 2;
                    var current = (int)grayImage[i, j];
                    if (current < mean)
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    else
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

            return bmp;
        }

        public static Bitmap Median(Bitmap bmp)
        {

            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    var vals = allValues.ToList();
                    vals.Sort();
                    int median = vals.Count() % 2 == 0 ? (int)(vals[vals.Count / 2] + vals[vals.Count / 2 + 1]) / 2 : (int)vals[vals.Count / 2];
                    var current = (int)grayImage[i, j];
                    if (current < median)
                    {
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    }
                    else
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

            return bmp;
        }
        //k -> const do zmiany, R -> dynamic range (od ilości bitów zależy)
        public static Bitmap Sauvola(Bitmap bmp, double k = 0.5)
        {
            const int R = 8;

            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    int min = (int)allValues.Min();
                    int max = (int)allValues.Max();
                    int mean = (max + min) / 2;
                    double standardDeviation = Math.Sqrt((Math.Pow((double)(grayImage[i, j] - mean), 2) + Math.Pow((double)(min - mean), 2) + Math.Pow((double)(max - mean), 2)) / 2);

                    var current = (int)grayImage[i, j];

                    if (current > mean * (1 + k * standardDeviation / R - 1))
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    else
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

            return bmp;
        }
        //tu lepiej k zostawić na 0
        public static Bitmap Phansalkar(Bitmap bmp, double k = 0)
        {
            const double R = 0.5;

            byte[,] grayImage = ImageTo2DByteArray(bmp);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] vs = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, vs, 0, vs.Length);

            for (int i = 0; i < grayImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayImage.GetLength(1); j++)
                {
                    byte[,] localArray;
                    if (i > 1 && i < grayImage.GetLength(0) - 2 && j > 1 && j < grayImage.GetLength(1) - 3)
                        localArray = new byte[5, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    else if (i < 2 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     {  grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     {  grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (i >= grayImage.GetLength(0) - 3 && j < 2)
                    {
                        localArray = new byte[3, 3] {
                     {  grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     {  grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     {  grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    else if (i < 2 && j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3 && i >= grayImage.GetLength(0) - 3)
                    {
                        localArray = new byte[3, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j]},
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j]},
                    };
                    }
                    else if (j < 2 && i > 1)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else if (j >= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[5, 3] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j] }
                    };
                    }
                    else if (i < 2 && j > 1 && j <= grayImage.GetLength(1) - 3)
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                     { grayImage[i + 1, j - 2], grayImage[i + 1, j - 1], grayImage[i + 1, j], grayImage[i + 1, j + 1], grayImage[i + 1, j + 2] },
                     { grayImage[i + 2, j - 2], grayImage[i + 2, j - 1], grayImage[i + 2, j], grayImage[i + 2, j + 1], grayImage[i + 2, j + 2] }
                    };
                    }
                    else
                    {
                        localArray = new byte[3, 5] {
                     { grayImage[i - 2, j - 2], grayImage[i - 2, j - 1], grayImage[i - 2, j], grayImage[i - 2, j + 1], grayImage[i - 2, j + 2] },
                     { grayImage[i - 1, j - 2], grayImage[i - 1, j - 1], grayImage[i - 1, j], grayImage[i - 1, j + 1], grayImage[i - 1, j+ 2] },
                     { grayImage[i , j - 2], grayImage[i, j - 1], grayImage[i, j], grayImage[i, j + 1], grayImage[i, j+ 2] },
                    };
                    }
                    // ja pierdolę
                    IEnumerable<byte> allValues = localArray.Cast<byte>();
                    int min = (int)allValues.Min();
                    int max = (int)allValues.Max();
                    int mean = (max + min) / 2;
                    double standardDeviation = Math.Sqrt((Math.Pow((double)(grayImage[i, j] - mean), 2) + Math.Pow((double)(min - mean), 2) + Math.Pow((double)(max - mean), 2)) / 2);

                    var current = (int)grayImage[i, j];
                    //t = mean * (1 + p * exp(-q * mean) + k * ((stdev / r) - 1))
                    //p -> jak bardzo ma klasyfikować bg na fg gdy jest <1 algorytm zachowuje się jak Sauvola
                    //q -> również sprawia, że zachowuje się jak Sauvola
                    double q = 10;
                    double p = 3;
                    double t = mean * (1 + p * Math.Exp(-q * mean) + k * ((standardDeviation / R) - 1));
                    if (current > t)
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MinValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(0, 0, 0));
                    else
                        vs[i * data.Stride + (j * 3)] = vs[i * data.Stride + (j * 3 + 1)] = vs[i * data.Stride + (j * 3 + 2)] = byte.MaxValue;
                        //bmp.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }

            }

            Marshal.Copy(vs, 0, data.Scan0, vs.Length);
            bmp.UnlockBits(data);

            return bmp;
        }
    }
}
