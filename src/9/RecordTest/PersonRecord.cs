namespace RecordTest
{
    internal record Person(string Name);

    internal record Person2
    {
        public string Name { get; init; } = string.Empty;
    }

    internal record Person3
    {
        public string Name { get; set; } = string.Empty;
    }
}
