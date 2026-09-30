using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace PressureTestCalculator;

public partial class MainWindow : Window
{
    private readonly (double psi, double secPerPsi)[] refs =
    {
        (500, 240), (1000, 120), (3000, 40),
        (5000, 24), (10000, 12), (15000, 8)
    };

    public MainWindow()
    {
        InitializeComponent();
        Calculate();
    }

    private double Interpolate(double psi)
    {
        if (psi <= refs[0].psi) return refs[0].secPerPsi;
        if (psi >= refs[^1].psi) return refs[^1].secPerPsi;

        for (int i = 0; i < refs.Length - 1; i++)
        {
            var a = refs[i];
            var b = refs[i + 1];
            if (psi >= a.psi && psi <= b.psi)
            {
                return a.secPerPsi +
                       (psi - a.psi) / (b.psi - a.psi) *
                       (b.secPerPsi - a.secPerPsi);
            }
        }
        throw new InvalidOperationException();
    }

    private void Calculate_Click(object sender, RoutedEventArgs e) => Calculate();

    private void Calculate()
    {
        if (!double.TryParse(PressureBox.Text.Replace(",", ""),
            NumberStyles.Float, CultureInfo.InvariantCulture, out var psi) || psi <= 0)
        {
            ShowError("Enter a valid test pressure.");
            return;
        }

        if (!double.TryParse(DurationBox.Text.Replace(",", ""),
            NumberStyles.Float, CultureInfo.InvariantCulture, out var minutes) || minutes <= 0)
        {
            ShowError("Enter a valid test duration.");
            return;
        }

        var secPerPsi = Interpolate(psi);
        var psiPerMinute = 60.0 / secPerPsi;

        // The reference table expresses the allowable drop as:
        // 0.05% per minute, equivalent to 3% over 60 minutes.
        // This is applied to the entered test duration.
        var allowablePercent = 0.05 * minutes;
        var maxDrop = psi * allowablePercent / 100.0;

        PressureResult.Text = $"Test pressure: {psi:N0} psi";
        DurationResult.Text = $"Test duration: {minutes:0.##} minutes";
        WaitResult.Text = $"Estimated wait: {secPerPsi:0.##} sec/psi";
        RateResult.Text = $"Estimated rate: {psiPerMinute:0.###} psi/min";
        DropResult.Text = $"Estimated maximum drop: {maxDrop:0.##} psi ({allowablePercent:0.##}%)";
    }

    private void ShowError(string message)
    {
        PressureResult.Text = message;
        DurationResult.Text = WaitResult.Text = RateResult.Text = DropResult.Text = "";
    }
}
