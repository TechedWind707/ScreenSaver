using System.Drawing;

public class PictureShape : Shape
{
    private readonly Image image;

    public PictureShape(Image image, float x, float y, float size) : base(x, y, size, Color.White)
    {
        this.image = image;
    }

    public override void Draw(Graphics g)
    {
        g.DrawImage(image, Bounds);
    }
}