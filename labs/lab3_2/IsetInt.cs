namespace lab3_2
{
    public interface IsetInt
    {
        int GetCount();
        bool Add(int value);
        bool Remove(int value);
        bool Contains(int value);
        void Clear();
        void CopyTo(int[] array);
        bool SetEquals(IsetInt other);
        void UnionWith(IsetInt other);
    }
}