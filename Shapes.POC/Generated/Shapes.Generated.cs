namespace Shapes.POC.Generated
{
    public class Triangle
    {
        public Triangle_StrokeColor StrokeColor(System.UInt32 strokeColor)
        {
            return new Triangle_StrokeColor();
        }

        public Triangle_FillColor FillColor(System.UInt32 fillColor)
        {
            return new Triangle_FillColor();
        }

        public Triangle_CornerRadius CornerRadius(System.Single cornerRadius)
        {
            return new Triangle_CornerRadius();
        }
    }

    public class Triangle_CornerRadius
    {
        private System.Single cornerRadius;
        public Triangle_CornerRadius_StrokeColor StrokeColor(System.UInt32 strokeColor)
        {
            return new Triangle_CornerRadius_StrokeColor();
        }

        public Triangle_CornerRadius_FillColor FillColor(System.UInt32 fillColor)
        {
            return new Triangle_CornerRadius_FillColor();
        }
    }

    public class Triangle_FillColor
    {
        private System.UInt32 fillColor;
        public Triangle_FillColor_StrokeColor StrokeColor(System.UInt32 strokeColor)
        {
            return new Triangle_FillColor_StrokeColor();
        }

        public Triangle_CornerRadius_FillColor CornerRadius(System.Single cornerRadius)
        {
            return new Triangle_CornerRadius_FillColor();
        }
    }

    public class Triangle_CornerRadius_FillColor
    {
        private System.Single cornerRadius;
        private System.UInt32 fillColor;
        public Triangle_CornerRadius_FillColor_StrokeColor StrokeColor(System.UInt32 strokeColor)
        {
            return new Triangle_CornerRadius_FillColor_StrokeColor();
        }
    }

    public class Triangle_StrokeColor
    {
        private System.UInt32 strokeColor;
        public void Draw()
        {
        }

        public Triangle_FillColor_StrokeColor FillColor(System.UInt32 fillColor)
        {
            return new Triangle_FillColor_StrokeColor();
        }

        public Triangle_CornerRadius_StrokeColor CornerRadius(System.Single cornerRadius)
        {
            return new Triangle_CornerRadius_StrokeColor();
        }
    }

    public class Triangle_CornerRadius_StrokeColor
    {
        private System.Single cornerRadius;
        private System.UInt32 strokeColor;
        public Triangle_CornerRadius_FillColor_StrokeColor FillColor(System.UInt32 fillColor)
        {
            return new Triangle_CornerRadius_FillColor_StrokeColor();
        }
    }

    public class Triangle_FillColor_StrokeColor
    {
        private System.UInt32 fillColor;
        private System.UInt32 strokeColor;
        public void Draw()
        {
        }

        public Triangle_CornerRadius_FillColor_StrokeColor CornerRadius(System.Single cornerRadius)
        {
            return new Triangle_CornerRadius_FillColor_StrokeColor();
        }
    }

    public class Triangle_CornerRadius_FillColor_StrokeColor
    {
        private System.Single cornerRadius;
        private System.UInt32 fillColor;
        private System.UInt32 strokeColor;
        public void Draw()
        {
        }
    }
}