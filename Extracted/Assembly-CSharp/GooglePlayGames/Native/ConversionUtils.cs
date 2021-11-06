using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;

namespace GooglePlayGames.Native
{
	// Token: 0x020001C0 RID: 448
	internal static class ConversionUtils
	{
		// Token: 0x06000E9B RID: 3739 RVA: 0x00046F94 File Offset: 0x00045194
		internal static ResponseStatus ConvertResponseStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.ResponseStatus)0:
				return ResponseStatus.Timeout;
			case CommonErrorStatus.ResponseStatus.VALID:
				return ResponseStatus.VersionUpdateRequired;
			case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				return ResponseStatus.NotAuthorized;
			case (CommonErrorStatus.ResponseStatus)3:
				return ResponseStatus.InternalError;
			case (CommonErrorStatus.ResponseStatus)4:
				return ResponseStatus.LicenseCheckFailed;
			case (CommonErrorStatus.ResponseStatus)6:
				return ResponseStatus.Success;
			case (CommonErrorStatus.ResponseStatus)7:
				return ResponseStatus.SuccessWithStale;
			}
			throw new InvalidOperationException("Unknown status: " + status);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00046FF8 File Offset: 0x000451F8
		internal static CommonStatusCodes ConvertResponseStatusToCommonStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.ResponseStatus)0:
				return CommonStatusCodes.Timeout;
			case CommonErrorStatus.ResponseStatus.VALID:
				return CommonStatusCodes.ServiceVersionUpdateRequired;
			case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				return CommonStatusCodes.AuthApiAccessForbidden;
			case (CommonErrorStatus.ResponseStatus)3:
				return CommonStatusCodes.InternalError;
			case (CommonErrorStatus.ResponseStatus)4:
				return CommonStatusCodes.LicenseCheckFailed;
			case (CommonErrorStatus.ResponseStatus)6:
				return CommonStatusCodes.Success;
			case (CommonErrorStatus.ResponseStatus)7:
				return CommonStatusCodes.SuccessCached;
			}
			Debug.LogWarning("Unknown ResponseStatus: " + status + ", defaulting to CommonStatusCodes.Error");
			return CommonStatusCodes.Error;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00047064 File Offset: 0x00045264
		internal static UIStatus ConvertUIStatus(CommonErrorStatus.UIStatus status)
		{
			switch (status + 12)
			{
			case (CommonErrorStatus.UIStatus)0:
				return UIStatus.UiBusy;
			case (CommonErrorStatus.UIStatus)6:
				return UIStatus.UserClosedUI;
			case (CommonErrorStatus.UIStatus)7:
				return UIStatus.Timeout;
			case (CommonErrorStatus.UIStatus)8:
				return UIStatus.VersionUpdateRequired;
			case (CommonErrorStatus.UIStatus)9:
				return UIStatus.NotAuthorized;
			case (CommonErrorStatus.UIStatus)10:
				return UIStatus.InternalError;
			case (CommonErrorStatus.UIStatus)13:
				return UIStatus.Valid;
			}
			throw new InvalidOperationException("Unknown status: " + status);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000470E4 File Offset: 0x000452E4
		internal static GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(DataSource source)
		{
			if (source == DataSource.ReadCacheOrNetwork)
			{
				return GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
			}
			if (source != DataSource.ReadNetworkOnly)
			{
				throw new InvalidOperationException("Found unhandled DataSource: " + source);
			}
			return GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
		}
	}
}
