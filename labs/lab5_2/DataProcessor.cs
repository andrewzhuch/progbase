namespace lab5_2
{
    public static class DataProcessor
    {
        public static string[] GetPages(int numberOfPage, Root root)
        {
            string[] array = new string[10];
            int counter = 0;
            for(int i = (numberOfPage * 10 - 10); i < numberOfPage * 10; i++)
            {
                array[counter] = root.courses[i].ConvertToSting();
                counter++;
            }
            return array;
        }
        public static Root GetSortedByEnroll(Root root)
        {
            root.courses.Sort(CompareTwoCourses);
            return root;
        }
        private static int CompareTwoCourses(Course x, Course y)
        {
            if(x.enrolled == y.enrolled)
            {
                return 0;
            }
            else if(x.enrolled > y.enrolled)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}