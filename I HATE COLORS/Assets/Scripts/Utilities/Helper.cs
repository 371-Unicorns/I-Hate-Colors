using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic helper.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Gets a weighted random from an array.
    /// Both input arrays have to be the same length.
    /// 
    /// Based of https://docs.unity3d.com/Manual/RandomNumbers.html.
    /// </summary>
    /// <param name="data">Array containing all possible element.</param>
    /// <param name="weight">Array containing the weight of the elements. weight[0] defines the weight of data[0]. Sum of all weights can be greater than 1.</param>
    /// <returns></returns>
    public static T WeightedRandom<T>(T[] data, float[] weight)
    {
        float total = 0;

        foreach (float elem in weight)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < weight.Length; i++)
        {
            if (randomPoint < weight[i])
            {
                return data[i];
            }
            else
            {
                randomPoint -= weight[i];
            }
        }
        return data[weight.Length - 1];
    }
}
