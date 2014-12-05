/*
 * Copyright (C) 2014 
 * Author: Ruben Macias
 * http://sharpmobilecode.com @SharpMobileCode
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

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
		public UIFont HeaderFont { get; set; }

		public UIButton DoneButton { get; set; }
		public string DoneButtonText { get; set; }

		public UIButton CancelButton { get; set; }
		public string CancelButtonText { get; set; }

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
		UIViewController _parent;
		UIView _internalView;

		public ModalPickerViewController(ModalPickerType pickerType, string headerText, UIViewController parent)
		{
			HeaderBackgroundColor = UIColor.White;
			HeaderTextColor = UIColor.Black;
			HeaderFont = UIFont.SystemFontOfSize (UIFont.SystemFontSize);
			HeaderText = headerText;
			PickerType = pickerType;
			_parent = parent;

			DoneButtonText = "Done";
			CancelButtonText = "Cancel";

			CancelButton = UIButton.FromType(UIButtonType.System);
			CancelButton.SetTitleColor(HeaderTextColor, UIControlState.Normal);
			CancelButton.BackgroundColor = UIColor.Clear;
			CancelButton.SetTitle(CancelButtonText, UIControlState.Normal);
			CancelButton.TouchUpInside += CancelButtonTapped;

			DoneButton = UIButton.FromType(UIButtonType.System);
			DoneButton.SetTitleColor(HeaderTextColor, UIControlState.Normal);
			DoneButton.BackgroundColor = UIColor.Clear;
			DoneButton.SetTitle(DoneButtonText, UIControlState.Normal);
			DoneButton.TouchUpInside += DoneButtonTapped;
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
			_headerLabel.TextAlignment = UITextAlignment.Center;
			_headerLabel.Font = HeaderFont;

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
			_internalView.AddSubview (CancelButton);
			_internalView.AddSubview(DoneButton);

			Add(_internalView);
		}

		void Show(bool onRotate = false)
		{
			var buttonSize = new Size(71, 30);

			var width = _parent.View.Frame.Width;

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
					internalViewFrame = new RectangleF(0, View.Bounds.Height - internalViewSize.Height,
						internalViewSize.Width, internalViewSize.Height);
				}
				else
				{
					internalViewFrame = new RectangleF(0, View.Frame.Height - internalViewSize.Height,
						internalViewSize.Width, internalViewSize.Height);
				}
			}
			_internalView.Frame = internalViewFrame;

			switch(_pickerType)
			{
			case ModalPickerType.Date:
				var pickerWidth = DatePicker.Frame.Width;
				var xOffset = (width - pickerWidth) / 2.0f;
				DatePicker.Frame = new RectangleF(xOffset, _headerBarHeight, pickerWidth,
					DatePicker.Frame.Height);
				break;
			case ModalPickerType.Custom:
				PickerView.Frame = new RectangleF(PickerView.Frame.X, _headerBarHeight, _internalView.Frame.Width,
					PickerView.Frame.Height);
				break;
			default:
				break;
			}

			_headerLabel.Frame = new RectangleF(20+buttonSize.Width, 4, _parent.View.Frame.Width - (40+2*buttonSize.Width), 35);
			DoneButton.Frame = new RectangleF(internalViewFrame.Width - buttonSize.Width - 10, 7, buttonSize.Width, buttonSize.Height);
			CancelButton.Frame = new RectangleF(10, 7, buttonSize.Width, buttonSize.Height);
		}

		void DoneButtonTapped (object sender, EventArgs e)
		{
			DismissViewController(true, null);
			if(OnModalPickerDismissed != null)
			{
				OnModalPickerDismissed(sender, e);
			}
		}

		void CancelButtonTapped (object sender, EventArgs e)
		{
			DismissViewController(true, null);
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

