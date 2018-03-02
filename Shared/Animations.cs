namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Animations
    {
        readonly FloatingButtonFlow flow;
        readonly List<FloatingButton.Action> actions;
        readonly FloatingButton button;
        float totalChange;
        bool isFirstAction = true;

        public Animations(FloatingButtonFlow flow, List<FloatingButton.Action> actions,
            FloatingButton button)
        {
            this.flow = flow;
            this.actions = actions;
            this.button = button;
        }

        public async Task<IList<Task>> GetShowAnimations()
        {
            var animator = GetShowAnimation(flow);
            var tasks = new List<Task>();

            foreach (var action in actions)
            {
                if (action.Parent == null)
                    await Nav.CurrentPage.Add(action);

                await action.BringToFront();

                var animation = animator(action);

                tasks.Add(animation);
            }

            return tasks;
        }

        public IList<Task> GetHideAnimations()
        {
            var animator = GetHideAnimation(flow);
            var tasks = new List<Task>();

            foreach (var action in actions)
            {
                var animation = animator(action);

                tasks.Add(animation);
            }

            return tasks;
        }

        Func<FloatingButton.Action, Task> GetShowAnimation(FloatingButtonFlow floatingButtonFlow)
        {
            if (floatingButtonFlow == FloatingButtonFlow.Up)
                return UpShowAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Right)
                return RightShowAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Down)
                return DownShowAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Left)
                return LeftShowAnimation;

            return ScaleUpShowAnimation;
        }

        Func<FloatingButton.Action, Task> GetHideAnimation(FloatingButtonFlow floatingButtonFlow)
        {
            if (floatingButtonFlow == FloatingButtonFlow.Up)
                return UpHideAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Right)
                return RightHideAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Down)
                return DownHideAnimation;

            if (floatingButtonFlow == FloatingButtonFlow.Left)
                return LeftHideAnimation;

            return ScaleUpHideAnimation;
        }

        Task UpShowAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX - action.textView.ActualWidth - (action.imageWrapper.ActualWidth - button.ActualWidth) / 2;
            var y = button.ActualY;

            action.X(x);
            action.Y(button.ActualY);

            var currentYchange = isFirstAction ? 0 : action.ActualHeight + button.ActionsPadding;
            y = y - (currentYchange + totalChange);

            totalChange += currentYchange;
            isFirstAction = false;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.Y(y);
                action.Opacity(1);
            }));
        }

        Task UpHideAnimation(FloatingButton.Action action)
        {
            var y = button.ActualY;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.Y(y);
                action.Opacity(0);
            }));
        }

        Task RightShowAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX - action.textView.ActualWidth - (action.imageWrapper.ActualWidth - button.ActualWidth) / 2;
            if (isFirstAction)
            {
                x += action.textView.ActualWidth;
            }

            action.X(x);
            action.Y(button.ActualY);

            var currentXChange = isFirstAction ? 0 : action.ActualWidth + button.ActionsPadding;
            x = x + (currentXChange + totalChange);

            if (isFirstAction)
            {
                currentXChange = action.textView.ActualWidth;
            }
            totalChange += currentXChange;
            isFirstAction = false;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.X(x);
                action.Opacity(1);
            }));
        }

        Task RightHideAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.X(x);
                action.Opacity(0);
            }));
        }

        Task DownShowAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX - action.textView.ActualWidth - (action.imageWrapper.ActualWidth - button.ActualWidth) / 2;
            var y = button.ActualY;

            action.X(x);
            action.Y(button.ActualY);

            var currentYchange = isFirstAction ? 0 : action.ActualHeight + button.ActionsPadding;
            y = y + (currentYchange + totalChange);

            totalChange += currentYchange;
            isFirstAction = false;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.Y(y);
                action.Opacity(1);
            }));
        }

        Task DownHideAnimation(FloatingButton.Action action)
        {
            var y = button.ActualY;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.Y(y);
                action.Opacity(0);
            }));
        }

        Task LeftShowAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX - action.textView.ActualWidth - (action.imageWrapper.ActualWidth - button.ActualWidth) / 2;

            action.X(x);
            action.Y(button.ActualY);

            var currentXChange = isFirstAction ? 0 : action.ActualWidth + button.ActionsPadding;
            x = x - (currentXChange + totalChange);

            if (isFirstAction)
            {
                currentXChange = action.textView.ActualWidth;
            }
            totalChange += currentXChange;
            isFirstAction = false;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.X(x);
                action.Opacity(1);
            }));
        }

        Task LeftHideAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.X(x);
                action.Opacity(0);
            }));
        }

        Task ScaleUpShowAnimation(FloatingButton.Action action)
        {
            var x = button.ActualX - action.textView.ActualWidth - (action.imageWrapper.ActualWidth - button.ActualWidth) / 2;
            var y = button.ActualY;

            var currentYchange = isFirstAction ? 0 : action.ActualHeight + button.ActionsPadding;
            y = y - (currentYchange + totalChange);

            action.X(x);
            action.Y(y);
            action.ScaleX(0.5f);
            action.ScaleY(0.5f);

            totalChange += currentYchange;
            isFirstAction = false;

            return action.Animate(button.AnimationFactory(() =>
            {
                action.ScaleX(1f);
                action.ScaleY(1f);
                action.Opacity(1);
            }));
        }

        Task ScaleUpHideAnimation(FloatingButton.Action action)
        {
            return action.Animate(button.AnimationFactory(() =>
            {
                action.ScaleX(0.5f);
                action.ScaleY(0.5f);
                action.Opacity(0);
            }));
        }
    }
}