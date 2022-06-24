using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] LevelData levelData;
    [SerializeField] Camera cam;
    int redCubeCount, yellowCubeCount, greenCubeCount, totalCubeCount;
    int redPercentage, yellowPercentage, greenPercentage;
    bool endGame = false;
    public bool EndGame { get { return endGame; } }
    public LevelData LevelDatas { get { return levelData; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        cam.transform.position = levelData.GetCameraPosition();
        levelData.DrawLevel();
    }
    private void GameEnd()
    {
        endGame = true;
        GetPercentages();
        if ((greenPercentage > redPercentage) && (greenPercentage > yellowPercentage))
            Win();
        else
            UIManager.Instance.OpenPanel("GameOver");
    }
    private void Win()
    {
        levelData.IncreaseLevelIndex();
        UIManager.Instance.OpenPanel("Win");
    }
    private void GetPercentages()
    {
        redPercentage = Mathf.RoundToInt((redCubeCount * 100) / (float)totalCubeCount);
        yellowPercentage = Mathf.RoundToInt((yellowCubeCount * 100) / (float)totalCubeCount);
        greenPercentage = Mathf.RoundToInt((greenCubeCount * 100) / (float)totalCubeCount);
    }
    public void CubeCount(string whichCube)
    {
        switch (whichCube)
        {
            case "GreenCube":
                greenCubeCount++;
                break;
            case "RedCube":
                redCubeCount++;
                break;
            case "YellowCube":
                yellowCubeCount++;
                break;
        }
        totalCubeCount = redCubeCount + yellowCubeCount + greenCubeCount;
        if (totalCubeCount == levelData.GetGroundCount())
            GameEnd();
    }
}