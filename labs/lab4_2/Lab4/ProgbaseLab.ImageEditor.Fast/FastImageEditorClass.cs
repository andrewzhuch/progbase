using System;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing.Imaging;

namespace ProgbaseLab.ImageEditor.Fast
{
    public class FastImageEditor : ImageEditor.Common.IImageEditor
    {
        public Bitmap Crop(Bitmap bmp, int left, int top, int width, int height)
        {
            Mat mat = BitmapConverter.ToMat(bmp);
            Rect rect = new Rect(left, top, width, height);
            Mat croppedImage = new Mat(mat, rect);
            return BitmapConverter.ToBitmap(croppedImage);
        }
        public Bitmap FlipHorizontal(Bitmap image)
        {
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return image;
        }

        public Bitmap RemoveGreen(Bitmap image)
        {
            Mat bmp = BitmapConverter.ToMat(image); 
            Mat[] channels = Cv2.Split(bmp); 
            channels[1].SetTo(0); 
            Mat result = new Mat(); 
            Cv2.Merge(channels, result); 
            return BitmapConverter.ToBitmap(result);
        }

        public Bitmap Sepia(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(result);
            ColorMatrix colorMatrix =  SepiaMatrix();
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(image, new Rectangle(0, 0 ,image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            attributes.Dispose();
            g.Dispose();
            return result;
        }

        public Bitmap Sharpen(Bitmap image, int amount)
        {
            Mat bmp = BitmapConverter.ToMat(image);
            Mat result = new Mat();
            float[,] data = {
            {-1, -1, -1, },
            {-1, 9, -1, },
            {-1, -1, -1, },
            };
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    data[i,j] = data[i, j] * amount / 2;
                }
            }
            Mat kernel = new Mat(data.GetLength(0), data.GetLength(1), MatType.CV_32FC1, data);
            Cv2.Filter2D(bmp, result, -1, kernel);
            Bitmap convertedResult = BitmapConverter.ToBitmap(result);
            return convertedResult;
        }
        private static ColorMatrix SepiaMatrix()
        {
            return new ColorMatrix(
            new float[][]
            {
                new float[] {0.393f, 0.349f, 0.272f, 0, 0},
                new float[] {0.769f, 0.686f, 0.534f, 0, 0},
                new float[] {0.189f, 0.168f, 0.131f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });
        }
    }
}
