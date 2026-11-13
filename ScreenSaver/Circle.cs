using System.Drawing;

public class Circle : Shape                 // ": Shape" = Circle IS-A Shape
{
    // Circle's constructor just forwards to Shape's.  ": base(...)" runs Shape(x, y, size, color)
    public Circle(float x, float y, float size) : base(x, y, size, Color.Gold) { }

    // REQUIRED. Without this, compile error: "Circle does not implement inherited abstract member Shape.Draw"
    public override void Draw(Graphics g)
    {
        // "using" = Dispose the brush when done. Brushes hold Windows (GDI) resources;
        // we make them ~50x a second, so leaking them adds up fast.
        using (SolidBrush brush = new SolidBrush(FillColor))
        {
            g.FillEllipse(brush, X, Y, Width, Height);   // ellipse that fits inside the box
        }
    }

    // VISUAL CHANGE #1 (changing colour): new random colour every time we bounce off a wall
    protected override void OnWallHit()
    {
        FillColor = Color.FromArgb(Rng.Next(256), Rng.Next(256), Rng.Next(256));
    }
}