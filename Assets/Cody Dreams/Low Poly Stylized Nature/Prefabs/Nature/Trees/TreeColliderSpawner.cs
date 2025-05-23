using UnityEngine;

public class TreeColliderSpawner : MonoBehaviour
{
    public Terrain terrain;
    public GameObject colliderPrefab;

    void Start()
    {
        if (terrain == null || colliderPrefab == null)
        {
            Debug.LogError("Terrain atau Collider Prefab belum di-assign.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPosition = terrain.transform.position;

        foreach (TreeInstance tree in terrainData.treeInstances)
        {
            Vector3 worldPos = Vector3.Scale(tree.position, terrainData.size) + terrainPosition;

            Instantiate(colliderPrefab, worldPos, Quaternion.identity, this.transform);
        }

        Debug.Log("Collider ditambahkan ke semua pohon Terrain.");
    }
}
