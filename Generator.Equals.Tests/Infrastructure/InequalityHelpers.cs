namespace Generator.Equals.Tests.Infrastructure;

/// <summary>
/// Shared helpers for inequality test assertions.
/// Import via: using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;
/// </summary>
public static class InequalityHelpers
{
    public static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);
    public static MemberPathSegment Fld(string name) => MemberPathSegment.Field(name);
    public static MemberPathSegment Idx(int i) => MemberPathSegment.Index(i);
    public static MemberPathSegment DKey(object k) => MemberPathSegment.Key(k);

    public static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);
}
