using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ClientApp.Models;
using MojTaxi.Core.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class PaymentViewModel : ObservableObject
{
    public PaymentViewModel()
    {
        Cards = new ObservableCollection<PaymentCardVm>
        {
            new PaymentCardVm(new PaymentCard
            {
                BrandIcon = "\ue870",
                BrandName = "VISA",
                MaskedNumber = "**** **** **** 4242",
                CardHolder = "Edin Begović",
                Expiry = "12/27",
                IsDefault = true
            }),
            new PaymentCardVm(new PaymentCard
            {
                BrandIcon = "\ue870",
                BrandName = "MasterCard",
                MaskedNumber = "**** **** **** 5544",
                CardHolder = "Edin Begović",
                Expiry = "09/28",
                IsDefault = false
            })
        };

        SelectedCard = Cards.FirstOrDefault(c => c.IsDefault);
    }

    public ObservableCollection<PaymentCardVm> Cards { get; }

    [ObservableProperty]
    private PaymentCardVm? selectedCard;

    [RelayCommand]
    private void SetDefault(PaymentCardVm card)
    {
        if (card == null)
            return;

        foreach (var c in Cards)
            c.IsDefault = false;

        card.IsDefault = true;
        SelectedCard = card;
    }
}
