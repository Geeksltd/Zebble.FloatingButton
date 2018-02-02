namespace Zebble
{
    public partial class FloatingButton
    {
        public class ActionButton
        {
            public readonly AsyncEvent Tapped = new AsyncEvent();

            public string ImagePath { get; set; }
            public Color Color { get; set; } = Colors.Pink;
            public ButtonSize Size { get; set; } = ButtonSize.Big;
        }
    }
}
