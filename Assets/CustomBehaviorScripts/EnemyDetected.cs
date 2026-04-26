using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/EnemyDetected")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "EnemyDetected", message: "[Agent] has spotted [Enemy]", category: "Events", id: "9ebdc41bd97c52b662ee2131e7774802")]
public sealed partial class EnemyDetected : EventChannel<GameObject, GameObject> { }

