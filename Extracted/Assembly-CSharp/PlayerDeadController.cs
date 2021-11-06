using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public sealed class PlayerDeadController : MonoBehaviour
{
	// Token: 0x060027F8 RID: 10232 RVA: 0x000C7A64 File Offset: 0x000C5C64
	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x000C7A94 File Offset: 0x000C5C94
	private void TryPlayAudioClip(GameObject obj)
	{
		if (!Defs.isSoundFX)
		{
			return;
		}
		AudioSource component = obj.GetComponent<AudioSource>();
		if (component == null)
		{
			return;
		}
		component.Play();
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000C7AC8 File Offset: 0x000C5CC8
	public void StartShow(Vector3 pos, Quaternion rot, int _typeDead, bool _isUseMine, Texture _skin)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		if (_typeDead == 1)
		{
			this.playerDeads[1].SetActive(true);
			this.TryPlayAudioClip(this.playerDeads[1]);
			this.deadExplosionController.StartAnim();
		}
		else if (_typeDead >= 2 && _typeDead <= 9)
		{
			this.playerDeads[2].SetActive(true);
			this.TryPlayAudioClip(this.playerDeads[2]);
			Color color = new Color(0f, 0.5f, 1f);
			if (_typeDead == 3)
			{
				color = new Color(1f, 0f, 0f);
			}
			if (_typeDead == 4)
			{
				color = new Color(1f, 0f, 0f);
			}
			if (_typeDead == 4)
			{
				color = new Color(1f, 0f, 1f);
			}
			if (_typeDead == 5)
			{
				color = new Color(0f, 0.5f, 1f);
			}
			if (_typeDead == 6)
			{
				color = new Color(1f, 0.91f, 0f);
			}
			if (_typeDead == 7)
			{
				color = new Color(0f, 0.85f, 0f);
			}
			if (_typeDead == 8)
			{
				color = new Color(1f, 0.55f, 0f);
			}
			if (_typeDead == 9)
			{
				color = new Color(1f, 1f, 1f);
			}
			this.deadEnergyController.StartAnim(color, _skin);
		}
		else
		{
			this.playerDeads[0].SetActive(true);
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000C7C80 File Offset: 0x000C5E80
	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.playerDeads[0].SetActive(false);
			if (!Device.isPixelGunLow)
			{
				this.playerDeads[1].SetActive(false);
				this.playerDeads[2].SetActive(false);
			}
			this.isUseMine = false;
		}
	}

	// Token: 0x04001C3D RID: 7229
	private float liveTime = -1f;

	// Token: 0x04001C3E RID: 7230
	private float maxliveTime = 4.8f;

	// Token: 0x04001C3F RID: 7231
	public bool isUseMine;

	// Token: 0x04001C40 RID: 7232
	private Transform myTransform;

	// Token: 0x04001C41 RID: 7233
	public Animation myAnimation;

	// Token: 0x04001C42 RID: 7234
	public GameObject[] playerDeads;

	// Token: 0x04001C43 RID: 7235
	public DeadEnergyController deadEnergyController;

	// Token: 0x04001C44 RID: 7236
	public DeadExplosionController deadExplosionController;
}
