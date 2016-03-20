using UnityEngine;
using System.Collections;

public class Objet : MonoBehaviour {

	//Variables
	public string _nom;
	public bool _objetOk;
	public bool _reponseJuste;
	public bool _objetInteract;

	public bool objetOk
	{
		get { return _objetOk; }
		set { _objetOk = value; }
	}

	public bool reponseJuste
	{
		get { return _reponseJuste; }
		set { _reponseJuste = value; }
	}

	public bool objetInteract
	{
		get { return _objetInteract; }
		set { _objetInteract = value; }
	}

	public string messageFeedback(bool justeOk)
	{
		string phraseResultat = "";
		switch(gameObject.name)
		{
		case "Mask":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nUn masque utilisé, c'est un masque à jeter !";
			} else {
				phraseResultat = "C'est incorrect.\nUn masque utilisé, est un masque contaminé.\nIl ne se réutilise jamais, mais se jette dès qu'on le retire.";
			}
			break;
		case "sha":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nLe SHA est périmé ! Un flacon entamé, ça ne se garde que 6 mois !";
			} else {
				phraseResultat = "C'est incorrect.\nUtiliser un produit périmé, c'est aussi risqué que de manger steak avarié.\nUn flacon entamé, ça ne se garde que 6 mois !\n";
			}
			break;
		case "Cactus":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nPot de fleur ou vase, c'est la même chanson ,c'est un risque d'infection\npour un patient immunodéprimé.\nPour un patient qui ne l'est pas, les fleurs ce n'est pas anormal.";
			} else {
				phraseResultat = "C'est incorrect.\nPot de fleur ou vase, c'est la même chanson, c'est un risque d'infection\npour un patient immunodéprimé.\nPour un patient qui ne l'est pas, les fleurs ce n'est pas anormal.";
			}
			break;
		case "Blood":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nUne souillure sur une surface est un réservoir de germes\npouvant transmettre un micro-organisme dangereux.";
			} else {
				phraseResultat = "C'est incorrect.\nUne souillure, c'est comme une mine !\nOn la touche, elle fait des victimes.";
			}
			break;
		case "Syringue":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nC’est bien, une aiguille qui traine, c'est un risque de piqure !\nUn virus te guette !";
			} else {
				phraseResultat = "C'est incorrect.\nQui dit aiguille, dit container pour pouvoir éliminer immédiatement\ntout objet piquant, coupant ou tranchant.";
			}
			break;
		case "towel":
			if (justeOk) {
				phraseResultat = "Bonne réponse !\nLinge ramassé = linge à évacuer, car contaminé par le sol !";
			} else {
				phraseResultat = "C'est incorrect.\nIl n’y a que à la maison qu’on a le droit\nde laisser trainer son slip.";
			}
			break;
		case "Book":
			if (justeOk) {
				phraseResultat = "";
			} else {
				phraseResultat = "";
			}
			break;
		case "clock":
			if (justeOk) {
				phraseResultat = "";
			} else {
				phraseResultat = "";
			}
			break;
		case "TV":
			if (justeOk) {
				phraseResultat = "";
			} else {
				phraseResultat = "";
			}
			break;
		case "Vin":
			if (justeOk) {
				phraseResultat = "Un p'tit verre ça ne fait pas de mal !";
			} else {
				phraseResultat = "Il faut savoir faire la fête des fois !";
			}
			break;
		}
		return phraseResultat;
	}
}
