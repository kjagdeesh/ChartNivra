namespace ChartNivra.Rendering.Skia
{
    using ChartNivra.Core.Charts;
    using ChartNivra.Core.Primitives;
    using ChartNivra.Core.Themes;
    using ChartNivra.Core.Transforms;
    using ChartNivra.Annotations;
    using ChartNivra.Rendering.Abstractions;

    public class AnnotatedCartesianRenderer : IChartRenderer
    {
        private readonly CartesianChartModel _model;
        private readonly List<AnnotationBase> _annotations;

        public AnnotatedCartesianRenderer(CartesianChartModel model, List<AnnotationBase> annotations)
        {
            _model = model;
            _annotations = annotations;
        }

        public void Render(IChartCanvas canvas, ChartRect bounds)
        {
            var baseRenderer = new CartesianChartRenderer(_model);
            baseRenderer.Render(canvas, bounds);

            // Compute transform for annotations
            var (xRange, yRange, niceMinY, niceMaxY, stepY) = _model.ComputeRanges();
            var topMargin = string.IsNullOrEmpty(_model.Title) ? 20.0 : 50.0;
            var plotArea = new ChartRect(
                bounds.Left + 50,
                bounds.Top + topMargin,
                Math.Max(10, bounds.Width - 75),
                Math.Max(10, bounds.Height - topMargin - 40));

            var transform = new CartesianTransform(xRange, yRange, plotArea);

            canvas.Save();
            canvas.ClipRect(plotArea);

            foreach (var annotation in _annotations)
            {
                annotation.Render(canvas, transform);
            }

            canvas.Restore();
        }
    }
}