"""
import_idle_sprites.py
Wave-1 character idle sprite import script for RIMA Unity project.
Copies PNGs, creates .meta files, writes/updates .anim files, creates .controller files.
"""

import os
import shutil
import uuid

PROJECT_ROOT = r"F:/Antigravity Projeler/2d roguelite/RIMA"

CLASSES = ["warblade", "elementalist", "ranger", "shadowblade"]
DIRECTIONS = ["south", "north", "east", "west"]

# Elementalist existing .anim GUIDs (from .meta files — do NOT regenerate)
ELEMENTALIST_ANIM_GUIDS = {
    "south": "24a2d6a8baad4ad4a88c56b2d90bff22",
    "north": "360d728ee5db8c149b909c60b1e171fe",
    "east":  "8a16b8e5cbd155840bd8eeb88b72393a",
    "west":  "bdf1134f2f0efe246b34aeb01cdc6617",
}

def gen_guid():
    return uuid.uuid4().hex

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

def png_meta_content(guid, class_name, direction):
    sprite_name = f"{class_name}_idle_{direction}"
    return f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  fileIDToRecycleName:
    21300000: {sprite_name}
  externalObjects: {{}}
  serializedVersion: 11
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 0
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {{x: 0.5, y: 0.5}}
  spritePixelsToUnits: 64
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  platformSettings:
  - serializedVersion: 3
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    spriteID: 5e97eb03825dee720800000000000000
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
  spritePackingTag:
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData:
  assetBundleName:
  assetBundleVariant:
"""

def anim_content(class_name, direction, sprite_guid):
    clip_name = f"{class_name}_idle_{direction}"
    return f"""%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!74 &7400000
AnimationClip:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: {clip_name}
  serializedVersion: 7
  m_Legacy: 0
  m_Compressed: 0
  m_UseHighQualityCurve: 1
  m_RotationCurves: []
  m_CompressedRotationCurves: []
  m_EulerCurves: []
  m_PositionCurves: []
  m_ScaleCurves: []
  m_FloatCurves: []
  m_PPtrCurves:
  - curve:
    - time: 0
      value: {{fileID: 21300000, guid: {sprite_guid}, type: 3}}
    attribute: m_Sprite
    path:
    classID: 212
    script: {{fileID: 0}}
  m_SampleRate: 1
  m_WrapMode: 0
  m_Bounds:
    m_Center: {{x: 0, y: 0, z: 0}}
    m_Extent: {{x: 0, y: 0, z: 0}}
  m_ClipBindingConstant:
    genericBindings:
    - serializedVersion: 2
      path: 0
      attribute: 0
      script: {{fileID: 0}}
      typeID: 212
      customType: 23
      isPPtrCurve: 1
    pptrCurveMapping:
    - {{fileID: 21300000, guid: {sprite_guid}, type: 3}}
  m_AnimationClipSettings:
    serializedVersion: 2
    m_AdditiveReferencePoseClip: {{fileID: 0}}
    m_AdditiveReferencePoseTime: 0
    m_StartTime: 0
    m_StopTime: 1
    m_OrientationOffsetY: 0
    m_Level: 0
    m_CycleOffset: 0
    m_HasAdditiveReferencePose: 0
    m_LoopTime: 1
    m_LoopBlend: 0
    m_LoopBlendOrientation: 0
    m_LoopBlendPositionY: 0
    m_LoopBlendPositionXZ: 0
    m_KeepOriginalOrientation: 0
    m_KeepOriginalPositionY: 1
    m_KeepOriginalPositionXZ: 0
    m_HeightFromFeet: 0
    m_Mirror: 0
  m_EditorCurves: []
  m_EulerEditorCurves: []
  m_HasGenericRootTransform: 0
  m_HasMotionFloatCurves: 0
  m_Events: []
"""

def anim_meta_content(guid):
    return f"""fileFormatVersion: 2
