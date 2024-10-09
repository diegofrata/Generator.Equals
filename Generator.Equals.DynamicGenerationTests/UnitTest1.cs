using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using SourceGeneratorTestHelpers;

namespace Generator.Equals.DynamicGenerationTests;

public class UnitTest1
{
    public static readonly List<PortableExecutableReference> References2 =
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(new[]
            {
                // add your app/lib specifics, e.g.:
                MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
            })
            .ToList();

    [Fact]
    public void Test1()
    {
        var input = SourceText.CSharp(
            """
            using System;
            using Generator.Equals;

             namespace Tests;

             [Equatable]
             public partial record MyRecord(
                 [property: OrderedEquality] string[] Fruits,
                 [property: StringEquality(StringComparison.OrdinalIgnoreCase)] string Fruit
             );

            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions()
            {
            },
            References2
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);
    }

    [Fact]
    public void Test2_Nested()
    {
        var input = SourceText.CSharp(
            """
            using System;
            using System.Collections.Generic;
            using Generator.Equals;


            namespace Generator.Equals.Tests.Records
            {
                public partial class CustomEquality
                {
                    [Equatable]
                    public partial record Sample
                    {
                        public Sample(string name1, string name2, string name3)
                        {
                            Name1 = name1;
                            Name2 = name2;
                            Name3 = name3;
                        }

                        [CustomEquality(typeof(Comparer1))] public string Name1 { get; }
                        [CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] public string Name2 { get; }
                        [CustomEquality(typeof(LengthEqualityComparer))] public string Name3 { get; }
                    }

                    class Comparer1
                    {
                        public static readonly LengthEqualityComparer Default = new();
                    }

                    class Comparer2
                    {
                        public static readonly LengthEqualityComparer Instance = new();
                    }

                    class LengthEqualityComparer : IEqualityComparer<string>
                    {
                        public bool Equals(string? x, string? y) => x?.Length == y?.Length;

                        public int GetHashCode(string obj) => obj.Length.GetHashCode();
                    }

                }
            }
            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions()
            {
            },
            References2
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);
    }

    [Fact]
    public void Test3_Struct_UnorderedEquality()
    {
        var input = SourceText.CSharp(
            """
            using System.Collections.Generic;

            namespace Generator.Equals.Tests.Structs
            {
                public partial class UnorderedEquality
                {
                    [Equatable]
                    public partial struct Sample
                    {
                        [UnorderedEquality] public List<int>? Properties { get; set; }
                    }
                }
            }
            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions()
            {
            },
            References2
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);
    }

    [Fact]
    public void Test3_Struct_ExplicitMode()
    {
        var input = SourceText.CSharp(
            """
            namespace Generator.Equals.Tests.Structs
            {
                public partial class ExplicitMode
                {
                    [Equatable(Explicit = true)]
                    public partial struct Sample
                    {
                        bool _flag;

                        public Sample(string name, int age, bool flag)
                        {
                            _flag = flag;
                            Name = name;
                            Age = age;
                        }

                        public string Name { get; }

                        [DefaultEquality]
                        public int Age { get; }
                    }
                }
            }
            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions()
            {
            },
            References2
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);
    }
}