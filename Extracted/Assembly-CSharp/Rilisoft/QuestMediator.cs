using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000728 RID: 1832
	internal sealed class QuestMediator
	{
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x00155D18 File Offset: 0x00153F18
		public static QuestEvents Events
		{
			get
			{
				return QuestMediator._eventSource;
			}
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x00155D20 File Offset: 0x00153F20
		public static void NotifyWin(ConnectSceneNGUIController.RegimGame mode, string map)
		{
			WinEventArgs winEventArgs = new WinEventArgs
			{
				Mode = mode,
				Map = (map ?? string.Empty)
			};
			try
			{
				QuestMediator._eventSource.RaiseWin(winEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyWin: {0}", new object[]
				{
					winEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x00155D9C File Offset: 0x00153F9C
		public static void NotifyKillOtherPlayer(ConnectSceneNGUIController.RegimGame mode, ShopNGUIController.CategoryNames weaponSlot, bool headshot = false, bool grenade = false, bool revenge = false, bool isInvisible = false, bool turretKill = false)
		{
			KillOtherPlayerEventArgs killOtherPlayerEventArgs = new KillOtherPlayerEventArgs
			{
				Mode = mode,
				WeaponSlot = weaponSlot,
				Headshot = headshot,
				Grenade = grenade,
				Revenge = revenge,
				IsInvisible = isInvisible
			};
			try
			{
				QuestMediator._eventSource.RaiseKillOtherPlayer(killOtherPlayerEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillOtherPlayer: {0}", new object[]
				{
					killOtherPlayerEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00155E2C File Offset: 0x0015402C
		public static void NotifyKillOtherPlayerWithFlag()
		{
			try
			{
				QuestMediator._eventSource.RaiseKillOtherPlayerWithFlag(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyKillOtherPlayerWithFlag.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x00155E80 File Offset: 0x00154080
		public static void NotifyCapture(ConnectSceneNGUIController.RegimGame mode)
		{
			CaptureEventArgs captureEventArgs = new CaptureEventArgs
			{
				Mode = mode
			};
			try
			{
				QuestMediator._eventSource.RaiseCapture(captureEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyCapture: {0}", new object[]
				{
					captureEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00155EE8 File Offset: 0x001540E8
		public static void NotifyKillMonster(ShopNGUIController.CategoryNames weaponSlot, bool campaign = false)
		{
			KillMonsterEventArgs killMonsterEventArgs = new KillMonsterEventArgs
			{
				WeaponSlot = weaponSlot,
				Campaign = campaign
			};
			try
			{
				QuestMediator._eventSource.RaiseKillMonster(killMonsterEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillMonster: {0}", new object[]
				{
					killMonsterEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00155F58 File Offset: 0x00154158
		public static void NotifyBreakSeries()
		{
			try
			{
				QuestMediator._eventSource.RaiseBreakSeries(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyBreakSeries.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x00155FAC File Offset: 0x001541AC
		public static void NotifyMakeSeries()
		{
			try
			{
				QuestMediator._eventSource.RaiseMakeSeries(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyMakeSeries.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x00156000 File Offset: 0x00154200
		public static void NotifySurviveWaveInArena(int currentWave)
		{
			SurviveWaveInArenaEventArgs surviveWaveInArenaEventArgs = new SurviveWaveInArenaEventArgs
			{
				WaveNumber = currentWave
			};
			try
			{
				QuestMediator._eventSource.RaiseSurviveWaveInArena(surviveWaveInArenaEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifySurviveWaveInArena: {0}", new object[]
				{
					surviveWaveInArenaEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x00156068 File Offset: 0x00154268
		public static void NotifyGetGotcha()
		{
			try
			{
				QuestMediator._eventSource.RaiseGetGotcha(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyGetGotcha.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x001560BC File Offset: 0x001542BC
		public static void NotifySocialInteraction(string kind)
		{
			SocialInteractionEventArgs socialInteractionEventArgs = new SocialInteractionEventArgs
			{
				Kind = (kind ?? string.Empty)
			};
			try
			{
				QuestMediator._eventSource.RaiseSocialInteraction(socialInteractionEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifySocialInteraction: {0}", new object[]
				{
					socialInteractionEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x00156130 File Offset: 0x00154330
		public static void NotifyJump()
		{
			try
			{
				QuestMediator._eventSource.RaiseJump(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyJump");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x00156184 File Offset: 0x00154384
		public static void NotifyTurretKill()
		{
			try
			{
				QuestMediator._eventSource.RaiseTurretKill(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyTurretKill");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x001561D8 File Offset: 0x001543D8
		public static void NotifyKillOtherPlayerOnFly(bool iamFly, bool killedFly)
		{
			KillOtherPlayerOnFlyEventArgs killOtherPlayerOnFlyEventArgs = new KillOtherPlayerOnFlyEventArgs
			{
				IamFly = iamFly,
				KilledPlayerFly = killedFly
			};
			try
			{
				QuestMediator._eventSource.RaiseKillOtherPlayerOnFly(killOtherPlayerOnFlyEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillOtherPlayerOnFly: {0}", new object[]
				{
					killOtherPlayerOnFlyEventArgs
				});
				Debug.LogException(exception);
			}
		}

		// Token: 0x04002F05 RID: 12037
		private static readonly QuestMediator.QuestEventSource _eventSource = new QuestMediator.QuestEventSource();

		// Token: 0x02000729 RID: 1833
		private sealed class QuestEventSource : QuestEvents
		{
			// Token: 0x06003FE6 RID: 16358 RVA: 0x00156250 File Offset: 0x00154450
			internal new void RaiseWin(WinEventArgs e)
			{
				base.RaiseWin(e);
			}

			// Token: 0x06003FE7 RID: 16359 RVA: 0x0015625C File Offset: 0x0015445C
			internal new void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
			{
				base.RaiseKillOtherPlayer(e);
			}

			// Token: 0x06003FE8 RID: 16360 RVA: 0x00156268 File Offset: 0x00154468
			internal new void RaiseKillOtherPlayerWithFlag(EventArgs e)
			{
				base.RaiseKillOtherPlayerWithFlag(e);
			}

			// Token: 0x06003FE9 RID: 16361 RVA: 0x00156274 File Offset: 0x00154474
			internal new void RaiseCapture(CaptureEventArgs e)
			{
				base.RaiseCapture(e);
			}

			// Token: 0x06003FEA RID: 16362 RVA: 0x00156280 File Offset: 0x00154480
			internal new void RaiseKillMonster(KillMonsterEventArgs e)
			{
				base.RaiseKillMonster(e);
			}

			// Token: 0x06003FEB RID: 16363 RVA: 0x0015628C File Offset: 0x0015448C
			internal new void RaiseBreakSeries(EventArgs e)
			{
				base.RaiseBreakSeries(e);
			}

			// Token: 0x06003FEC RID: 16364 RVA: 0x00156298 File Offset: 0x00154498
			internal new void RaiseMakeSeries(EventArgs e)
			{
				base.RaiseMakeSeries(e);
			}

			// Token: 0x06003FED RID: 16365 RVA: 0x001562A4 File Offset: 0x001544A4
			internal new void RaiseSurviveWaveInArena(SurviveWaveInArenaEventArgs e)
			{
				base.RaiseSurviveWaveInArena(e);
			}

			// Token: 0x06003FEE RID: 16366 RVA: 0x001562B0 File Offset: 0x001544B0
			internal new void RaiseGetGotcha(EventArgs e)
			{
				base.RaiseGetGotcha(e);
			}

			// Token: 0x06003FEF RID: 16367 RVA: 0x001562BC File Offset: 0x001544BC
			internal new void RaiseSocialInteraction(SocialInteractionEventArgs e)
			{
				base.RaiseSocialInteraction(e);
			}

			// Token: 0x06003FF0 RID: 16368 RVA: 0x001562C8 File Offset: 0x001544C8
			internal new void RaiseJump(EventArgs e)
			{
				base.RaiseJump(e);
			}

			// Token: 0x06003FF1 RID: 16369 RVA: 0x001562D4 File Offset: 0x001544D4
			internal new void RaiseTurretKill(EventArgs e)
			{
				base.RaiseTurretKill(e);
			}

			// Token: 0x06003FF2 RID: 16370 RVA: 0x001562E0 File Offset: 0x001544E0
			internal new void RaiseKillOtherPlayerOnFly(KillOtherPlayerOnFlyEventArgs e)
			{
				base.RaiseKillOtherPlayerOnFly(e);
			}
		}
	}
}
