using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    [SerializeField] Button prevBtn, nextBtn;
    [SerializeField] GameObject pages, naviDots, DotPrefab;
    public List<GameObject> Dots;
    private int currentPage = 1, totalPage;
    public Sprite dotSprite0, dotSprite1;

    void Start()
    {
        this.totalPage = pages.transform.childCount;
        float dotWidth = DotPrefab.GetComponent<RectTransform>().rect.width;

        for(int i=0;i<totalPage;i++)
        {
            GameObject dot = Instantiate(DotPrefab);
            dot.transform.SetParent(naviDots.transform);
            float x = dotWidth*i - (totalPage-1)*dotWidth/2;
            dot.transform.localPosition = new Vector3(x,0,0);
            dot.transform.localScale = new Vector3(1,1,1);
            Dots.Add(dot);
        }
        showCurrentPage(1,1);
    }
    public void NextPage()
    {
        if(this.currentPage == this.totalPage)
        {
            return;
        }
        int prevPage = currentPage;
        this.currentPage++;
        showCurrentPage(currentPage, prevPage);
    }
    public void PrevPage()
    {
        if(this.currentPage == 1)
        {
            return;
        }
        int prevPage = currentPage;
        this.currentPage--;
        showCurrentPage(currentPage, prevPage);
    }

    void showCurrentPage(int currentPage ,int prevPage)
    {
        pages.transform.GetChild(prevPage-1).gameObject.SetActive(false);
        pages.transform.GetChild(currentPage-1).gameObject.SetActive(true);
        dotNavigate(currentPage, prevPage);
    }

    void dotNavigate(int currentPage, int prevPage)
    {
        Dots[prevPage-1].GetComponent<Image>().sprite = dotSprite1;
        Dots[currentPage-1].GetComponent<Image>().sprite = dotSprite0;
    }
    
}
