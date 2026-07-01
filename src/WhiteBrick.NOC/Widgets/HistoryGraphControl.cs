using Avalonia;
using Avalonia.Media;
using System.Collections;

namespace WhiteBrick.NOC.Widgets;

public sealed class HistoryGraphControl : AnimatedWidgetBase
{
    public static readonly StyledProperty<IEnumerable?> ValuesProperty =
        AvaloniaProperty.Register<HistoryGraphControl, IEnumerable?>(nameof(Values));

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<HistoryGraphControl, string>(nameof(Label), "");

    public IEnumerable? Values
    {
        get => GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var w = Bounds.Width;
        var h = Bounds.Height;
        context.FillRectangle(new SolidColorBrush(Color.FromArgb(74, 9, 12, 24)), Bounds);

        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(38, 155, 130, 255)), 1);
        for (int i = 1; i < 5; i++)
        {
            var y = h * i / 5;
            context.DrawLine(gridPen, new Point(0, y), new Point(w, y));
        }

        var nums = new List<double>();
        if (Values is not null)
        {
            foreach (var v in Values)
            {
                if (v is double d) nums.Add(d);
                else if (double.TryParse(v?.ToString(), out var parsed)) nums.Add(parsed);
            }
        }

        if (nums.Count >= 2)
        {
            var min = nums.Min();
            var max = nums.Max();
            if (Math.Abs(max - min) < 0.1) max = min + 1;

            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                for (int i = 0; i < nums.Count; i++)
                {
                    var x = w * i / Math.Max(1, nums.Count - 1);
                    var y = h - ((nums[i] - min) / (max - min)) * (h * 0.72) - h * 0.14;
                    if (i == 0) ctx.BeginFigure(new Point(x, y), false);
                    else ctx.LineTo(new Point(x, y));
                }
            }

            context.DrawGeometry(null, new Pen(new SolidColorBrush(Color.FromArgb(105, 166, 126, 255)), 6), geo);
            context.DrawGeometry(null, new Pen(new SolidColorBrush(Color.FromArgb(245, 184, 168, 255)), 2), geo);
        }

        if (!string.IsNullOrWhiteSpace(Label))
        {
            var text = new FormattedText(Label, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Typeface.Default, 12, new SolidColorBrush(Color.FromArgb(180, 217, 247, 255)));
            context.DrawText(text, new Point(10, 8));
        }
    }
}
