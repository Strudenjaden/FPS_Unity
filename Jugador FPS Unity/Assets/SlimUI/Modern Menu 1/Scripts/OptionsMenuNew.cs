using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuNew : MonoBehaviour {

	[Header("VIDEO SETTINGS")]
	public GameObject fullscreentext;
	public GameObject vsynctext;
	public GameObject texturelowtextLINE;
	public GameObject texturemedtextLINE;
	public GameObject texturehightextLINE;
	

	// sliders
	public GameObject musicSlider;
	private float sliderValue = 0.0f;

	//Valor que le pasamos al script del Menú de Opciones en la otra escena y que utilizamos para comprobar.
	public static int qualityValue;
	

	public void  Start (){

		// check slider values
		musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

		 //check full screen
		if(Screen.fullScreen == true){
			fullscreentext.GetComponent<TMP_Text>().text = "PANTALLA COMPLETA";
		}
		else if(Screen.fullScreen == false){
			fullscreentext.GetComponent<TMP_Text>().text = "MODO VENTANA";
		}


		// check vsync
		if(QualitySettings.vSyncCount == 0){
			vsynctext.GetComponent<TMP_Text>().text = "OFF";
		}
		else if(QualitySettings.vSyncCount == 1){
			vsynctext.GetComponent<TMP_Text>().text = "ON";
		}

		// check texture quality
		if (qualityValue == 0)
		{
			texturemedtextLINE.SetActive(false);
			texturelowtextLINE.SetActive(false);

		}

		if (qualityValue == 1)
		{
			texturehightextLINE.SetActive(false);
			texturelowtextLINE.SetActive(false);

		}

		if (qualityValue== 2)
		{
			texturehightextLINE.SetActive(false);
			texturemedtextLINE.SetActive(false);

		}
	}

	public void  Update (){
		sliderValue = musicSlider.GetComponent<Slider>().value;

	}

	public void  FullScreen (){
		
		Screen.fullScreen = !Screen.fullScreen;
		Debug.Log(Screen.fullScreen);

		if(Screen.fullScreen == true){
			fullscreentext.GetComponent<TMP_Text>().text = "MODO VENTANA";
		}
		else if(Screen.fullScreen == false){
			fullscreentext.GetComponent<TMP_Text>().text = "PANTALLA COMPLETA";
		}
	}

	public void MusicSlider (){
		PlayerPrefs.SetFloat("MusicVolume", sliderValue);
	}

	public void vsync (){
		if(QualitySettings.vSyncCount == 0){
			QualitySettings.vSyncCount = 1;
			vsynctext.GetComponent<TMP_Text>().text = "ON";
			
		}
		else if(QualitySettings.vSyncCount == 1){
			QualitySettings.vSyncCount = 0;
			vsynctext.GetComponent<TMP_Text>().text = "OFF";
		}
	}

    public void QualityManager(int qualityIndex)
    {
		QualitySettings.SetQualityLevel(qualityIndex);

		if (qualityIndex == 0)
        {
			qualityValue = qualityIndex;
            texturemedtextLINE.SetActive(false);
            texturelowtextLINE.SetActive(false);

        }

        if (qualityIndex == 1)
        {
			qualityValue = qualityIndex;
			texturehightextLINE.SetActive(false);
            texturelowtextLINE.SetActive(false);

        }

        if (qualityIndex == 2)
        {
			qualityValue = qualityIndex;
			texturehightextLINE.SetActive(false);
            texturemedtextLINE.SetActive(false);

        }
    }
}