namespace Zebble
{
    using System.Threading.Tasks;

    public abstract class BaseFloatingButton : Canvas
    {
        readonly ImageView ImageView = new ImageView();

        public string ImagePath
        {
            get => ImageView.Path;
            set
            {
                if (ImageView.Path != value) ImageView.Path = value;
            }
        }

        public override async Task OnInitializing()
        {
            await base.OnInitializing();
            
            await Add(ImageView);
        }
    }
}
