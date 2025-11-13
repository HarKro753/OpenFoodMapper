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

   public  static bool HasLanguagePrefix(string value)
{
    // Check if value starts with language code like "en:", "fr:", "de:", etc.
    if (value.Length < 3)
        return false;

    return value.Length >= 3 &&
           char.IsLower(value[0]) &&
           char.IsLower(value[1]) &&
           value[2] == ':';
}
    public static List<string> GetCsvFilePaths()
    {
        var files = new List<string>();
        var directory = Directory.GetCurrentDirectory();
        var foodDirectory = Path.Combine(directory, "Food");

        // Generate file names from part_aa to part_bp
        for (char first = 'a'; first <= 'b'; first++)
        {
            var endChar = first == 'a' ? 'z' : 'p';
            for (char second = 'a'; second <= endChar; second++)
            {
                var fileName = $"part_{first}{second}";
                var filePath = Path.Combine(foodDirectory, fileName);

                if (File.Exists(filePath))
                {
                    files.Add(filePath);
                }
                else
                {
                    Console.WriteLine($"Warning: File not found: {filePath}");
                }
            }
        }

        return files;
    }
}

