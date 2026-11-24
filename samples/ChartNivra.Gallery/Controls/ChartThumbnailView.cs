namespace ChartNivra.Gallery.Controls;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

public enum ChartThumbnailKind
{
    Line, Bar, Area, Bubble, Step, Range, Waterfall, Pie, Donut, Sunburst, Radar, Heatmap,
    Treemap, Candlestick, Indicator, Histogram, BoxPlot, Gauge, Bullet, Funnel, ThreeD, Surface
}

/// <summary>Small platform-neutral vector previews used by the dashboard catalog.</summary>
public sealed class ChartThumbnailView : GraphicsView
{
    public ChartThumbnailView(ChartThumbnailKind kind)
    {
        HeightRequest = 126;
        Drawable = new ThumbnailDrawable(kind);
    }

    private sealed class ThumbnailDrawable(ChartThumbnailKind kind) : IDrawable
    {
        private static readonly Color Blue = Color.FromArgb("#0EA5E9");
        private static readonly Color Indigo = Color.FromArgb("#6366F1");
        private static readonly Color Green = Color.FromArgb("#22C55E");
        private static readonly Color Orange = Color.FromArgb("#F97316");
        private static readonly Color Red = Color.FromArgb("#EF4444");
        private static readonly Color Grid = Color.FromArgb("#E2E8F0");

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var x = dirtyRect.X + 16;
            var y = dirtyRect.Y + 14;
            var w = Math.Max(1, dirtyRect.Width - 32);
            var h = Math.Max(1, dirtyRect.Height - 28);
            canvas.Antialias = true;

