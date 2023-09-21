using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    SkinnedMeshRenderer M_translucent;
    public Material[] mater;
    void Start()
    {
        M_translucent = GetComponent<SkinnedMeshRenderer>();

        Material newMaterial = mater[0];

        Material[] materials = M_translucent.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = newMaterial; // 새로운 Material을 적용합니다.
        }
        M_translucent.materials = materials;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
