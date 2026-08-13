namespace Generator.Equals.Tests.Infrastructure;

/// <summary>
/// Record for encapsulating equality test case data.
/// Can be used for more complex test scenarios requiring metadata.
/// </summary>
/// <typeparam name="T">The type being tested</typeparam>
/// <param name="First">First object in the comparison</param>
/// <param name="Second">Second object in the comparison</param>
/// <param name="ExpectedEqual">Whether the objects should be considered equal</param>
/// <param name="Description">Optional description for the test case</param>
public record EqualityTestData<T>(
    T First,
    T Second,
    bool ExpectedEqual,
    string? Description = null)
{
    public override string ToString() =>
        Description ?? $"{First} vs {Second} => {(ExpectedEqual ? "Equal" : "NotEqual")}";
}
