using System.Runtime.CompilerServices;
using DiffEngine;

namespace Generator.Equals.SnapshotTests
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init()
        {
            DiffTools.UseOrder(DiffTool.VisualStudioCode);
            VerifySourceGenerators.Enable();
            ClipboardAccept.Enable();
        }
    }
}