using RunwayLighting.Core;

namespace RunwayLighting.Tests;

public class BrightnessAdvisorTests
{
    private readonly BrightnessAdvisor _advisor = new();

    private static RunwayConfiguration SingleCircuit(CircuitType type) =>
        new("Test runway", [new Circuit("C1", "Circuit 1", type)]);

    [Theory]
    [InlineData(10000, 1)]
    [InlineData(5000, 1)]
    [InlineData(3000, 2)]
    [InlineData(1000, 3)]
    [InlineData(500, 4)]
    [InlineData(200, 5)]
    public void Night_step_increases_as_visibility_drops(int visibility, int expectedStep)
    {
        var result = _advisor.Recommend(
            SingleCircuit(CircuitType.RunwayEdge), new VisibilityConditions(visibility, IsNight: true));

        Assert.Equal(expectedStep, result.Single().Step);
    }

    [Fact]
    public void Daylight_adds_one_step()
    {
        var result = _advisor.Recommend(
            SingleCircuit(CircuitType.RunwayEdge), new VisibilityConditions(3000, IsNight: false));

        Assert.Equal(3, result.Single().Step);
    }

    [Fact]
    public void Daylight_never_exceeds_the_top_step()
    {
        var result = _advisor.Recommend(
            SingleCircuit(CircuitType.RunwayEdge), new VisibilityConditions(100, IsNight: false));

        Assert.Equal(5, result.Single().Step);
    }

    [Fact]
    public void Taxiway_runs_one_step_below_the_runway()
    {
        var result = _advisor.Recommend(
            SingleCircuit(CircuitType.Taxiway), new VisibilityConditions(1000, IsNight: true));

        Assert.Equal(2, result.Single().Step);
    }

    [Fact]
    public void Every_circuit_of_the_configuration_gets_a_recommendation()
    {
        var result = _advisor.Recommend(SampleConfigurations.Standard, new VisibilityConditions(2000, IsNight: true));

        Assert.Equal(SampleConfigurations.Standard.Circuits.Count, result.Count);
    }

    [Fact]
    public void Label_uses_the_B_prefix()
    {
        var result = _advisor.Recommend(
            SingleCircuit(CircuitType.Approach), new VisibilityConditions(200, IsNight: true));

        Assert.Equal("B5", result.Single().Label);
    }

    [Fact]
    public void Negative_visibility_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new VisibilityConditions(-1, IsNight: true));
    }
}
