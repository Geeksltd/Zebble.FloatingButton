namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class FloatingButton : FloatingButtonBase
    {
        bool IsInitializedWithMarkup;

        public List<ActionButton> ActionButtons { get; set; }

        public bool IsShowing { get; set; }

        public bool IsActionItemsShowing { get; set; }

        public ActionButtonAlignments ActionButtonAlignment { get; set; } = ActionButtonAlignments.Top;

        public FloatingButton() { ActionButtons = new List<ActionButton>(); Visible = IsInitializedWithMarkup = true; }

        public FloatingButton(ActionButtonAlignments actionButtonAlignment, params ActionButton[] actionItems)
        {
            ActionButtonAlignment = actionButtonAlignment;
            ActionButtons = new List<ActionButton>(actionItems);
            Visible = true;
        }

        public virtual async Task Show()
        {
            UpdatePosition();
            if (IsShowing) return;

            if (Parent != null && Parent.AllChildren.Contains(this))
                Visible = true;
            else
            {
                if (Nav.CurrentPage.AllChildren.Contains(this))
                    Visible = true;
                else
                    await Nav.CurrentPage.Add(this);
            }

            IsShowing = true;
        }

        public virtual async Task Hide()
        {
            if (!IsShowing) return;

            Visible = false;
            IsShowing = false;
        }

        public async Task ShowAsActionMenu()
        {
            if (ActionButtons == null)
            {
                Device.Log.Error("ActionButton source is null");
                return;
            }

            UpdatePosition();

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

            float lastItemTop = 0, lastItemLeft = 0;

            if (!IsInitializedWithMarkup)
            {
                lastItemTop = Y.CurrentValue;
                lastItemLeft = X.CurrentValue;
            }

            var animations = new List<Task>();

            foreach (var actionItem in ActionButtons)
            {
                actionItem.ZIndex(ZIndex - 1);
                if (!IsInitializedWithMarkup)
                {
                    actionItem.Y(Y.CurrentValue);
                    actionItem.X(X.CurrentValue);
                }

                if (actionItem.Parent == null)
                    await Nav.CurrentPage.Add(actionItem);

                switch (ActionButtonAlignment)
                {
                    case ActionButtonAlignments.Top:
                        lastItemTop -= actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignments.Right:
                        lastItemLeft += actionItem.ActualWidth + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignments.Bottom:
                        lastItemTop += actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case ActionButtonAlignments.Left:
                        lastItemLeft -= actionItem.ActualWidth + CONTAINER_MARGIN;
                        break;
                    default:
                        break;
                }

                actionItem.X(lastItemLeft).Y(lastItemTop).ScaleX(0).ScaleY(0);
                var animation = new Animation
                {
                    Change = () => actionItem.ScaleX(1).ScaleY(1),
                    Duration = 100.Milliseconds(),
                    Easing = AnimationEasing.EaseInOut
                };

                animations.Add(actionItem.Animate(animation));
                actionItem.Visible = true;
            }

            await Task.WhenAll(animations);

            IsActionItemsShowing = true;
        }

        public async Task HideActionItems()
        {
            if (!IsActionItemsShowing) return;

            var animations = new List<Task>();

            foreach (var actionItem in ActionButtons)
            {
                animations.Add(actionItem.Animate(100.Milliseconds(), AnimationEasing.EaseInOut, child => child.ScaleX(0).ScaleY(0))
                      .ContinueWith((a) => actionItem.Visible = false));
            }

            await Task.WhenAll(animations);

            IsActionItemsShowing = false;
        }

        public override async Task OnInitialized()
        {
            await base.OnInitialized();

            if (IsInitializedWithMarkup)
            {
                await Show();
                if (ActionButtons.Any())
                    await ShowAsActionMenu();
            }
        }

        public override async Task<TView> Add<TView>(TView child, bool awaitNative = false)
        {
            var result = await base.Add(child, awaitNative);
            var actionButton = result as ActionButton;
            if (actionButton != null)
                ActionButtons.Add(actionButton);

            return result;
        }
    }
}
