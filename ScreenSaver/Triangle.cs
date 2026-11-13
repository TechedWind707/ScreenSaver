using System.Drawing;

public class Triangle : Shape                 // ": Shape" = Triangle IS-A Shape
{
    private bool growing = true;

    // Triangle's constructor just forwards to Shape's.  ": base(...)" runs Shape(x, y, size, color)
    public Triangle(float x, float y, float size) : base(x, y, size, Color.Gold) { }

    // Override Move to ADD behaviour, not replace it:
    public override void Move(Rectangle area)
    {
        const float growthSpeed = 1.5f;
        base.Move(area);

        if (growing)
        {
            Width += growthSpeed;
            if (Width >= 90)
            {
                Width = 90;
                growing = false;
            }
        }
        else
        {
            Width -= growthSpeed;
            if (Width <= 30)
            {
                Width = 30;
                growing = true;
            }
        }

        // Keep the triangle's bounding box square so its whole shape grows and shrinks.
        Height = Width;
    }

    // REQUIRED. Without this, compile error: "Triangle does not implement inherited abstract member Shape.Draw"
    public override void Draw(Graphics g)
    {
        Point[] trianglePoints = {
                new Point ((int)(X + Width / 2), (int)Y),          // Top vertex
                new Point ((int)X, (int)(Y + Height)),          // Bottom-left vertex
                new Point ((int)(X + Width), (int)(Y + Height))   // Bottom-right vertex
            };

        // "using" = Dispose the brush when done. Brushes hold Windows (GDI) resources;
        // we make them ~50x a second, so leaking them adds up fast.
        using (SolidBrush brush = new SolidBrush(FillColor))
        {
            
            g.FillPolygon(brush, trianglePoints);   // triangle that fits inside the box
        }
    }

    // VISUAL CHANGE #3 (changing colour): make the triangle grow to ~90px and shrink to !~30px over and over
    protected override void OnWallHit()
    {
        FillColor = Color.FromArgb(Rng.Next(256), Rng.Next(256), Rng.Next(256));
    }
}