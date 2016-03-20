using UnityEngine;
using System.Collections;

public class scriptFondu : MonoBehaviour {

	//Variables
	private Rect rectangle;
	private Texture2D texture;
	private int screenW;
	private int screenH;
	private float degradeNoir;
	private int dureeEcranNoir;
	
	void Awake() {

		screenW = Screen.width;
		screenH = Screen.height;

	}
	
	//Use this for initialization
	void Start() {

		dureeEcranNoir = 0;

		rectangle = new Rect (0, 0, screenW, screenH);
		texture = new Texture2D(1, 1);

	}
	
	//Update is called once per frame
	void Update() {
	
	}

	//GUI
	void OnGUI() {

		if(VariablesGlobales.etatFondu == "off") {
			degradeNoir = 0.0f;
		}
		else if(VariablesGlobales.etatFondu == "on") {
			degradeNoir = 1.1f;

			//On modifie l'alpha
			Color newColor = new Color(0, 0, 0, degradeNoir);          
			texture.SetPixel(0, 0, newColor);
			texture.Apply();
			
			//Affichage
			GUI.skin.box.normal.background = texture;
			GUI.Box(rectangle, GUIContent.none);
		}
		else {
			if(VariablesGlobales.etatFondu == "in") {

				//On modifie l'alpha
				degradeNoir += 0.006f;

				//Fin
				if(degradeNoir >= 1.0f) {
					degradeNoir = 1.0f;
					if(dureeEcranNoir >= 60) {
						dureeEcranNoir = 0;
						VariablesGlobales.etatFondu = "on";
					}
					else if(dureeEcranNoir == 0) {
						dureeEcranNoir++;
					}
					else {
						dureeEcranNoir++;
					}

				}

			}
			else if(VariablesGlobales.etatFondu == "out") {

				//On modifie l'alpha
				degradeNoir -= 0.01f;

				//Fin
				if(degradeNoir <= 0.0f) {
					VariablesGlobales.etatFondu = "off";
				}

			}

			//On modifie l'alpha
			Color newColor = new Color(0, 0, 0, degradeNoir);          
			texture.SetPixel(0, 0, newColor);
			texture.Apply();

			//Affichage
			GUI.skin.box.normal.background = texture;
			GUI.Box(rectangle, GUIContent.none);
		}

	}
}
