using System.Collections.Generic;
using UnityEngine;

public class AppleManager : MonoBehaviour
{
    public List<Apple> apples = new List<Apple>();
    public List<Sprite> appleSprite = new List<Sprite>();
    public GameObject applePrefab;
    public List<Apple> selectedApples = new List<Apple>();
    [SerializeField] GameManager gameManager;
    [SerializeField] ComboManager comboManager;

    public float appleScale = 0.9f;
    public int appleAmount = 7;

    public void createApples()
    {
        apples = new List<Apple>();
        //총 갯수가 appleAmount^2 이면서 sum이 10인 배열 랜덤으로 찾기

        int[] appleNumbers;
        if(!gameManager.bonusMode)
        {
            appleNumbers = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 
                                         2, 2, 2, 2, 2, 2, 2, 2,
                                         3, 3, 3, 3, 3, 3, 3,
                                         4, 4, 4, 4, 4, 4, 4,
                                         5, 5, 5, 5, 5,
                                         6, 6, 6, 6, 6,
                                         7, 7, 7,
                                         8, 8, 8,
                                         9, 9, 9};
        }
        else
        {
            appleNumbers = new int[]{ 5,5,5,5,5,5,5,
                                        5,5,5,5,5,5,5,
                                        5,5,5,5,5,5,5,
                                        5,5,5,5,5,5,5,
                                        5,5,5,5,5,5,5,
                                        5,5,5,5,5,5,5,
                                        5,5,5,5,2,3,5,
                                    }; 
        }

        int k = 0;
        for(int i=0;i<appleAmount;i++)
        {
            for(int j=0; j<appleAmount;j++)
            {
                //number 추후에 합 10이 되게
                GameObject apple = Instantiate(applePrefab, Vector3.zero, Quaternion.identity, transform);
                Apple comp = apple.GetComponent<Apple>();
                comp.coord = new Vector2(i,j);
                comp.number = appleNumbers[k];
                k++;
                // Apple로 넘기기
                apple.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = appleSprite[comp.number-1];
                apples.Add(comp); 
            }
        }
    }

    public void ClearSelected()
    {
        for(var i = selectedApples.Count-1; i>=0; i--)
        {
            selectedApples[i]?.Unselect();
        }
        //selectedApples.Clear();
    }
    public void clearApples()
    {
        for(int i = 0; i < apples.Count;i++)
        {
            if(apples[i]!=null){ //?.으로 바꾸면 에러
                Destroy(apples[i].gameObject);
            }
        }
        apples?.Clear();
    }

    public void ShuffleApples()
    {
        List<int> numbers = new List<int>();
        foreach(var apple in apples)
        {
            if(apple!=null)
            {
                numbers.Add(apple.number);
            }            
        }

        var result = "";
        foreach (var item in numbers)
        {
            result += item.ToString() + ", ";
        }

        numbers.Sort((a, b)=> 1 - 2 * Random.Range(0, 2));
        
        result = "";
        foreach (var item in numbers)
        {
            result += item.ToString() + ", ";
        }

        int k = 0;
        for(int i=0;i<apples.Count;i++)
        {
            if(apples[i]!=null)
            {
                apples[i].number = numbers[k];
                apples[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = appleSprite[numbers[k]-1];
                k++;
            }
        }
    }

    private int CalSelect(List<Apple> Apples)
    {
        int sum = 0;
        foreach(var apple in Apples)
        {
            sum+=apple.number;
        }
        return sum;
    }

    private void AISelect()
    {
        
    }

    public void BoxingApple()
    {
        float appleSum = CalSelect(selectedApples);
        if(appleSum == 10 || appleSum == 20)
        {
            foreach(var apple in selectedApples)
            {
                apple?.BoxingOut();
            }
            int leftAppleAmount = leftApples();
            comboManager.CalBaseComboTime(leftAppleAmount);
            gameManager.CalScore(selectedApples);
            if(leftAppleAmount==0){
                gameManager.nextStage();
            }
        }
    }
    public int leftApples()
    {
        
        int appleAmount = 0;
        for(int i=0; i<apples.Count; i++)
        {
            if(apples[i]!=null){
                appleAmount++;
            }
        }
        return appleAmount;
    }

}
