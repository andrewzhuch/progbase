using System;
using System.Drawing;

namespace ProgbaseLab.ImageEditor.Pixel
{
    public class PixelImageEditor : ImageEditor.Common.IImageEditor
    {
        public Bitmap Crop(Bitmap bmp, int left, int top, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            int counter1 = 0;
            for(int i = top; i < top + height; i ++)
            {
                int counter2 = 0;
                for(int j = left; j < left + width; j++)
                {
                    result.SetPixel(counter2, counter1, bmp.GetPixel(j, i));
                    counter2++;
                }
                counter1++;
            }
            return result;
        }
        public Bitmap FlipHorizontal(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            for(int i = 0; i < image.Width / 2; i++)
            {
                int counter = image.Height;
                for(int j = 0; j < image.Height; j++)
                {
                    result.SetPixel(i, counter - 1, image.GetPixel(i, j));
                    counter--;
                }
            }
            for(int i = image.Width / 2; i < image.Width; i++)
            {
                int counter = image.Height;
                for(int j = 0; j < image.Height; j++)
                {
                    result.SetPixel(i, counter - 1, image.GetPixel(i, j));
                    counter--;
                }
            }
            result.RotateFlip(RotateFlipType.Rotate180FlipNone);
            return result;
        }

        public Bitmap RemoveGreen(Bitmap image)
        {
            for(int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color color = image.GetPixel(j, i);
                    Color newColor = Color.FromArgb(color.A, color.R, 0, color.B);
                    image.SetPixel(j, i, newColor);
                }
            }
            return image;
        }

        public Bitmap Sepia(Bitmap image)
        {
            Bitmap resut = new Bitmap(image.Width, image.Height);
            for(int i = 0; i < image.Height; i++)
            {
                for(int j = 0; j < image.Width; j++)
                {
                    Color color = image.GetPixel(j, i);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int newRed = (int)Math.Min(255, 0.393*R + 0.769*G + 0.189*B);
                    int newGreen = (int)Math.Min(255, 0.349*R + 0.686*G + 0.168*B);
                    int newBlue = (int)Math.Min(255, 0.272*R + 0.534*G + 0.131*B);
                    Color newColor = Color.FromArgb(255, newRed, newGreen, newBlue);
                    resut.SetPixel(j, i, newColor);
                }
            }
            return resut;
        }

        public Bitmap Sharpen(Bitmap image, int amount)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            double[,] filter = new double[,] {
            {-1, -1, -1},
            {-1, 9, -1},
            {-1, -1, -1}
            };
            for(int x = 0; x < image.Width; x++)
            {
                for(int y = 0; y < image.Height; y++)
                {
                    Color newColor = ApplySharpFilter(image, x, y, filter, amount);
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;
        }
        private static Color ApplySharpFilter(Bitmap image, int x, int y, double[,] filter, double factor)
        {
            double red = 0.0;
            double green = 0.0;
            double blue = 0.0;
            int filterSize = filter.GetLength(0);
            int radius = filterSize / 2;
            int w = image.Width;
            int h = image.Height;
            for (int filterX = -radius; filterX <= radius; filterX++)
            {
                for (int filterY = -radius; filterY <= radius; filterY++)
                {
                    double filterValue = filter[filterX + radius, filterY + radius];
                    int imageX = (x + filterX + w) % w;
                    int imageY = (y + filterY + h) % h;
                    Color imageColor = image.GetPixel(imageX, imageY);
                    red += imageColor.R * filterValue;
                    green += imageColor.G * filterValue;
                    blue += imageColor.B * filterValue;
                }
            }
            int r = Math.Min(Math.Max((int)(factor * red), 0), 255);
            int g = Math.Min(Math.Max((int)(factor * green), 0), 255);
            int b = Math.Min(Math.Max((int)(factor * blue), 0), 255);
            return Color.FromArgb(r, g, b);
        }
    }
}
