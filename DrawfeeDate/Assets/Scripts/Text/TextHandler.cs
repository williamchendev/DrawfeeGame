using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextHandler : MonoBehaviour {

	//Data
    [SerializeField] private string text_data;
    [SerializeField] private string id_data;
    [SerializeField] private string effect_data;
    private List<string> sentence_data;

    private EventManager em;

    //Settings
    [SerializeField] private float text_spd = 0.17f;
    [SerializeField] private float text_width = 15f;
    [SerializeField] private float text_third_width = 12f;

    //Variables
    private bool clicked;
    private bool is_choice;

    private bool text_active;
    private bool text_wait;
    private bool text_click;
    private float text_time;
    private int text_line;
    private int text_line_reg;
    private float sin_val;
    private float currentLerpTime;

    private TextMesh debug_mesh;
    private TextMesh letter_mesh;
    private TextMesh[] show_mesh;
    private TextSize[] text_size;

    private GameObject text_box;
    private GameObject text_arrow;

    //Init Event
	void Awake () {
        //Variables
        clicked = false;
        is_choice = false;

        text_active = true;
        text_wait = false;
        text_click = false;
        text_time = 0;
        text_line = 0;
        text_line_reg = 0;
        sin_val = 0;

        //Text Background
        text_box = new GameObject("Background");
        text_box.transform.parent = transform;
        text_box.transform.position = new Vector3(0f, -4f, transform.position.z + 0.1f);
        text_box.transform.localScale = new Vector3(2f, 2f, 1);
        text_box.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Text/sDebugTextbox");
        //Debug
        //text_box.GetComponent<SpriteRenderer>().enabled = false;

        //Text Meshes
        GameObject mesh_object = new GameObject("Debug Mesh");
        mesh_object.transform.parent = transform;
        mesh_object.transform.position = new Vector3(-7.2f, -1.78f, transform.position.z);
        mesh_object.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        debug_mesh = mesh_object.AddComponent<TextMesh>();
        debug_mesh.font = Resources.Load<Font>("Fonts/KaoriGel");
        debug_mesh.GetComponent<MeshRenderer>().material = Resources.Load<Font>("Fonts/KaoriGel").material;
        debug_mesh.fontSize = 72;
        debug_mesh.color = Color.white;

        text_size = new TextSize[4];
        text_size[3] = new TextSize(debug_mesh);
        show_mesh = new TextMesh[3];

        for (int i = 0; i < 3; i++) {
            mesh_object = new GameObject("Show Mesh " + i);
            mesh_object.transform.parent = transform;
            mesh_object.transform.position = new Vector3(-7.2f, -2.15f + (-i * 0.9f), transform.position.z);
            mesh_object.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            mesh_object.AddComponent<TextMovingScript>();
            show_mesh[i] = mesh_object.AddComponent<TextMesh>();
            show_mesh[i].font = Resources.Load<Font>("Fonts/KaoriGel");
            show_mesh[i].GetComponent<MeshRenderer>().material = Resources.Load<Font>("Fonts/KaoriGel").material;
            show_mesh[i].fontSize = 72;
            show_mesh[i].color = Color.white;
            text_size[i] = new TextSize(show_mesh[i]);
        }

        mesh_object = new GameObject("Letter Mesh");
        mesh_object.transform.parent = transform;
        mesh_object.transform.position = new Vector3(-7.2f, -1.78f, transform.position.z);
        mesh_object.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        letter_mesh = mesh_object.AddComponent<TextMesh>();
        letter_mesh.font = Resources.Load<Font>("Fonts/KaoriGel");
        letter_mesh.GetComponent<MeshRenderer>().material = Resources.Load<Font>("Fonts/KaoriGel").material;
        letter_mesh.fontSize = 72;
        letter_mesh.color = Color.white;

        letter_mesh.text = "";

        //Text Arrow
        text_arrow = new GameObject("Text Arrow");
        text_arrow.transform.parent = transform;
        text_arrow.transform.position = new Vector3(7f, -4.55f, transform.position.z);
        text_arrow.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        text_arrow.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Text/sDialogueArrow");
        text_arrow.GetComponent<SpriteRenderer>().color = Color.clear;
	}

    //Load Textbox
    void Start() {
        sentence_data = arrangeSentences(text_data);
    }

    //Update Event
    void Update () {
        //Input
        sin_val += 0.017f;
        if (sin_val > 1) {
            sin_val = 0;
        }
        float draw_sin = (Mathf.Sin(sin_val * 2 * Mathf.PI) * 2.0f) - 1;
        if (PlayerManager.instance.cursorClick) {
            clicked = true;
            if (is_choice) {
                if (!text_active) {
                    clicked = false;
                }
            }
        }

        //Text Box Management
        if (text_active) {
            if (text_wait) {
                if (!text_click) {
                    text_arrow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    text_arrow.transform.position = new Vector3(7f, -4.55f + (draw_sin * 0.05f), transform.position.z);

                    if (clicked) {
                        clicked = false;
                        text_click = true;
                        for (int k = 0; k < 3; k++){
                            show_mesh[k].GetComponent<TextMovingScript>().debugMove();
                        }
                    }
                }
                else {
                    text_arrow.GetComponent<SpriteRenderer>().color = Color.clear;

                    if (clicked) {
                        for (int k = 0; k < 3; k++){
                            show_mesh[k].GetComponent<TextMovingScript>().reset();
                        }
                        clicked = false;
                    }
                    bool resettrue = true;
                    for (int k = 0; k < 3; k++){
                        if (!show_mesh[k].GetComponent<TextMovingScript>().isReset) {
                            resettrue = false;
                        }
                    }
                    if (resettrue) {
                        text_wait = false;
                    }
                }
            }
            else {
                text_arrow.GetComponent<SpriteRenderer>().color = Color.clear;

                text_time += text_spd;
		        while (text_time > 1) {
                    if (show_mesh[text_line_reg].text.Length >= sentence_data[text_line].Length) {
                        text_line++;
                        text_line_reg++;
                        if (text_line >= sentence_data.Count) {
                            text_active = false;
                            text_click = false;
                            text_time = 0;
                            letter_mesh.text = "";
                            return;
                        }
                        if (text_line_reg >= 3) {
                            text_line_reg = 0;
                            text_wait = true;
                            text_click = false;
                            text_time = 0;
                            letter_mesh.text = "";
                            return;
                        }
                    }

                    if (clicked) {
                        clicked = false;
                        while (text_line_reg < 3 && text_line < sentence_data.Count) {
                            show_mesh[text_line_reg].text = sentence_data[text_line];
                            text_line_reg++;
                            text_line++;
                        }
                        text_line_reg--;
                        text_line--;
                        continue;
                    }
                    show_mesh[text_line_reg].text = sentence_data[text_line].Substring(0, Mathf.Clamp(show_mesh[text_line_reg].text.Length + 1, 0, sentence_data[text_line].Length));
                    if (sentence_data[text_line].Length > 0) {
                        letter_mesh.text = sentence_data[text_line].ToCharArray()[Mathf.Clamp(show_mesh[text_line_reg].text.Length, 0, Mathf.Max(0, sentence_data[text_line].Length - 1))].ToString();
                    }
                    text_time--;
                }
                letter_mesh.transform.position = new Vector3(text_size[text_line_reg].width + show_mesh[text_line_reg].transform.position.x, show_mesh[text_line_reg].transform.position.y + Mathf.SmoothStep(0.7f, -0.2f, text_time % 1.0f), transform.position.z);
            }
        }
        else {
            if (effect_data == "fade") {
                if (text_click) {
                    text_arrow.GetComponent<SpriteRenderer>().color = Color.clear;

                    //Lerp Text Box
                    currentLerpTime += Time.deltaTime;
                    if (currentLerpTime > 1) {
                        currentLerpTime = 1;
                    }
                    float t = currentLerpTime / 1;
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    text_box.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(text_box.GetComponent<SpriteRenderer>().color.a, 0, Time.deltaTime * 4f));
                    transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(transform.position.y, -5, t * 0.1f), transform.position.z);

                    if (clicked) {
                        for (int k = 0; k < 3; k++){
                            show_mesh[k].GetComponent<TextMovingScript>().reset();
                        }
                        clicked = false;
                    }
                    bool resettrue = true;
                    for (int k = 0; k < 3; k++){
                        if (!show_mesh[k].GetComponent<TextMovingScript>().isReset) {
                            resettrue = false;
                        }
                    }
                    if (resettrue) {
                        if (!is_choice) {
                            em.continueEvent();
                        }
                        Destroy(gameObject);
                    }
                }
                else {
                    text_arrow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    text_arrow.transform.position = new Vector3(7f, -4.55f + (draw_sin * 0.05f), transform.position.z);

                    if (clicked) {
                        text_click = true;
                        for (int k = 0; k < 3; k++){
                            show_mesh[k].GetComponent<TextMovingScript>().debugMove();
                        }
                        clicked = false;
                    }
                }
            }
            else {
                if (!is_choice) {
                    if (clicked) {
                        em.continueEvent();
                        Destroy(gameObject);
                    }
                }

                text_arrow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                text_arrow.transform.position = new Vector3(7f, -4.55f + (draw_sin * 0.05f), transform.position.z);
            }
        }

        if (is_choice) {
            text_arrow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
	}

    //Parsing Methods
    private List<string> arrangeSentences(string data) {
        List<string> sentences = new List<string>();
        if (data == null){
            sentences.Add("");
            return sentences;
        }
        data = data.Replace("\n", " #$# ");
        data = data.Replace(System.Environment.NewLine, " #$# ");
        RegexOptions options = RegexOptions.None;
        Regex regex = new Regex("[ ]{2,}", options);     
        data = regex.Replace(data, " ");
        string[] words = data.Split(' ');

        int sentence_num = 0;
        string sentence = "";
        for (int i = 0; i < words.Length; i++) {
            float text_use_width = text_width;
            if (sentence_num % 3 == 2) {
                text_use_width = text_third_width;
            }
            string temp_sentence = sentence + words[i] + " ";
            debug_mesh.text = temp_sentence;

            if (words[i] == "#$#") {
                sentences.Add(sentence);
                sentence = "";
                sentence_num++;
            }
            else if (text_size[3].width >= text_use_width) {
                sentences.Add(sentence);
                sentence = words[i] + " ";
                sentence_num++;
            }
            else {
                sentence = temp_sentence;
            }

            debug_mesh.text = "";
        }

        if (sentence != null && sentence.Trim().Length != 0){
            sentences.Add(sentence);
        }

        return sentences;
    }

    //Setter Methods
    public string text {
        set {
            text_data = value;
        }
    }

    public string id {
        set {
            id_data = value;
        }
    }

    public string effect {
        set {
            effect_data = value;
        }
    }

    public bool choice {
        set {
            is_choice = value;
        }
    }

    public EventManager event_manager {
        set {
            em = value;
        }
    }
}

public class TextMovingScript : MonoBehaviour {

    //Settings
    private TextMesh tx;

    //Variables
    private float alpha;
    private float y_pos;
    private float y_target;

    void Start() {
        tx = GetComponent<TextMesh>();

        alpha = 1;
        y_pos = transform.position.y;
        y_target = y_pos;
    }

    void Update() {
        tx.color = new Color(1f, 1f, 1f, alpha);

        if (alpha < 1) {
            alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * 3.5f);
            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(transform.position.y, y_target, Time.deltaTime * 4), transform.position.z);
            if (alpha <= 0.01f) {
                tx.text = "";
                y_target = y_pos;
                transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);
                alpha = 1;
            }
        }
    }

    public void debugMove() {
        alpha = 0.99f;
        y_target = -5f;
    }

    public void reset() {
        tx.text = "";
        y_target = y_pos;
        transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);
        alpha = 1;
    }

    public bool isReset {
        get {
            if (alpha >= 1) {
                return true;
            }
            return false;
        }
    }

}