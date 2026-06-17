using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shuffles collections around without having
/// to do the whole shebang everytime.
/// Also, it's in extension form!
/// </summary>
public static class CollectionShuffler
{
    /// <summary>
    /// Shuffles lists
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int randomIndex = Random.Range(0, i + 1);

            // Swap the elements
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        return list;
    }
    
    /// <summary>
    /// Shuffles arrays
    /// </summary>
    /// <param name="arr"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] Shuffle<T>(this T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int randomIndex = Random.Range(0, i + 1);

            // Swap the elements
            (arr[i], arr[randomIndex]) = (arr[randomIndex], arr[i]);
        }
        return arr;
    }
}