using System.Text.RegularExpressions;

namespace IronOCR_Demos.Demos;

/// <summary>
/// Reusable utility for comparing extracted OCR text against expected text.
/// Provides word-level accuracy measurement for use across demos.
/// </summary>
public static class TextComparer
{
    /// <summary>
    /// Calculates word-level accuracy: the percentage of expected words found in extracted text.
    /// Returns 0–100. Handles whitespace normalization and case-insensitive matching.
    /// </summary>
    public static double CalculateAccuracy(string extracted, string expected)
    {
        if (string.IsNullOrWhiteSpace(expected)) return 0;
        if (string.IsNullOrWhiteSpace(extracted)) return 0;

        var extractedWords = Normalize(extracted).Split(' ');
        var expectedWords = Normalize(expected).Split(' ');

        var extractedSet = new HashSet<string>(extractedWords);
        int matchCount = expectedWords.Count(w => extractedSet.Contains(w));

        return (double)matchCount / expectedWords.Length * 100.0;
    }

    private static string Normalize(string s) =>
        Regex.Replace(s.Trim().ToLowerInvariant(), @"\s+", " ");
}
