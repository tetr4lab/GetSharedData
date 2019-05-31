using System.Collections;
using System.Collections.Generic;

public static partial class Cns {
	// 名前
	public enum Nam {
		None = 0,
		Culture,
		Language,
		Welcome,
	}

	// 名前に対応する文字列
	private static readonly Dictionary<Nam, string>[] str = {
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊 (キー変更不可))
			{ Nam.Culture,	"English" },	// 内部言語名 (特殊 (キー変更不可))
			{ Nam.Language,	"English" },	// 外部言語名 (特殊 (キー変更不可))
			{ Nam.Welcome,	"Hi Torachin, Welcome to the fate of the cave." },	// 記入例 ()
		},
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊 (キー変更不可))
			{ Nam.Culture,	"Japanese" },	// 内部言語名 (特殊 (キー変更不可))
			{ Nam.Language,	"日本語" },	// 外部言語名 (特殊 (キー変更不可))
			{ Nam.Welcome,	"やあトラちん、運命の洞窟へようこそ。" },	// 記入例 ()
		},
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊 (キー変更不可))
			{ Nam.Culture,	"Chinese" },	// 内部言語名 (特殊 (キー変更不可))
			{ Nam.Language,	"中文" },	// 外部言語名 (特殊 (キー変更不可))
			{ Nam.Welcome,	"嗨Torachin，欢迎来到洞穴的命运。" },	// 記入例 ()
		},
	};
}
