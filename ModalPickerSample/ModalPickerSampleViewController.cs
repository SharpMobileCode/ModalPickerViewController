using System;
using System.Collections.Generic;

using MonoTouch.UIKit;

using SharpMobileCode.ModalPicker;
using MonoTouch.Foundation;

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
                PickedLabel.Text = customDatesList[index];
            };

            await PresentViewControllerAsync(modalPicker, true);
        }

    }
}

