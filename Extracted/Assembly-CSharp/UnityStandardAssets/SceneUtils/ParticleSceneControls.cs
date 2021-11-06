using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x020004E0 RID: 1248
	public class ParticleSceneControls : MonoBehaviour
	{
		// Token: 0x06002C68 RID: 11368 RVA: 0x000EB5EC File Offset: 0x000E97EC
		private void Awake()
		{
			this.Select(ParticleSceneControls.s_SelectedIndex);
			this.previousButton.onClick.AddListener(new UnityAction(this.Previous));
			this.nextButton.onClick.AddListener(new UnityAction(this.Next));
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000EB63C File Offset: 0x000E983C
		private void OnDisable()
		{
			this.previousButton.onClick.RemoveListener(new UnityAction(this.Previous));
			this.nextButton.onClick.RemoveListener(new UnityAction(this.Next));
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000EB684 File Offset: 0x000E9884
		private void Previous()
		{
			ParticleSceneControls.s_SelectedIndex--;
			if (ParticleSceneControls.s_SelectedIndex == -1)
			{
				ParticleSceneControls.s_SelectedIndex = this.demoParticles.items.Length - 1;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000EB6C8 File Offset: 0x000E98C8
		public void Next()
		{
			ParticleSceneControls.s_SelectedIndex++;
			if (ParticleSceneControls.s_SelectedIndex == this.demoParticles.items.Length)
			{
				ParticleSceneControls.s_SelectedIndex = 0;
			}
			this.Select(ParticleSceneControls.s_SelectedIndex);
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000EB70C File Offset: 0x000E990C
		private void Update()
		{
			this.sceneCamera.localPosition = Vector3.SmoothDamp(this.sceneCamera.localPosition, Vector3.forward * (float)(-(float)ParticleSceneControls.s_Selected.camOffset), ref this.m_CamOffsetVelocity, 1f);
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				return;
			}
			if (this.CheckForGuiCollision())
			{
				return;
			}
			bool flag = Input.GetMouseButtonDown(0) && ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Instantiate;
			bool flag2 = Input.GetMouseButton(0) && ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail;
			if (flag || flag2)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit))
				{
					Quaternion rotation = Quaternion.LookRotation(raycastHit.normal);
					if (ParticleSceneControls.s_Selected.align == ParticleSceneControls.AlignMode.Up)
					{
						rotation = Quaternion.identity;
					}
					Vector3 vector = raycastHit.point + raycastHit.normal * this.spawnOffset;
					if ((vector - this.m_LastPos).magnitude > ParticleSceneControls.s_Selected.minDist)
					{
						if (ParticleSceneControls.s_Selected.mode != ParticleSceneControls.Mode.Trail || this.m_Instance == null)
						{
							this.m_Instance = (Transform)UnityEngine.Object.Instantiate(ParticleSceneControls.s_Selected.transform, vector, rotation);
							if (this.m_ParticleMultiplier != null)
							{
								this.m_Instance.GetComponent<ParticleSystemMultiplier>().multiplier = this.multiply;
							}
							this.m_CurrentParticleList.Add(this.m_Instance);
							if (ParticleSceneControls.s_Selected.maxCount > 0 && this.m_CurrentParticleList.Count > ParticleSceneControls.s_Selected.maxCount)
							{
								if (this.m_CurrentParticleList[0] != null)
								{
									UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
								}
								this.m_CurrentParticleList.RemoveAt(0);
							}
						}
						else
						{
							this.m_Instance.position = vector;
							this.m_Instance.rotation = rotation;
						}
						if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail)
						{
							this.m_Instance.transform.GetComponent<ParticleSystem>().enableEmission = false;
							this.m_Instance.transform.GetComponent<ParticleSystem>().Emit(1);
						}
						this.m_Instance.parent = raycastHit.transform;
						this.m_LastPos = vector;
					}
				}
			}
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x000EB98C File Offset: 0x000E9B8C
		private bool CheckForGuiCollision()
		{
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.pressPosition = Input.mousePosition;
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.graphicRaycaster.Raycast(pointerEventData, list);
			return list.Count > 0;
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000EB9E4 File Offset: 0x000E9BE4
		private void Select(int i)
		{
			ParticleSceneControls.s_Selected = this.demoParticles.items[i];
			this.m_Instance = null;
			foreach (ParticleSceneControls.DemoParticleSystem demoParticleSystem in this.demoParticles.items)
			{
				if (demoParticleSystem != ParticleSceneControls.s_Selected && demoParticleSystem.mode == ParticleSceneControls.Mode.Activate)
				{
					demoParticleSystem.transform.gameObject.SetActive(false);
				}
			}
			if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
			{
				ParticleSceneControls.s_Selected.transform.gameObject.SetActive(true);
			}
			this.m_ParticleMultiplier = ParticleSceneControls.s_Selected.transform.GetComponent<ParticleSystemMultiplier>();
			this.multiply = 1f;
			if (this.clearOnChange)
			{
				while (this.m_CurrentParticleList.Count > 0)
				{
					UnityEngine.Object.Destroy(this.m_CurrentParticleList[0].gameObject);
					this.m_CurrentParticleList.RemoveAt(0);
				}
			}
			this.instructionText.text = ParticleSceneControls.s_Selected.instructionText;
			this.titleText.text = ParticleSceneControls.s_Selected.transform.name;
		}

		// Token: 0x04002162 RID: 8546
		public ParticleSceneControls.DemoParticleSystemList demoParticles;

		// Token: 0x04002163 RID: 8547
		public float spawnOffset = 0.5f;

		// Token: 0x04002164 RID: 8548
		public float multiply = 1f;

		// Token: 0x04002165 RID: 8549
		public bool clearOnChange;

		// Token: 0x04002166 RID: 8550
		public Text titleText;

		// Token: 0x04002167 RID: 8551
		public Transform sceneCamera;

		// Token: 0x04002168 RID: 8552
		public Text instructionText;

		// Token: 0x04002169 RID: 8553
		public Button previousButton;

		// Token: 0x0400216A RID: 8554
		public Button nextButton;

		// Token: 0x0400216B RID: 8555
		public GraphicRaycaster graphicRaycaster;

		// Token: 0x0400216C RID: 8556
		public EventSystem eventSystem;

		// Token: 0x0400216D RID: 8557
		private ParticleSystemMultiplier m_ParticleMultiplier;

		// Token: 0x0400216E RID: 8558
		private List<Transform> m_CurrentParticleList = new List<Transform>();

		// Token: 0x0400216F RID: 8559
		private Transform m_Instance;

		// Token: 0x04002170 RID: 8560
		private static int s_SelectedIndex;

		// Token: 0x04002171 RID: 8561
		private Vector3 m_CamOffsetVelocity = Vector3.zero;

		// Token: 0x04002172 RID: 8562
		private Vector3 m_LastPos;

		// Token: 0x04002173 RID: 8563
		private static ParticleSceneControls.DemoParticleSystem s_Selected;

		// Token: 0x020004E1 RID: 1249
		public enum Mode
		{
			// Token: 0x04002175 RID: 8565
			Activate,
			// Token: 0x04002176 RID: 8566
			Instantiate,
			// Token: 0x04002177 RID: 8567
			Trail
		}

		// Token: 0x020004E2 RID: 1250
		public enum AlignMode
		{
			// Token: 0x04002179 RID: 8569
			Normal,
			// Token: 0x0400217A RID: 8570
			Up
		}

		// Token: 0x020004E3 RID: 1251
		[Serializable]
		public class DemoParticleSystem
		{
			// Token: 0x0400217B RID: 8571
			public Transform transform;

			// Token: 0x0400217C RID: 8572
			public ParticleSceneControls.Mode mode;

			// Token: 0x0400217D RID: 8573
			public ParticleSceneControls.AlignMode align;

			// Token: 0x0400217E RID: 8574
			public int maxCount;

			// Token: 0x0400217F RID: 8575
			public float minDist;

			// Token: 0x04002180 RID: 8576
			public int camOffset = 15;

			// Token: 0x04002181 RID: 8577
			public string instructionText;
		}

		// Token: 0x020004E4 RID: 1252
		[Serializable]
		public class DemoParticleSystemList
		{
			// Token: 0x04002182 RID: 8578
			public ParticleSceneControls.DemoParticleSystem[] items;
		}
	}
}
