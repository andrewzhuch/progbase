using System;
using static System.Math;

namespace lab3_2
{
    public class ArraySetInt : IsetInt
    {
        /*
        Set based on array of boolean values, which makes some methods faster, but it can contain only limited number of integers
        User can set range of possible values
        in case of union of two sets, lowest and largest borders of two sets will set new range of values
        */
        private bool[] _items;
        private int _size;
        private int _minValue;
        private int _maxValue;
        public ArraySetInt(int minValue, int maxValue)
        {
            if(minValue < 0 && maxValue < 0)
            {
                this._items = new bool[Math.Abs(minValue) - Math.Abs(maxValue) + 1];
            }
            else if(minValue < 0 && maxValue >= 0)
            {
                this._items = new bool[Math.Abs(minValue) + maxValue + 1];
            }
            else if(minValue >= 0 && maxValue >= 0)
            {
                this._items = new bool[maxValue - minValue + 1];
            }
            this._size = 0;
            this._minValue = minValue;
            this._maxValue = maxValue;
        }

        public bool Add(int value)
        {
            bool isHere = this.Contains(value);
            if(isHere)
            {
                return false;
            }
            if(this._minValue >= 0)
            {
                this._items[value - this._minValue] = true;
            }
            else
            {
                this._items[value + Math.Abs(this._minValue)] = true;
            }
            _size++;
            return true;
        }

        public void Clear()
        {
            for(int i = 0; i < this._items.Length; i++)
            {
                this._items[i] = false;
            }
            this._size = 0;
        }

        public bool Contains(int value)
        {
            if(this._minValue >= 0)
            {
                if(this._items[value - this._minValue] == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if(this._items[value + Math.Abs(this._minValue)] == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void CopyTo(int[] array)
        {
            int counter = 0;
            if(this._minValue >= 0)
            {
                for(int i = 0; i < this._items.Length; i++)
                {
                    if(this._items[i] == true)
                    {
                        array[counter] = i + this._minValue;
                        counter++;
                    }
                }
            }
            else
            {
                for(int i = 0; i < this._items.Length; i++)
                {
                    if(this._items[i] == true)
                    {
                        array[counter] = i - Math.Abs(this._minValue);
                        counter++;
                    }
                }
            }
        }

        public int GetCount()
        {
            return _size;
        }

        public bool Remove(int value)
        {
            if(this._minValue >= 0)
            {
                if(this._items[value - this._minValue] == false)
                {
                    return false;
                }
                else
                {
                    this._items[value - this._minValue] = false;
                    this._size--;
                    return true;
                }
            }
            else
            {
                if(this._items[value + Math.Abs(this._minValue)] == false)
                {
                    return false;
                }
                else
                {
                    this._items[value + Math.Abs(this._minValue)] = false;
                    this._size--;
                    return true; 
                }
            }
        }
        public bool SetEquals(IsetInt other)
        {
            int[] array1 = new int[this.GetCount()];
            this.CopyTo(array1);
            int[] array2 = new int[other.GetCount()];
            other.CopyTo(array2);
            if(array1.Length != array2.Length)
            {
                return false;
            }
            for(int i = 0; i < array1.Length; i++)
            {
                if(array1[i] != array2[i])
                {
                   return false;
                }
            }
            return true;
        }
        public void UnionWith(IsetInt other)
        {
            int[] array2 = new int[other.GetCount()];
            other.CopyTo(array2);
            int min2 = array2[0];
            int max2 = array2[0];
            for(int i = 0; i < array2.Length; i++)
            {
                if(array2[i] < min2)
                {
                    min2 = array2[i];
                }
                if(array2[i] > max2)
                {
                    max2 = array2[i];
                }
            }
            if(min2 < this._minValue && max2 > this._maxValue)
            {
                if(min2 < 0 && max2 < 0)
                {
                    Array.Resize(ref this._items, Math.Abs(min2) - Math.Abs(max2) + 1);
                    this._minValue = min2;
                    this._maxValue = max2;
                }
                if(min2 < 0 && max2 >= 0)
                {
                    Array.Resize(ref this._items, Math.Abs(min2) + max2 + 1);
                    this._minValue = min2;
                    this._maxValue = max2;
                }
                if(min2 >= 0 && max2 >= 0)
                {
                    Array.Resize(ref this._items, max2 - min2 + 1);
                    this._minValue = min2;
                    this._maxValue = max2;
                }
            }
            else if(min2 < this._minValue && max2 <= this._maxValue)
            {
                if(min2 < 0 && this._maxValue < 0)
                {
                    Array.Resize(ref this._items, Math.Abs(min2) - Math.Abs(this._maxValue) + 1);
                    this._minValue = min2;
                }
                if(min2 < 0 && this._maxValue >= 0)
                {
                    Array.Resize(ref this._items, Math.Abs(min2) + this._maxValue + 1);
                    this._minValue = min2;
                }
                if(min2 >= 0 && this._maxValue >= 0)
                {
                    Array.Resize(ref this._items, this._maxValue - min2 + 1);
                    this._minValue = min2;
                }
            }
            else if(min2 >= this._minValue && max2 > this._maxValue)
            {
                if(this._minValue < 0 && max2 < 0)
                {
                    Array.Resize(ref this._items, Math.Abs(this._minValue) - Math.Abs(max2) + 1);
                    this._maxValue = max2;
                }
                if(this._minValue < 0 && max2 >= 0)
                {
                    Array.Resize(ref this._items, Math.Abs(this._minValue) + max2 + 1);
                    this._maxValue = max2;
                }
                if(this._minValue >= 0 && max2 >= 0)
                {
                    Array.Resize(ref this._items, max2 - this._minValue+ 1);
                    this._maxValue = max2;
                }
            }
            for(int i = 0; i < array2.Length; i++)
            {
                this.Add(array2[i]);
            }
        }
    }
}   