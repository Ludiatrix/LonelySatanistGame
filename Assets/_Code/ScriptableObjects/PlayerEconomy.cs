using LSG;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "LSG/Create Player Economy")]
public class PlayerEconomy : ScriptableObject
{
    public int Tape = 0;
    public int Page = 0;
    public int Milestone = 0;
    public int Power = 0;
    public int Sanity = 20;
    public int Rizz = 1;
    public MilestoneData MilestoneDataSource;

    private void OnEnable()
    {
        // Riza: ScriptableObjects are lifetime independent. So it's a good idea to reset on game start.
        // Also, I recommend not on start of application just in case we have a case where Player starts game without
        // starting restarting the application.
        GameEvents.StartGame?.AddListener(_ => Reset());
        GameEvents.PageRead?.AddListener(UpdatePageRewards);
    }

    private void OnDisable()
    {
        GameEvents.StartGame?.RemoveListener(_ => Reset());
        GameEvents.PageRead?.RemoveListener(UpdatePageRewards);
    }

    private void UpdatePageRewards()
    {
        Page++;
        Milestone++;
        Power++;
        Sanity--;
        int tapeAmount = MilestoneDataSource.Milestones[Milestone].TapeAmount;
        if (tapeAmount > 0)
        {
            Tape = tapeAmount;
            GameEvents.TapeEarnedEvent?.Invoke();
        }
    }

private void Reset()
    {
        Tape = 0;
        Milestone = 0;
        Power = 0;
        Sanity = 20;
        Rizz = 1;
    }
}
