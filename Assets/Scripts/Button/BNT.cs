using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BNT : MonoBehaviour {

    public GameObject cube;
	public Button yourButton;

	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
        cube.SetActive(!cube.activeSelf);
	}

}
