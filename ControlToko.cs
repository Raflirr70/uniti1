using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ControlToko : MonoBehaviour
{
    private int tunjuk = 1;
    public TextMeshProUGUI penunjuk;
    public TextMeshProUGUI jml;
    public TextMeshProUGUI gold;
    private float y = 355.4f;
    private Texture loadedTexture;
    private Texture2D tex;
    public RawImage rawImage;
    private string[] items = { "roti", "soda", "gulunganExp", "sapu", "penyiram", "sarungTangan", "masker", "ransel" };
    private int[] harga    = { 200, 50, 550, 400, 2000, 6000, 8500, 12000};
    string path = Path.Combine(Application.streamingAssetsPath, "playerItem.json");
    private PlayerItem pItem;
    string tempat = SceneManager.GetActiveScene().name;

    void init()
    {
        PlayerPrefs.SetInt("sapu", 0);
        PlayerPrefs.SetInt("penyiram", 0);
        PlayerPrefs.SetInt("sarungTangan", 0);
        PlayerPrefs.SetInt("masker", 0);
        PlayerPrefs.SetInt("ransel", 0);
        PlayerPrefs.SetInt("score", 50000);
        PlayerPrefs.SetInt("psp", 0);
        PlayerPrefs.SetInt("waitSapu", 0);
        PlayerPrefs.SetString("EventSampah", " ");
        PlayerPrefs.Save();
    }

    void Start()
    {
        // init();
        sambungJson();
        jml.text = PlayerPrefs.GetInt("psp").ToString() +"x";
        if(PlayerPrefs.GetInt("penjualKantor") == 1){
            tex = Resources.Load<Texture2D>("Item/" + "psp");
        }else{
            cetakJumlah();
            tex = Resources.Load<Texture2D>("Item/" + items[0]);
        }
        rawImage.texture = tex;
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("penjualKantor") == 1){
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Kantor");
            if (Input.GetKeyDown(KeyCode.Space)){
                if (PlayerPrefs.GetInt("psp") != 1) {
                    beliTools("psp", 20000);
                }
            }
            gold.text = PlayerPrefs.GetInt("score").ToString() + " G";
        }else{
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Kota");
            }
            if (Input.GetKeyDown(KeyCode.S) && tunjuk != 8)
            {
                tunjuk++;
                tex = Resources.Load<Texture2D>("Item/" + items[tunjuk-1]);
                rawImage.texture = tex;
                y -= 52.05f;
            }
            else if(Input.GetKeyDown(KeyCode.W) && tunjuk != 1)
            {
                tunjuk--;
                tex = Resources.Load<Texture2D>("Item/" + items[tunjuk-1]);
                rawImage.texture = tex;
                y += 52.05f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (tunjuk <= 3)
                {
                    tambahItem(items[tunjuk-1]);
                    cetakJumlah();
                }
                else {
                    switch (tunjuk)
                    {
                        case 4:
                            if(PlayerPrefs.GetInt("sapu") != 1) {
                                PlayerPrefs.GetInt("score");
                                beliTools("sapu", 400);
                            }
                            break;
                        case 5:
                            if (PlayerPrefs.GetInt("penyiram") != 1) {
                                beliTools("penyiram", 2000);
                            }
                            break;
                        case 6:
                            if(PlayerPrefs.GetInt("sarungTangan") != 1) {
                                beliTools("sarungTangan", 6000);
                            }
                            break;
                        case 7:
                            if(PlayerPrefs.GetInt("masker") != 1) {
                                beliTools("masker", 8500);
                            }
                            break;
                        case 8:
                            if(PlayerPrefs.GetInt("ransel") != 1) {
                                beliTools("ransel", 12000);
                            }
                            break;
                    }
                }
            }

            gold.text = PlayerPrefs.GetInt("score").ToString() + " G";
            penunjuk.rectTransform.anchoredPosition = new Vector2(-799.847f, y);
        }
        
    }

    void sambungJson()
    {
        string json = File.ReadAllText(path);
        pItem = JsonUtility.FromJson<PlayerItem>(json);
    }
    int jumlahDipunya(string x)
    {
        foreach (var it in pItem.item)
        {
            if(it.nama == x)
            {
                return it.jumlah;
            }
        }
        return 0;
    }
    void cetakJumlah()
    {
        jml.text = "";
        for (int a = 0; a < 3; a++)
        {
            jml.text += jumlahDipunya(items[a]).ToString() + "x\n";
        }
    }
    void tambahItem(string x)
    {
        bool ada = false;
        int n = -1;
        foreach (var it in pItem.item)
        {
            if (it.nama == x)
            {
                ada = true;
                n = cariItem(it.nama);
                break;
            }
        }
        if (ada)
        {
            foreach(var it in pItem.item)
            {
                if (it.nama == x)
                {
                    if (PlayerPrefs.GetInt("score") - harga[n] >= 0)
                    {
                        it.jumlah++;
                        string updatedJson = JsonUtility.ToJson(pItem, true);
                        File.WriteAllText(path, updatedJson);
                        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") - harga[n]);
                        PlayerPrefs.Save();
                    }
                }
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("score") - harga[tunjuk] >=0)
            {
                Item newItem = new Item
                {
                    nama = x,
                    jumlah = 1
                };
                pItem.item.Add(newItem);
                string updatedJson = JsonUtility.ToJson(pItem, true);
                File.WriteAllText(path, updatedJson);
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") - harga[tunjuk]);

            }
        }
    }
    void beliTools(string x, int j)
    {
        if((PlayerPrefs.GetInt("score") - j) > 0)
        {
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") - j);
            Item newItem = new Item
            {
                nama = x,
                jumlah = 1
            };
            pItem.item.Add(newItem);
            string updatedJson = JsonUtility.ToJson(pItem, true);
            File.WriteAllText(path, updatedJson);
            PlayerPrefs.SetInt(x, 1);
            PlayerPrefs.Save();
        }
    }
    int cariItem(string x)
    {
        for(int a = 0; a < 3; a++)
        {
            if (items[a] == x) return a;
        }
        return -1;
    }
}



