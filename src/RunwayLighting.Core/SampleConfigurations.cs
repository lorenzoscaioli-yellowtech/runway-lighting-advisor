namespace RunwayLighting.Core;

/// <summary>Invented runway configurations used by the web app. Not real airports.</summary>
public static class SampleConfigurations
{
    public static RunwayConfiguration Standard { get; } = new(
        "Pista 12/30 — impianto standard",
        [
            new Circuit("APP-12", "Avvicinamento 12", CircuitType.Approach),
            new Circuit("THR-12", "Soglia 12", CircuitType.Threshold),
            new Circuit("EDG-12", "Bordo pista", CircuitType.RunwayEdge),
            new Circuit("CTL-12", "Asse pista", CircuitType.RunwayCentreline),
            new Circuit("TWY-A", "Taxiway A", CircuitType.Taxiway),
        ]);

    public static IReadOnlyList<RunwayConfiguration> All { get; } = [Standard];
}
