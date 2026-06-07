using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Config_Resolucion : MonoBehaviour
{
    public Toggle toggleFullscreen;
    public TMP_Dropdown dropdownResoluciones;

    private List<Resolution> listaResolucionesFiltradas = new List<Resolution>();

    void Start()
    {
        // 1. Configurar Pantalla Completa Inicial
        int pantallaCompletaGuardada = PlayerPrefs.GetInt("PantallaCompleta", 1);
        bool esFullscreen = (pantallaCompletaGuardada == 1);
        toggleFullscreen.SetIsOnWithoutNotify(esFullscreen);

        // 2. Cargar y Filtrar las Resoluciones del Monitor
        ConfigurarMenuResoluciones();

        // 3. Escuchar los cambios de la UI de forma automática
        toggleFullscreen.onValueChanged.AddListener(CambiarPantallaCompleta);
        dropdownResoluciones.onValueChanged.AddListener(CambiarResolucion);
    }

    private void ConfigurarMenuResoluciones()
    {
        Resolution[] todasLasResoluciones = Screen.resolutions;
        dropdownResoluciones.ClearOptions();

        List<string> opcionesTexto = new List<string>();
        listaResolucionesFiltradas.Clear();

        // Filtrar duplicados de Hercios (Hz) para dejar solo Width x Height limpias
        for (int i = 0; i < todasLasResoluciones.Length; i++)
        {
            // Solo nos interesan las opciones que tengan una tasa de refresco nativa o común
            // (Evitamos añadir la misma resolución repetida varias veces)
            bool yaExiste = false;
            foreach (Resolution res in listaResolucionesFiltradas)
            {
                if (res.width == todasLasResoluciones[i].width && res.height == todasLasResoluciones[i].height)
                {
                    yaExiste = true;
                    break;
                }
            }

            if (!yaExiste)
            {
                listaResolucionesFiltradas.Add(todasLasResoluciones[i]);
                string opcion = todasLasResoluciones[i].width + " x " + todasLasResoluciones[i].height;
                opcionesTexto.Add(opcion);
            }
        }

        dropdownResoluciones.AddOptions(opcionesTexto);

        // Buscar qué índice corresponde a la resolución actual para inicializar bien el menú
        int indiceResolucionActual = 0;
        for (int i = 0; i < listaResolucionesFiltradas.Count; i++)
        {
            if (listaResolucionesFiltradas[i].width == Screen.currentResolution.width &&
                listaResolucionesFiltradas[i].height == Screen.currentResolution.height)
            {
                indiceResolucionActual = i;
            }
        }

        // Si el usuario ya había guardado una resolución antes, usamos esa.
        // Si no (da -1), usamos la resolución actual que acabamos de detectar.
        int indiceGuardado = PlayerPrefs.GetInt("numeroResolucion", -1);
        if (indiceGuardado != -1 && indiceGuardado < listaResolucionesFiltradas.Count)
        {
            indiceResolucionActual = indiceGuardado;
        }

        dropdownResoluciones.SetValueWithoutNotify(indiceResolucionActual);
        dropdownResoluciones.RefreshShownValue();

        // Aplicamos la resolución cargada por primera vez
        AplicarCambiosDePantalla(indiceResolucionActual, Screen.fullScreen);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", indiceResolucion);
        PlayerPrefs.Save();
        AplicarCambiosDePantalla(indiceResolucion, Screen.fullScreen);
    }

    public void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();
        AplicarCambiosDePantalla(dropdownResoluciones.value, pantallaCompleta);
    }

    private void AplicarCambiosDePantalla(int indiceResolucion, bool fullscreen)
    {
        if (listaResolucionesFiltradas.Count == 0) return;

        Resolution resolucionSeleccionada = listaResolucionesFiltradas[indiceResolucion];

        FullScreenMode modoPantalla = fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, modoPantalla);
    }
}