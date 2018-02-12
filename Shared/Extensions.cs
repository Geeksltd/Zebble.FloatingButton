namespace Zebble
{
    using System;

    [EscapeGCop("Method names are fine")]
    public static class FloatingButtonExtensions
    {
        public static TView Position<TView>(this TView view, FloatingButtonPosition value) where TView : FloatingButton
        {
            return view.Set(x => x.Position = value);
        }

        public static TView Flow<TView>(this TView view, FloatingButtonFlow value) where TView : FloatingButton
        {
            return view.Set(x => x.Flow = value);
        }

        public static TView ImagePath<TView>(this TView view, string value) where TView : FloatingButton
        {
            return view.Set(x => x.ImagePath = value);
        }
    }
}
