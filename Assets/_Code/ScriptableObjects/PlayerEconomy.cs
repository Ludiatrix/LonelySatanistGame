using System;
using LSG;
using LSG.Classes;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "LSG/Create Player Economy")]
public class PlayerEconomy : ScriptableObject
{
    public string PlayerName = "Smol";
    public int Tape = 0;
    public int Page = 0;
    public int Power = 0;
    public int Sanity = 20;
    public int Rizz = 1;
    public int WhiteSuitPoints = 0;

    private void OnEnable()
    {
        // Riza: ScriptableObjects are lifetime independent. So it's a good idea to reset on game start.
        // Also, I recommend not on start of application just in case we have a case where Player starts game without
        // starting restarting the application.
        GameEvents.StartGame?.AddListener(_ => Reset());
        GameEvents.PageRead?.AddListener(UpdatePageRewards);
        GameEvents.CardTaken?.AddListener(OnPageTaken);
        GameEvents.ChangeState?.AddListener(OnChangeState);
        GameEvents.DemonEncountered?.AddListener(OnDemonEncountered);
        EconomyEvents.SendPayload?.AddListener(OnSendPayload);
    }

    private void OnSendPayload(ModifierPayload payload)
    {
        Debug.Log($"Received Payload of the following: \n" +
                  $"Power: {payload.Power}" +
                  $"Sanity: {payload.Sanity}" +
                  $"Rizz: {payload.Rizz}" +
                  $"Tape: {payload.Tape}");
        Sanity += payload.Sanity;
        Rizz += payload.Rizz;
        ModifyPower(payload.Power);
        Tape += payload.Tape;
    }

    private void OnDemonEncountered(DemonData data)
    {
        Rizz++;
        Sanity--;

        if (Sanity <= 0)
        {
            GameEvents.ChangeState?.Invoke(Enums.GameState.LosePhase);
        }
    }

    private void OnDisable()
    {
        GameEvents.StartGame?.RemoveListener(_ => Reset());
        GameEvents.PageRead?.RemoveListener(UpdatePageRewards);
    }

    private void UpdatePageRewards()
    {
        Page++;
        GameEvents.TapeEarnedEvent?.Invoke();
    }
    
    private void OnPageTaken(CardData takenCard)
    {
        if (takenCard.Suit == Enums.Suit.White)
        {
            WhiteSuitPoints+= takenCard.PageModifier.Power;
        }

        if (WhiteSuitPoints > 8)
        {
            GameEvents.ChangeState?.Invoke(Enums.GameState.LosePhase);
        }
        else
        {
            GameEvents.WhiteSuitPointEarned?.Invoke(WhiteSuitPoints);
        }
    }
    
    private void OnChangeState(Enums.GameState newState)
    {
        WhiteSuitPoints = 0;
    }

    public void ModifyPower(int amountToChange)
    {
        Power += amountToChange;
        Tape += DataManager.Instance.MilestoneDataSource.GetTapeAmountAtPower(Power);
    }

    public void Reset()
    {
        Tape = 0;
        Page = 0;
        Power = 0;
        Sanity = 20;
        Rizz = 1;
        WhiteSuitPoints = 0;
    }
}
