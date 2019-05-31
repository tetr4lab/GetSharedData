using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class GetSharedData : EditorWindow {

	#region Static

	static string Application;
	static string Keyword;
	static string Document;
	static string ScriptPath;

	const string ApplicationPrefKey = "GetSharedData/Application";
	const string KeywordPrefKey = "GetSharedData/Keyword";
	static string DocumentPrefKey { get { return $"{PlayerSettings.companyName}/{PlayerSettings.productName}/Document"; } }
	static string ScriptPathPrefKey { get { return $"{PlayerSettings.companyName}/{PlayerSettings.productName}/ScriptPath"; } }
	const string WindowIconPath = "Assets/GetSharedData/Textures/SpreadsheetFavicon3.png";

	[MenuItem ("Tools/GetSharedData %&g")]
	private static void onMenuSelect () {
		if (ScriptPath [ScriptPath.Length - 1] != '/') {
			ScriptPath = $"{ScriptPath}/";
		}
		GetSharedData.getWebtext ();
	}

	[MenuItem ("Window/GetSharedData")]
	private static void Open () {
		var window = GetWindow<GetSharedData> ();
		var icon = AssetDatabase.LoadAssetAtPath<Texture> (WindowIconPath);
		window.titleContent = new GUIContent ("GetSharedData", icon);
		Debug.Log ($"{PlayerSettings.companyName}/{PlayerSettings.productName}");
	}

	private static void getWebtext () {
		getFile ($"{Application}?k={Keyword}&d={Document}&s=テキスト&a=A2", $"{ScriptPath}Text.cs");
		getFile ($"{Application}?k={Keyword}&d={Document}&s=数値&a=A2", $"{ScriptPath}Number.cs");
	}

	// URLをファイルとして取得する
	private static void getFile (string url, string filePath) {
		using (UnityWebRequest www = UnityWebRequest.Get (url)) {
			// StartCoroutine () の代用
			IEnumerator getText = GetText (www, filePath);
			if (getText != null) {
				while (getText.MoveNext ()) { }
			}
		}
	}

	// 取得と生成
	private static IEnumerator GetText(UnityWebRequest www, string filePath) {
		www.SendWebRequest ();	// リクエスト
		while (!www.isDone) {
			yield return null;	// 完了を待つ
		}
		if (www.isNetworkError || www.isHttpError) {
			Debug.LogError (www.error);
		} else {
			var text = www.downloadHandler.text;
			if (text.Contains ("<title>エラー</title>")) {
				Debug.LogError ($"Error: {text}");
			} else { // エラーがなければファイルに書き出す
				Debug.Log ($"Write '{filePath}' with …\n{text}");
				File.WriteAllText (filePath, text);
				AssetDatabase.Refresh ();   // アセットを更新
			}
		}
	}

	#endregion

	#region EventHandler

	private void OnEnable () {
		Application = EditorPrefs.GetString (ApplicationPrefKey, "https://script.google.com/macros/s/[APP ID]/exec");
		Keyword = EditorPrefs.GetString (KeywordPrefKey);
		Document = EditorPrefs.GetString (DocumentPrefKey);
		ScriptPath = EditorPrefs.GetString (ScriptPathPrefKey, "Assets/Scripts/");
	}

	private void OnDisable () {
		EditorPrefs.SetString (ApplicationPrefKey, Application);
		EditorPrefs.SetString (KeywordPrefKey, Keyword);
		EditorPrefs.SetString (DocumentPrefKey, Document);
		EditorPrefs.SetString (ScriptPathPrefKey, ScriptPath);
	}

	private void OnGUI () {
		GUILayout.Label ("Settings");
		Application = EditorGUILayout.TextField (new GUIContent ("Application URL*"), Application);
		Keyword = EditorGUILayout.TextField (new GUIContent ("Access Key*"), Keyword);
		Document = EditorGUILayout.TextField (new GUIContent ("Document ID"), Document);
		ScriptPath = EditorGUILayout.TextField (new GUIContent ("Assets/Scripts/ Folder"), ScriptPath);
		GUILayout.Space (10f);
		EditorGUI.BeginDisabledGroup (EditorApplication.isCompiling);
		if (GUILayout.Button ("Get Shared Data")) {
			onMenuSelect ();
		}
		EditorGUI.EndDisabledGroup ();
	}

	#endregion

}
