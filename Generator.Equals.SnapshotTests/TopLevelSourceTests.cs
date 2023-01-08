using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.SnapshotTests
{
    [UsesVerify]
    public class TopLevelSourceTests : SnapshotTestBase
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

            var topLevelSource = File.ReadAllText(topLevelProgramPath);

            await VerifyGeneratedSource(
                Path.Combine(Path.GetDirectoryName(ThisPath())!, "TopLevel")!,
                Path.GetFileName(topLevelProgramPath),
                topLevelSource,
                outputKind: OutputKind.ConsoleApplication);
        }
    }
}