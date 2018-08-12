using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFileScript : MonoBehaviour {

	[SerializeField] private bool save;
    [SerializeField] private string file_name;
    private InnoData file;

	void Start () {
		if (save) {
            file = new InnoData();
            commands();
            InnoData.saveFile(file_name, file);
        }
        save = false;
	}

    private void commands() {
        file.addDialogue("", "Hi there!");
        file.addDialogue("", "I really hope this works!");
        file.addChoices("", "Is this good?", "Yes!", 1, "No...", 3);
        file.addDialogue("", "Oh Dang");
        file.addDialogue("", "Oh Dangg");
        file.addDialogue("", "Oh Danggg");
    }
	
}
