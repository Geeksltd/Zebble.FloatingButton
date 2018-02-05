namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class FloatingButton : Canvas
    {
        float MINI_SIZE = 40;
        float BIG_SIZE = 56;
        float BTN_SIZE = 24;
        float BIG_PADDING = 14;
        float MINI_PADDING = 6;

        readonly Button Button = new Button();

        protected float CONTAINER_MARGIN = 10;

        public bool IsShowing { get; set; }

        ButtonSize? buttonSize;
        public ButtonSize? Size
        {
            get { return buttonSize; }
            set
            {
                buttonSize = value;
                if (buttonSize == ButtonSize.Small)
                    this.Width(MINI_SIZE).Height(MINI_SIZE).Border(all: 1, radius: MINI_SIZE).Padding(all: MINI_PADDING);
                else
                    this.Width(BIG_SIZE).Height(BIG_SIZE).Border(all: 1, radius: BIG_SIZE).Padding(all: BIG_PADDING);

                Button.Width(BTN_SIZE).Height(BTN_SIZE);
            }
        }

        public Color Color
        {
            get { return BackgroundColor; }
            set
            {
                if (BackgroundColor == value) return;

                BackgroundColor = value;
                this.Border(color: value);
            }
        }

        public string ImagePath
        {
            get { return Button.BackgroundImagePath; }
            set
            {
                if (Button.BackgroundImagePath == value) return;
                Button.BackgroundImagePath = value;
            }
        }

        ButtonAlignment buttonAlignment;
        public ButtonAlignment Alignment
        {
            get { return buttonAlignment; }
            set
            {
                buttonAlignment = value;
                UpdatePosition();
            }
        }

        Color shadowColor;
        public Color ShadowColor
        {
            get { return shadowColor; }
            set
            {
                if (shadowColor == value) return;

                shadowColor = value;
                this.BoxShadow(1, 1, 5, shadowColor);
            }
        }

        public async override Task OnInitializing()
        {
            await base.OnInitializing();

            Button.Tapped.Handle(Tapped.Raise);
            Button.Padding(all: 0);
            Button.BackgroundColor = Colors.Transparent;

            if (Size == null) Size = ButtonSize.Small;
            if (Color == Colors.Transparent) Color = Colors.Pink;
            if (ShadowColor == Colors.Transparent) ShadowColor = Colors.Silver;

            var maxZindex = Nav.CurrentPage.AllDescendents().Except(d => d is FloatingButton).Max(c => c.ZIndex);
            Id = "FloatingButton".WithSuffix(Id);
            Tapped.Handle(() => { Button.Flash(); });
            this.Absolute().ZIndex(maxZindex + 1);

            await Add(Button);
        }

        public virtual async Task Show()
        {
            UpdatePosition();
            if (IsShowing) return;

            await Nav.CurrentPage.Add(this);
            IsShowing = true;
        }

        public virtual async Task Hide()
        {
            if (!IsShowing) return;

            await Nav.CurrentPage.Remove(this);
            IsShowing = false;
        }

        protected virtual void UpdatePosition()
        {
            switch (Alignment)
            {
                default:
                case ButtonAlignment.BottomRight:
                    X.BindTo(Root.Width, Width, Margin.Right, (rw, fbw, mr) => rw - (fbw + mr + CONTAINER_MARGIN));
                    Y.BindTo(Root.Height, Height, Margin.Bottom, (rh, fbh, mb) => rh - (fbh + mb + CONTAINER_MARGIN));
                    break;
                case ButtonAlignment.BottomLeft:
                    X.BindTo(Margin.Left, ml => ml + CONTAINER_MARGIN);
                    Y.BindTo(Root.Height, Height, Margin.Bottom, (rh, fbh, mb) => rh - (fbh + mb + CONTAINER_MARGIN));
                    break;
                case ButtonAlignment.TopLeft:
                    X.BindTo(Margin.Left, ml => ml + CONTAINER_MARGIN);
                    Y.BindTo(Margin.Top, mt => mt + CONTAINER_MARGIN);
                    break;
                case ButtonAlignment.TopRight:
                    X.BindTo(Root.Width, Width, Margin.Right, (rw, fbw, mr) => rw - (fbw + mr + CONTAINER_MARGIN));
                    Y.BindTo(Margin.Top, mt => mt + CONTAINER_MARGIN);
                    break;
                case ButtonAlignment.Custom:
                    break;
            }
        }
    }

    public class FloatingButton<TActionButtonCollection> : FloatingButton where TActionButtonCollection : FloatingButton.ActionButtonCollection
    {
        readonly List<FloatingButton> CurrentActionItems = new List<FloatingButton>();

        public TActionButtonCollection Source { get; set; }

        public bool IsActionItemsShowing { get; set; }

        public async override Task OnInitializing()
        {
            await base.OnInitializing();

            Shown.Handle(() => this.ZIndex(CurrentActionItems.MaxOrNull(f => f.ZIndex) ?? ZIndex + 1));
        }

        public async Task ShowAsActionMenu()
        {
            if (Source == null)
            {
                Device.Log.Error("ActionButton source is null");
                return;
            }

            UpdatePosition();

            Size = ButtonSize.Big;
            Tapped.Handle(ShowActionItems);

            await Show();
        }

        public async Task ShowActionItems()
        {
            if (IsActionItemsShowing)
            {
                await HideActionItems();
                return;
            }

            var lastItemTop = Y.CurrentValue;
            var lastItemLeft = X.CurrentValue;
            Source.Update();

            List<Task> animations = new List<Task>();

            foreach (var actionItem in Source.ActionButtons)
            {
                actionItem.ZIndex(ZIndex - 1);
                actionItem.Y(Y.CurrentValue);
                actionItem.X(X.CurrentValue);

                switch (Source.Alignment)
                {
                    case ActionButtonAlignment.Top:
                        lastItemTop -= actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignment.Right:
                        lastItemLeft += actionItem.ActualWidth + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignment.Bottom:
                        lastItemTop += actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignment.Left:
                        lastItemLeft -= actionItem.ActualWidth + CONTAINER_MARGIN;
                        break;
                    default:
                        break;
                }

                CurrentActionItems.Add(actionItem);
                actionItem.X(lastItemLeft).Y(lastItemTop).ScaleX(0).ScaleY(0);

                animations.Add(Nav.CurrentPage.AddWithAnimation(actionItem, new Animation
                {
                    Change = () => actionItem.ScaleX(1).ScaleY(1),
                    Duration = 100.Milliseconds(),
                    Easing = AnimationEasing.EaseInOut
                }));
            }

            await Task.WhenAll(animations);

            IsActionItemsShowing = true;
        }

        public async Task HideActionItems()
        {
            if (!IsActionItemsShowing) return;

            List<Task> animations = new List<Task>();

            foreach (var actionItem in CurrentActionItems)
            {
                animations.Add(actionItem.Animate(100.Milliseconds(), AnimationEasing.EaseInOut, child => child.ScaleX(0).ScaleY(0))
                      .ContinueWith(async (a) => await Nav.CurrentPage.Remove(actionItem)));
            }

            await Task.WhenAll(animations);

            IsActionItemsShowing = false;
        }
    }
}
