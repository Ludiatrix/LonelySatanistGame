using UnityEngine;
using UnityEngine.SceneManagement;

namespace LSG.UI.Emitters
{
    public class SceneRestartEmitter : MonoBehaviour
    {
        public void Emit()
        {
            SceneManager.LoadScene(0);
        }
    }
}
