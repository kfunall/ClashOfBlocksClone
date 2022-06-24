using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton(string sceneName) => SceneManager.LoadScene(sceneName);
}