using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask layerMask;
    bool canClick;

    private void Awake()
    {
        canClick = false;
    }
    private void Update()
    {
        GetTouchPosition();
    }
    private void GetTouchPosition()
    {
        if (Input.touchCount > 0 && !GameManager.Instance.EndGame)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = cam.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if ((Physics.Raycast(ray, out hit, layerMask) && !canClick) && (hit.collider.gameObject.tag != "RedCube" || hit.collider.gameObject.tag != "YellowCube"))
            {
                canClick = true;
                Vector3 spawnPos = hit.point;
                spawnPos.x = Mathf.Round(hit.point.x);
                spawnPos.z = Mathf.Round(hit.point.z);
                spawnPos.y = 0.125f;
                ObjectPool.Instance.SpawnFromPool("GreenCube", spawnPos, Vector3.zero);
                GetCubeReferances();
            }
        }
    }
    private void GetCubeReferances()
    {
        GameObject[] redCubes = GameObject.FindGameObjectsWithTag("RedCube");
        if (redCubes != null)
        {
            for (int i = 0; i < redCubes.Length; i++)
            {
                redCubes[i].GetComponent<Cube>().enabled = true;
            }
        }
        GameObject[] yellowCubes = GameObject.FindGameObjectsWithTag("YellowCube");
        if (yellowCubes != null)
        {
            for (int i = 0; i < yellowCubes.Length; i++)
            {
                yellowCubes[i].GetComponent<Cube>().enabled = true;
            }
        }
    }
}