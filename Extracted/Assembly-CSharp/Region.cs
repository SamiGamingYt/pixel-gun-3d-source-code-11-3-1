using System;

// Token: 0x02000423 RID: 1059
public class Region
{
	// Token: 0x0600263B RID: 9787 RVA: 0x000BF2FC File Offset: 0x000BD4FC
	public static CloudRegionCode Parse(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionCode result = CloudRegionCode.none;
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			result = (CloudRegionCode)((int)Enum.Parse(typeof(CloudRegionCode), codeAsString));
		}
		return result;
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x000BF340 File Offset: 0x000BD540
	internal static CloudRegionFlag ParseFlag(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionFlag result = (CloudRegionFlag)0;
		if (Enum.IsDefined(typeof(CloudRegionFlag), codeAsString))
		{
			result = (CloudRegionFlag)((int)Enum.Parse(typeof(CloudRegionFlag), codeAsString));
		}
		return result;
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x000BF384 File Offset: 0x000BD584
	public override string ToString()
	{
		return string.Format("'{0}' \t{1}ms \t{2}", this.Code, this.Ping, this.HostAndPort);
	}

	// Token: 0x04001A90 RID: 6800
	public CloudRegionCode Code;

	// Token: 0x04001A91 RID: 6801
	public string HostAndPort;

	// Token: 0x04001A92 RID: 6802
	public int Ping;
}
