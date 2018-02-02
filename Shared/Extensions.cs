namespace Zebble
{
    using System;

    [EscapeGCop("Method names are fine")]
    public static class FloatingButtonExtensions
    {
        public static TView Alignment<TView>(this TView view, FloatingButton.ButtonAlignment value) where TView : FloatingButton
        {
            return view.Set(x => x.Alignment = value);
        }

        public static TView Color<TView>(this TView view, Color value) where TView : FloatingButton
        {
            return view.Set(x => x.Color = value);
        }

        public static TView ImagePath<TView>(this TView view, string value) where TView : FloatingButton
        {
            return view.Set(x => x.ImagePath = value);
        }

        public static TView Size<TView>(this TView view, FloatingButton.ButtonSize value) where TView : FloatingButton
        {
            return view.Set(x => x.Size = value);
        }
    }
}
