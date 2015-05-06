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
using System.Collections.Generic;

using UIKit;

using SharpMobileCode.ModalPicker;
using Foundation;
using CoreGraphics;

namespace ModalPickerSample
{
    public partial class ModalPickerSampleViewController : UIViewController
    {
        private DateTime[] _customDates;

        public ModalPickerSampleViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            _customDates = new DateTime[] 
            { 
                DateTime.Now, DateTime.Now.AddDays(7), DateTime.Now.AddDays(7*2), 
                DateTime.Now.AddDays(7*3), DateTime.Now.AddDays(7*4), DateTime.Now.AddDays(7*5),
                DateTime.Now.AddDays(7*6), DateTime.Now.AddDays(7*7), DateTime.Now.AddDays(7*8),
                DateTime.Now.AddDays(7*9), DateTime.Now.AddDays(7*10), DateTime.Now.AddDays(7*11), 
                DateTime.Now.AddDays(7*12), DateTime.Now.AddDays(7*13), DateTime.Now.AddDays(7*14)
            };
        }
            

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            DatePickerButton.TouchUpInside += DatePickerButtonTapped;
            CustomPickerButton.TouchUpInside += CustomPickerButtonTapped;
            SelectDateTextField.ShouldBeginEditing += OnTextFieldShouldBeginEditing;
        }

        async void DatePickerButtonTapped (object sender, EventArgs e)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Select A Date", this)
            {
                HeaderBackgroundColor = UIColor.Red,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom
            };

            modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

            modalPicker.OnModalPickerDismissed += (s, ea) => 
            {
                var dateFormatter = new NSDateFormatter()
                {
                    DateFormat = "MMMM dd, yyyy"
                };

                PickedLabel.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
            };

            await PresentViewControllerAsync(modalPicker, true);
        }

        async void CustomPickerButtonTapped (object sender, EventArgs e)
        {
            //Create custom data source
            var customDatesList = new List<string>();
            foreach(var date in _customDates)
            {
                customDatesList.Add(date.ToString("ddd, MMM dd, yyyy"));
            }

            //Create the modal picker and style it as you see fit
            var modalPicker = new ModalPickerViewController(ModalPickerType.Custom, "Select A Date", this)
            {
                HeaderBackgroundColor = UIColor.Blue,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom
            };

            //Create the model for the Picker View
            modalPicker.PickerView.Model = new CustomPickerModel(customDatesList);

            //On an item is selected, update our label with the selected item.
            modalPicker.OnModalPickerDismissed += (s, ea) => 
            {
                var index = modalPicker.PickerView.SelectedRowInComponent(0);
                PickedLabel.Text = customDatesList[(int)index];
            };

            await PresentViewControllerAsync(modalPicker, true);
        }

        bool OnTextFieldShouldBeginEditing(UITextField textField)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Select A Date", this)
            {
                HeaderBackgroundColor = UIColor.Red,
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom
            };

            modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

            modalPicker.OnModalPickerDismissed += (s, ea) => 
            {
                var dateFormatter = new NSDateFormatter()
                {
                    DateFormat = "MMMM dd, yyyy"
                };

                textField.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
            };

            PresentViewController(modalPicker, true, null);

            return false;
        }
    }
}