            switch (kind)
            {
                case ChartThumbnailKind.Bar: Bars(canvas, x, y, w, h, false); break;
                case ChartThumbnailKind.Histogram: Bars(canvas, x, y, w, h, true); break;
                case ChartThumbnailKind.Area: Area(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Bubble: Bubbles(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Step: Step(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Range: Range(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Waterfall: Waterfall(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Pie: Pie(canvas, x, y, w, h, false); break;
                case ChartThumbnailKind.Donut: Pie(canvas, x, y, w, h, true); break;
                case ChartThumbnailKind.Sunburst: Sunburst(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Radar: Radar(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Heatmap: Heatmap(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Treemap: Treemap(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Candlestick: Candles(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Indicator: Indicators(canvas, x, y, w, h); break;
                case ChartThumbnailKind.BoxPlot: BoxPlot(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Gauge: Gauge(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Bullet: Bullet(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Funnel: Funnel(canvas, x, y, w, h); break;
                case ChartThumbnailKind.ThreeD: ThreeD(canvas, x, y, w, h); break;
                case ChartThumbnailKind.Surface: Surface(canvas, x, y, w, h); break;
                default: Line(canvas, x, y, w, h); break;
            }
        }

        private static void Axes(ICanvas c, float x, float y, float w, float h)
        {
            c.StrokeColor = Grid; c.StrokeSize = 1;
            c.DrawLine(x, y + h * .84f, x + w, y + h * .84f);
            c.DrawLine(x + w * .08f, y, x + w * .08f, y + h * .84f);
        }

        private static void Line(ICanvas c, float x, float y, float w, float h)
        {
            Axes(c, x, y, w, h); c.StrokeColor = Blue; c.StrokeSize = 3;
            var p = new[] { .10f, .32f, .58f, .43f, .70f, .32f, .50f, .24f };
            for (var i = 0; i < p.Length - 1; i++) c.DrawLine(x + w * (i + .5f) / p.Length, y + h * p[i], x + w * (i + 1.5f) / p.Length, y + h * p[i + 1]);
            c.StrokeColor = Indigo; c.StrokeSize = 2;
            for (var i = 0; i < p.Length - 1; i++) c.DrawLine(x + w * (i + .5f) / p.Length, y + h * (p[i] + .12f), x + w * (i + 1.5f) / p.Length, y + h * (p[i + 1] + .12f));
        }

        private static void Bars(ICanvas c, float x, float y, float w, float h, bool histogram)
        {
            Axes(c, x, y, w, h); var values = histogram ? new[] { .18f, .35f, .62f, .82f, .66f, .40f, .20f } : new[] { .52f, .30f, .62f, .18f, .42f, .25f, .58f };
            var bw = w / (values.Length * 1.55f);
            c.FillColor = Blue;
            for (var i = 0; i < values.Length; i++) c.FillRoundedRectangle(x + w * .14f + i * bw * 1.35f, y + h * values[i], bw, h * (.84f - values[i]), 3);
        }

        private static void Area(ICanvas c, float x, float y, float w, float h)
        {
            Axes(c, x, y, w, h); var path = new PathF(); path.MoveTo(x + w * .1f, y + h * .7f); path.LineTo(x + w * .28f, y + h * .35f); path.LineTo(x + w * .5f, y + h * .5f); path.LineTo(x + w * .72f, y + h * .18f); path.LineTo(x + w * .94f, y + h * .46f); path.LineTo(x + w * .94f, y + h * .84f); path.LineTo(x + w * .1f, y + h * .84f); path.Close(); c.FillColor = Color.FromArgb("#BDEBFD"); c.FillPath(path); c.StrokeColor = Indigo; c.StrokeSize = 3; c.DrawPath(path);
        }

        private static void Bubbles(ICanvas c, float x, float y, float w, float h)
        {
            Axes(c, x, y, w, h); var points = new[] { (.28f, .58f, 6f, Blue), (.45f, .36f, 9f, Blue), (.66f, .52f, 11f, Indigo), (.82f, .25f, 7f, Blue), (.77f, .68f, 8f, Green) };
            foreach (var p in points) { c.FillColor = p.Item4; c.FillCircle(x + w * p.Item1, y + h * p.Item2, p.Item3); }
        }

        private static void Step(ICanvas c, float x, float y, float w, float h) { Axes(c, x, y, w, h); c.StrokeColor = Orange; c.StrokeSize = 3; var a=x+w*.12f; c.DrawLine(a,y+h*.72f,a+w*.18f,y+h*.72f); c.DrawLine(a+w*.18f,y+h*.72f,a+w*.18f,y+h*.4f); c.DrawLine(a+w*.18f,y+h*.4f,a+w*.44f,y+h*.4f); c.DrawLine(a+w*.62f,y+h*.4f,a+w*.62f,y+h*.2f); c.DrawLine(a+w*.62f,y+h*.2f,a+w*.78f,y+h*.2f); }
        private static void Range(ICanvas c, float x, float y, float w, float h) { Axes(c,x,y,w,h); c.FillColor=Color.FromArgb("#BAE6FD"); c.FillRectangle(x+w*.1f,y+h*.30f,w*.82f,h*.34f); c.StrokeColor=Blue;c.StrokeSize=2;c.DrawLine(x+w*.1f,y+h*.3f,x+w*.92f,y+h*.35f);c.DrawLine(x+w*.1f,y+h*.64f,x+w*.92f,y+h*.6f); }
        private static void Waterfall(ICanvas c, float x, float y, float w, float h) { Axes(c,x,y,w,h); var vals=new[] {(.56f,.28f,Blue),(.42f,.12f,Green),(.30f,.12f,Green),(.44f,.10f,Red),(.26f,.38f,Blue)}; for(var i=0;i<vals.Length;i++){c.FillColor=vals[i].Item3;c.FillRoundedRectangle(x+w*(.14f+i*.16f),y+h*vals[i].Item1,w*.1f,h*vals[i].Item2,3);} }
        private static void Pie(ICanvas c, float x, float y, float w, float h, bool donut)
        {
            var cx = x + w * .5f; var cy = y + h * .46f; var r = Math.Min(w, h) * .31f;
            FillWedge(c, cx, cy, r, -90, 118, Blue);
            FillWedge(c, cx, cy, r, 28, 88, Indigo);
            FillWedge(c, cx, cy, r, 116, 94, Green);
            FillWedge(c, cx, cy, r, 210, 60, Orange);
            if (donut) { c.FillColor = Colors.White; c.FillCircle(cx, cy, r * .48f); }
        }

        private static void Sunburst(ICanvas c, float x, float y, float w, float h)
        {
            var cx = x + w * .5f; var cy = y + h * .46f; var r = Math.Min(w, h) * .38f;
            FillWedge(c, cx, cy, r, -90, 74, Blue); FillWedge(c, cx, cy, r, -16, 66, Color.FromArgb("#38BDF8"));
            FillWedge(c, cx, cy, r, 50, 94, Indigo); FillWedge(c, cx, cy, r, 144, 76, Green); FillWedge(c, cx, cy, r, 220, 50, Color.FromArgb("#A78BFA"));
            c.FillColor = Colors.White; c.FillCircle(cx, cy, r * .42f);
            FillWedge(c, cx, cy, r * .42f, -90, 130, Blue); FillWedge(c, cx, cy, r * .42f, 40, 112, Green); FillWedge(c, cx, cy, r * .42f, 152, 118, Indigo);
            c.FillColor = Colors.White; c.FillCircle(cx, cy, r * .18f);
        }
        private static void Radar(ICanvas c,float x,float y,float w,float h){var cx=x+w*.5f;var cy=y+h*.46f;var r=Math.Min(w,h)*.30f;c.StrokeColor=Grid;c.StrokeSize=1;for(var i=0;i<5;i++){var a=(float)(Math.PI*2*i/5-Math.PI/2);c.DrawLine(cx,cy,cx+r*(float)Math.Cos(a),cy+r*(float)Math.Sin(a));}c.StrokeColor=Indigo;c.StrokeSize=3;for(var i=0;i<5;i++){var a=(float)(Math.PI*2*i/5-Math.PI/2);var b=(float)(Math.PI*2*((i+1)%5)/5-Math.PI/2);c.DrawLine(cx+r*.8f*(float)Math.Cos(a),cy+r*.8f*(float)Math.Sin(a),cx+r*.8f*(float)Math.Cos(b),cy+r*.8f*(float)Math.Sin(b));}}
        private static void Heatmap(ICanvas c,float x,float y,float w,float h){var colors=new[]{Color.FromArgb("#DBEAFE"),Color.FromArgb("#93C5FD"),Color.FromArgb("#3B82F6"),Color.FromArgb("#1D4ED8")};for(var row=0;row<4;row++)for(var col=0;col<7;col++){c.FillColor=colors[(row*3+col*2)%4];c.FillRoundedRectangle(x+w*.1f+col*w*.115f,y+h*.18f+row*h*.16f,w*.09f,h*.11f,2);}}
        private static void Treemap(ICanvas c,float x,float y,float w,float h){var xx=x+w*.12f;var yy=y+h*.14f;c.FillColor=Blue;c.FillRoundedRectangle(xx,yy,w*.42f,h*.58f,3);c.FillColor=Indigo;c.FillRoundedRectangle(xx+w*.45f,yy,w*.22f,h*.36f,3);c.FillColor=Green;c.FillRoundedRectangle(xx+w*.7f,yy,w*.16f,h*.22f,3);c.FillColor=Color.FromArgb("#7DD3FC");c.FillRoundedRectangle(xx,yy+h*.62f,w*.42f,h*.18f,3);c.FillColor=Color.FromArgb("#C4B5FD");c.FillRoundedRectangle(xx+w*.45f,yy+h*.4f,w*.22f,h*.4f,3);}
        private static void Candles(ICanvas c,float x,float y,float w,float h){Axes(c,x,y,w,h);for(var i=0;i<6;i++){var xx=x+w*(.18f+i*.12f);var up=i%3!=2;c.StrokeColor=up?Green:Red;c.StrokeSize=2;c.DrawLine(xx,y+h*(up?.25f:.35f),xx,y+h*(up?.7f:.78f));c.FillColor=up?Green:Red;c.FillRectangle(xx-w*.025f,y+h*(up?.4f:.48f),w*.05f,h*.2f);}}
        private static void Indicators(ICanvas c,float x,float y,float w,float h){Range(c,x,y,w,h);c.StrokeColor=Orange;c.StrokeSize=2;c.DrawLine(x+w*.1f,y+h*.48f,x+w*.92f,y+h*.5f);c.StrokeColor=Blue;c.StrokeSize=3;c.DrawLine(x+w*.1f,y+h*.38f,x+w*.5f,y+h*.34f);c.DrawLine(x+w*.5f,y+h*.34f,x+w*.92f,y+h*.22f);}
        private static void BoxPlot(ICanvas c,float x,float y,float w,float h){Axes(c,x,y,w,h);for(var i=0;i<3;i++){var xx=x+w*(.25f+i*.23f);c.StrokeColor=i==1?Indigo:Green;c.StrokeSize=2;c.DrawLine(xx,y+h*.2f,xx,y+h*.72f);c.DrawRectangle(xx-w*.045f,y+h*.36f,w*.09f,h*.22f);c.DrawLine(xx-w*.07f,y+h*.28f,xx+w*.07f,y+h*.28f);c.DrawLine(xx-w*.07f,y+h*.66f,xx+w*.07f,y+h*.66f);}}
        private static void Gauge(ICanvas c,float x,float y,float w,float h)
        {
            var cx=x+w*.5f; var cy=y+h*.76f; var r=Math.Min(w,h)*.38f;
            StrokeArc(c, cx, cy, r, 180, 360, Grid, 12);
            StrokeArc(c, cx, cy, r, 180, 290, Green, 12);
            var angle = 290f * (float)Math.PI / 180f;
            c.StrokeColor=Color.FromArgb("#0F172A"); c.StrokeSize=3;
            c.DrawLine(cx,cy,cx+r*.68f*(float)Math.Cos(angle),cy+r*.68f*(float)Math.Sin(angle));
            c.FillColor=Color.FromArgb("#0F172A"); c.FillCircle(cx,cy,4);
        }
        private static void Bullet(ICanvas c,float x,float y,float w,float h){c.FillColor=Grid;c.FillRoundedRectangle(x+w*.15f,y+h*.34f,w*.7f,h*.18f,8);c.FillColor=Blue;c.FillRoundedRectangle(x+w*.15f,y+h*.34f,w*.48f,h*.18f,8);c.StrokeColor=Color.FromArgb("#0F172A");c.StrokeSize=3;c.DrawLine(x+w*.72f,y+h*.27f,x+w*.72f,y+h*.59f);}
        private static void Funnel(ICanvas c,float x,float y,float w,float h){var yy=y+h*.12f;var widths=new[]{.78f,.62f,.48f,.34f};var cols=new[]{Blue,Color.FromArgb("#38BDF8"),Color.FromArgb("#7DD3FC"),Color.FromArgb("#BAE6FD")};for(var i=0;i<4;i++){c.FillColor=cols[i];c.FillRectangle(x+w*(.5f-widths[i]/2),yy+i*h*.16f,w*widths[i],h*.13f);}}
        private static void ThreeD(ICanvas c,float x,float y,float w,float h){Axes(c,x,y,w,h);var cols=new[]{Blue,Indigo,Green,Orange};for(var i=0;i<4;i++){var xx=x+w*(.18f+i*.17f);var hh=h*(.3f+i*.07f);c.FillColor=cols[i];c.FillRectangle(xx,y+h*.78f-hh,w*.1f,hh);c.FillColor=Color.FromArgb("#BFDBFE");c.FillRectangle(xx+w*.1f,y+h*.74f-hh,w*.035f,hh);}}
        private static void Surface(ICanvas c,float x,float y,float w,float h){for(var i=0;i<4;i++){c.StrokeColor=i%2==0?Blue:Indigo;c.StrokeSize=2;c.DrawLine(x+w*.1f,y+h*(.7f-i*.08f),x+w*.42f,y+h*(.28f+i*.08f));c.DrawLine(x+w*.42f,y+h*(.28f+i*.08f),x+w*.88f,y+h*(.58f-i*.07f));}}

        private static void FillWedge(ICanvas c, float cx, float cy, float radius, float startDegrees, float sweepDegrees, Color color)
        {
            var path = new PathF();
            path.MoveTo(cx, cy);
            const int segments = 18;
            for (var i = 0; i <= segments; i++)
            {
                var angle = (startDegrees + sweepDegrees * i / segments) * (float)Math.PI / 180f;
                path.LineTo(cx + radius * (float)Math.Cos(angle), cy + radius * (float)Math.Sin(angle));
            }
            path.Close(); c.FillColor = color; c.FillPath(path);
        }

        private static void StrokeArc(ICanvas c, float cx, float cy, float radius, float startDegrees, float endDegrees, Color color, float width)
        {
            c.StrokeColor = color; c.StrokeSize = width;
            const int segments = 36;
            for (var i = 0; i < segments; i++)
            {
                var a = (startDegrees + (endDegrees - startDegrees) * i / segments) * (float)Math.PI / 180f;
                var b = (startDegrees + (endDegrees - startDegrees) * (i + 1) / segments) * (float)Math.PI / 180f;
                c.DrawLine(cx + radius * (float)Math.Cos(a), cy + radius * (float)Math.Sin(a), cx + radius * (float)Math.Cos(b), cy + radius * (float)Math.Sin(b));
            }
        }
    }
}
