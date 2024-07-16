using CommunityToolkit.Mvvm.ComponentModel;

namespace Aps.Sample.App
{
    public class User : ObservableObject
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }
}