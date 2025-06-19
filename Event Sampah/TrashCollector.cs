using UnityEngine;
using System.Collections;
using TMPro;

public class TrashCollector : MonoBehaviour
{
    public EventTrashTrigger ts;

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    public float collectDistance = 1f;
    private bool bersih = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");

            foreach (GameObject trash in trashObjects)
            {
                if (Vector3.Distance(transform.position, trash.transform.position) <= collectDistance)
                {
                    Destroy(trash);
                    StartCoroutine(CheckIfAllTrashCollected());
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && bersih)
        {
            bersih = false;
            dialogBox.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    IEnumerator CheckIfAllTrashCollected()
    {
        yield return null; // Tunggu 1 frame

        if (GameObject.FindGameObjectsWithTag("Trash").Length == 0)
        {
            bersih = true;
            PlayerPrefs.SetString("EventSampah", "hadiah");
            PlayerPrefs.Save();
            ts.statusEvent = "hadiah";
            Debug.Log("Semua sampah sudah dibersihkan!");
            dialogBox.SetActive(true);
            dialogText.text = "Sampah Telah Dibersihkan Semua !!\n Kota Telah Kembali Bersih !!";
            Time.timeScale = 0f;
        }
    }

}
