using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for an [Equatable] class inheriting from a non-[Equatable] base that hand-rolls a complete
/// equality contract (overrides both Equals(object) and GetHashCode). The generated code must delegate
/// to base.Equals()/base.GetHashCode() so the base's custom semantics are honored - rather than
/// re-deriving equality from the base's public properties (see issue #86).
/// A single-override base (Equals XOR GetHashCode) is a CS0659/CS0661 bug and must NOT be delegated to;
/// it falls back to comparing the base's public properties so the generated pair stays self-consistent.
/// </summary>
public partial class InheritedFromManualEqualityTests : SnapshotTestBase
{
    // Base WITHOUT [Equatable], with a complete hand-written contract using CASE-INSENSITIVE semantics.
    // The case-insensitivity is the tell: it is only observed if base.Equals()/base.GetHashCode() are called.
    public class Animal
    {
        public Animal(string name) => Name = name;

        public string Name { get; }

        public override bool Equals(object? obj) =>
            obj is Animal other && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }

    [Equatable]
    public partial class Dog : Animal
    {
        public Dog(string name, string breed) : base(name) => Breed = breed;

        public string Breed { get; }
    }

    public static TheoryData<Dog, Dog, bool> EqualityCases => new()
    {
        // Identical
        { new Dog("Rex", "Lab"), new Dog("Rex", "Lab"), true },
        // Base Name differs only by case -> equal ONLY if the base's case-insensitive Equals is delegated to
        { new Dog("Rex", "Lab"), new Dog("REX", "Lab"), true },
        // Base Name differs -> not equal (base portion)
        { new Dog("Rex", "Lab"), new Dog("Fido", "Lab"), false },
        // Derived Breed differs -> not equal (derived portion)
        { new Dog("Rex", "Lab"), new Dog("Rex", "Terrier"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Dog a, Dog b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // A non-[Equatable] subclass of Dog. Animal's hand-written Equals accepts ANY Animal, so without an
    // exact-runtime-type check a Dog and a Puppy with the same values would compare equal (and the
    // relationship would be asymmetric once Puppy adds members). The generated Equals must keep the
    // other.GetType() == this.GetType() guard in front of the base delegation.
    public class Puppy : Dog
    {
        public Puppy(string name, string breed) : base(name, breed) { }
    }

    [Fact]
    public void SubclassOfEquatableOverManualBase_IsNotEqual()
    {
        var dog = new Dog("Rex", "Lab");
        var puppy = new Puppy("Rex", "Lab");

        EqualityAssert.Verify(dog, puppy, false);
        EqualityAssert.Verify<Dog>(puppy, dog, false);
    }

    // A plain class (no equality of its own) sitting between the [Equatable] leaf and the manual base:
    // its members must be collected explicitly, while the manual grandparent is reached via base.Equals().
    public class GrandBase
    {
        public GrandBase(string tag) => Tag = tag;

        public string Tag { get; }

        public override bool Equals(object? obj) =>
            obj is GrandBase other && string.Equals(Tag, other.Tag, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Tag);
    }

    public class Middle : GrandBase
    {
        public Middle(string tag, int level) : base(tag) => Level = level;

        public int Level { get; }
    }

    [Equatable]
    public partial class Leaf : Middle
    {
        public Leaf(string tag, int level, string name) : base(tag, level) => Name = name;

        public string Name { get; }
    }

    public static TheoryData<Leaf, Leaf, bool> GrandparentCases => new()
    {
        { new Leaf("X", 1, "a"), new Leaf("X", 1, "a"), true },
        // Grandparent Tag differs only by case -> equal (manual grandparent delegated via base.Equals)
        { new Leaf("X", 1, "a"), new Leaf("x", 1, "a"), true },
        // Grandparent Tag differs -> not equal
        { new Leaf("X", 1, "a"), new Leaf("Y", 1, "a"), false },
        // Plain intermediate Level differs -> not equal (collected member)
        { new Leaf("X", 1, "a"), new Leaf("X", 2, "a"), false },
        // Leaf's own Name differs -> not equal
        { new Leaf("X", 1, "a"), new Leaf("X", 1, "b"), false },
    };

    [Theory]
    [MemberData(nameof(GrandparentCases))]
    public void GrandparentEquality(Leaf a, Leaf b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // "Comparer behind manual": an [Equatable] root (Widget), a hand-written intermediate WITHOUT
    // [Equatable] that overrides ONLY Equals(object)/GetHashCode (no typed Equals(TIntermediate)), and an
    // [Equatable] tip (Button). Because Widget generates a protected Equals(Widget?), delegating with a
    // ManualPanel-typed argument would bind to THAT overload and skip ManualPanel.Equals (dropping Slot);
    // the generator casts the argument to object so the hand-written Equals(object) is selected instead.
    [Equatable]
    public partial class Widget
    {
        public Widget(string sku) => Sku = sku;

        public string Sku { get; }
    }

    public class ManualPanel : Widget
    {
        public ManualPanel(string sku, int slot) : base(sku) => Slot = slot;

        public int Slot { get; }

        public override bool Equals(object? obj) =>
            obj is ManualPanel other && base.Equals(other) && Slot == other.Slot;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Slot);
    }

    [Equatable]
    public partial class Button : ManualPanel
    {
        public Button(string sku, int slot, string label) : base(sku, slot) => Label = label;

        public string Label { get; }
    }

    public static TheoryData<Button, Button, bool> ComparerBehindManualCases => new()
    {
        { new Button("s", 1, "ok"), new Button("s", 1, "ok"), true },
        // Root (Widget.Sku) differs -> not equal
        { new Button("s", 1, "ok"), new Button("t", 1, "ok"), false },
        // Manual intermediate (ManualPanel.Slot) differs -> not equal. This is the regression case:
        // pre-fix, base.Equals bound to Widget.Equals(Widget?) and Slot was never compared (equal).
        { new Button("s", 1, "ok"), new Button("s", 2, "ok"), false },
        // Tip's own member (Button.Label) differs -> not equal
        { new Button("s", 1, "ok"), new Button("s", 1, "no"), false },
    };

    [Theory]
    [MemberData(nameof(ComparerBehindManualCases))]
    public void ComparerBehindManualEquality(Button a, Button b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // A SEALED [Equatable] over a hand-written base: the nested comparer dispatches via x.Equals(y)
    // (not x.Equals((object?)y)) yet the (object?)-cast __BaseEquals bridge is still emitted, so the
    // base's case-insensitive Equals must still be honored.
    [Equatable]
    public sealed partial class SealedDog : Animal
    {
        public SealedDog(string name, string breed) : base(name) => Breed = breed;

        public string Breed { get; }
    }

    public static TheoryData<SealedDog, SealedDog, bool> SealedCases => new()
    {
        { new SealedDog("Rex", "Lab"), new SealedDog("Rex", "Lab"), true },
        // Base Name differs only by case -> equal only if the base's Equals is delegated to
        { new SealedDog("Rex", "Lab"), new SealedDog("REX", "Lab"), true },
        { new SealedDog("Rex", "Lab"), new SealedDog("Fido", "Lab"), false },
        { new SealedDog("Rex", "Lab"), new SealedDog("Rex", "Terrier"), false },
    };

    [Theory]
    [MemberData(nameof(SealedCases))]
    public void SealedOverManualBaseEquality(SealedDog a, SealedDog b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // [Equatable(IgnoreInheritedMembers = true)] over a hand-written base: delegation and the bridge
    // must be fully suppressed — only the type's own members are compared, the base is ignored.
    [Equatable(IgnoreInheritedMembers = true)]
    public partial class IgnoringDog : Animal
    {
        public IgnoringDog(string name, string breed) : base(name) => Breed = breed;

        public string Breed { get; }
    }

    public static TheoryData<IgnoringDog, IgnoringDog, bool> IgnoreInheritedCases => new()
    {
        // Base Name differs but is IGNORED; same Breed -> equal
        { new IgnoringDog("Rex", "Lab"), new IgnoringDog("Fido", "Lab"), true },
        // Own member (Breed) differs -> not equal
        { new IgnoringDog("Rex", "Lab"), new IgnoringDog("Rex", "Terrier"), false },
        { new IgnoringDog("Rex", "Lab"), new IgnoringDog("Rex", "Lab"), true },
    };

    [Theory]
    [MemberData(nameof(IgnoreInheritedCases))]
    public void IgnoreInheritedMembersOverManualBaseEquality(IgnoringDog a, IgnoringDog b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // Base providing value equality via IEquatable<TSelf> ONLY (a public typed Equals + GetHashCode,
    // NO Equals(object) override). The generator now detects this and delegates via the typed cast
    // base.Equals(other as IEquatableBird) -> IEquatableBird.Equals(IEquatableBird), honoring its
    // case-insensitive semantics, rather than falling back to property comparison.
    public class IEquatableBird : IEquatable<IEquatableBird>
    {
        public IEquatableBird(string species) => Species = species;

        public string Species { get; }

        public bool Equals(IEquatableBird? other) =>
            other is not null && string.Equals(Species, other.Species, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Species);
    }

    [Equatable]
    public partial class Sparrow : IEquatableBird
    {
        public Sparrow(string species, int wingspan) : base(species) => Wingspan = wingspan;

        public int Wingspan { get; }
    }

    public static TheoryData<Sparrow, Sparrow, bool> IEquatableOnlyCases => new()
    {
        { new Sparrow("Robin", 10), new Sparrow("Robin", 10), true },
        // Base Species differs only by case -> equal only if the base's IEquatable is delegated to
        { new Sparrow("Robin", 10), new Sparrow("ROBIN", 10), true },
        { new Sparrow("Robin", 10), new Sparrow("Wren", 10), false },
        // Own member (Wingspan) differs -> not equal
        { new Sparrow("Robin", 10), new Sparrow("Robin", 20), false },
    };

    [Theory]
    [MemberData(nameof(IEquatableOnlyCases))]
    public void IEquatableOnlyBaseEquality(Sparrow a, Sparrow b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // IEquatable<TSelf>-only GRANDPARENT behind a plain intermediate. The typed Equals lives on the
    // grandparent, not the immediate base, so delegation must cast to the grandparent's type; casting
    // to the plain intermediate (or object) would bind to object.Equals and compare by reference.
    public class PlainPerch : IEquatableBird
    {
        public PlainPerch(string species, int height) : base(species) => Height = height;

        public int Height { get; }
    }

    [Equatable]
    public partial class Finch : PlainPerch
    {
        public Finch(string species, int height, string color) : base(species, height) => Color = color;

        public string Color { get; }
    }

    public static TheoryData<Finch, Finch, bool> IEquatableGrandparentCases => new()
    {
        { new Finch("Robin", 3, "red"), new Finch("Robin", 3, "red"), true },
        // Grandparent Species differs only by case -> equal only if its IEquatable is delegated to
        { new Finch("Robin", 3, "red"), new Finch("ROBIN", 3, "red"), true },
        { new Finch("Robin", 3, "red"), new Finch("Wren", 3, "red"), false },
        // Plain intermediate Height differs -> not equal (collected member)
        { new Finch("Robin", 3, "red"), new Finch("Robin", 4, "red"), false },
        // Own member (Color) differs -> not equal
        { new Finch("Robin", 3, "red"), new Finch("Robin", 3, "blue"), false },
    };

    [Theory]
    [MemberData(nameof(IEquatableGrandparentCases))]
    public void IEquatableOnlyGrandparentEquality(Finch a, Finch b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    // One combined source so a single snapshot captures every shape (the snapshot file name is keyed
    // to this test file, so all scenarios must share one VerifyGeneratedSource call):
    //  - ManualBaseDog            : delegates to a complete hand-written base
    //  - ManualLeaf               : delegates to a manual grandparent, collects the plain parent's member
    //  - EqualsOnlyChild (CS0661) : incomplete base contract -> falls back to property comparison
    //  - GetHashCodeOnlyChild (CS0659): incomplete base contract -> falls back to property comparison
    //  - SealedManualDog          : sealed [Equatable] over a hand-written base (private Equals, bridge)
    //  - IgnoringManualDog        : IgnoreInheritedMembers=true over a hand-written base (no delegation)
    //  - ManualSparrow            : IEquatable<TSelf>-only base, delegated via the typed cast
    //  - ManualFinch              : IEquatable<TSelf>-only grandparent behind a plain parent (typed cast to the grandparent)
    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                // Complete hand-written contract (no [Equatable]) -> delegated to.
                                public class ManualBaseAnimal
                                {
                                    public ManualBaseAnimal(string name) => Name = name;

                                    public string Name { get; }

                                    public override bool Equals(object? obj) =>
                                        obj is ManualBaseAnimal other && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

                                    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
                                }

                                [Equatable]
                                public partial class ManualBaseDog : ManualBaseAnimal
                                {
                                    public ManualBaseDog(string name, string breed) : base(name) => Breed = breed;

                                    public string Breed { get; }
                                }

                                // Manual grandparent behind a plain parent: parent member collected, grandparent delegated.
                                public class ManualGrandBase
                                {
                                    public ManualGrandBase(string tag) => Tag = tag;

                                    public string Tag { get; }

                                    public override bool Equals(object? obj) =>
                                        obj is ManualGrandBase other && string.Equals(Tag, other.Tag, StringComparison.OrdinalIgnoreCase);

                                    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Tag);
                                }

                                public class ManualPlainMiddle : ManualGrandBase
                                {
                                    public ManualPlainMiddle(string tag, int level) : base(tag) => Level = level;

                                    public int Level { get; }
                                }

                                [Equatable]
                                public partial class ManualLeaf : ManualPlainMiddle
                                {
                                    public ManualLeaf(string tag, int level, string name) : base(tag, level) => Name = name;

                                    public string Name { get; }
                                }

                                // Base overrides Equals but NOT GetHashCode (CS0661): incomplete contract -> NOT delegated,
                                // falls back to comparing the base's public properties so the generated pair stays consistent.
                                public class EqualsOnlyBase
                                {
                                    public EqualsOnlyBase(string name) => Name = name;

                                    public string Name { get; }

                                    public override bool Equals(object? obj) =>
                                        obj is EqualsOnlyBase other && Name == other.Name;
                                }

                                [Equatable]
                                public partial class EqualsOnlyChild : EqualsOnlyBase
                                {
                                    public EqualsOnlyChild(string name, int id) : base(name) => Id = id;

                                    public int Id { get; }
                                }

                                // Base overrides GetHashCode but NOT Equals (CS0659): incomplete contract -> same fallback.
                                public class GetHashCodeOnlyBase
                                {
                                    public GetHashCodeOnlyBase(string name) => Name = name;

                                    public string Name { get; }

                                    public override int GetHashCode() => Name.GetHashCode();
                                }

                                [Equatable]
                                public partial class GetHashCodeOnlyChild : GetHashCodeOnlyBase
                                {
                                    public GetHashCodeOnlyChild(string name, int id) : base(name) => Id = id;

                                    public int Id { get; }
                                }

                                // Complete hand-written base for the sealed / ignore-inherited shapes below.
                                public class ManualCritter
                                {
                                    public ManualCritter(string name) => Name = name;

                                    public string Name { get; }

                                    public override bool Equals(object? obj) =>
                                        obj is ManualCritter other && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

                                    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
                                }

                                // Sealed [Equatable] over a hand-written base: private Equals, x.Equals(y) dispatch,
                                // base.Equals((object?) other) delegation, and the __BaseEquals bridge.
                                [Equatable]
                                public sealed partial class SealedManualDog : ManualCritter
                                {
                                    public SealedManualDog(string name, string breed) : base(name) => Breed = breed;

                                    public string Breed { get; }
                                }

                                // IgnoreInheritedMembers=true over a hand-written base: no base delegation, no bridge;
                                // only the type's own members are compared after the runtime-type check.
                                [Equatable(IgnoreInheritedMembers = true)]
                                public partial class IgnoringManualDog : ManualCritter
                                {
                                    public IgnoringManualDog(string name, string breed) : base(name) => Breed = breed;

                                    public string Breed { get; }
                                }

                                // Value equality via IEquatable<TSelf> ONLY (public typed Equals + GetHashCode, no
                                // Equals(object) override): delegated to via the typed cast, not the object fallback.
                                public class IEquatableBird : System.IEquatable<IEquatableBird>
                                {
                                    public IEquatableBird(string species) => Species = species;

                                    public string Species { get; }

                                    public bool Equals(IEquatableBird? other) =>
                                        other is not null && string.Equals(Species, other.Species, StringComparison.OrdinalIgnoreCase);

                                    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Species);
                                }

                                [Equatable]
                                public partial class ManualSparrow : IEquatableBird
                                {
                                    public ManualSparrow(string species, int wingspan) : base(species) => Wingspan = wingspan;

                                    public int Wingspan { get; }
                                }

                                // IEquatable<TSelf>-only grandparent behind a plain intermediate: the typed cast targets the
                                // grandparent (IEquatableBird), not the immediate base, and the parent's member is collected.
                                public class ManualPlainPerch : IEquatableBird
                                {
                                    public ManualPlainPerch(string species, int height) : base(species) => Height = height;

                                    public int Height { get; }
                                }

                                [Equatable]
                                public partial class ManualFinch : ManualPlainPerch
                                {
                                    public ManualFinch(string species, int height, string color) : base(species, height) => Color = color;

                                    public string Color { get; }
                                }
                                """;
}
