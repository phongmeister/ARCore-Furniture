using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject2 : MonoBehaviour
{
    public GameObject myPrefabObject = null;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Instantiate(myPrefabObject, transform.position, Quaternion.Euler(0f, 0f, 180f));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
