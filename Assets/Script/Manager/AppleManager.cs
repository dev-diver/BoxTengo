using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppleManager : MonoBehaviour
{
    private List<Apple> Apples = new List<Apple>();
    public List<Sprite> AppleSprite = new List<Sprite>();
    public GameObject ApplePrefab;

    public HashSet<Apple> SelectedApples = new HashSet<Apple>();
    [SerializeField] GameManager gameManager;
    [SerializeField] ComboManager comboManager;

    public int AppleGridSize = 7;
    private int _leftApples = 0;

    public void CreateApples()
    {
        Apples = new List<Apple>();
        //총 갯수가 appleAmount^2 이면서 sum이 10인 배열 랜덤으로 찾기

        int[] appleNumbers;
        if (!gameManager.bonusMode)
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
        for (int i = 0; i < AppleGridSize; i++)
        {
            for (int j = 0; j < AppleGridSize; j++)
            {
                //number 추후에 합 10이 되게
                Apple apple = CreateApple(appleNumbers[k], i, j);
                k++;
                Apples.Add(apple);
            }
        }
        _leftApples = AppleGridSize * AppleGridSize;
    }

    private Apple CreateApple(int number, int x, int y)
    {
        GameObject apple = Instantiate(ApplePrefab, Vector3.zero, Quaternion.identity, transform);
        Apple comp = apple.GetComponent<Apple>();
        comp.coord = new Vector2(x, y);
        comp.number = number;
        comp.

        // Apple로 넘기기
        apple.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = AppleSprite[comp.number - 1];
        return comp;
    }

    public void SelectApple(Apple apple)
    {
        apple?.Selected();
        SelectedApples.Add(apple);
    }

    public void UnSelectApple(Apple apple)
    {
        apple?.Unselected();
        SelectedApples.Remove(apple);
    }

    public void UnSelectAllApples()
    {

        var unselectedApples = SelectedApples.ToList();

        foreach (var apple in unselectedApples)
        {
            apple?.Unselected();
        }
        SelectedApples.Clear();
    }

    public void ClearAllApples()
    {
        for (int i = 0; i < Apples.Count; i++)
        {
            if (Apples[i] != null)
            { //?.으로 바꾸면 에러
                Destroy(Apples[i].gameObject);
            }
        }
        Apples?.Clear();
    }

    public void ShuffleApples()
    {
        List<int> numbers = new List<int>();
        foreach (var apple in Apples)
        {
            if (apple != null)
            {
                numbers.Add(apple.number);
            }
        }

        var result = "";
        foreach (var item in numbers)
        {
            result += item.ToString() + ", ";
        }

        numbers.Sort((a, b) => 1 - 2 * Random.Range(0, 2));

        result = "";
        foreach (var item in numbers)
        {
            result += item.ToString() + ", ";
        }

        int k = 0;
        for (int i = 0; i < Apples.Count; i++)
        {
            if (Apples[i] != null)
            {
                Apples[i].number = numbers[k];
                Apples[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = AppleSprite[numbers[k] - 1];
                k++;
            }
        }
    }

    private int CalSelect(HashSet<Apple> Apples)
    {
        int sum = 0;
        foreach (var apple in Apples)
        {
            sum += apple.number;
        }
        return sum;
    }

    public void BoxingApple()
    {
        float appleSum = CalSelect(SelectedApples);
        if (appleSum == 10 || appleSum == 20)
        {
            foreach (var apple in SelectedApples)
            {
                apple?.BoxingOut();
                Apples.Remove(apple);
            }

            _leftApples -= SelectedApples.Count;
            Debug.Log($"남은 사과 : {_leftApples}");
            comboManager.CalBaseComboTime(_leftApples);
            gameManager.CalScore(SelectedApples);

            if (_leftApples == 0)
            {
                gameManager.nextStage();
            }
        }
    }



}
