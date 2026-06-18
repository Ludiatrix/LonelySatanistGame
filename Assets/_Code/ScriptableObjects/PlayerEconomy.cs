using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "LSG/Create Player Economy")]
public class PlayerEconomy : ScriptableObject
{
    public int Tape = 0;
    public int Page = 0;
    public int Power = 0;
    public int Sanity = 20;
    public int Rizz = 1;
    public int WhiteSuitPoints = 0;
    public MilestoneData MilestoneDataSource;
    public PlayerDeck PlayerDeckSource;

    public float NormalizedPower => Mathf.InverseLerp(0, 14, Power);

    private void OnEnable()
    {
        // Riza: ScriptableObjects are lifetime independent. So it's a good idea to reset on game start.
        // Also, I recommend not on start of application just in case we have a case where Player starts game without
        // starting restarting the application.
        GameEvents.StartGame?.AddListener(_ => Reset());
        GameEvents.PageRead?.AddListener(UpdatePageRewards);
        GameEvents.PageTaken?.AddListener(OnPageTaken);
        GameEvents.ChangeState?.AddListener(OnChangeState);
    }

    private void OnDisable()
    {
        GameEvents.StartGame?.RemoveListener(_ => Reset());
        GameEvents.PageRead?.RemoveListener(UpdatePageRewards);
    }

    private void UpdatePageRewards()
    {
        Page++;
        Power++;
        Tape = MilestoneDataSource.GetTapeAmountAtPower(Power);
        GameEvents.TapeEarnedEvent?.Invoke();
    }
    
    private void OnPageTaken(PageData takenPage)
    {
        if (takenPage.Suit == Enums.Suit.White)
        {
            WhiteSuitPoints++;
        }

        if (WhiteSuitPoints > 8)
        {
            GameEvents.ChangeState?.Invoke(Enums.GameState.EndPhase);
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

    private void Reset()
    {
        Tape = 0;
        Page = 0;
        Power = 0;
        Sanity = 20;
        Rizz = 1;
        WhiteSuitPoints = 0;
        PlayerDeckSource.SetToDefault();
        MilestoneDataSource.Reset();
    }
}
