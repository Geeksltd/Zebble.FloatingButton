namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    [EscapeGCop("Method names are fine")]
    public static class FloatingButtonExtensions
    {
        static string Top = "top";
        static string Right = "right";
        static string Bottom = "bottom";
        static string Left = "left";

        public static async Task<TView> SetPosition<TView>(this TView view, FloatingButtonPosition value) where TView : FloatingButton
        {
            view.Position = value;
            var temp = view.CssClass;

            temp = temp.KeepReplacing(Top, "")
                .KeepReplacing(Right, "")
                .KeepReplacing(Bottom, "")
                .KeepReplacing(Left, "");

            switch (value)
            {
                case FloatingButtonPosition.TopRight:
                    temp += $" {Top} {Right}";
                    break;

                case FloatingButtonPosition.TopLeft:
                    temp += $" {Top} {Right}";
                    break;

                case FloatingButtonPosition.BottomRight:
                    temp += $" {Bottom} {Right}";
                    break;

                case FloatingButtonPosition.BottomLeft:
                    temp += $" {Bottom} {Left}";
                    break;
            }

            await view.SetCssClass(temp);

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
