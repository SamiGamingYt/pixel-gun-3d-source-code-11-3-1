using System;
using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x020004E5 RID: 1253
	public class PlaceTargetWithMouse : MonoBehaviour
	{
		// Token: 0x06002C72 RID: 11378 RVA: 0x000EBB38 File Offset: 0x000E9D38
		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (!Physics.Raycast(ray, out raycastHit))
			{
				return;
			}
			base.transform.position = raycastHit.point + raycastHit.normal * this.surfaceOffset;
			if (this.setTargetOn != null)
			{
				this.setTargetOn.SendMessage("SetTarget", base.transform);
			}
		}

		// Token: 0x04002183 RID: 8579
		public float surfaceOffset = 1.5f;

		// Token: 0x04002184 RID: 8580
		public GameObject setTargetOn;
	}
}
