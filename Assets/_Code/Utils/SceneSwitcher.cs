using UnityEngine;
using UnityEngine.SceneManagement;

namespace LSG
{
    /// <summary>
    /// Switches scenes from a UI Button. Button OnClick can't call SceneManager
    /// directly (it's not a component), so wire OnClick to one of these methods.
    /// Put this on any GameObject in the scene.
    ///
    /// NOTE: the target scene must be added to the build list
    /// (File > Build Profiles > Scene List) or LoadScene will fail.
    /// </summary>
    public class SceneSwitcher : MonoBehaviour
    {
        [Tooltip("Scene to load when LoadConfiguredScene() is called (used by the no-argument OnClick option).")]
        [SerializeField] private string sceneName;

        /// <summary>Loads the scene set in the inspector. Wire OnClick to this for the simplest setup.</summary>
        public void LoadConfiguredScene()
        {
            LoadScene(sceneName);
        }

        /// <summary>Loads a scene by name. OnClick can pass the name as a string argument.</summary>
        public void LoadScene(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"{nameof(SceneSwitcher)} on '{gameObject.name}': no scene name provided.", this);
                return;
            }

            SceneManager.LoadScene(name);
        }

        /// <summary>Loads a scene by build index. OnClick can pass the index as an int argument.</summary>
        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>Reloads the currently active scene.</summary>
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>Quits the game (no-op note: does nothing in the editor).</summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
