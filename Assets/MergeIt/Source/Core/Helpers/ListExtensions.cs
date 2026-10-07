// Copyright (c) 2024, Awessets

using System.Collections.Generic;

namespace MergeIt.Core.Helpers
{
    public static class ListExtensions
    {
        public static IList<int> GenerateShuffledArray(int size)
        {
            IList<int> array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }

            array.Shuffle();

            return array;
        }
        
        public static void Shuffle<TSource>(this IList<TSource> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);

                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}