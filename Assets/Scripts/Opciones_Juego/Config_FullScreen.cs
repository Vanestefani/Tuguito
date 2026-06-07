using UnityEngine;
using UnityEngine.UI;
public class Config_FullScreen : MonoBehaviour
{
    public Toggle toggle;
      void Start()
    {
        int pantallaCompletaGuardada = PlayerPrefs.GetInt("PantallaCompleta", 1);
        bool esFullscreen = (pantallaCompletaGuardada == 1);
        toggle.SetIsOnWithoutNotify(esFullscreen);
        ActiveFULLS(esFullscreen);
        toggle.onValueChanged.AddListener(ActiveFULLS);
        
    }
    public void ActiveFULLS(bool fullscreen)
    {
        if (fullscreen)
        {
              Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
            PlayerPrefs.SetInt("PantallaCompleta", 1);
        }
        else
        {
             Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
            PlayerPrefs.SetInt("PantallaCompleta", 0);
        }

        PlayerPrefs.Save(); 
    }
}


