// Copyright (c) 2024, Awessets

using System;
using System.Text;

namespace MergeIt.Core.Helpers
{
    public static class TimeExtensions
    {
        private static readonly DateTimeOffset UnixEpoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds)
        {
            return UnixEpoch.AddMilliseconds(milliseconds);
        }

        public static string FormatTime(this long value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);

            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan:hh\\:mm\\:ss}";
            }

            return $"{timeSpan:mm\\:ss}";
        }

        public static string FormatTime(this float value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);

            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan:hh\\:mm\\:ss}";
            }

            return $"{timeSpan:mm\\:ss}";
        }

        public static string FormatTime(this int value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);
            var sb = new StringBuilder();

            if (timeSpan.Hours > 0)
            {
                sb.Append($"{timeSpan.Hours}h");
            }

            if (timeSpan.Minutes > 0)
            {
                sb.Append($"{timeSpan.Minutes}m");
            }

            if (timeSpan.Seconds > 0)
            {
                sb.Append($"{timeSpan.Seconds}s");
            }

            return sb.ToString();
        }
    }
}