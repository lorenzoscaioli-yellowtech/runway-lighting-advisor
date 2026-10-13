namespace RunwayLighting.Core;

/// <summary>Kind of lighting circuit installed on the airfield.</summary>
public enum CircuitType
{
    Approach,
    Threshold,
    RunwayEdge,
    RunwayCentreline,
    Taxiway,
}

/// <summary>A lighting circuit fed by one constant current regulator (CCR).</summary>
public sealed record Circuit(string Id, string Name, CircuitType Type);

/// <summary>The set of circuits configured for one runway.</summary>
public sealed record RunwayConfiguration(string Name, IReadOnlyList<Circuit> Circuits);

/// <summary>Current conditions on the airfield.</summary>
public sealed record VisibilityConditions(int VisibilityMeters, bool IsNight)
{
    public int VisibilityMeters { get; } = VisibilityMeters >= 0
        ? VisibilityMeters
        : throw new ArgumentOutOfRangeException(nameof(VisibilityMeters), "Visibility cannot be negative.");
}

/// <summary>Brightness step suggested for a circuit.</summary>
public sealed record BrightnessRecommendation(Circuit Circuit, int Step)
{
    public string Label => $"B{Step}";
}
