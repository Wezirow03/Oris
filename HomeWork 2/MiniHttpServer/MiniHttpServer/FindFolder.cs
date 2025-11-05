public class FindFolder
{
    private static readonly Dictionary<string, string> folderPath = new()
    {
        {"gpt.html","public/gpt/gpt.html"},
        {"gptstyle.css", "public/gpt/gptstyle.css"},
        {"finger.html", "public/finger/finger.html"},
        {"iam.jpg", "public/images/iam.jpg"},
        {"forget.html", "public/loginpage/forget.html"},
        {"index.html", "public/loginpage/index.html"},
        {"register.html", "public/loginpage/register.html"},
        {"style.css", "public/loginpage/style.css"},
        {"olora.html", "public/olora/olora.html"},
        {"olora.css", "public/olora/css/olora.css"},
        {"circle.png", "public/olora/img/circle.png"},
        {"Illustration.png", "public/olora/img/Illustration.png"},
        {"File.zip","public/File.zip" },
        {"bg.jpg", "public/images/bg.jpg"},
        {"logo.png", "public/images/logo.png"},
        {"Deepseek.jpg", "public/images/Deepseek.jpg"},
        {"Gpt.jpg", "public/images/Gpt.jpg"},
        {"in.jpg", "public/images/in.jpg"},
        {"Github.jpg", "public/images/Github.jpg"},
        {"GG.jpg", "public/images/GG.jpg"},
        {"Plus.jpg", "public/images/Plus.jpg"},
        {"youtube.jpg","public/images/youtube.jpg" }
    };

    public string DicFind(string path)
    {
        return folderPath.TryGetValue(path, out string? value) ? value : null;
    }
}
