using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Polygon : Shape
{

    int currentTemplateIndex = 0; // Index of the current template to use

    // Each PointF here is (fraction across, fraction down) inside the box.
    // (0,0) = top-left of box, (1,1) = bottom-right.
    private static readonly PointF[][] Template =
    {
        new PointF[5] {
            new PointF(0.0f, 0.0f),
            new PointF(1.0f, 0.0f),
            new PointF(1.0f, 1.0f),
            new PointF(0.0f, 1.0f),
            new PointF(0.5f, 0.5f)
        },
        new PointF[6] {
            new PointF(0.1f, 0.0f),
            new PointF(0.9f, 0.2f),
            new PointF(0.8f, 0.9f),
            new PointF(0.5f, 0.7f),
            new PointF(0.2f, 0.9f),
            new PointF(0.1f, 0.2f)
        },
        new PointF[7] {
            new PointF(0.5f, 0.0f),
            new PointF(0.9f, 0.2f),
            new PointF(1.0f, 0.6f),
            new PointF(0.7f, 1.0f),
            new PointF(0.3f, 1.0f),
            new PointF(0.0f, 0.6f),
            new PointF(0.1f, 0.2f)
        },
    };

    // ... YOUR points. 5-7 lopsided ones = "irregular". Go clockwise so it doesn't cross itself

    public Polygon(float x, float y, float size) : base(x, y, size, Color.White) { }

    public override void Draw(Graphics g)
    {
        PointF[] currentTemplate = Template[currentTemplateIndex];
        PointF[] pts = new PointF[currentTemplate.Length];
        for (int i = 0; i < currentTemplate.Length; i++)
        {
            // fraction -> real pixel position
            pts[i] = new PointF(X + currentTemplate[i].X * Width,
                                Y + currentTemplate[i].Y * Height);
        }
        using (SolidBrush brush = new SolidBrush(FillColor))
            g.FillPolygon(brush, pts);
    }

    public void CycleTemplate()
    {
        currentTemplateIndex = (currentTemplateIndex + 1) % Template.Length;
    }
}