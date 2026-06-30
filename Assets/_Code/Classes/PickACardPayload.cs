using System.Collections.Generic;
using LSG.ScriptableObjects;
using static LSG.Core.Enums;

public class PickACardPayload
{
    public string Reason;
    public List<CardData> Cards;
    public int AmountAbleToChoose = 1;
    public PickACardAfterEffectState StateAfterChoice = PickACardAfterEffectState.ShuffleRestIntoDeck;

    public PickACardPayload(string reason, List<CardData> cards,int amount, PickACardAfterEffectState state)
    {
        Reason = reason;
        Cards = cards;
        AmountAbleToChoose = amount;
        StateAfterChoice = state;
    }
}