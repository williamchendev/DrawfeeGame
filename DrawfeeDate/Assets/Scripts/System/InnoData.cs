using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class InnoData {

    //Data
    [SerializeField] private List<int> int_data;
    [SerializeField] private List<float> float_data;
    [SerializeField] private List<Vector3> vector_data;
    [SerializeField] private List<string> string_data;

    //Constructor
	public InnoData() {
        int_data = new List<int>();
        float_data = new List<float>();
        vector_data = new List<Vector3>();
        string_data = new List<string>();
    }

    //Read Data
    public List<ArrayList> readData() {
        List<ArrayList> data = new List<ArrayList>();
        int i = 0;
        while (i < int_data.Count) {
            ArrayList array = new ArrayList();
            array.Add(int_data[i]);

            if (int_data[i] == 0) {
                //End
                i++;
            }
            else if (int_data[i] == 1) {
                //Dialogue
                array.Add(string_data[int_data[i + 1]]); //ID
                array.Add(string_data[int_data[i + 2]]); //Text
                array.Add(string_data[int_data[i + 3]]); //Effect
                i += 4;
            }
            else if (int_data[i] == 2) {
                //Choices
                array.Add(int_data[i + 1]); //Text Boxes
                array.Add(string_data[int_data[i + 2]]); //ID
                array.Add(string_data[int_data[i + 3]]); //Choice Text

                array.Add(string_data[int_data[i + 4]]); //ChoiceA
                array.Add(string_data[int_data[i + 5]]); //ChoiceB
                array.Add(string_data[int_data[i + 6]]); //ChoiceC
                array.Add(string_data[int_data[i + 7]]); //ChoiceD

                array.Add(int_data[i + 8]); //SkipA
                array.Add(int_data[i + 9]); //SkipB
                array.Add(int_data[i + 10]); //SkipC
                array.Add(int_data[i + 11]); //SkipD

                i += 12;
            }
            else if (int_data[i] == 3) {
                //Animation
                array.Add(string_data[int_data[i + 1]]); //ID
                array.Add(string_data[int_data[i + 2]]); //Animation
                i += 3;
            }
            else if (int_data[i] == 4) {
                //Movement
                array.Add(string_data[int_data[i + 1]]); //ID
                array.Add(string_data[int_data[i + 2]]); //Effect
                array.Add(int_data[i + 3]); //Variable (Bool)
                array.Add(vector_data[int_data[i + 4]]); //Variable
                i += 5;
            }
            else if (int_data[i] == 5) {
                //Character
                array.Add(string_data[int_data[i + 1]]); //ID
                array.Add(string_data[int_data[i + 2]]); //Effect
                array.Add(vector_data[int_data[i + 3]]); //Position
                i += 4;
            }
            else if (int_data[i] == 6) {
                //Music & SFX
                array.Add(int_data[i + 1]); //Is Music (Bool)
                array.Add(string_data[int_data[i + 2]]); //Music or SFX
                i += 3;
            }
            else if (int_data[i] == 7) {
                //Transition
                array.Add(string_data[int_data[i + 1]]); //Transition
                i += 2;
            }
            else if (int_data[i] == 8) {
                //Background
                array.Add(string_data[int_data[i + 1]]); //Background
                i += 2;
            }
            else if (int_data[i] == 9) {
                //File Load
                array.Add(string_data[int_data[i + 1]]); //File Load
                i += 2;
            }
            else if (int_data[i] == 10) {
                //Key
                array.Add(int_data[i + 1]); //Key Num
                array.Add(int_data[i + 2]); //Keybool (bool)
                i += 3;
            }
            else if (int_data[i] == 11) {
                //Value
                array.Add(string_data[int_data[i + 1]]); //Value ID
                array.Add(float_data[int_data[i + 2]]); //Value Change
                i += 3;
            }
            else if (int_data[i] == 12) {
                //Skip
                array.Add(int_data[i + 1]); //Skip size
                i += 2;
            }
            else if (int_data[i] == 13) {
                //Skip (Key Condition)
                array.Add(int_data[i + 1]); //Skip size
                array.Add(int_data[i + 2]); //Key Num
                array.Add(int_data[i + 3]); //Key Condition (bool)
                i += 4;
            }
            else if (int_data[i] == 14) {
                //Skip (Value Condition)
                array.Add(int_data[i + 1]); //Skip size
                array.Add(string_data[int_data[i + 2]]); //Value ID
                array.Add(float_data[int_data[i + 3]]); //Value
                array.Add(int_data[i + 4]); //Above value? (bool)
                i += 5;
            }
            else if (int_data[i] == 15) {
                //Wait
                array.Add(float_data[int_data[i + 1]]); //Seconds
                i += 2;
            }

            data.Add(array);
        }
        return data;
    }

    //Entry Methods
    public void addEnd() {
        int_data.Add(0);
    }

    public void addDialogue(string id, string text) {
        int_data.Add(1);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Text
        string_data.Add(text);

        int_data.Add(string_data.Count);  //Effect
        string effect = null;
        string_data.Add(effect);
    }

    public void addDialogue(string id, string text, string effect) {
        int_data.Add(1);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Text
        string_data.Add(text);

        int_data.Add(string_data.Count);  //Effect
        string_data.Add(effect);
    }

    public void addChoices(string id, string text, string choiceA, int skipA, string choiceB, int skipB) {
        int_data.Add(2);

        int_data.Add(2); //Number of text boxes

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);
        int_data.Add(string_data.Count);  //Choice Text
        string_data.Add(text);

        int_data.Add(string_data.Count);  //Choice Option A
        string_data.Add(choiceA);

        int_data.Add(string_data.Count);  //Choice Option B
        string_data.Add(choiceB);

        int_data.Add(string_data.Count);  //Choice Option C
        string_data.Add("");

        int_data.Add(string_data.Count);  //Choice Option D
        string_data.Add("");

        int_data.Add(skipA);  //Skips A through D
        int_data.Add(skipB);
        int_data.Add(-1);
        int_data.Add(-1);
    }

    public void addChoices(string id, string text, string choiceA, int skipA, string choiceB, int skipB, string choiceC, int skipC) {
        int_data.Add(2);

        int_data.Add(3); //Number of text boxes

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);
        int_data.Add(string_data.Count);  //Choice Text
        string_data.Add(text);

        int_data.Add(string_data.Count);  //Choice Option A
        string_data.Add(choiceA);

        int_data.Add(string_data.Count);  //Choice Option B
        string_data.Add(choiceB);

        int_data.Add(string_data.Count);  //Choice Option C
        string_data.Add(choiceC);

        int_data.Add(string_data.Count);  //Choice Option D
        string_data.Add("");

        int_data.Add(skipA);  //Skips A through D
        int_data.Add(skipB);
        int_data.Add(skipC);
        int_data.Add(-1);
    }

    public void addChoices(string id, string text, string choiceA, int skipA, string choiceB, int skipB, string choiceC, int skipC, string choiceD, int skipD) {
        int_data.Add(2);

        int_data.Add(4); //Number of text boxes

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);
        int_data.Add(string_data.Count);  //Choice Text
        string_data.Add(text);

        int_data.Add(string_data.Count);  //Choice Option A
        string_data.Add(choiceA);

        int_data.Add(string_data.Count);  //Choice Option B
        string_data.Add(choiceB);

        int_data.Add(string_data.Count);  //Choice Option C
        string_data.Add(choiceC);

        int_data.Add(string_data.Count);  //Choice Option D
        string_data.Add(choiceD);

        int_data.Add(skipA);  //Skips A through D
        int_data.Add(skipB);
        int_data.Add(skipC);
        int_data.Add(skipD);
    }

    public void addAnimation(string id, string animation) {
        int_data.Add(3);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Animation
        string_data.Add(animation);
    }

    public void addMovement(string id, string effect) {
        int_data.Add(4);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Effect
        string_data.Add(effect);

        int_data.Add(-1);  //No Variable

        int_data.Add(vector_data.Count);  //Variable
        vector_data.Add(Vector3.zero);
    }

    public void addMovement(string id, string effect, Vector3 variable) {
        int_data.Add(4);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Effect
        string_data.Add(effect);

        int_data.Add(1);  //Yes Variable

        int_data.Add(vector_data.Count);  //Variable
        vector_data.Add(variable);
    }

    public void addCharacter(string id, string effect) {
        int_data.Add(5);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Effect
        string_data.Add(effect);

        int_data.Add(vector_data.Count);  //Position
        vector_data.Add(Vector3.zero);
    }

    public void addCharacter(string id, string effect, Vector3 position) {
        int_data.Add(5);

        int_data.Add(string_data.Count);  //ID
        string_data.Add(id);

        int_data.Add(string_data.Count);  //Effect
        string_data.Add(effect);

        int_data.Add(vector_data.Count);  //Position
        vector_data.Add(position);
    }

    public void addMusic(string music) {
        int_data.Add(6);

        int_data.Add(1);  //Is Music

        int_data.Add(string_data.Count);  //Music Name
        string_data.Add(music);
    }

    public void addSFX(string sfx) {
        int_data.Add(6);

        int_data.Add(-1);  //Is not Music

        int_data.Add(string_data.Count);  //SFX Name
        string_data.Add(sfx);
    }

    public void addTransition(string transition) {
        int_data.Add(7);

        int_data.Add(string_data.Count);  //Transition
        string_data.Add(transition);
    }

    public void addBackground(string background) {
        int_data.Add(8);

        int_data.Add(string_data.Count);  //Background
        string_data.Add(background);
    }

    public void addFile(string filename) {
        int_data.Add(9);

        int_data.Add(string_data.Count);  //File Name
        string_data.Add(filename);
    }

    public void addKey(int key) {
        int_data.Add(10);

        int_data.Add(key);  //Key
        int_data.Add(1);  //Key Value
    }

    public void addKey(int key, bool keybool) {
        int_data.Add(10);

        int_data.Add(key);  //Key
        
        //Key Value
        if (keybool) {
            int_data.Add(1);
        }
        else {
            int_data.Add(-1);
        }
    }

    public void addValue(string value_id, float change) {
        int_data.Add(11);

        int_data.Add(string_data.Count);  //Value ID
        string_data.Add(value_id);

        int_data.Add(float_data.Count);  //Change in Value
        float_data.Add(change);
    }

    public void addSkip(int skip) {
        int_data.Add(12);

        int_data.Add(skip);  //Skip size
    }

    public void addSkip(int skip, int key, bool key_condition) {
        int_data.Add(13);

        int_data.Add(skip);  //Skip size

        int_data.Add(key);  //Key

        //Key Condition
        if (key_condition) {
            int_data.Add(1);
        }
        else {
            int_data.Add(-1);
        }
    }

    public void addSkip(int skip, string value_id, float value, bool above) {
        int_data.Add(14);

        int_data.Add(skip);  //Skip size

        int_data.Add(string_data.Count);  //Value ID
        string_data.Add(value_id);

        int_data.Add(float_data.Count);  //Value
        float_data.Add(value);

        //Above Percent?
        if (above) {
            int_data.Add(1);
        }
        else {
            int_data.Add(-1);
        }
    }

    public void addWait(float seconds) {
        int_data.Add(15);

        int_data.Add(float_data.Count);  //Seconds
        float_data.Add(seconds);
    }

    //Save & Load File Functionality
    public static void saveFile(string file_name, InnoData file) {
        string json_save = JsonUtility.ToJson(file, true);
        string path = Application.streamingAssetsPath + "/Events/" + file_name + ".json";
        if (!File.Exists(path)){
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/Events/");
        }
        File.WriteAllText (path, json_save);
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    public static InnoData loadFile(string file_name) {
        string path = Application.streamingAssetsPath + file_name + ".json";
        if (File.Exists(path)){
            string json_save = File.ReadAllText(path);
            return JsonUtility.FromJson<InnoData>(json_save);
        }
        else {
            Debug.LogError("File \"" + file_name + "\" not found");
            return new InnoData();
        }
    }

}
