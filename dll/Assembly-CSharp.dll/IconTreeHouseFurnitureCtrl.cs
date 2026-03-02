using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class IconTreeHouseFurnitureCtrl : MonoBehaviour
{
	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x001682B6 File Offset: 0x001664B6
	// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x001682BE File Offset: 0x001664BE
	public TreeHouseFurnitureStatic treeHouseFurnitureStatic { get; private set; }

	// Token: 0x06001CA3 RID: 7331 RVA: 0x001682C8 File Offset: 0x001664C8
	public void Setup(IconTreeHouseFurnitureCtrl.SetupParam param)
	{
		this.treeHouseFurnitureStatic = param.thfs;
		if (this.treeHouseFurnitureStatic != null)
		{
			base.gameObject.SetActive(true);
			if (this.texFurniture != null)
			{
				this.DispFurnitureChange();
				this.texFurniture.m_RawImage.raycastTarget = false;
			}
			if (this.rawImgRareFrame != null)
			{
				string text = "Texture2D/Frame_CardInterior/interior_frame_0" + ((int)this.treeHouseFurnitureStatic.GetRarity()).ToString();
				this.rawImgRareFrame.SetRawImage(text, true, false, null);
			}
			if (this.textCardName != null)
			{
				this.textCardName.text = this.treeHouseFurnitureStatic.GetName();
				return;
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x0016838C File Offset: 0x0016658C
	public void DispFurnitureChange()
	{
		if (this.treeHouseFurnitureStatic != null)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.treeHouseFurnitureStatic.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
			{
				if (this.treeHouseFurnitureStatic.embedTexturePath.StartsWith("Texture2D/Icon_Dressup/") && this.treeHouseFurnitureStatic.embedTexturePathSub.StartsWith("Texture2D/Icon_Dressup_Bg/"))
				{
					flag2 = true;
				}
				if (!flag2 && this.treeHouseFurnitureStatic.embedTexturePath.StartsWith("Texture2D/Photo/Card_Photo/"))
				{
					flag = true;
				}
			}
			if (this.texBasePoster != null)
			{
				if (flag || flag2)
				{
					this.texBasePoster.gameObject.SetActive(true);
				}
				else
				{
					this.texBasePoster.gameObject.SetActive(false);
				}
			}
			if (this.texPhotoPoster != null)
			{
				if (flag)
				{
					this.texPhotoPoster.gameObject.SetActive(true);
					this.texPhotoPoster.SetRawImage(this.treeHouseFurnitureStatic.embedTexturePath, true, false, null);
				}
				else
				{
					this.texPhotoPoster.gameObject.SetActive(false);
				}
			}
			if (this.texFriendsPosterBg != null)
			{
				if (flag2)
				{
					this.texFriendsPosterBg.gameObject.SetActive(true);
					this.texFriendsPosterBg.SetRawImage(this.treeHouseFurnitureStatic.embedTexturePathSub, true, false, null);
				}
				else
				{
					this.texFriendsPosterBg.gameObject.SetActive(false);
				}
			}
			if (this.texFriendsPosterChara != null)
			{
				if (flag2)
				{
					this.texFriendsPosterChara.gameObject.SetActive(true);
					this.texFriendsPosterChara.SetRawImage(this.treeHouseFurnitureStatic.embedTexturePath, true, false, null);
				}
				else
				{
					this.texFriendsPosterChara.gameObject.SetActive(false);
				}
			}
			if (this.texFurniture != null)
			{
				if (flag || flag2)
				{
					this.texFurniture.gameObject.SetActive(false);
					return;
				}
				this.texFurniture.gameObject.SetActive(true);
				this.texFurniture.SetRawImage(this.treeHouseFurnitureStatic.infoThumbnailPath, true, false, null);
			}
		}
	}

	// Token: 0x04001558 RID: 5464
	[SerializeField]
	private PguiRawImageCtrl texFurniture;

	// Token: 0x04001559 RID: 5465
	[SerializeField]
	private PguiImageCtrl texBasePoster;

	// Token: 0x0400155A RID: 5466
	[SerializeField]
	private PguiRawImageCtrl texFriendsPosterBg;

	// Token: 0x0400155B RID: 5467
	[SerializeField]
	private PguiRawImageCtrl texFriendsPosterChara;

	// Token: 0x0400155C RID: 5468
	[SerializeField]
	private PguiRawImageCtrl texPhotoPoster;

	// Token: 0x0400155D RID: 5469
	[SerializeField]
	private PguiRawImageCtrl rawImgRareFrame;

	// Token: 0x0400155E RID: 5470
	[SerializeField]
	private PguiTextCtrl textCardName;

	// Token: 0x02000F14 RID: 3860
	public class SetupParam
	{
		// Token: 0x06004E99 RID: 20121 RVA: 0x00236D57 File Offset: 0x00234F57
		public SetupParam()
		{
			this.thfs = null;
		}

		// Token: 0x040055B9 RID: 21945
		public TreeHouseFurnitureStatic thfs;
	}
}
