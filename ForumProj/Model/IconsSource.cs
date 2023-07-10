using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;

namespace ForumProj.Model;

public class IconsSource
{
    public static string UserGrayIcon => Path.GetFullPath(@"..\..\..\Windows\icons\user.png");
    public static string FileWhiteIcon => Path.GetFullPath(@"..\..\..\Windows\icons\logout.png");
    public static string PowerOffWhiteIcon => Path.GetFullPath(@"..\..\..\Windows\icons\power-off.png");
    public static string SettingWhiteIcon => Path.GetFullPath(@"..\..\..\Windows\icons\setting.png");
}
