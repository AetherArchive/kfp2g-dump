using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000226 RID: 550
	[RequireComponent(typeof(RectTransform))]
	public class AlphaMask : MonoBehaviour
	{
		// Token: 0x06002315 RID: 8981 RVA: 0x00195E08 File Offset: 0x00194008
		public void Apply(GameObject item)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			Graphic[] componentsInChildren = item.GetComponentsInChildren<Graphic>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.ReplaceShaderToAlphaMask(componentsInChildren[i].material);
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x00195E50 File Offset: 0x00194050
		private void Initialize()
		{
			Canvas.ForceUpdateCanvases();
			Canvas canvas = null;
			Transform transform = base.transform;
			while (transform != null)
			{
				Canvas component = transform.GetComponent<Canvas>();
				if (component != null)
				{
					canvas = component;
				}
				transform = transform.parent;
			}
			RectTransform rectTransform = canvas.transform as RectTransform;
			RectTransform rectTransform2 = base.transform as RectTransform;
			this.scale.x = rectTransform2.rect.width;
			this.scale.y = rectTransform2.rect.height;
			this.offset = Vector2.zero;
			transform = base.transform;
			while (transform != null)
			{
				this.scale.x = this.scale.x * transform.localScale.x;
				this.scale.y = this.scale.y * transform.localScale.y;
				this.offset.x = this.offset.x + -transform.localPosition.x;
				this.offset.y = this.offset.y + -transform.localPosition.y;
				if (transform == canvas.transform)
				{
					break;
				}
				transform = transform.parent;
			}
			float num = rectTransform.rect.width * 0.5f;
			float num2 = rectTransform.rect.height * 0.5f;
			float num3 = num + rectTransform2.rect.x + rectTransform2.rect.width * rectTransform2.pivot.x + this.offset.x;
			float num4 = num2 + rectTransform2.rect.y + rectTransform2.rect.height * rectTransform2.pivot.y + this.offset.y;
			Vector3 vector = canvas.worldCamera.ViewportToWorldPoint(new Vector3(num3 / rectTransform.rect.width, num4 / rectTransform.rect.height, canvas.planeDistance));
			this.scale.x = 1f / this.scale.x;
			this.scale.y = 1f / this.scale.y;
			this.offset.x = vector.x;
			this.offset.y = vector.y;
			this.offset.x = this.offset.x * this.scale.x + rectTransform2.pivot.x;
			this.offset.y = this.offset.y * this.scale.y + rectTransform2.pivot.y;
			this.initialized = true;
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0019611C File Offset: 0x0019431C
		private Material ReplaceShaderToAlphaMask(Material material)
		{
			Shader shader = Shader.Find("Hidden/" + material.shader.name);
			if (shader == null)
			{
				return material;
			}
			for (int i = 0; i < this.matList.Count; i++)
			{
				if (this.matList[i].shader.name == shader.name)
				{
					return this.matList[i];
				}
			}
			Material material2 = Object.Instantiate<Material>(material);
			material2.shader = shader;
			material2.SetTexture("_AlphaTex", this.texture);
			material2.SetTextureOffset("_AlphaTex", this.offset);
			material2.SetTextureScale("_AlphaTex", this.scale);
			this.matList.Add(material2);
			return material2;
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x001961E3 File Offset: 0x001943E3
		private void Awake()
		{
			this.matList = new List<Material>();
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x001961F0 File Offset: 0x001943F0
		private void OnDestroy()
		{
			this.matList.Clear();
			this.matList = null;
		}

		// Token: 0x04001A78 RID: 6776
		[SerializeField]
		private Texture2D texture;

		// Token: 0x04001A79 RID: 6777
		private Vector2 offset;

		// Token: 0x04001A7A RID: 6778
		private Vector2 scale;

		// Token: 0x04001A7B RID: 6779
		private List<Material> matList;

		// Token: 0x04001A7C RID: 6780
		private bool initialized;
	}
}
