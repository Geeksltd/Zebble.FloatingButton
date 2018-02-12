namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class FloatingButton : BaseFloatingButton
    {
        bool IsInitializedWithMarkup;

        public List<Action> Actions { get; set; }

        public bool IsShowing { get; set; }

        public bool IsActionsShowing { get; set; }

        public FloatingButtonFlow Flow { get; set; } = FloatingButtonFlow.Top;

        public FloatingButton() { Actions = new List<Action>(); Visible = IsInitializedWithMarkup = true; }

        public FloatingButton(FloatingButtonFlow flow, params Action[] actionItems)
        {
            Flow = flow;
            Actions = new List<Action>(actionItems);
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

            if (Actions != null && Actions.Any())
                Tapped.Handle(ShowActions);

            IsShowing = true;
        }

        public virtual async Task Hide()
        {
            if (!IsShowing) return;

            Visible = false;
            IsShowing = false;
        }

        public async Task ShowActions()
        {
            if (IsActionsShowing)
            {
                await HideActions();
                return;
            }

            float lastItemTop = 0, lastItemLeft = 0;

            if (!IsInitializedWithMarkup)
            {
                lastItemTop = Y.CurrentValue;
                lastItemLeft = X.CurrentValue;
            }

            var animations = new List<Task>();

            foreach (var actionItem in Actions)
            {
                actionItem.ZIndex(ZIndex - 1);
                if (!IsInitializedWithMarkup)
                {
                    actionItem.Y(Y.CurrentValue);
                    actionItem.X(X.CurrentValue);
                }

                if (actionItem.Parent == null)
                    await Nav.CurrentPage.Add(actionItem);

                switch (Flow)
                {
                    case FloatingButtonFlow.Top:
                        lastItemTop -= actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case FloatingButtonFlow.Right:
                        lastItemLeft += actionItem.ActualWidth + CONTAINER_MARGIN;
                        break;
                    case FloatingButtonFlow.Bottom:
                        lastItemTop += actionItem.ActualHeight + CONTAINER_MARGIN;
                        break;
                    case FloatingButtonFlow.Left:
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

            IsActionsShowing = true;
        }

        public async Task HideActions()
        {
            if (!IsActionsShowing) return;

            var animations = new List<Task>();

            foreach (var actionItem in Actions)
            {
                animations.Add(actionItem.Animate(100.Milliseconds(), AnimationEasing.EaseInOut, child => child.ScaleX(0).ScaleY(0))
                      .ContinueWith((a) => actionItem.Visible = false));
            }

            await Task.WhenAll(animations);

            IsActionsShowing = false;
        }

        public override async Task OnInitialized()
        {
            await base.OnInitialized();

            if (IsInitializedWithMarkup)
                await Show();
        }

        public override async Task<TView> Add<TView>(TView child, bool awaitNative = false)
        {
            var result = await base.Add(child, awaitNative);
            var actionButton = result as Action;
            if (actionButton != null)
                Actions.Add(actionButton);

            return result;
        }
    }
}
