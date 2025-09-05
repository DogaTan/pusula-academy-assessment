#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

public static class LongestVowelSubsequenceSolver{
 public static string LongestVowelSubsequenceAsJson(List<string> words)
    {
        if (words == null || words.Count == 0)
            return "[]";

        // hem küçük hem büyük harfleri destekle
        var vowels = new HashSet<char>(new[]{'a','e','i','o','u','A','E','I','O','U'});

        var results = new List<object>(capacity: words.Count);

        foreach (var word in words)
        {
            if (string.IsNullOrEmpty(word))
            {
                results.Add(new { word = word ?? string.Empty, sequence = "", length = 0 });
                continue;
            }

            int bestStart = -1, bestLen = 0;
            int currStart = -1, currLen = 0;

            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (vowels.Contains(c))
                {
                    if (currLen == 0)
                        currStart = i;
                    currLen++;
                }
                else
                {
                    // blok kapat
                    if (currLen > 0)
                    {
                        if (currLen > bestLen)
                        {
                            bestLen = currLen;
                            bestStart = currStart;
                        }
                        // sıfırla
                        currStart = -1;
                        currLen = 0;
                    }
                }
            }

            // kelime sonundaki olası blok
            if (currLen > 0 && currLen > bestLen)
            {
                bestLen = currLen;
                bestStart = currStart;
            }

            string seq = (bestLen > 0 && bestStart >= 0) ? word.Substring(bestStart, bestLen) : "";

            results.Add(new { word = word, sequence = seq, length = seq.Length });
        }

        // örneklerdeki anahtarlar küçük harfli: word/sequence/length
        return JsonSerializer.Serialize(results);
    }
}