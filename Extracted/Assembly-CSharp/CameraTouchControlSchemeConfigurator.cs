using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007DE RID: 2014
public sealed class CameraTouchControlSchemeConfigurator : MonoBehaviour
{
	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x060048EF RID: 18671 RVA: 0x00195070 File Offset: 0x00193270
	// (set) Token: 0x060048F0 RID: 18672 RVA: 0x00195078 File Offset: 0x00193278
	public static CameraTouchControlSchemeConfigurator Instance { get; private set; }

	// Token: 0x04003607 RID: 13831
	public Toggle toggleCleanNGUI;

	// Token: 0x04003608 RID: 13832
	public Toggle toggleSmoothDump;

	// Token: 0x04003609 RID: 13833
	public Toggle toggleLowPassFilter;

	// Token: 0x0400360A RID: 13834
	public Toggle toggleUFPS;

	// Token: 0x0400360B RID: 13835
	public RectTransform panelCleanNGUI;

	// Token: 0x0400360C RID: 13836
	public RectTransform panelSmoothDump;

	// Token: 0x0400360D RID: 13837
	public RectTransform panelLowPassFilter;

	// Token: 0x0400360E RID: 13838
	public RectTransform panelUFPS;

	// Token: 0x0400360F RID: 13839
	public InputField firstDragClampedMax1;

	// Token: 0x04003610 RID: 13840
	public InputField startMovingThreshold1;

	// Token: 0x04003611 RID: 13841
	public InputField senseModifier;

	// Token: 0x04003612 RID: 13842
	public InputField senseModifierByAxisX;

	// Token: 0x04003613 RID: 13843
	public InputField senseModifierByAxisY;

	// Token: 0x04003614 RID: 13844
	public InputField dampingTime;

	// Token: 0x04003615 RID: 13845
	public InputField firstDragClampedMax2;

	// Token: 0x04003616 RID: 13846
	public InputField lerpCoeff;

	// Token: 0x04003617 RID: 13847
	public InputField startMovingThreshold2;

	// Token: 0x04003618 RID: 13848
	public InputField mouseLookSensitivityX;

	// Token: 0x04003619 RID: 13849
	public InputField mouseLookSensitivityY;

	// Token: 0x0400361A RID: 13850
	public InputField mouseLookSmoothSteps;

	// Token: 0x0400361B RID: 13851
	public InputField mouseLookSmoothWeight;

	// Token: 0x0400361C RID: 13852
	public Toggle mouseLookAcceleration;

	// Token: 0x0400361D RID: 13853
	public InputField mouseLookAccelerationThreshold;
}
