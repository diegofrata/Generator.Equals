using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.SnapshotTests
{
    [UsesVerify]
    public class GeneratedSourceTests : SnapshotTestBase
    {
        static IEnumerable<object[]> FindSampleFiles([CallerFilePath] string? path = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var testProjectPath = Path.GetFullPath(Path.Combine(path, "../../Generator.Equals.Tests"));
            var files = Directory.EnumerateFiles(testProjectPath, "*.Sample.cs", SearchOption.AllDirectories);

            return Enum.GetValues<TargetFramework>().SelectMany(framework =>
            {
                return files.Select(f =>
                {
                    return new object[]
                    {
                        Path.Combine(Path.GetDirectoryName(path)!, Directory.GetParent(f)!.Name),
                        Path.GetFileName(f).Replace(".Sample.cs", ""),
                        f,
                        framework
                    };
                });
            });
        }

        public static IEnumerable<object[]> SampleFiles => FindSampleFiles();

        [Theory]
        [MemberData(nameof(SampleFiles))]
        public async Task Check(string directory, string fileName, string filePath, TargetFramework targetFramework)
        {
            var sample = File.ReadAllText(filePath);
            await VerifyGeneratedSource(directory, fileName, sample, targetFramework: targetFramework);
        }
    }
}