guid: {guid}
NativeFormatImporter:
  externalObjects: {{}}
  mainObjectFileID: 7400000
  userData:
  assetBundleName:
  assetBundleVariant:
"""

def controller_content(class_name, class_title, anim_guids):
    # anim_guids: dict {dir: guid}
    # Use fixed negative fileIDs matching Elementalist structure pattern
    # We generate stable negative int IDs from the class name for uniqueness
    import hashlib
    def stable_neg_id(seed):
        h = int(hashlib.md5(seed.encode()).hexdigest(), 16)
        # Make it a large negative int64-range number
        val = (h % (2**62)) + 1
        return -val

    id_south = stable_neg_id(f"{class_name}_south_state")
    id_north = stable_neg_id(f"{class_name}_north_state")
    id_east  = stable_neg_id(f"{class_name}_east_state")
    id_west  = stable_neg_id(f"{class_name}_west_state")
    id_sm    = stable_neg_id(f"{class_name}_statemachine")

    return f"""%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1102 &{id_south}
AnimatorState:
  serializedVersion: 6
  m_ObjectHideFlags: 1
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: idle_south
  m_Speed: 1
  m_CycleOffset: 0
  m_Transitions: []
  m_StateMachineBehaviours: []
  m_Position: {{x: 50, y: 50, z: 0}}
  m_IKOnFeet: 0
  m_WriteDefaultValues: 1
  m_Mirror: 0
  m_SpeedParameterActive: 0
  m_MirrorParameterActive: 0
  m_CycleOffsetParameterActive: 0
  m_TimeParameterActive: 0
  m_Motion: {{fileID: 7400000, guid: {anim_guids['south']}, type: 2}}
  m_Tag:
  m_SpeedParameter:
  m_MirrorParameter:
  m_CycleOffsetParameter:
  m_TimeParameter:
--- !u!1102 &{id_east}
AnimatorState:
  serializedVersion: 6
  m_ObjectHideFlags: 1
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: idle_east
  m_Speed: 1
  m_CycleOffset: 0
  m_Transitions: []
  m_StateMachineBehaviours: []
  m_Position: {{x: 50, y: 50, z: 0}}
  m_IKOnFeet: 0
  m_WriteDefaultValues: 1
  m_Mirror: 0
  m_SpeedParameterActive: 0
  m_MirrorParameterActive: 0
  m_CycleOffsetParameterActive: 0
  m_TimeParameterActive: 0
  m_Motion: {{fileID: 7400000, guid: {anim_guids['east']}, type: 2}}
  m_Tag:
  m_SpeedParameter:
  m_MirrorParameter:
  m_CycleOffsetParameter:
  m_TimeParameter:
--- !u!1102 &{id_north}
AnimatorState:
  serializedVersion: 6
  m_ObjectHideFlags: 1
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: idle_north
  m_Speed: 1
  m_CycleOffset: 0
  m_Transitions: []
  m_StateMachineBehaviours: []
  m_Position: {{x: 50, y: 50, z: 0}}
  m_IKOnFeet: 0
  m_WriteDefaultValues: 1
  m_Mirror: 0
  m_SpeedParameterActive: 0
  m_MirrorParameterActive: 0
  m_CycleOffsetParameterActive: 0
  m_TimeParameterActive: 0
  m_Motion: {{fileID: 7400000, guid: {anim_guids['north']}, type: 2}}
  m_Tag:
  m_SpeedParameter:
  m_MirrorParameter:
  m_CycleOffsetParameter:
  m_TimeParameter:
