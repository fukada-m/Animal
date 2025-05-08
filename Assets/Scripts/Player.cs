using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject LastFrend {get; set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LastFrend = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
