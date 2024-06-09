using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectosCerebro : MonoBehaviour
{
    public float velocidadGiro = 180f; // Velocidad de giro en grados por segundo
    public float velocidadPulso = 1f; // Velocidad del pulso de brillo
    public float amplitudPulso = 0.2f; // Amplitud del pulso de brillo

    private Vector3 escalaInicial;
    private Transform brilloTransform; // Referencia al objeto de brillo

    void Start()
    {
        // Buscar el objeto de brillo como hijo del cerebro
        brilloTransform = transform.Find("EfectoBrillo");

        // Almacenar la escala inicial del objeto de brillo
        if (brilloTransform != null)
        {
            escalaInicial = brilloTransform.localScale;
        }
    }

    void Update()
    {
        // Girar el cerebro sobre su propio eje Y
        transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);


        // Pulsar el efecto de brillo
        if (brilloTransform != null)
        {
            float pulso = Mathf.Sin(Time.time * velocidadPulso) * amplitudPulso;
            brilloTransform.localScale = escalaInicial + Vector3.one * pulso;
        }
    }
}
//a