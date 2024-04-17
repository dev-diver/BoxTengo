using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOSdisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
            this.gameObject.SetActive(false);
        #endif
    }
}
