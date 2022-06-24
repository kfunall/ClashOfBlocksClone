using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text levelText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start() => levelText.text = "Level " + GameManager.Instance.LevelDatas.GetLevelIndex().ToString();
    public void ReloadSecene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OpenPanel(string whichPanel)
    {
        StartCoroutine(EndGameUI(whichPanel));
    }
    IEnumerator EndGameUI(string panel)
    {
        yield return new WaitForSeconds(0.65f);
        switch (panel)
        {
            case "GameOver":
                gameOverPanel.SetActive(true);
                break;
            case "Win":
                winPanel.SetActive(true);
                break;
        }
    }
}