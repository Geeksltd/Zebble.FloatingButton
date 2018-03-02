namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    [EscapeGCop("Method names are fine")]
    public static class FloatingButtonExtensions
    {
        static readonly string Top = "top";
        static readonly string Right = "right";
        static readonly string Bottom = "bottom";
        static readonly string Left = "left";

        public static async Task<TView> SetPosition<TView>(this TView view, FloatingButtonPosition value)
            where TView : FloatingButton
        {
            view.Position = value;
            var cssClass = view.CssClass;

            cssClass = cssClass.KeepReplacing(Top, "")
                .KeepReplacing(Right, "")
                .KeepReplacing(Bottom, "")
                .KeepReplacing(Left, "");

            switch (value)
            {
                case FloatingButtonPosition.TopRight:
                    cssClass += $" {Top} {Right}";
                    break;

                case FloatingButtonPosition.TopLeft:
                    cssClass += $" {Top} {Left}";
                    break;

                case FloatingButtonPosition.BottomRight:
                    cssClass += $" {Bottom} {Right}";
                    break;

                case FloatingButtonPosition.BottomLeft:
                    cssClass += $" {Bottom} {Left}";
                    break;
            }

            await view.SetCssClass(cssClass);

            return view;
        }

        public static TView Position<TView>(this TView view, FloatingButtonPosition value) where TView : FloatingButton
        {
            return view.Set(x => x.Position = value);
        }

        public static TView Flow<TView>(this TView view, FloatingButtonFlow value) where TView : FloatingButton
        {
            return view.Set(x => x.Flow = value);
        }

        public static TView ImagePath<TView>(this TView view, string value) where TView : BaseFloatingButton
        {
            return view.Set(x => x.ImagePath = value);
        }
    }
}