//HintName: ClassSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

partial class ClassSample : global::System.IEquatable<ClassSample>
{
    /// <summary>
    /// Indicates whether the object on the left is equal to the object on the right.
    /// </summary>
    /// <param name="left">The left object</param>
    /// <param name="right">The right object</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public static bool operator ==(
        global::ClassSample? left,
        global::ClassSample? right) =>
        global::Generator.Equals.DefaultEqualityComparer<global::ClassSample?>.Default
            .Equals(left, right);
    
    /// <summary>
    /// Indicates whether the object on the left is not equal to the object on the right.
    /// </summary>
    /// <param name="left">The left object</param>
    /// <param name="right">The right object</param>
    /// <returns>true if the objects are not equal; otherwise, false.</returns>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public static bool operator !=(global::ClassSample? left, global::ClassSample? right) =>
        !(left == right);
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public override bool Equals(object? obj) =>
        Equals(obj as global::ClassSample);
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    bool global::System.IEquatable<global::ClassSample>.Equals(global::ClassSample? obj) => Equals((object?) obj);
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    protected bool Equals(global::ClassSample? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return other.GetType() == this.GetType()
            && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
            && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Age!, other.Age!)
            ;
    }
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public override int GetHashCode()
    {
        var hashCode = new global::System.HashCode();
        
        hashCode.Add(this.GetType());
        hashCode.Add(
            this.Name!,
            global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
        hashCode.Add(
            this.Age!,
            global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default);
        
        return hashCode.ToHashCode();
    }
}
