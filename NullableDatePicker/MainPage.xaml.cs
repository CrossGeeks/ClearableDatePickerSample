using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace NullableDatePicker
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public DateTime? MyDate { get; set; } 
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
    }
}
