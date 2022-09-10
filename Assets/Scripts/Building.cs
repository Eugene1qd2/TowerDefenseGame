using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Building : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    MeshRenderer[] renderers;
    [SerializeField] public int cost = 10;
    [SerializeField] public int upgradeCost;
    [SerializeField] public TowerAttack thisAttack;

    public void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }
    public void SetTransperent(bool available)
    {
        if (available)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color=Color.green;
            }
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.red;
            }
        }
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetNormal()
    {
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.white;
            }
        }
        GameObject.FindGameObjectWithTag("Floar").GetComponent<BuildingGrid>().UpdateCoins(-cost);
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
                }
                else
                {
                    Gizmos.color = new Color(0.5f, 1f, 0f, 0.3f);
                }
                Gizmos.DrawCube(transform.position + new Vector3(x, -0.4f, y), new Vector3(1, 0.2f, 1));
            }
        }
    }
}
