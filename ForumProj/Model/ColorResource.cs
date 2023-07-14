using System.Windows.Media;

namespace ForumProj.Model;

public static class ColorResource
{
    public static readonly Brush DarkGray  = (Brush)new BrushConverter().ConvertFrom("#2E3440"); 
    public static readonly Brush MediumGray = (Brush)new BrushConverter().ConvertFrom("#4C566A");
    public static readonly Brush LightGray = (Brush)new BrushConverter().ConvertFrom("#3B4252");
    public static readonly Brush Green = (Brush)new BrushConverter().ConvertFrom("#A3BE8C");

}