using System;

namespace Algorithm_Visualizer
{
    internal class Utility
    {
        public static class RandomGeneration
        {
            public static Random Random = new Random();

            public static T[] GenerateRandomArray<T>(int size, Func<T> generator) where T : IComparable<T>
            {
                T[] arr = new T[size];
                for (int i = 0; i < size; i++)
                {
                    arr[i] = generator(); // Use the generator function to create values
                }
                return arr;
            }
        }
    }
}
