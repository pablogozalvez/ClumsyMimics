using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlItems : MonoBehaviour
{
    public float desplazamiento = 0.25f;
    public string tipoDeItem;
    public float cooldownRecoger = 1f;

    public Transform textura;
    public Transform sombra;
    public Vector3 tamanyoInicioSombra = new Vector3(0.5f, 0.1f, 1f);
    public Vector3 tamanyoFinalSombra = new Vector3(0.4f, 0.06f, 1f);
    private ControlInventario inventario;
    private bool collectable = false;
    private static int numeroItems = 0;
    private int id;
    void Start()
    {
        numeroItems++;
        id = numeroItems;
        inventario = FindObjectOfType<ControlInventario>();
        StartCoroutine(EmpezarCooldown());
        StartCoroutine(EmpezarAnimacion());
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (!other.CompareTag("Player"))
            return;

        if (!collectable || inventario.InventarioLleno())
            return;

        inventario.AnyadirObjetoExterno(tipoDeItem, id);
        Destroy(gameObject);
    }

    private IEnumerator EmpezarCooldown()
    {
        if (collectable)
        yield return false;

        yield return new WaitForSeconds(cooldownRecoger);
        collectable = true;
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
