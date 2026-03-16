using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[RequireComponent(typeof(ScrollRect))]
	public class TableViewController<T> : MonoBehaviour
	{
		public RectTransform CachedRectTransform
		{
			get
			{
				if (this.cachedRectTransform == null)
				{
					this.cachedRectTransform = base.GetComponent<RectTransform>();
				}
				return this.cachedRectTransform;
			}
		}

		public ScrollRect CachedScrollRect
		{
			get
			{
				if (this.cachedScrollRect == null)
				{
					this.cachedScrollRect = base.GetComponent<ScrollRect>();
					this.orgRect = this.cachedScrollRect.content.rect;
				}
				return this.cachedScrollRect;
			}
		}

		public TableViewController<T>.Direction ScrollDirection
		{
			get
			{
				return this.direction;
			}
		}

		public void Clear()
		{
			foreach (object obj in this.CachedScrollRect.content)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			this.cells.Clear();
		}

		protected virtual float CellSizeAtIndex(int index)
		{
			return 0f;
		}

		protected void UpdateContentSize()
		{
			float num = 0f;
			for (int i = 0; i < this.tableData.Count; i++)
			{
				num += this.CellSizeAtIndex(i);
				if (i > 0)
				{
					num += this.spacing;
				}
			}
			Vector2 sizeDelta = this.CachedScrollRect.content.sizeDelta;
			RectTransform content = this.CachedScrollRect.content;
			if (this.direction == TableViewController<T>.Direction.Horizontal)
			{
				sizeDelta.x = (float)this.padding.left + num + (float)this.padding.right;
				if (content.anchorMax.x != content.anchorMin.x)
				{
					sizeDelta.x -= this.orgRect.width;
				}
			}
			else if (this.direction == TableViewController<T>.Direction.Vertical)
			{
				sizeDelta.y = (float)this.padding.top + num + (float)this.padding.bottom;
				if (content.anchorMax.y != content.anchorMin.y)
				{
					sizeDelta.y -= this.orgRect.height;
				}
			}
			this.CachedScrollRect.content.sizeDelta = sizeDelta;
		}

		private TableViewCell<T> CreateCellForIndex(int index)
		{
			TableViewCell<T> component = Manager.Create(this.cellBase, this.CachedScrollRect.content, null, null, Layer.UI).GetComponent<TableViewCell<T>>();
			if (this.onCreate != null)
			{
				this.onCreate(component, index);
			}
			this.UpdateCellForIndex(component, index);
			this.cells.AddLast(component);
			return component;
		}

		private void UpdateCellForIndex(TableViewCell<T> cell, int index)
		{
			cell.DataIndex = index;
			if (cell.DataIndex >= 0 && cell.DataIndex <= this.tableData.Count - 1)
			{
				cell.gameObject.SetActive(true);
				cell.UpdateContent(this.tableData[cell.DataIndex], index);
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					cell.Width = this.CellSizeAtIndex(cell.DataIndex);
					return;
				}
				if (this.direction == TableViewController<T>.Direction.Vertical)
				{
					cell.Height = this.CellSizeAtIndex(cell.DataIndex);
					return;
				}
			}
			else
			{
				cell.gameObject.SetActive(false);
			}
		}

		private void UpdateVisibleRect()
		{
			this.visibleRect.x = -this.CachedScrollRect.content.anchoredPosition.x;
			this.visibleRect.y = -this.CachedScrollRect.content.anchoredPosition.y;
			this.visibleRect.width = this.CachedRectTransform.rect.width;
			this.visibleRect.height = this.CachedRectTransform.rect.height;
		}

		protected void UpdateContents()
		{
			this.UpdateContentSize();
			this.UpdateVisibleRect();
			if (this.cells.Count <= 0)
			{
				float num = (float)(-(float)this.padding.top);
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					num = (float)this.padding.left;
				}
				int i = 0;
				while (i < this.tableData.Count)
				{
					float num2 = this.CellSizeAtIndex(i);
					float num3 = num + -num2;
					if (this.direction == TableViewController<T>.Direction.Horizontal)
					{
						num3 = num + num2;
					}
					float num4 = this.visibleRect.y - this.visibleRect.height;
					float num5 = this.visibleRect.y;
					if (this.direction == TableViewController<T>.Direction.Horizontal)
					{
						num4 = this.visibleRect.x;
						num5 = this.visibleRect.x + this.visibleRect.width;
					}
					if ((num <= num5 && num >= num4) || (num3 <= num5 && num3 >= num4))
					{
						TableViewCell<T> tableViewCell = this.CreateCellForIndex(i);
						if (this.direction == TableViewController<T>.Direction.Horizontal)
						{
							tableViewCell.Left = num;
							break;
						}
						if (this.direction == TableViewController<T>.Direction.Vertical)
						{
							tableViewCell.Top = num;
							break;
						}
						break;
					}
					else
					{
						if (this.direction == TableViewController<T>.Direction.Horizontal)
						{
							num = num3 - this.spacing;
						}
						else if (this.direction == TableViewController<T>.Direction.Vertical)
						{
							num = num3 + this.spacing;
						}
						i++;
					}
				}
				this.FillVisibleRectWithCells();
				return;
			}
			LinkedListNode<TableViewCell<T>> linkedListNode = this.cells.First;
			this.UpdateCellForIndex(linkedListNode.Value, linkedListNode.Value.DataIndex);
			for (linkedListNode = linkedListNode.Next; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.UpdateCellForIndex(linkedListNode.Value, linkedListNode.Previous.Value.DataIndex + 1);
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					linkedListNode.Value.Left = linkedListNode.Previous.Value.Right + this.spacing;
				}
				else if (this.direction == TableViewController<T>.Direction.Vertical)
				{
					linkedListNode.Value.Top = linkedListNode.Previous.Value.Bottom - this.spacing;
				}
			}
			this.FillVisibleRectWithCells();
		}

		private void FillVisibleRectWithCells()
		{
			if (this.cells.Count <= 0)
			{
				return;
			}
			TableViewCell<T> tableViewCell = this.cells.Last.Value;
			int num = tableViewCell.DataIndex + 1;
			float nextCellStart = tableViewCell.Bottom - this.spacing;
			if (this.direction == TableViewController<T>.Direction.Horizontal)
			{
				nextCellStart = tableViewCell.Right + this.spacing;
			}
			Func<bool> func = delegate
			{
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					return nextCellStart <= this.visibleRect.x + this.visibleRect.width;
				}
				return this.direction == TableViewController<T>.Direction.Vertical && nextCellStart >= this.visibleRect.y - this.visibleRect.height;
			};
			while (num < this.tableData.Count && func())
			{
				TableViewCell<T> tableViewCell2 = this.CreateCellForIndex(num);
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					tableViewCell2.Left = nextCellStart;
				}
				else if (this.direction == TableViewController<T>.Direction.Vertical)
				{
					tableViewCell2.Top = nextCellStart;
				}
				tableViewCell = tableViewCell2;
				num = tableViewCell.DataIndex + 1;
				if (this.direction == TableViewController<T>.Direction.Horizontal)
				{
					nextCellStart = tableViewCell.Right + this.spacing;
				}
				else if (this.direction == TableViewController<T>.Direction.Vertical)
				{
					nextCellStart = tableViewCell.Bottom - this.spacing;
				}
			}
		}

		private void OnScrollPosChanged(Vector2 scrollPos)
		{
			this.UpdateVisibleRect();
			if (this.direction == TableViewController<T>.Direction.Horizontal)
			{
				this.ReuseCells((scrollPos.x > this.prevScrollPos.x) ? 1 : (-1));
			}
			else if (this.direction == TableViewController<T>.Direction.Vertical)
			{
				this.ReuseCells((scrollPos.y < this.prevScrollPos.y) ? 1 : (-1));
			}
			this.prevScrollPos = scrollPos;
		}

		private void ReuseCells(int scrollDirection)
		{
			if (this.cells.Count <= 0)
			{
				return;
			}
			if (scrollDirection > 0)
			{
				TableViewCell<T> tableViewCell = this.cells.First.Value;
				while ((this.direction == TableViewController<T>.Direction.Vertical && tableViewCell.Bottom > this.visibleRect.y) || (this.direction == TableViewController<T>.Direction.Horizontal && tableViewCell.Right < this.visibleRect.x))
				{
					TableViewCell<T> value = this.cells.Last.Value;
					this.UpdateCellForIndex(tableViewCell, value.DataIndex + 1);
					if (this.direction == TableViewController<T>.Direction.Horizontal)
					{
						tableViewCell.Left = value.Right + this.spacing;
					}
					else if (this.direction == TableViewController<T>.Direction.Vertical)
					{
						tableViewCell.Top = value.Bottom - this.spacing;
					}
					this.cells.AddLast(tableViewCell);
					this.cells.RemoveFirst();
					tableViewCell = this.cells.First.Value;
				}
				this.FillVisibleRectWithCells();
				return;
			}
			if (scrollDirection < 0)
			{
				TableViewCell<T> tableViewCell2 = this.cells.Last.Value;
				while ((this.direction == TableViewController<T>.Direction.Vertical && tableViewCell2.Top < this.visibleRect.y - this.visibleRect.height) || (this.direction == TableViewController<T>.Direction.Horizontal && tableViewCell2.Left > this.visibleRect.x + this.visibleRect.width))
				{
					TableViewCell<T> value2 = this.cells.First.Value;
					this.UpdateCellForIndex(tableViewCell2, value2.DataIndex - 1);
					if (this.direction == TableViewController<T>.Direction.Horizontal)
					{
						tableViewCell2.Right = value2.Left - this.spacing;
					}
					else if (this.direction == TableViewController<T>.Direction.Vertical)
					{
						tableViewCell2.Bottom = value2.Top + this.spacing;
					}
					this.cells.AddFirst(tableViewCell2);
					this.cells.RemoveLast();
					tableViewCell2 = this.cells.Last.Value;
				}
			}
		}

		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
			this.CachedScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollPosChanged));
			this.direction = (this.CachedScrollRect.vertical ? TableViewController<T>.Direction.Vertical : TableViewController<T>.Direction.Horizontal);
			if (this.direction == TableViewController<T>.Direction.Horizontal)
			{
				this.CachedScrollRect.vertical = false;
				return;
			}
			if (this.direction == TableViewController<T>.Direction.Vertical)
			{
				this.CachedScrollRect.horizontal = false;
			}
		}

		public Action<TableViewCell<T>, int> onCreate;

		[SerializeField]
		private RectOffset padding;

		[SerializeField]
		private float spacing;

		[SerializeField]
		private GameObject cellBase;

		protected List<T> tableData = new List<T>();

		private RectTransform cachedRectTransform;

		private ScrollRect cachedScrollRect;

		private Rect orgRect;

		private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();

		private Rect visibleRect;

		private Vector2 prevScrollPos;

		private TableViewController<T>.Direction direction;

		public enum Direction
		{
			Vertical,
			Horizontal
		}
	}
}
