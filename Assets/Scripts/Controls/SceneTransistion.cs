using UnityEngine;

namespace Runtime
{
    public class SceneTransistion : MonoBehaviour
    {
        public static void LoadScene(string _sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        }

        public static void Exit()
        {
            Application.Quit();
        }
    }
}
