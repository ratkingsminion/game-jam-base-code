﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.Base {
	
	// very simple tweening class, just to be independent from plugins like LeanTween
	[DefaultExecutionOrder(-100000)]
	public class Tweens : MonoBehaviour {

		public static class Ease {

			public static float Linear(float t) {
				return t;
			}

			public static float SmoothStep(float t) {
				return Mathf.SmoothStep(0f, 1f, t);
			}

			// easings from: https://github.com/ai/eaMathf.Sings.net/blob/master/src/eaMathf.Sings/eaMathf.SingsFunctions.ts

			const float PI = Mathf.PI;
			const float c1 = 1.70158f;
			const float c2 = c1 * 1.525f;
			const float c3 = c1 + 1f;
			const float c4 = (2f * PI) / 3f;
			const float c5 = (2f * PI) / 4.5f;

			static float BounceOut(float t) {
				float n1 = 7.5625f;
				float d1 = 2.75f;
				if (t < 1f / d1) { return n1 * t * t; }
				else if (t < 2f / d1) { return n1 * (t -= 1.5f / d1) * t + 0.75f; }
				else if (t < 2.5f / d1) { return n1 * (t -= 2.25f / d1) * t + 0.9375f; }
				return n1 * (t -= 2.625f / d1) * t + 0.984375f;
			}

			public static float InQuad(float t) {
				return t * t;
			}
			public static float OutQuad(float t) {
				return 1f - (1f - t) * (1f - t);
			}
			public static float InOutQuad(float t) {
				return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
			}
			public static float InCubic(float t) {
				return t * t * t;
			}
			public static float OutCubic(float t) {
				return 1f - Mathf.Pow(1f - t, 3f);
			}
			public static float InOutCubic(float t) {
				return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f;
			}
			public static float InQuart(float t) {
				return t * t * t * t;
			}
			public static float OutQuart(float t) {
				return 1f - Mathf.Pow(1f - t, 4f);
			}
			public static float InOutQuart(float t) {
				return t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) * 0.5f;
			}
			public static float InQuint(float t) {
				return t * t * t * t * t;
			}
			public static float OutQuint(float t) {
				return 1f - Mathf.Pow(1f - t, 5f);
			}
			public static float InOutQuint(float t) {
				return t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) * 0.5f;
			}
			public static float InSin(float t) {
				return 1f - Mathf.Cos((t * PI) * 0.5f);
			}
			public static float OutSin(float t) {
				return Mathf.Sin((t * PI) * 0.5f);
			}
			public static float InOutSin(float t) {
				return -(Mathf.Cos(PI * t) - 1f) * 0.5f;
			}
			public static float InExpo(float t) {
				return t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
			}
			public static float OutExpo(float t) {
				return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
			}
			public static float InOutExpo(float t) {
				return t == 0f ? 0f
					: t == 1f ? 1f
					: t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) * 0.5f
					: (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f;
			}
			public static float InCirc(float t) {
				return 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2f));
			}
			public static float OutCirc(float t) {
				return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
			}
			public static float InOutCirc(float t) {
				return t < 0.5f
					? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) * 0.5f
					: (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) * 0.5f;
			}
			public static float InBack(float t) {
				return c3 * t * t * t - c1 * t * t;
			}
			public static float OutBack(float t) {
				return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
			}
			public static float InOutBack(float t) {
				return t < 0.5f
					? (Mathf.Pow(2f * t, 2f) * ((c2 + 1f) * 2f * t - c2)) * 0.5f
					: (Mathf.Pow(2f * t - 2f, 2f) * ((c2 + 1f) * (t * 2f - 2f) + c2) + 2f) * 0.5f;
			}
			public static float InElastic(float t) {
				return t == 0f ? 0f
					: t == 1f ? 1f
					: -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * c4);
			}
			public static float OutElastic(float t) {
				return t == 0f ? 0f
					: t == 1f ? 1f
					: Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
			}
			public static float InOutElastic(float t) {
				return t == 0f ? 0f
					: t == 1f ? 1f
					: t < 0.5f ? -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) * 0.5f
					: (Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) * 0.5f + 1f;
			}
			public static float InBounce(float t) {
				return 1f - BounceOut(1f - t);
			}
			public static float OutBounce(float t) {
				return BounceOut(t);
			}
			public static float InOutBounce(float t) {
				return t < 0.5f ? (1f - BounceOut(1f - 2f * t)) * 0.5f
					: (1f + BounceOut(2f * t - 1f)) * 0.5f;
			}
		}

		// stolen from https://github.com/dentedpixel/LeanTween/blob/master/Assets/LeanTween/Framework/LeanTween.cs#L271
		public static readonly AnimationCurve punch = new AnimationCurve( new Keyframe(0.0f, 0.0f ), new Keyframe(0.112586f, 0.9976035f ), new Keyframe(0.3120486f, -0.1720615f ), new Keyframe(0.4316337f, 0.07030682f ), new Keyframe(0.5524869f, -0.03141804f ), new Keyframe(0.6549395f, 0.003909959f ), new Keyframe(0.770987f, -0.009817753f ), new Keyframe(0.8838775f, 0.001939224f ), new Keyframe(1.0f, 0.0f ) );
    	public static readonly AnimationCurve shake = new AnimationCurve( new Keyframe(0f, 0f), new Keyframe(0.25f, 1f), new Keyframe(0.75f, -1f), new Keyframe(1f, 0f) ) ;

		//

		static int curID = int.MinValue;
		public static int GetNewID() { if (++curID == int.MinValue) { curID++; } return curID; }

		public class Tween {
			public string name = null;
			public int id = int.MinValue;
			public Object uo = null;
			public bool cancelWithObject = false;
			public float factor = 0f;
			public float speed = 0f;
			public float start;
			public float end;
			public AnimationCurve easeCurve;
			public System.Func<float, float> easeFunc;
			public System.Action<float> updateFunc;
			public System.Action completeFunc;
			public float delay = -1f;
			public bool ignoreTimeScale = false;

			public Tween Reset(float start, float end, float seconds) {
				name = null;
				id = GetNewID();
				uo = null;
				cancelWithObject = false;
				factor = 0f;
				this.speed = Mathf.Abs(seconds != 0f ? (1f / seconds) : float.MaxValue);
				this.start = start;
				this.end = end;
				easeFunc = Tweens.Ease.Linear;
				updateFunc = null;
				completeFunc = null;
				delay = -1f;
				ignoreTimeScale = false;
				return this;
			}

			public Tween(float start, float end, float seconds) {
				id = GetNewID();
				this.speed = seconds != 0f ? (1f / seconds) : float.MaxValue;
				this.start = start;
				this.end = end;
				easeFunc = Tweens.Ease.Linear;
			}

			public Tween Ease(AnimationCurve curve) {
				easeFunc = f => curve.Evaluate(f);
				return this;
			}

			public Tween Ease(System.Func<float, float> func) {
				easeFunc = func;
				return this;
			}

			public Tween OnUpdate(System.Action<float> func) {
				updateFunc = func;
				return this;
			}

			public Tween OnComplete(System.Action func) {
				completeFunc = func;
				return this;
			}

			public Tween Initialise() {
				if (updateFunc != null) { updateFunc(0f); }
				return this;
			}

			public Tween Name(string name) {
				this.name = name;
				return this;
			}

			public Tween UnityObject(Object uo, bool cancelWith = true) {
				this.uo = uo;
				this.cancelWithObject = cancelWith && uo != null;
				return this;
			}

			// set to -1f if you want no delay, 0f if you want only a frame of delay
			public Tween Delay(float delay) {
				this.delay = delay;
				return this;
			}

			public Tween IgnoreTimeScale(bool ignoreTimeScale) {
				this.ignoreTimeScale = ignoreTimeScale;
				return this;
			}
		}

		//

		static readonly Stack<Tween> poolTweens = new Stack<Tween>(64);
		static void PoolPushTween(Tween t) {
			t.id = int.MinValue;
			if (curTweens.Remove(t)) { poolTweens.Push(t); }
		}
		static Tween PoolPopTween(float start, float end, float seconds) {
			var t = (poolTweens.Count == 0) ? new Tween(start, end, seconds) : poolTweens.Pop().Reset(start, end, seconds);
			newTweens.Add(t);
			return t;
		}
		static readonly List<Tween> newTweens = new List<Tween>(64);
		static readonly List<Tween> curTweens = new List<Tween>(64);
		static readonly List<Tween> updTweens = new List<Tween>(64);
		static Tweens inst = null;
		
		static void CreateInstance() {
			var go = new GameObject("<TWEENS>");
			DontDestroyOnLoad(go);
			inst = go.AddComponent<Tweens>();
		}

		//
			
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void OnRuntimeInitializeOnLoad() {
			curID = int.MinValue;
			poolTweens.Clear();
			newTweens.Clear();
			curTweens.Clear();
			updTweens.Clear();
			inst = null;
		}

		//

		void Update() {
#if UNITY_EDITOR
			name = "<TWEENS> Count:" + curTweens.Count + (newTweens.Count > 0 ? " + New: " + newTweens.Count : "");
#endif
			if (newTweens.Count > 0) {
				// foreach (var t in newTweens) { if (t.id == int.MinValue) { t.id = GetNewID(); } }
				curTweens.AddRange(newTweens);
				newTweens.Clear();
			}
			updTweens.Clear();
			updTweens.AddRange(curTweens);
			foreach (var t in updTweens) {
				if (t.cancelWithObject && t.uo == null) {
					PoolPushTween(t);
					continue;
				}
				var dt = t.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
				if (t.delay >= 0f) { t.delay -= dt; continue; }
				t.factor = Mathf.Clamp01(t.factor + dt * t.speed);
				if (t.id > int.MinValue && t.updateFunc != null) {
					var e = t.easeFunc(t.factor);
					t.updateFunc(t.start * (1f - e) + t.end * e);
				}
				if (t.factor >= 1f) { // done
					if (t.id > int.MinValue && t.completeFunc != null) { t.completeFunc(); }
					PoolPushTween(t);
				}
			}
		}

		//

		public static Tween NextFrame(System.Action completeFunc) {
			if (inst == null) { CreateInstance(); }
			var tween = PoolPopTween(0f, 0f, 0f);
			tween.completeFunc = completeFunc;
			tween.ignoreTimeScale = true;
			return tween;
		}

		public static Tween Timer(float seconds, System.Action completeFunc) {
			if (inst == null) { CreateInstance(); }
			var tween = PoolPopTween(0f, 0f, 0f);
			tween.delay = seconds;
			tween.completeFunc = completeFunc;
			return tween;
		}

		public static Tween Do(float seconds, System.Action<float> updateFunc = null) {
			if (inst == null) { CreateInstance(); }
			var tween = PoolPopTween(0f, 1f, seconds);
			tween.updateFunc = updateFunc;
			return tween;
		}

		public static Tween Do(float start, float end, float seconds, System.Action<float> updateFunc = null) {
			if (inst == null) { CreateInstance(); }
			var tween = PoolPopTween(start, end, seconds);
			tween.updateFunc = updateFunc;
			return tween;
		}

		public static void StopAll() {
			if (inst == null) { return; }
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				var t = curTweens[i];
				if (!poolTweens.Contains(t)) { poolTweens.Push(t); }
			}
			curTweens.Clear();
			newTweens.Clear();
		}

		public static bool Stop(Tween tween, bool withComplete = false) {
			if (inst == null || tween == null) { return false; }
			if (!curTweens.Remove(tween)) { return false; }
			if (withComplete && tween.completeFunc != null) { tween.completeFunc(); }
			PoolPushTween(tween);
			return true;
		}

		public static bool Stop(int id, bool withComplete = false) {
			if (inst == null) { return false; }
			var removed = false;
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				var t = curTweens[i];
				if (t.id != id) { continue; }
				if (withComplete && t.completeFunc != null) { t.completeFunc(); }
				PoolPushTween(t);
				removed = true;
			}
			return removed;
		}

		public static bool Stop(string name, bool withComplete = false) {
			if (inst == null) { return false; }
			var removed = false;
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				var t = curTweens[i];
				if (t.name != name) { continue; }
				if (withComplete && t.completeFunc != null) { t.completeFunc(); }
				PoolPushTween(t);
				removed = true;
			}
			return removed;
		}

		public static bool Stop(Object uo, bool withComplete = false) {
			if (inst == null || uo.IsGone()) { return false; }
			var removed = false;
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				var t = curTweens[i];
				if (t.uo != uo) { continue; }
				if (withComplete && t.completeFunc != null) { t.completeFunc(); }
				PoolPushTween(t);
				removed = true;
			}
			return removed;
		}

		public static bool Stop(Object uo, string name, bool withComplete = false) {
			if (inst == null || uo.IsGone()) { return false; }
			var removed = false;
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				var t = curTweens[i];
				if (t.uo != uo || t.name != name) { continue; }
				if (withComplete && t.completeFunc != null) { t.completeFunc(); }
				PoolPushTween(t);
				removed = true;
			}
			return removed;
		}

		public static bool IsTweening(int id) {
			if (inst == null) { return false; }
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				if (curTweens[i].id == id) { return true; }
			}
			return false;
		}

		public static bool IsTweening(string name) {
			if (inst == null) { return false; }
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				if (curTweens[i].name == name) { return true; }
			}
			return false;
		}

		public static bool IsTweening(Object uo) {
			if (inst == null) { return false; }
			for (int i = curTweens.Count - 1; i >= 0; --i) {
				if (curTweens[i].uo == uo) { return true; }
			}
			return false;
		}
	}

}