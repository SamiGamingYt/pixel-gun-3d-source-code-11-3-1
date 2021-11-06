using System;
using UnityEngine;

// Token: 0x020006AB RID: 1707
[RequireComponent(typeof(Camera))]
internal sealed class MainMenuCameraMoveInOut : MonoBehaviour
{
	// Token: 0x06003BB4 RID: 15284 RVA: 0x00135D58 File Offset: 0x00133F58
	public void HandleClickTrigger()
	{
		if (this.CurrentState is MainMenuCameraMoveInOut.IdleState)
		{
			this._currentState = new MainMenuCameraMoveInOut.TransitionState();
			this._currentState = new MainMenuCameraMoveInOut.ActiveState();
		}
		else
		{
			string message = string.Format("Ignoring click while in {0} state.", this._currentState);
			Debug.Log(message);
		}
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00135DA8 File Offset: 0x00133FA8
	public void HandleBackRequest()
	{
		if (this.CurrentState is MainMenuCameraMoveInOut.ActiveState)
		{
			this._currentState = new MainMenuCameraMoveInOut.TransitionState();
			this._currentState = new MainMenuCameraMoveInOut.IdleState();
		}
		else
		{
			string message = string.Format("Ignoring click while in {0} state.", this._currentState);
			Debug.LogWarning(message);
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x00135DF8 File Offset: 0x00133FF8
	public void Reset()
	{
		base.gameObject.transform.position = this._initialPosition;
		base.gameObject.transform.rotation = this._initialRotation;
		this._currentState = new MainMenuCameraMoveInOut.IdleState();
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06003BB7 RID: 15287 RVA: 0x00135E3C File Offset: 0x0013403C
	internal MainMenuCameraMoveInOut.State CurrentState
	{
		get
		{
			return this._currentState;
		}
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x00135E44 File Offset: 0x00134044
	private void Awake()
	{
		this._initialPosition = base.gameObject.transform.position;
		this._initialRotation = base.gameObject.transform.rotation;
	}

	// Token: 0x04002C23 RID: 11299
	private Vector3 _initialPosition;

	// Token: 0x04002C24 RID: 11300
	private Quaternion _initialRotation;

	// Token: 0x04002C25 RID: 11301
	private MainMenuCameraMoveInOut.State _currentState = new MainMenuCameraMoveInOut.IdleState();

	// Token: 0x020006AC RID: 1708
	public abstract class State
	{
	}

	// Token: 0x020006AD RID: 1709
	public sealed class IdleState : MainMenuCameraMoveInOut.State
	{
	}

	// Token: 0x020006AE RID: 1710
	public sealed class ActiveState : MainMenuCameraMoveInOut.State
	{
	}

	// Token: 0x020006AF RID: 1711
	public sealed class TransitionState : MainMenuCameraMoveInOut.State
	{
	}
}
