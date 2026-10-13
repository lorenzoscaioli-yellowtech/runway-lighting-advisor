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

/// <summary>
/// A lighting circuit fed by one constant current regulator (CCR).
/// Older regulators expose 3 brightness steps, newer ones expose 5.
/// </summary>
public sealed record Circuit
{
    public Circuit(string id, string name, CircuitType type, int regulatorSteps)
    {
        if (regulatorSteps is not (3 or 5))
        {
            throw new ArgumentOutOfRangeException(nameof(regulatorSteps), regulatorSteps,
                "A regulator exposes either 3 or 5 brightness steps.");
        }

        Id = id;
        Name = name;
        Type = type;
        RegulatorSteps = regulatorSteps;
    }

    public string Id { get; }
    public string Name { get; }
    public CircuitType Type { get; }
    public int RegulatorSteps { get; }
}

/// <summary>The set of circuits configured for one runway.</summary>
public sealed record RunwayConfiguration(string Name, IReadOnlyList<Circuit> Circuits);

/// <summary>Current conditions on the airfield.</summary>
public sealed record VisibilityConditions(int VisibilityMeters, bool IsNight)
{
    public int VisibilityMeters { get; } = VisibilityMeters >= 0
        ? VisibilityMeters
        : throw new ArgumentOutOfRangeException(nameof(VisibilityMeters), "Visibility cannot be negative.");
}

/// <summary>Brightness step suggested for a circuit, on the circuit's own regulator scale.</summary>
public sealed record BrightnessRecommendation(Circuit Circuit, int Step)
{
    public string Label => $"B{Step}";
}
