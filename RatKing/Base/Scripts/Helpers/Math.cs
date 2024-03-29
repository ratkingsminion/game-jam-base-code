﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.Base {
	
	public static class Math {
		// vector3 stuff:
		public static Vector3 Abs(this Vector3 v) {
			return new Vector3(v.x > 0f ? v.x : -v.x, v.y > 0f ? v.y : -v.y, v.z > 0f ? v.z : -v.z);
		}
		public static float GetManhattanDistance(this Vector3 a, Vector3 b) {
			return (Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y) + Mathf.Abs(b.z - a.z));
		}
		public static Vector3 Vec3Mul(this Vector3 a, Vector3 b) {
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		public static Vector3 Vec3Div(this Vector3 a, Vector3 b) {
			return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
		}
		public static Vector3 Vec3Div(float a, Vector3 b) {
			return new Vector3(a / b.x, a / b.y, a / b.z);
		}
		public static Vector3 Vec3Div(this Vector3 a, float b) {
			return new Vector3(a.x / b, a.y / b, a.z / b);
		}
		public static Vector3 Rotate(this Vector3 vector, Vector3 axis, float angle) {
			return Quaternion.AngleAxis(angle, axis) * vector;
		}
		public static Vector3 Rotate(this Vector3 vector, float x, float y, float z) {
			return Quaternion.Euler(x, y, z) * vector;
		}
		public static Vector3 RotateAround(this Vector3 vector, Vector3 origin, Vector3 axis, float angle) {
			return origin + Quaternion.AngleAxis(angle, axis) * (vector - origin);
		}
		public static Vector3 Rotate(this Vector3 vector, Vector3 origin, float x, float y, float z) {
			return origin + Quaternion.Euler(x, y, z) * (vector - origin);
		}
		// from http://forum.unity3d.com/threads/33215-Vector-rotation
		public static float GetPitch(this Vector3 v) {
			float len = Mathf.Sqrt((v.x * v.x) + (v.z * v.z));
			return -Mathf.Atan2(v.y, len);
		}
		public static float GetYaw(this Vector3 v) {
			return Mathf.Atan2(v.x, v.z);
		}
		public static Vector3 QuantizeRound(Vector3 v, float e = 1f) {
			float ie = 1f / e;
			v.x = Mathf.Round(v.x * ie) * e;
			v.y = Mathf.Round(v.y * ie) * e;
			v.z = Mathf.Round(v.z * ie) * e;
			return v; //.normalized;
		}
		public static Vector3 QuantizeFloor(Vector3 v, float e = 1f) {
			float ie = 1f / e;
			v.x = Mathf.Floor(v.x * ie) * e;
			v.y = Mathf.Floor(v.y * ie) * e;
			v.z = Mathf.Floor(v.z * ie) * e;
			return v;
		}
		// vector2 stuff:
		public static Vector2 Rotate(this Vector2 vector, float angle) {
			return Quaternion.Euler(0f, 0f, angle) * vector;
		}
		// https://answers.unity.com/questions/1229302/rotate-a-vector2-around-the-z-axis-on-a-mathematic.html
		public static Vector2 RotateAroundPivot(this Vector2 vector, Vector2 pivot, float angle) {
			return Quaternion.Euler(0f, 0f, angle) * (vector - pivot) + new Vector3(pivot.x, pivot.y);
			/*
			vector -= pivot;
			float rad = degrees * Mathf.Deg2Rad;
			float s = Mathf.Sin(rad);
			float c = Mathf.Cos(rad);
			return pivot + new Vector2(vector.x * c - vector.y * s, vector.y * c + vector.x * s);
			*/
		}
		// quaternion stuff:
		public static float Length(this Quaternion q) {
			return Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
		}
		public static void Normalize(ref Quaternion q) {
			float l = 1f / Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
			q.w *= l; q.x *= l; q.y *= l; q.z *= l;
		}
		public static Vector3 Slerp(Vector3 a, Vector3 b, float t) {
			var qa = Quaternion.FromToRotation(a, b);
			return Slerp(Quaternion.identity, qa, t) * a;
		}
		// from http://www.euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/slerp/
		// has auto normalization
		public static Quaternion Slerp(Quaternion qa, Quaternion qb, float t) {
			// quaternion to return
			// Calculate angle between them.
			float cosHalfTheta = qa.w * qb.w + qa.x * qb.x + qa.y * qb.y + qa.z * qb.z;
			//Debug.Log (	Mathf.Sqrt(qa.w * qa.w + qa.x * qa.x + qa.y * qa.y + qa.z * qa.z) + " - " + 
			//			Mathf.Sqrt(qb.w * qb.w + qb.x * qb.x + qb.y * qb.y + qb.z * qb.z));
			// if qa=qb or qa=-qb then theta = 0 and we can return qa
			if (Mathf.Abs(cosHalfTheta) >= 1f) {
				//qm.w = qa.w;qm.x = qa.x;qm.y = qa.y;qm.z = qa.z;
				return qa;
			}
			// Calculate temporary values.
			float halfTheta = Mathf.Acos(cosHalfTheta);
			float sinHalfTheta = Mathf.Sqrt(1f - cosHalfTheta * cosHalfTheta);
			Quaternion qm = new Quaternion();
			// if theta = 180 degrees then result is not fully defined
			// we could rotate around any axis normal to qa or qb
			if (Mathf.Abs(sinHalfTheta) < 0.001f) {
				qm.w = (qa.w * 0.5f + qb.w * 0.5f);
				qm.x = (qa.x * 0.5f + qb.x * 0.5f);
				qm.y = (qa.y * 0.5f + qb.y * 0.5f);
				qm.z = (qa.z * 0.5f + qb.z * 0.5f);
			}
			else {
				float ratioA = Mathf.Sin((1f - t) * halfTheta) / sinHalfTheta;
				float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta;
				//calculate Quaternion.
				qm.w = (qa.w * ratioA + qb.w * ratioB);
				qm.x = (qa.x * ratioA + qb.x * ratioB);
				qm.y = (qa.y * ratioA + qb.y * ratioB);
				qm.z = (qa.z * ratioA + qb.z * ratioB);
			}
			float il = 1f / Mathf.Sqrt(qm.w * qm.w + qm.x * qm.x + qm.y * qm.y + qm.z * qm.z);
			qm.w *= il; qm.x *= il; qm.y *= il; qm.z *= il;
			return qm;
		}
		// transitions:
		public static float Hermite(float start, float end, float value) {
			return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
		}
		public static float Sinerp(float start, float end, float value) {
			return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
		}
		public static float Coserp(float start, float end, float value) {
			return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * Mathf.PI * 0.5f));
		}
		public static float Berp(float start, float end, float value) {
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}
		public static float SmoothStep(float x, float min, float max) {
			x = Mathf.Clamp(x, min, max);
			float v1 = (x - min) / (max - min);
			float v2 = (x - min) / (max - min);
			return -2 * v1 * v1 * v1 + 3 * v2 * v2;
		}
		public static float Lerp(float start, float end, float value) {
			return ((1f - value) * start) + (value * end);
		}
		
		// from https://stackoverflow.com/questions/64663170/how-to-find-nearest-point-in-segment-in-a-3d-space
		public static float SegmentNearestPointDistance(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			var s = lineEnd - lineStart;
			var w = point - lineStart;
			var ps = Vector3.Dot(w, s);
			if (ps <= 0f) { return w.magnitude; }
			var l2 = Vector3.Dot(s, s);
			var closest = ps >= l2 ? lineEnd : (lineStart + ps / l2 * s);
			return Vector3.Distance(point, closest);
		}
		public static Vector2 SegmentNearestPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point) {
			var fullDirection = lineEnd - lineStart;
			var lineDirection = fullDirection.normalized;
			var closestPoint = Vector2.Dot((point - lineStart), lineDirection) / lineDirection.sqrMagnitude;
			return lineStart + Mathf.Clamp(closestPoint, 0f, fullDirection.magnitude) * lineDirection;
		}
		public static Vector3 SegmentNearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			var fullDirection = lineEnd - lineStart;
			var lineDirection = fullDirection.normalized;
			var closestPoint = Vector3.Dot((point - lineStart), lineDirection) / lineDirection.sqrMagnitude;
			return lineStart + Mathf.Clamp(closestPoint, 0f, fullDirection.magnitude) * lineDirection;
		}
		
		public static float LineNearestPointDistance(Ray ray, Vector3 point) {
			return Vector3.Dot((point - ray.origin), ray.direction) / Vector3.Dot(ray.direction, ray.direction);
		}
		public static float LineNearestPointDistance(Vector2 lineStart, Vector2 lineDirection, Vector2 point) {
			return Vector2.Dot((point - lineStart), lineDirection) / Vector2.Dot(lineDirection, lineDirection);
		}
		public static Vector3 LineNearestPoint(Ray ray, Vector3 point) {
			var closestPoint = Vector3.Dot((point - ray.origin), ray.direction);
			return ray.origin + (closestPoint * ray.direction);
		}
		public static Vector2 LineNearestPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point) {
			var lineDirection = (lineEnd - lineStart).normalized;
			var closestPoint = Vector3.Dot((point - lineStart), lineDirection);
			return lineStart + (closestPoint * lineDirection);
		}
		public static Vector3 LineNearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			var lineDirection = (lineEnd - lineStart).normalized;
			var closestPoint = Vector3.Dot((point - lineStart), lineDirection);
			return lineStart + (closestPoint * lineDirection);
		}
		
		public static float Bounce(float x) {
			return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
		}
		/*
		* CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
		* This is useful when interpolating eulerAngles and the object
		* crosses the 0/360 boundary.  The standard Lerp function causes the object
		* to rotate in the wrong direction and looks stupid. Clerp fixes that.
		*/
		public static float Clerp(float start, float end, float value) {
			float min = 0f;
			float max = 360f;
			float half = Mathf.Abs((max - min) / 2f);//half the distance between min and max

			if ((end - start) < -half) {
				return start + ((max - start) + end) * value;
			}
			else if ((end - start) > half) {
				return start - ((max - end) + start) * value;
			}
			return start + (end - start) * value;
		}
		// from http://pastebin.com/NZrstYL4
		public static Vector3 Qarp(Vector3 a, Vector3 b, Vector3 c, float blend) {
			Vector3 a_b = Vector3.Lerp(a, b, blend);
			return a_b + (Vector3.Lerp(b, c, blend) - a_b) * blend;
		}
		// from http://pastebin.com/NZrstYL4
		public static Vector3 SuperLerp(Vector3 p0, Vector3 p1, Vector3 v0, Vector3 v1, float l) {
			return Qarp(p0, (p0 + v0 * 0.5f + p1 - v1 * 0.5f) / 2f, p1, l);
		}
		//
		// frame independent lerping
		public static float FrinLerp(float a, float b, float t, float hertz = 60f) {
			t = Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz);
			return t * a + (1f - t) * b;
		}
		public static float FrinLerp(float a, float b, float t, float hertz, float dt) {
			t = Mathf.Pow(1f - t, dt * hertz);
			return t * a + (1f - t) * b;
		}
		public static float FrinLerpClamped(float a, float b, float t, float hertz = 60f) {
			t = Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz);
			return Mathf.Clamp(t * a + (1f - t) * b, a, b);
		}
		public static Vector3 FrinLerp(Vector3 a, Vector3 b, float t, float hertz = 60f) {
			t = Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz);
			return new Vector3(
				t * a.x + (1f - t) * b.x,
				t * a.y + (1f - t) * b.y,
				t * a.z + (1f - t) * b.z
				);
		}
		public static Vector4 FrinLerp(Vector4 a, Vector4 b, float t, float hertz = 60f) {
			t = Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz);
			return new Vector4(
				t * a.x + (1f - t) * b.x,
				t * a.y + (1f - t) * b.y,
				t * a.z + (1f - t) * b.z,
				t * a.w + (1f - t) * b.w
				);
		}
		public static Vector3 FrinLerp(Vector3 a, Vector3 b, float t, float hertz, float dt) {
			t = Mathf.Pow(1f - t, dt * hertz);
			return new Vector3(
				t * a.x + (1f - t) * b.x,
				t * a.y + (1f - t) * b.y,
				t * a.z + (1f - t) * b.z
				);
		}
		public static Vector3 FrinSlerp(Vector3 a, Vector3 b, float t, float hertz = 60f) {
			return Vector3.Slerp(a, b, 1f - Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz));
		}
		public static Vector3 FrinSlerp(Vector3 a, Vector3 b, float t, float hertz, float dt) {
			return Vector3.Slerp(a, b, 1f - Mathf.Pow(1f - t, dt * hertz));
		}
		public static Quaternion FrinSlerp(Quaternion a, Quaternion b, float t, float hertz = 60f) {
			return Quaternion.Slerp(a, b, 1f - Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz));
		}
		public static Quaternion FrinSlerp(Quaternion a, Quaternion b, float t, float hertz, float dt) {
			return Quaternion.Slerp(a, b, 1f - Mathf.Pow(1f - t, dt * hertz));
		}
		public static Quaternion FrinSlerpAdd(Quaternion a, Quaternion b, float t, float hertz = 60f) {
			return Quaternion.Slerp(a, b * a, 1f - Mathf.Pow(1f - t, Time.unscaledDeltaTime * hertz));
		}
		public static Quaternion FrinSlerpAdd(Quaternion a, Quaternion b, float t, float hertz, float dt) {
			return Quaternion.Slerp(a, b * a, 1f - Mathf.Pow(1f - t, dt * hertz));
		}
		//
		public static bool Approx(this float f1, float f2, float epsilon = 0.01f) {
			return f1 < f2 + epsilon && f1 > f2 - epsilon;
		}
		public static bool Approx(this Vector3 v1, Vector3 v2, float epsilon = 0.01f) {
			return v1.x < v2.x + epsilon && v1.x > v2.x - epsilon &&
					v1.y < v2.y + epsilon && v1.y > v2.y - epsilon &&
					v1.z < v2.z + epsilon && v1.z > v2.z - epsilon;
		}
		public static bool Approx(this Vector4 v1, Vector4 v2, float epsilon = 0.01f) {
			return v1.x < v2.x + epsilon && v1.x > v2.x - epsilon &&
					v1.y < v2.y + epsilon && v1.y > v2.y - epsilon &&
					v1.z < v2.z + epsilon && v1.z > v2.z - epsilon &&
					v1.w < v2.w + epsilon && v1.w > v2.w - epsilon;
		}
		// remapping, from https://stackoverflow.com/questions/5294955/how-to-scale-down-a-range-of-numbers-with-a-known-min-and-max-value
		public static float Remap(this float value, float oldMin, float oldMax, float newMin, float newMax) {
			return ((newMax - newMin) * (value - oldMin) / (oldMax - oldMin)) + newMin;
		}
		public static float RemapClamped(this float value, float oldMin, float oldMax, float newMin, float newMax) {
			return ((newMax - newMin) * Mathf.Clamp01((value - oldMin) / (oldMax - oldMin))) + newMin;
		}

		// from https://answers.unity.com/questions/390291/is-there-a-way-to-smoothdamp-a-lookat.html?childToView=1486611#answer-1486611
		public static Quaternion SmoothDamp(Quaternion a, Quaternion b, ref float curVel, float smoothTime, float maxSpeed = float.MaxValue) {
			float delta = Quaternion.Angle(a, b);
			if (delta == 0f) { return a; }
		    float t = Mathf.SmoothDampAngle(delta, 0.0f, ref curVel, smoothTime, maxSpeed);
		    return Quaternion.Slerp(a, b, 1.0f - (t / delta));
		}
		public static Quaternion SmoothDamp(Quaternion a, Quaternion b, ref float curVel, float smoothTime, float maxSpeed, float deltaTime) {
			float delta = Quaternion.Angle(a, b);
			if (delta == 0f) { return a; }
		    float t = Mathf.SmoothDampAngle(delta, 0.0f, ref curVel, smoothTime, maxSpeed, deltaTime);
		    return Quaternion.Slerp(a, b, 1.0f - (t / delta));
		}
		
		//

		public static void Swap<T>(ref T a, ref T b) {
			var t = a; a = b; b = t;
		}
	}

}