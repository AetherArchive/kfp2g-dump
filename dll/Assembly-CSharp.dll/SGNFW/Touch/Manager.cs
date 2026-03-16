using System;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Touch
{
	public class Manager : Singleton<Manager>
	{
		private static event Manager.SingleAction eventSwipe;

		private static event Manager.SingleAction eventSwipeUp;

		private static event Manager.SingleAction eventSwipeDown;

		private static event Manager.SingleAction eventSwipeRight;

		private static event Manager.SingleAction eventSwipeLeft;

		private static event Manager.SingleAction eventMove;

		private static event Manager.SingleAction eventStart;

		private static event Manager.SingleAction eventStationary;

		private static event Manager.SingleAction eventRelease;

		private static event Manager.SingleAction eventTap;

		private static event Manager.SingleAction eventDoubleTap;

		private static event Manager.SingleAction eventLongPress;

		private static event Manager.DoubleAction eventMoveTwo;

		private static event Manager.DoubleAction eventStartTwo;

		private static event Manager.DoubleAction eventStopTwo;

		private static event Manager.DoubleAction eventPinch;

		private static event Manager.DoubleAction eventRotation;

		private static event Manager.WheelAction eventMouseWheel;

		public static bool IsSelected(Vector2 pos, Transform trans, Camera camera)
		{
			RaycastHit raycastHit;
			return Physics.Raycast(camera.ScreenPointToRay(pos), out raycastHit) && raycastHit.transform == trans;
		}

		public static void RegisterSwipe(Manager.SingleAction action)
		{
			Manager.eventSwipe += action;
		}

		public static void RegisterSwipeUp(Manager.SingleAction action)
		{
			Manager.eventSwipeUp += action;
		}

		public static void RegisterSwipeDown(Manager.SingleAction action)
		{
			Manager.eventSwipeDown += action;
		}

		public static void RegisterSwipeRight(Manager.SingleAction action)
		{
			Manager.eventSwipeRight += action;
		}

		public static void RegisterSwipeLeft(Manager.SingleAction action)
		{
			Manager.eventSwipeLeft += action;
		}

		public static void RegisterMove(Manager.SingleAction action)
		{
			Manager.eventMove += action;
		}

		public static void RegisterStart(Manager.SingleAction action)
		{
			Manager.eventStart += action;
		}

		public static void RegisterStationary(Manager.SingleAction action)
		{
			Manager.eventStationary += action;
		}

		public static void RegisterRelease(Manager.SingleAction action)
		{
			Manager.eventRelease += action;
		}

		public static void RegisterTap(Manager.SingleAction action)
		{
			Manager.eventTap += action;
		}

		public static void RegisterDoubleTap(Manager.SingleAction action)
		{
			Manager.eventDoubleTap += action;
		}

		public static void RegisterLongPress(Manager.SingleAction action)
		{
			Manager.eventLongPress += action;
		}

		public static void RegisterStartTwo(Manager.DoubleAction action)
		{
			Manager.eventStartTwo += action;
		}

		public static void RegisterMoveTwo(Manager.DoubleAction action)
		{
			Manager.eventMoveTwo += action;
		}

		public static void RegisterStopTwo(Manager.DoubleAction action)
		{
			Manager.eventStopTwo += action;
		}

		public static void RegisterPinch(Manager.DoubleAction action)
		{
			Manager.eventPinch += action;
		}

		public static void RegisterRotation(Manager.DoubleAction action)
		{
			Manager.eventRotation += action;
		}

		public static void RegisterMouseWheel(Manager.WheelAction action)
		{
			Manager.eventMouseWheel += action;
		}

		public static void UnRegisterSwipe(Manager.SingleAction action)
		{
			Manager.eventSwipe -= action;
		}

		public static void UnRegisterSwipeUp(Manager.SingleAction action)
		{
			Manager.eventSwipeUp -= action;
		}

		public static void UnRegisterSwipeDown(Manager.SingleAction action)
		{
			Manager.eventSwipeDown -= action;
		}

		public static void UnRegisterSwipeRight(Manager.SingleAction action)
		{
			Manager.eventSwipeRight -= action;
		}

		public static void UnRegisterSwipeLeft(Manager.SingleAction action)
		{
			Manager.eventSwipeLeft -= action;
		}

		public static void UnRegisterMove(Manager.SingleAction action)
		{
			Manager.eventMove -= action;
		}

		public static void UnRegisterStart(Manager.SingleAction action)
		{
			Manager.eventStart -= action;
		}

		public static void UnRegisterStationary(Manager.SingleAction action)
		{
			Manager.eventStationary -= action;
		}

		public static void UnRegisterRelease(Manager.SingleAction action)
		{
			Manager.eventRelease -= action;
		}

		public static void UnRegisterTap(Manager.SingleAction action)
		{
			Manager.eventTap -= action;
		}

		public static void UnRegisterDoubleTap(Manager.SingleAction action)
		{
			Manager.eventDoubleTap -= action;
		}

		public static void UnRegisterLongPress(Manager.SingleAction action)
		{
			Manager.eventLongPress -= action;
		}

		public static void UnRegisterStartTwo(Manager.DoubleAction action)
		{
			Manager.eventStartTwo -= action;
		}

		public static void UnRegisterMoveTwo(Manager.DoubleAction action)
		{
			Manager.eventMoveTwo -= action;
		}

		public static void UnRegisterStopTwo(Manager.DoubleAction action)
		{
			Manager.eventStopTwo -= action;
		}

		public static void UnRegisterPinch(Manager.DoubleAction action)
		{
			Manager.eventPinch -= action;
		}

		public static void UnRegisterRotation(Manager.DoubleAction action)
		{
			Manager.eventRotation -= action;
		}

		public static void UnRegisterMouseWheel(Manager.WheelAction action)
		{
			Manager.eventMouseWheel -= action;
		}

		public static void UnRegisterAll()
		{
			Manager.eventSwipe = null;
			Manager.eventSwipeUp = null;
			Manager.eventSwipeDown = null;
			Manager.eventSwipeRight = null;
			Manager.eventSwipeLeft = null;
			Manager.eventMove = null;
			Manager.eventRelease = null;
			Manager.eventStart = null;
			Manager.eventStationary = null;
			Manager.eventTap = null;
			Manager.eventDoubleTap = null;
			Manager.eventLongPress = null;
			Manager.eventStartTwo = null;
			Manager.eventMoveTwo = null;
			Manager.eventStopTwo = null;
			Manager.eventPinch = null;
			Manager.eventRotation = null;
			Manager.eventMouseWheel = null;
		}

		public bool IsSamePosition(Vector2 posA, Vector2 posB)
		{
			float num = 1280f;
			float num2 = 720f;
			if (Screen.height > Screen.width)
			{
				float num3 = num;
				num = num2;
				num2 = num3;
			}
			num = this.offsetTouch * (float)Screen.width / num;
			num2 = this.offsetTouch * (float)Screen.height / num2;
			return posA.x >= posB.x - num && posA.x <= posB.x + num && posA.y >= posB.y - num2 && posA.y <= posB.y + num2;
		}

		private void MouseDetection()
		{
			Vector2 vector = Input.mousePosition - this.lastPositionToDelta;
			Info info = this.CreateTouchInfoClass(Input.mousePosition, this.initPosition, vector);
			if (Input.GetMouseButtonDown(0))
			{
				this.twoInfo = new Info();
				Vector2 vector2 = new Vector2((float)(Screen.width / 4), (float)(Screen.height / 4));
				if (info.CurrentPosition.x > vector2.x * 2f)
				{
					vector2.x = -vector2.x;
				}
				if (info.CurrentPosition.y > vector2.y * 2f)
				{
					vector2.y = -vector2.y;
				}
				this.twoInfo.CurrentPosition = info.CurrentPosition + vector2;
				this.twoInfo.DeltaPosition = info.DeltaPosition;
				this.twoInfo.Direction = info.Direction;
				this.twoInfo.InitPosition = info.InitPosition;
				this.twoInfo.Speed = info.Speed;
				if (!Input.GetKey(KeyCode.LeftControl) || !this.CalculateStartTwoFinger(info, this.twoInfo))
				{
					this.CalculateStart(info);
				}
				this.lastPositionToDelta = Input.mousePosition;
			}
			else if (Input.GetMouseButton(0))
			{
				if (this.lastPositionToDelta == info.CurrentPosition)
				{
					if (this.timeToBeStopped < Time.time)
					{
						if (Input.GetKey(KeyCode.LeftControl))
						{
							this.CalculateStationaryTwoFinger(info, this.twoInfo);
						}
						else
						{
							this.CalculateStationary(info);
						}
					}
				}
				else
				{
					this.timeToBeStopped = Time.time + this.mouseTimeToBeStopped;
					if (!Input.GetKey(KeyCode.LeftControl) || !this.CalculateMoveTwoFinger(info, this.twoInfo))
					{
						this.CalculateMove(info);
					}
				}
				this.lastPositionToDelta = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				this.CalculateSwipe(info);
				this.CalculateRelease(info);
				this.initPosition = Vector2.zero;
			}
			else if (Input.GetMouseButtonDown(1))
			{
				if (!this.CalculateStartTwoFinger(info, info))
				{
					this.CalculateStart(info);
				}
				this.lastPositionToDelta = Input.mousePosition;
			}
			else if (Input.GetMouseButton(1))
			{
				if (this.lastPositionToDelta == info.CurrentPosition)
				{
					if (this.timeToBeStopped < Time.time && !this.CalculateStationaryTwoFinger(info, info))
					{
						this.CalculateStationary(info);
					}
				}
				else
				{
					this.timeToBeStopped = Time.time + this.mouseTimeToBeStopped;
					if (!this.CalculateMoveTwoFinger(info, info))
					{
						this.CalculateMove(info);
					}
				}
				this.lastPositionToDelta = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(1))
			{
				this.CalculateRelease(info);
				this.initPosition = Vector2.zero;
			}
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f && Manager.eventMouseWheel != null)
			{
				Manager.eventMouseWheel(info, axis);
			}
		}

		private void CalculateSwipe(Info finalPosition)
		{
			bool flag = false;
			if (finalPosition.CurrentPosition.x >= finalPosition.InitPosition.x + (float)this.minMoveToSwipe)
			{
				flag = true;
				if (Manager.eventSwipeRight != null)
				{
					Manager.eventSwipeRight(finalPosition);
				}
			}
			if (finalPosition.CurrentPosition.x < finalPosition.InitPosition.x - (float)this.minMoveToSwipe)
			{
				flag = true;
				if (Manager.eventSwipeLeft != null)
				{
					Manager.eventSwipeLeft(finalPosition);
				}
			}
			if (finalPosition.CurrentPosition.y >= finalPosition.InitPosition.y + (float)this.minMoveToSwipe)
			{
				flag = true;
				if (Manager.eventSwipeUp != null)
				{
					Manager.eventSwipeUp(finalPosition);
				}
			}
			if (finalPosition.CurrentPosition.y < finalPosition.InitPosition.y - (float)this.minMoveToSwipe)
			{
				flag = true;
				if (Manager.eventSwipeDown != null)
				{
					Manager.eventSwipeDown(finalPosition);
				}
			}
			if (Manager.eventSwipe != null && flag)
			{
				Manager.eventSwipe(finalPosition);
			}
		}

		private void CalculateStart(Info pos)
		{
			this.initPosition = Input.mousePosition;
			if (Manager.eventStart != null)
			{
				Manager.eventStart(pos);
			}
			if (Manager.eventTap != null)
			{
				this.timeTap = Time.time + this.elapsedTimeTap;
			}
			if (Manager.eventDoubleTap != null)
			{
				bool flag = false;
				if (this.timeDoubleTap >= Time.time)
				{
					if (this.IsSamePosition(pos.CurrentPosition, this.lastPosition))
					{
						Manager.eventDoubleTap(pos);
						this.simpleTap = false;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					this.timeDoubleTap = Time.time + this.elapsedTimeDoubleTap;
				}
			}
			if (Manager.eventLongPress != null)
			{
				this.timeLongTap = Time.time + this.timeLongTouch;
				this.simpleTap = false;
			}
			this.timeToBeStopped = Time.time + this.mouseTimeToBeStopped;
		}

		private bool CalculateStartTwoFinger(Info posA, Info posB)
		{
			this.initDistance = Vector2.Distance(posA.CurrentPosition, posB.CurrentPosition);
			this.initPosition = posA.CurrentPosition;
			if (Manager.eventStartTwo != null)
			{
				float num = Vector2.Distance(posA.CurrentPosition, posB.CurrentPosition);
				Manager.eventStartTwo(posA, posB, num, 0f);
				return true;
			}
			return false;
		}

		private void CalculateStationary(Info pos)
		{
			if (Manager.eventLongPress != null && this.IsSamePosition(this.initPosition, pos.CurrentPosition) && Time.time > this.timeLongTap)
			{
				Manager.eventLongPress(pos);
			}
			if (Manager.eventStationary != null)
			{
				Manager.eventStationary(pos);
			}
		}

		private bool CalculateStationaryTwoFinger(Info posA, Info posB)
		{
			this.initDistance = Vector2.Distance(posA.CurrentPosition, posB.CurrentPosition);
			if (Manager.eventStopTwo != null)
			{
				float num = Vector2.Distance(posA.CurrentPosition, posB.CurrentPosition);
				Manager.eventStopTwo(posA, posB, num, 0f);
			}
			return true;
		}

		private void CalculateMove(Info direction)
		{
			if (Manager.eventMove != null)
			{
				Manager.eventMove(direction);
			}
		}

		private bool CalculateMoveTwoFinger(Info posA, Info posB)
		{
			if (Manager.eventPinch != null || Manager.eventRotation != null || Manager.eventMoveTwo != null)
			{
				float num = Vector2.Distance(posA.CurrentPosition, posB.CurrentPosition);
				if (Manager.eventPinch != null)
				{
					float num2 = num - this.initDistance;
					if (num2 != this.initDistance)
					{
						Manager.eventPinch(posA, posB, num2, 0f);
					}
				}
				if (Manager.eventRotation != null)
				{
					Vector2 vector = posB.CurrentPosition - posA.CurrentPosition;
					Vector2 vector2 = posB.DeltaPosition - posA.DeltaPosition;
					if (Vector2.Angle(vector2, vector) > this.minAngleRotation)
					{
						Vector3 vector3 = Vector3.Cross(vector2, vector);
						Manager.eventRotation(posA, posB, num, vector3.z);
					}
				}
				if (Manager.eventMoveTwo != null)
				{
					Manager.eventMoveTwo(posA, posB, num, 0f);
				}
				return true;
			}
			return false;
		}

		private void CalculateRelease(Info pos)
		{
			if (Manager.eventRelease != null)
			{
				Manager.eventRelease(pos);
			}
			if (Manager.eventTap != null && this.timeTap >= Time.time && this.IsSamePosition(pos.CurrentPosition, pos.InitPosition))
			{
				if (Manager.eventDoubleTap == null)
				{
					Manager.eventTap(pos);
				}
				else
				{
					this.tapInfo = pos;
					this.simpleTap = true;
				}
			}
			this.lastPosition = this.initPosition;
			this.timeLongTap = 0f;
			this.timeToBeStopped = 0f;
		}

		private Info CreateTouchInfoClass(Vector2 pos, Vector2 initPos, Vector2 deltaPos)
		{
			return new Info
			{
				CurrentPosition = pos,
				InitPosition = initPos,
				DeltaPosition = deltaPos,
				Direction = (pos - initPos).normalized,
				Speed = deltaPos.magnitude / Time.deltaTime
			};
		}

		private void Start()
		{
			this.initPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
		}

		private void LateUpdate()
		{
			this.MouseDetection();
			if (Time.time > this.timeTap && Manager.eventTap != null && this.simpleTap)
			{
				this.simpleTap = false;
				Manager.eventTap(this.tapInfo);
			}
		}

		[SerializeField]
		private int minMoveToSwipe = 50;

		[SerializeField]
		private float timeLongTouch = 1f;

		[SerializeField]
		private float offsetTouch = 11f;

		[SerializeField]
		private float elapsedTimeDoubleTap = 0.5f;

		[SerializeField]
		private float elapsedTimeTap = 0.35f;

		[SerializeField]
		private float minAngleRotation = 50f;

		[SerializeField]
		private float mouseTimeToBeStopped = 0.1f;

		private Vector2 initPosition;

		private Vector2 lastPosition;

		private Vector2 lastPositionToDelta;

		private float initDistance;

		private float timeTap;

		private float timeDoubleTap;

		private float timeLongTap;

		private float timeToBeStopped;

		private Info twoInfo;

		private bool simpleTap;

		private Info tapInfo;

		public delegate void SingleAction(Info finger);

		public delegate void DoubleAction(Info fingerA, Info fingerB, float distance, float rotation);

		public delegate void WheelAction(Info finger, float delta);
	}
}
