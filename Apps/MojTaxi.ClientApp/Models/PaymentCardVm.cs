using CommunityToolkit.Mvvm.ComponentModel;
using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.ClientApp.Models
{

    public partial class PaymentCardVm : ObservableObject
    {
        public PaymentCard Model { get; }

        public PaymentCardVm(PaymentCard model)
        {
            Model = model;
            IsDefault = model.IsDefault;
        }

        public string BrandIcon => Model.BrandIcon;
        public string BrandName => Model.BrandName;
        public string MaskedNumber => Model.MaskedNumber;
        public string CardHolder => Model.CardHolder;
        public string Expiry => Model.Expiry;

        [ObservableProperty]
        private bool isDefault;

        partial void OnIsDefaultChanged(bool value)
        {
            Model.IsDefault = value;
        }
    }
}