--- !u!1107 &{id_sm}
AnimatorStateMachine:
  serializedVersion: 6
  m_ObjectHideFlags: 1
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: Base Layer
  m_ChildStates:
  - serializedVersion: 1
    m_State: {{fileID: {id_south}}}
    m_Position: {{x: 200, y: 0, z: 0}}
  - serializedVersion: 1
    m_State: {{fileID: {id_north}}}
    m_Position: {{x: 235, y: 65, z: 0}}
  - serializedVersion: 1
    m_State: {{fileID: {id_east}}}
    m_Position: {{x: 270, y: 130, z: 0}}
  - serializedVersion: 1
    m_State: {{fileID: {id_west}}}
    m_Position: {{x: 305, y: 195, z: 0}}
  m_ChildStateMachines: []
  m_AnyStateTransitions: []
  m_EntryTransitions: []
  m_StateMachineTransitions: {{}}
  m_StateMachineBehaviours: []
  m_AnyStatePosition: {{x: 50, y: 20, z: 0}}
  m_EntryPosition: {{x: 50, y: 120, z: 0}}
  m_ExitPosition: {{x: 800, y: 120, z: 0}}
  m_ParentStateMachinePosition: {{x: 800, y: 20, z: 0}}
  m_DefaultState: {{fileID: {id_south}}}
--- !u!91 &9100000
AnimatorController:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: {class_title}
  serializedVersion: 5
  m_AnimatorParameters: []
  m_AnimatorLayers:
  - serializedVersion: 5
    m_Name: Base Layer
    m_StateMachine: {{fileID: {id_sm}}}
    m_Mask: {{fileID: 0}}
    m_Motions: []
    m_Behaviours: []
    m_BlendingMode: 0
    m_SyncedLayerIndex: -1
    m_DefaultWeight: 0
    m_IKPass: 0
    m_SyncedLayerAffectsTiming: 0
    m_Controller: {{fileID: 9100000}}
--- !u!1102 &{id_west}
AnimatorState:
  serializedVersion: 6
  m_ObjectHideFlags: 1
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: idle_west
  m_Speed: 1
  m_CycleOffset: 0
  m_Transitions: []
  m_StateMachineBehaviours: []
  m_Position: {{x: 50, y: 50, z: 0}}
  m_IKOnFeet: 0
  m_WriteDefaultValues: 1
  m_Mirror: 0
  m_SpeedParameterActive: 0
  m_MirrorParameterActive: 0
  m_CycleOffsetParameterActive: 0
  m_TimeParameterActive: 0
  m_Motion: {{fileID: 7400000, guid: {anim_guids['west']}, type: 2}}
  m_Tag:
  m_SpeedParameter:
  m_MirrorParameter:
  m_CycleOffsetParameter:
  m_TimeParameter:
"""

def controller_meta_content(guid):
    return f"""fileFormatVersion: 2
guid: {guid}
NativeFormatImporter:
  externalObjects: {{}}
  mainObjectFileID: 9100000
  userData:
  assetBundleName:
  assetBundleVariant:
