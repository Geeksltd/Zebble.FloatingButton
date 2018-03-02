namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class FloatingButton : BaseFloatingButton
    {
        readonly Overlay overlay;
        Func<System.Action, Animation> animationFactory;

        public List<Action> Actions { get; set; } = new List<Action>();

        public FloatingButtonPosition Position { get; set; } = FloatingButtonPosition.BottomRight;

        public Func<System.Action, Animation> AnimationFactory
        {
            get => animationFactory ?? DefaultAnimationFactory;
            set => animationFactory = value;
        }

        public int ActionsPadding { get; set; } = 10;

        public bool IsShowing { get; set; }

        public bool IsActionsShowing { get; set; }

        public FloatingButtonFlow Flow { get; set; }

        public bool EnableOverlay { get; set; }

        public FloatingButton() : this(FloatingButtonFlow.Up)
        {
        }

        public FloatingButton(FloatingButtonFlow flow, params Action[] actionItems)
        {
            Flow = flow;
            Actions.AddRange(actionItems);
            ClipChildren = false;
            overlay = new Overlay();
            overlay.Tapped.Handle(OverlayTapped);
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

            await BringToFront();
            IsShowing = true;
        }

        public virtual async Task Hide()
        {
            if (!IsShowing) return;

            overlay.Visible = false;
            Visible = false;
            IsShowing = false;
        }

        public async Task ShowActions()
        {
            if (IsActionsShowing) return;

            var tasks = new List<Task>();

            if (EnableOverlay)
            {
                if (Nav.CurrentPage.AllChildren.Contains(overlay))
                    overlay.Visible = true;
                else
                    await Nav.CurrentPage.Add(overlay);

                await overlay.BringToFront();
                tasks.Add(overlay.Animate(Animation.FadeDuration, x => x.Opacity(0.5f)));
            }

            tasks.Add(this.Animate(Animation.FadeDuration, x => x.Visible(false)));

            var animations = new Animations(Flow, Actions, this);
            tasks.AddRange(await animations.GetShowAnimations());
                
            await Task.WhenAll(tasks);
            await BringToFront();

            IsActionsShowing = true;
        }

        public async Task HideActions()
        {
            if (!IsActionsShowing) return;

            if (EnableOverlay)
            {
                overlay.Opacity = 0;
                overlay.Visible = false;
            }

            var animations = new Animations(Flow, Actions, this);
            var tasks = animations.GetHideAnimations();
            tasks.Add(this.Animate(Animation.FadeDuration, x => x.Visible(true)));

            await Task.WhenAll(tasks);

            IsActionsShowing = false;
        }

        public override async Task OnInitialized()
        {
            await this.On(x => x.Tapped, TappedHandler).SetPosition(Position);

            await base.OnInitialized();

            if (Parent != null)
                await Show();
        }

        public override async Task<TView> AddAt<TView>(int index, TView child, bool awaitNative = false)
        {
            var result = await base.AddAt(index, child, awaitNative);
            var actionButton = result as Action;
            if (actionButton != null && Actions.Lacks(actionButton))
                Actions.Add(actionButton);

            return result;
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

        Task TappedHandler()
        {
            if (Actions.Count == 0) return Task.CompletedTask;

            if (IsActionsShowing)
                return HideActions();

            return ShowActions();
        }

        Task OverlayTapped(TouchEventArgs arg)
        {
            return HideActions();
        }
    }
}