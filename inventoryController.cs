using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class inventoryController : MonoBehaviour
{
    public RawImage penunjukItem;
    private float x = -27.50751f; //61.856542
    private float y = 112.9355f;
    private int tunjuk = 0;
    string path = Path.Combine(Application.streamingAssetsPath, "playerItem.json");
    private PlayerItem pItem;
    private Texture2D tex;
    public Transform container;
    public GameObject rawImagePrefab;
    public GameObject rawTextPrefab;
    private bool perluUpdateUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sambungJson();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("MenuInventori") == 1)
        {
            if (!perluUpdateUI)
            {
                tampilItem();
                perluUpdateUI = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && tunjuk != 4 && tunjuk != 9)
            {
                x += 77.3206775f;
                tunjuk++;
            }
            else if (Input.GetKeyDown(KeyCode.A) && tunjuk != 0 && tunjuk != 5)
            {
                x -= 77.3206775f;
                tunjuk--;
            }
            else if (Input.GetKeyDown(KeyCode.W) && tunjuk >= 5 )
            {
                y += 72.3215f;
                tunjuk -=5;
            }
            else if (Input.GetKeyDown(KeyCode.S) && tunjuk <= 4)
            {
                y -= 72.3215f;
                tunjuk +=5;
            }
            penunjukItem.rectTransform.anchoredPosition = new Vector2(x, y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int index = 0; index < pItem.item.Count; index++)
                {
                    if (index == tunjuk)
                    {
                        var it = pItem.item[index];
                        if (it.nama == "roti")
                        {
                            PlayerPrefs.SetInt("stamina", PlayerPrefs.GetInt("stamina") + 25);
                            PlayerPrefs.Save();
                            it.jumlah--;
                        }
                        else if (it.nama == "soda")
                        {
                            PlayerPrefs.SetInt("stamina", PlayerPrefs.GetInt("stamina") + 5);
                            PlayerPrefs.Save();
                            it.jumlah--;
                        }
                        else if (it.nama == "gulunganExp")
                        {
                            PlayerPrefs.SetInt("xp", PlayerPrefs.GetInt("xp") + 100);
                            PlayerPrefs.Save();
                            it.jumlah--;
                        }
                        else if(it.nama == "psp"){
                            SceneManager.LoadScene("Minigame2Menu");
                        }

                        if (it.jumlah <= 0)
                        {
                            pItem.item.RemoveAt(index);
                        }
                        else
                        {
                            pItem.item[index] = it; // update jika jumlah berubah
                        }

                        string updatedJson = JsonUtility.ToJson(pItem, true);
                        File.WriteAllText(path, updatedJson);
                        break;
                    }
                }
                Debug.Log("Stamina : "+PlayerPrefs.GetInt("stamina").ToString());
                Debug.Log("Exp     : "+PlayerPrefs.GetInt("xp").ToString());
                perluUpdateUI = false;
            }

        }
    }
    void sambungJson()
    {
        string json = File.ReadAllText(path);
        pItem = JsonUtility.FromJson<PlayerItem>(json);
    }
    void tampilItem()
    {
        // Bersihkan semua UI item sebelumnya
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        float xItem = -155.3f;
        float yItem = 36f;
        int n = 0;
        foreach (var it in pItem.item)
        {
            tex = Resources.Load<Texture2D>("Item/" + it.nama);
            GameObject newImageObj = Instantiate(rawImagePrefab, container);
            GameObject newTextObj = Instantiate(rawTextPrefab, container);
            newImageObj.SetActive(true);  // Pastikan aktif

            RawImage rawImage = newImageObj.GetComponent<RawImage>();
            Text tx = newTextObj.GetComponentInChildren<Text>();
            rawImage.texture = tex;
            tx.text = it.jumlah.ToString();

            if(n == 5)
            {
                yItem -= 72.3215f;
                n = 0;
            }
            float Px = xItem + 77.3206775f * n;

            RectTransform ri = newImageObj.GetComponent<RectTransform>();
            RectTransform rt = newTextObj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(Px, yItem);
            ri.anchoredPosition = new Vector2(Px, yItem);
            n++;
        }
    }

}
