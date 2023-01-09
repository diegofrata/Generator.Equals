//HintName: StructSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

partial struct StructSample : global::System.IEquatable<StructSample>
{
    /// <summary>
    /// Indicates whether the object on the left is equal to the object on the right.
    /// </summary>
    /// <param name="left">The left object</param>
    /// <param name="right">The right object</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public static bool operator ==(
        global::StructSample left,
        global::StructSample right) =>
        global::Generator.Equals.DefaultEqualityComparer<global::StructSample>.Default
            .Equals(left, right);
    
    /// <summary>
    /// Indicates whether the object on the left is not equal to the object on the right.
    /// </summary>
    /// <param name="left">The left object</param>
    /// <param name="right">The right object</param>
    /// <returns>true if the objects are not equal; otherwise, false.</returns>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public static bool operator !=(global::StructSample left, global::StructSample right) =>
        !(left == right);
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public override bool Equals(object? obj) =>
        obj is global::StructSample o && Equals(o);
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public bool Equals(global::StructSample other)
    {
        return true
            && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
            && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Age!, other.Age!)
            ;
    }
    
    /// <inheritdoc/>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
    public override int GetHashCode()
    {
        var hashCode = new global::System.HashCode();
        
        hashCode.Add(
            this.Name!,
            global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
        hashCode.Add(
            this.Age!,
            global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default);
        
        return hashCode.ToHashCode();
    }
}
