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
    public int Rizz = 0;
    public int WhiteSuitPoints = 0;

    private void OnEnable()
    {
        // Riza: ScriptableObjects are lifetime independent. So it's a good idea to reset on game start.
        // Also, I recommend not on start of application just in case we have a case where Player starts game without
        // starting restarting the application.
        GameEvents.StartGame?.AddListener(OnStartGame);
        GameEvents.PageRead?.AddListener(UpdatePageRewards);
        GameEvents.CardTaken?.AddListener(OnPageTaken);
        PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
        GameEvents.DemonEncountered?.AddListener(OnDemonEncountered);
        EconomyEvents.SendPayload?.AddListener(OnSendPayload);
    }

    private void OnStartGame(Enums.GameState _) => Reset();

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

        // A card read can drain Sanity to 0 or below; summon The Book if so.
        TriggerTheBookIfOutOfSanity();
    }

    /// <summary>
    /// When Sanity hits 0 or lower, summon The Book instead of showing the lose screen.
    /// </summary>
    private void TriggerTheBookIfOutOfSanity()
    {
        if (Sanity > 0)
        {
            return;
        }

        var pool = DataManager.Instance.DemonDatingPoolSource;
        pool.QueueForcedEncounter(pool.TheBook);
        GameEvents.ChangeState?.Invoke(Enums.GameState.EncounterPhase);
    }

    private void OnDemonEncountered(DemonData data)
    {
        var pool = DataManager.Instance.DemonDatingPoolSource;

        // Papiyawn / The Book are forced lose-condition encounters; they must not
        // tick the normal per-encounter stat changes (and must not re-trigger).
        if (pool.IsForcedEncounterDemon(data))
        {
            return;
        }

        // NOTE: the longshot Rizz bonus is granted after a failed date roll
        // (see EncounterPhase.OnDiceRollResult), not on encounter.
        Sanity--;

        // The per-encounter Sanity hit can take us to 0 or below; summon The Book if so.
        TriggerTheBookIfOutOfSanity();
    }

    private void OnDisable()
    {
        // Must mirror OnEnable exactly. This is a ScriptableObject, so OnEnable/OnDisable
        // fire across play-mode entries and domain reloads; any listener left subscribed
        // accumulates and gets applied multiple times (e.g. a card's payload counted twice).
        GameEvents.StartGame?.RemoveListener(OnStartGame);
        GameEvents.PageRead?.RemoveListener(UpdatePageRewards);
        GameEvents.CardTaken?.RemoveListener(OnPageTaken);
        PhaseEvents.SummoningPhaseStarted?.RemoveListener(OnSummoningPhaseStarted);
        GameEvents.DemonEncountered?.RemoveListener(OnDemonEncountered);
        EconomyEvents.SendPayload?.RemoveListener(OnSendPayload);
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

        if (WhiteSuitPoints > 7)
        {
            // Too many Daggers read: summon Papiyawn instead of the lose screen.
            var pool = DataManager.Instance.DemonDatingPoolSource;
            pool.QueueForcedEncounter(pool.Papiyawn);
            GameEvents.ChangeState?.Invoke(Enums.GameState.EncounterPhase);
        }
        else
        {
            GameEvents.WhiteSuitPointEarned?.Invoke(WhiteSuitPoints);
        }
    }
    
    private void OnSummoningPhaseStarted()
    {
        // Dagger power is per-summoning. Reset at the start of each summoning round
        // (which fires before the first page is drawn) rather than on every phase
        // change — otherwise the first card of the round gets counted and then wiped.
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
        Rizz = 0;
        WhiteSuitPoints = 0;
    }
}
