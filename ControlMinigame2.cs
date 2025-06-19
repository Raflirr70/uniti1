using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlMinigame2 : MonoBehaviour
{
    public RawImage panelDialogAkhir;
    public Text textAkhir;

    public Text penunjuk;
    public Text statusLawan;
    public RawImage penunjukItem;
    public Image darahPlayer;
    public Image darahEnemy;
    private int tunjuk = 1;
    private int tunjukItem = 1;
    private bool inven = false;
    private float x = -296.9f;
    private float y = -293.1f;
    //private bool serang = false;

    //-==-==-=-=-=-=-=-=-=-
    public enum Turn { Player, Enemy }
    public Turn currentTurn = Turn.Player;
    //-==-=-=-=-=-=-=-=-=-=-

    public float[] statusEnemy = new float[2];
    public float[] statusPlayer = new float[2];

    void StatusLawan()
    {
        statusLawan.text= "";
        statusLawan.text += "LVL "+PlayerPrefs.GetInt("enemyLevel").ToString()+"\n\n";
        statusLawan.text += "HP   : " + statusEnemy[0].ToString("F2") + " / " + PlayerPrefs.GetFloat("enemyHp").ToString("F2") + "\n";
        statusLawan.text += "Atk  : " + PlayerPrefs.GetFloat("enemyAtk").ToString("F2");
    }
    void StatusEnemyReset()
    {
        PlayerPrefs.SetFloat("enemyHp", 10);
        PlayerPrefs.SetFloat("enemyAtk", 1);
        PlayerPrefs.SetInt("enemyLevel", 1);
        PlayerPrefs.Save();
    }
    void StatusPlayerReset()
    {
        PlayerPrefs.SetFloat("playerHp", 10);
        PlayerPrefs.SetFloat("playerAtk", 2);
        PlayerPrefs.SetInt("playerLevel", 1);
        PlayerPrefs.SetFloat("playerXp", 0);
        PlayerPrefs.Save();
    }
    void Initbattle()
    {
        statusEnemy[0] = PlayerPrefs.GetFloat("enemyHp");
        statusEnemy[1] = PlayerPrefs.GetFloat("enemyAtk");
        statusPlayer[0] = PlayerPrefs.GetFloat("playerHp");
        statusPlayer[1] = PlayerPrefs.GetFloat("playerAtk");
    }

    void Start()
    {
        // StatusEnemyReset();
        // StatusPlayerReset();
        Initbattle();
        StatusLawan();
        penunjukItem.gameObject.SetActive(false);
        panelDialogAkhir.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && panelDialogAkhir.gameObject.activeSelf)
        {
            SceneManager.LoadScene("Minigame2Menu");
        }
        if (!inven)
        {
            if (Input.GetKeyDown(KeyCode.S) && tunjuk != 3)
            {
                tunjuk++;
            }else if (Input.GetKeyDown(KeyCode.W) && tunjuk != 1)
            {
                tunjuk--;
            }

            switch (tunjuk)
            {
                case 1:
                    penunjuk.rectTransform.anchoredPosition = new Vector2(-850.99f, -301f);
                    break;
                case 2:
                    penunjuk.rectTransform.anchoredPosition = new Vector2(-850.99f, -366.82f);
                    break;
                case 3:
                    penunjuk.rectTransform.anchoredPosition = new Vector2(-850.99f, -445.76f);
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {   
                if (tunjuk == 1)
                {
                    StartCoroutine(TurnBaseAttack());
                }
                else if(tunjuk == 2){
                    penunjukItem.gameObject.SetActive(true);
                    penunjuk.text = "";
                    inven = true;
                }else{
                    SceneManager.LoadScene("Minigame2Menu");
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && tunjukItem > 6)
            {
                tunjukItem -= 6;
                y += 104.9f;
            }
            else if (Input.GetKeyDown(KeyCode.S) && tunjukItem < 7)
            {
                tunjukItem += 6;
                y -= 104.9f;
            }
            else if (Input.GetKeyDown(KeyCode.A) && (tunjukItem != 1 && tunjukItem != 7))
            {
                tunjukItem--;
                x -= 121.6f;
            }
            else if (Input.GetKeyDown(KeyCode.D) && (tunjukItem != 6 && tunjukItem != 12))
            {
                tunjukItem++;
                x += 121.6f;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inven = false;
                penunjukItem.gameObject.SetActive(false);
                penunjuk.text = "♦";
                x = -296.9f;
                y = -293.1f;
                tunjukItem = 1;
                penunjukItem.rectTransform.anchoredPosition = new Vector2(x, y);
            }
            penunjukItem.rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }


    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-


    IEnumerator TurnBaseAttack()
    {
        if (statusPlayer[0] > 0)
            yield return StartCoroutine(Attack("Player"));
        yield return new WaitForSeconds(1f);
        if(statusEnemy[0] > 0)
            yield return StartCoroutine(Attack("Enemy"));
    }

    IEnumerator Attack(string attacker)
    {
        Debug.Log(attacker + " attacks!");

        yield return new WaitForSeconds(1f); // animasi atau delay
        float damageEnemy = Random.Range(statusEnemy[1] - (statusEnemy[1] * 0.1f), statusEnemy[1] + (statusEnemy[1] * 0.1f));
        float damagePlayer = Random.Range(statusPlayer[1] - (statusPlayer[1] * 0.1f), statusPlayer[1] + (statusPlayer[1] * 0.1f));

        if (attacker == "Player")
        {
            statusEnemy[0] -= damagePlayer;
            statusEnemy[0] = Mathf.Clamp(statusEnemy[0], 0f, PlayerPrefs.GetFloat("enemyHp"));
            UpdateHPBar(darahEnemy, statusEnemy[0], "enemy");
            Debug.Log("Enemy HP: " + statusEnemy[0]);
            if (statusEnemy[0] == 0f)
            {
                yield return new WaitForSeconds(1f);
                DialogAkhir("menang");
            }
        }
        else
        {
            statusPlayer[0] -= damageEnemy;
            statusPlayer[0] = Mathf.Clamp(statusPlayer[0], 0f, PlayerPrefs.GetFloat("playerHp"));
            UpdateHPBar(darahPlayer, statusPlayer[0], "player");
            Debug.Log("Player HP: " + statusPlayer[0]);
            if (statusPlayer[0] == 0f)
            {
                yield return new WaitForSeconds(1f);
                DialogAkhir("kalah");
            }
        }

        yield return new WaitForSeconds(1f); // jeda antar giliran
    }
    void UpdateHPBar(Image darahBar, float currentHP, string C)
    {
        StatusLawan();
        float percent = 0;
        if (C == "player")
        {
            percent = currentHP / PlayerPrefs.GetFloat("playerHp");
        }
        else
        {
            percent = currentHP / PlayerPrefs.GetFloat("enemyHp");
        }
            darahBar.fillAmount = percent;
    }
    
    void DialogAkhir(string x)
    {
        panelDialogAkhir.gameObject.SetActive(true);
        if(x == "menang")
        {
            PlayerPrefs.SetInt("enemyLevel", PlayerPrefs.GetInt("enemyLevel") + 1);
            PlayerPrefs.SetFloat("enemyHp", PlayerPrefs.GetFloat("enemyHp") + PlayerPrefs.GetFloat("enemyHp") * 0.3f);
            PlayerPrefs.SetFloat("enemyAtk", PlayerPrefs.GetFloat("enemyAtk") + PlayerPrefs.GetFloat("enemyAtk") * 0.3f);
            PlayerPrefs.SetFloat("playerXp", PlayerPrefs.GetFloat("playerXp")+ PlayerPrefs.GetInt("enemyLevel"));
            textAkhir.text = "Victory";
            PlayerPrefs.Save();
        }
        else
        {
            textAkhir.text = "Deffet";
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") - 500);
            PlayerPrefs.Save();
        }
        

    }

}


