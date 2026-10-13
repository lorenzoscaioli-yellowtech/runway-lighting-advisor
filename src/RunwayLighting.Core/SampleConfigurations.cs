namespace RunwayLighting.Core;

/// <summary>Invented runway configurations used by the web app. Not real airports.</summary>
public static class SampleConfigurations
{
    public static RunwayConfiguration Standard { get; } = new(
        "Pista 12/30 — impianto standard",
        [
            new Circuit("APP-12", "Avvicinamento 12", CircuitType.Approach, 5),
            new Circuit("THR-12", "Soglia 12", CircuitType.Threshold, 5),
            new Circuit("EDG-12", "Bordo pista", CircuitType.RunwayEdge, 5),
            new Circuit("CTL-12", "Asse pista", CircuitType.RunwayCentreline, 5),
            new Circuit("TWY-A", "Taxiway A", CircuitType.Taxiway, 5),
        ]);

    public static RunwayConfiguration Mixed { get; } = new(
        "Pista 09/27 — impianto misto",
        [
            new Circuit("APP-09", "Avvicinamento 09", CircuitType.Approach, 5),
            new Circuit("THR-09", "Soglia 09", CircuitType.Threshold, 5),
            new Circuit("EDG-09", "Bordo pista", CircuitType.RunwayEdge, 3),
            new Circuit("CTL-09", "Asse pista", CircuitType.RunwayCentreline, 5),
            new Circuit("TWY-B", "Taxiway B", CircuitType.Taxiway, 3),
        ]);

    public static IReadOnlyList<RunwayConfiguration> All { get; } = [Standard, Mixed];
}
