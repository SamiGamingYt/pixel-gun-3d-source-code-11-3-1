using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class PhotonPingManager
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x0600260D RID: 9741 RVA: 0x000BE8E0 File Offset: 0x000BCAE0
	public Region BestRegion
	{
		get
		{
			Region result = null;
			int num = int.MaxValue;
			foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
			{
				UnityEngine.Debug.Log("BestRegion checks region: " + region);
				if (region.Ping != 0 && region.Ping < num)
				{
					num = region.Ping;
					result = region;
				}
			}
			return result;
		}
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x0600260E RID: 9742 RVA: 0x000BE97C File Offset: 0x000BCB7C
	public bool Done
	{
		get
		{
			return this.PingsRunning == 0;
		}
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x000BE988 File Offset: 0x000BCB88
	public IEnumerator PingSocket(Region region)
	{
		region.Ping = PhotonPingManager.Attempts * PhotonPingManager.MaxMilliseconsPerPing;
		this.PingsRunning++;
		PhotonPing ping;
		if (PhotonHandler.PingImplementation == typeof(PingNativeDynamic))
		{
			UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
			ping = new PingNativeDynamic();
		}
		else if (PhotonHandler.PingImplementation == typeof(PingMono))
		{
			ping = new PingMono();
		}
		else
		{
			ping = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
		}
		float rttSum = 0f;
		int replyCount = 0;
		string cleanIpOfRegion = region.HostAndPort;
		int indexOfColon = cleanIpOfRegion.LastIndexOf(':');
		if (indexOfColon > 1)
		{
			cleanIpOfRegion = cleanIpOfRegion.Substring(0, indexOfColon);
		}
		cleanIpOfRegion = PhotonPingManager.ResolveHost(cleanIpOfRegion);
		for (int i = 0; i < PhotonPingManager.Attempts; i++)
		{
			bool overtime = false;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				ping.StartPing(cleanIpOfRegion);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				UnityEngine.Debug.Log("catched: " + e);
				this.PingsRunning--;
				break;
			}
			while (!ping.Done())
			{
				if (sw.ElapsedMilliseconds >= (long)PhotonPingManager.MaxMilliseconsPerPing)
				{
					overtime = true;
					break;
				}
				yield return 0;
			}
			int rtt = (int)sw.ElapsedMilliseconds;
			if (!PhotonPingManager.IgnoreInitialAttempt || i != 0)
			{
				if (ping.Successful && !overtime)
				{
					rttSum += (float)rtt;
					replyCount++;
					region.Ping = (int)(rttSum / (float)replyCount);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		this.PingsRunning--;
		yield return null;
		yield break;
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x000BE9B4 File Offset: 0x000BCBB4
	public static string ResolveHost(string hostName)
	{
		string text = string.Empty;
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			if (hostAddresses.Length == 1)
			{
				return hostAddresses[0].ToString();
			}
			foreach (IPAddress ipaddress in hostAddresses)
			{
				if (ipaddress != null)
				{
					if (ipaddress.ToString().Contains(":"))
					{
						return ipaddress.ToString();
					}
					if (string.IsNullOrEmpty(text))
					{
						text = hostAddresses.ToString();
					}
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
		}
		return text;
	}

	// Token: 0x04001A7E RID: 6782
	public bool UseNative;

	// Token: 0x04001A7F RID: 6783
	public static int Attempts = 5;

	// Token: 0x04001A80 RID: 6784
	public static bool IgnoreInitialAttempt = true;

	// Token: 0x04001A81 RID: 6785
	public static int MaxMilliseconsPerPing = 800;

	// Token: 0x04001A82 RID: 6786
	private int PingsRunning;
}
