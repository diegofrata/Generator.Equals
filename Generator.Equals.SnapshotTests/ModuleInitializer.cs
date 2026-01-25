using System.Runtime.CompilerServices;
using DiffEngine;

namespace Generator.Equals.SnapshotTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        DiffTools.UseOrder(DiffTool.VisualStudioCode);
        VerifySourceGenerators.Initialize();
        ClipboardAccept.Enable();
    }
}
