using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;

namespace SharpMobileCode.ModalPicker
{
    public class CustomPickerModel : UIPickerViewModel
    {
        private List<string> _itemsList;

        public CustomPickerModel(List<string> itemsList)
        {
            _itemsList = itemsList;
        }

        public override int GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override int GetRowsInComponent(UIPickerView picker, int component)
        {
            return _itemsList.Count;
        }

        public override UIView GetView(UIPickerView picker, int row, int component, UIView view)
        {
            var label = new UILabel(new RectangleF(0, 0, 300, 37))
            {
                BackgroundColor = UIColor.Clear,
                Text = _itemsList[row],
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(22.0f)
            };

            return label;
        }
    }
}

