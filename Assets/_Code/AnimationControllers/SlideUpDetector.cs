using LSG.Core;
using UnityEngine;

namespace LSG.AnimationControllers
{
    public class SlideUpDetector : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameEvents.NecronomiconFinishedSliding?.Invoke();
        }
    }
}
