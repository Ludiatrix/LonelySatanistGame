using LSG.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LSG.UI
{
    /// <summary>
    /// Resets all game data (player deck, economy, etc.) and reloads the scene.
    /// Used by the End Game button shown after a terminal encounter
    /// (Papiyawn / The Book) in place of the "Go to Store" button.
    /// </summary>
    public class EndGameEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[EndGameEmitter] Resetting game data and reloading the scene.");

            // DataManager is DontDestroyOnLoad, so reloading the scene alone won't
            // reset the ScriptableObject-backed state. Reset it explicitly first.
            if (DataManager.Instance != null)
            {
                DataManager.Instance.ResetData();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
