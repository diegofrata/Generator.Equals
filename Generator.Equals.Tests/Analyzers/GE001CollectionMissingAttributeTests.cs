extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE001: Collection property missing equality attribute.
/// </summary>
public sealed class GE001CollectionMissingAttributeTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task ListProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public List<int> Items { get; set; }
            }
            """;

        // List<int> is 9 chars, starts at col 12, ends at col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ArrayProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public int[] Numbers { get; set; }
            }
            """;

        // int[] is 5 chars, starts at col 12, ends at col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(6, 12, 6, 17)
                .WithArguments("Numbers"));
    }

    [Fact]
    public async Task CollectionProperty_WithOrderedEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithUnorderedEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithSetEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [SetEquality]
                public HashSet<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithIgnoreEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [IgnoreEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithReferenceEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [ReferenceEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ExplicitMode_CollectionWithDefaultEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                [DefaultEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(8, 12, 8, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ExplicitMode_CollectionWithoutDefaultEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StringProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task DictionaryProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public Dictionary<string, int> Lookup { get; set; }
            }
            """;

        // Dictionary<string, int> is 23 chars, starts at col 12, ends at col 35
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 35)
                .WithArguments("Lookup"));
    }

    [Fact]
    public async Task IEnumerableProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public IEnumerable<int> Items { get; set; }
            }
            """;

        // IEnumerable<int> is 16 chars, starts at col 12, ends at col 28
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 28)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task CollectionProperty_WithDefaultEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [DefaultEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [DefaultEquality] is an explicit opt-in to the default comparer - the user has stated
        // their intent, so GE001 stays quiet (same escape hatch as GE002/GE003)
        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithDefaultAndOrderedEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [DefaultEquality]
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [DefaultEquality] + [OrderedEquality] satisfies the requirement
        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CustomCollectionProperty_WithEquatable_NoDiagnostic()
    {
        const string source = """
            using System.Collections;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public MyCollection<int> Items { get; set; }
            }

            [Equatable]
            public partial class MyCollection<T> : IEnumerable<T>
            {
                [OrderedEquality]
                private readonly List<T> _list = new();
                public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                public void Add(T item) => _list.Add(item);
            }
            """;

        // The collection generates its own structural equality, which the default comparer
        // already delegates to - no collection attribute needed
        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CustomCollectionProperty_WithHandWrittenIEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using System.Collections;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public MyCollection Items { get; set; }
            }

            public class MyCollection : IEnumerable<int>, IEquatable<MyCollection>
            {
                private readonly List<int> _list = new();
                public IEnumerator<int> GetEnumerator() => _list.GetEnumerator();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                public bool Equals(MyCollection other) => ReferenceEquals(this, other);
            }
            """;

        // IEquatable<T> alone says nothing about the semantics - it may well be reference-based,
        // as it is here. The user opts out explicitly with [DefaultEquality] instead.
        // MyCollection is 12 chars, starts at col 12, ends at col 24
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(9, 12, 9, 24)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ImmutableArrayProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Immutable;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public ImmutableArray<int> Items { get; set; }
            }
            """;

        // ImmutableArray<T> implements IEquatable<ImmutableArray<T>> by comparing the underlying
        // array by reference - GE001 must still fire here
        // ImmutableArray<int> is 19 chars, starts at col 12, ends at col 31
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 31)
                .WithArguments("Items"));
    }
}
