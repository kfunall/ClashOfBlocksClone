using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    Vector3[] directions = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    string[] tags = new string[] { "Left", "Right", "Forward", "Back" };
    private void Start()
    {
        StartCoroutine(Wait());
    }
    private void CheckAround(Vector3 direction, string tag)
    {
        Vector3 pos = transform.position + direction;
        if (!Physics.Raycast(transform.position, direction, 1.25f, layerMask))
        {
            GameObject a = ObjectPool.Instance.SpawnFromPool(gameObject.tag, pos, Vector3.zero);
            a.GetComponent<Animator>().SetBool(tag, true);
        }
    }
    IEnumerator Wait()
    {
        for (int i = 0; i < 4; i++)
        {
            CheckAround(directions[i], tags[i]);
            switch (gameObject.tag)
            {
                case "RedCube":
                    yield return new WaitForSeconds(0.066f);
                    break;
                case "YellowCube":
                    yield return new WaitForSeconds(0.066f);
                    break;
                case "GreenCube":
                    yield return new WaitForSeconds(0.065f);
                    break;
            }
        }
    }
}