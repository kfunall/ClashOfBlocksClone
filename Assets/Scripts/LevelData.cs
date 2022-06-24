using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] Level[] levels;
    int levelIndex = 1;

    private void OnDisable() => ResetLevelIndex();
    public void ResetLevelIndex() => levelIndex = 1;
    public void IncreaseLevelIndex()
    {
        if (levelIndex == levels.Length)
            levelIndex = 0;
        levelIndex++;
    }
    public void DrawLevel() => levels[levelIndex - 1].CreateLevel();
    public int GetLevelIndex()
    {
        return levelIndex;
    }
    public int GetGroundCount()
    {
        return levels[levelIndex - 1].GroundCount();
    }
    public Vector3 GetCameraPosition()
    {
        float x = Mathf.FloorToInt(levels[levelIndex - 1].Width / 2f);
        float y = levels[levelIndex - 1].Width * 2f;
        return new Vector3(x, y, -1f);
    }
}