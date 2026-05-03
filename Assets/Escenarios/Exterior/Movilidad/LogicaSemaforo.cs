using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicaSemaforo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Semáforo Izquierda")]
    public GameObject LuzRojaIzq;
    public GameObject LuzAmarillaIzq;
    public GameObject LuzVerdeIzq;

    [Header("Semáforo Derecha")]
    public GameObject LuzRojaDer;
    public GameObject LuzAmarillaDer;
    public GameObject LuzVerdeDer;

    void Start()
    {
        StartCoroutine(CicloSemaforo());
    }
    IEnumerator CicloSemaforo()
    {
        while (true) 
        {
            SetLuces(LuzVerdeIzq, LuzRojaDer);
            yield return new WaitForSeconds(5f);
            SetLuces(LuzAmarillaIzq, LuzRojaDer);
            yield return new WaitForSeconds(2f);
            SetLuces(LuzRojaIzq, LuzVerdeDer);
            yield return new WaitForSeconds(5f);
            SetLuces(LuzRojaIzq, LuzAmarillaDer);
            yield return new WaitForSeconds(2f);

        }
    }
    void SetLuces(GameObject activaA, GameObject activaB)
    {
  
        LuzRojaIzq.SetActive(false); LuzAmarillaIzq.SetActive(false); LuzVerdeIzq.SetActive(false);
        LuzRojaDer.SetActive(false); LuzAmarillaDer.SetActive(false); LuzVerdeDer.SetActive(false);

       
        activaA.SetActive(true);
        activaB.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
