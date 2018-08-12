using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //Data
    private List<ArrayList> event_data;

    //Settings
    public bool event_active {get; private set;}
    private int event_num;

    //Variables
    private bool text_active;

	//Init Event
	void Start () {
        //Load first scene
        event_data = new List<ArrayList>();

        //Settings
        event_active = false;
        text_active = false;
		event_num = 0;
	}
	
    //Event Handling
    private void handleEvent(ArrayList event_command) {
        int event_type = (int) event_command[0];

        if (event_type == 1) {
            //Dialogue
            string id = (string) event_command[1];
            string text = (string) event_command[2];
            string effect = (string) event_command[3];

            //Text box
            TextHandler textbox = new GameObject("Textbox").AddComponent<TextHandler>();
            textbox.text = text;
            textbox.id = id;
            textbox.effect = effect;
            textbox.event_manager = gameObject.GetComponent<EventManager>();

            text_active = true;
        }
        else if (event_type == 2) {
            //Choices
            int choice_num = (int) event_command[1];
            string id = (string) event_command[2];
            string choice_text = (string) event_command[3];

            string[] choices = {(string) event_command[4], (string) event_command[5], (string) event_command[6], (string) event_command[7]};
            int[] skips = {(int) event_command[8], (int) event_command[9], (int) event_command[10], (int) event_command[11]};

            //Choices + Textbox
            ChoiceHandler choicebox = new GameObject("Choice").AddComponent<ChoiceHandler>();
            choicebox.setData(choice_num, choice_text, choices, skips);
            choicebox.id = id;
            choicebox.event_manager = gameObject.GetComponent<EventManager>();

            text_active = true;
        }
        else if (event_type == 3) {
            //Animation
            string id = (string) event_command[1];
            string animation = (string) event_command[2];
        }
        else if (event_type == 4) {
            //Movement
            string id = (string) event_command[1];
            string effect = (string) event_command[2];
            bool use_variable = intToBool((int) event_command[3]);
            Vector3 variable = (Vector3) event_command[4];
        }
        else if (event_type == 5) {
            //Character
            string id = (string) event_command[1];
            string effect = (string) event_command[2];
            Vector3 position = (Vector3) event_command[3];
        }
        else if (event_type == 6) {
            //Music & SFX
            bool is_music = intToBool((int) event_command[1]);
            string song_name = (string) event_command[2];
        }
        else if (event_type == 7) {
            //Transition
            string transition_name = (string) event_command[1];
        }
        else if (event_type == 8) {
            //Background
            string background_name = (string) event_command[1];
        }
        else if (event_type == 9) {
            //File Load
            string file_name = (string) event_command[1];
        }
        else if (event_type == 10) {
            //Key
            int key_num = (int) event_command[1];
            bool key_bool = intToBool((int) event_command[2]);
        }
        else if (event_type == 11) {
            //Value
            string value_id = (string) event_command[1];
            float value_change = (float) event_command[2];
        }
        else if (event_type == 12) {
            //Simple Skip
            int skip_size = (int) event_command[1];
            setSkip(skip_size);
        }
        else if (event_type == 13) {
            //Key conditional Skip
            int skip_size = (int) event_command[1];
            int key_num = (int) event_command[2];
            bool key_condition = intToBool((int) event_command[3]);
        }
        else if (event_type == 14) {
            //Value conditional Skip
            int skip_size = (int) event_command[1];
            string value_id = (string) event_command[2];
            float value = (float) event_command[3];
            bool above = intToBool((int) event_command[4]);
        }
        else if (event_type == 15) {
            //Wait
            float seconds = (float) event_command[1];
        }
    }

    //Event Methods
    public void startEvent(string file_path) {
        readFile(file_path);
        if (event_data.Count > 0) {
            event_active = true;
            text_active = false;
            event_num = 0;

            PlayerManager.instance.canMove = false;
            loopEvent();
        }
    }

    public void continueEvent() {
        text_active = false;

        loopEvent();
    }

    private void loopEvent() {
        if (canHandle) {
            handleEvent(event_data[event_num]);
            event_num++;
            loopEvent();
        }
        else if (event_num >= event_data.Count) {
            endEvent();
        }
    }

    private void endEvent() {
        PlayerManager.instance.canMove = true;
        event_active = false;
    }

    public void setSkip(int skip) {
        event_num += skip;
    }

    //Misc Methods
    private bool intToBool(int integer) {
        if (integer < 0) {
            return false;
        }
        else {
            return true;
        }
    }

    //Getter & Setter Methods
    private bool canHandle {
        get {
            if (event_num < event_data.Count) {
                if (!text_active) {
                    if (event_active) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    //File IO
    public void readFile(string file_path) {
        event_data = InnoData.loadFile(file_path).readData();
    }

}
