using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006F2 RID: 1778
	public class PetIndicationController : MonoBehaviour
	{
		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003DCE RID: 15822 RVA: 0x0014142C File Offset: 0x0013F62C
		private PetEngine _engine
		{
			get
			{
				if (this._engineValue == null)
				{
					this._engineValue = base.gameObject.GetComponentInParents<PetEngine>();
				}
				return this._engineValue;
			}
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x00141464 File Offset: 0x0013F664
		public void CreateFrameLabel()
		{
			if (this.labelsFrame.Count < this.deltaPos.Length)
			{
				for (int i = 0; i < this.deltaPos.Length; i++)
				{
					this.labelsFrame.Add(UnityEngine.Object.Instantiate<GameObject>(this.LabelObj));
				}
			}
			for (int j = 0; j < this.labelsFrame.Count; j++)
			{
				this.labelsFrame[j].transform.SetParent(this.LabelObj.transform);
				this.labelsFrame[j].transform.localScale = Vector3.one;
				this.labelsFrame[j].transform.localRotation = Quaternion.identity;
				this.labelsFrame[j].transform.localPosition = this.deltaPos[j];
				this.labelsFrame[j].GetComponent<TextMesh>().color = Color.black;
				this.labelsFrame[j].GetComponent<TextMesh>().text = this.TextMesh.text;
				this.labelsFrame[j].GetComponent<TextMesh>().offsetZ = 0.1f;
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x001415AC File Offset: 0x0013F7AC
		private void Awake()
		{
			this._meshRenderer = this.TextMesh.gameObject.GetComponent<MeshRenderer>();
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x001415C4 File Offset: 0x0013F7C4
		private void Update()
		{
			if (this.LookToCamera == null)
			{
				this.LookToCamera = Camera.main;
			}
			if (this.LookToCamera == null)
			{
				return;
			}
			string text = string.Empty;
			if (this.isUpdateNameFromInfo)
			{
				if (this._engine.Info != null && this._engine.Info.Id != string.Empty)
				{
					text = Singleton<PetsManager>.Instance.GetPlayerPet(this._engine.Info.Id).PetName;
				}
			}
			else
			{
				text = this._engine.PetName;
			}
			this.SetLabelColor();
			if (this.TextMesh.text != text)
			{
				this.TextMesh.text = text;
				for (int i = 0; i < this.labelsFrame.Count; i++)
				{
					this.labelsFrame[i].GetComponent<TextMesh>().text = this.TextMesh.text;
				}
			}
			if (!this._engine.IsAlive || !this._engine.Model.activeInHierarchy)
			{
				this.LabelObj.SetActiveSafe(false);
				this.IconObject.SetActiveSafe(false);
				this.DirectionObj.SetActiveSafe(false);
				return;
			}
			bool flag = this._engine.IsAlive;
			Vector3 vector = this.LookToCamera.WorldToScreenPoint(this._engine._lookPoint.transform.position);
			flag = (vector.x > 0f && vector.x < (float)Screen.width && vector.y > 0f && vector.y < (float)Screen.height);
			if (this._engine.Enabled && this._engine.IsMine)
			{
				this.LabelObj.SetActiveSafe(flag);
				this.IconObject.SetActiveSafe(flag);
				this.DirectionObj.SetActiveSafe(!flag);
			}
			else if (!this._engine.Enabled && !this._engine.IsMine)
			{
				this.LabelObj.SetActiveSafe(flag);
				this.IconObject.SetActiveSafe(false);
				this.DirectionObj.SetActiveSafe(false);
			}
			else if (!this._engine.Enabled)
			{
				this.LabelObj.SetActiveSafe(true);
				this.IconObject.SetActiveSafe(false);
				this.DirectionObj.SetActiveSafe(false);
			}
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0014185C File Offset: 0x0013FA5C
		private void SetLabelColor()
		{
			if (this._engine == null || this._engine.Owner == null)
			{
				return;
			}
			this.SetLabelMaterial();
			if (Defs.isMulti)
			{
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
				{
					NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
					if (myNetworkStartTable == null)
					{
						this.TextMesh.color = this.ColorPetEnemy;
					}
					else if (this._engine.Owner == myNetworkStartTable)
					{
						this.TextMesh.color = this.ColorPetMy;
					}
					else if (myNetworkStartTable != null && this._engine.Owner.myCommand == myNetworkStartTable.myCommand)
					{
						this.TextMesh.color = this.ColorPetMyTeam;
					}
					else
					{
						this.TextMesh.color = this.ColorPetEnemy;
					}
				}
				else if (Defs.isDaterRegim || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
				{
					this.TextMesh.color = ((!(this._engine.Owner == WeaponManager.sharedManager.myPlayerMoveC)) ? this.ColorPetCoop : this.ColorPetMy);
				}
				else
				{
					this.TextMesh.color = ((!this._engine.IsMine) ? this.ColorPetEnemy : this.ColorPetMy);
				}
			}
			else
			{
				this.TextMesh.color = this.ColorPetMy;
			}
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00141A08 File Offset: 0x0013FC08
		private void SetLabelMaterial()
		{
			if (this._engine.IsMine)
			{
				if (this._meshRenderer.sharedMaterial != this.MaterialMy)
				{
					this._meshRenderer.sharedMaterial = this.MaterialMy;
				}
			}
			else if (this._meshRenderer.sharedMaterial != this.MaterialOur)
			{
				this._meshRenderer.sharedMaterial = this.MaterialOur;
			}
		}

		// Token: 0x04002D9A RID: 11674
		public Camera LookToCamera;

		// Token: 0x04002D9B RID: 11675
		public GameObject LabelObj;

		// Token: 0x04002D9C RID: 11676
		public TextMesh TextMesh;

		// Token: 0x04002D9D RID: 11677
		public GameObject IconObject;

		// Token: 0x04002D9E RID: 11678
		public GameObject DirectionObj;

		// Token: 0x04002D9F RID: 11679
		public Color ColorPetMy = new Color(0f, 1f, 0f);

		// Token: 0x04002DA0 RID: 11680
		public Color ColorPetEnemy = new Color(1f, 0f, 0f);

		// Token: 0x04002DA1 RID: 11681
		public Color ColorPetMyTeam = new Color(0f, 0f, 1f);

		// Token: 0x04002DA2 RID: 11682
		public Color ColorPetCoop = new Color(1f, 1f, 1f);

		// Token: 0x04002DA3 RID: 11683
		public Material MaterialMy;

		// Token: 0x04002DA4 RID: 11684
		public Material MaterialOur;

		// Token: 0x04002DA5 RID: 11685
		public bool isUpdateNameFromInfo;

		// Token: 0x04002DA6 RID: 11686
		private MeshRenderer _meshRenderer;

		// Token: 0x04002DA7 RID: 11687
		private PetEngine _engineValue;

		// Token: 0x04002DA8 RID: 11688
		private List<GameObject> labelsFrame = new List<GameObject>();

		// Token: 0x04002DA9 RID: 11689
		private Vector3[] deltaPos = new Vector3[]
		{
			new Vector3(0f, 0.2f, 0f),
			new Vector3(0f, -0.2f, 0f),
			new Vector3(0.2f, 0.2f, 0f),
			new Vector3(-0.2f, 0.2f, 0f),
			new Vector3(0.2f, 0f, 0f),
			new Vector3(-0.2f, 0f, 0f),
			new Vector3(0.2f, -0.2f, 0f),
			new Vector3(-0.2f, 0.2f, 0f)
		};
	}
}
