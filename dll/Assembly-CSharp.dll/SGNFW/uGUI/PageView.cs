using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SGNFW.uGUI
{
	// Token: 0x02000230 RID: 560
	public class PageView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x0600234B RID: 9035 RVA: 0x00196F6F File Offset: 0x0019516F
		public void Setup(int count)
		{
			this.Setup(count, this.prefab);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x00196F80 File Offset: 0x00195180
		public void Setup(int count, Object obj)
		{
			this.Setup(count, new GameObject[]
			{
				Manager.Create(obj, this.contentRoot, null, "0", Layer.UI),
				Manager.Create(obj, this.contentRoot, null, "1", Layer.UI)
			});
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x00196FC8 File Offset: 0x001951C8
		public void Setup(int count, GameObject[] gos)
		{
			this.count = count;
			for (int i = 0; i < gos.Length; i++)
			{
				this.contents[i] = gos[i].transform as RectTransform;
				Vector2 anchoredPosition = this.contents[i].anchoredPosition;
				if (this.direction == PageView.Direction.Horizontal)
				{
					anchoredPosition.x += this.viewport.rect.width * (float)i;
				}
				else
				{
					anchoredPosition.y += -this.viewport.rect.height * (float)i;
				}
				this.contents[i].anchoredPosition = anchoredPosition;
				if (i == 0)
				{
					this.contents[i].gameObject.SetActive(true);
				}
				else
				{
					this.contents[i].gameObject.SetActive(false);
				}
				if (this.onStart != null)
				{
					this.onStart(i, this.contents[i].gameObject);
				}
			}
			this.active = 0;
			this.current = 0;
			this.move = PageView.Move.None;
			this.down = false;
			if (this.onChange != null)
			{
				this.onChange(this.current, this.contents[0].gameObject);
			}
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x001970FC File Offset: 0x001952FC
		public void Play(bool forward)
		{
			if (this.move != PageView.Move.None)
			{
				return;
			}
			if (forward)
			{
				this.move = PageView.Move.Next;
			}
			else
			{
				this.move = PageView.Move.Back;
			}
			this.Exec();
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00197120 File Offset: 0x00195320
		public virtual void OnBeginDrag(PointerEventData data)
		{
			if (this.move != PageView.Move.None && !this.down)
			{
				return;
			}
			this.downPos = data.position;
			this.time = Time.realtimeSinceStartup;
			this.down = true;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x00197154 File Offset: 0x00195354
		public virtual void OnEndDrag(PointerEventData data)
		{
			if (this.move != PageView.Move.None && this.down)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.time < 1f)
			{
				Vector2 vector = data.position - this.downPos;
				float num = Mathf.Abs(vector.x);
				float num2 = Mathf.Abs(vector.y);
				if (this.direction == PageView.Direction.Horizontal && num > num2 && num > this.scrollDistance)
				{
					if (vector.x < 0f)
					{
						this.move = PageView.Move.Next;
					}
					else
					{
						this.move = PageView.Move.Back;
					}
				}
				else if (this.direction == PageView.Direction.Vertical && num < num2 && num2 > this.scrollDistance)
				{
					if (vector.y > 0f)
					{
						this.move = PageView.Move.Next;
					}
					else
					{
						this.move = PageView.Move.Back;
					}
				}
				this.Exec();
			}
			this.down = false;
			this.time = 0f;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x00197231 File Offset: 0x00195431
		public virtual void OnDrag(PointerEventData data)
		{
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x00197234 File Offset: 0x00195434
		protected void Exec()
		{
			if (this.move == PageView.Move.None)
			{
				return;
			}
			int num = this.active ^ 1;
			this.contents[num].gameObject.SetActive(true);
			Vector2 anchoredPosition = this.contents[num].anchoredPosition;
			if (this.move == PageView.Move.Next)
			{
				this.current++;
				if (this.current >= this.count)
				{
					this.current = 0;
				}
				if (this.direction == PageView.Direction.Horizontal)
				{
					anchoredPosition.x = this.viewport.rect.width;
					this.contents[num].anchoredPosition = anchoredPosition;
					this.moveTo[0] = this.contents[0].anchoredPosition;
					this.moveTo[1] = this.contents[1].anchoredPosition;
					Vector2[] array = this.moveTo;
					int num2 = 0;
					array[num2].x = array[num2].x - this.viewport.rect.width;
					Vector2[] array2 = this.moveTo;
					int num3 = 1;
					array2[num3].x = array2[num3].x - this.viewport.rect.width;
				}
				else
				{
					anchoredPosition.y = -this.viewport.rect.height;
					this.contents[num].anchoredPosition = anchoredPosition;
					this.moveTo[0] = this.contents[0].anchoredPosition;
					this.moveTo[1] = this.contents[1].anchoredPosition;
					Vector2[] array3 = this.moveTo;
					int num4 = 0;
					array3[num4].y = array3[num4].y + this.viewport.rect.height;
					Vector2[] array4 = this.moveTo;
					int num5 = 1;
					array4[num5].y = array4[num5].y + this.viewport.rect.height;
				}
			}
			else if (this.move == PageView.Move.Back)
			{
				this.current--;
				if (this.current < 0)
				{
					this.current = this.count - 1;
				}
				if (this.direction == PageView.Direction.Horizontal)
				{
					anchoredPosition.x = -this.viewport.rect.width;
					this.contents[num].anchoredPosition = anchoredPosition;
					this.moveTo[0] = this.contents[0].anchoredPosition;
					this.moveTo[1] = this.contents[1].anchoredPosition;
					Vector2[] array5 = this.moveTo;
					int num6 = 0;
					array5[num6].x = array5[num6].x + this.viewport.rect.width;
					Vector2[] array6 = this.moveTo;
					int num7 = 1;
					array6[num7].x = array6[num7].x + this.viewport.rect.width;
				}
				else
				{
					anchoredPosition.y = this.viewport.rect.height;
					this.contents[num].anchoredPosition = anchoredPosition;
					this.moveTo[0] = this.contents[0].anchoredPosition;
					this.moveTo[1] = this.contents[1].anchoredPosition;
					Vector2[] array7 = this.moveTo;
					int num8 = 0;
					array7[num8].y = array7[num8].y - this.viewport.rect.height;
					Vector2[] array8 = this.moveTo;
					int num9 = 1;
					array8[num9].y = array8[num9].y - this.viewport.rect.height;
				}
			}
			if (this.onChange != null)
			{
				this.onChange(this.current, this.contents[num].gameObject);
			}
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x001975CC File Offset: 0x001957CC
		private void Update()
		{
			if (this.move != PageView.Move.None)
			{
				float num = 1f - Mathf.Pow(1f - Mathf.Clamp01(this.scrollSpeed), Time.deltaTime / 0.016666668f);
				for (int i = 0; i < this.contents.Length; i++)
				{
					this.contents[i].anchoredPosition = Vector3.Lerp(this.contents[i].anchoredPosition, this.moveTo[i], num);
				}
				float num2;
				if (this.direction == PageView.Direction.Horizontal)
				{
					num2 = this.contents[0].anchoredPosition.x - this.moveTo[0].x;
				}
				else
				{
					num2 = this.contents[0].anchoredPosition.y - this.moveTo[0].y;
				}
				num2 = Mathf.Abs(num2);
				if (num2 < 0.5f)
				{
					this.move = PageView.Move.None;
					this.contents[this.active].gameObject.SetActive(false);
					this.contents[0].anchoredPosition = this.moveTo[0];
					this.contents[1].anchoredPosition = this.moveTo[1];
					this.active ^= 1;
				}
			}
		}

		// Token: 0x04001AA1 RID: 6817
		public Action<int, GameObject> onStart;

		// Token: 0x04001AA2 RID: 6818
		public Action<int, GameObject> onChange;

		// Token: 0x04001AA3 RID: 6819
		[SerializeField]
		protected GameObject prefab;

		// Token: 0x04001AA4 RID: 6820
		[SerializeField]
		protected PageView.Direction direction;

		// Token: 0x04001AA5 RID: 6821
		[SerializeField]
		protected RectTransform viewport;

		// Token: 0x04001AA6 RID: 6822
		[SerializeField]
		protected RectTransform contentRoot;

		// Token: 0x04001AA7 RID: 6823
		[SerializeField]
		protected float scrollDistance = 100f;

		// Token: 0x04001AA8 RID: 6824
		[SerializeField]
		[Range(0f, 1f)]
		protected float scrollSpeed = 0.3f;

		// Token: 0x04001AA9 RID: 6825
		private PageView.Move move;

		// Token: 0x04001AAA RID: 6826
		private int active;

		// Token: 0x04001AAB RID: 6827
		private int current;

		// Token: 0x04001AAC RID: 6828
		private int count;

		// Token: 0x04001AAD RID: 6829
		private RectTransform[] contents = new RectTransform[2];

		// Token: 0x04001AAE RID: 6830
		private Vector2[] moveTo = new Vector2[2];

		// Token: 0x04001AAF RID: 6831
		private float time;

		// Token: 0x04001AB0 RID: 6832
		private bool down;

		// Token: 0x04001AB1 RID: 6833
		private Vector2 downPos;

		// Token: 0x02001066 RID: 4198
		protected enum Direction
		{
			// Token: 0x04005BB2 RID: 23474
			Horizontal,
			// Token: 0x04005BB3 RID: 23475
			Vertical
		}

		// Token: 0x02001067 RID: 4199
		protected enum Move
		{
			// Token: 0x04005BB5 RID: 23477
			None,
			// Token: 0x04005BB6 RID: 23478
			Next,
			// Token: 0x04005BB7 RID: 23479
			Back
		}
	}
}
