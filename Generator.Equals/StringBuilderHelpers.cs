using System.Text;

namespace Generator.Equals
{
    public static class StringBuilderHelpers
    {
        public static StringBuilder AppendMargin(
            this StringBuilder builder,
            int level)
        {
            for (var i = 0; i < level; i++)
            {
                builder.Append("    ");
            }

            return builder;
        }

        public static StringBuilder AppendLine(
            this StringBuilder builder,
            int level)
        {
            builder.AppendMargin(level);
            builder.AppendLine();

            return builder;
        }

        public static StringBuilder AppendLine(
            this StringBuilder builder,
            int level,
            string value)
        {
            builder.AppendMargin(level);
            builder.AppendLine(value);

            return builder;
        }

        public static StringBuilder AppendLine(
            this StringBuilder builder,
            int level,
            string[] values)
        {
            foreach(var value in values)
            {
                builder.AppendMargin(level);
                builder.AppendLine(value);
            }

            return builder;
        }

        public static StringBuilder AppendOpenBracket(
            this StringBuilder builder,
            ref int level)
        {
            builder.AppendLine(level, "{");
            level++;

            return builder;
        }

        public static StringBuilder AppendCloseBracket(
            this StringBuilder builder,
            ref int level)
        {
            level--;
            builder.AppendLine(level, "}");

            return builder;
        }
    }
}