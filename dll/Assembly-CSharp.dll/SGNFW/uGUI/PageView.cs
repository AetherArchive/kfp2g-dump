using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SGNFW.uGUI
{
	public class PageView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
	{
		public void Setup(int count)
		{
			this.Setup(count, this.prefab);
		}

		public void Setup(int count, Object obj)
		{
			this.Setup(count, new GameObject[]
			{
				Manager.Create(obj, this.contentRoot, null, "0", Layer.UI),
				Manager.Create(obj, this.contentRoot, null, "1", Layer.UI)
			});
		}

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

		public virtual void OnDrag(PointerEventData data)
		{
		}

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

		public Action<int, GameObject> onStart;

		public Action<int, GameObject> onChange;

		[SerializeField]
		protected GameObject prefab;

		[SerializeField]
		protected PageView.Direction direction;

		[SerializeField]
		protected RectTransform viewport;

		[SerializeField]
		protected RectTransform contentRoot;

		[SerializeField]
		protected float scrollDistance = 100f;

		[SerializeField]
		[Range(0f, 1f)]
		protected float scrollSpeed = 0.3f;

		private PageView.Move move;

		private int active;

		private int current;

		private int count;

		private RectTransform[] contents = new RectTransform[2];

		private Vector2[] moveTo = new Vector2[2];

		private float time;

		private bool down;

		private Vector2 downPos;

		protected enum Direction
		{
			Horizontal,
			Vertical
		}

		protected enum Move
		{
			None,
			Next,
			Back
		}
	}
}
