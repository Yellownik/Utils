using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// Marks animations and states of Animators as readonly in TargetFolder.
// Used to prevent changing default animations those used accross a project
public static class AnimationReadOnlyMarker
{
    private const string TargetFolder = "Assets/_Project/_DefaultAnimations/";

    private static void ChangeHideFlags(Object obj)
    {
        obj.hideFlags |= HideFlags.NotEditable;
        //obj.hideFlags &= ~HideFlags.NotEditable;
        
        EditorUtility.SetDirty(obj);
    }
    
    [MenuItem("Tools/" + nameof(MarkReadOnlyClips))]
    public static void MarkReadOnlyClips()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:AnimationClip"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.StartsWith(TargetFolder))
                continue;
            
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            ChangeHideFlags(clip);
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
            ChangeHideFlags(animator);

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
                    ChangeHideFlags(state);
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
                ChangeHideFlags(state);
            }
        
            foreach (var childSm in sm.stateMachines) 
                MarkStateMachineRecursive(childSm.stateMachine);
        }
    }
}
