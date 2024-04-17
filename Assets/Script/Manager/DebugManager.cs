using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool debugMode = false;
    public static DebugManager Instance;
    private void Awake()
    {
        if(Instance !=null)
		{
			Destroy(gameObject); //새로 생긴걸 파괴
			return;
		}
		Instance = this; //this는 현재 Instance를 가리킴.
        DontDestroyOnLoad(gameObject);
    }

}
