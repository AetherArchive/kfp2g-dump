using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	// Token: 0x02000218 RID: 536
	public class SceneBattle_GUI
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x001930E1 File Offset: 0x001912E1
		public GameObject PlayerSkill
		{
			get
			{
				return this._playerSkill;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x001930E9 File Offset: 0x001912E9
		public AEImage PlayerSkillAE
		{
			get
			{
				return this._playerSkillAE;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x001930F1 File Offset: 0x001912F1
		public PguiButtonCtrl BtnPlayerSkill
		{
			get
			{
				return this._btnPlayerSkill;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x001930F9 File Offset: 0x001912F9
		public PguiReplaceSpriteCtrl IconPlayerSkill
		{
			get
			{
				return this._iconPlayerSkill;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x00193101 File Offset: 0x00191301
		public PguiTextCtrl RemPlayerSkill
		{
			get
			{
				return this._remPlayerSkill;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x00193109 File Offset: 0x00191309
		public GameObject PlayerSkillBan
		{
			get
			{
				return this._playerSkillBan;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x00193111 File Offset: 0x00191311
		public GameObject TacticsSkill
		{
			get
			{
				return this._tacticsSkill;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x00193119 File Offset: 0x00191319
		public PguiAECtrl TacticsSkillAE
		{
			get
			{
				return this._tacticsSkillAE;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x00193121 File Offset: 0x00191321
		public PguiReplaceSpriteCtrl IconTacticsSkill
		{
			get
			{
				return this._iconTacticsSkill;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x00193129 File Offset: 0x00191329
		public GameObject DeckAll
		{
			get
			{
				return this._deckAll;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x00193131 File Offset: 0x00191331
		public List<GameObject> Chara
		{
			get
			{
				return this._chara;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00193139 File Offset: 0x00191339
		public List<Transform> TouchChara
		{
			get
			{
				return this._touchChara;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x00193141 File Offset: 0x00191341
		public List<Transform> TouchWait
		{
			get
			{
				return this._touchWait;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x00193149 File Offset: 0x00191349
		public List<PguiAECtrl> CharaSel
		{
			get
			{
				return this._charaSel;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x00193151 File Offset: 0x00191351
		public List<AEImage> EffArtsOk
		{
			get
			{
				return this._effArtsOk;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x00193159 File Offset: 0x00191359
		public List<AEImage> EffArtsOkDbl
		{
			get
			{
				return this._effArtsOkDbl;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x00193161 File Offset: 0x00191361
		public List<Transform> TouchArts
		{
			get
			{
				return this._touchArts;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x00193169 File Offset: 0x00191369
		public AEImage DeckAllAE
		{
			get
			{
				return this._deckAllAE;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x00193171 File Offset: 0x00191371
		public GameObject ActInfoAll
		{
			get
			{
				return this._actInfoAll;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x00193179 File Offset: 0x00191379
		public GameObject ActGageBase
		{
			get
			{
				return this._actGageBase;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x00193181 File Offset: 0x00191381
		public PguiImageCtrl ActGageGage
		{
			get
			{
				return this._actGageGage;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x00193189 File Offset: 0x00191389
		public PguiImageCtrl ActGageGageAdd
		{
			get
			{
				return this._actGageGageAdd;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x00193191 File Offset: 0x00191391
		public PguiImageCtrl ActGageGageAdd2
		{
			get
			{
				return this._actGageGageAdd2;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06002266 RID: 8806 RVA: 0x00193199 File Offset: 0x00191399
		public GameObject ActGageGageFlash
		{
			get
			{
				return this._actGageGageFlash;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x001931A1 File Offset: 0x001913A1
		public PguiTextCtrl NumActBefore
		{
			get
			{
				return this._numActBefore;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x001931A9 File Offset: 0x001913A9
		public PguiTextCtrl NumActAfter
		{
			get
			{
				return this._numActAfter;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x001931B1 File Offset: 0x001913B1
		public Transform TouchActGage
		{
			get
			{
				return this._touchActGage;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x001931B9 File Offset: 0x001913B9
		public CanvasGroup ActInfo
		{
			get
			{
				return this._actInfo;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x001931C1 File Offset: 0x001913C1
		public PguiTextCtrl NumUseBefore
		{
			get
			{
				return this._numUseBefore;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x001931C9 File Offset: 0x001913C9
		public PguiTextCtrl NumUseAfter
		{
			get
			{
				return this._numUseAfter;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x001931D1 File Offset: 0x001913D1
		public AEImage ActGageAE
		{
			get
			{
				return this._actGageAE;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x001931D9 File Offset: 0x001913D9
		public PguiTextCtrl ActGageExtra
		{
			get
			{
				return this._actGageExtra;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x001931E1 File Offset: 0x001913E1
		public AEImage ActInfoBackAE
		{
			get
			{
				return this._actInfoBackAE;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x001931E9 File Offset: 0x001913E9
		public AEImage ActInfoFrontAE
		{
			get
			{
				return this._actInfoFrontAE;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x001931F1 File Offset: 0x001913F1
		public GameObject ActGageBan
		{
			get
			{
				return this._actGageBan;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x001931F9 File Offset: 0x001913F9
		public GameObject TimeAll
		{
			get
			{
				return this._timeAll;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x00193201 File Offset: 0x00191401
		public GameObject MenuAll
		{
			get
			{
				return this._menuAll;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x00193209 File Offset: 0x00191409
		public PguiToggleButtonCtrl BtnAuto
		{
			get
			{
				return this._btnAuto;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x00193211 File Offset: 0x00191411
		public GameObject BtnAutoLock
		{
			get
			{
				return this._btnAutoLock;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x00193219 File Offset: 0x00191419
		public PguiTextCtrl BtnAutoTxt
		{
			get
			{
				return this._btnAutoTxt;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06002277 RID: 8823 RVA: 0x00193221 File Offset: 0x00191421
		public SimpleAnimation AnimAutoInfo
		{
			get
			{
				return this._animAutoInfo;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x00193229 File Offset: 0x00191429
		public GameObject BtnAutoBan
		{
			get
			{
				return this._btnAutoBan;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06002279 RID: 8825 RVA: 0x00193231 File Offset: 0x00191431
		public GameObject BtnAutoLamp
		{
			get
			{
				return this._btnAutoLamp;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x00193239 File Offset: 0x00191439
		public PguiToggleButtonCtrl BtnFast
		{
			get
			{
				return this._btnFast;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600227B RID: 8827 RVA: 0x00193241 File Offset: 0x00191441
		public PguiButtonCtrl BtnMenu
		{
			get
			{
				return this._btnMenu;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x00193249 File Offset: 0x00191449
		public PguiTextCtrl NumYaseiTotal
		{
			get
			{
				return this._numYaseiTotal;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x00193251 File Offset: 0x00191451
		public GameObject PlasmShare
		{
			get
			{
				return this._plasmShare;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x00193259 File Offset: 0x00191459
		public PguiAECtrl PlasmShareCmnAE
		{
			get
			{
				return this._plasmShareCmnAE;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x00193261 File Offset: 0x00191461
		public List<AEImage> PlasmShareAE
		{
			get
			{
				return this._plasmShareAE;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x00193269 File Offset: 0x00191469
		public PguiTextCtrl numWave
		{
			get
			{
				return this._numWave;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x00193271 File Offset: 0x00191471
		public PguiTextCtrl numTurn
		{
			get
			{
				return this._numTurn;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x00193279 File Offset: 0x00191479
		public GameObject SelectCard
		{
			get
			{
				return this._selectCard;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x00193281 File Offset: 0x00191481
		public SimpleAnimation AnimSelectCard
		{
			get
			{
				return this._animSelectCard;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x00193289 File Offset: 0x00191489
		public GameObject ActionCard
		{
			get
			{
				return this._actionCard;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x00193291 File Offset: 0x00191491
		public SimpleAnimation AnimActionCard
		{
			get
			{
				return this._animActionCard;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00193299 File Offset: 0x00191499
		public GameObject AnimTurn
		{
			get
			{
				return this._animTurn;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x001932A1 File Offset: 0x001914A1
		public AEImage AnimTurnAE
		{
			get
			{
				return this._animTurnAE;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x001932A9 File Offset: 0x001914A9
		public PguiReplaceSpriteCtrl AnimNumTurn1
		{
			get
			{
				return this._animNumTurn1;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x001932B1 File Offset: 0x001914B1
		public PguiReplaceSpriteCtrl AnimNumTurn2
		{
			get
			{
				return this._animNumTurn2;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600228A RID: 8842 RVA: 0x001932B9 File Offset: 0x001914B9
		public GameObject AnimTurnAct
		{
			get
			{
				return this._animTurnAct;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x001932C1 File Offset: 0x001914C1
		public AEImage AnimTurnActAE
		{
			get
			{
				return this._animTurnActAE;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x001932C9 File Offset: 0x001914C9
		public PguiTextCtrl AnimNumTurnAct
		{
			get
			{
				return this._animNumTurnAct;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x001932D1 File Offset: 0x001914D1
		public GameObject AnimWave
		{
			get
			{
				return this._animWave;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x001932D9 File Offset: 0x001914D9
		public AEImage AnimWaveAE
		{
			get
			{
				return this._animWaveAE;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x001932E1 File Offset: 0x001914E1
		public PguiReplaceSpriteCtrl AnimNumWaveC1
		{
			get
			{
				return this._animNumWaveC1;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x001932E9 File Offset: 0x001914E9
		public PguiReplaceSpriteCtrl AnimNumWaveC2
		{
			get
			{
				return this._animNumWaveC2;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x001932F1 File Offset: 0x001914F1
		public PguiReplaceSpriteCtrl AnimNumWaveM1
		{
			get
			{
				return this._animNumWaveM1;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x001932F9 File Offset: 0x001914F9
		public PguiReplaceSpriteCtrl AnimNumWaveM2
		{
			get
			{
				return this._animNumWaveM2;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x00193301 File Offset: 0x00191501
		public SimpleAnimation InfoPlayerSkill
		{
			get
			{
				return this._infoPlayerSkill;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x00193309 File Offset: 0x00191509
		public PguiTextCtrl NumPlayerSkill
		{
			get
			{
				return this._numPlayerSkill;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x00193311 File Offset: 0x00191511
		public PguiButtonCtrl BtnInfoSkill
		{
			get
			{
				return this._btnInfoSkill;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x00193319 File Offset: 0x00191519
		public GameObject AnimPlayerSkill
		{
			get
			{
				return this._animPlayerSkill;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x00193321 File Offset: 0x00191521
		public AEImage AnimPlayerSkillAE
		{
			get
			{
				return this._animPlayerSkillAE;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x00193329 File Offset: 0x00191529
		public AEImage RareInAE
		{
			get
			{
				return this._rareInAE;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x00193331 File Offset: 0x00191531
		public AEImage BossInAE
		{
			get
			{
				return this._bossInAE;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x00193339 File Offset: 0x00191539
		public AEImage BattleStartAE
		{
			get
			{
				return this._battleStartAE;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x00193341 File Offset: 0x00191541
		public AEImage ResultAE
		{
			get
			{
				return this._resultAE;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x00193349 File Offset: 0x00191549
		public PguiAECtrl KemonoArtsAE
		{
			get
			{
				return this._kemonoArtsAE;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x00193351 File Offset: 0x00191551
		public PguiAECtrl KemonoFadeAE
		{
			get
			{
				return this._kemonoFadeAE;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x00193359 File Offset: 0x00191559
		public PguiAECtrl ChainFrontAE
		{
			get
			{
				return this._chainFrontAE;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x00193361 File Offset: 0x00191561
		public PguiAECtrl ChainBackAE
		{
			get
			{
				return this._chainBackAE;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x00193369 File Offset: 0x00191569
		public PguiAECtrl ChainCountAE
		{
			get
			{
				return this._chainCountAE;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x00193371 File Offset: 0x00191571
		public PguiAECtrl BeatActAE
		{
			get
			{
				return this._beatActAE;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x00193379 File Offset: 0x00191579
		public PguiAECtrl ActionActAE
		{
			get
			{
				return this._actionActAE;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x00193381 File Offset: 0x00191581
		public PguiAECtrl SpecialActAE
		{
			get
			{
				return this._specialActAE;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x00193389 File Offset: 0x00191589
		public PguiAECtrl TryActAE
		{
			get
			{
				return this._tryActAE;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x00193391 File Offset: 0x00191591
		public float TryActMove
		{
			get
			{
				return this._tryActMove;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x00193399 File Offset: 0x00191599
		public PguiAECtrl TryActPlasmAE
		{
			get
			{
				return this._tryActPlasmAE;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x001933A1 File Offset: 0x001915A1
		public List<AEImage> StaySkill
		{
			get
			{
				return this._staySkill;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x001933A9 File Offset: 0x001915A9
		public List<AEImage> ScheduledSkillListAE
		{
			get
			{
				return this._scheduledSkillListAE;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x001933B1 File Offset: 0x001915B1
		public List<AEImage> EnemyScheduledSkillListAE
		{
			get
			{
				return this._enemyScheduledSkillListAE;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x001933B9 File Offset: 0x001915B9
		public Transform HpGage
		{
			get
			{
				return this._hpGage;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x001933C1 File Offset: 0x001915C1
		public Transform NumDamage
		{
			get
			{
				return this._numDamage;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x001933C9 File Offset: 0x001915C9
		public Transform OrderCard
		{
			get
			{
				return this._orderCard;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x001933D1 File Offset: 0x001915D1
		public PguiAECtrl PvpChainCountAE
		{
			get
			{
				return this._pvpChainCountAE;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x001933D9 File Offset: 0x001915D9
		public PguiAECtrl PvpBeatActAE
		{
			get
			{
				return this._pvpBeatActAE;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x001933E1 File Offset: 0x001915E1
		public PguiAECtrl PvpActionActAE
		{
			get
			{
				return this._pvpActionActAE;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x001933E9 File Offset: 0x001915E9
		public PguiAECtrl PvpTryActAE
		{
			get
			{
				return this._pvpTryActAE;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x001933F1 File Offset: 0x001915F1
		public PguiAECtrl PvpSpecialActAE
		{
			get
			{
				return this._pvpSpecialActAE;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x001933F9 File Offset: 0x001915F9
		public List<AEImage> PvpStaySkill
		{
			get
			{
				return this._pvpStaySkill;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x00193401 File Offset: 0x00191601
		public SimpleAnimation PvpCoinGet
		{
			get
			{
				return this._pvpCoinGet;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x00193409 File Offset: 0x00191609
		public SimpleAnimation ArtsInfo
		{
			get
			{
				return this._artsInfo;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x00193411 File Offset: 0x00191611
		public SimpleAnimation StatInfo
		{
			get
			{
				return this._statInfo;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x00193419 File Offset: 0x00191619
		public HorizontalLayoutGroup TrainingScoreL
		{
			get
			{
				return this._trainingScoreL;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x00193421 File Offset: 0x00191621
		public HorizontalLayoutGroup TrainingScoreR
		{
			get
			{
				return this._trainingScoreR;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x00193429 File Offset: 0x00191629
		public AEImage TrainingMissionAE1
		{
			get
			{
				return this._trainingMissionAE1;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x00193431 File Offset: 0x00191631
		public AEImage TrainingMissionAE2
		{
			get
			{
				return this._trainingMissionAE2;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x00193439 File Offset: 0x00191639
		public Transform PopUpInfoCard
		{
			get
			{
				return this._popUpInfoCard;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x00193441 File Offset: 0x00191641
		public Transform PopUpInfoAction
		{
			get
			{
				return this._popUpInfoAction;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x00193449 File Offset: 0x00191649
		public PguiTextCtrl CardInfo
		{
			get
			{
				return this._cardInfo;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x00193451 File Offset: 0x00191651
		public PguiTextCtrl CardInfoV
		{
			get
			{
				return this._cardInfoV;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x00193459 File Offset: 0x00191659
		public PguiTextCtrl PvpTrainingScore
		{
			get
			{
				return this._pvpTrainingScore;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x00193461 File Offset: 0x00191661
		public PguiTextCtrl PvpTrainingScoreV
		{
			get
			{
				return this._pvpTrainingScoreV;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x00193469 File Offset: 0x00191669
		public PguiAECtrl HugeFadeAE
		{
			get
			{
				return this._hugeFadeAE;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x00193471 File Offset: 0x00191671
		public PguiAECtrl CaptainCautionAE
		{
			get
			{
				return this._captainCautionAE;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x00193479 File Offset: 0x00191679
		public AEImage TacticsPlasmScoreAE
		{
			get
			{
				return this._tacticsPlasmScoreAE;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x00193481 File Offset: 0x00191681
		public AEImage TacticsAttackAE
		{
			get
			{
				return this._tacticsAttackAE;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x00193489 File Offset: 0x00191689
		public AEImage TacticsAE
		{
			get
			{
				return this._tacticsAE;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x00193491 File Offset: 0x00191691
		public AEImage TacticsPlayerAE
		{
			get
			{
				return this._tacticsPlayerAE;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x00193499 File Offset: 0x00191699
		public AEImage TacticsVersusAE
		{
			get
			{
				return this._tacticsVersusAE;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x001934A1 File Offset: 0x001916A1
		public AEImage TacticsEndScoreAE
		{
			get
			{
				return this._tacticsEndScoreAE;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x001934A9 File Offset: 0x001916A9
		public PguiAECtrl TickleStartAE
		{
			get
			{
				return this._tickleStartAE;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x001934B1 File Offset: 0x001916B1
		public PguiAECtrl TickleResultAE
		{
			get
			{
				return this._tickleResultAE;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x001934B9 File Offset: 0x001916B9
		public GameObject PracticeSign
		{
			get
			{
				return this._practiceSign;
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x001934C4 File Offset: 0x001916C4
		public SceneBattle_GUI(Transform baseTr)
		{
			this._playerSkill = baseTr.Find("PlayerSkill").gameObject;
			this._playerSkillAE = this._playerSkill.transform.Find("AEImage").GetComponent<AEImage>();
			this._playerSkillAE.playLoop = false;
			this._playerSkillAE.autoPlay = true;
			this._btnPlayerSkill = this._playerSkill.transform.Find("PlayerSkill/Btn_PlayerSkill").GetComponent<PguiButtonCtrl>();
			this._iconPlayerSkill = this._btnPlayerSkill.transform.Find("BaseImage/Icon_PlayerSkill").GetComponent<PguiReplaceSpriteCtrl>();
			this._remPlayerSkill = this._btnPlayerSkill.transform.Find("BaseImage/Num_Act").GetComponent<PguiTextCtrl>();
			this._playerSkillBan = this._btnPlayerSkill.transform.Find("AEImage_MarkBan").gameObject;
			this._tacticsSkill = baseTr.Find("TacticsSkill").gameObject;
			this._tacticsSkillAE = this._tacticsSkill.transform.Find("AEImage").GetComponent<PguiAECtrl>();
			this._iconTacticsSkill = this._tacticsSkill.transform.Find("TacticsSkill/BaseImage/Icon_TactiksSkill").GetComponent<PguiReplaceSpriteCtrl>();
			this._deckAll = baseTr.Find("DeckAll").gameObject;
			this._chara = new List<GameObject>();
			this._touchChara = new List<Transform>();
			this._touchWait = new List<Transform>();
			this._charaSel = new List<PguiAECtrl>();
			this._effArtsOk = new List<AEImage>();
			this._effArtsOkDbl = new List<AEImage>();
			this._touchArts = new List<Transform>();
			for (int i = 1; i <= this.numberFriends; i++)
			{
				Transform transform = this._deckAll.transform.Find("Chara0" + i.ToString());
				this._chara.Add(transform.gameObject);
				this._touchChara.Add(transform.Find("TouchCollision"));
				this._touchWait.Add(transform.Find("Card/Btn_StaySkill"));
				this._charaSel.Add(transform.Find("Card/AEImage_CharaSelect").GetComponent<PguiAECtrl>());
				transform = this._deckAll.transform.Find("Eff_ArtsOK_Front0" + i.ToString());
				AEImage aeimage = transform.GetComponent<AEImage>();
				aeimage.playLoop = true;
				aeimage.autoPlay = true;
				aeimage.playInTime = 0f;
				aeimage.playOutTime = aeimage.duration;
				aeimage.enabled = false;
				this._effArtsOk.Add(aeimage);
				this._touchArts.Add(transform.Find("TouchCollision"));
				transform = this._deckAll.transform.Find("Eff_MPdouble_ArtsOK_Front0" + i.ToString());
				aeimage = transform.GetComponent<AEImage>();
				aeimage.playLoop = true;
				aeimage.autoPlay = true;
				aeimage.playInTime = 0f;
				aeimage.playOutTime = aeimage.duration;
				aeimage.enabled = false;
				this._effArtsOkDbl.Add(aeimage);
			}
			this._deckAllAE = this._deckAll.transform.Find("AEImage_CardCtr").GetComponent<AEImage>();
			this._deckAllAE.playLoop = false;
			this._deckAllAE.autoPlay = false;
			this._deckAllAE.playTime = 2.5f;
			this._actInfoAll = baseTr.Find("ActInfoAll").gameObject;
			this._actGageBase = this._actInfoAll.transform.Find("ActGage_Base").gameObject;
			this._actGageGage = this._actGageBase.transform.Find("ActGage_Gage").GetComponent<PguiImageCtrl>();
			this._actGageGageAdd = this._actGageBase.transform.Find("ActGage_Gage_Add").GetComponent<PguiImageCtrl>();
			this._actGageGageAdd2 = this._actGageBase.transform.Find("ActGage_Gage_Add2").GetComponent<PguiImageCtrl>();
			this._actGageGageFlash = this._actGageBase.transform.Find("AEImage_GageFlash").gameObject;
			this._numActBefore = this._actGageBase.transform.Find("Num_Act_Before").GetComponent<PguiTextCtrl>();
			this._numActAfter = this._actGageBase.transform.Find("Num_Act_After").GetComponent<PguiTextCtrl>();
			this._touchActGage = this._actGageBase.transform.Find("TouchCollision");
			this._actInfo = this._actInfoAll.transform.Find("ActInfo").GetComponent<CanvasGroup>();
			this._numUseBefore = this._actInfo.transform.Find("Num_Use_Before").GetComponent<PguiTextCtrl>();
			this._numUseAfter = this._actInfo.transform.Find("Num_Use_After").GetComponent<PguiTextCtrl>();
			this._actGageAE = this._actInfoAll.transform.Find("AEImage_ActGage").GetComponent<AEImage>();
			this._actGageAE.playLoop = false;
			this._actGageAE.autoPlay = true;
			this._actGageExtra = this._actInfoAll.transform.Find("ExtraPoint").GetComponent<PguiTextCtrl>();
			this._actInfoBackAE = this._actInfoAll.transform.Find("AEImage_ActLight_Back").GetComponent<AEImage>();
			this._actInfoBackAE.playLoop = false;
			this._actInfoBackAE.autoPlay = true;
			this._actInfoFrontAE = this._actInfoAll.transform.Find("AEImage_ActLight_Front").GetComponent<AEImage>();
			this._actInfoFrontAE.playLoop = false;
			this._actInfoFrontAE.autoPlay = true;
			this._actGageBan = this._actInfoAll.transform.Find("AEImage_MarkBan").gameObject;
			this._timeAll = baseTr.Find("TimeAll").gameObject;
			this._menuAll = baseTr.Find("MenuAll").gameObject;
			this._btnAuto = this._menuAll.transform.Find("Btn_Auto").GetComponent<PguiToggleButtonCtrl>();
			this._btnAutoLock = this._btnAuto.transform.Find("Mark_Lock").gameObject;
			this._btnAutoTxt = this._btnAuto.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this._animAutoInfo = this._timeAll.transform.Find("PopUpInfo").GetComponent<SimpleAnimation>();
			this._btnAutoBan = this._btnAuto.transform.Find("AEImage_MarkBan").gameObject;
			this._btnAutoLamp = this._btnAuto.transform.Find("AEImage_AutLamp").gameObject;
			this._btnFast = this._menuAll.transform.Find("Btn_Fast").GetComponent<PguiToggleButtonCtrl>();
			this._btnMenu = this._menuAll.transform.Find("Btn_Menu").GetComponent<PguiButtonCtrl>();
			this._numYaseiTotal = this._timeAll.transform.Find("YaseiInfo/Num_Yasei_Total").GetComponent<PguiTextCtrl>();
			this._plasmShare = baseTr.Find("AEImage_PlasmShare").gameObject;
			this._plasmShareCmnAE = this._plasmShare.transform.Find("AEImage_PlasmShare_Cmn").GetComponent<PguiAECtrl>();
			this._plasmShareAE = new List<AEImage>();
			int num = 1;
			for (;;)
			{
				Transform transform2 = this._plasmShare.transform.Find("AEImage_PlasmShare" + num.ToString("D2"));
				if (transform2 == null)
				{
					break;
				}
				this._plasmShareAE.Add(transform2.GetComponent<AEImage>());
				num++;
			}
			this._numWave = this._timeAll.transform.Find("Num_Wave").GetComponent<PguiTextCtrl>();
			this._numTurn = this._timeAll.transform.Find("Num_Turn").GetComponent<PguiTextCtrl>();
			this._selectCard = baseTr.Find("CardDone_Center").gameObject;
			this._animSelectCard = this._selectCard.GetComponent<SimpleAnimation>();
			this._animSelectCard.playAutomatically = false;
			this._actionCard = baseTr.Find("CardDone_Top").gameObject;
			this._animActionCard = this._actionCard.GetComponent<SimpleAnimation>();
			this._animActionCard.playAutomatically = false;
			this._animTurn = baseTr.Find("Anim_Turn").gameObject;
			this._animTurnAE = this._animTurn.transform.Find("AEImage").GetComponent<AEImage>();
			this._animTurnAE.playLoop = false;
			this._animTurnAE.autoPlay = true;
			this._animTurnAE.playOutTime = this._animTurnAE.duration;
			this._animNumTurn1 = this._animTurn.transform.Find("Num_Turn/Num_Turn01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumTurn2 = this._animTurn.transform.Find("Num_Turn/Num_Turn02").GetComponent<PguiReplaceSpriteCtrl>();
			this._animTurnAct = this._animTurn.transform.Find("ActGageInfo").gameObject;
			this._animTurnActAE = this._animTurnAct.transform.Find("AEImage").GetComponent<AEImage>();
			this._animTurnActAE.playLoop = false;
			this._animTurnActAE.autoPlay = true;
			this._animTurnActAE.playOutTime = this._animTurnAE.duration;
			this._animNumTurnAct = this._animTurnAct.transform.Find("ActGageInfo/Num_ActGage").GetComponent<PguiTextCtrl>();
			this._animWave = baseTr.Find("Anim_Wave").gameObject;
			this._animWaveAE = this._animWave.transform.Find("AEImage").GetComponent<AEImage>();
			this._animWaveAE.playLoop = false;
			this._animWaveAE.autoPlay = true;
			this._animWaveAE.playOutTime = this._animWaveAE.duration;
			this._animNumWaveC1 = this._animWave.transform.Find("WaveAll/Num_Wave_Current01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveC2 = this._animWave.transform.Find("WaveAll/Num_Wave_Current02").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveM1 = this._animWave.transform.Find("WaveAll/Num_Wave_Max01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveM2 = this._animWave.transform.Find("WaveAll/Num_Wave_Max02").GetComponent<PguiReplaceSpriteCtrl>();
			this._infoPlayerSkill = baseTr.Find("Info_PlayerSkill").GetComponent<SimpleAnimation>();
			this._numPlayerSkill = this._infoPlayerSkill.transform.Find("Num_Times").GetComponent<PguiTextCtrl>();
			this._btnInfoSkill = this._infoPlayerSkill.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this._animPlayerSkill = baseTr.Find("Anim_PlayerSkillCutIn").gameObject;
			this._animPlayerSkillAE = this._animPlayerSkill.transform.Find("AEImage").GetComponent<AEImage>();
			this._animPlayerSkillAE.playLoop = false;
			this._animPlayerSkillAE.autoPlay = true;
			this._animPlayerSkillAE.playOutTime = this._animPlayerSkillAE.duration;
			this._rareInAE = baseTr.Find("AEImage_RareEnemyIn").GetComponent<AEImage>();
			this._rareInAE.playLoop = false;
			this._rareInAE.autoPlay = true;
			this._rareInAE.playOutTime = this._rareInAE.duration;
			this._bossInAE = baseTr.Find("AEImage_BossIn").GetComponent<AEImage>();
			this._bossInAE.playLoop = false;
			this._bossInAE.autoPlay = true;
			this._bossInAE.playOutTime = this._bossInAE.duration;
			this._battleStartAE = baseTr.Find("AEImage_BattleStart").GetComponent<AEImage>();
			this._battleStartAE.playLoop = false;
			this._battleStartAE.autoPlay = true;
			this._battleStartAE.playOutTime = this._battleStartAE.duration;
			this._resultAE = baseTr.Find("AEImage_Result").GetComponent<AEImage>();
			this._resultAE.playLoop = false;
			this._resultAE.autoPlay = true;
			this._kemonoArtsAE = baseTr.Find("AEImage_KemonoArtsAuth").GetComponent<PguiAECtrl>();
			this._kemonoFadeAE = baseTr.Find("AEImage_KemonoArtsAuth_Fade").GetComponent<PguiAECtrl>();
			this._chainFrontAE = baseTr.Find("AEImage_Chain_Front").GetComponent<PguiAECtrl>();
			this._chainBackAE = baseTr.Find("AEImage_Chain_Back/AEImage").GetComponent<PguiAECtrl>();
			this._chainCountAE = baseTr.Find("AEImage_ChainCount").GetComponent<PguiAECtrl>();
			this._beatActAE = baseTr.Find("AEImage_OrederCutIn_R/AEImage").GetComponent<PguiAECtrl>();
			this._actionActAE = baseTr.Find("AEImage_OrederCutIn_B/AEImage").GetComponent<PguiAECtrl>();
			this._specialActAE = baseTr.Find("AEImage_OrederCutIn_S/AEImage").GetComponent<PguiAECtrl>();
			this._tryActAE = baseTr.Find("AEImage_OrederCutIn_G/AEImage").GetComponent<PguiAECtrl>();
			this._tryActAE.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
			this._tryActMove = this._tryActAE.m_AEImage.playInTime;
			this._tryActPlasmAE = this._tryActAE.transform.parent.Find("AEImage_TryPlasmUp").GetComponent<PguiAECtrl>();
			Transform transform3 = baseTr.Find("StaySkill");
			this._staySkill = new List<AEImage>();
			int num2 = 1;
			for (;;)
			{
				Transform transform4 = transform3.Find("AEImage_StaySkill0" + num2.ToString());
				if (transform4 == null)
				{
					break;
				}
				AEImage aeimage2 = transform4.GetComponent<AEImage>();
				aeimage2.autoPlay = true;
				aeimage2.playOutTime = aeimage2.duration;
				aeimage2.gameObject.AddComponent<Grayscale>();
				this._staySkill.Add(aeimage2);
				aeimage2 = transform4.Find("AEImage_StaySkill_Front").GetComponent<AEImage>();
				aeimage2.autoPlay = true;
				aeimage2.playOutTime = aeimage2.duration;
				transform4.gameObject.SetActive(false);
				num2++;
			}
			transform3 = baseTr.Find("ScheduledSkill");
			this._scheduledSkillListAE = new List<AEImage>();
			if (transform3 != null)
			{
				int num3 = 1;
				for (;;)
				{
					Transform transform5 = transform3.Find("AEImage_ScheduledSkill0" + num3.ToString());
					if (transform5 == null)
					{
						break;
					}
					AEImage component = transform5.GetComponent<AEImage>();
					component.autoPlay = true;
					component.playOutTime = component.duration;
					this._scheduledSkillListAE.Add(component);
					transform5.gameObject.SetActive(false);
					num3++;
				}
			}
			transform3 = baseTr.Find("EnemyScheduledSkill");
			this._enemyScheduledSkillListAE = new List<AEImage>();
			if (transform3 != null)
			{
				int num4 = 1;
				for (;;)
				{
					Transform transform6 = transform3.Find("AEImage_ScheduledSkill0" + num4.ToString());
					if (transform6 == null)
					{
						break;
					}
					AEImage component2 = transform6.GetComponent<AEImage>();
					component2.autoPlay = true;
					component2.playOutTime = component2.duration;
					this._enemyScheduledSkillListAE.Add(component2);
					transform6.gameObject.SetActive(false);
					num4++;
				}
			}
			transform3.gameObject.SetActive(true);
			this._hpGage = new GameObject("HpGage").AddComponent<RectTransform>().transform;
			this._hpGage.SetParent(baseTr, false);
			this._hpGage.SetSiblingIndex(0);
			this._numDamage = new GameObject("NumDamage").AddComponent<RectTransform>().transform;
			this._numDamage.SetParent(baseTr, false);
			this._orderCard = new GameObject("OrderCard").transform;
			this._orderCard.SetParent(baseTr, false);
			this._orderCard.gameObject.SetActive(false);
			this._pvpChainCountAE = baseTr.Find("AEImage_ChainCount_PvPEnemy").GetComponent<PguiAECtrl>();
			this._pvpBeatActAE = baseTr.Find("AEImage_OrederCutIn_R_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpActionActAE = baseTr.Find("AEImage_OrederCutIn_B_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpTryActAE = baseTr.Find("AEImage_OrederCutIn_G_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpSpecialActAE = baseTr.Find("AEImage_OrederCutIn_S_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			transform3 = baseTr.Find("StaySkill_PvPEnemy");
			this._pvpStaySkill = new List<AEImage>();
			int num5 = 1;
			for (;;)
			{
				Transform transform7 = transform3.Find("AEImage_StaySkill0" + num5.ToString());
				if (transform7 == null)
				{
					break;
				}
				AEImage aeimage3 = transform7.GetComponent<AEImage>();
				aeimage3.autoPlay = true;
				aeimage3.playOutTime = aeimage3.duration;
				aeimage3.gameObject.AddComponent<Grayscale>();
				this._pvpStaySkill.Add(aeimage3);
				aeimage3 = transform7.Find("AEImage_StaySkill_Front").GetComponent<AEImage>();
				aeimage3.autoPlay = true;
				aeimage3.playOutTime = aeimage3.duration;
				transform7.gameObject.SetActive(false);
				num5++;
			}
			transform3.gameObject.SetActive(true);
			this._pvpCoinGet = baseTr.Find("PvP_CutInInfo_Get").GetComponent<SimpleAnimation>();
			this._artsInfo = baseTr.Find("TapInfo_KemonoMiracle").GetComponent<SimpleAnimation>();
			this._statInfo = baseTr.Find("TapInfo_BattleStatus").GetComponent<SimpleAnimation>();
			this._trainingMissionAE1 = baseTr.Find("AEImage_Training_MissionClear/Bg").GetComponent<AEImage>();
			this._trainingMissionAE1.playLoop = false;
			this._trainingMissionAE1.autoPlay = true;
			this._trainingMissionAE1.playOutTime = this._trainingMissionAE1.duration;
			this._trainingMissionAE1.IsUnscaledTime = true;
			this._trainingMissionAE2 = baseTr.Find("AEImage_Training_MissionClear/MissionClear").GetComponent<AEImage>();
			this._trainingMissionAE2.playLoop = false;
			this._trainingMissionAE2.autoPlay = true;
			this._trainingMissionAE2.playOutTime = this._trainingMissionAE2.duration;
			this._trainingMissionAE2.IsUnscaledTime = true;
			this._trainingScoreL = baseTr.Find("Training_ScoreL/ScoreAll").GetComponent<HorizontalLayoutGroup>();
			this._trainingScoreR = baseTr.Find("Training_ScoreR/ScoreAll").GetComponent<HorizontalLayoutGroup>();
			this._popUpInfoCard = baseTr.Find("Null_PopUpInfo_Card");
			this._popUpInfoAction = baseTr.Find("Null_PopUpInfo_Action");
			this._cardInfo = this._timeAll.transform.Find("CardInfo").GetComponent<PguiTextCtrl>();
			this._cardInfoV = this._timeAll.transform.Find("CardInfoV").GetComponent<PguiTextCtrl>();
			this._pvpTrainingScore = this._timeAll.transform.Find("PvpTrainingScore").GetComponent<PguiTextCtrl>();
			this._pvpTrainingScoreV = this._timeAll.transform.Find("PvpTrainingScoreV").GetComponent<PguiTextCtrl>();
			this._hugeFadeAE = baseTr.Find("AEImage_WhiteFade").GetComponent<PguiAECtrl>();
			this._captainCautionAE = baseTr.Find("AEImage_Caution").GetComponent<PguiAECtrl>();
			this._tacticsPlasmScoreAE = baseTr.Find("AEImage_Tactics_PlasmScore").GetComponent<AEImage>();
			this._tacticsAttackAE = baseTr.Find("AEImage_Tactics_Attack").GetComponent<AEImage>();
			this._tacticsAE = baseTr.Find("AEImage_Tactics/Bg").GetComponent<AEImage>();
			this._tacticsAE.playLoop = false;
			this._tacticsAE.autoPlay = true;
			this._tacticsAE.playOutTime = this._tacticsAE.duration;
			this._tacticsAE.IsUnscaledTime = true;
			this._tacticsPlayerAE = baseTr.Find("AEImage_Tactics/AEImage_OwnTactics").GetComponent<AEImage>();
			this._tacticsPlayerAE.playLoop = false;
			this._tacticsPlayerAE.autoPlay = true;
			this._tacticsPlayerAE.playOutTime = this._tacticsPlayerAE.duration;
			this._tacticsPlayerAE.IsUnscaledTime = true;
			this._tacticsVersusAE = baseTr.Find("AEImage_Tactics/AEImage_EnemyTactics").GetComponent<AEImage>();
			this._tacticsVersusAE.playLoop = false;
			this._tacticsVersusAE.autoPlay = true;
			this._tacticsVersusAE.playOutTime = this._tacticsVersusAE.duration;
			this._tacticsVersusAE.IsUnscaledTime = true;
			this._tacticsEndScoreAE = baseTr.Find("AEImage_TacticsEnd_ToalDmage").GetComponent<AEImage>();
			baseTr.Find("AEImage_Tactics").SetAsLastSibling();
			this._tickleStartAE = baseTr.Find("AEImage_Tickling_start").GetComponent<PguiAECtrl>();
			this._tickleResultAE = baseTr.Find("AEImage_Tickling_result").GetComponent<PguiAECtrl>();
			this._practiceSign = baseTr.Find("Practice").gameObject;
		}

		// Token: 0x04001973 RID: 6515
		private readonly int numberFriends = 5;

		// Token: 0x04001974 RID: 6516
		private GameObject _playerSkill;

		// Token: 0x04001975 RID: 6517
		private AEImage _playerSkillAE;

		// Token: 0x04001976 RID: 6518
		private PguiButtonCtrl _btnPlayerSkill;

		// Token: 0x04001977 RID: 6519
		private PguiReplaceSpriteCtrl _iconPlayerSkill;

		// Token: 0x04001978 RID: 6520
		private PguiTextCtrl _remPlayerSkill;

		// Token: 0x04001979 RID: 6521
		private GameObject _playerSkillBan;

		// Token: 0x0400197A RID: 6522
		private GameObject _tacticsSkill;

		// Token: 0x0400197B RID: 6523
		private PguiAECtrl _tacticsSkillAE;

		// Token: 0x0400197C RID: 6524
		private PguiReplaceSpriteCtrl _iconTacticsSkill;

		// Token: 0x0400197D RID: 6525
		private GameObject _deckAll;

		// Token: 0x0400197E RID: 6526
		private List<GameObject> _chara;

		// Token: 0x0400197F RID: 6527
		private List<Transform> _touchChara;

		// Token: 0x04001980 RID: 6528
		private List<Transform> _touchWait;

		// Token: 0x04001981 RID: 6529
		private List<PguiAECtrl> _charaSel;

		// Token: 0x04001982 RID: 6530
		private List<AEImage> _effArtsOk;

		// Token: 0x04001983 RID: 6531
		private List<AEImage> _effArtsOkDbl;

		// Token: 0x04001984 RID: 6532
		private List<Transform> _touchArts;

		// Token: 0x04001985 RID: 6533
		private AEImage _deckAllAE;

		// Token: 0x04001986 RID: 6534
		private GameObject _actInfoAll;

		// Token: 0x04001987 RID: 6535
		private GameObject _actGageBase;

		// Token: 0x04001988 RID: 6536
		private PguiImageCtrl _actGageGage;

		// Token: 0x04001989 RID: 6537
		private PguiImageCtrl _actGageGageAdd;

		// Token: 0x0400198A RID: 6538
		private PguiImageCtrl _actGageGageAdd2;

		// Token: 0x0400198B RID: 6539
		private GameObject _actGageGageFlash;

		// Token: 0x0400198C RID: 6540
		private PguiTextCtrl _numActBefore;

		// Token: 0x0400198D RID: 6541
		private PguiTextCtrl _numActAfter;

		// Token: 0x0400198E RID: 6542
		private Transform _touchActGage;

		// Token: 0x0400198F RID: 6543
		private CanvasGroup _actInfo;

		// Token: 0x04001990 RID: 6544
		private PguiTextCtrl _numUseBefore;

		// Token: 0x04001991 RID: 6545
		private PguiTextCtrl _numUseAfter;

		// Token: 0x04001992 RID: 6546
		private AEImage _actGageAE;

		// Token: 0x04001993 RID: 6547
		private PguiTextCtrl _actGageExtra;

		// Token: 0x04001994 RID: 6548
		private AEImage _actInfoBackAE;

		// Token: 0x04001995 RID: 6549
		private AEImage _actInfoFrontAE;

		// Token: 0x04001996 RID: 6550
		private GameObject _actGageBan;

		// Token: 0x04001997 RID: 6551
		private GameObject _timeAll;

		// Token: 0x04001998 RID: 6552
		private GameObject _menuAll;

		// Token: 0x04001999 RID: 6553
		private PguiToggleButtonCtrl _btnAuto;

		// Token: 0x0400199A RID: 6554
		private GameObject _btnAutoLock;

		// Token: 0x0400199B RID: 6555
		private PguiTextCtrl _btnAutoTxt;

		// Token: 0x0400199C RID: 6556
		private SimpleAnimation _animAutoInfo;

		// Token: 0x0400199D RID: 6557
		private GameObject _btnAutoBan;

		// Token: 0x0400199E RID: 6558
		private GameObject _btnAutoLamp;

		// Token: 0x0400199F RID: 6559
		private PguiToggleButtonCtrl _btnFast;

		// Token: 0x040019A0 RID: 6560
		private PguiButtonCtrl _btnMenu;

		// Token: 0x040019A1 RID: 6561
		private PguiTextCtrl _numYaseiTotal;

		// Token: 0x040019A2 RID: 6562
		private GameObject _plasmShare;

		// Token: 0x040019A3 RID: 6563
		private PguiAECtrl _plasmShareCmnAE;

		// Token: 0x040019A4 RID: 6564
		private List<AEImage> _plasmShareAE;

		// Token: 0x040019A5 RID: 6565
		private PguiTextCtrl _numWave;

		// Token: 0x040019A6 RID: 6566
		private PguiTextCtrl _numTurn;

		// Token: 0x040019A7 RID: 6567
		private GameObject _selectCard;

		// Token: 0x040019A8 RID: 6568
		private SimpleAnimation _animSelectCard;

		// Token: 0x040019A9 RID: 6569
		private GameObject _actionCard;

		// Token: 0x040019AA RID: 6570
		private SimpleAnimation _animActionCard;

		// Token: 0x040019AB RID: 6571
		private GameObject _animTurn;

		// Token: 0x040019AC RID: 6572
		private AEImage _animTurnAE;

		// Token: 0x040019AD RID: 6573
		private PguiReplaceSpriteCtrl _animNumTurn1;

		// Token: 0x040019AE RID: 6574
		private PguiReplaceSpriteCtrl _animNumTurn2;

		// Token: 0x040019AF RID: 6575
		private GameObject _animTurnAct;

		// Token: 0x040019B0 RID: 6576
		private AEImage _animTurnActAE;

		// Token: 0x040019B1 RID: 6577
		private PguiTextCtrl _animNumTurnAct;

		// Token: 0x040019B2 RID: 6578
		private GameObject _animWave;

		// Token: 0x040019B3 RID: 6579
		private AEImage _animWaveAE;

		// Token: 0x040019B4 RID: 6580
		private PguiReplaceSpriteCtrl _animNumWaveC1;

		// Token: 0x040019B5 RID: 6581
		private PguiReplaceSpriteCtrl _animNumWaveC2;

		// Token: 0x040019B6 RID: 6582
		private PguiReplaceSpriteCtrl _animNumWaveM1;

		// Token: 0x040019B7 RID: 6583
		private PguiReplaceSpriteCtrl _animNumWaveM2;

		// Token: 0x040019B8 RID: 6584
		private SimpleAnimation _infoPlayerSkill;

		// Token: 0x040019B9 RID: 6585
		private PguiTextCtrl _numPlayerSkill;

		// Token: 0x040019BA RID: 6586
		private PguiButtonCtrl _btnInfoSkill;

		// Token: 0x040019BB RID: 6587
		private GameObject _animPlayerSkill;

		// Token: 0x040019BC RID: 6588
		private AEImage _animPlayerSkillAE;

		// Token: 0x040019BD RID: 6589
		private AEImage _rareInAE;

		// Token: 0x040019BE RID: 6590
		private AEImage _bossInAE;

		// Token: 0x040019BF RID: 6591
		private AEImage _battleStartAE;

		// Token: 0x040019C0 RID: 6592
		private AEImage _resultAE;

		// Token: 0x040019C1 RID: 6593
		private PguiAECtrl _kemonoArtsAE;

		// Token: 0x040019C2 RID: 6594
		private PguiAECtrl _kemonoFadeAE;

		// Token: 0x040019C3 RID: 6595
		private PguiAECtrl _chainFrontAE;

		// Token: 0x040019C4 RID: 6596
		private PguiAECtrl _chainBackAE;

		// Token: 0x040019C5 RID: 6597
		private PguiAECtrl _chainCountAE;

		// Token: 0x040019C6 RID: 6598
		private PguiAECtrl _beatActAE;

		// Token: 0x040019C7 RID: 6599
		private PguiAECtrl _actionActAE;

		// Token: 0x040019C8 RID: 6600
		private PguiAECtrl _specialActAE;

		// Token: 0x040019C9 RID: 6601
		private PguiAECtrl _tryActAE;

		// Token: 0x040019CA RID: 6602
		private float _tryActMove;

		// Token: 0x040019CB RID: 6603
		private PguiAECtrl _tryActPlasmAE;

		// Token: 0x040019CC RID: 6604
		private List<AEImage> _staySkill;

		// Token: 0x040019CD RID: 6605
		private List<AEImage> _scheduledSkillListAE;

		// Token: 0x040019CE RID: 6606
		private List<AEImage> _enemyScheduledSkillListAE;

		// Token: 0x040019CF RID: 6607
		private Transform _hpGage;

		// Token: 0x040019D0 RID: 6608
		private Transform _numDamage;

		// Token: 0x040019D1 RID: 6609
		private Transform _orderCard;

		// Token: 0x040019D2 RID: 6610
		private PguiAECtrl _pvpChainCountAE;

		// Token: 0x040019D3 RID: 6611
		private PguiAECtrl _pvpBeatActAE;

		// Token: 0x040019D4 RID: 6612
		private PguiAECtrl _pvpActionActAE;

		// Token: 0x040019D5 RID: 6613
		private PguiAECtrl _pvpTryActAE;

		// Token: 0x040019D6 RID: 6614
		private PguiAECtrl _pvpSpecialActAE;

		// Token: 0x040019D7 RID: 6615
		private List<AEImage> _pvpStaySkill;

		// Token: 0x040019D8 RID: 6616
		private SimpleAnimation _pvpCoinGet;

		// Token: 0x040019D9 RID: 6617
		private SimpleAnimation _artsInfo;

		// Token: 0x040019DA RID: 6618
		private SimpleAnimation _statInfo;

		// Token: 0x040019DB RID: 6619
		private HorizontalLayoutGroup _trainingScoreL;

		// Token: 0x040019DC RID: 6620
		private HorizontalLayoutGroup _trainingScoreR;

		// Token: 0x040019DD RID: 6621
		private AEImage _trainingMissionAE1;

		// Token: 0x040019DE RID: 6622
		private AEImage _trainingMissionAE2;

		// Token: 0x040019DF RID: 6623
		private Transform _popUpInfoCard;

		// Token: 0x040019E0 RID: 6624
		private Transform _popUpInfoAction;

		// Token: 0x040019E1 RID: 6625
		private PguiTextCtrl _cardInfo;

		// Token: 0x040019E2 RID: 6626
		private PguiTextCtrl _cardInfoV;

		// Token: 0x040019E3 RID: 6627
		private PguiTextCtrl _pvpTrainingScore;

		// Token: 0x040019E4 RID: 6628
		private PguiTextCtrl _pvpTrainingScoreV;

		// Token: 0x040019E5 RID: 6629
		private PguiAECtrl _hugeFadeAE;

		// Token: 0x040019E6 RID: 6630
		private PguiAECtrl _captainCautionAE;

		// Token: 0x040019E7 RID: 6631
		private AEImage _tacticsPlasmScoreAE;

		// Token: 0x040019E8 RID: 6632
		private AEImage _tacticsAttackAE;

		// Token: 0x040019E9 RID: 6633
		private AEImage _tacticsAE;

		// Token: 0x040019EA RID: 6634
		private AEImage _tacticsPlayerAE;

		// Token: 0x040019EB RID: 6635
		private AEImage _tacticsVersusAE;

		// Token: 0x040019EC RID: 6636
		private AEImage _tacticsEndScoreAE;

		// Token: 0x040019ED RID: 6637
		private PguiAECtrl _tickleStartAE;

		// Token: 0x040019EE RID: 6638
		private PguiAECtrl _tickleResultAE;

		// Token: 0x040019EF RID: 6639
		private GameObject _practiceSign;
	}
}
