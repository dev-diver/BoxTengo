using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Start : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject popup;
    public void StartOrAd()
    {
        
        if(AdManager.Instance.GameCount > AdManager.Instance.GameToAdCount)
        {
            gameManager.resetGame();
            popup.SetActive(true);
        }
        else
        {
            gameManager.newGame();
        }
    }
}
