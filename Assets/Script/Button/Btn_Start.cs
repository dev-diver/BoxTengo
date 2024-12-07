using UnityEngine;

public class Btn_Start : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject popup;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void StartOrAd()
    {

        if (AdManager.Instance.GameCount > AdManager.Instance.GameToAdCount)
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
