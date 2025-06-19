using UnityEngine;

public class eventsampah : MonoBehaviour
{
    public GameObject trashPrefab;           // Prefab sampah
    public int trashCount = 10;              // Jumlah sampah yang ingin di-spawn
    public Vector2 spawnAreaMin;             // Koordinat minimal area spawn (X, Z)
    public Vector2 spawnAreaMax;             // Koordinat maksimal area spawn (X, Z)
    public bool hasSpawned = false;

    public void TriggerTrashSpawnEvent()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        for (int i = 0; i < trashCount; i++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 spawnPos = new Vector3(x, 0f, z); // asumsikan tanah di Y = 0

            GameObject spawnedTrash = Instantiate(trashPrefab, spawnPos, Quaternion.Euler(-66.976f, -48.182f, 45.81f));
            spawnedTrash.tag = "Trash";
        }

        Debug.Log($"{trashCount} sampah telah di-spawn secara acak.");
    }
}
