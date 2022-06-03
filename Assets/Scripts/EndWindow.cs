using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndWindow : MonoBehaviour
{
   public GameObject window;
    public static EndWindow instance;
    public Text finalText, score;
    bool opened = false;
    public Sprite loseSprite;
    private void Awake()
    {
        instance = this;
    }
    public void Open(bool isWon)
    {


        if (!isWon)
        {
            window.transform.GetChild(0).GetComponent<Image>().sprite = loseSprite;
        }
        if (opened) return;
        opened = true;
       // transform.GetChild(0).gameObject.SetActive(true);
        if (isWon) finalText.text = $"You are winner!";
        else finalText.text = $"Duel was lose..";
        window.SetActive(true);
        
        score.text = $"<color=blue>Kills: {GameController.instance.kills}</color> \n<color=red>Deads: {GameController.instance.deaths}</color>";
        StartCoroutine(delayExit());
    }

  
    IEnumerator delayExit()
    {
        yield return new WaitForSeconds(2);
       
      
     
        GameManager.instance.QuitBtn();
       // GameController.instance.darkAnim.Play("DarkToLightFast");
    }


}
