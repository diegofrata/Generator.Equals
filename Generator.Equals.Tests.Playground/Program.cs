using System.Collections.Immutable;
using System.Data;
using Generator.Equals;

var customerA = new Customer
{
    Id = Guid.NewGuid(),
    Name = "John Doe",
    Addresses = ImmutableDictionary<string, Address>.Empty.Add(
        "A",
        new Address
        {
            Street = "123 Main St",
            City = "Seattle"
        }
    )
};

var mutatedCustomerA = customerA with
{
    Name = "Johnny Doe",
    Addresses = customerA.Addresses.SetItem("A", customerA.Addresses["A"] with { Street = "121 Main St" })
};


var diff = Customer.EqualityComparer.Default.Inequalities(customerA, mutatedCustomerA);

foreach (var item in diff)
{
    Console.WriteLine(item.Path.);
}

readonly record struct Index(int Row, int Col);


[Equatable]
partial record Customer
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    [UnorderedEquality] public required ImmutableDictionary<string, Address> Addresses { get; init; }
}

[Equatable]
partial record Address
{
    public required string Street { get; init; }
    public required string City { get; init; }
}