using System;
using System.Text;

namespace Generator.Equals
{
    public readonly struct MemberPath : IEquatable<MemberPath>
    {
        readonly MemberPathSegment[]? _segments;

        public MemberPathSegment[] Segments => _segments ?? Array.Empty<MemberPathSegment>();

        public MemberPath(MemberPathSegment[] segments)
        {
            _segments = segments;
        }

        public MemberPath Append(MemberPathSegment segment)
        {
            var existing = Segments;
            var result = new MemberPathSegment[existing.Length + 1];
            Array.Copy(existing, result, existing.Length);
            result[existing.Length] = segment;
            return new MemberPath(result);
        }

        public MemberPath Append(MemberPath other)
        {
            var left = Segments;
            var right = other.Segments;
            if (left.Length == 0) return other;
            if (right.Length == 0) return this;
            var result = new MemberPathSegment[left.Length + right.Length];
            Array.Copy(left, result, left.Length);
            Array.Copy(right, 0, result, left.Length, right.Length);
            return new MemberPath(result);
        }

        public override string ToString()
        {
            var segments = Segments;
            if (segments.Length == 0) return "";

            var sb = new StringBuilder();
            for (var i = 0; i < segments.Length; i++)
            {
                var seg = segments[i];
                if (seg.Kind == MemberPathSegmentKind.Property)
                {
                    if (i > 0) sb.Append('.');
                    sb.Append(seg.ToString());
                }
                else
                {
                    sb.Append(seg.ToString());
                }
            }

            return sb.ToString();
        }

        public bool Equals(MemberPath other)
        {
            var a = Segments;
            var b = other.Segments;
            if (a.Length != b.Length) return false;
            for (var i = 0; i < a.Length; i++)
            {
                if (!a[i].Equals(b[i])) return false;
            }

            return true;
        }

        public override bool Equals(object? obj) => obj is MemberPath other && Equals(other);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var seg in Segments)
                hashCode.Add(seg);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(MemberPath left, MemberPath right) => left.Equals(right);
        public static bool operator !=(MemberPath left, MemberPath right) => !left.Equals(right);
    }
}
