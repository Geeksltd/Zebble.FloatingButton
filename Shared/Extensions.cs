namespace Zebble
{
    using System;

    [EscapeGCop("Method names are fine")]
    public static class FloatingButtonExtensions
    {
        public static TView Alignment<TView>(this TView view, FloatingButton.ButtonAlignments value) where TView : FloatingButton
        {
            return view.Set(x => x.Alignment = value);
        }

        public static TView ActionButtonAlignment<TView>(this TView view, FloatingButton.ActionButtonAlignments value) where TView : FloatingButton
        {
            return view.Set(x => x.ActionButtonAlignment = value);
        }

        public static TView ImagePath<TView>(this TView view, string value) where TView : FloatingButton
        {
            return view.Set(x => x.ImagePath = value);
        }
    }
}
