using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace SharpMobileCode.ModalPicker
{
    public delegate void ModalPickerDimissedEventHandler(object sender, EventArgs e);

    public class ModalPickerViewController : UIViewController
    {
        public event ModalPickerDimissedEventHandler OnModalPickerDismissed;
        const float _headerBarHeight = 40;

        public UIColor HeaderBackgroundColor { get; set; }
        public UIColor HeaderTextColor { get; set; }
        public string HeaderText { get; set; }

        public UIDatePicker DatePicker { get; set; }
        public UIPickerView PickerView { get; set; }
        private ModalPickerType _pickerType;
        public ModalPickerType PickerType 
        { 
            get { return _pickerType; }
            set
            {
                switch(value)
                {
                    case ModalPickerType.Date:
                        DatePicker = new UIDatePicker(RectangleF.Empty);
                        PickerView = null;
                        break;
                    case ModalPickerType.Custom:
                        DatePicker = null;
                        PickerView = new UIPickerView(RectangleF.Empty);
                        break;
                    default:
                        break;
                }

                _pickerType = value;
            }
        }

        UILabel _headerLabel;
        UIButton _doneButton;
        UIViewController _parent;
        UIView _internalView;

        public ModalPickerViewController(ModalPickerType pickerType, string headerText, UIViewController parent)
        {
            HeaderBackgroundColor = UIColor.White;
            HeaderTextColor = UIColor.Black;
            HeaderText = headerText;
            PickerType = pickerType;
            _parent = parent;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeControls();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            Show();
        }

        void InitializeControls()
        {
            View.BackgroundColor = UIColor.Clear;
            _internalView = new UIView();

            _headerLabel = new UILabel(new RectangleF(0, 0, 320/2, 44));
            _headerLabel.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            _headerLabel.BackgroundColor = HeaderBackgroundColor;
            _headerLabel.TextColor = HeaderTextColor;
            _headerLabel.Text = HeaderText;

            _doneButton = UIButton.FromType(UIButtonType.System);
            _doneButton.SetTitleColor(HeaderTextColor, UIControlState.Normal);
            _doneButton.BackgroundColor = UIColor.Clear;
            _doneButton.SetTitle("Done", UIControlState.Normal);
            _doneButton.TouchUpInside += DoneButtonTapped;

            switch(PickerType)
            {
                case ModalPickerType.Date:
                    DatePicker.BackgroundColor = UIColor.White;
                    _internalView.AddSubview(DatePicker);
                    break;
                case ModalPickerType.Custom:
                    PickerView.BackgroundColor = UIColor.White;
                    _internalView.AddSubview(PickerView);
                    break;
                default:
                    break;
            }
            _internalView.BackgroundColor = HeaderBackgroundColor;

            _internalView.AddSubview(_headerLabel);
            _internalView.AddSubview(_doneButton);

            Add(_internalView);
        }

        void Show(bool onRotate = false)
        {
            var doneButtonSize = new Size(71, 30);

            var width = UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.Portrait ? 
                _parent.View.Frame.Width : _parent.View.Frame.Height;

            var internalViewSize = SizeF.Empty;
            switch(_pickerType)
            {
                case ModalPickerType.Date:
                    DatePicker.Frame = RectangleF.Empty;
                    internalViewSize = new SizeF(width, DatePicker.Frame.Height + _headerBarHeight);
                    break;
                case ModalPickerType.Custom:
                    PickerView.Frame = RectangleF.Empty;
                    internalViewSize = new SizeF(width, PickerView.Frame.Height + _headerBarHeight);
                    break;
                default:
                    break;
            }

            var internalViewFrame = RectangleF.Empty;
            if (InterfaceOrientation == UIInterfaceOrientation.Portrait)
            {
                if (onRotate)
                {
                    internalViewFrame = new RectangleF(0, View.Frame.Height - internalViewSize.Height,
                                                       internalViewSize.Width, internalViewSize.Height);
                }
                else
                {
                    internalViewFrame = new RectangleF(0, View.Bounds.Height - internalViewSize.Height,
                                                       internalViewSize.Width, internalViewSize.Height);
                }
            }
            else
            {
                if (onRotate)
                {
                    internalViewFrame = new RectangleF(0, View.Frame.Width - internalViewSize.Height,
                                                       internalViewSize.Width, internalViewSize.Height);
                }
                else
                {
                    internalViewFrame = new RectangleF(0, View.Bounds.Width - internalViewSize.Height,
                                                       internalViewSize.Width, internalViewSize.Height);
                }
            }
            _internalView.Frame = internalViewFrame;

            switch(_pickerType)
            {
                case ModalPickerType.Date:
                    DatePicker.Frame = new RectangleF(DatePicker.Frame.X, _headerBarHeight, DatePicker.Frame.Width,
                                                      DatePicker.Frame.Height);
                    break;
                case ModalPickerType.Custom:
                    PickerView.Frame = new RectangleF(PickerView.Frame.X, _headerBarHeight, _internalView.Frame.Width,
                                                      PickerView.Frame.Height);
                    break;
                default:
                    break;
            }

            _headerLabel.Frame = new RectangleF(10, 4, _parent.View.Frame.Width - 100, 35);
            _doneButton.Frame = new RectangleF(internalViewFrame.Width - doneButtonSize.Width - 10, 7, doneButtonSize.Width, doneButtonSize.Height);
        }

        void DoneButtonTapped (object sender, EventArgs e)
        {
            DismissViewController(true, null);
            if(OnModalPickerDismissed != null)
            {
                OnModalPickerDismissed(sender, e);
            }
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            if (InterfaceOrientation == UIInterfaceOrientation.Portrait ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                Show(true);
                View.SetNeedsDisplay();
            }
        }
    }

    public enum ModalPickerType
    {
        Date = 0,
        Custom = 1
    }
}

