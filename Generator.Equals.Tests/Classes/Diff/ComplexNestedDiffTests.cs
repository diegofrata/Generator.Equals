using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for deeply nested object graphs with multiple levels of [Equatable] types,
/// ordered lists, unordered sets, and dictionaries — demonstrating that Diff can detect
/// and collect changes at every level of the hierarchy.
/// </summary>
public partial class ComplexNestedDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);
    static MemberPathSegment Idx(int i) => MemberPathSegment.Index(i);
    static MemberPathSegment DKey(object k) => MemberPathSegment.Key(k);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

    // ── Level 3: leaf type ──────────────────────────────────────────────
    [Equatable]
    public partial class Employee
    {
        public string? Name { get; set; }

        [StringEquality(StringComparison.OrdinalIgnoreCase)]
        public string? Role { get; set; }
    }

    // ── Level 2: mid-level aggregate ────────────────────────────────────
    [Equatable]
    public partial class Department
    {
        public string? Name { get; set; }

        [OrderedEquality]
        public List<Employee>? Staff { get; set; }

        [UnorderedEquality]
        public Dictionary<string, int>? Budget { get; set; }

        [SetEquality]
        public HashSet<string>? Tags { get; set; }
    }

    // ── Level 1: top-level root ─────────────────────────────────────────
    [Equatable]
    public partial class Organisation
    {
        public string? Name { get; set; }

        [OrderedEquality]
        public List<Department>? Departments { get; set; }

        [UnorderedEquality]
        public Dictionary<string, string>? Settings { get; set; }
    }

    // ── helpers ─────────────────────────────────────────────────────────
    static Employee Emp(string name, string role) => new() { Name = name, Role = role };

    static Department Dept(string name, List<Employee>? staff = null,
        Dictionary<string, int>? budget = null, HashSet<string>? tags = null) =>
        new() { Name = name, Staff = staff, Budget = budget, Tags = tags };

    #region Leaf (Employee) Diff

    [Fact]
    public void Employee_SameValues_NoDiffs()
    {
        var a = Emp("Alice", "Engineer");
        var b = Emp("Alice", "ENGINEER"); // case-insensitive role

        Employee.EqualityComparer.Default.Inequalities(a, b).Should().BeEmpty();
    }

    [Fact]
    public void Employee_DifferentName_ReportsDiff()
    {
        var a = Emp("Alice", "Engineer");
        var b = Emp("Bob", "Engineer");

        Employee.EqualityComparer.Default.Inequalities(a, b).ToList()
            .Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    [Fact]
    public void Employee_DifferentRole_CaseSensitiveMismatch_ReportsDiff()
    {
        var a = Emp("Alice", "Engineer");
        var b = Emp("Alice", "Manager");

        Employee.EqualityComparer.Default.Inequalities(a, b).ToList()
            .Should().BeEquivalentTo(new[] { Ineq("Engineer", "Manager", Prop("Role")) });
    }

    #endregion

    #region Mid-level (Department) Diff — lists, dictionaries, sets

    [Fact]
    public void Department_SameEverything_NoDiffs()
    {
        var a = Dept("Engineering",
            staff: [Emp("Alice", "Lead"), Emp("Bob", "IC")],
            budget: new() { ["Q1"] = 100_000, ["Q2"] = 120_000 },
            tags: ["backend", "infra"]);
        var b = Dept("Engineering",
            staff: [Emp("Alice", "Lead"), Emp("Bob", "IC")],
            budget: new() { ["Q2"] = 120_000, ["Q1"] = 100_000 }, // order irrelevant
            tags: ["infra", "backend"]); // order irrelevant

        Department.EqualityComparer.Default.Inequalities(a, b).Should().BeEmpty();
    }

    [Fact]
    public void Department_StaffMemberChanged_ReportsIndex()
    {
        var alice = Emp("Alice", "Lead");
        var bob = Emp("Bob", "IC");
        var charlie = Emp("Charlie", "IC");

        var a = Dept("Engineering", staff: [alice, bob]);
        var b = Dept("Engineering", staff: [alice, charlie]);

        var diffs = Department.EqualityComparer.Default.Inequalities(a, b).ToList();

        // Ordered list with equatable elements drills down to per-property diffs
        diffs.Should().BeEquivalentTo(new[] { Ineq("Bob", "Charlie", Prop("Staff"), Idx(1), Prop("Name")) });
    }

    [Fact]
    public void Department_BudgetAndTagsDiffer_ReportsAll()
    {
        var a = Dept("Engineering",
            staff: [Emp("Alice", "Lead")],
            budget: new() { ["Q1"] = 100_000, ["Q2"] = 120_000 },
            tags: ["backend", "infra"]);
        var b = Dept("Engineering",
            staff: [Emp("Alice", "Lead")],
            budget: new() { ["Q1"] = 100_000, ["Q3"] = 90_000 },
            tags: ["backend", "frontend"]);

        var diffs = Department.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            // Dictionary: Q2 removed, Q3 added
            Ineq(120_000, null, Prop("Budget"), DKey("Q2")),
            Ineq(null, 90_000, Prop("Budget"), DKey("Q3")),
            // Set: infra removed, frontend added
            Ineq("infra", null, Prop("Tags"), MemberPathSegment.Removed()),
            Ineq(null, "frontend", Prop("Tags"), MemberPathSegment.Added())
        });
    }

    [Fact]
    public void Department_NameStaffBudgetAllDiffer_ReportsEverything()
    {
        var a = Dept("Engineering",
            staff: [Emp("Alice", "Lead"), Emp("Bob", "IC")],
            budget: new() { ["Q1"] = 100_000 },
            tags: ["backend"]);
        var b = Dept("Sales",
            staff: [Emp("Alice", "Lead")],
            budget: new() { ["Q1"] = 50_000, ["Q2"] = 60_000 },
            tags: ["backend", "outbound"]);

        var diffs = Department.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Engineering", "Sales", Prop("Name")),
            // Staff[1] is length mismatch — reports whole object
            Ineq(Emp("Bob", "IC"), null, Prop("Staff"), Idx(1)),
            Ineq(100_000, 50_000, Prop("Budget"), DKey("Q1")),
            Ineq(null, 60_000, Prop("Budget"), DKey("Q2")),
            Ineq(null, "outbound", Prop("Tags"), MemberPathSegment.Added())
        });
    }

    #endregion

    #region Top-level (Organisation) Diff — nested equatable objects in collections

    [Fact]
    public void Organisation_AllSame_NoDiffs()
    {
        var eng = Dept("Engineering", staff: [Emp("Alice", "Lead")],
            budget: new() { ["Q1"] = 100_000 });
        var sales = Dept("Sales", staff: [Emp("Bob", "Rep")],
            budget: new() { ["Q1"] = 50_000 });

        var a = new Organisation
        {
            Name = "Acme",
            Departments = [eng, sales],
            Settings = new() { ["theme"] = "dark", ["region"] = "US" }
        };
        var b = new Organisation
        {
            Name = "Acme",
            Departments =
            [
                Dept("Engineering", staff: [Emp("Alice", "Lead")],
                    budget: new() { ["Q1"] = 100_000 }),
                Dept("Sales", staff: [Emp("Bob", "Rep")],
                    budget: new() { ["Q1"] = 50_000 })
            ],
            Settings = new() { ["region"] = "US", ["theme"] = "dark" }
        };

        Organisation.EqualityComparer.Default.Inequalities(a, b).Should().BeEmpty();
    }

    [Fact]
    public void Organisation_SettingsChanged_ReportsKeys()
    {
        var dept = Dept("Engineering");

        var a = new Organisation
        {
            Name = "Acme",
            Departments = [dept],
            Settings = new() { ["theme"] = "dark", ["region"] = "US" }
        };
        var b = new Organisation
        {
            Name = "Acme",
            Departments = [Dept("Engineering")],
            Settings = new() { ["theme"] = "light", ["locale"] = "en-GB" }
        };

        var diffs = Organisation.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("dark", "light", Prop("Settings"), DKey("theme")),
            Ineq("US", null, Prop("Settings"), DKey("region")),
            Ineq(null, "en-GB", Prop("Settings"), DKey("locale"))
        });
    }

    [Fact]
    public void Organisation_DepartmentSwapped_ReportsIndices()
    {
        var eng = Dept("Engineering", staff: [Emp("Alice", "Lead")]);
        var sales = Dept("Sales", staff: [Emp("Bob", "Rep")]);

        var a = new Organisation
        {
            Name = "Acme",
            Departments = [eng, sales],
            Settings = new() { ["x"] = "1" }
        };
        var b = new Organisation
        {
            Name = "Acme",
            Departments =
            [
                Dept("Sales", staff: [Emp("Bob", "Rep")]),
                Dept("Engineering", staff: [Emp("Alice", "Lead")])
            ],
            Settings = new() { ["x"] = "1" }
        };

        var diffs = Organisation.EqualityComparer.Default.Inequalities(a, b).ToList();

        // Ordered list with equatable elements: drills down to per-property diffs
        diffs.Should().BeEquivalentTo(new[]
        {
            // Index 0: Engineering → Sales
            Ineq("Engineering", "Sales", Prop("Departments"), Idx(0), Prop("Name")),
            Ineq("Alice", "Bob", Prop("Departments"), Idx(0), Prop("Staff"), Idx(0), Prop("Name")),
            Ineq("Lead", "Rep", Prop("Departments"), Idx(0), Prop("Staff"), Idx(0), Prop("Role")),
            // Index 1: Sales → Engineering
            Ineq("Sales", "Engineering", Prop("Departments"), Idx(1), Prop("Name")),
            Ineq("Bob", "Alice", Prop("Departments"), Idx(1), Prop("Staff"), Idx(0), Prop("Name")),
            Ineq("Rep", "Lead", Prop("Departments"), Idx(1), Prop("Staff"), Idx(0), Prop("Role")),
        });
    }

    #endregion

    #region Composing Diff across levels — drill-down pattern

    [Fact]
    public void DrillDown_OrganisationAutomaticallyDrillsIntoNestedEquatableTypes()
    {
        // Identical department
        var sharedDept = Dept("Sales", staff: [Emp("Bob", "Rep")],
            budget: new() { ["Q1"] = 50_000 });

        // Department that differs between the two organisations
        var engA = Dept("Engineering",
            staff: [Emp("Alice", "Lead"), Emp("Carol", "IC")],
            budget: new() { ["Q1"] = 100_000, ["Q2"] = 120_000 },
            tags: ["backend"]);

        var engB = Dept("Engineering",
            staff: [Emp("Alice", "Lead"), Emp("Dave", "IC")],
            budget: new() { ["Q1"] = 110_000, ["Q3"] = 90_000 },
            tags: ["backend", "platform"]);

        var orgA = new Organisation
        {
            Name = "Acme",
            Departments = [engA, sharedDept],
            Settings = new() { ["region"] = "US" }
        };
        var orgB = new Organisation
        {
            Name = "Acme",
            Departments = [engB, Dept("Sales", staff: [Emp("Bob", "Rep")],
                budget: new() { ["Q1"] = 50_000 })],
            Settings = new() { ["region"] = "US" }
        };

        // Single-step: Organisation Diff automatically drills down through all nested [Equatable] types
        var diffs = Organisation.EqualityComparer.Default.Inequalities(orgA, orgB).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            // Staff[1] drilled down: Carol → Dave (only Name differs, Role "IC" matches case-insensitive)
            Ineq("Carol", "Dave", Prop("Departments"), Idx(0), Prop("Staff"), Idx(1), Prop("Name")),
            // Budget Q1 changed, Q2 removed, Q3 added
            Ineq(100_000, 110_000, Prop("Departments"), Idx(0), Prop("Budget"), DKey("Q1")),
            Ineq(120_000, null, Prop("Departments"), Idx(0), Prop("Budget"), DKey("Q2")),
            Ineq(null, 90_000, Prop("Departments"), Idx(0), Prop("Budget"), DKey("Q3")),
            // Tags: platform added
            Ineq(null, "platform", Prop("Departments"), Idx(0), Prop("Tags"), MemberPathSegment.Added()),
        });
    }

    [Fact]
    public void DrillDown_WithBasePath_ProducesFullyQualifiedPaths()
    {
        var engA = Dept("Engineering",
            staff: [Emp("Alice", "Lead")],
            budget: new() { ["Q1"] = 100_000 });
        var engB = Dept("Engineering",
            staff: [Emp("Alice", "Manager")], // role change (case-sensitive mismatch)
            budget: new() { ["Q1"] = 100_000, ["Q2"] = 50_000 });

        // Department-level diff with a path prefix simulating the parent context
        var basePath = new MemberPath(new[] { Prop("Org"), Prop("Departments"), Idx(0) });
        var deptDiffs = Department.EqualityComparer.Default
            .Inequalities(engA, engB, basePath)
            .ToList();

        // Auto drill-down: Staff[0] drills into Employee and reports Role change directly
        deptDiffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Lead", "Manager", Prop("Org"), Prop("Departments"), Idx(0), Prop("Staff"), Idx(0), Prop("Role")),
            Ineq(null, 50_000, Prop("Org"), Prop("Departments"), Idx(0), Prop("Budget"), DKey("Q2"))
        });
    }

    #endregion
}
