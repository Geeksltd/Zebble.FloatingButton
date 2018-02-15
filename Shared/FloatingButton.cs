namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class FloatingButton : BaseFloatingButton
    {
        public List<Action> Actions { get; set; } = new List<Action>();

        public FloatingButtonPosition Position { get; set; } = FloatingButtonPosition.Custom;

        Func<System.Action, Animation> animationFactory;

        public Func<System.Action, Animation> AnimationFactory
        {
            get => animationFactory ?? DefaultAnimationFactory;
            set => animationFactory = value;
        }

        public int ActionsPadding { get; set; } = 10;

        public bool IsShowing { get; set; }

        public bool IsActionsShowing { get; set; }

        public FloatingButtonFlow Flow { get; set; }

        public FloatingButton() : this(FloatingButtonFlow.Up) { }

        public FloatingButton(FloatingButtonFlow flow, params Action[] actionItems)
        {
            Flow = flow;
            Actions.AddRange(actionItems);
            ClipChildren = false;
        }

        Animation DefaultAnimationFactory(System.Action action)
        {
            return new Animation
            {
                Change = action,
                Duration = 100.Milliseconds(),
                Easing = AnimationEasing.EaseInOut
            };
        }

        public virtual async Task Show()
        {
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

            await this.BringToFront();
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
            if (IsActionsShowing) return;

            var animations = new List<Task>();

            foreach (var action in Actions)
            {
                if (action.Parent == null)
                    await Add(action);

                ResetActionPosition(action);

                await action.BringToFront();

                var x = action.ActualX;
                var y = action.ActualY;
                var oneBaseIndex = Actions.IndexOf(action) + 1;

                switch (Flow)
                {
                    case FloatingButtonFlow.Up:
                        y = -(action.ActualHeight + ActionsPadding) * oneBaseIndex;
                        break;

                    case FloatingButtonFlow.Right:
                        x += this.ActualWidth + (action.ActualWidth + ActionsPadding) * oneBaseIndex;
                        break;

                    case FloatingButtonFlow.Down:
                        y = this.ActualHeight + (action.ActualHeight + ActionsPadding) * oneBaseIndex;
                        break;

                    case FloatingButtonFlow.Left:
                        x += -(action.ActualWidth + ActionsPadding) * oneBaseIndex;
                        break;

                    default:
                        break;
                }

                animations.Add(action.Animate(AnimationFactory(() =>
                {
                    action.X(x);
                    action.Y(y);
                    action.Opacity(1);
                })));
            }
            await Task.WhenAll(animations);
            await this.BringToFront();

            IsActionsShowing = true;
        }

        void ResetActionPosition(Action action)
        {
            action.Opacity(0)
                .X((ActualWidth - action.ActualWidth) / 2)
                .Y((ActualHeight - action.ActualHeight) / 2);
        }

        public async Task HideActions()
        {
            if (!IsActionsShowing) return;

            var animations = new List<Task>();

            foreach (var action in Actions)
                animations.Add(action.Animate(AnimationFactory(() => ResetActionPosition(action))));

            await Task.WhenAll(animations);

            IsActionsShowing = false;
        }

        public override async Task OnInitialized()
        {
            await this.On(x => x.Tapped, TappedHandler).SetPosition(Position);

            await base.OnInitialized();

            if (Parent != null)
                await Show();
        }

        Task TappedHandler()
        {
            if (Actions.Count == 0) return Task.CompletedTask;

            if (IsActionsShowing)
                return HideActions();
            else
                return ShowActions();
        }

        public override async Task<TView> AddAt<TView>(int index, TView child, bool awaitNative = false)
        {
            var result = await base.AddAt(index, child, awaitNative);
            var actionButton = result as Action;
            if (actionButton != null && Actions.Lacks(actionButton))
                Actions.Add(actionButton);

            return result;
        }
    }
}
