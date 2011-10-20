
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
namespace MonoTouch.Dialog
{
	public class MultilineEntryElementCell: UITableViewCell {
		
		public static NSString KEY = new NSString ("EntryElement");
			
		protected UITextView _entry;
		protected MultilineEntryElement _element;

        private UITableViewStyle tableStyle;
		
		public MultilineEntryElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
				
		}
		
		public void Update(MultilineEntryElement element, UITableView tableView){
			_element = element;
			
			if (_entry==null){
				PrepareEntry(tableView);
			}

            tableStyle = tableView.Style;

			_entry.Text = element.Value ?? "";
			_entry.SecureTextEntry = element.IsPassword;
			_entry.AutocapitalizationType = element.AutoCapitalize;
			_entry.KeyboardType = element.KeyboardType;
			TextLabel.Text = element.Caption;
			
			tableView.BeginUpdates();
			tableView.EndUpdates();
			
		}
			
		public override bool BecomeFirstResponder ()
		{
			return _entry.BecomeFirstResponder();
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_element = null;
		}
			
		protected virtual void PrepareEntry(UITableView tableview){

            var topspace = string.IsNullOrEmpty(_element.Caption)? 0 : 25;
            var leftspace = 5;
            var rightspace = tableStyle == UITableViewStyle.Grouped ? 40 : 0;
            var bottomspace = 40;
			_entry = new UITextView(new RectangleF(leftspace,topspace,Frame.Width-rightspace, Frame.Height-bottomspace));
			
			TextLabel.BackgroundColor = UIColor.Clear;
			TextLabel.TextColor = UIColor.Black;
			TextLabel.Font = UIFont.SystemFontOfSize(14);
			_entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleLeftMargin;
			_entry.Editable = true;
            _entry.BackgroundColor = UIColor.Clear;
			_entry.Font = Font;
			_entry.ScrollEnabled = false;
			_entry.Bounces = false;
			_entry.ContentMode = UIViewContentMode.TopLeft;
			_entry.EnablesReturnKeyAutomatically = true;
			_entry.Changed += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				_entry.Frame = new RectangleF(leftspace,topspace,Frame.Width-rightspace, Frame.Height-bottomspace);
				tableview.EndUpdates();
			};
			_entry.Ended += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				tableview.EndUpdates();
				_entry.Frame = new RectangleF(leftspace,topspace,Frame.Width-rightspace, Frame.Height-bottomspace);
			};

			_entry.Started += delegate {
				EntryElement self = null;
				var returnType = UIReturnKeyType.Default;
				
				foreach (var e in (_element.Parent as Section).Elements){
					if (e == _element)
						self = _element;
					else if (self != null && e is EntryElement)
						returnType = UIReturnKeyType.Next;
				}
				_entry.ReturnKeyType = returnType;
			};
				
			ContentView.AddSubview (_entry);
			ContentView.BringSubviewToFront(TextLabel);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			
			TextLabel.Frame = new RectangleF(8,0,300,30);
			
			var topspace = string.IsNullOrEmpty(_element.Caption)? 0 : 25;
            var leftspace = 5;
            var rightspace = tableStyle == UITableViewStyle.Grouped ? 40 : 0;
            var bottomspace = 40;
			_entry.Frame = new RectangleF(leftspace,topspace,Frame.Width-rightspace, Frame.Height-bottomspace);
		}
		
		public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
	}
}
