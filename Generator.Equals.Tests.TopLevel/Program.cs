using System.Diagnostics;
using Generator.Equals;

var barA = new ClassSample("Bar", 45);
var barB = new ClassSample("Bar", 44);
var barC = new ClassSample("Bar", 44);

Debug.Assert(barA != barB);
Debug.Assert(barB == barC);

var fuzA = new RecordSample("Dog", 2);
var fuzB = new RecordSample("Dog", 2);

Debug.Assert(fuzA.Equals(fuzB));

[Equatable]
public partial class ClassSample
{
    public ClassSample(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string Name { get; init; }
    public int Age { get; init; }
}

[Equatable]
public partial record RecordSample(string Name, int Age);

[Equatable]
public partial record struct RecordStructSample(string Name, int Age);

[Equatable]
public partial struct StructSample
{
    public string Name { get; }
    public int Age { get; }
}