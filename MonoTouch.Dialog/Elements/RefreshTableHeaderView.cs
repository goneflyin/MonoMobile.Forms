//
// RefreshTableHeaderView.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace MonoTouch.Dialog
{
	public class RefreshTableHeaderView : UIView {
		static UIImage arrow = RefreshHeaderUtil.FromResource (null, "arrow.png");
		UIActivityIndicatorView activity;
		UILabel lastUpdateLabel, statusLabel;
		UIImageView arrowView;		
			
		public RefreshTableHeaderView (RectangleF rect) : base (rect)
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			
			BackgroundColor = new UIColor (0.88f, 0.9f, 0.92f, 1);
			lastUpdateLabel = new UILabel (){
				Font = UIFont.SystemFontOfSize (13f),
				TextColor = new UIColor (0.47f, 0.50f, 0.57f, 1),
				ShadowColor = UIColor.White, 
				ShadowOffset = new SizeF (0, 1),
				BackgroundColor = this.BackgroundColor,
				Opaque = true,
				TextAlignment = UITextAlignment.Center,
				AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin
			};
			AddSubview (lastUpdateLabel);
			
			statusLabel = new UILabel (){
				Font = UIFont.BoldSystemFontOfSize (14),
				TextColor = new UIColor (0.47f, 0.50f, 0.57f, 1),
				ShadowColor = lastUpdateLabel.ShadowColor,
				ShadowOffset = new SizeF (0, 1),
				BackgroundColor = this.BackgroundColor,
				Opaque = true,
				TextAlignment = UITextAlignment.Center,
				AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin
			};
			AddSubview (statusLabel);
			SetStatus (RefreshViewStatus.PullToReload);
			
			arrowView = new UIImageView (){
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = arrow,
				AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin
			};
			arrowView.Layer.Transform = CATransform3D.MakeRotation ((float) Math.PI, 0, 0, 1);
			AddSubview (arrowView);
			
			activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray) {
				HidesWhenStopped = true,
				AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin
			};
			AddSubview (activity);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var bounds = Bounds;
			
			lastUpdateLabel.Frame = new RectangleF (0, bounds.Height - 30, bounds.Width, 20);
			statusLabel.Frame = new RectangleF (0, bounds.Height-48, bounds.Width, 20);
			arrowView.Frame = new RectangleF (20, bounds.Height - 65, 30, 55);
			activity.Frame = new RectangleF (25, bounds.Height-38, 20, 20);
		}
		
		RefreshViewStatus status = (RefreshViewStatus) (-1);
		
		public virtual void SetStatus (RefreshViewStatus status)
		{
			if (this.status == status)
				return;
			
			string s = "Release to refresh";
	
			switch (status){
			case RefreshViewStatus.Loading:
				s = "Loading..."; 
				break;
				
			case RefreshViewStatus.PullToReload:
				s = "Pull down to refresh...";
				break;
			}
			statusLabel.Text = s;
		}
		
		public override void Draw (RectangleF rect)
		{
			var context = UIGraphics.GetCurrentContext ();
			context.DrawPath (CGPathDrawingMode.FillStroke);
			statusLabel.TextColor.SetStroke ();
			context.BeginPath ();
			context.MoveTo (0, Bounds.Height-1);
			context.AddLineToPoint (Bounds.Width, Bounds.Height-1);
			context.StrokePath ();
		}		
		
		public bool IsFlipped;
		
		public void Flip (bool animate)
		{
			UIView.BeginAnimations (null);
			UIView.SetAnimationDuration (animate ? .18f : 0);
			arrowView.Layer.Transform = IsFlipped 
				? CATransform3D.MakeRotation ((float)Math.PI, 0, 0, 1) 
				: CATransform3D.MakeRotation ((float)Math.PI * 2, 0, 0, 1);
				
			UIView.CommitAnimations ();
			IsFlipped = !IsFlipped;
		}
		
		DateTime lastUpdateTime;
		public DateTime LastUpdate {
			get {
				return lastUpdateTime;
			}
			set {
				if (value == lastUpdateTime)
					return;
				
				lastUpdateTime = value;
				if (value == DateTime.MinValue){
					lastUpdateLabel.Text = "Last Updated: never";
				} else 
					lastUpdateLabel.Text = String.Format ("Last Updated: {0:g}", value);
			}
		}
		
		public void SetActivity (bool active)
		{
			if (active){
				activity.StartAnimating ();
				arrowView.Hidden = true;
				SetStatus (RefreshViewStatus.Loading);
			} else {
				activity.StopAnimating ();
				arrowView.Hidden = false;
			}
		}	
	}
}
