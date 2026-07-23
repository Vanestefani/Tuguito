using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public GameObject panelDialogo;
    public TMP_Text textoNombre;
    public TMP_Text textoDialogo;
    public Image imagenRetrato;
    public Button[] botonesOpcion;
    public TMP_Text[] textosOpcion;

    private NPCInteractable npcActual;
    private NodoDialogo nodoActual;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }

    public void IniciarDialogo(NPCInteractable npc)
    {
        npcActual = npc;
        panelDialogo.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (textoNombre != null)
            textoNombre.text = npc.nombreVisible;

        MostrarNodo(npc.dialogoData.idNodoInicial);
    }

    private void MostrarNodo(string idNodo)
    {
        nodoActual = npcActual.dialogoData.ObtenerNodo(idNodo);
        if (nodoActual == null)
        {
            Debug.LogWarning($"Nodo de dialogo no encontrado: {idNodo}");
            CerrarDialogo();
            return;
        }

        textoDialogo.text = nodoActual.texto;

        if (imagenRetrato != null)
            imagenRetrato.sprite = npcActual.ObtenerRetrato(nodoActual.emocion);

        MostrarOpciones();
    }

    private void MostrarOpciones()
    {
        int afinidad = npcActual.ObtenerAfinidad();
        int reputacionGlobal = ReputacionManager.Instance != null ? ReputacionManager.Instance.ObtenerReputacion() : 0;

        List<OpcionDialogo> opcionesVisibles = new List<OpcionDialogo>();

        foreach (var opcion in nodoActual.opciones)
        {
            bool cumpleAfinidad = afinidad >= opcion.afinidadRequerida;
            bool cumpleReputacion = reputacionGlobal >= opcion.reputacionGlobalRequerida;
            bool yaUsada = opcion.unaSolaVez && npcActual.OpcionYaUsada(opcion.idOpcion);
            bool puedePagar = opcion.costoMonedas <= 0 ||
                (EconomiaManager.Instance != null && EconomiaManager.Instance.PuedeComprar(opcion.costoMonedas));

            if (cumpleAfinidad && cumpleReputacion && !yaUsada && puedePagar)
                opcionesVisibles.Add(opcion);
        }

        if (nodoActual.esFinal || opcionesVisibles.Count == 0)
        {
            for (int i = 1; i < botonesOpcion.Length; i++)
                botonesOpcion[i].gameObject.SetActive(false);

            botonesOpcion[0].gameObject.SetActive(true);
            textosOpcion[0].text = "Cerrar";
            botonesOpcion[0].onClick.RemoveAllListeners();
            botonesOpcion[0].onClick.AddListener(CerrarDialogo);
            return;
        }

        for (int i = 0; i < botonesOpcion.Length; i++)
        {
            if (i < opcionesVisibles.Count)
            {
                OpcionDialogo opcion = opcionesVisibles[i];
                botonesOpcion[i].gameObject.SetActive(true);
                textosOpcion[i].text = opcion.texto;
                botonesOpcion[i].onClick.RemoveAllListeners();
                botonesOpcion[i].onClick.AddListener(() => SeleccionarOpcion(opcion));
            }
            else
            {
                botonesOpcion[i].gameObject.SetActive(false);
            }
        }
    }

    private void SeleccionarOpcion(OpcionDialogo opcion)
    {
        // Costo (si aplica) se cobra antes de dar recompensas
        if (opcion.costoMonedas > 0)
            EconomiaManager.Instance?.GastarMonedas(opcion.costoMonedas);

        if (opcion.cambioAfinidad != 0)
            npcActual.SumarAfinidad(opcion.cambioAfinidad);

        if (opcion.cambioReputacionGlobal != 0)
            ReputacionManager.Instance?.SumarReputacion(opcion.cambioReputacionGlobal);

        if (opcion.experienciaGanada != 0)
            ExperienciaManager.Instance?.GanarExperiencia(opcion.experienciaGanada);

        if (opcion.monedasGanadas != 0)
            EconomiaManager.Instance?.GanarMonedas(opcion.monedasGanadas);

        if (opcion.unaSolaVez)
            npcActual.MarcarOpcionUsada(opcion.idOpcion);

        if (string.IsNullOrEmpty(opcion.siguienteNodoId))
            CerrarDialogo();
        else
            MostrarNodo(opcion.siguienteNodoId);
    }

    public void CerrarDialogo()
    {
        panelDialogo.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (npcActual != null)
            npcActual.TerminarInteraccion(null);

        npcActual = null;
        nodoActual = null;
    }
}