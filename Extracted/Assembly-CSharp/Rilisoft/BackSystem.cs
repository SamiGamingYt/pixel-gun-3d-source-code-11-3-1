using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000568 RID: 1384
	internal sealed class BackSystem : MonoBehaviour
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x000FA5C8 File Offset: 0x000F87C8
		public static BackSystem Instance
		{
			get
			{
				return BackSystem._instance.Value;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x000FA5D4 File Offset: 0x000F87D4
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000FA5D8 File Offset: 0x000F87D8
		public IDisposable Register(Action callback, string context = null)
		{
			BackSystem.Subscription result = new BackSystem.Subscription(callback, context, this._subscriptions);
			if (Application.isEditor)
			{
				Debug.Log(string.Format("<color=lightblue>Back stack after registration: {0}</color>", this));
			}
			return result;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000FA610 File Offset: 0x000F8810
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000FA620 File Offset: 0x000F8820
		private void Update()
		{
			if (BackSystem.EscapePressed())
			{
				Input.ResetInputAxes();
				string arg = (!Application.isEditor) ? string.Empty : this.ToString();
				this.CollectGarbage();
				LinkedListNode<BackSystem.Subscription> last = this._subscriptions.Last;
				if (last == null)
				{
					return;
				}
				last.Value.Invoke();
				if (Application.isEditor)
				{
					Debug.Log(string.Format("<color=#db7093ff>Back stack on invoke: {0} -> {1}</color>", arg, this));
				}
			}
			else if (Input.GetKeyUp(KeyCode.Backspace) && Application.isEditor)
			{
				Debug.Log(string.Format("<color=#db7093ff>Current back stack: {0}</color>", this));
			}
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000FA6C0 File Offset: 0x000F88C0
		private static bool EscapePressed()
		{
			return BackSystem.Active && Input.GetKeyUp(KeyCode.Escape);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000FA6D8 File Offset: 0x000F88D8
		private object ToJson()
		{
			List<string> list = new List<string>(this._subscriptions.Count);
			foreach (BackSystem.Subscription subscription in this._subscriptions)
			{
				if (subscription.Disposed)
				{
					list.Add(subscription.Context + " (Disposed)");
				}
				else
				{
					list.Add(subscription.Context);
				}
			}
			return list;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000FA77C File Offset: 0x000F897C
		private void CollectGarbage()
		{
			for (LinkedListNode<BackSystem.Subscription> last = this._subscriptions.Last; last != null; last = this._subscriptions.Last)
			{
				if (!last.Value.Disposed)
				{
					return;
				}
				this._subscriptions.RemoveLast();
			}
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000FA7C8 File Offset: 0x000F89C8
		private static BackSystem InitializeInstance()
		{
			BackSystem backSystem = UnityEngine.Object.FindObjectOfType<BackSystem>();
			if (backSystem != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(backSystem.gameObject);
				return backSystem;
			}
			GameObject gameObject = new GameObject("Rilisoft.BackSystem");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<BackSystem>();
		}

		// Token: 0x0400233B RID: 9019
		private static readonly Lazy<BackSystem> _instance = new Lazy<BackSystem>(new Func<BackSystem>(BackSystem.InitializeInstance));

		// Token: 0x0400233C RID: 9020
		private readonly LinkedList<BackSystem.Subscription> _subscriptions = new LinkedList<BackSystem.Subscription>();

		// Token: 0x02000569 RID: 1385
		private sealed class Subscription : IDisposable
		{
			// Token: 0x06002FFB RID: 12283 RVA: 0x000FA810 File Offset: 0x000F8A10
			internal Subscription(Action callback, string context, LinkedList<BackSystem.Subscription> list)
			{
				this._callback = callback;
				this._context = (context ?? string.Empty);
				if (list != null)
				{
					this._node = list.AddLast(this);
				}
			}

			// Token: 0x17000847 RID: 2119
			// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000FA848 File Offset: 0x000F8A48
			public string Context
			{
				get
				{
					return this._context;
				}
			}

			// Token: 0x17000848 RID: 2120
			// (get) Token: 0x06002FFD RID: 12285 RVA: 0x000FA850 File Offset: 0x000F8A50
			public bool Disposed
			{
				get
				{
					return this._node == null;
				}
			}

			// Token: 0x06002FFE RID: 12286 RVA: 0x000FA85C File Offset: 0x000F8A5C
			public void Dispose()
			{
				if (this.Disposed)
				{
					return;
				}
				this._callback = null;
				LinkedList<BackSystem.Subscription> list = this._node.List;
				if (list != null)
				{
					list.Remove(this._node);
				}
				this._node = null;
			}

			// Token: 0x06002FFF RID: 12287 RVA: 0x000FA8A4 File Offset: 0x000F8AA4
			public void Invoke()
			{
				if (this.Disposed)
				{
					Debug.LogWarning("Attempt to invoke disposed handler.");
					return;
				}
				if (this._callback != null)
				{
					this._callback();
				}
			}

			// Token: 0x0400233D RID: 9021
			private Action _callback;

			// Token: 0x0400233E RID: 9022
			private readonly string _context;

			// Token: 0x0400233F RID: 9023
			private LinkedListNode<BackSystem.Subscription> _node;
		}
	}
}
