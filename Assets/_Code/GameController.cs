using UnityEngine;

namespace LSG
{
    public class GameController : MonoBehaviour
    {
        public Enums.GameState CurrentPhase = Enums.GameState.StartPhase;
        
        [Header("Developer Mode")] 
        public bool previousState = false;
        public bool nextState = false;
    }
    
}
