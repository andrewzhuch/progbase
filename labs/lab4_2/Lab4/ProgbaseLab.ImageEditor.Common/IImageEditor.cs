using System.Drawing;

namespace ProgbaseLab.ImageEditor.Common
{
    public interface IImageEditor
    {
        Bitmap Crop(Bitmap bmp, int left, int top, int width, int height);
        Bitmap FlipHorizontal(Bitmap image);
        Bitmap RemoveGreen(Bitmap image);
        Bitmap Sepia(Bitmap image);
        Bitmap Sharpen(Bitmap image, int amount);
    }
}
