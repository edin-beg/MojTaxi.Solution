using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MojTaxi.Client.Models;
using System.Collections.ObjectModel;

namespace MojTaxi.Client.ViewModels;

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

    public ObservableCollection<PaymentCard> Cards { get; }

    [ObservableProperty]
    private PaymentCard? selectedCard;

    [RelayCommand]
    private async Task AddPaymentMethod()
    {
        var page = Application.Current?.Windows[0].Page;
        if (page != null)
            await page.DisplayAlertAsync("Dodaj karticu", "Otvara se forma za dodavanje kartice (dummy).", "OK");
    }

    [RelayCommand]
    private async Task DeleteCard(PaymentCard card)
    {
        if (card is null) return;

        var page = Application.Current?.Windows[0].Page;
        if (page == null) return;

        bool ok = await page.DisplayAlertAsync(
            "Brisanje kartice",
            $"Da li želiš obrisati karticu {card.MaskedNumber} ?",
            "Da",
            "Ne");

        if (!ok) return;

        Cards.Remove(card);

        if (card.IsDefault && Cards.Any())
        {
            Cards[0].IsDefault = true;
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
