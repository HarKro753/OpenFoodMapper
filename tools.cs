class Tools
{
    public static List<string> ParseCommaSeparatedValues(string input)
    {
    if (string.IsNullOrWhiteSpace(input))
        return new List<string>();

    var values = input.Split(',')
        .Select(v => v.Trim())
        .Where(v => !string.IsNullOrWhiteSpace(v))
        .Where(v => !HasLanguagePrefix(v)) // Filter out entries with language prefixes
        .ToList();

    return values;
    }

   private  static bool HasLanguagePrefix(string value)
    {
    // Check if value starts with language code like "en:", "fr:", "de:", etc.
    if (value.Length < 3)
        return false;

    return value.Length >= 3 &&
           char.IsLower(value[0]) &&
           char.IsLower(value[1]) &&
           value[2] == ':';
    }
}

