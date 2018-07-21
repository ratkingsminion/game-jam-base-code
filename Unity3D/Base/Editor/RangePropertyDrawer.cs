﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RatKing.Base {

	[CustomPropertyDrawer(typeof(RangeInt))]
	[CustomPropertyDrawer(typeof(RangeFloat))]
	public class RangePropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var labelW = 30;
			var width = position.width * 0.5f - labelW;

			var minLabelRect = new Rect(position.x, position.y, labelW, position.height);
			var minRect			= new Rect(position.x + labelW, position.y, width, position.height);
			var maxLabelRect	= new Rect(position.x + width + labelW, position.y, labelW, position.height);
			var maxRect			= new Rect(position.x + width + labelW * 2, position.y, width, position.height);
			
			EditorGUI.PrefixLabel(minLabelRect, new GUIContent("min"));
			EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);

			EditorGUI.PrefixLabel(maxLabelRect, new GUIContent("max"));
			EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}

}