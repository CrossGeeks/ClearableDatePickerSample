using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Widget;
using System.ComponentModel;
using NullableDatePicker.Droid;

[assembly: ExportRenderer(typeof(NullableDatePicker.Controls.NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace NullableDatePicker.Droid
{
    public class NullableDatePickerRenderer : ViewRenderer<Controls.NullableDatePicker, EditText>
    {
        DatePickerDialog _dialog;
        
        public NullableDatePickerRenderer(Context context) : base(context)
        {

        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Controls.NullableDatePicker> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new Android.Widget.EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = (NullableDatePicker.Controls.NullableDatePicker)this.Element;

            this.Control.Click += OnPickerClick;
            this.Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = (NullableDatePicker.Controls.NullableDatePicker)this.Element;

                if (this.Element.Format == entry.PlaceHolder)
                {
                    this.Control.Text = entry.PlaceHolder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            Controls.NullableDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
           {
               view.Date = e.Date;
               ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
               Control.ClearFocus();

               _dialog = null;
           }, year, month, day);

            _dialog.SetButton("Done", (sender, e) =>
           {
               this.Element.Format = this.Element._originalFormat;
               SetDate(_dialog.DatePicker.DateTime);
               this.Element.AssignValue();
           });
            _dialog.SetButton2("Clear", (sender, e) =>
            {
                this.Element.CleanDate();
                Control.Text = this.Element.Format;
            });
        }
    }
}
