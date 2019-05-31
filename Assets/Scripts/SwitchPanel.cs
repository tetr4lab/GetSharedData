using UnityEngine;
using UnityEngine.UI;
using Nam = Cns.Nam;

public class SwitchPanel : MonoBehaviour {

	[SerializeField] private Text TextPanel = null;
	[SerializeField] private ToggleGroup togleGroup = null;

	private void Start () {
		if (TextPanel != null) {
			foreach (var toggle in togleGroup.GetComponentsInChildren<Toggle> ()) {
				var l = int.Parse (toggle.name);
				toggle.isOn = (l == Cns.Locale);
				toggle.GetComponentInChildren<Text> ().text = Cns.S (l, Nam.Culture);
			}
		}
		Debug.Log (togleGroup);
	}

	public void OnChange (Toggle toggle) {
		if (TextPanel != null) {
			Cns.Locale = int.Parse (toggle.name);
			TextPanel.text = $"{Cns.S (Nam.Welcome)}\nTest = {Cns.Test}\n\nLanguage: {Cns.S (Nam.Language)}";
			Debug.Log ($"{toggle.name} {Cns.Locale} {TextPanel.text}");
		}
	}

}
