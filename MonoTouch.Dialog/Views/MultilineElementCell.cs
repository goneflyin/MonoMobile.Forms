
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
 public class MultilineElementCell: UITableViewCell {
     
     public static NSString KEY = new NSString ("MultiLineElement");
         
     protected UITextView _entry;
     protected MultilineElement _element;

        private UITableViewStyle tableStyle;
     
     public MultilineElementCell():base(UITableViewCellStyle.Default, KEY){
         SelectionStyle = UITableViewCellSelectionStyle.None;
             
     }
     
     public virtual void Update(MultilineElement element, UITableView tableView){
         _element = element;
         
         if (_entry==null){
             PrepareEntry(tableView);
         }

            tableStyle = tableView.Style;

         _entry.Text = element.Value ?? "";
         TextLabel.Text = element.Caption;
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

            SizeF size = new SizeF (255, float.MaxValue);
            var topspace = string.IsNullOrEmpty(_element.Caption)? 0 : 25;
            var leftspace = 5;
            var rightspace = tableStyle == UITableViewStyle.Grouped ? 40 : 0;
            var bottomspace = 40;
            var height = new SizeF(265, tableview.StringSize ("", Font, size, UILineBreakMode.WordWrap).Height).Height + 10;
         _entry = new UITextView(new RectangleF(leftspace,topspace,Frame.Width-rightspace, height));
            _entry.UserInteractionEnabled = false;
         
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

                var heightUpdate = new SizeF(265, tableview.StringSize (_entry.Text, Font, size, UILineBreakMode.WordWrap).Height).Height + 10;
             tableview.BeginUpdates();
             _entry.Frame = new RectangleF(leftspace,topspace,Frame.Width-rightspace, heightUpdate);
             tableview.EndUpdates();
         };

         ContentView.AddSubview (_entry);
         ContentView.BringSubviewToFront(TextLabel);
     }
     
     public override void LayoutSubviews ()
     {
         base.LayoutSubviews ();

            SizeF size = new SizeF (255, float.MaxValue);
         TextLabel.Frame = new RectangleF(8,0,300,30);
         
         var topspace = string.IsNullOrEmpty(_element.Caption)? 0 : 25;
            var leftspace = 5;
            var rightspace = tableStyle == UITableViewStyle.Grouped ? 40 : 0;
            var bottomspace = 40;
            var heightUpdate = new SizeF(265, this.StringSize (_entry.Text, Font, size, UILineBreakMode.WordWrap).Height).Height + 10;
             _entry.Frame = new RectangleF(leftspace,topspace,Frame.Width-rightspace, heightUpdate);
     }
     
     public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
 }
}
