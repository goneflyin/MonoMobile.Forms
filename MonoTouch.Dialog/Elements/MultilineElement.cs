//
// EntryElement.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
namespace MonoTouch.Dialog
{
 public class MultilineElement : StringElement, IElementSizing {
     
     
     public MultilineElement (string caption , string value) : base (caption, value)
     {
         Value = value;
     }
     
     public override string Summary ()
     {
         return Value;
     }
     
     public override UITableViewCell GetCell (UITableView tv)
     {
         MultilineElementCell cell = (MultilineElementCell)tv.DequeueReusableCell(MultilineElementCell.KEY);
         if (cell == null){
             cell = new MultilineElementCell();
         } 
         
         cell.Update(this, tv);
             
         return cell;
     }
     
     public override bool Matches (string text)
     {
         return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
     }
     
     public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
     {
         ((MultilineElementCell)tableView.CellAt(path)).BecomeFirstResponder();
     }

     
     public float GetHeight (UITableView tableView, NSIndexPath indexPath)
     {
         return ComputeEntrySize(tableView).Height;
     }
     
     
     public SizeF ComputeEntrySize(UITableView tableView)
     {
         SizeF size = new SizeF (265, float.MaxValue);

         var extraCaptionSpace = string.IsNullOrEmpty(Caption)? 0 : 20;
         
         if (string.IsNullOrEmpty(Value)) 
             return new SizeF(265, Font.LineHeight+25+extraCaptionSpace);
         
         return new SizeF(265, tableView.StringSize (Value, Font, size, UILineBreakMode.WordWrap).Height + 35 + extraCaptionSpace);
     }
     
     
     public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
     
 }
 
}
