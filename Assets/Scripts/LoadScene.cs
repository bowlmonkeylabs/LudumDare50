using UnityEngine;
using UnityEngine.SceneManagement;

namespace BML.Scripts
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private string SceneToLoad = "Main";

        public void Activate()
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}