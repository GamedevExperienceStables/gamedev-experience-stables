namespace Game.Utils
{
    public static class StringExtensions
    {
        public static string UppercaseFirst(this string str)
            => string.IsNullOrEmpty(str) ? string.Empty : $"{char.ToUpper(str[0])}{str[1..]}";
    }
}