using System.Drawing;
using System.Drawing.Drawing2D;

public class Square : Shape                 // ": Shape" = Square IS-A Shape
{
    private float angle = 0;   // for spinning the gradient

    // Square's constructor just forwards to Shape's.  ": base(...)" runs Shape(x, y, size, color)
    public Square(float x, float y, float size) : base(x, y, size, Color.OliveDrab) { }

    // Override Move to ADD behaviour, not replace it:
    public override void Move(Rectangle area)
    {
        base.Move(area);                 // 1. do the normal move + bounce from Shape
        angle = (angle + 4) % 360;       // 2. then spin the gradient a bit each tick
    }


    // REQUIRED. Without this, compile error: "Square does not implement inherited abstract member Shape.Draw"
    public override void Draw(Graphics g)
    {
        RectangleF r = Bounds;   // the box that contains the shape

        // gradient from FillColor to White, across the box, at "angle" degrees
        // NOTE: throws if r has 0 width or height - matters if you ever shrink shapes
        using (LinearGradientBrush brush = new LinearGradientBrush(r, FillColor, Color.White, angle))
        {
            g.FillRectangle(brush, r);
        }
    }
}