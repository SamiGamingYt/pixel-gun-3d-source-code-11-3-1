using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class InRoomRoundTimer : MonoBehaviour
{
	// Token: 0x060026A9 RID: 9897 RVA: 0x000C1CDC File Offset: 0x000BFEDC
	private void StartRoundNow()
	{
		if (PhotonNetwork.time < 9.999999747378752E-05)
		{
			this.startRoundWhenTimeIsSynced = true;
			return;
		}
		this.startRoundWhenTimeIsSynced = false;
		Hashtable hashtable = new Hashtable();
		hashtable["st"] = PhotonNetwork.time;
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x000C1D34 File Offset: 0x000BFF34
	public void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.StartRoundNow();
		}
		else
		{
			Debug.Log("StartTime already set: " + PhotonNetwork.room.customProperties.ContainsKey("st"));
		}
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x000C1D80 File Offset: 0x000BFF80
	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("st"))
		{
			this.StartTime = (double)propertiesThatChanged["st"];
		}
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x000C1DB4 File Offset: 0x000BFFB4
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!PhotonNetwork.room.customProperties.ContainsKey("st"))
		{
			Debug.Log("The new master starts a new round, cause we didn't start yet.");
			this.StartRoundNow();
		}
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x000C1DE0 File Offset: 0x000BFFE0
	private void Update()
	{
		if (this.startRoundWhenTimeIsSynced)
		{
			this.StartRoundNow();
		}
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x000C1DF4 File Offset: 0x000BFFF4
	public void OnGUI()
	{
		double num = PhotonNetwork.time - this.StartTime;
		double num2 = (double)this.SecondsPerTurn - num % (double)this.SecondsPerTurn;
		int num3 = (int)(num / (double)this.SecondsPerTurn);
		GUILayout.BeginArea(this.TextPos);
		GUILayout.Label(string.Format("elapsed: {0:0.000}", num), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("remaining: {0:0.000}", num2), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("turn: {0:0}", num3), new GUILayoutOption[0]);
		if (GUILayout.Button("new round", new GUILayoutOption[0]))
		{
			this.StartRoundNow();
		}
		GUILayout.EndArea();
	}

	// Token: 0x04001B2D RID: 6957
	private const string StartTimeKey = "st";

	// Token: 0x04001B2E RID: 6958
	public int SecondsPerTurn = 5;

	// Token: 0x04001B2F RID: 6959
	public double StartTime;

	// Token: 0x04001B30 RID: 6960
	public Rect TextPos = new Rect(0f, 80f, 150f, 300f);

	// Token: 0x04001B31 RID: 6961
	private bool startRoundWhenTimeIsSynced;
}
