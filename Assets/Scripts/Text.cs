using System.Collections;
using System.Collections.Generic;

public static partial class Cns {
	// 名前
	public enum Nam {
		None = 0,
		Culture,
		Language,
	}

	// 名前に対応する文字列
	private static readonly Dictionary<Nam, string>[] str = {
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊)
			{ Nam.Culture,	"English" },	// 内部言語名 (特殊)
			{ Nam.Language,	"English" },	// 外部言語名 (特殊)
		},
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊)
			{ Nam.Culture,	"Japanese" },	// 内部言語名 (特殊)
			{ Nam.Language,	"日本語" },	// 外部言語名 (特殊)
		},
		new Dictionary<Nam, string> {
			{ Nam.None,	"" },	// なし (特殊)
			{ Nam.Culture,	"Chinese" },	// 内部言語名 (特殊)
			{ Nam.Language,	"中文" },	// 外部言語名 (特殊)
		},
	};
}
