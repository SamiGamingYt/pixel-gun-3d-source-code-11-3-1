using System;
using System.Text;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
public class X3DateConverter : MonoBehaviour
{
	// Token: 0x060033B6 RID: 13238 RVA: 0x0010BD6C File Offset: 0x00109F6C
	private double ConvertToUnixTimestamp(DateTime date)
	{
		DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		DateTime d2 = DateTime.SpecifyKind(date, DateTimeKind.Utc);
		return Math.Floor((d2 - d).TotalSeconds);
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x0010BDA8 File Offset: 0x00109FA8
	public void CalculateAndCopyClick()
	{
		string s = string.Format("{0}T{1}", this.dateStartInput.value, this.timeStartInput.value);
		DateTime date = default(DateTime);
		if (!DateTime.TryParse(s, out date))
		{
			this.statusLabel.text = "Incorrect date or time format!";
			return;
		}
		float num;
		if (!float.TryParse(this.durationInput.value, out num))
		{
			this.statusLabel.text = "Incorrect duration format!";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("{");
		stringBuilder.AppendFormat("\t\"start\": {0}\n", this.ConvertToUnixTimestamp(date));
		float num2 = num * 360f;
		stringBuilder.AppendFormat("\t\"duration\": {0}\n", num2);
		stringBuilder.AppendLine("}");
		EditorListBuilder.CopyTextInClipboard(stringBuilder.ToString());
		this.statusLabel.text = "Converted complete!";
	}

	// Token: 0x0400260B RID: 9739
	public UIInput dateStartInput;

	// Token: 0x0400260C RID: 9740
	public UIInput timeStartInput;

	// Token: 0x0400260D RID: 9741
	public UIInput durationInput;

	// Token: 0x0400260E RID: 9742
	public UILabel statusLabel;
}
