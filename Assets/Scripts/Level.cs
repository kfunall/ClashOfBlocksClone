using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level", fileName = "New Level")]
public class Level : ScriptableObject
{
    public List<Vector3> GroundPositions = new List<Vector3>();
    public List<Vector3> BorderPositions = new List<Vector3>();
    public List<Vector3> RedCubesPositions = new List<Vector3>();
    public List<Vector3> YellowCubesPositions = new List<Vector3>();

    [SerializeField] int width = 5;
    [SerializeField] int height = 5;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public int GroundCount()
    {
        return GroundPositions.Count;
    }
    public void CreateLevel()
    {
        for (int i = 0; i < GroundPositions.Count; i++)
        {
            ObjectPool.Instance.SpawnFromPool("Ground", GroundPositions[i], Vector3.right * 90f);
        }
        for (int i = 0; i < BorderPositions.Count; i++)
        {
            ObjectPool.Instance.SpawnFromPool("Border", BorderPositions[i], Vector3.zero);
        }
        for (int i = 0; i < RedCubesPositions.Count; i++)
        {
            GameObject a = ObjectPool.Instance.SpawnFromPool("RedCube", RedCubesPositions[i], Vector3.zero);
            a.GetComponent<Cube>().enabled = false;
        }
        for (int i = 0; i < YellowCubesPositions.Count; i++)
        {
            GameObject a = ObjectPool.Instance.SpawnFromPool("YellowCube", YellowCubesPositions[i], Vector3.zero);
            a.GetComponent<Cube>().enabled = false;
        }
    }
}