using UnityEngine;

namespace Runtime.Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private void Start() => SceneTransistion.LoadScene(SceneNames.game);
    }
}