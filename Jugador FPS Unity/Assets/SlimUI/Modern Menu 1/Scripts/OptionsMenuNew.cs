using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuNew : MonoBehaviour {

	public enum Platform {Desktop, Mobile};
	public Platform platform;

	[Header("VIDEO SETTINGS")]
	public GameObject fullscreentext;
	public GameObject vsynctext;
	public GameObject texturelowtextLINE;
	public GameObject texturemedtextLINE;
	public GameObject texturehightextLINE;

	// sliders
	public GameObject musicSlider;
	private float sliderValue = 0.0f;
	

	public void  Start (){

		// check slider values
		musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

		// check full screen
		if(Screen.fullScreen == true){
			fullscreentext.GetComponent<TMP_Text>().text = "ON";
		}
		else if(Screen.fullScreen == false){
			fullscreentext.GetComponent<TMP_Text>().text = "OFF";
		}


		// check vsync
		if(QualitySettings.vSyncCount == 0){
			vsynctext.GetComponent<TMP_Text>().text = "OFF";
		}
		else if(QualitySettings.vSyncCount == 1){
			vsynctext.GetComponent<TMP_Text>().text = "ON";
		}

		// check texture quality
		if(PlayerPrefs.GetInt("Textures") == 0){
			QualitySettings.masterTextureLimit = 2;
			texturelowtextLINE.gameObject.SetActive(true);
			texturemedtextLINE.gameObject.SetActive(false);
			texturehightextLINE.gameObject.SetActive(false);
		}
		else if(PlayerPrefs.GetInt("Textures") == 1){
			QualitySettings.masterTextureLimit = 1;
			texturelowtextLINE.gameObject.SetActive(false);
			texturemedtextLINE.gameObject.SetActive(true);
			texturehightextLINE.gameObject.SetActive(false);
		}
		else if(PlayerPrefs.GetInt("Textures") == 2){
			QualitySettings.masterTextureLimit = 0;
			texturelowtextLINE.gameObject.SetActive(false);
			texturemedtextLINE.gameObject.SetActive(false);
			texturehightextLINE.gameObject.SetActive(true);
		}
	}

	public void  Update (){
		sliderValue = musicSlider.GetComponent<Slider>().value;
	}

	public void  FullScreen (){
		Screen.fullScreen = !Screen.fullScreen;

		if(Screen.fullScreen == true){
			fullscreentext.GetComponent<TMP_Text>().text = "ON";
		}
		else if(Screen.fullScreen == false){
			fullscreentext.GetComponent<TMP_Text>().text = "OFF";
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

	public void  TexturesLow (){
		PlayerPrefs.SetInt("Textures",0);
		QualitySettings.masterTextureLimit = 2;
		texturelowtextLINE.gameObject.SetActive(true);
		texturemedtextLINE.gameObject.SetActive(false);
		texturehightextLINE.gameObject.SetActive(false);
	}

	public void  TexturesMed (){
		PlayerPrefs.SetInt("Textures",1);
		QualitySettings.masterTextureLimit = 1;
		texturelowtextLINE.gameObject.SetActive(false);
		texturemedtextLINE.gameObject.SetActive(true);
		texturehightextLINE.gameObject.SetActive(false);
	}

	public void  TexturesHigh (){
		PlayerPrefs.SetInt("Textures",2);
		QualitySettings.masterTextureLimit = 0;
		texturelowtextLINE.gameObject.SetActive(false);
		texturemedtextLINE.gameObject.SetActive(false);
		texturehightextLINE.gameObject.SetActive(true);
	}
}