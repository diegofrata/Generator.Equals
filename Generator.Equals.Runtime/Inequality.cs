using System;

namespace Generator.Equals
{
    public readonly struct Inequality : IEquatable<Inequality>
    {
        public MemberPath Path { get; }
        public object? Left { get; }
        public object? Right { get; }

        public Inequality(MemberPath path, object? left, object? right)
        {
            Path = path;
            Left = left;
            Right = right;
        }

        public bool Equals(Inequality other)
        {
            return Path.Equals(other.Path)
                && Equals(Left, other.Left)
                && Equals(Right, other.Right);
        }

        public override bool Equals(object? obj) => obj is Inequality other && Equals(other);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Path);
            hashCode.Add(Left);
            hashCode.Add(Right);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(Inequality left, Inequality right) => left.Equals(right);
        public static bool operator !=(Inequality left, Inequality right) => !left.Equals(right);

        public override string ToString() => $"{Path}: {Left} \u2192 {Right}";
    }
}
