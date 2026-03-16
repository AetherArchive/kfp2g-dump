using System;
using UnityEngine;

namespace SGNFW.Touch
{
	public class Info
	{
		public Vector2 CurrentPosition
		{
			get
			{
				return this.currentPosition;
			}
			set
			{
				this.currentPosition = value;
			}
		}

		public Vector2 InitPosition
		{
			get
			{
				return this.initPosition;
			}
			set
			{
				this.initPosition = value;
			}
		}

		public Vector2 Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		public Vector2 DeltaPosition
		{
			get
			{
				return this.deltaPosition;
			}
			set
			{
				this.deltaPosition = value;
			}
		}

		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		public Info()
		{
			this.currentPosition = Vector2.zero;
			this.initPosition = Vector2.zero;
			this.direction = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.speed = 0f;
		}

		private Vector2 currentPosition;

		private Vector2 initPosition;

		private Vector2 direction;

		private Vector2 deltaPosition;

		private float speed;
	}
}
