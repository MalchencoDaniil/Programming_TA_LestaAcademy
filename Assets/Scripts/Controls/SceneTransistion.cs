using UnityEngine;

namespace Runtime
{
    public class SceneTransistion : MonoBehaviour
    {
        private void OnEnable()
        {
            PauseManager._instance.OffPauseGame();
        }

        public static void LoadScene(string _sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        }

        public static void Exit()
        {
            Application.Quit();
        }

        public void Restart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
