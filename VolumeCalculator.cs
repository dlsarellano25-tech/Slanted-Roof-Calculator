namespace RoofBlockCalculator;

/// <summary>
/// Dimensions of a Pentahedral Slanted-Roof Block:
///   - A rectangular prism base of length L, width W, height h_base.
///   - A slanted-roof wedge on top with two opposite edges of
///     potentially different heights h1 (front) and h2 (back).
/// </summary>
public record SolidDimensions(
    double Length,
    double Width,
    double BaseHeight,
    double RoofHeight1,
    double RoofHeight2
);

/// <summary>
/// Result returned to the client.
/// </summary>
public record CalculationResult(
    double BasePrismVolume,
    double SlantedRoofVolume,
    double TotalVolume,
    string Formula
);

/// <summary>
/// Pure calculation logic for the Pentahedral Slanted-Roof Block.
/// Kept in its own class so it can be unit-tested independently of the web layer.
///
/// Formula:
///   V = L * W * ( h_base + (h1 + h2) / 2 )
/// </summary>
public static class VolumeCalculator
{
    public const string FormulaText = "V = L * W * (h_base + (h1 + h2) / 2)";

    /// <summary>
    /// Validates the user-supplied dimensions. Returns false with a message
    /// if any value is out of range; true and a null message otherwise.
    /// </summary>
    public static bool TryValidate(SolidDimensions d, out string? error)
    {
        if (d.Length <= 0 || d.Width <= 0)
        {
            error = "Length and width must be greater than zero.";
            return false;
        }

        if (d.BaseHeight < 0 || d.RoofHeight1 < 0 || d.RoofHeight2 < 0)
        {
            error = "Heights cannot be negative.";
            return false;
        }

        if (d.RoofHeight1 == 0 && d.RoofHeight2 == 0 && d.BaseHeight == 0)
        {
            error = "At least one height must be greater than zero.";
            return false;
        }

        error = null;
        return true;
    }

    /// <summary>
    /// Computes the volume of the rectangular prism base.
    ///   V_base = L * W * h_base
    /// </summary>
    public static double BasePrismVolume(SolidDimensions d)
        => d.Length * d.Width * d.BaseHeight;

    /// <summary>
    /// Computes the volume of the slanted-roof wedge on top.
    /// The wedge is a prism whose cross-section is a trapezoid with
    /// parallel sides h1 and h2 and width W, extruded along length L:
    ///   V_roof = L * W * (h1 + h2) / 2
    /// </summary>
    public static double SlantedRoofVolume(SolidDimensions d)
        => d.Length * d.Width * (d.RoofHeight1 + d.RoofHeight2) / 2.0;

    /// <summary>
    /// Computes the total volume of the Pentahedral Slanted-Roof Block.
    /// </summary>
    public static CalculationResult Calculate(SolidDimensions d)
    {
        double basePrism   = BasePrismVolume(d);
        double slantedRoof = SlantedRoofVolume(d);
        double total       = basePrism + slantedRoof;

        return new CalculationResult(
            BasePrismVolume:   Math.Round(basePrism,   4),
            SlantedRoofVolume: Math.Round(slantedRoof, 4),
            TotalVolume:       Math.Round(total,       4),
            Formula:           FormulaText
        );
    }
}
