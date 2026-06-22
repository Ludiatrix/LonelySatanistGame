using LSG.Core;
using UnityEngine;

namespace LSG.AnimationControllers
{
    public class NecronomiconOpenDetector : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("NecronomiconOpenDetector OnStateEnter");
            GameEvents.ChangeState?.Invoke(Enums.GameState.SummoningPhase);
        }
    }
}
