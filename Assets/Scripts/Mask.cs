using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Material[] materials;
    Person person;
    MeshRenderer meshRenderer;
    void Start()
    {
        person = transform.parent.GetComponent<Person>();
        meshRenderer = transform.GetComponent<MeshRenderer>();
        Material[] tempMaterials = meshRenderer.materials;
        int indexMaterial = 0;
        switch(person.mask)
        {
            case Person.Masks.None:
                indexMaterial = 0;
                break;
            case Person.Masks.Cloth:
                indexMaterial = 1;
                break;
            case Person.Masks.N95:
                indexMaterial = 2;
                break;
        }
        Material mat = Instantiate(materials[indexMaterial]);
        if (person.mask == Person.Masks.None)
        {
            tempMaterials[0] = mat;
        }
        tempMaterials[1] = mat;
        meshRenderer.materials = tempMaterials;
    }
}
