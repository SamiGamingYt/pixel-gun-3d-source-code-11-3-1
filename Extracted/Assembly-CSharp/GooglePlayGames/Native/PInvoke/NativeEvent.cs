using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200024C RID: 588
	internal class NativeEvent : BaseReferenceHolder, IEvent
	{
		// Token: 0x060012AB RID: 4779 RVA: 0x0004E320 File Offset: 0x0004C520
		internal NativeEvent(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0004E32C File Offset: 0x0004C52C
		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0004E340 File Offset: 0x0004C540
		public string Name
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Name(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0004E354 File Offset: 0x0004C554
		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060012AF RID: 4783 RVA: 0x0004E368 File Offset: 0x0004C568
		public string ImageUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_ImageUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x0004E37C File Offset: 0x0004C57C
		public ulong CurrentCount
		{
			get
			{
				return Event.Event_Count(base.SelfPtr());
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0004E38C File Offset: 0x0004C58C
		public EventVisibility Visibility
		{
			get
			{
				Types.EventVisibility eventVisibility = Event.Event_Visibility(base.SelfPtr());
				Types.EventVisibility eventVisibility2 = eventVisibility;
				if (eventVisibility2 == Types.EventVisibility.HIDDEN)
				{
					return EventVisibility.Hidden;
				}
				if (eventVisibility2 != Types.EventVisibility.REVEALED)
				{
					throw new InvalidOperationException("Unknown visibility: " + eventVisibility);
				}
				return EventVisibility.Revealed;
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004E3D4 File Offset: 0x0004C5D4
		protected override void CallDispose(HandleRef selfPointer)
		{
			Event.Event_Dispose(selfPointer);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0004E3DC File Offset: 0x0004C5DC
		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeEvent: DELETED]";
			}
			return string.Format("[NativeEvent: Id={0}, Name={1}, Description={2}, ImageUrl={3}, CurrentCount={4}, Visibility={5}]", new object[]
			{
				this.Id,
				this.Name,
				this.Description,
				this.ImageUrl,
				this.CurrentCount,
				this.Visibility
			});
		}
	}
}
