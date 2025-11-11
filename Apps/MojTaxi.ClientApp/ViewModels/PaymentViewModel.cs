using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.ClientApp.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.ClientApp.ViewModels;

public partial class PaymentViewModel : ObservableObject
{
    public PaymentViewModel()
    {
        // Dummy dvije kartice
        Cards = new ObservableCollection<PaymentCard>
        {
            new PaymentCard
            {
                BrandIcon = "\ue8cb", // Visa
                BrandName = "VISA",
                MaskedNumber = "**** **** **** 4242",
                CardHolder = "Edin Begović",
                Expiry = "12/27",
                IsDefault = true
            },
            new PaymentCard
            {
                BrandIcon = "\ue8cc", // MasterCard
                BrandName = "MasterCard",
                MaskedNumber = "**** **** **** 5544",
                CardHolder = "Edin Begović",
                Expiry = "09/28",
                IsDefault = false
            }
        };
    }

    // kolekcija kartica za binding
    public ObservableCollection<PaymentCard> Cards { get; }

    [ObservableProperty]
    private PaymentCard? selectedCard;

    [RelayCommand]
    private async Task AddPaymentMethod()
    {
        // samo dummy poruka za sada
        await Application.Current.MainPage.DisplayAlert("Dodaj karticu", "Otvara se forma za dodavanje kartice (dummy).", "OK");
    }

    [RelayCommand]
    private async Task DeleteCard(PaymentCard card)
    {
        if (card is null) return;

        bool ok = await Application.Current.MainPage.DisplayAlert(
            "Brisanje kartice",
            $"Da li želiš obrisati karticu {card.MaskedNumber} ?",
            "Da",
            "Ne");

        if (!ok) return;

        Cards.Remove(card);

        // ako je bila default, postavi prvu kao default
        if (card.IsDefault && Cards.Any())
        {
            Cards[0].IsDefault = true;
            // notifikacija ručno ako treba (simple approach)
            SelectedCard = Cards[0];
        }
    }

    [RelayCommand]
    private Task SetDefault(PaymentCard card)
    {
        if (card is null) return Task.CompletedTask;

        foreach (var c in Cards)
            c.IsDefault = false;

        card.IsDefault = true;
        SelectedCard = card;
        return Task.CompletedTask;
    }
}
