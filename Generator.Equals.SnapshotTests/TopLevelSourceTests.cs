using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.SnapshotTests
{
    [UsesVerify]
    public class TopLevelSourceTests
    {
        static string ThisPath([CallerFilePath] string path = null!) => path;

        [Fact]
        public async Task Check()
        {
            var topLevelProgramPath = Path.GetFullPath(
                Path.Combine(
                    ThisPath(),
                    "../../Generator.Equals.Tests.TopLevel/Program.cs")
            );

            var topLevelSource = await File.ReadAllTextAsync(topLevelProgramPath);

            await GeneratedSourceTests.VerifyGeneratedSource(
                Path.Combine(Path.GetDirectoryName(ThisPath())!, "TopLevel")!,
                Path.GetFileName(topLevelProgramPath),
                topLevelSource,
                outputKind: OutputKind.ConsoleApplication);
        }
    }
}