#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json;

public static class MaxIncreasingSubArraySolver
{

    public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
            return "[]";

        int bestStart = 0, bestEnd = 0;              // [bestStart, bestEnd] inclusive
        long bestSum = numbers[0];

        int currStart = 0;
        long currSum = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                // artış devam ediyor
                currSum += numbers[i];
            }
            else
            {
                // mevcut alt diziyi kapat, en iyiyle karşılaştır
                UpdateBestIfNeeded(ref bestStart, ref bestEnd, ref bestSum,
                                   currStart, i - 1, currSum, numbers);

                // yeni alt dizi başlat
                currStart = i;
                currSum = numbers[i];
            }
        }

        // son alt diziyi de değerlendir
        UpdateBestIfNeeded(ref bestStart, ref bestEnd, ref bestSum,
                           currStart, numbers.Count - 1, currSum, numbers);

        // seçilen aralığı JSON olarak döndür
        var result = numbers.GetRange(bestStart, bestEnd - bestStart + 1);
        return JsonSerializer.Serialize(result);
    }

    private static void UpdateBestIfNeeded(ref int bestStart, ref int bestEnd, ref long bestSum,
                                           int candStart, int candEnd, long candSum, List<int> numbers)
    {
        if (candEnd < candStart) return;

        bool better = false;

        if (candSum > bestSum)
        {
            better = true;
        }
        else if (candSum == bestSum)
        {
            int bestLen = bestEnd - bestStart + 1;
            int candLen = candEnd - candStart + 1;

            if (candLen > bestLen)
                better = true;
            else if (candLen == bestLen && candStart < bestStart)
                better = true;
        }

        if (better)
        {
            bestStart = candStart;
            bestEnd = candEnd;
            bestSum = candSum;
        }
    }
}

