class MyClass
{
    public IEnumerator<string> GetEnumerator()
    {
        // Get enumerable
        var enumerable = BlackAndWhite();

        // Return enumerator
        return enumerable.GetEnumerator();
    }
    
    // Iterator
    public IEnumerable<string> BlackAndWhite()
    {
        yield return "Black";
        yield return "gray";
        yield return "White";
    }
}