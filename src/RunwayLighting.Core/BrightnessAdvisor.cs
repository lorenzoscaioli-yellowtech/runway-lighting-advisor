namespace RunwayLighting.Core;

/// <summary>
/// Suggests a brightness step for every circuit of a runway, given the current visibility.
/// The rules are described in docs/requisiti.md.
/// </summary>
public sealed class BrightnessAdvisor
{
    private const int MaxStep = 5;

    public IReadOnlyList<BrightnessRecommendation> Recommend(
        RunwayConfiguration configuration, VisibilityConditions conditions)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(conditions);

        var baseStep = BaseStep(conditions);

        return configuration.Circuits
            .Select(circuit => new BrightnessRecommendation(circuit, AdjustForCircuitType(baseStep, circuit.Type)))
            .ToList();
    }

    /// <summary>Step on the 5-step scale: lower visibility needs brighter lights.</summary>
    internal static int BaseStep(VisibilityConditions conditions)
    {
        var nightStep = conditions.VisibilityMeters switch
        {
            >= 5000 => 1,
            >= 1500 => 2,
            >= 800 => 3,
            >= 400 => 4,
            _ => 5,
        };

        // In daylight the lights must stand out against the ambient light.
        return conditions.IsNight ? nightStep : Math.Min(nightStep + 1, MaxStep);
    }

    /// <summary>Taxiway lights run one step below runway lights.</summary>
    internal static int AdjustForCircuitType(int step, CircuitType type) =>
        type == CircuitType.Taxiway ? Math.Max(step - 1, 1) : step;
}
