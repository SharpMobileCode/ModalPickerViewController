ModalPickerViewController
===========

The ModalPickerViewController is a view controller that is designed to replace [ActionSheetDatePicker](http://developer.xamarin.com/recipes/ios/standard_controls/actionsheet/actionsheet_date_picker/).  The ActionSheetDatePicker was an example provided by Xamarin to show a nice looking Date Picker that did not take up the whole screen.

Though this did do the job, the UIActionSheet was not designed to be subclassed or views added to its hierarchy.  Starting with iOS 7, Apple started issuing runtime warnings, but did not crash the app.  The warnings looked something like this:

> Error: CGContextSetFillColorWithColor: invalid context 0x0. This is a serious error. 
> This application, or a library it uses, is using an invalid context  and is thereby contributing 
> to an overall degradation of system stability and reliability. 
> This notice is a courtesy: please fix this problem. 
> It will become a fatal error in an upcoming update.

Notice the last two lines of the warning.  Apple was warning developers in iOS 7 that this was not the way the UIActionSheet was intended to be used, and that it will **BREAK** in a future version of iOS.  Well, it now crashes on iOS 8.

## Example Usage
Please see the blog post at [http://sharpmobilecode.com](http://sharpmobilecode.com) for example usage

## Areas For Improvement
When you rotate between Portrait and Landscape, the transition is not as smooth as I would like it to be.  Feel free to submit a pull request and take a shot at it!  :)

## Contributors
Ruben Macias @sharpmobilecode

## License
Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html)
