using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClerkShadow
{
    public class MenuController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
