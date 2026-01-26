class MyClass
{
    public IEnumerator<string> GetEnumerator()
    {
        return BlackAndWhite();
    }
    
    // Iterator
    public IEnumerator<string> BlackAndWhite()
    {
        yield return "Black";
        yield return "gray";
        yield return "White";
    }
}