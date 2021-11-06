using System;

// Token: 0x02000458 RID: 1112
public interface IPunTurnManagerCallbacks
{
	// Token: 0x06002726 RID: 10022
	void OnTurnBegins(int turn);

	// Token: 0x06002727 RID: 10023
	void OnTurnCompleted(int turn);

	// Token: 0x06002728 RID: 10024
	void OnPlayerMove(PhotonPlayer player, int turn, object move);

	// Token: 0x06002729 RID: 10025
	void OnPlayerFinished(PhotonPlayer player, int turn, object move);

	// Token: 0x0600272A RID: 10026
	void OnTurnTimeEnds(int turn);
}
