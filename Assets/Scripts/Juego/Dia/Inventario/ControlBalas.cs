using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBalas : MonoBehaviour
{
    public float desplazamiento = 0.1f;

    public Transform textura;
    public Transform sombra;
    public Vector3 tamanyoInicioSombra = new Vector3(0.5f, 0.1f, 1f);
    public Vector3 tamanyoFinalSombra = new Vector3(0.4f, 0.06f, 1f);
    private ControlDia controlDia;
    private bool collectable = false;
    void Start()
    {
        controlDia = FindObjectOfType<ControlDia>();
        StartCoroutine(EmpezarAnimacion());
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (!other.CompareTag("Player"))
            return;

        if (controlDia.RecuentoBalas() >= 5)
            return;
            
        controlDia.AnadirBala();
        Destroy(gameObject);
    }

    private IEnumerator EmpezarAnimacion()
    {
        while (true)
        {
            float elapsedTime = 0f;
            float animationDuration = 1.5f;
            Vector3 initialTexturePosition = textura.position;

            Vector3 targetTexturePosition = new Vector3(textura.position.x, textura.position.y + desplazamiento, textura.position.z);

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / animationDuration);
                textura.position = Vector3.Lerp(initialTexturePosition, targetTexturePosition, t);
                sombra.localScale = Vector3.Lerp(tamanyoInicioSombra, tamanyoFinalSombra, t);
                yield return null;
            }

            elapsedTime = 0f;
            initialTexturePosition = textura.position;

            targetTexturePosition = new Vector3(textura.position.x, textura.position.y - desplazamiento, textura.position.z);

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / animationDuration);
                textura.position = Vector3.Lerp(initialTexturePosition, targetTexturePosition, t);
                sombra.localScale = Vector3.Lerp(tamanyoFinalSombra, tamanyoInicioSombra, t);
                yield return null;
            }
        }
    }
}
