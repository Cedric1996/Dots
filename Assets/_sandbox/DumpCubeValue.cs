using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpCubeValue : MonoBehaviour
{
    RepCube2 repCube;
    // Start is called before the first frame update
    void Start()
    {
        repCube = GetComponent<RepCube2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            //  repCube.Value;
            Debug.Log(repCube.Value.position.x);
        }
    }
}
