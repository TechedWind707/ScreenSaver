using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;    // List<T>
using System.Drawing;
using System.Drawing.Drawing2D;      // SmoothingMode, LinearGradientBrush
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class Form1 : Form
    {
        // THE list. Its type is Shape, so it can hold Circle, Square, ... anything that IS-A Shape.
        // It doesn't know or care which kind each one is.
        private List<Shape> shapes = new List<Shape>();
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        // ===== runs every 20 ms =====
        private void timer1_Tick(object sender, EventArgs e)
        {
            Rectangle area = ClientRectangle;   // the form's CURRENT inside size. Read it EVERY tick.

            foreach (Shape s in shapes)
                s.Move(area);                   // polymorphism here too: PictureShape/Square/Triangle use their own Move

            CheckCollisions();                  // Step 10

            Invalidate();   // "this window is out of date, please repaint it"
                            // It does NOT draw. Windows will raise the Paint event shortly after.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;                // the "pen + paper" Windows hands you. Only valid inside Paint.
            g.SmoothingMode = SmoothingMode.AntiAlias;   // smooth edges (optional)

            // >>>>>>>>>>>> THE POLYMORPHISM MOMENT <<<<<<<<<<<<
            // Compiler sees: s is a Shape, Shape has Draw -> OK.
            // At run time, C# checks what s REALLY is and calls THAT class's Draw.
            // No if/switch on type anywhere. Add a new shape class later: this loop never changes.
            foreach (Shape s in shapes)
                s.Draw(g);

        }

        private int nextType = 0;    // which shape the next click makes (field, at top of Form1)

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // e.X, e.Y = mouse position INSIDE the form (already client coordinates)
            Shape s;

            switch (nextType)
            {
                // EXPLICIT UPCAST: (Shape) turns "a Circle" into "a Shape reference" before it goes in the list.
                // C# would do this automatically (every Circle IS-A Shape, so it's always safe),
                // but the rubric wants to SEE it, so write it out.
                case 0: s = (Shape)new Circle(e.X, e.Y, 50); break;
                case 1: s = (Shape)new Square(e.X, e.Y, 50); break;
                case 2: s = (Shape)new Triangle(e.X, e.Y, 50); break;
                case 3: s = (Shape)new Polygon(e.X, e.Y, 60); break;
                default:
                    s = (Shape)new PictureShape(
                        Properties.Resources.Me_Headshot,
                        e.X,
                        e.Y,
                        60);
                    break;
            }

            shapes.Add(s);
            nextType = (nextType + 1) % 5;    // 0,1,2,3,4,0,1,... (% = remainder, wraps around)
            Invalidate();
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    bool isIntersecting = shapes[i].Bounds.IntersectsWith(shapes[j].Bounds);

                    float gapX = shapes[i].X - shapes[j].X;
                    float gapY = shapes[i].Y - shapes[j].Y;

                    float speedDiffX = shapes[i].DX - shapes[j].DX;
                    float speedDiffY = shapes[i].DY - shapes[j].DY;
                    
                    bool isMovingTowards = (gapX * speedDiffX) + (gapY * speedDiffY) < 0;

                    bool isColliding = isIntersecting && isMovingTowards;

                    if (isColliding)
                    {
                        // Swap DX and DY for a simple bounce effect
                        float tempDX = shapes[i].DX;
                        float tempDY = shapes[i].DY;
                        shapes[i].DX = shapes[j].DX;
                        shapes[i].DY = shapes[j].DY;
                        shapes[j].DX = tempDX;
                        shapes[j].DY = tempDY;
                        // Call OnWallHit to change color on collision
                        //shapes[i].OnWallHit();
                        //shapes[j].OnWallHit();

                        if (shapes[i] is Polygon polygon1 && shapes[j] is Polygon polygon2)
                        {
                            // If both shapes are colliding polygons, cycle their templates
                            polygon1.CycleTemplate();
                            polygon2.CycleTemplate();
                        }
                    }
                }
            }
        }
    }
}
