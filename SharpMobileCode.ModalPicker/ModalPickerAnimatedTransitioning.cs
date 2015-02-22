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
    public class ModalPickerAnimatedTransitioning : UIViewControllerAnimatedTransitioning
    {
        public bool IsPresenting { get; set; }

        float _transitionDuration = 0.25f;

        public ModalPickerAnimatedTransitioning()
        {

        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return _transitionDuration;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;

            var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

            inView.AddSubview(toViewController.View);

            toViewController.View.Frame = CGRect.Empty;

            var startingPoint = GetStartingPoint(fromViewController.InterfaceOrientation);
            if (fromViewController.InterfaceOrientation == UIInterfaceOrientation.Portrait)
            {
                toViewController.View.Frame = new CGRect(startingPoint.X, startingPoint.Y, 
                                                             fromViewController.View.Frame.Width,
                                                             fromViewController.View.Frame.Height);
            }
            else
            {
                toViewController.View.Frame = new CGRect(startingPoint.X, startingPoint.Y, 
                                                             fromViewController.View.Frame.Height,
                                                             fromViewController.View.Frame.Width);
            }

            UIView.AnimateNotify(_transitionDuration,
                                 () =>
            {
                var endingPoint = GetEndingPoint(fromViewController.InterfaceOrientation);
                toViewController.View.Frame = new CGRect(endingPoint.X, endingPoint.Y, fromViewController.View.Frame.Width,
                                                                 fromViewController.View.Frame.Height);
                fromViewController.View.Alpha = 0.5f;
            },
                                 (finished) => transitionContext.CompleteTransition(true)
                                );
        }

        CGPoint GetStartingPoint(UIInterfaceOrientation orientation)
        {
            var screenBounds = UIScreen.MainScreen.Bounds;
            var coordinate = CGPoint.Empty;
            switch(orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    coordinate = new CGPoint(0, screenBounds.Height);
                    break;
                case UIInterfaceOrientation.LandscapeLeft:
                    coordinate = new CGPoint(screenBounds.Width, 0);
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    coordinate = new CGPoint(screenBounds.Width * -1, 0);
                    break;
                default:
                    coordinate = new CGPoint(0, screenBounds.Height);
                    break;
            }

            return coordinate;
        }

        CGPoint GetEndingPoint(UIInterfaceOrientation orientation)
        {
            var screenBounds = UIScreen.MainScreen.Bounds;
            var coordinate = CGPoint.Empty;
            switch(orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    coordinate = new CGPoint(0, 0);
                    break;
                case UIInterfaceOrientation.LandscapeLeft:
                    coordinate = new CGPoint(0, 0);
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    coordinate = new CGPoint(0, 0);
                    break;
                default:
                    coordinate = new CGPoint(0, 0);
                    break;
            }

            return coordinate;
        }
    }
}

