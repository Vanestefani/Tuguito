using UnityEngine;

public class Menu_Muebles : MonoBehaviour
{
    public GameObject Panel_muebles;
    private bool PanelActivo = false;
     void Start()
    {
        Panel_muebles.SetActive(false);
        Cursor.visible = true;
    }

    public void Abrir_MenuMuebles()
    {
        if (!PanelActivo)
        {
            PanelActivo = true;
            Panel_muebles.SetActive(true);
            Debug.Log("Panel de muebles abierto");
        }
    }
    public void Cerrar_MenuMuebles()
    {
        if (PanelActivo)
        {
            PanelActivo = false;
            Panel_muebles.SetActive(false);
            Debug.Log("Panel de muebles cerrado");
        }
    }
    public void Alternar_MenuColores()
    {
        if (PanelActivo)
            Cerrar_MenuMuebles();
        else
            Abrir_MenuMuebles();
    }

}
