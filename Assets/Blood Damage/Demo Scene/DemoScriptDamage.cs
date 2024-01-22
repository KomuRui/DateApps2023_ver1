using UnityEngine;
using System.Collections;

public class DemoScriptDamage : MonoBehaviour {

	private bool damageIsOn=false;
	private bool male=true;
	private bool female=false;
	private bool maleant;
	private bool femaleant;

	// Use this for initialization
	void Start () {
		male = BloodDamage.instance.GetSoundGender () == BloodDamage.Gender.male;
		female = !male;
	}

	void OnGUI() {
		if (BloodDamage.instance.startDamage)
			return;
		maleant = male;
		femaleant = female;
		male = GUI.Toggle(new Rect(200, 5, 150, 20), male, "Male");
		if(male)
			female=false;
		female = GUI.Toggle(new Rect(200, 35, 150, 20), female, "Female");
		if(female) 
			male=false;

		if(male && male!=maleant)
			BloodDamage.instance.SetSoundGender(BloodDamage.Gender.male);

		if(female && female!=femaleant)
			BloodDamage.instance.SetSoundGender(BloodDamage.Gender.female);


		if (GUI.Button (new Rect (5, 5, 180, 23), "Damage 0")) {
			BloodDamage.instance.doDamage(0);
		}
		if (GUI.Button (new Rect (5, 30, 180, 23), "Damage 1")) {
			BloodDamage.instance.doDamage(1);
		}
		if (GUI.Button (new Rect (5, 55, 180, 23), "Damage 2")) {
			BloodDamage.instance.doDamage(2);
		}
		if (GUI.Button (new Rect (5, 80, 180, 23), "Damage 3")) {
			BloodDamage.instance.doDamage(3);
		}
		if (GUI.Button (new Rect (5, 105, 180, 23), "Damage 4")) {
			BloodDamage.instance.doDamage(4);
		}
		if (GUI.Button (new Rect (5, 130, 180, 23), "Damage Up")) {
			BloodDamage.instance.doDirectionalDamage(BloodDamage.damageDirection.up);
		}
		if (GUI.Button (new Rect (5, 155, 180, 23), "Damage Down")) {
			BloodDamage.instance.doDirectionalDamage(BloodDamage.damageDirection.down);
		}
		if (GUI.Button (new Rect (5, 180, 180, 23), "Damage Left")) {
			BloodDamage.instance.doDirectionalDamage(BloodDamage.damageDirection.left);
		}
		if (GUI.Button (new Rect (5, 205, 180, 23), "Damage Right")) {
			BloodDamage.instance.doDirectionalDamage(BloodDamage.damageDirection.right);
		}

		if (GUI.Button (new Rect (Screen.width-190, 5, 180, 23), "Clear Fix Damage")) {
			BloodDamage.instance.ClearFixedDamage();
		}
		if (GUI.Button (new Rect (Screen.width-190, 30, 180, 23), "Fix Damage 0")) {
			BloodDamage.instance.SetFixedDamage(0);
		}
		if (GUI.Button (new Rect (Screen.width-190, 55, 180, 23), "Fix Damage 1")) {
			BloodDamage.instance.SetFixedDamage(1);
		}
		if (GUI.Button (new Rect (Screen.width-190, 80, 180, 23), "Fix Damage 2")) {
			BloodDamage.instance.SetFixedDamage(2);
		}
		if (GUI.Button (new Rect (Screen.width-190, 105, 180, 23), "Fix Damage 3")) {
			BloodDamage.instance.SetFixedDamage(3);
		}
		if (GUI.Button (new Rect (Screen.width-190, 130, 180, 23), "Fix Damage 4")) {
			BloodDamage.instance.SetFixedDamage(4);
		}
	}
}
