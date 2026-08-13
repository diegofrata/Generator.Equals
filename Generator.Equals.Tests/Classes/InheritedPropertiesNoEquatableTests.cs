using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for the new behavior where inherited properties are explicitly compared
/// when NO ancestor has [Equatable]. This ensures that inherited members are not
/// silently ignored.
/// </summary>
public partial class InheritedPropertiesNoEquatableTests : SnapshotTestBase
{
    // GrandParent with no [Equatable]
    public class GrandParent
    {
        public GrandParent(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    // Parent with no [Equatable]
    public class Parent : GrandParent
    {
        public Parent(string name, int age) : base(name)
        {
            Age = age;
        }

        public int Age { get; }
    }

    // Child with [Equatable] - should compare Name, Age, AND School
    [Equatable]
    public partial class Child : Parent
    {
        public Child(string name, int age, string school) : base(name, age)
        {
            School = school;
        }

        public string School { get; }
    }

    public static TheoryData<Child, Child, bool> EqualityCases => new()
    {
        // All same
        { new Child("Dave", 35, "MIT"), new Child("Dave", 35, "MIT"), true },
        // Different Name (inherited from GrandParent)
        { new Child("Dave", 35, "MIT"), new Child("John", 35, "MIT"), false },
        // Different Age (inherited from Parent)
        { new Child("Dave", 35, "MIT"), new Child("Dave", 40, "MIT"), false },
        // Different School (declared on Child)
        { new Child("Dave", 35, "MIT"), new Child("Dave", 35, "Harvard"), false },
        // All different
        { new Child("Dave", 35, "MIT"), new Child("John", 40, "Harvard"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Child a, Child b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                // GrandParent with no [Equatable]
                                public class InheritedPropertiesGrandParent
                                {
                                    public InheritedPropertiesGrandParent(string name)
                                    {
                                        Name = name;
                                    }

                                    public string Name { get; }
                                }

                                // Parent with no [Equatable]
                                public class InheritedPropertiesParent : InheritedPropertiesGrandParent
                                {
                                    public InheritedPropertiesParent(string name, int age) : base(name)
                                    {
                                        Age = age;
                                    }

                                    public int Age { get; }
                                }

                                // Child with [Equatable] - should compare Name, Age, AND School
                                [Equatable]
                                public partial class InheritedPropertiesChild : InheritedPropertiesParent
                                {
                                    public InheritedPropertiesChild(string name, int age, string school) : base(name, age)
                                    {
                                        School = school;
                                    }

                                    public string School { get; }
                                }
                                """;
}
