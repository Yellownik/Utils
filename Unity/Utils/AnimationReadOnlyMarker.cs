using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// Marks animations and states of Animators as readonly in TargetFolder.
// Used to prevent changing default animations those used accross a project
public static class AnimationReadOnlyMarker
{
    private const string TargetFolder = "Assets/_Project/_DefaultAnimations/";

    [MenuItem("Tools/" + nameof(MarkReadOnlyClips))]
    public static void MarkReadOnlyClips()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:AnimationClip"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.StartsWith(TargetFolder))
                continue;
            
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            clip.hideFlags |= HideFlags.NotEditable;
            EditorUtility.SetDirty(clip);
        }
        
        AssetDatabase.SaveAssets();
    }
    
        
    [MenuItem("Tools/" + nameof(MarkReadOnlyAnimators))]
    public static void MarkReadOnlyAnimators()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:AnimatorController"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.StartsWith(TargetFolder))
                continue;
            
            var animator = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            animator.hideFlags |= HideFlags.NotEditable;
            EditorUtility.SetDirty(animator);

            MarkStatesReadOnly(animator);
        }
        
        AssetDatabase.SaveAssets();
        
        void MarkStatesReadOnly(AnimatorController controller)
        {
            foreach (var layer in controller.layers)
            {
                var sm = layer.stateMachine;
                foreach (var child in sm.states)
                {
                    var state = child.state;
                    state.hideFlags |= HideFlags.NotEditable;
                    EditorUtility.SetDirty(state);
                }

                foreach (ChildAnimatorStateMachine childSm in sm.stateMachines) 
                    MarkStateMachineRecursive(childSm.stateMachine);
            }
        }
        
        void MarkStateMachineRecursive(AnimatorStateMachine sm)
        {
            foreach (var child in sm.states)
            {
                var state = child.state;
                state.hideFlags |= HideFlags.NotEditable;
                EditorUtility.SetDirty(state);
            }
        
            foreach (var childSm in sm.stateMachines) 
                MarkStateMachineRecursive(childSm.stateMachine);
        }
    }
}
