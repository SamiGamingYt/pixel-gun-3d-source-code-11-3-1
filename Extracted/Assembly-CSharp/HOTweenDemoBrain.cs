using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class HOTweenDemoBrain : MonoBehaviour
{
	// Token: 0x060014FC RID: 5372 RVA: 0x00053070 File Offset: 0x00051270
	private void Start()
	{
		HOTween.Init(true, true, true);
		HOTween.To(this.CubeTrans1, 4f, "position", new Vector3(-3f, 6f, 0f));
		HOTween.To(this.CubeTrans2, 3f, new TweenParms().Prop("position", new Vector3(0f, 6f, 0f), true).Prop("rotation", new Vector3(0f, 1024f, 0f), true).Loops(-1, LoopType.Yoyo).Ease(EaseType.EaseInOutQuad).OnStepComplete(new TweenDelegate.TweenCallback(this.Cube2StepComplete)));
		HOTween.To(this, 3f, new TweenParms().Prop("SampleString", "Hello I'm a sample tweened string").Ease(EaseType.Linear).Loops(-1, LoopType.Yoyo));
		TweenParms p_parms = new TweenParms().Prop("SampleFloat", 27.5f).Ease(EaseType.Linear).Loops(-1, LoopType.Yoyo);
		HOTween.To(this, 3f, p_parms);
		Color color = this.CubeTrans3.GetComponent<Renderer>().material.color;
		color.a = 0f;
		Sequence sequence = new Sequence(new SequenceParms().Loops(-1, LoopType.Yoyo));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, new TweenParms().Prop("rotation", new Vector3(360f, 0f, 0f))));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, new TweenParms().Prop("position", new Vector3(0f, 6f, 0f), true)));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, new TweenParms().Prop("rotation", new Vector3(0f, 360f, 0f))));
		sequence.Insert(sequence.duration * 0.5f, HOTween.To(this.CubeTrans3.GetComponent<Renderer>().material, sequence.duration * 0.5f, new TweenParms().Prop("color", color)));
		sequence.Play();
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000532D4 File Offset: 0x000514D4
	private void OnGUI()
	{
		GUILayout.Label("String tween: " + this.SampleString, new GUILayoutOption[0]);
		GUILayout.Label("Float tween: " + this.SampleFloat, new GUILayoutOption[0]);
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00053314 File Offset: 0x00051514
	private void Cube2StepComplete()
	{
		Debug.Log("HOTween: Cube 2 Step Complete");
	}

	// Token: 0x04000C3D RID: 3133
	public Transform CubeTrans1;

	// Token: 0x04000C3E RID: 3134
	public Transform CubeTrans2;

	// Token: 0x04000C3F RID: 3135
	public Transform CubeTrans3;

	// Token: 0x04000C40 RID: 3136
	public string SampleString;

	// Token: 0x04000C41 RID: 3137
	public float SampleFloat;
}
