using System;
using LitJson;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class AdjustBalls : MonoBehaviour
{
	// Token: 0x060009DB RID: 2523 RVA: 0x0003A3AC File Offset: 0x000385AC
	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{
		OptionalMiddleStruct container = default(OptionalMiddleStruct);
		for (int i = 0; i < ssObjects.Length; i++)
		{
			if (ssObjects[i].Keys.Contains("name"))
			{
				container.name = ssObjects[i]["name"].ToString();
			}
			if (ssObjects[i].Keys.Contains("color"))
			{
				container.color = this.GetColor(ssObjects[i]["color"].ToString());
			}
			if (ssObjects[i].Keys.Contains("drag"))
			{
				container.drag = float.Parse(ssObjects[i]["drag"].ToString());
			}
			this.UpdateObjectValues(container);
		}
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0003A478 File Offset: 0x00038678
	private void UpdateObjectValues(OptionalMiddleStruct container)
	{
		GameObject gameObject = GameObject.Find(container.name);
		gameObject.GetComponent<Renderer>().sharedMaterial.color = container.color;
		gameObject.GetComponent<Rigidbody>().drag = container.drag;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0003A4BC File Offset: 0x000386BC
	private Color GetColor(string color)
	{
		switch (color)
		{
		case "black":
			return Color.black;
		case "blue":
			return Color.blue;
		case "clear":
			return Color.clear;
		case "cyan":
			return Color.cyan;
		case "gray":
			return Color.gray;
		case "green":
			return Color.green;
		case "grey":
			return Color.grey;
		case "magenta":
			return Color.magenta;
		case "red":
			return Color.red;
		case "white":
			return Color.white;
		case "yellow":
			return Color.yellow;
		}
		return Color.grey;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0003A640 File Offset: 0x00038840
	public void ResetBalls()
	{
		OptionalMiddleStruct container = default(OptionalMiddleStruct);
		container.color = Color.white;
		container.drag = 0f;
		string str = "Ball";
		for (int i = 1; i < 4; i++)
		{
			container.name = str + i.ToString();
			this.UpdateObjectValues(container);
		}
	}
}
