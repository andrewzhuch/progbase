namespace lab5_2
{
    public static class ImageProcessor
    {
        public static void DrawImage(Course[] courses, string path)
        {
            ScottPlot.Plot plt = new ScottPlot.Plot(400, 300);
            double[] xs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] valuesB = new double[10];
            for(int i = 0; i < 10; i++)
            {
                valuesB[i] = (double)courses[i].limit;
            }
            double[] valuesA = new double[10];
            for(int i = 0; i < 10; i++)
            {
                valuesA[i] = (double)courses[i].enrolled;
            }
            plt.PlotBar(xs, valuesB, label: "Series B");
            plt.PlotBar(xs, valuesA, label: "Series A");
            plt.Title("Stacked Bar Charts");
            plt.SaveFig(path);
        }
    }
}