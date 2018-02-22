namespace Zebble
{
    using System.Threading.Tasks;

    public abstract class BaseFloatingButton : Canvas
    {
        readonly ImageView ImageView = new ImageView();
        protected readonly Canvas MainButton = new Canvas().Id("MainButton");

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

            await MainButton.Add(ImageView);
            await Add(ImageView);
        }
    }
}