using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// Hook for the StartPhase interface and holds the start button in order to turn it off.
    /// Note: Should elements turn themselves off on EndPhase of their phase type or let their parent phase do it?
    /// </summary>
    public class TitlePhase : Phase
    {
        public GameObject[] GameObjectsToToggle;
        
        public override void StartPhase()
        {
            Debug.Log("[TitlePhase] Starting Phase!");
            base.StartPhase();
            SetGameObjectActiveState(true);
        }

        public override void EndPhase()
        {
            Debug.Log("[TitlePhase] Ending Phase!");
            base.EndPhase();
            SetGameObjectActiveState(false);
        }

        private void SetGameObjectActiveState(bool activeState)
        {
            foreach (var obj in GameObjectsToToggle)
            {
                obj.SetActive(activeState);
            }
        }
    }
}
