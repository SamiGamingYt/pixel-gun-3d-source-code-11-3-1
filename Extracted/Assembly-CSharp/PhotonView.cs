using System;
using System.Collections.Generic;
using System.Reflection;
using Photon;
using UnityEngine;

// Token: 0x0200041E RID: 1054
[AddComponentMenu("Photon Networking/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x060025EC RID: 9708 RVA: 0x000BDF90 File Offset: 0x000BC190
	// (set) Token: 0x060025ED RID: 9709 RVA: 0x000BDFCC File Offset: 0x000BC1CC
	public int prefix
	{
		get
		{
			if (this.prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				this.prefixBackup = (int)PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return this.prefixBackup;
		}
		set
		{
			this.prefixBackup = value;
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x060025EE RID: 9710 RVA: 0x000BDFD8 File Offset: 0x000BC1D8
	// (set) Token: 0x060025EF RID: 9711 RVA: 0x000BE004 File Offset: 0x000BC204
	public object[] instantiationData
	{
		get
		{
			if (!this.didAwake)
			{
				this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
			}
			return this.instantiationDataField;
		}
		set
		{
			this.instantiationDataField = value;
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x060025F0 RID: 9712 RVA: 0x000BE010 File Offset: 0x000BC210
	// (set) Token: 0x060025F1 RID: 9713 RVA: 0x000BE018 File Offset: 0x000BC218
	public int viewID
	{
		get
		{
			return this.viewIdField;
		}
		set
		{
			bool flag = this.didAwake && this.viewIdField == 0;
			this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			this.viewIdField = value;
			if (flag)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000BE064 File Offset: 0x000BC264
	public bool isSceneView
	{
		get
		{
			return this.CreatorActorNr == 0;
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000BE070 File Offset: 0x000BC270
	public PhotonPlayer owner
	{
		get
		{
			return PhotonPlayer.Find(this.ownerId);
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000BE080 File Offset: 0x000BC280
	public int OwnerActorNr
	{
		get
		{
			return this.ownerId;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000BE088 File Offset: 0x000BC288
	public bool isOwnerActive
	{
		get
		{
			return this.ownerId != 0 && PhotonNetwork.networkingPeer.mActors.ContainsKey(this.ownerId);
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000BE0B0 File Offset: 0x000BC2B0
	public int CreatorActorNr
	{
		get
		{
			return this.viewIdField / PhotonNetwork.MAX_VIEW_IDS;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000BE0C0 File Offset: 0x000BC2C0
	public bool isMine
	{
		get
		{
			return this.ownerId == PhotonNetwork.player.ID || (!this.isOwnerActive && PhotonNetwork.isMasterClient);
		}
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x000BE0F8 File Offset: 0x000BC2F8
	protected internal void Awake()
	{
		if (this.viewID != 0)
		{
			PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
		}
		this.didAwake = true;
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x000BE130 File Offset: 0x000BC330
	public void RequestOwnership()
	{
		PhotonNetwork.networkingPeer.RequestOwnership(this.viewID, this.ownerId);
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x000BE148 File Offset: 0x000BC348
	public void TransferOwnership(PhotonPlayer newOwner)
	{
		this.TransferOwnership(newOwner.ID);
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000BE158 File Offset: 0x000BC358
	public void TransferOwnership(int newOwnerId)
	{
		PhotonNetwork.networkingPeer.TransferOwnership(this.viewID, newOwnerId);
		this.ownerId = newOwnerId;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x000BE174 File Offset: 0x000BC374
	protected internal void OnDestroy()
	{
		if (!this.removedFromLocalViewList)
		{
			bool flag = PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
			bool flag2 = false;
			if (flag && !flag2 && this.instantiationId > 0 && !PhotonHandler.AppQuits && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("PUN-instantiated '" + base.gameObject.name + "' got destroyed by engine. This is OK when loading levels. Otherwise use: PhotonNetwork.Destroy().");
			}
		}
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x000BE1E8 File Offset: 0x000BC3E8
	public void SerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.SerializeComponent(this.observed, stream, info);
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.SerializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x000BE250 File Offset: 0x000BC450
	public void DeserializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.DeserializeComponent(this.observed, stream, info);
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.DeserializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x000BE2B8 File Offset: 0x000BC4B8
	protected internal void DeserializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyRotation:
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyScale:
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.PositionAndRotation:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.All:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			switch (this.onSerializeRigidBodyOption)
			{
			case OnSerializeRigidBody.OnlyVelocity:
				rigidbody.velocity = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeRigidBody.OnlyAngularVelocity:
				rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeRigidBody.All:
				rigidbody.velocity = (Vector3)stream.ReceiveNext();
				rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
				break;
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			switch (this.onSerializeRigidBodyOption)
			{
			case OnSerializeRigidBody.OnlyVelocity:
				rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
				break;
			case OnSerializeRigidBody.OnlyAngularVelocity:
				rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
				break;
			case OnSerializeRigidBody.All:
				rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
				rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
				break;
			}
		}
		else
		{
			Debug.LogError("Type of observed is unknown when receiving.");
		}
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x000BE4E8 File Offset: 0x000BC6E8
	protected internal void SerializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				stream.SendNext(transform.localPosition);
				break;
			case OnSerializeTransform.OnlyRotation:
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.OnlyScale:
				stream.SendNext(transform.localScale);
				break;
			case OnSerializeTransform.PositionAndRotation:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.All:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				stream.SendNext(transform.localScale);
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			switch (this.onSerializeRigidBodyOption)
			{
			case OnSerializeRigidBody.OnlyVelocity:
				stream.SendNext(rigidbody.velocity);
				break;
			case OnSerializeRigidBody.OnlyAngularVelocity:
				stream.SendNext(rigidbody.angularVelocity);
				break;
			case OnSerializeRigidBody.All:
				stream.SendNext(rigidbody.velocity);
				stream.SendNext(rigidbody.angularVelocity);
				break;
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			switch (this.onSerializeRigidBodyOption)
			{
			case OnSerializeRigidBody.OnlyVelocity:
				stream.SendNext(rigidbody2D.velocity);
				break;
			case OnSerializeRigidBody.OnlyAngularVelocity:
				stream.SendNext(rigidbody2D.angularVelocity);
				break;
			case OnSerializeRigidBody.All:
				stream.SendNext(rigidbody2D.velocity);
				stream.SendNext(rigidbody2D.angularVelocity);
				break;
			}
		}
		else
		{
			Debug.LogError("Observed type is not serializable: " + component.GetType());
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x000BE724 File Offset: 0x000BC924
	protected internal void ExecuteComponentOnSerialize(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		IPunObservable punObservable = component as IPunObservable;
		if (punObservable != null)
		{
			punObservable.OnPhotonSerializeView(stream, info);
		}
		else if (component != null)
		{
			MethodInfo methodInfo = null;
			if (!this.m_OnSerializeMethodInfos.TryGetValue(component, out methodInfo))
			{
				if (!NetworkingPeer.GetMethod(component as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out methodInfo))
				{
					Debug.LogError("The observed monobehaviour (" + component.name + ") of this PhotonView does not implement OnPhotonSerializeView()!");
					methodInfo = null;
				}
				this.m_OnSerializeMethodInfos.Add(component, methodInfo);
			}
			if (methodInfo != null)
			{
				methodInfo.Invoke(component, new object[]
				{
					stream,
					info
				});
			}
		}
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000BE7D8 File Offset: 0x000BC9D8
	public void RefreshRpcMonoBehaviourCache()
	{
		this.RpcMonoBehaviours = base.GetComponents<UnityEngine.MonoBehaviour>();
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000BE7E8 File Offset: 0x000BC9E8
	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, false, parameters);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000BE7F4 File Offset: 0x000BC9F4
	public void RpcSecure(string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, encrypt, parameters);
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x000BE804 File Offset: 0x000BCA04
	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, false, parameters);
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x000BE810 File Offset: 0x000BCA10
	public void RpcSecure(string methodName, PhotonPlayer targetPlayer, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, encrypt, parameters);
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000BE820 File Offset: 0x000BCA20
	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000BE828 File Offset: 0x000BCA28
	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000BE830 File Offset: 0x000BCA30
	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000BE840 File Offset: 0x000BCA40
	public override string ToString()
	{
		return string.Format("View ({3}){0} on {1} {2}", new object[]
		{
			this.viewID,
			(!(base.gameObject != null)) ? "GO==null" : base.gameObject.name,
			(!this.isSceneView) ? string.Empty : "(scene)",
			this.prefix
		});
	}

	// Token: 0x04001A67 RID: 6759
	public int ownerId;

	// Token: 0x04001A68 RID: 6760
	public int group;

	// Token: 0x04001A69 RID: 6761
	protected internal bool mixedModeIsReliable;

	// Token: 0x04001A6A RID: 6762
	public bool OwnerShipWasTransfered;

	// Token: 0x04001A6B RID: 6763
	public int prefixBackup = -1;

	// Token: 0x04001A6C RID: 6764
	internal object[] instantiationDataField;

	// Token: 0x04001A6D RID: 6765
	protected internal object[] lastOnSerializeDataSent;

	// Token: 0x04001A6E RID: 6766
	protected internal object[] lastOnSerializeDataReceived;

	// Token: 0x04001A6F RID: 6767
	public Component observed;

	// Token: 0x04001A70 RID: 6768
	public ViewSynchronization synchronization;

	// Token: 0x04001A71 RID: 6769
	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	// Token: 0x04001A72 RID: 6770
	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	// Token: 0x04001A73 RID: 6771
	public OwnershipOption ownershipTransfer;

	// Token: 0x04001A74 RID: 6772
	public List<Component> ObservedComponents;

	// Token: 0x04001A75 RID: 6773
	private Dictionary<Component, MethodInfo> m_OnSerializeMethodInfos = new Dictionary<Component, MethodInfo>(3);

	// Token: 0x04001A76 RID: 6774
	[SerializeField]
	private int viewIdField;

	// Token: 0x04001A77 RID: 6775
	public int instantiationId;

	// Token: 0x04001A78 RID: 6776
	protected internal bool didAwake;

	// Token: 0x04001A79 RID: 6777
	[SerializeField]
	protected internal bool isRuntimeInstantiated;

	// Token: 0x04001A7A RID: 6778
	protected internal bool removedFromLocalViewList;

	// Token: 0x04001A7B RID: 6779
	internal UnityEngine.MonoBehaviour[] RpcMonoBehaviours;

	// Token: 0x04001A7C RID: 6780
	private MethodInfo OnSerializeMethodInfo;

	// Token: 0x04001A7D RID: 6781
	private bool failedToFindOnSerialize;
}
