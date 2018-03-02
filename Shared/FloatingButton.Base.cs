namespace Zebble
{
    using System.Threading.Tasks;

    public abstract class BaseFloatingButton : Canvas
    {
        internal readonly Canvas imageWrapper = new Canvas();
        internal readonly ImageView imageView = new ImageView();
        internal readonly TextView textView = new TextView();
        internal readonly Stack stack = new Stack {Direction = RepeatDirection.Horizontal, HorizontalAlignment = HorizontalAlignment.Right};

        public string ImagePath
        {
            get => imageView.Path;
            set
            {
                if (imageView.Path != value) imageView.Path = value;
            }
        }

        public string Text
        {
            get => textView.Text;
            set
            {
                if (textView.Text != value) textView.Text = value;
            }
        }

        public override async Task OnInitializing()
        {
            await base.OnInitializing();

            this.Width(Length.AutoStartegy.Content);
            stack.Width(Length.AutoStartegy.Content);

            imageWrapper.Id("ImageWrapper");

            await Add(stack);

            if (!string.IsNullOrEmpty(Text))
            {
                textView.AutoSizeWidth();
                textView.MiddleAlign();

                await stack.Add(textView);
            }

            await stack.Add(imageWrapper);
            await imageWrapper.Add(imageView);

            // There is a bug in Zebble with Length.AutoStartegy.Content
            // These are just temporary workarounds, we can remove these after fixing it
            stack.Width.BindTo(imageWrapper.Width, textView.Width, (iw, tv) => iw + tv);
            Width.BindTo(stack.Width);
        }
    }
}