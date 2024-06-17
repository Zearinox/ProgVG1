using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoObjetivo : MonoBehaviour
{
    public float duracionEnPantalla = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        duracionEnPantalla -= Time.deltaTime;
        if (duracionEnPantalla <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
