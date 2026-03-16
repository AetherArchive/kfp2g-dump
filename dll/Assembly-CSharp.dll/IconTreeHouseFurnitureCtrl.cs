using System;
using UnityEngine;

public class IconTreeHouseFurnitureCtrl : MonoBehaviour
{
	public TreeHouseFurnitureStatic treeHouseFurnitureStatic { get; private set; }

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

	[SerializeField]
	private PguiRawImageCtrl texFurniture;

	[SerializeField]
	private PguiImageCtrl texBasePoster;

	[SerializeField]
	private PguiRawImageCtrl texFriendsPosterBg;

	[SerializeField]
	private PguiRawImageCtrl texFriendsPosterChara;

	[SerializeField]
	private PguiRawImageCtrl texPhotoPoster;

	[SerializeField]
	private PguiRawImageCtrl rawImgRareFrame;

	[SerializeField]
	private PguiTextCtrl textCardName;

	public class SetupParam
	{
		public SetupParam()
		{
			this.thfs = null;
		}

		public TreeHouseFurnitureStatic thfs;
	}
}
