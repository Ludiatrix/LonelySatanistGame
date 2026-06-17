using LSG.Core;
using UnityEngine;

namespace LSG.AnimationControllers
{
    public class NecronomiconOpenDetector : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("NecronomiconOpenDetector OnStateExit");
            GameEvents.NecronomiconOpened?.Invoke(Enums.GameState.SummoningPhase);
        }
    }
}
