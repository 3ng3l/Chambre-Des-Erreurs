using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour {

    public GameObject joueur;
    public float vitesseDeplacementJoueur;
    private bool deplacementEnCour = false;
    private Vector3 positionArriverJoueur;
    public GameObject Marqueurs;
	//Variables :
	private RaycastHit hit;
	private Ray joueurRay;
    private GameObject iconeOeil;
    private GameObject iconesChoix;
    private GameObject iconeExit;
    private GameObject iconeLieu;
	private GameObject texteFeedback;
	private GameObject ecranFeedback;
    private GameObject viseur;
    private float attenteValider;
    private float attenteChoix;
    private bool modeChoix;
    private GameObject objetHit;
    private GameObject objZoom;
	private Quaternion saveRotationJoueur;

    // Use this for initialization
    void Start()
	{
        iconeLieu= GameObject.Find("IconeLieu").transform.gameObject;
        iconeExit = GameObject.Find("IconeExit").transform.gameObject;
        iconeOeil = GameObject.Find("IconeOeil").transform.gameObject;
		texteFeedback = GameObject.Find("TexteFeedback").transform.gameObject;
		ecranFeedback = GameObject.Find("PanneauResultat").transform.gameObject;
		ecranFeedback.SetActive (false);
        iconesChoix = GameObject.Find("IconesChoix").transform.gameObject;
        viseur = GameObject.Find("Joueur").transform.FindChild("Camera").transform.FindChild("Sphere").transform.gameObject;
		saveRotationJoueur = joueur.transform.FindChild("Camera").transform.rotation;
        modeChoix = false;
        VariablesGlobales.etatJeu = "play";
    }
    void FixedUpdate()
    {
		if(VariablesGlobales.etatJeu == "play") {
			joueurRay = new Ray (transform.position, transform.forward);
			if (Physics.Raycast (joueurRay, out hit, 20.0f)) {
				//print(hit.collider.name);
				//Mode objets
				if (modeChoix == false) {
					if (!hit.collider.name.Equals ("Sol")) {
						//print(hit.collider.name);
						checknameObject (hit.collider.gameObject);
					} else {
						iconeOeil.transform.position = new Vector3 (0, -3f, 0);
						iconeLieu.transform.position = new Vector3 (0, -3f, 0);
						iconeExit.transform.position = new Vector3 (iconeExit.transform.position.x, -3f, iconeExit.transform.position.z);
						attenteValider = 0.01f;
						attenteChoix = 0.001f;
					}
				}
                //Mode choix ok ou ko
                else {
					if (hit.collider.name == "BtnOk") {
						GestionBarreChoixOk ();
					} else if (hit.collider.name == "BtnKo") {
						GestionBarreChoixKo ();
					} else {
						attenteChoix = 0.001f;
						//On efface les barres de validation des choix
						iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale = new Vector3 (0, iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale.z);
						iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale = new Vector3 (0, iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale.z);
					}
				}
			} else {
				iconeOeil.transform.position = new Vector3 (0, -3f, 0);
				iconeLieu.transform.position = new Vector3 (0, -3f, 0);
				iconeExit.transform.position = new Vector3 (iconeExit.transform.position.x, -3f, iconeExit.transform.position.z);
				//On efface les barres de validation des choix
				iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale = new Vector3 (0, iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild ("BtnOk").transform.FindChild ("BarreValide").transform.localScale.z);
				iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale = new Vector3 (0, iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild ("BtnKo").transform.FindChild ("BarreValide").transform.localScale.z);
				attenteValider = 0.01f;
				attenteChoix = 0.001f;
			}
		}
		else if (VariablesGlobales.etatJeu == "fin") {
			joueurRay = new Ray (transform.position, transform.forward);
			if(Physics.Raycast(joueurRay, out hit, 20.0f)) {
				if(hit.collider.name.Equals("BtnValider")) {
					GestionValiderFin();
				}
			}
		}
        
        if (deplacementEnCour)
        {
            VariablesGlobales.etatJeu = "move";
            joueur.transform.position = Vector3.Lerp(joueur.transform.position, positionArriverJoueur, vitesseDeplacementJoueur);
            float joueurX = (int)(joueur.transform.position.x * 1000) / 1000;
            float positionX = (int)(positionArriverJoueur.x * 1000) / 1000;
            float joueurZ = (int)(joueur.transform.position.z * 1000) / 1000;
            float positionZ = (int)(positionArriverJoueur.z * 1000) / 1000;
            if ( joueurX==positionX && joueurZ==positionZ)
            {
                attenteValider = 0.01f;
                gestionAffichageMarqueurs(true);
                
                deplacementEnCour = false;
                VariablesGlobales.etatJeu = "play";
                //print("Deplacement FINININI");
            }
        }
    }
        // Update is called once per frame
        void Update()
	{
		
	}
    private void checknameObject(GameObject monObjet)
    {
        float angle = 0f;
        float dx = 0f;
        float dz = 0f;
        
        switch (monObjet.name)
        {
            
            case "MarqueurDeplacement":
                //On place l'icone oeil au-dessus de l'objet cible
                 angle = 0f;
                 dx = 0f;
                 dz = 0f;
                dx = monObjet.transform.position.x - gameObject.transform.position.x;
                dz = monObjet.transform.position.z - gameObject.transform.position.z;
                angle = Mathf.Atan2(dx, dz);
                angle = Mathf.Floor(angle * 180 / Mathf.PI);
                iconeLieu.transform.position = new Vector3(monObjet.transform.position.x, monObjet.transform.position.y + 0.5f, monObjet.transform.position.z);
                iconeLieu.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                GestionBarreDeplacement(monObjet);
                
                break;
            default:
                objetHit = monObjet;
                if (monObjet.transform.parent.transform.name.Equals("Objets"))
                {
                    if (monObjet.GetComponent<Objet>().objetInteract == false)
                    {

                        //On place l'icone oeil au-dessus de l'objet cible
                         angle = 0f;
                         dx = 0f;
                         dz = 0f;
                        dx = monObjet.transform.position.x - gameObject.transform.position.x;
                        dz = monObjet.transform.position.z - gameObject.transform.position.z;
                        angle = Mathf.Atan2(dx, dz);
                        angle = Mathf.Floor(angle * 180 / Mathf.PI);
                        iconeOeil.transform.position = new Vector3(monObjet.transform.position.x, monObjet.transform.position.y + 1f, monObjet.transform.position.z);
                        iconeOeil.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        GestionBarreValider();
                    }
                    
                    else {
                        iconeOeil.transform.position = new Vector3(0, -3f, 0);
                        iconeLieu.transform.position = new Vector3(0, -3f, 0);
                        iconeExit.transform.position = new Vector3(iconeExit.transform.position.x, -3f, iconeExit.transform.position.z);
                        attenteValider = 0.01f;
                        attenteChoix = 0.001f;
                    }
                }
                else if (monObjet.name == "Porte")
                {
                    iconeExit.transform.position = new Vector3(iconeExit.transform.position.x, 2f, iconeExit.transform.position.z);

                    GestionBarreExit();
                }
                else
                {
                    iconeOeil.transform.position = new Vector3(0, -3f, 0);
                    iconeLieu.transform.position = new Vector3(0, -3f, 0);
                    iconeExit.transform.position = new Vector3(iconeExit.transform.position.x, -3f, iconeExit.transform.position.z);
                    attenteValider = 0.01f;
                    attenteChoix = 0.001f;
                }
                
                break;
        }
       
    }
    private void lancerDeplacement(Vector3 positionArriver)
    {
        if (deplacementEnCour == false)
        {
            gestionAffichageMarqueurs(false);
            positionArriverJoueur = new Vector3(positionArriver.x, joueur.transform.position.y, positionArriver.z);
            deplacementEnCour = true;
        }        
    }
    private void GestionBarreExit()
    {
        iconeExit.transform.FindChild("BarreValide").transform.localScale = new Vector2(attenteValider, iconeOeil.transform.FindChild("BarreValide").transform.localScale.y);
        attenteValider += 0.04f;
        if (attenteValider >= 2)
        {
            print("Fin du jeu");
            VariablesGlobales.etatJeu = "exit";
            iconeExit.transform.position = new Vector3(iconeExit.transform.position.x, -3f, iconeExit.transform.position.z);
            //Fondu
            VariablesGlobales.etatFondu = "in";
            //On affiche l'ecran de bilan
            Invoke("AfficherEcranBilan", 2);
        }
    }
    private void AfficherEcranBilan()
    {
        gestionAffichageMarqueurs(false);
        joueur.transform.position = new Vector3(5, joueur.transform.position.y, -4.8f);
		EcranFeedback ();
        //Fondu
        VariablesGlobales.etatFondu = "off";
    }
    private void GestionBarreDeplacement(GameObject monObjet)
    {
        iconeLieu.transform.FindChild("BarreValide").transform.localScale = new Vector2(attenteValider, iconeOeil.transform.FindChild("BarreValide").transform.localScale.y);
        attenteValider += 0.04f;
        if (attenteValider >= 2)
        {
            //On efface l'icone oeil et la barre de chargement
            iconeLieu.transform.position = new Vector3(0, -5f, 0);
            lancerDeplacement(monObjet.transform.position);
        }
    }
    private void GestionBarreValider()
    {
        iconeOeil.transform.FindChild("BarreValide").transform.localScale = new Vector2(attenteValider, iconeOeil.transform.FindChild("BarreValide").transform.localScale.y);
        attenteValider += 0.04f;
        if (attenteValider >= 2)
        {
            modeChoix = true;
            //On efface l'icone oeil et la barre de chargement
            iconeOeil.transform.position = new Vector3(0, -5f, 0);
            //On affiche le choix ok/ko
            gestionAffichageMarqueurs(false);
            float angle = 0f;
            float dx = 0f;
            float dz = 0f;
            dx = viseur.transform.position.x - gameObject.transform.position.x;
            dz = viseur.transform.position.z - gameObject.transform.position.z;
            angle = Mathf.Atan2(dx, dz);
            angle = Mathf.Floor(angle * 180 / Mathf.PI);
            iconesChoix.transform.position = new Vector3(viseur.transform.position.x, viseur.transform.position.y + 0.05f, viseur.transform.position.z);
            iconesChoix.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //On efface les barres de validation des choix
            iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale = new Vector3(0, iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale.z);
            iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale = new Vector3(0, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.z);
            //On affiche l'objet en zoom
            objZoom = Instantiate(Resources.Load(objetHit.name + "Zoom", typeof(GameObject))) as GameObject;
            objZoom.transform.position = new Vector3(viseur.transform.position.x, viseur.transform.position.y + 0.05f, viseur.transform.position.z);
        }
    }

    private void GestionBarreChoixOk()
    {
        iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale = new Vector3(attenteChoix, iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnOk").transform.FindChild("BarreValide").transform.localScale.z);
        attenteChoix += 0.02f;
        if (attenteChoix >= 1.2f)
        {
            VariablesGlobales.etatJeu = "feedback";
            iconesChoix.transform.position = new Vector3(0, -5f, 0);
            objetHit.GetComponent<Objet>().objetInteract = true;
            //On teste si c'est ok pour l'objet
            if (objetHit.GetComponent<Objet>().objetOk == true)
            {
                print("Bonne réponse !!!");
                objetHit.GetComponent<Objet>().reponseJuste = true;
				print ( objetHit.GetComponent<Objet>().messageFeedback(true));
				AfficherFeedback(objetHit.GetComponent<Objet>().messageFeedback(true));
            }
            else {
                print("Mauvais choix !!!");
                objetHit.GetComponent<Objet>().reponseJuste = false;
				print ( objetHit.GetComponent<Objet>().messageFeedback(false));
				AfficherFeedback(objetHit.GetComponent<Objet>().messageFeedback(false));
            }
            //On supprime l'objet zoom
            Destroy(objZoom);
			//On affiche le 
            //Suite
            Invoke("RepriseJeu", 5f);
        }
    }
    private void GestionBarreChoixKo()
    {
        iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale = new Vector3(attenteChoix, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.z);
        attenteChoix += 0.02f;
        if (attenteChoix >= 1.2f)
        {
            VariablesGlobales.etatJeu = "feedback";
            iconesChoix.transform.position = new Vector3(0, -5f, 0);
            objetHit.GetComponent<Objet>().objetInteract = true;
            //On teste si c'est ok pour l'objet
            if (objetHit.GetComponent<Objet>().objetOk == false)
            {
                print("Bonne réponse !!!");
                objetHit.GetComponent<Objet>().reponseJuste = true;
				print ( objetHit.GetComponent<Objet>().messageFeedback(true));
				AfficherFeedback(objetHit.GetComponent<Objet>().messageFeedback(true));
            }
            else {
                print("Mauvais choix !!!");
                objetHit.GetComponent<Objet>().reponseJuste = false;
				print ( objetHit.GetComponent<Objet>().messageFeedback(false));
				AfficherFeedback(objetHit.GetComponent<Objet>().messageFeedback(false));
            }
            //On supprime l'objet zoom
            Destroy(objZoom);
            //Suite
            Invoke("RepriseJeu", 5f);
        }
    }

	//Valider fin
	private void GestionValiderFin() {
		ecranFeedback.transform.FindChild("BtnValider").transform.FindChild("BarreValide").transform.localScale = new Vector3(attenteValider, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.z);
		attenteValider += 0.02f;
		if(attenteValider >= 1.2f) {
			VariablesGlobales.etatJeu = "reset";
			//Fondu
			VariablesGlobales.etatFondu = "in";
			//Suite
			Invoke("ReseterJeu", 3f);
		}
	}

	//Reseter le jeu
	private void ReseterJeu() {
		ecranFeedback.SetActive(false);
		//On reset les data
		var nbrEle = GameObject.Find("Objets").transform.childCount;
		for (var c = 0; c < nbrEle; c++)
		{
			GameObject.Find("Objets").transform.GetChild(c).gameObject.GetComponent<Objet>().reponseJuste = false;
			GameObject.Find("Objets").transform.GetChild(c).gameObject.GetComponent<Objet>().objetInteract = false;
		}
		//On replace le joueur devant la porte
		joueur.transform.position = new Vector3(3, joueur.transform.position.y, -4.6f);
		joueur.transform.FindChild("Camera").transform.rotation = saveRotationJoueur;
		gestionAffichageMarqueurs(true);
		VariablesGlobales.etatJeu = "play";
		//Fondu
		VariablesGlobales.etatFondu = "off";
	}

	//Message feedback
	private void AfficherFeedback(string phrase) {
		float angle = 0f;
		float dx = 0f;
		float dz = 0f;
		dx = viseur.transform.position.x - gameObject.transform.position.x;
		dz = viseur.transform.position.z - gameObject.transform.position.z;
		angle = Mathf.Atan2(dx, dz);
		angle = Mathf.Floor(angle * 180 / Mathf.PI);
		texteFeedback.transform.position = new Vector3(viseur.transform.position.x, viseur.transform.position.y + 0.07f, viseur.transform.position.z);
		texteFeedback.transform.rotation = Quaternion.Euler(0f, angle, 0f);
		//Phrase
		texteFeedback.GetComponent<TextMesh>().text = phrase;
	}

    //Suite du jeu
    private void RepriseJeu()
    {
        print("Reprise jeu !");
		texteFeedback.transform.position = new Vector3(viseur.transform.position.x, -4f, viseur.transform.position.z);
        VariablesGlobales.etatJeu = "play";
        modeChoix = false;
        gestionAffichageMarqueurs(true);
    }
    private void gestionAffichageMarqueurs(bool affichageOK)
    {
        Marqueurs.SetActive(affichageOK);
    }

	//Ecran resultat
	private void EcranFeedback() {
		attenteValider = 0.01f;
		VariablesGlobales.etatJeu = "fin";
		ecranFeedback.SetActive (true);
		//Controle des resultats
		var nbrRepJuste = 0;
		//On recupere la liste des objets a controler
		var nbrEle = GameObject.Find("Objets").transform.childCount;
		for (var c = 0; c < nbrEle; c++)
		{
			if (GameObject.Find("Objets").transform.GetChild(c).gameObject.GetComponent<Objet>().reponseJuste == true && GameObject.Find("Objets").transform.GetChild(c).gameObject.GetComponent<Objet>().objetInteract == true)
			{
				nbrRepJuste++;
			}
		}
		print("Résultat : " + nbrRepJuste + "/" + nbrEle);
		ecranFeedback.transform.FindChild ("Label2").GetComponent<TextMesh> ().text = nbrRepJuste + "/" + nbrEle;
		ecranFeedback.transform.FindChild("BtnValider").transform.FindChild("BarreValide").transform.localScale = new Vector3(0, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.y, iconesChoix.transform.FindChild("BtnKo").transform.FindChild("BarreValide").transform.localScale.z);
	}
}