"""

# --- Counters ---
files_copied = 0
meta_files_created = 0
anim_files_written = 0
controllers_created = 0

# --- Summary rows ---
summary_rows = []

# --- Track sprite GUIDs ---
sprite_guids = {}  # sprite_guids[class][dir] = guid
anim_guids_all = {}  # anim_guids_all[class][dir] = guid

for cls in CLASSES:
    sprite_guids[cls] = {}
    anim_guids_all[cls] = {}

# =====================================================================
# STEP 1 + 2: Copy PNGs and create PNG .meta files
# =====================================================================
for cls in CLASSES:
    class_title = cls.capitalize()
    sprite_dest_dir = os.path.join(PROJECT_ROOT, "Assets", "Sprites", "Characters", class_title)
    ensure_dir(sprite_dest_dir)

    for direction in DIRECTIONS:
        src = os.path.join(PROJECT_ROOT, "Characters", "anchors", cls, "rotations", f"{direction}.png")
        dest_name = f"{cls}_idle_{direction}.png"
        dest = os.path.join(sprite_dest_dir, dest_name)

        if not os.path.exists(src):
            print(f"  WARNING: Source not found: {src}")
            continue

        shutil.copy2(src, dest)
        files_copied += 1

        # Generate sprite GUID
        sg = gen_guid()
        sprite_guids[cls][direction] = sg

        # Write PNG .meta
        meta_path = dest + ".meta"
        with open(meta_path, "w", encoding="utf-8", newline="\n") as f:
            f.write(png_meta_content(sg, cls, direction))
        meta_files_created += 1

print(f"\nStep 1+2 done: {files_copied} PNGs copied, {meta_files_created} PNG .meta files created.\n")

# =====================================================================
# STEP 3 + 4: Create/update .anim files and their .meta files
# =====================================================================
for cls in CLASSES:
    class_title = cls.capitalize()
    anim_dir = os.path.join(PROJECT_ROOT, "Assets", "Animations", "Characters", class_title)
    ensure_dir(anim_dir)
    is_elementalist = (cls == "elementalist")

    for direction in DIRECTIONS:
        anim_filename = f"{cls}_idle_{direction}.anim"
        anim_path = os.path.join(anim_dir, anim_filename)
        meta_path = anim_path + ".meta"

        # Determine anim GUID
        if is_elementalist:
            anim_guid = ELEMENTALIST_ANIM_GUIDS[direction]
        else:
            anim_guid = gen_guid()
        anim_guids_all[cls][direction] = anim_guid

        sprite_guid = sprite_guids[cls].get(direction, "MISSING")

        # Write .anim (always — even for elementalist, we update the empty files)
        with open(anim_path, "w", encoding="utf-8", newline="\n") as f:
            f.write(anim_content(cls, direction, sprite_guid))
        anim_files_written += 1

        # Write .anim .meta only if it doesn't exist (preserve elementalist meta GUIDs)
        if not os.path.exists(meta_path):
            with open(meta_path, "w", encoding="utf-8", newline="\n") as f:
                f.write(anim_meta_content(anim_guid))
            meta_files_created += 1
        # else: elementalist meta already exists with correct GUID — leave it alone

        summary_rows.append({
            "class": cls,
            "dir": direction,
            "sprite_guid": sprite_guid,
            "anim_guid": anim_guid,
            "status": "UPDATED" if is_elementalist else "CREATED",
        })

print(f"Step 3+4 done: {anim_files_written} .anim files written.\n")

# =====================================================================
# STEP 5: .controller files for Ranger, Shadowblade, Warblade
# =====================================================================
for cls in CLASSES:
    if cls == "elementalist":
        continue  # skip — already exists

    class_title = cls.capitalize()
    anim_dir = os.path.join(PROJECT_ROOT, "Assets", "Animations", "Characters", class_title)
    ctrl_path = os.path.join(anim_dir, f"{class_title}.controller")
    ctrl_meta_path = ctrl_path + ".meta"

    ctrl_guid = gen_guid()

    with open(ctrl_path, "w", encoding="utf-8", newline="\n") as f:
        f.write(controller_content(cls, class_title, anim_guids_all[cls]))
    controllers_created += 1

    with open(ctrl_meta_path, "w", encoding="utf-8", newline="\n") as f:
        f.write(controller_meta_content(ctrl_guid))
    meta_files_created += 1

    print(f"  Controller created: {ctrl_path}  (guid: {ctrl_guid})")

print(f"\nStep 5 done: {controllers_created} controllers created.\n")

# =====================================================================
# SUMMARY TABLE
# =====================================================================
print("\n" + "="*90)
print(f"{'CLASS':<15} {'DIR':<8} {'SPRITE_GUID':<36} {'ANIM_GUID':<36} {'STATUS'}")
print("="*90)
for row in summary_rows:
    print(f"{row['class']:<15} {row['dir']:<8} {row['sprite_guid']:<36} {row['anim_guid']:<36} {row['status']}")

print("="*90)
print(f"\nFINAL COUNT:")
print(f"  PNGs copied:          {files_copied}")
print(f"  .meta files created:  {meta_files_created}")
print(f"  .anim files written:  {anim_files_written}")
print(f"  controllers created:  {controllers_created}")
