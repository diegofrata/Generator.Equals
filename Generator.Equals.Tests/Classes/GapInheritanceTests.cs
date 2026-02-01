using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for inheritance chains where there's a "gap" - a class without [Equatable]
/// between two classes that have [Equatable].
/// Pattern: A [Equatable] → B (no attribute) → C [Equatable]
/// C should call base.Equals() which eventually reaches A's generated Equals.
/// </summary>
public partial class GapInheritanceTests : SnapshotTestBase
{
    // Root class with [Equatable]
    [Equatable]
    public partial class GrandParent
    {
        public string Name { get; set; } = "";
    }

    // Middle class WITHOUT [Equatable] - inherits GrandParent's Equals
    public class Parent : GrandParent
    {
        public int Age { get; set; }
    }

    // Leaf class with [Equatable] - should call base.Equals() to reach GrandParent's equality
    [Equatable]
    public partial class Child : Parent
    {
        public string School { get; set; } = "";
    }

    public static TheoryData<Child, Child, bool> Cases => new()
    {
        // Same Name, Age, School - should be equal (Name compared via GrandParent)
        {
            new Child { Name = "Alice", Age = 10, School = "PS101" },
            new Child { Name = "Alice", Age = 10, School = "PS101" },
            true
        },
        // Different Name - should NOT be equal (Name compared via GrandParent.Equals)
        {
            new Child { Name = "Alice", Age = 10, School = "PS101" },
            new Child { Name = "Bob", Age = 10, School = "PS101" },
            false
        },
        // Different Age - should be equal (Age is in Parent which has no [Equatable])
        // Parent doesn't have equality comparison, and GrandParent only compares Name
        {
            new Child { Name = "Alice", Age = 10, School = "PS101" },
            new Child { Name = "Alice", Age = 20, School = "PS101" },
            true
        },
        // Different School - should NOT be equal (School compared by Child)
        {
            new Child { Name = "Alice", Age = 10, School = "PS101" },
            new Child { Name = "Alice", Age = 10, School = "PS202" },
            false
        },
    };

    [Theory, MemberData(nameof(Cases))]
    public void Equality(Child a, Child b, bool expected) => EqualityAssert.Verify(a, b, expected);

    [Theory, MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                // Root class with [Equatable]
                                [Equatable]
                                public partial class GapGrandParent
                                {
                                    public string Name { get; set; } = "";
                                }

                                // Middle class WITHOUT [Equatable] - inherits GrandParent's Equals
                                public class GapParent : GapGrandParent
                                {
                                    public int Age { get; set; }
                                }

                                // Leaf class with [Equatable] - should call base.Equals() to reach GrandParent's equality
                                [Equatable]
                                public partial class GapChild : GapParent
                                {
                                    public string School { get; set; } = "";
                                }
                                """;
}
