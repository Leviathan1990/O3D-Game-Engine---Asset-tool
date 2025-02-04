using System.Windows.Forms;

namespace AssetTool
{
    public static class ApplyTheme
    {
        public static void SetTheme(Form form)
        {
            string theme = ThemeManager.LoadConfig("Theme", "Light");

            if (theme == "Dark")
            {
                ThemeManager.ApplyDarkMode(form);
            }
            else if (theme == "Light")
            {
                ThemeManager.ApplyLightMode(form);
            }
            else
            {
                ThemeManager.ApplyDefaultMode(form);
            }
        }
    }
}
