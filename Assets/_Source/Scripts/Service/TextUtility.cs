using System;

public static class TextUtility
{
    public static string FormatTime(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"hh\:mm\:ss");
    }

    public static string FormatTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"hh\:mm\:ss");
    }

    public static string FormatMinute(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"mm\:ss");
    }

    public static string FormatMinute(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"mm\:ss");
    }

    public static string FormatSeconds(int seconds)
    {
        return $"{MathF.Round(seconds)}";
    }

    public static string FormatSeconds(float seconds)
    {
        return $"{MathF.Round(seconds)}";
    }
}