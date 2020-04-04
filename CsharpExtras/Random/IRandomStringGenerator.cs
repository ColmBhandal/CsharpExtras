namespace CsharpExtras.RandomDataGen.Impl
{
    public interface IRandomStringGenerator
    {
        string RandomAlphaNumericLowerCaseString(int length);
        string RandomAlphaNumericMixedCaseString(int length);
        string RandomAlphaNumericUpperCaseString(int length);
        string RandomString(int length, string alphabet);
    }
}