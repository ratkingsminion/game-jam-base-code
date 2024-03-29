﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.Base {

	public class Sounds : MonoBehaviour {

		public class SoundProperties {
			public float waiting = -1f;
			public int lastRandomIndex = -1;
		}

		public static Sounds Inst { get; private set; }
		static float globalVolume = 1f;

		Transform parentPool;
		Transform parentPlay;
		readonly Dictionary<SoundType, Stack<Sound>> curPooled = new Dictionary<SoundType, Stack<Sound>>();
		Sound globalPrefab;
		readonly Dictionary<SoundType, Sound> typedPrefabs = new Dictionary<SoundType, Sound>();
		readonly List<Sound> playingSounds = new List<Sound>();
		readonly Dictionary<SoundType, SoundProperties> soundsProperties = new Dictionary<SoundType, SoundProperties>();
		readonly HashSet<Sound> globallyPausedSounds = new HashSet<Sound>();
		
#if UNITY_EDITOR
		int prevCount = -1;
#endif

		//

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void OnRuntimeInitializeOnLoad() {
			Inst = null;
			globalVolume = 1f;
		}

		//

		public static Sounds GetOrCreateInstance() {
			if (Inst != null) { return Inst; }
			return new GameObject("<SOUNDS>").AddComponent<Sounds>();
		}

		//

		void Awake() {
			DontDestroyOnLoad(gameObject);
			Inst = this;
			parentPool = new GameObject("POOL").transform;
			parentPool.SetParent(transform);
			parentPool.gameObject.SetActive(false);
			parentPlay = new GameObject("PLAY").transform;
			parentPlay.SetParent(transform);
		}

		void Update() {
			var count = playingSounds.Count;
#if UNITY_EDITOR
			if (prevCount != count) { name = "<SOUNDS> Types:" + typedPrefabs.Count + " Playing:" + count; prevCount = count; }
#endif
			for (int i = count - 1; i >= 0; --i) {
				var sound = playingSounds[i];
				if (sound == null) {
					playingSounds.RemoveAt(i);
					continue;
				}
				if (globallyPausedSounds.Contains(sound)) {
					continue;
				}
				if (!sound.InUse || (!sound.IsPaused && !sound.IsPlaying)) {
					playingSounds.RemoveAt(i);
					globallyPausedSounds.Remove(sound);
					curPooled[sound.Type].Push(sound);
					sound.transform.SetParent(parentPool);
					continue;
				} 
				if (sound.Follow != null) {
					sound.transform.position = sound.Follow.position;
				}
			}
		}

		//

		public static float GlobalVolume {
			get {
				return globalVolume;
			}
			set {
				globalVolume = value;
				if (Inst == null) { return; }
				foreach (var sound in Inst.playingSounds) {
					sound.Volume = sound.Volume;
				}
			}
		}

		public static int PauseSeveral(string tag = null) {
			if (Inst == null) { return 0; }
			var soundsToPause =
				tag == null
				? Inst.playingSounds
				: Inst.playingSounds.FindAll(s => s.Tag == tag);
			Inst.globallyPausedSounds.UnionWith(soundsToPause);
			foreach (var stp in soundsToPause) {
				stp.GetComponent<AudioSource>().Pause();
			}
			return soundsToPause.Count;
		}

		public static int ResumeSeveral(string tag = null) {
			if (Inst == null) { return 0; }
			var soundsToResume =
				tag == null
				? Inst.playingSounds
				: Inst.playingSounds.FindAll(s => s.Tag == tag);
			Inst.globallyPausedSounds.ExceptWith(soundsToResume);
			foreach (var str in soundsToResume) {
				if (!str.IsPaused) { str.GetComponent<AudioSource>().Play(); }
			}
			return soundsToResume.Count;
		}

		public static int StopSeveral(string tag = null) {
			if (Inst == null) { return 0; }
			var soundsToStop =
				tag == null
				? Inst.playingSounds
				: Inst.playingSounds.FindAll(s => s.Tag == tag);
			foreach (var sts in soundsToStop) {
				sts.Stop();
			}
			return soundsToStop.Count;
		}

		//

		public Sound Play(SoundType type, Vector3 pos = default) {
			if (type == null) { return null; }
			//
			var mightBeWaiting = type.WaitSeconds.min > 0f;
			var clipsCount = type.Clips.Length;
			SoundProperties props = null;
			if (mightBeWaiting || clipsCount > 1) {
				if (!soundsProperties.TryGetValue(type, out props)) {
					soundsProperties[type] = props = new SoundProperties();
				}
			}
			//
			if (mightBeWaiting && props.waiting > Time.unscaledTime) { return null; }
			//
			if (!curPooled.TryGetValue(type, out Stack<Sound> pool)) { curPooled.Add(type, pool = new Stack<Sound>()); }
			if (pool.Count == 0) {
				var prefab = GetOrCreateTypedPrefab(type, pool);
				while (pool.Count < type.PoolAddCount) { InstantiateTypedPrefabInPool(type, prefab, pool); }
			}
			//
			var sound = curPooled[type].Pop();
			sound.transform.SetParent(parentPlay);
			int clipIndex = 0;
			if (clipsCount > 1) {
				props.lastRandomIndex = clipIndex
					= props.lastRandomIndex < 0
					? Random.Range(1, clipsCount)
					: (props.lastRandomIndex + Random.Range(1, clipsCount)) % clipsCount;
			}
			sound.PlayType(type, clipIndex, pos);
			playingSounds.Add(sound);
			if (mightBeWaiting) { props.waiting = Time.unscaledTime + type.WaitSeconds.Random(); }
			return sound;
		}

		//

		public float GetWaitTimeOf(SoundType type) {
			if (!soundsProperties.TryGetValue(type, out SoundProperties props)) { return 0f; }
			return Mathf.Max(0f, props.waiting - Time.unscaledTime);
		}

		//

		void CreateGlobalPrefab() {
			var pgo = new GameObject("<Global Prefab Sound>");
			pgo.transform.SetParent(parentPool);
#if UNITY_EDITOR
			pgo.hideFlags = HideFlags.HideAndDontSave;
#endif
			var source = pgo.AddComponent<AudioSource>();
			source.playOnAwake = false;
			globalPrefab = pgo.AddComponent<Sound>();
		}

		Sound GetOrCreateTypedPrefab(SoundType type, Stack<Sound> pool) {
			if (type == null) { return null; }
			if (typedPrefabs.TryGetValue(type, out Sound prefab)) { return prefab; }
			if (globalPrefab == null) { CreateGlobalPrefab(); }
			prefab = Instantiate(globalPrefab);
#if UNITY_EDITOR
			prefab.name = "<Prefab " + type.name + ">";
			prefab.gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
			var source = prefab.GetComponent<AudioSource>();
			source.priority = (int)((1f - type.Priority) * 256f);
			source.loop = type.Loop;
			source.panStereo = type.Pan;
			source.spatialBlend = type.SpatialBlend;
			if (type.SpatialBlend > 0f) {
				source.spread = type.Spread3D;
				source.dopplerLevel = type.DopplerLevel;
				source.rolloffMode = AudioRolloffMode.Linear;
				source.minDistance = type.Distance3D.min;
				source.maxDistance = type.Distance3D.max;
			}
			// TODO: more specific things of sound prefab - linear and so on
			typedPrefabs.Add(type, prefab);
			for (int i = 0; i < type.PoolStartCount; ++i) { InstantiateTypedPrefabInPool(type, prefab, pool); }
			return prefab;
		}

		Sound InstantiateTypedPrefabInPool(SoundType type, Sound prefab, Stack<Sound> pool) {
			var sound = Instantiate(prefab, parentPool);
#if UNITY_EDITOR
			sound.name = type.name;
#endif
			pool.Push(sound);
			return sound;
		}
	}

}