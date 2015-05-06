using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class conversation : MonoBehaviour {
	string dug;
	string partner;
	bool triggered;
	bool fade;
	Text dialogue;
	int n;
	// Use this for initialization
	void Start () {
		triggered = false;
		dialogue = dialogue = this.gameObject.GetComponent<Text> ();
		n = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered) { //dug should not move
			if (fade) { //while text fades in 
				Color c = dialogue.color;
				c.a += 0.1f * Time.deltaTime * 6;
				dialogue.color = c;
				//print ("text fades in");
				if (dialogue.color.a >= 1) {
					fade = false; //false
					//print ("dialogue.color.a >= 1");
				}
			}
		
			else { //or text fades out
				Color c = dialogue.color;
				c.a -= 0.1f * Time.deltaTime * 6;
				dialogue.color = c;
				//print ("text fades out");
				if (dialogue.color.a <= 0){ //done
					fade = true; //true
					triggered = false;
					//print ("dialogue.color.a <= 0");
					dialogue = null;
				}
			}
		if (Input.GetKey (KeyCode.Space)) {
			n+=1;
		}
	}
}
	void talk() {
		switch (n) {
		case 0:
			dug="Hi.";
			partner="Dug?";
			dialogue.text = partner;
			break;
		case 1:
			dug="I missed you too.";
			partner="I missed you.";
			break;
		case 2:
			dug="I'd really like something to drink.";
			partner="Do you,\n do you need anything?";
			break;
		case 3:
			dug="Thank you.";
			partner="I can do that.";
			break;
		case 4:
			dug="How do I look?";
			partner="How do you feel?";
			break;
		case 5:
			dug="...";
			partner="Like shit.";
			break;
		case 6:
			dug="No, it's fine.";
			partner="I'm sorry.";
			break;
		case 7:
			dug="";
			partner="They never stopped talking about you.";;
			break;
		case 8:
			dug="";
			partner="Every day I'd heard something new.";
			break;
		case 9:
			dug="";
			partner="Sounds like you saved a lot of digglets.";
			break;
		case 10:
			dug="Why'd you let me go?";
			partner="Is something wrong?";
			break;
		case 11:
			dug="One morning, I wake up.";
			partner="What?";
			break;
		case 12:
			dug="And suddenly, I've decided it's my responsibility--";
			partner="";
			break;
		case 13:
			dug="--to save us.";
			partner="";
			break;
		case 14:
			dug="I'd never fought a day in my life. You knew that.";
			partner="";
			break;
		case 15:
			dug="And I tell you that I'm going to kill the FUCKING king.";
			partner="";
			break;
		case 16:
			dug="And you just let me go?"; //smaller font;
			partner="";
			break;
		case 17:
			dug="How about, \"It's too dangerous.\"";
			partner="What was I supposed to say?";
			break;
		case 18:
			dug="\"You're gonna get yourself KILLED.\"";
			partner="";
			break;
		case 19:
			dug="Or just, \"Please. Don't go.\"";
			partner="";
			break;
		case 20:
			dug="There's alot you could have said.";
			partner="";
			break;
		case 21:
			dug="";
			partner="Look how things turned out.";
			break;
		case 22:
			dug="I didn't want to go.";
			partner="Why does it matter?";
			break;
		case 23:
			dug="I'm not so sure about that.";
			partner="No one said you had to.";
			break;
		case 24:
			dug="I didn't do any of this.";
			partner="What're you talking about?";
			break;
		case 25:
			dug="The morning I left, I lost control.";
			partner="";
			break;
		case 26:
			dug="It was like I was a passenger in my own body.";
			partner="";
			break;
		case 27:
			dug="Sitting behind my own eyes.";
			partner="";
			break;
		case 28:
			dug="Just watching as I killed them.";
			partner="";
			break;
		case 29:
			dug="Do you have any idea what that's like?";
			partner="";
			break;
		case 30:
			dug="";
			partner="I...";
			break;
		case 31:
			dug="So this is how it's gonna be from now on?";
			partner="You need some rest, Dug.";
			break;
		case 32:
			dug="If you don't believe me--";
			partner="";
			break;
		case 33:
			dug="";
			partner="--I believe you, Dug.";
			break;
		case 34:
			dug="";
			partner="But you're tired. You need some rest.";
			break;
		case 35:
			//voice="It's time to go Dug; display";
			dug="I don't know.";
			partner="Where are you going?";
			break;
		case 36:
			dug="I love you.";
			break;
		}

	}
	
}
