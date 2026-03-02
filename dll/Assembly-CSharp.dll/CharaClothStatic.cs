using System;
using SGNFW.Mst;

// Token: 0x02000072 RID: 114
public class CharaClothStatic : ItemStaticBase
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060003CF RID: 975 RVA: 0x0001BB24 File Offset: 0x00019D24
	// (set) Token: 0x060003D0 RID: 976 RVA: 0x0001BB2C File Offset: 0x00019D2C
	private int Id { get; set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060003D1 RID: 977 RVA: 0x0001BB35 File Offset: 0x00019D35
	// (set) Token: 0x060003D2 RID: 978 RVA: 0x0001BB3D File Offset: 0x00019D3D
	public int CharaId { get; private set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060003D3 RID: 979 RVA: 0x0001BB46 File Offset: 0x00019D46
	// (set) Token: 0x060003D4 RID: 980 RVA: 0x0001BB4E File Offset: 0x00019D4E
	public int GetRank { get; private set; }

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x0001BB57 File Offset: 0x00019D57
	// (set) Token: 0x060003D6 RID: 982 RVA: 0x0001BB5F File Offset: 0x00019D5F
	public int SortNum { get; private set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x0001BB68 File Offset: 0x00019D68
	// (set) Token: 0x060003D8 RID: 984 RVA: 0x0001BB70 File Offset: 0x00019D70
	private int Rarity { get; set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001BB79 File Offset: 0x00019D79
	// (set) Token: 0x060003DA RID: 986 RVA: 0x0001BB81 File Offset: 0x00019D81
	private string Name { get; set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060003DB RID: 987 RVA: 0x0001BB8A File Offset: 0x00019D8A
	// (set) Token: 0x060003DC RID: 988 RVA: 0x0001BB92 File Offset: 0x00019D92
	private string FlavorText { get; set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060003DD RID: 989 RVA: 0x0001BB9B File Offset: 0x00019D9B
	// (set) Token: 0x060003DE RID: 990 RVA: 0x0001BBA3 File Offset: 0x00019DA3
	public int ImageId { get; private set; }

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060003DF RID: 991 RVA: 0x0001BBAC File Offset: 0x00019DAC
	public string displayTexturePath
	{
		get
		{
			return "Texture2D/Icon_Dressup/icon_dressup_" + this.Id.ToString("D4");
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001BBD6 File Offset: 0x00019DD6
	// (set) Token: 0x060003E1 RID: 993 RVA: 0x0001BBDE File Offset: 0x00019DDE
	public string displayBgTexturePath { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x0001BBE7 File Offset: 0x00019DE7
	// (set) Token: 0x060003E3 RID: 995 RVA: 0x0001BBEF File Offset: 0x00019DEF
	public int HpBonus { get; private set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x0001BBF8 File Offset: 0x00019DF8
	// (set) Token: 0x060003E5 RID: 997 RVA: 0x0001BC00 File Offset: 0x00019E00
	public int AtkBonus { get; private set; }

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001BC09 File Offset: 0x00019E09
	// (set) Token: 0x060003E7 RID: 999 RVA: 0x0001BC11 File Offset: 0x00019E11
	public int DefBonus { get; private set; }

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0001BC1A File Offset: 0x00019E1A
	// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0001BC22 File Offset: 0x00019E22
	public bool LongSkirt { get; private set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001BC2B File Offset: 0x00019E2B
	// (set) Token: 0x060003EB RID: 1003 RVA: 0x0001BC33 File Offset: 0x00019E33
	public CharaClothStatic.PlayMotionType PlayMotion { get; private set; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001BC3C File Offset: 0x00019E3C
	// (set) Token: 0x060003ED RID: 1005 RVA: 0x0001BC44 File Offset: 0x00019E44
	public int replaceItemNum { get; private set; }

	// Token: 0x060003EE RID: 1006 RVA: 0x0001BC4D File Offset: 0x00019E4D
	public static string GetDefaultClothPath(int charaId)
	{
		return "Texture2D/Icon_Dressup_Bg/icon_dressup_bg_default";
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001BC54 File Offset: 0x00019E54
	public override int GetId()
	{
		return this.Id;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001BC5C File Offset: 0x00019E5C
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CLOTHES;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001BC60 File Offset: 0x00019E60
	public override string GetName()
	{
		return this.Name;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001BC68 File Offset: 0x00019E68
	public override string GetInfo()
	{
		return this.FlavorText;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001BC70 File Offset: 0x00019E70
	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.Rarity;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001BC78 File Offset: 0x00019E78
	public override int GetStackMax()
	{
		return 99999999;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001BC7F File Offset: 0x00019E7F
	public override string GetIconName()
	{
		return "Texture2D/Icon_Item/" + this.icon_name;
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001BC91 File Offset: 0x00019E91
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001BC94 File Offset: 0x00019E94
	public CharaClothStatic(MstCharaClothesData mst, int cId)
	{
		this.Id = mst.id;
		this.CharaId = cId;
		this.Name = mst.name;
		this.FlavorText = mst.flavorText;
		this.SortNum = mst.sort;
		this.ImageId = mst.imgId;
		this.GetRank = mst.getRank;
		this.displayBgTexturePath = mst.bgTextureName;
		this.HpBonus = mst.hpBonus;
		this.AtkBonus = mst.atkBonus;
		this.DefBonus = mst.defBonus;
		this.icon_name = mst.iconName;
		this.LongSkirt = 1 == mst.longskirt;
		this.Rarity = mst.rarity;
		this.replaceItemNum = mst.replaceItemNum;
		this.PlayMotion = (CharaClothStatic.PlayMotionType)mst.playMotionType;
	}

	// Token: 0x040004D7 RID: 1239
	private string icon_name;

	// Token: 0x02000655 RID: 1621
	public enum PlayMotionType
	{
		// Token: 0x04002E81 RID: 11905
		Default,
		// Token: 0x04002E82 RID: 11906
		HandsNotUse
	}
}
