using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using Nam = Cns.Nam;

public static partial class Cns {

	// 正引き
	public static string S (params Nam [] nams) {
		return string.Join ("", Array.ConvertAll (nams, (nam) => Cns.str [Cns.locale] [nam]));
	}
	public static string S (int locale, params Nam [] nams) {
		return string.Join ("", Array.ConvertAll (nams, (nam) => Cns.str [locale] [nam]));
	}

	// 逆引き (ロケール)
	public static int L (string str, Nam nam = Nam.Culture) {
		for (var l = 0; l < Cns.str.Length; l++) {
			if (str.StartsWith (Cns.str [l] [nam])) {
				return l;
			}
		}
		return -1;      // 見つからない
	}

	// 逆引き (名前)
	public static Nam N (string str, int lcale = -1) {
		if (locale < 0) { lcale = Cns.locale; }
		foreach (var n in Cns.str [locale].Keys) {
			if (str.StartsWith (Cns.str [locale] [n])) {
				return n;
			}
		}
		return Nam.None;    // 見つからない
	}

	// 言語英名
	public static string Language {
		get { return (Cns.select < 0) ? Application.systemLanguage.ToString () : S (Nam.Culture); }
	}

	// 設定・取得
	public static int Locale {
		get { return Cns.locale; }
		set {
			Cns.select = value;
			if (value < 0) {            // システム設定に従う
				value = Mathf.Max (0, L (Application.systemLanguage.ToString ()));
			}
			Cns.locale = value;
			Debug.LogFormat ("locale: {0}", Cns.S (Nam.Language));
		}
	}

	// ロケール
	private static int locale = 0;      // 結果
	private static int select = -1;     // 選択

}
