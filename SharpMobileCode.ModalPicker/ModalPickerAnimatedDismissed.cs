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
using UIKit;
using CoreGraphics;

namespace SharpMobileCode.ModalPicker
{
    public class ModalPickerAnimatedDismissed : UIViewControllerAnimatedTransitioning
    {
        public bool IsPresenting { get; set; }
        float _transitionDuration = 0.25f;

        public ModalPickerAnimatedDismissed()
        {
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return _transitionDuration;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {

            var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

            var screenBounds = UIScreen.MainScreen.Bounds;
            var fromFrame = fromViewController.View.Frame;

            UIView.AnimateNotify(_transitionDuration, 
                                 () =>
            {
                toViewController.View.Alpha = 1.0f;

                switch(fromViewController.InterfaceOrientation)
                {
                    case UIInterfaceOrientation.Portrait:
                        fromViewController.View.Frame = new CGRect(0, screenBounds.Height, fromFrame.Width, fromFrame.Height);
                        break;
                    case UIInterfaceOrientation.LandscapeLeft:
                        fromViewController.View.Frame = new CGRect(screenBounds.Width, 0, fromFrame.Width, fromFrame.Height);
                        break;
                    case UIInterfaceOrientation.LandscapeRight:
                        fromViewController.View.Frame = new CGRect(screenBounds.Width * -1, 0, fromFrame.Width, fromFrame.Height);
                        break;
                    default:
                        break;
                }

            },
                                 (finished) => transitionContext.CompleteTransition(true));
        }
    }
}

