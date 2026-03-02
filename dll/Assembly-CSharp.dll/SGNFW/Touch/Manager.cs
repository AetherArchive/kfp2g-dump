using System;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Touch
{
	// Token: 0x0200023D RID: 573
	public class Manager : Singleton<Manager>
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06002400 RID: 9216 RVA: 0x0019B08C File Offset: 0x0019928C
		// (remove) Token: 0x06002401 RID: 9217 RVA: 0x0019B0C0 File Offset: 0x001992C0
		private static event Manager.SingleAction eventSwipe;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06002402 RID: 9218 RVA: 0x0019B0F4 File Offset: 0x001992F4
		// (remove) Token: 0x06002403 RID: 9219 RVA: 0x0019B128 File Offset: 0x00199328
		private static event Manager.SingleAction eventSwipeUp;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06002404 RID: 9220 RVA: 0x0019B15C File Offset: 0x0019935C
		// (remove) Token: 0x06002405 RID: 9221 RVA: 0x0019B190 File Offset: 0x00199390
		private static event Manager.SingleAction eventSwipeDown;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06002406 RID: 9222 RVA: 0x0019B1C4 File Offset: 0x001993C4
		// (remove) Token: 0x06002407 RID: 9223 RVA: 0x0019B1F8 File Offset: 0x001993F8
		private static event Manager.SingleAction eventSwipeRight;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06002408 RID: 9224 RVA: 0x0019B22C File Offset: 0x0019942C
		// (remove) Token: 0x06002409 RID: 9225 RVA: 0x0019B260 File Offset: 0x00199460
		private static event Manager.SingleAction eventSwipeLeft;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600240A RID: 9226 RVA: 0x0019B294 File Offset: 0x00199494
		// (remove) Token: 0x0600240B RID: 9227 RVA: 0x0019B2C8 File Offset: 0x001994C8
		private static event Manager.SingleAction eventMove;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600240C RID: 9228 RVA: 0x0019B2FC File Offset: 0x001994FC
		// (remove) Token: 0x0600240D RID: 9229 RVA: 0x0019B330 File Offset: 0x00199530
		private static event Manager.SingleAction eventStart;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600240E RID: 9230 RVA: 0x0019B364 File Offset: 0x00199564
		// (remove) Token: 0x0600240F RID: 9231 RVA: 0x0019B398 File Offset: 0x00199598
		private static event Manager.SingleAction eventStationary;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06002410 RID: 9232 RVA: 0x0019B3CC File Offset: 0x001995CC
		// (remove) Token: 0x06002411 RID: 9233 RVA: 0x0019B400 File Offset: 0x00199600
		private static event Manager.SingleAction eventRelease;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06002412 RID: 9234 RVA: 0x0019B434 File Offset: 0x00199634
		// (remove) Token: 0x06002413 RID: 9235 RVA: 0x0019B468 File Offset: 0x00199668
		private static event Manager.SingleAction eventTap;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06002414 RID: 9236 RVA: 0x0019B49C File Offset: 0x0019969C
		// (remove) Token: 0x06002415 RID: 9237 RVA: 0x0019B4D0 File Offset: 0x001996D0
		private static event Manager.SingleAction eventDoubleTap;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06002416 RID: 9238 RVA: 0x0019B504 File Offset: 0x00199704
		// (remove) Token: 0x06002417 RID: 9239 RVA: 0x0019B538 File Offset: 0x00199738
		private static event Manager.SingleAction eventLongPress;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06002418 RID: 9240 RVA: 0x0019B56C File Offset: 0x0019976C
		// (remove) Token: 0x06002419 RID: 9241 RVA: 0x0019B5A0 File Offset: 0x001997A0
		private static event Manager.DoubleAction eventMoveTwo;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600241A RID: 9242 RVA: 0x0019B5D4 File Offset: 0x001997D4
		// (remove) Token: 0x0600241B RID: 9243 RVA: 0x0019B608 File Offset: 0x00199808
		private static event Manager.DoubleAction eventStartTwo;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600241C RID: 9244 RVA: 0x0019B63C File Offset: 0x0019983C
		// (remove) Token: 0x0600241D RID: 9245 RVA: 0x0019B670 File Offset: 0x00199870
		private static event Manager.DoubleAction eventStopTwo;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600241E RID: 9246 RVA: 0x0019B6A4 File Offset: 0x001998A4
		// (remove) Token: 0x0600241F RID: 9247 RVA: 0x0019B6D8 File Offset: 0x001998D8
		private static event Manager.DoubleAction eventPinch;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06002420 RID: 9248 RVA: 0x0019B70C File Offset: 0x0019990C
		// (remove) Token: 0x06002421 RID: 9249 RVA: 0x0019B740 File Offset: 0x00199940
		private static event Manager.DoubleAction eventRotation;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06002422 RID: 9250 RVA: 0x0019B774 File Offset: 0x00199974
		// (remove) Token: 0x06002423 RID: 9251 RVA: 0x0019B7A8 File Offset: 0x001999A8
		private static event Manager.WheelAction eventMouseWheel;

		// Token: 0x06002424 RID: 9252 RVA: 0x0019B7DC File Offset: 0x001999DC
		public static bool IsSelected(Vector2 pos, Transform trans, Camera camera)
		{
			RaycastHit raycastHit;
			return Physics.Raycast(camera.ScreenPointToRay(pos), out raycastHit) && raycastHit.transform == trans;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0019B810 File Offset: 0x00199A10
		public static void RegisterSwipe(Manager.SingleAction action)
		{
			Manager.eventSwipe += action;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0019B818 File Offset: 0x00199A18
		public static void RegisterSwipeUp(Manager.SingleAction action)
		{
			Manager.eventSwipeUp += action;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0019B820 File Offset: 0x00199A20
		public static void RegisterSwipeDown(Manager.SingleAction action)
		{
			Manager.eventSwipeDown += action;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0019B828 File Offset: 0x00199A28
		public static void RegisterSwipeRight(Manager.SingleAction action)
		{
			Manager.eventSwipeRight += action;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0019B830 File Offset: 0x00199A30
		public static void RegisterSwipeLeft(Manager.SingleAction action)
		{
			Manager.eventSwipeLeft += action;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0019B838 File Offset: 0x00199A38
		public static void RegisterMove(Manager.SingleAction action)
		{
			Manager.eventMove += action;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0019B840 File Offset: 0x00199A40
		public static void RegisterStart(Manager.SingleAction action)
		{
			Manager.eventStart += action;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0019B848 File Offset: 0x00199A48
		public static void RegisterStationary(Manager.SingleAction action)
		{
			Manager.eventStationary += action;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0019B850 File Offset: 0x00199A50
		public static void RegisterRelease(Manager.SingleAction action)
		{
			Manager.eventRelease += action;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0019B858 File Offset: 0x00199A58
		public static void RegisterTap(Manager.SingleAction action)
		{
			Manager.eventTap += action;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0019B860 File Offset: 0x00199A60
		public static void RegisterDoubleTap(Manager.SingleAction action)
		{
			Manager.eventDoubleTap += action;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0019B868 File Offset: 0x00199A68
		public static void RegisterLongPress(Manager.SingleAction action)
		{
			Manager.eventLongPress += action;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0019B870 File Offset: 0x00199A70
		public static void RegisterStartTwo(Manager.DoubleAction action)
		{
			Manager.eventStartTwo += action;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0019B878 File Offset: 0x00199A78
		public static void RegisterMoveTwo(Manager.DoubleAction action)
		{
			Manager.eventMoveTwo += action;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0019B880 File Offset: 0x00199A80
		public static void RegisterStopTwo(Manager.DoubleAction action)
		{
			Manager.eventStopTwo += action;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0019B888 File Offset: 0x00199A88
		public static void RegisterPinch(Manager.DoubleAction action)
		{
			Manager.eventPinch += action;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0019B890 File Offset: 0x00199A90
		public static void RegisterRotation(Manager.DoubleAction action)
		{
			Manager.eventRotation += action;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0019B898 File Offset: 0x00199A98
		public static void RegisterMouseWheel(Manager.WheelAction action)
		{
			Manager.eventMouseWheel += action;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0019B8A0 File Offset: 0x00199AA0
		public static void UnRegisterSwipe(Manager.SingleAction action)
		{
			Manager.eventSwipe -= action;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0019B8A8 File Offset: 0x00199AA8
		public static void UnRegisterSwipeUp(Manager.SingleAction action)
		{
			Manager.eventSwipeUp -= action;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0019B8B0 File Offset: 0x00199AB0
		public static void UnRegisterSwipeDown(Manager.SingleAction action)
		{
			Manager.eventSwipeDown -= action;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0019B8B8 File Offset: 0x00199AB8
		public static void UnRegisterSwipeRight(Manager.SingleAction action)
		{
			Manager.eventSwipeRight -= action;
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0019B8C0 File Offset: 0x00199AC0
		public static void UnRegisterSwipeLeft(Manager.SingleAction action)
		{
			Manager.eventSwipeLeft -= action;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0019B8C8 File Offset: 0x00199AC8
		public static void UnRegisterMove(Manager.SingleAction action)
		{
			Manager.eventMove -= action;
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0019B8D0 File Offset: 0x00199AD0
		public static void UnRegisterStart(Manager.SingleAction action)
		{
			Manager.eventStart -= action;
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0019B8D8 File Offset: 0x00199AD8
		public static void UnRegisterStationary(Manager.SingleAction action)
		{
			Manager.eventStationary -= action;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0019B8E0 File Offset: 0x00199AE0
		public static void UnRegisterRelease(Manager.SingleAction action)
		{
			Manager.eventRelease -= action;
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0019B8E8 File Offset: 0x00199AE8
		public static void UnRegisterTap(Manager.SingleAction action)
		{
			Manager.eventTap -= action;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0019B8F0 File Offset: 0x00199AF0
		public static void UnRegisterDoubleTap(Manager.SingleAction action)
		{
			Manager.eventDoubleTap -= action;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0019B8F8 File Offset: 0x00199AF8
		public static void UnRegisterLongPress(Manager.SingleAction action)
		{
			Manager.eventLongPress -= action;
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0019B900 File Offset: 0x00199B00
		public static void UnRegisterStartTwo(Manager.DoubleAction action)
		{
			Manager.eventStartTwo -= action;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0019B908 File Offset: 0x00199B08
		public static void UnRegisterMoveTwo(Manager.DoubleAction action)
		{
			Manager.eventMoveTwo -= action;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0019B910 File Offset: 0x00199B10
		public static void UnRegisterStopTwo(Manager.DoubleAction action)
		{
			Manager.eventStopTwo -= action;
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0019B918 File Offset: 0x00199B18
		public static void UnRegisterPinch(Manager.DoubleAction action)
		{
			Manager.eventPinch -= action;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0019B920 File Offset: 0x00199B20
		public static void UnRegisterRotation(Manager.DoubleAction action)
		{
			Manager.eventRotation -= action;
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0019B928 File Offset: 0x00199B28
		public static void UnRegisterMouseWheel(Manager.WheelAction action)
		{
			Manager.eventMouseWheel -= action;
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0019B930 File Offset: 0x00199B30
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

		// Token: 0x0600244A RID: 9290 RVA: 0x0019B9AC File Offset: 0x00199BAC
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

		// Token: 0x0600244B RID: 9291 RVA: 0x0019BA38 File Offset: 0x00199C38
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

		// Token: 0x0600244C RID: 9292 RVA: 0x0019BD20 File Offset: 0x00199F20
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

		// Token: 0x0600244D RID: 9293 RVA: 0x0019BE18 File Offset: 0x0019A018
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

		// Token: 0x0600244E RID: 9294 RVA: 0x0019BEEC File Offset: 0x0019A0EC
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

		// Token: 0x0600244F RID: 9295 RVA: 0x0019BF4C File Offset: 0x0019A14C
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

		// Token: 0x06002450 RID: 9296 RVA: 0x0019BFA0 File Offset: 0x0019A1A0
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

		// Token: 0x06002451 RID: 9297 RVA: 0x0019BFF0 File Offset: 0x0019A1F0
		private void CalculateMove(Info direction)
		{
			if (Manager.eventMove != null)
			{
				Manager.eventMove(direction);
			}
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0019C004 File Offset: 0x0019A204
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

		// Token: 0x06002453 RID: 9299 RVA: 0x0019C0E4 File Offset: 0x0019A2E4
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

		// Token: 0x06002454 RID: 9300 RVA: 0x0019C170 File Offset: 0x0019A370
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

		// Token: 0x06002455 RID: 9301 RVA: 0x0019C1BF File Offset: 0x0019A3BF
		private void Start()
		{
			this.initPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0019C1D7 File Offset: 0x0019A3D7
		private void LateUpdate()
		{
			this.MouseDetection();
			if (Time.time > this.timeTap && Manager.eventTap != null && this.simpleTap)
			{
				this.simpleTap = false;
				Manager.eventTap(this.tapInfo);
			}
		}

		// Token: 0x04001B18 RID: 6936
		[SerializeField]
		private int minMoveToSwipe = 50;

		// Token: 0x04001B19 RID: 6937
		[SerializeField]
		private float timeLongTouch = 1f;

		// Token: 0x04001B1A RID: 6938
		[SerializeField]
		private float offsetTouch = 11f;

		// Token: 0x04001B1B RID: 6939
		[SerializeField]
		private float elapsedTimeDoubleTap = 0.5f;

		// Token: 0x04001B1C RID: 6940
		[SerializeField]
		private float elapsedTimeTap = 0.35f;

		// Token: 0x04001B1D RID: 6941
		[SerializeField]
		private float minAngleRotation = 50f;

		// Token: 0x04001B1E RID: 6942
		[SerializeField]
		private float mouseTimeToBeStopped = 0.1f;

		// Token: 0x04001B1F RID: 6943
		private Vector2 initPosition;

		// Token: 0x04001B20 RID: 6944
		private Vector2 lastPosition;

		// Token: 0x04001B21 RID: 6945
		private Vector2 lastPositionToDelta;

		// Token: 0x04001B22 RID: 6946
		private float initDistance;

		// Token: 0x04001B23 RID: 6947
		private float timeTap;

		// Token: 0x04001B24 RID: 6948
		private float timeDoubleTap;

		// Token: 0x04001B25 RID: 6949
		private float timeLongTap;

		// Token: 0x04001B26 RID: 6950
		private float timeToBeStopped;

		// Token: 0x04001B27 RID: 6951
		private Info twoInfo;

		// Token: 0x04001B28 RID: 6952
		private bool simpleTap;

		// Token: 0x04001B29 RID: 6953
		private Info tapInfo;

		// Token: 0x02001079 RID: 4217
		// (Invoke) Token: 0x0600530B RID: 21259
		public delegate void SingleAction(Info finger);

		// Token: 0x0200107A RID: 4218
		// (Invoke) Token: 0x0600530F RID: 21263
		public delegate void DoubleAction(Info fingerA, Info fingerB, float distance, float rotation);

		// Token: 0x0200107B RID: 4219
		// (Invoke) Token: 0x06005313 RID: 21267
		public delegate void WheelAction(Info finger, float delta);
	}
}
