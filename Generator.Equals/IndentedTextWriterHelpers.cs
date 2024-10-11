using System.CodeDom.Compiler;

namespace Generator.Equals
{
    public static class IndentedTextWriterHelpers
    {
        public static void WriteLine(
            this IndentedTextWriter writer,
            int offset,
            string value)
        {
            _ = writer ?? throw new System.ArgumentNullException(nameof(writer));

            writer.Indent += offset;
            writer.WriteLine(value);
            writer.Indent -= offset;
        }

        public static void WriteLines(
            this IndentedTextWriter writer,
            string[] lines)
        {
            _ = writer ?? throw new System.ArgumentNullException(nameof(writer));
            _ = lines ?? throw new System.ArgumentNullException(nameof(lines));

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }

        public static void AppendOpenBracket(
            this IndentedTextWriter writer)
        {
            _ = writer ?? throw new System.ArgumentNullException(nameof(writer));

            writer.WriteLine("{");
            writer.Indent++;
        }

        public static void AppendCloseBracket(
            this IndentedTextWriter writer)
        {
            _ = writer ?? throw new System.ArgumentNullException(nameof(writer));

            writer.Indent--;
            writer.WriteLine("}");
        }

        public static void UnwindOpenedBrackets(
            this IndentedTextWriter writer)
        {
            _ = writer ?? throw new System.ArgumentNullException(nameof(writer));

            while (writer.Indent != 0)
            {
                AppendCloseBracket(writer);
            }
        }
    }
}