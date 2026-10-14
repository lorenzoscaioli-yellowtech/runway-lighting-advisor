using RunwayLighting.Core;

namespace RunwayLighting.Tests;

/// <summary>
/// Executable specification for the rules documented in docs/requisiti.md.
/// The expectations in this class are deliberately expressed through the public API.
/// </summary>
public class RequirementsComplianceTests
{
    private readonly BrightnessAdvisor _advisor = new();

    private static RunwayConfiguration SingleCircuit(CircuitType type, int regulatorSteps) =>
        new("Test runway", [new Circuit("C1", "Circuit 1", type, regulatorSteps)]);

    private int RecommendStep(
        int visibilityMeters,
        bool isNight = true,
        CircuitType type = CircuitType.RunwayEdge,
        int regulatorSteps = 5) =>
        _advisor.Recommend(
            SingleCircuit(type, regulatorSteps),
            new VisibilityConditions(visibilityMeters, isNight)).Single().Step;

    [Theory]
    [InlineData(int.MaxValue, 1)]
    [InlineData(5000, 1)]
    [InlineData(4999, 2)]
    [InlineData(1500, 2)]
    [InlineData(1499, 3)]
    [InlineData(800, 3)]
    [InlineData(799, 4)]
    [InlineData(400, 4)]
    [InlineData(399, 5)]
    [InlineData(0, 5)]
    public void Night_reference_scale_honours_every_visibility_boundary(
        int visibilityMeters,
        int expectedStep)
    {
        Assert.Equal(expectedStep, RecommendStep(visibilityMeters));
    }

    [Theory]
    [InlineData(5000, 2)]
    [InlineData(1500, 3)]
    [InlineData(800, 4)]
    [InlineData(400, 5)]
    [InlineData(0, 5)]
    public void Day_is_one_step_brighter_and_is_capped_at_B5(
        int visibilityMeters,
        int expectedStep)
    {
        Assert.Equal(expectedStep, RecommendStep(visibilityMeters, isNight: false));
    }

    [Theory]
    [InlineData(CircuitType.Approach)]
    [InlineData(CircuitType.Threshold)]
    [InlineData(CircuitType.RunwayEdge)]
    [InlineData(CircuitType.RunwayCentreline)]
    public void Non_taxiway_circuit_types_use_the_runway_reference_step(CircuitType type)
    {
        Assert.Equal(3, RecommendStep(1000, type: type));
    }

    [Theory]
    [InlineData(5000, 1)]
    [InlineData(1500, 1)]
    [InlineData(800, 2)]
    [InlineData(400, 3)]
    [InlineData(0, 4)]
    public void Taxiway_is_one_step_below_runway_with_a_B1_floor(
        int visibilityMeters,
        int expectedStep)
    {
        Assert.Equal(expectedStep, RecommendStep(visibilityMeters, type: CircuitType.Taxiway));
    }

    [Theory]
    [InlineData(5000, 1)] // ceil(1 * 3 / 5)
    [InlineData(1500, 2)] // ceil(2 * 3 / 5)
    [InlineData(800, 2)]  // ceil(3 * 3 / 5)
    [InlineData(400, 3)]  // ceil(4 * 3 / 5)
    [InlineData(0, 3)]    // ceil(5 * 3 / 5)
    public void Three_step_regulator_scales_proportionally_and_rounds_up(
        int visibilityMeters,
        int expectedStep)
    {
        var step = RecommendStep(visibilityMeters, regulatorSteps: 3);

        Assert.Equal(expectedStep, step);
        Assert.InRange(step, 1, 3);
    }

    [Fact]
    public void Standard_configuration_recommends_all_circuits_on_their_five_step_scale()
    {
        var recommendations = _advisor.Recommend(
            SampleConfigurations.Standard,
            new VisibilityConditions(5000, IsNight: false));

        Assert.Collection(
            recommendations,
            recommendation => AssertRecommendation(recommendation, "APP-12", 2, 5),
            recommendation => AssertRecommendation(recommendation, "THR-12", 2, 5),
            recommendation => AssertRecommendation(recommendation, "EDG-12", 2, 5),
            recommendation => AssertRecommendation(recommendation, "CTL-12", 2, 5),
            recommendation => AssertRecommendation(recommendation, "TWY-A", 1, 5));
    }

    [Fact]
    public void Mixed_configuration_uses_each_circuits_own_regulator_scale()
    {
        var recommendations = _advisor.Recommend(
            SampleConfigurations.Mixed,
            new VisibilityConditions(0, IsNight: true));

        Assert.Collection(
            recommendations,
            recommendation => AssertRecommendation(recommendation, "APP-09", 5, 5),
            recommendation => AssertRecommendation(recommendation, "THR-09", 5, 5),
            recommendation => AssertRecommendation(recommendation, "EDG-09", 3, 3),
            recommendation => AssertRecommendation(recommendation, "CTL-09", 5, 5),
            recommendation => AssertRecommendation(recommendation, "TWY-B", 3, 3));
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    public void Negative_visibility_is_rejected_for_day_and_night(int visibilityMeters)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new VisibilityConditions(visibilityMeters, IsNight: true));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new VisibilityConditions(visibilityMeters, IsNight: false));
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(int.MaxValue)]
    public void Any_regulator_size_other_than_three_or_five_is_rejected(int regulatorSteps)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Circuit("C1", "Circuit 1", CircuitType.RunwayEdge, regulatorSteps));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public void Supported_regulator_sizes_are_accepted(int regulatorSteps)
    {
        var circuit = new Circuit("C1", "Circuit 1", CircuitType.RunwayEdge, regulatorSteps);

        Assert.Equal(regulatorSteps, circuit.RegulatorSteps);
    }

    private static void AssertRecommendation(
        BrightnessRecommendation recommendation,
        string expectedCircuitId,
        int expectedStep,
        int expectedRegulatorSteps)
    {
        Assert.Equal(expectedCircuitId, recommendation.Circuit.Id);
        Assert.Equal(expectedStep, recommendation.Step);
        Assert.Equal($"B{expectedStep}", recommendation.Label);
        Assert.Equal(expectedRegulatorSteps, recommendation.Circuit.RegulatorSteps);
    }
}
