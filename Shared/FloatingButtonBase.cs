﻿namespace Zebble
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class FloatingButtonBase : View
    {
        protected const float CONTAINER_MARGIN = 10;
        readonly Button Button = new Button();

        public string ImagePath
        {
            get { return Button.BackgroundImagePath; }
            set
            {
                if (Button.BackgroundImagePath == value) return;
                Button.BackgroundImagePath = value;
            }
        }

        FloatingButton.ButtonAlignments buttonAlignment;
        public FloatingButton.ButtonAlignments Alignment
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
                shadowColor = value;
                if (Parent != null)
                    this.BoxShadow(6, 0, 6, color: shadowColor);
            }
        }

        protected FloatingButtonBase() { Visible = false; }

        public override async Task OnInitializing()
        {
            await base.OnInitializing();

            Button.Tapped.Handle(Tapped.Raise);
            Button.Padding(all: 0);
            Button.BackgroundColor = Colors.Transparent;

            if (ShadowColor == null || ShadowColor == Colors.Transparent) ShadowColor = Colors.Silver;

            var maxZindex = Nav.CurrentPage.AllDescendents().Except(d => d is FloatingButton).Max(c => c.ZIndex);
            Tapped.Handle(() => { Button.Flash(); });
            this.Absolute().ZIndex(maxZindex + 1);

            await Add(Button);
        }

        protected virtual void UpdatePosition()
        {
            switch (Alignment)
            {
                default:
                case FloatingButton.ButtonAlignments.BottomRight:
                    X.BindTo(Root.Width, Width, Margin.Right, (rw, fbw, mr) => rw - (fbw + mr + CONTAINER_MARGIN));
                    Y.BindTo(Root.Height, Height, Margin.Bottom, (rh, fbh, mb) => rh - (fbh + mb + CONTAINER_MARGIN));
                    break;
                case FloatingButton.ButtonAlignments.BottomLeft:
                    X.BindTo(Margin.Left, ml => ml + CONTAINER_MARGIN);
                    Y.BindTo(Root.Height, Height, Margin.Bottom, (rh, fbh, mb) => rh - (fbh + mb + CONTAINER_MARGIN));
                    break;
                case FloatingButton.ButtonAlignments.TopLeft:
                    X.BindTo(Margin.Left, ml => ml + CONTAINER_MARGIN);
                    Y.BindTo(Margin.Top, mt => mt + CONTAINER_MARGIN);
                    break;
                case FloatingButton.ButtonAlignments.TopRight:
                    X.BindTo(Root.Width, Width, Margin.Right, (rw, fbw, mr) => rw - (fbw + mr + CONTAINER_MARGIN));
                    Y.BindTo(Margin.Top, mt => mt + CONTAINER_MARGIN);
                    break;
                case FloatingButton.ButtonAlignments.Custom:
                    break;
            }
        }
    }
}
