namespace Zebble
{
    using System;
    using System.Collections.Generic;

    public partial class FloatingButton
    {
        public class ActionButtonCollection
        {
            public ActionButton[] ActionItems;

            public readonly List<FloatingButton> ActionButtons = new List<FloatingButton>();
            public ActionButtonAlignment Alignment { get; set; } = ActionButtonAlignment.Top;

            public ActionButtonCollection(ActionButtonAlignment alignment, params ActionButton[] items)
            {
                Alignment = alignment;
                ActionItems = items;
                Adapt();
            }

            public ActionButtonCollection(params ActionButton[] items)
            {
                ActionItems = items;
                Adapt();
            }

            public void Update()
            {
                ActionButtons.Clear();
                Adapt();
            }

            protected virtual void Adapt()
            {
                var index = 1;
                foreach (var item in ActionItems)
                {
                    var flButton = new FloatingButton
                    {
                        Alignment = ButtonAlignment.Custom,
                        Size = item.Size,
                        ImagePath = item.ImagePath,
                        Color = item.Color,
                        Id = "ActionItem".WithSuffix(index.ToString())
                    };

                    flButton.Tapped.Handle(item.Tapped.Raise);
                    ActionButtons.Add(flButton);
                    index++;
                }
            }
        }
    }
}
