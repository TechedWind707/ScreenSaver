using System;
using System.Drawing;          // Graphics, Color, RectangleF, PointF

public abstract class Shape    // abstract = you can never write "new Shape(...)"
{
    // ---- WHERE it is and HOW BIG ----
    // float, not int: movement like 2.5px per tick would be lost with ints
    public float X { get; set; }        // top-left corner
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    // ---- HOW FAST and WHICH WAY ----
    // DX = pixels moved sideways each tick (+ right, - left)
    // DY = pixels moved up/down each tick   (+ down,  - up)  <- screen Y grows DOWNWARD
    public float DX { get; set; }
    public float DY { get; set; }

    public Color FillColor { get; set; }

    // ONE Random for every shape. "protected" = children can use it too.
    // (Making a new Random() per shape created at the same moment can give identical numbers.)
    protected static readonly Random Rng = new Random();

    // "protected" constructor: only children call it, via ": base(...)"
    protected Shape(float x, float y, float size, Color color)
    {
        X = x; Y = y; Width = size; Height = size; FillColor = color;

        // random speed 2..5, random direction (+ or -)
        DX = Rng.Next(2, 6) * (Rng.Next(2) == 0 ? -1 : 1);
        DY = Rng.Next(2, 6) * (Rng.Next(2) == 0 ? -1 : 1);
    }

    // The invisible box around the shape. Used for walls AND collisions.
    // "=>" = a read-only property computed fresh every time you ask.
    public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

    // ====== THE POLYMORPHISM HOOK ======
    // No body. Every child MUST write its own "public override void Draw".
    public abstract void Draw(Graphics g);

    // Shared movement + wall bounce. virtual = children MAY override it.
    // "area" = the current drawing area, passed in fresh every tick (that's what makes resizing work)
    public virtual void Move(Rectangle area)
    {
        X += DX;            // step
        Y += DY;

        bool hitWall = false;

        // LEFT wall: we went past it -> put us back ON the wall, force direction to the RIGHT
        if (X < area.Left) { X = area.Left; DX = Math.Abs(DX); hitWall = true; }
        // RIGHT wall: our right edge (X + Width) went past it -> snap back, force LEFT
        else if (X + Width > area.Right) { X = area.Right - Width; DX = -Math.Abs(DX); hitWall = true; }

        // same thing, top/bottom
        if (Y < area.Top) { Y = area.Top; DY = Math.Abs(DY); hitWall = true; }
        else if (Y + Height > area.Bottom) { Y = area.Bottom - Height; DY = -Math.Abs(DY); hitWall = true; }

        if (hitWall) OnWallHit();   // give the child a chance to react (colour change etc.)
    }

    // Empty on purpose. Children override it to do a visual change on bounce.
    protected virtual void OnWallHit() { }
}