using System;
using System.Drawing;
using OpenCvSharp;
using System.Diagnostics;

namespace lab4_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat bmp = new Mat("./inputImage.png");
            CheckArguments(args);
            if(args[3] == "sharpen")
            {
                ProcessCommandSharpen(args);
            }
            else if(args[3] == "removeGreen")
            {
                ProcessCommandRemoveGreen(args);
            }
            else if(args[3] == "sepia")
            {
                ProcessCommandSepia(args);
            }
            else if(args[3] == "crop")
            {
                ProcessCommandCrop(args);
            }
            else if(args[3] == "flipHorizontal")
            {
                ProcessCommandFlipHorizontal(args);
            }
        }
        static void CheckArguments(string[] args)
        {
            if(args.Length < 4)
            {
                throw new ArgumentException("Not enough aruments.");
            }
            if(args[0] != "pixel" && args[0] != "fast")
            {
                throw new ArgumentException("Wrong name of module.");
            }
            try
            {
                Bitmap check = new Bitmap(args[1]);
            }
            catch
            {
                throw new ArgumentException("File does not exit or file is not an image.");
            }
            if(args[3] != "crop" && args[3] != "flipHorizontal" && args[3] != "removeGreen" && args[3] != "sepia" && args[3] != "sharpen")
            {
                throw new ArgumentException("Wrong method.");
            }
            if(args[3] == "crop" && args.Length <5)
            {
                throw new ArgumentException("For method 'crop' you should input additional arguments.");
            }
            if(args[3] == "sharpen" && args.Length < 5)
            {
                throw new ArgumentException("For method 'sharpen' you should input additional arguments.");
            }
            if((args.Length > 4 && (args[3] != "sharpen" && args[3] != "crop")) || ((args[3] == "sharpen" || args[3] == "crop") && args.Length > 5))
            {
                throw new ArgumentException("Too many arguments.");
            }
            if(args[3] == "sharpen")
            {
                try
                {
                    int checkSharpenArgument = int.Parse(args[4]);
                }
                catch
                {
                    throw new ArgumentException("Value for 'sharpen' should be integer.");
                }
                finally
                {
                    if(int.Parse(args[4]) > 20 || int.Parse(args[4]) < 1)
                    {
                        throw new ArgumentException("Value for 'sharpen' should be in range of 1 to 20.");
                    }
                }
            }
            if(args[3] == "crop")
            {
                int[] cropValues = new int[4];
                string[] temporary = args[4].Split('+');
                if(temporary.Length != 3)
                {
                    throw new Exception("Wrong additional value for method 'crop'");
                }
                string[] temporary2 = temporary[0].Split('x');
                if(temporary2.Length != 2)
                {
                    throw new Exception("Wrong additional value for method 'crop'");
                }
                try
                {
                    cropValues[0] = int.Parse(temporary2[0]);
                    cropValues[1] = int.Parse(temporary2[1]);
                    cropValues[2] = int.Parse(temporary[1]);
                    cropValues[3] = int.Parse(temporary[2]);
                }
                catch
                {
                    throw new Exception("Wrong additional value for method'crop'");
                }
                finally
                {
                    Bitmap image = new Bitmap(args[1]);
                    if(int.Parse(temporary[2]) + int.Parse(temporary2[1]) > image.Height)
                    {
                        throw new Exception("Inputted rectangke cannot be cropped.");
                    }
                    if(int.Parse(temporary[1]) + int.Parse(temporary2[0]) > image.Width)
                    {
                        throw new Exception("Inputted rectangke cannot be cropped.");
                    }
                }
            }
        }
        static void ProcessCommandCrop(string[] args)
        {
            int[] cropValues = new int[4];
            string[] temporary = args[4].Split('+');
            string[] temporary2 = temporary[0].Split('x');
            int left = int.Parse(temporary[1]);
            int top = int.Parse(temporary[2]);
            int width = int.Parse(temporary2[0]);
            int height = int.Parse(temporary2[1]);
            if(args[0] == "fast")
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Fast.FastImageEditor imageEditor = new ProgbaseLab.ImageEditor.Fast.FastImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Crop(image, left, top, width, height);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
            else
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Pixel.PixelImageEditor imageEditor = new ProgbaseLab.ImageEditor.Pixel.PixelImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Crop(image, left, top, width, height);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
        }
        static void ProcessCommandFlipHorizontal(string[] args)
        {
            if(args[0] == "fast")
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Fast.FastImageEditor imageEditor = new ProgbaseLab.ImageEditor.Fast.FastImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.FlipHorizontal(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
            else
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Pixel.PixelImageEditor imageEditor = new ProgbaseLab.ImageEditor.Pixel.PixelImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.FlipHorizontal(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
        }
        static void ProcessCommandSharpen(string[] args)
        {
            if(args[0] == "fast")
            {
                Bitmap image = new Bitmap(args[1]);
                int amount = int.Parse(args[4]);
                ProgbaseLab.ImageEditor.Fast.FastImageEditor imageEditor = new ProgbaseLab.ImageEditor.Fast.FastImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Sharpen(image, amount);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
            else
            {
                Bitmap image = new Bitmap(args[1]);
                int amount = int.Parse(args[4]);
                ProgbaseLab.ImageEditor.Pixel.PixelImageEditor imageEditor = new ProgbaseLab.ImageEditor.Pixel.PixelImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Sharpen(image, amount);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
        }
        static void ProcessCommandRemoveGreen(string[] args)
        {
            if(args[0] == "fast")
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Fast.FastImageEditor imageEditor = new ProgbaseLab.ImageEditor.Fast.FastImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.RemoveGreen(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
            else
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Pixel.PixelImageEditor imageEditor = new ProgbaseLab.ImageEditor.Pixel.PixelImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.RemoveGreen(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
        }
        static void ProcessCommandSepia(string[] args)
        {
            if(args[0] == "fast")
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Fast.FastImageEditor imageEditor = new ProgbaseLab.ImageEditor.Fast.FastImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Sepia(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
            else
            {
                Bitmap image = new Bitmap(args[1]);
                ProgbaseLab.ImageEditor.Pixel.PixelImageEditor imageEditor = new ProgbaseLab.ImageEditor.Pixel.PixelImageEditor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Bitmap result = imageEditor.Sepia(image);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"Time:{ts}");
                result.Save(args[2]);
            }
        }
    }
}