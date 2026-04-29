# PixelLab Aseprite Extension - Otomatik Sistem Haritası (V4 - Eksiksiz)

Bu belge, SADECE "generate" yazan araçları değil; reduce colors (quantize), arka plan silme, unzoom yapma, dialog ayarları ve extension a ait **VAR OLAN TÜM MODÜLLERİ** tarayarak eksiksiz olarak listeler.

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** dialog-settings-advanced.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_selected_reference_image | Text/Kısayol: 'Set', Checked |
| button | clear_selected_reference_image | Text/Kısayol: 'Clear', Checked |
| button | estimate_skeleton | Text/Kısayol: 'Estimate skeleton' |
| button | edit | Text/Kısayol: 'Stop edit (ctrl+space+e)' |
| button | new_frame | Text/Kısayol: 'New frame', Checked |
| combobox | isometric_tile_shape | Label: 'Isometric tile shape:', Text/Kısayol: 'thick tile', Seçenekler: [model.dialog_json[guidance][isometric_tile_shapes]] |
| button | set_style_image | Label: 'Style image:', Text/Kısayol: 'Set' |
| button | clear_style_image | Text/Kısayol: 'Clear' |
| button | edit | Text/Kısayol: 'Stop edit (ctrl+space+e)' |
| button | get_pose | Text/Kısayol: 'Replace skeleton' |
| check | max_size | Text/Kısayol: 'Custom size (max ', Checked |
| check | use_tiling | Text/Kısayol: 'Force tiling', Checked |
| combobox | tiling_position | Label: 'Tiling position:', Seçenekler: [{   --       northwest] |
| check | vertical_tiling | Text/Kısayol: 'vertical tiling', Checked |
| check | horizontal_tiling | Text/Kısayol: 'horizontal tiling', Checked |
| button | selected_reference_image | Text/Kısayol: 'Set reference image', Checked |
| slider | depth_to_index | Label: 'Depth strength:' |
| button | selected_interpolation_from | Text/Kısayol: 'Set from image', Checked |
| button | selected_interpolation_to | Text/Kısayol: 'Set to image', Checked |
| slider | image_guidance_scale | Label: 'Image guidance weight:' |
| slider | resize_image_strength | Label: 'Image strength:' |
| slider | inspirational_image_strength | Label: 'Init image strength:' |
| check | use_inpainting | Text/Kısayol: 'Use inpainting', Checked |
| check | use_reference_image | Text/Kısayol: 'Use reference image', Checked |
| check | use_init_image | Text/Kısayol: 'Use init image', Checked |
| slider | init_image_strength_top | Label: 'Init image strength:' |
| slider | style_strength | Label: 'Style image strength' |
| slider | style_guidance_scale | Label: 'Style guidance weight:' |
| slider | intermediate_guidance_scale | Label: 'Intermediate guidance weight:' |
| slider | shape_guidance_scale | Label: 'Shape guidance weight:' |
| slider | reference_guidance_scale | Label: 'Reference guidance weight:' |
| slider | pose_guidance_scale | Label: 'Pose guidance weight:' |
| slider | inpainting_guidance_scale | Label: 'Inpainting guidance weight:' |
| entry | outer_description | - |
| entry | transition_description | - |
| entry | inner_description | - |
| combobox | background_removal_task | Seçenekler: [options] |
| entry | text | - |
| entry | description | - |
| entry | ref_usage_ | - |
| check | style_color_palette | Text/Kısayol: 'Copy color palette', Checked |
| check | style_outline | Text/Kısayol: 'Copy outline style', Checked |
| check | style_detail | Text/Kısayol: 'Copy level of detail', Checked |
| check | style_shading | Text/Kısayol: 'Copy shading technique', Checked |
| entry | color_palette | - |
| entry | style_description | - |
| entry | character | Label: 'Description:' |
| entry | negative_description | Label: 'Negative description:' |
| entry | action | Label: 'Action description:' |
| button | enhance_prompt | Text/Kısayol: 'Enhance prompt' |
| button | enhance_prompt | Text/Kısayol: 'Enhance prompt' |
| slider | frame_count | Label: 'Number of frames:' |
| check | view_direction | Text/Kısayol: 'Use view and direction', Checked |
| combobox | character_type | Label: 'Character type:', Seçenekler: [model.dialog_json[character_types]] |
| combobox | animation_type | Label: 'Animation:', Seçenekler: [model.dialog_json[animation_types][model.current_json[character_type]:gsub(-] |
| check | vfx | Text/Kısayol: 'Use VFX', Checked |
| combobox | direction_type | Label: 'Direction type:', Seçenekler: [model.dialog_json[direction_types]] |
| slider | scale | Label: 'Scale %:' |
| combobox | from_view | Label: 'From view:', Seçenekler: [translateList(model.dialog_json[guidance][from_view])] |
| combobox | direction_change | Label: 'Rotation:', Seçenekler: [model.dialog_json[guidance][direction_change]] |
| combobox | direction_angle_dropdown | Label: 'Rotation angle:', Seçenekler: [{ -180] |
| combobox | view_change | Label: 'Tilt:', Seçenekler: [model.dialog_json[guidance][view_change]] |
| slider | tilt_angle_slider | Label: 'Tilt angle:' |
| combobox | view | Label: 'Camera view:', Seçenekler: [translateList(options)] |
| combobox | direction | Label: 'Direction:', Seçenekler: [translateList(options)] |
| combobox | outline | Label: 'Outline/Shading/Details:', Seçenekler: [{] |
| combobox | shading | Seçenekler: [{] |
| combobox | detail | Seçenekler: [{] |
| combobox | modifier | Label: 'Modifier (beta):', Seçenekler: [{ none] |
| combobox | transition_size | Label: 'Transition size:', Seçenekler: [model.dialog_json.transition_size_options(model.dialog_json)], Checked |
| combobox | tileset_type | Label: 'Tileset type (experimental):', Seçenekler: [model.dialog_json.tileset_type_options(model.dialog_json)], Checked |
| combobox | theme | Label: 'Theme:', Seçenekler: [model.dialog_json[theme]] |
| combobox | isometric_tile_size | Label: 'Tile size:', Seçenekler: [{         32x32] |
| combobox | reference_image_size | Label: 'Tile size:', Seçenekler: [{         32x32] |
| slider | border_jitter | Label: 'Border jitter:' |
| slider | map_strength | Label: 'Map strength:' |
| slider | tile_strength | Label: 'Tile strength:' |
| slider | ai_border_freedom | Label: 'AI border freedom:' |
| slider | tileset_adherence | Label: 'Tileset adherence:' |
| slider | tileset_adherence_freedom | Label: 'Tileset adherence freedom:' |
| combobox | category | Label: 'Category:', Seçenekler: [template_info:getAllCategoriesDisplayNames(rotation_requirement)] |
| combobox | template_name | Label: 'Template:', Seçenekler: [template_info:getTemplateDisplayNamesByCategory(model.current_json[category]] |
| combobox | template_animation_id | Label: 'Animation:', Seçenekler: [displayNames] |
| slider | ai_freedom | Label: 'AI freedom:' |
| slider | coverage_percentage | Label: 'Canvas coverage (%):' |
| slider | n_frames | Label: 'Number of frames / Start frame' |
| slider | start_frame_index | - |
| check | isometric | Text/Kısayol: 'Isometric', Checked |
| check | oblique_projection | Text/Kısayol: 'Oblique projection (beta)', Checked |
| slider | size | - |
| combobox | map_zoom | Label: 'Tile size:', Seçenekler: [{         32x32] |
| entry | action | Label: 'Action description:' |
| slider | action_guidance_scale | Label: 'Action guidance weight:' |
| slider | text_guidance_scale | Label: 'Guidance weight:' |
| slider | text_guidance_scale | Label: 'Guidance weight:' |
| slider | guidance_scale | Label: 'Guidance weight:' |
| slider | guidance_scale | Label: 'Guidance weight:' |
| slider | zoom | Label: 'Zoom (20 = x2):' |
| slider | fidelity | Label: 'Fidelity:' |
| entry | action | Label: 'Action description:' |
| slider | action_guidance_scale | Label: 'Guidance weight:' |
| check | view_direction | Text/Kısayol: 'Use view and direction', Checked |
| combobox | view | Label: 'View:', Seçenekler: [translateList(options)] |
| combobox | direction | Label: 'Direction:', Seçenekler: [translateList(options)] |
| slider | view_direction_guidance_scale | Label: 'Guidance weight:' |
| slider | view_direction_size_guidance_scale | Label: 'Guidance weight:' |
| combobox | color_image | Label: 'Target palette:', Seçenekler: [options] |
| slider | color_guidance_scale | Label: 'Guidance weight:' |
| check | force_colors | Text/Kısayol: 'Force colors', Checked |
| check | forced_colors | Text/Kısayol: 'Force colors', Checked |
| combobox | init_image | Label: 'Use init image: ', Seçenekler: [{         Yes] |
| combobox | init_images | Label: 'Use init images:', Seçenekler: [{         Yes] |
| slider | init_image_strength | Label: 'Init image strength:' |
| combobox | output_method | Label: 'Output method:', Seçenekler: [{         New frame] |
| check | use_selection | Text/Kısayol: 'Use selection tool to select painting area', Checked |
| check | no_background | Text/Kısayol: 'Blank background', Checked |
| check | no_background | Text/Kısayol: 'Remove background', Checked |
| slider | no_background_guidance_scale | Label: 'Gray background guidance weight:' |
| check | crop_to_mask | Text/Kısayol: 'Crop to mask', Checked |
| check | transparent_background | Text/Kısayol: 'Remove background', Checked |
| slider | pixelart_style_guidance_scale | Label: 'Pixelart style guidance weight:' |
| slider | n_frames | Label: 'Number of frames:' |
| slider | intermediate | Label: 'Intermediate:' |
| check | forced_symmetry | Text/Kısayol: 'Force symmetry', Checked |
| entry | seed | Label: 'seed (0 = random seed):' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** dialog-settings-skeleton.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | generate_dialog | Text/Kısayol: 'Go to Generation' |
| button | documentation | Text/Kısayol: 'Documentation' |
| button | export_skeleton_for_api | Text/Kısayol: 'Export skeleton for API' |
| button | insert_template | Text/Kısayol: 'Insert template' |
| button | rescale | Text/Kısayol: 'Rescale', Checked |
| button | skeleton_setup_reset | Text/Kısayol: 'Reset', Checked |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | selected_reference_image | Text/Kısayol: 'Set reference', Checked |
| check | advanced_options | Label: '               ', Text/Kısayol: 'Advanced options', Checked |
| button | generate | Text/Kısayol: 'Generate' |
| button | reset | Text/Kısayol: 'Reset', Checked |
| button | template_dialog | Text/Kısayol: 'Go to edit skeleton' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | selected_reference_image | Text/Kısayol: 'Set reference', Checked |
| button | estimate_skeleton | Text/Kısayol: 'Estimate skeleton' |
| button | edit | Text/Kısayol: 'Edit skeleton (ctrl+space+e)' |
| slider | id | Label: 'Scroll:' |
| combobox | id | Label: 'Reference direction:', Seçenekler: [{ automatic] |
| combobox | type_template | Label: 'Template types:', Seçenekler: [{ bipedal realistic] |
| combobox | id | Label: 'Animation template:', Seçenekler: [get_animations_names_by_type(model.dialog_json[template_type])] |
| combobox | id | Label: 'Template view/direction:', Seçenekler: [translateList({ high top-down] |
| combobox | id | Seçenekler: [translateList({ north] |
| check | id | Text/Kısayol: '3D controls', Checked |
| slider | id | Label: 'Angle:' |
| slider | id | Label: 'Tilt:' |
| check | id | Label: 'Fixed head (copy reference):', Text/Kısayol: 'Always', Checked |
| check | id | Text/Kısayol: 'Same reference and template direction', Checked |
| slider | id | - |
| slider | id | - |
| slider | id | - |
| slider | id | Label: 'Skeleton head size:' |
| slider | id | Label: 'Offset x/y:         ' |
| slider | id | - |
| slider | id | Label: 'Scroll:' |
| button | set_animation | Text/Kısayol: 'Set animation', Checked |
| button | clear_animation | Text/Kısayol: 'Clear animation', Checked |
| button | show_reference_over_display | Text/Kısayol: 'Show reference' |
| check | id | Label: 'Fixed head:', Text/Kısayol: 'Always (copy head from reference)', Checked |
| slider | id | - |
| slider | id | - |
| slider | id | Label: 'Skeleton head size:' |
| slider | id | Label: 'Offset x/y:         ' |
| slider | id | - |
| check | generation_setup_one_reference | Label: 'Generation setup:', Text/Kısayol: 'Freeze 1 -> Generate 2 frames', Checked |
| check | generation_setup_three_reference | Text/Kısayol: 'Freeze 2 -> Generate 1 frame', Checked |
| check | generation_setup_custom_reference | Text/Kısayol: 'Custom', Checked |
| check | use_inpainting | Text/Kısayol: 'Use inpainting', Checked |
| slider | guidance_scale | Label: 'Guidance weight:' |
| slider | reference_guidance_scale | Label: 'Reference guidance weight:' |
| slider | pose_guidance_scale | Label: 'Pose guidance weight:' |
| combobox | view | Label: 'Camera view:', Seçenekler: [translateList(model.dialog_json[guidance][view])] |
| combobox | direction | Label: 'Direction:', Seçenekler: [translateList(model.dialog_json[guidance][direction])] |
| check | isometric | Text/Kısayol: 'Isometric', Checked |
| check | oblique_projection | Text/Kısayol: 'Oblique projection (beta)', Checked |
| combobox | init_images | Label: 'Use init images:', Seçenekler: [{             Yes] |
| slider | init_image_strength | Label: 'Init image strength:' |
| combobox | color_image | Label: 'Target palette:', Seçenekler: [model.dialog_json[color_image][options]] |
| combobox | output_method | Label: 'Output method:', Seçenekler: [{             New frame] |
| entry | seed | Label: 'seed (0 = random seed):' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** dialog-settings.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_ | Text/Kısayol: 'Set' |
| button | clear_style_image | Text/Kısayol: 'Clear' |
| button | set_ | Text/Kısayol: 'Set' |
| button | clear_style_image | Text/Kısayol: 'Clear' |
| button | set_selected_reference_image | Text/Kısayol: 'Set', Checked |
| button | clear_selected_reference_image | Text/Kısayol: 'Clear', Checked |
| button | estimate_skeleton | Text/Kısayol: 'Estimate skeleton' |
| button | edit | Text/Kısayol: 'Stop edit (ctrl+space+e)' |
| button | new_frame | Text/Kısayol: 'New frame', Checked |
| button | set_style_image | Label: 'Style image:', Text/Kısayol: 'Set' |
| button | clear_style_image | Text/Kısayol: 'Clear' |
| combobox | isometric_tile_shape | Label: 'Isometric tile shape:', Text/Kısayol: 'thick tile', Seçenekler: [model.dialog_json[guidance][isometric_tile_shapes]] |
| button | edit | Text/Kısayol: 'Stop edit (ctrl+space+e)' |
| button | get_pose | Text/Kısayol: 'Replace skeleton' |
| check | max_size | Text/Kısayol: 'Custom size (max ', Checked |
| check | use_inpainting | Text/Kısayol: 'Use inpainting', Checked |
| check | use_reference_image | Text/Kısayol: 'Use reference image', Checked |
| check | use_init_image | Text/Kısayol: 'Use init image', Checked |
| slider | init_image_strength_top | Label: 'Init image strength:' |
| button | selected_reference_image | Text/Kısayol: 'Set reference image', Checked |
| button | selected_interpolation_from | Text/Kısayol: 'Set from image', Checked |
| button | selected_interpolation_to | Text/Kısayol: 'Set to image', Checked |
| slider | pose_guidance_scale | Label: 'Pose guidance weight:' |
| slider | resize_image_strength | Label: 'Image strength:' |
| slider | inspirational_image_strength | Label: 'Init image strength:' |
| slider | style_strength | Label: 'Style image strength' |
| entry | outer_description | - |
| entry | transition_description | - |
| entry | inner_description | - |
| combobox | background_removal_task | Seçenekler: [options] |
| entry | text | - |
| entry | description | - |
| entry | ref_usage_ | - |
| check | style_color_palette | Text/Kısayol: 'Copy color palette', Checked |
| check | style_outline | Text/Kısayol: 'Copy outline style', Checked |
| check | style_detail | Text/Kısayol: 'Copy level of detail', Checked |
| check | style_shading | Text/Kısayol: 'Copy shading technique', Checked |
| entry | color_palette | - |
| entry | character | Label: 'Description:' |
| entry | action | Label: 'Action description: ' |
| button | enhance_prompt | Text/Kısayol: 'Enhance prompt' |
| button | enhance_prompt | Text/Kısayol: 'Enhance prompt' |
| slider | frame_count | - |
| check | view_direction | Text/Kısayol: 'Use view and direction', Checked |
| combobox | character_type | Label: 'Character type:', Seçenekler: [model.dialog_json[character_types]] |
| combobox | animation_type | Label: 'Animation:', Seçenekler: [model.dialog_json[animation_types][model.current_json[character_type]:gsub(-] |
| check | vfx | Text/Kısayol: 'Use VFX', Checked |
| combobox | direction_type | Label: 'Direction type:', Seçenekler: [model.dialog_json[direction_types]] |
| slider | scale | Label: 'Scale %:' |
| combobox | direction_change | Label: 'Rotation:', Seçenekler: [model.dialog_json[guidance][direction_change]] |
| combobox | view_change | Label: 'Tilt:', Seçenekler: [model.dialog_json[guidance][view_change]] |
| combobox | view | Label: 'Camera view:', Seçenekler: [translateList(options)] |
| combobox | direction | Label: 'Direction:', Seçenekler: [translateList(options)] |
| combobox | outline | Label: 'Outline/Shading/Details:', Seçenekler: [{] |
| combobox | shading | Seçenekler: [{] |
| combobox | detail | Seçenekler: [{] |
| combobox | transition_size | Label: 'Transition size:', Seçenekler: [model.dialog_json.transition_size_options(model.dialog_json)], Checked |
| combobox | tileset_type | Label: 'Tileset type (experimental):', Seçenekler: [model.dialog_json.tileset_type_options(model.dialog_json)], Checked |
| combobox | theme | Label: 'Theme:', Seçenekler: [model.dialog_json[theme]] |
| combobox | category | Label: 'Category:', Seçenekler: [template_info:getAllCategoriesDisplayNames(rotation_requirement)] |
| combobox | template_name | Label: 'Template:', Seçenekler: [template_info:getTemplateDisplayNamesByCategory(model.current_json[category]] |
| combobox | template_animation_id | Label: 'Animation:', Seçenekler: [displayNames] |
| combobox | isometric_tile_size | Label: 'Tile size:', Seçenekler: [{         32x32] |
| combobox | reference_image_size | Label: 'Tile size:', Seçenekler: [{         32x32] |
| slider | tileset_adherence | Label: 'Tileset adherence:' |
| slider | tileset_adherence_freedom | Label: 'Tileset adherence freedom:' |
| slider | coverage_percentage | Label: 'Canvas coverage (%):' |
| check | isometric | Text/Kısayol: 'Isometric', Checked |
| check | oblique_projection | Text/Kısayol: 'Oblique projection (beta)', Checked |
| combobox | map_zoom | Label: 'Tile size:', Seçenekler: [{         32x32] |
| slider | size | - |
| slider | zoom | Label: 'Zoom (20 = x2)::' |
| slider | fidelity | Label: 'Fidelity:' |
| entry | action | Label: 'Action description: ' |
| check | view_direction | Text/Kısayol: 'Use view and direction', Checked |
| combobox | view | Label: 'View:', Seçenekler: [translateList(options)] |
| combobox | direction | Label: 'Direction:', Seçenekler: [translateList(options)] |
| combobox | color_image | Label: 'Target palette:', Seçenekler: [options] |
| check | force_colors | Text/Kısayol: 'Force colors', Checked |
| check | forced_colors | Text/Kısayol: 'Force colors', Checked |
| combobox | init_image | Label: 'Use init image:', Seçenekler: [{         Yes] |
| combobox | init_images | Label: 'Use init images:', Seçenekler: [{         Yes] |
| slider | init_image_strength | Label: 'Init image strength:' |
| combobox | output_method | Label: 'Output method:', Seçenekler: [{         New frame] |
| check | use_selection | Text/Kısayol: 'Use selection tool to select painting area', Checked |
| check | no_background | Text/Kısayol: 'Blank background', Checked |
| check | no_background | Text/Kısayol: 'Remove background', Checked |
| check | crop_to_mask | Text/Kısayol: 'Crop to mask', Checked |
| check | transparent_background | Text/Kısayol: 'Remove background', Checked |
| check | forced_symmetry | Text/Kısayol: 'Force symmetry', Checked |
| slider | n_frames | Label: 'Number of frames:' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** export-skeleton-for-api.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| check | selected_frames | Text/Kısayol: 'Export selected frames only', Checked |
| check | all_frames | Text/Kısayol: 'Export all frames', Checked |
| button | ok | Text/Kısayol: 'OK' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Character with 4 rotations**
**📂 Dosya:** generate-4-rotations.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "man with a straw hat and blue overalls" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "side" |
| seed | "0" |
| template_name | "female-humanoid" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| ai_freedom | 0 |
| n_rows | 4 |
| n_columns | 1 |
| category | "realistic" |
| output_method | "New frame" |
| model_name | "generate_4_rotations" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size32x32 | Text/Kısayol: '32x32' |
| button | size48x48 | Text/Kısayol: '48x48' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size80x80 | Text/Kısayol: '80x80' |
| button | size128x128 | Text/Kısayol: '128x128' |
| button | size160x160 | Text/Kısayol: '160x160' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Character with 8 rotations**
**📂 Dosya:** generate-8-rotations.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "man with a straw hat and blue overalls" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "side" |
| seed | "0" |
| template_name | "female-humanoid" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| ai_freedom | 0 |
| n_rows | 1 |
| n_columns | 8 |
| category | "realistic" |
| output_method | "New frame" |
| model_name | "generate_8_rotations" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size32x32 | Text/Kısayol: '32x32' |
| button | size48x48 | Text/Kısayol: '48x48' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size80x80 | Text/Kısayol: '80x80' |
| button | size128x128 | Text/Kısayol: '128x128' |
| button | size160x160 | Text/Kısayol: '160x160' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create animated object/character (pro)**
**📂 Dosya:** generate-animate-character-object.lua
**💰 Maliyet:** This tool costs 20-40 generations per call.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| action | "bouncing and then exploding into pieces" |
| description | "a cute red slime monster" |
| seed | "0" |
| no_background | true |
| output_method | "New frame" |
| model_name | "generate_animate_character_object" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | size96x64 | Text/Kısayol: '96x64 (experimental)' |
| button | size64x96 | Text/Kısayol: '64x96 (experimental)' |
| button | size256 | Text/Kısayol: '256x256 (beta)' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Animate with text (new)**
**📂 Dosya:** generate-animate-with-text-v3.lua
**💰 Maliyet:** This tool costs 1-9 generations depending on size.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_reference_image | "" |
| action | "Walking" |
| no_background | true |
| frame_count | 8 |
| seed | "0" |
| output_method | "New frame" |
| model_name | "generate_animate_with_text_v3" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_reference_image | Text/Kısayol: 'Set reference image' |
| button | clear_reference_image | Text/Kısayol: 'Clear reference image' |

## 🛠 Arayüz Adı: **Animate with text (pro)**
**📂 Dosya:** generate-animate-with-text.lua
**💰 Maliyet:** This tool costs 20-40 generations per call.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_reference_image | "" |
| action | "Walking" |
| view | "none" |
| direction | "none" |
| seed | "0" |
| no_background | true |
| output_method | "New frame" |
| model_name | "generate_animate_with_text" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_reference_image | Text/Kısayol: 'Set reference image' |
| button | clear_reference_image | Text/Kısayol: 'Clear reference image' |

## 🛠 Arayüz Adı: **Animate with skeleton (new)**
**📂 Dosya:** generate-animation-skeleton.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| action | "walk" |
| description | "" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "low top-down" |
| direction | "south" |
| seed | "0" |
| n_images | 8 |
| ai_freedom | 750 |
| augmented_to_index | 400 |
| isometric | false |
| oblique_projection | false |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_animation" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size128x128 | Text/Kısayol: '128x128' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size32x32 | Text/Kısayol: '32x32' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| slider | n_images | Label: 'Number of frames:' |
| button | set_reference_image | Text/Kısayol: 'Set reference image' |
| button | edit_skeleton | Text/Kısayol: 'Insert / Edit skeleton' |

## 🛠 Arayüz Adı: **Animations for character**
**📂 Dosya:** generate-animation-template.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "man with a straw hat and blue overalls" |
| action | "walk" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "side" |
| direction | "south" |
| seed | "0" |
| template_name | "female-humanoid" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| template_animation_id | "walk" |
| ai_freedom | 0 |
| set_reference_image | "" |
| n_rows | 1 |
| n_columns | 8 |
| category | "realistic" |
| output_method | "New frame" |
| model_name | "generate_animation" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size32x32 | Text/Kısayol: '32x32' |
| button | size48x48 | Text/Kısayol: '48x48' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size80x80 | Text/Kısayol: '80x80' |
| button | size128x128 | Text/Kısayol: '128x128' |
| button | size160x160 | Text/Kısayol: '160x160' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | set_reference_image | Text/Kısayol: 'Set reference image' |
| button | clear_reference_image | Text/Kısayol: 'Clear reference image' |

## 🛠 Arayüz Adı: **Animation to animation**
**📂 Dosya:** generate-animation-to-animation.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| action | "walk" |
| description | "man with a straw hat and blue overalls" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "side" |
| direction | "none" |
| seed | "0" |
| n_images | 4 |
| ai_freedom | 750 |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| adjust_animation_to_reference | false |
| use_reference_image | false |
| set_reference_image | "" |
| use_init_image | false |
| init_image_strength | 150 |
| output_method | "New frame" |
| model_name | "generate_animation" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size96x96 | Text/Kısayol: '96x96' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size32x32 | Text/Kısayol: '32x32' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| slider | n_images | Label: 'Number of images:' |
| button | set_reference_image | Text/Kısayol: 'Set reference image' |
| check | adjust_animation_to_reference | Text/Kısayol: 'Adjust animation to reference', Checked |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-canny.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "Dog" |
| text_guidance_scale | 8 |
| view_direction | false |
| view | "side" |
| direction | "south" |
| view_direction_guidance_scale | 4 |
| pixelart_style_guidance_scale | 4 |
| image_size | { width |
| no_background | false |
| no_background_guidance_scale | 4 |
| init_image | "No" |
| init_image_strength | 300 |
| color_image | "No" |
| canny_image | "" |
| canny_guidance_scale | 1 |
| seed | "0" |
| model_name | "generate_canny" |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-character-old.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 6 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "side" |
| direction | "south" |
| style_image | "No" |
| style_image_size | { width |
| style_guidance_scale | 5, --8 |
| inpainting_image | "No" |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| use_selection | false |
| selection_origin | { 0, 0 } |
| max_size | { 64, 64 } |
| output_method | "New frame" |
| model_name | "generate_style" |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-character.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "low top-down" |
| direction | "south" |
| style_image | "No" |
| style_image_size | { width |
| style_strength | 0 |
| inpainting_image | "No" |
| reference_image | "Yes" |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| coverage_percentage | 0.9 |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create animations (automatic)**
**📂 Dosya:** generate-complete-character.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| south_reference_image | "No" |
| north_reference_image | "No" |
| east_reference_image | "No" |
| south_east_reference_image | "No" |
| north_east_reference_image | "No" |
| description | "beautiful woman" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| character_type | "bipedal-semi-chibi" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "high top-down" |
| scale | 1 |
| head_scale | 1 |
| arms_scale | 1 |
| legs_scale | 1 |
| seed | "0" |
| vfx | false |
| vfx_color | { 255, 255, 255 } |
| animation_type | "walk" |
| direction_type | "cardinal" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "low detail" |
| output_method | "New frame" |
| model_name | "generate_one_shot" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Pixel correction**
**📂 Dosya:** generate-correct-pixelart.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_images | {} |
| image_size | { width |
| strength | 10 |
| output_method | "New frame" |
| model_name | "generate_correct_pixelart" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_image | Text/Kısayol: 'Set image' |
| button | clear_images | Text/Kısayol: 'Clear images' |
| slider | strength | Label: 'Strength:' |

## 🛠 Arayüz Adı: **Create UI elements**
**📂 Dosya:** generate-create-ui.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "Gothic health bar and loading bar featuring dark metal frames" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 150 |
| image_size | { width |
| no_background | true |
| seed | "0" |
| map_tile | false |
| ui_set | true |
| output_method | "New frame" |
| model_name | "generate_pixelart_flux_ui" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Edit animation (pro)**
**📂 Dosya:** generate-edit-animation-pro.lua
**💰 Maliyet:** This tool costs 20-40 generations and edits multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_edit_images | {},     -- Images to edit (2-16 frames) |
| description | "add a sword",  -- Text instruction |
| seed | "0" |
| no_background | true |
| output_method | "New frame" |
| output_format | "frames" |
| model_name | "edit_animation_pro" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | add_image | Text/Kısayol: 'Add image' |
| button | clear_images | Text/Kısayol: 'Clear all' |

## 🛠 Arayüz Adı: **Edit image (pro)**
**📂 Dosya:** generate-edit-image-pro.lua
**💰 Maliyet:** This tool costs 20-40 generations and edits a single image.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_reference_image | "", -- For reference method |
| display_edit_image | "",      -- Single image to edit |
| description | "add a sword",  -- For text method |
| method | "text",              -- "text" or "reference" |
| seed | "0" |
| no_background | true |
| use_selection | false,        -- Whether selection is being used |
| selection_origin | { 0, 0 },  -- Origin of selection on canvas |
| image_size | { width |
| output_method | "New frame" |
| output_format | "frames" |
| model_name | "edit_image_pro" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| combobox | method | Label: 'Method:', Seçenekler: [{ Edit with text] |
| button | set_reference_image | Text/Kısayol: 'Set reference' |
| button | set_edit_image | Text/Kısayol: 'Set image' |
| button | clear_images | Text/Kısayol: 'Clear all' |
| button | set_edit_image | Text/Kısayol: 'Set image' |
| button | clear_image | Text/Kısayol: 'Clear' |

## 🛠 Arayüz Adı: **Edit image**
**📂 Dosya:** generate-edit.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| image | "" |
| description | "change facial expression to smiling" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| no_background | true |
| seed | "0" |
| use_selection | false,       -- Whether selection is being used |
| selection_origin | { 0, 0 }, -- Origin of selection on canvas |
| output_method | "New frame" |
| model_name | "generate_edit" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create character with same style**
**📂 Dosya:** generate-flux-same-style.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| style_image | "" |
| description | "full-body view elven warrior" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "none" |
| direction | "none" |
| no_background | true |
| seed | "0" |
| isometric | false |
| oblique_projection | false |
| outline | "" |
| shading | "" |
| detail | "" |
| output_method | "New frame" |
| model_name | "generate_flux_same_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create M-XL image**
**📂 Dosya:** generate-flux.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "full-body view elven warrior" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 150 |
| image_size | { width |
| view | "side" |
| direction | "south" |
| no_background | true |
| seed | "0" |
| map_tile | false |
| isometric | false |
| oblique_projection | false |
| outline | "single color black outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| modifier | "none" |
| output_method | "New frame" |
| model_name | "generate_pixelart_flux" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-general-images-xl.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "Castle on a hill" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| guidance_scale | 8 |
| view_direction | false |
| view | "none" |
| direction | "none" |
| image_size | { width |
| no_background | false |
| init_image | "No" |
| init_image_strength | 300 |
| color_image | "No" |
| seed | "0" |
| model_name | "generate_general_xl" |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-general-images.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "Castle on a hill" |
| text_guidance_scale | 8 |
| view_direction | false |
| view | "high top-down" |
| direction | "none" |
| view_direction_guidance_scale | 8 |
| pixelart_style_guidance_scale | 4 |
| image_size | { width |
| no_background | false |
| no_background_guidance_scale | 4 |
| init_image | "No" |
| init_image_strength | 300 |
| color_image | "No" |
| seed | "0" |
| model_name | "generate_general" |


## 🛠 Arayüz Adı: **Create S-XL image (pro)**
**📂 Dosya:** generate-image-new.lua
**💰 Maliyet:** This tool costs 20-40 generations and creates multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "cute wizard" |
| seed | 0,                      -- 0 |
| no_background | false |
| reference_images_display | {}, -- Array of {image, usage_description} |
| style_image_display | "",      -- Single style image |
| style_options | { |
| color_palette | true |
| outline | true |
| detail | true |
| shading | true |
| model_name | "generate_image_new" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_style_image | Text/Kısayol: 'Set style' |
| button | clear_style_image | Text/Kısayol: 'Clear style' |
| button | add_reference_image | Text/Kısayol: 'Add reference' |
| button | clear_reference_images | Text/Kısayol: 'Clear reference' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size344x192 | Text/Kısayol: '344x192 (experimental)' |
| button | size344 | Text/Kısayol: '344x344 (beta)' |
| button | size512 | Text/Kısayol: '512x512 (beta)' |
| button | size512x288 | Text/Kısayol: '512x288 (beta)' |
| button | size688x384 | Text/Kısayol: '688x384 (beta)' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Image to pixel art**
**📂 Dosya:** generate-image-to-pixelart.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| image | "" |
| text_guidance_scale | 8.0 |
| width | 64 |
| height | 64 |
| image_size | { width |
| no_background | false |
| seed | "0" |
| model_name | "generate_image_to_pixelart" |
| output_method | "New frame" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | optimal | Label: 'Recommended (small)' |
| button | optimal_large | Label: 'Recommended (large)' |
| button | useCurrent | Text/Kısayol: 'Use current' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Extend map (v2)**
**📂 Dosya:** generate-inpainting-map-v2.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "oak tree and dirt path" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| use_selection | false |
| selection_origin | { 0, 0 } |
| view | "side" |
| seed | "0" |
| map_tile | true |
| map_zoom | "16x16" |
| isometric | false |
| inpainting_to_index | 0 |
| inpainting_image | "" |
| mask_image | "No" |
| outline | "" |
| shading | "" |
| detail | "" |
| output_method | "New frame" |
| model_name | "generate_inpainting_map_v2" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size160 | Text/Kısayol: '160x160' |
| button | size128 | Text/Kısayol: '128x128' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Extend map**
**📂 Dosya:** generate-inpainting-map.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "oak tree and dirt path" |
| negative_description | "" |
| reference_image | "Yes" |
| view | "high top-down" |
| direction | "none" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| init_image | "No" |
| init_image_strength | 300 |
| inpainting_image | "" |
| seed | "0" |
| model_name | "generate_inpainting_map" |
| use_selection | false |
| map_tile | true |
| map_zoom | "16x16" |
| selection_origin | { 0, 0 } |
| outline | "lineless" |
| shading | "medium shading" |
| detail | "medium detail" |
| isometric | false |
| force_colors | false |
| output_method | "New layer" |


## 🛠 Arayüz Adı: **Inpaint M-L (pixpatch v2)**
**📂 Dosya:** generate-inpainting-pixpatch-v2.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "woman holding a sword" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| use_selection | false |
| selection_origin | { 0, 0 } |
| view | "side" |
| direction | "south" |
| no_background | true |
| seed | "0" |
| map_tile | false |
| isometric | false |
| oblique_projection | false |
| inpainting_to_index | 0 |
| inpainting_image | "" |
| mask_image | "No" |
| outline | "" |
| shading | "" |
| detail | "" |
| output_method | "New frame" |
| model_name | "generate_inpainting_pixpatch_v2" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size160 | Text/Kısayol: '160x160' |
| button | size128 | Text/Kısayol: '128x128' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Inpaint (v3)**
**📂 Dosya:** generate-inpainting-v3.lua
**💰 Maliyet:** This tool costs 20-40 generations and creates multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "replace with grass" |
| image_size | { width |
| seed | "0" |
| no_background | false |
| crop_to_mask | false |
| display_inpainting_image | "", -- Single image to inpaint |
| use_selection | false,         -- Whether selection is being used |
| selection_origin | { 0, 0 },   -- Origin of selection on canvas |
| output_method | "New layer" |
| output_format | "frames" |
| model_name | "generate_inpainting_v3" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size32x32 | Text/Kısayol: '32x32' |
| button | size64x64 | Text/Kısayol: '64x64' |
| button | size128x128 | Text/Kısayol: '128x128' |
| button | size256x256 | Text/Kısayol: '256x256' |
| button | size512x512 | Text/Kısayol: '512x512 (beta)' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Inpaint**
**📂 Dosya:** generate-inpainting.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "Woman holding a sword" |
| negative_description | "amateur. ugly. artifacts" |
| view_direction | false |
| reference_image | "Yes" |
| view | "none" |
| direction | "none" |
| text_guidance_scale | 8 |
| color_image | "Current image" |
| image_size | { width |
| init_image | "No" |
| init_image_strength | 500 |
| inpainting_image | "" |
| seed | "0" |
| model_name | "generate_inpainting" |
| use_selection | false |
| selection_origin | { 0, 0 } |
| outline | "" |
| shading | "" |
| detail | "" |
| transparent_background | true |
| isometric | false |
| oblique_projection | false |
| force_colors | false |
| output_method | "New layer" |


## 🛠 Arayüz Adı: **Create walking character**
**📂 Dosya:** generate-instant-character.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "man with a straw hat and blue overalls" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| view | "side" |
| seed | "0" |
| template_name | "female-humanoid" |
| category | "realistic" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| ai_freedom | 0 |
| n_rows | 4 |
| n_columns | 4 |
| output_method | "New frame" |
| model_name | "generate_spritesheet" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size32x32 | Text/Kısayol: '32x32' |
| button | size32x40 | Text/Kısayol: '32x40' |
| button | size32x48 | Text/Kısayol: '32x48' |
| button | size48x48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Interpolate (pro)**
**📂 Dosya:** generate-interpolation-pro.lua
**💰 Maliyet:** This tool costs 20-40 generations.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_start_image | "" |
| display_end_image | "" |
| action | "transforming" |
| seed | "0" |
| no_background | true |
| output_method | "New frame" |
| model_name | "interpolation" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_start_image | Text/Kısayol: 'Set start image' |
| button | clear_start_image | Text/Kısayol: 'Clear start' |
| button | set_end_image | Text/Kısayol: 'Set end image' |
| button | clear_end_image | Text/Kısayol: 'Clear end' |

## 🛠 Arayüz Adı: **Interpolate (new)**
**📂 Dosya:** generate-interpolation-v3.lua
**💰 Maliyet:** This tool costs 1-9 generations depending on size.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_start_image | "" |
| display_end_image | "" |
| action | "transforming" |
| no_background | true |
| frame_count | 8 |
| seed | "0" |
| output_method | "New frame" |
| model_name | "generate_interpolation_v3" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_start_image | Text/Kısayol: 'Set start image' |
| button | clear_start_image | Text/Kısayol: 'Clear start' |
| button | set_end_image | Text/Kısayol: 'Set end image' |
| button | clear_end_image | Text/Kısayol: 'Clear end' |

## 🛠 Arayüz Adı: **Interpolate**
**📂 Dosya:** generate-interpolation.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| character | "cute dragon" |
| action | "walk" |
| negative_description | "" |
| text_guidance_scale | 8 |
| direction | "east" |
| view | "low top-down" |
| interpolation_from | "" |
| interpolation_to | "" |
| image_guidance_scale | 1 |
| selected_reference_image | "" |
| color_image | "Current image" |
| seed | "0" |
| model_name | "generate_interpolation" |
| output_method | "New frame" |
| use_selection | false |
| selection_origin | { 0, 0 } |
| image_size | { width |
| max_size | { 64, 64 } |


## 🛠 Arayüz Adı: **Create isometric tile**
**📂 Dosya:** generate-isometric-tile.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "grass on top of dirt" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| seed | "0" |
| map_tile | false |
| isometric_tile | true |
| isometric_tile_size | "32x32" |
| isometric_tile_shape | "thick tile" |
| set_reference_shape_image | "" |
| outline | "" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_isometric_tile" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | set_reference_shape_image | Text/Kısayol: 'Set reference shape' |
| button | clear_reference_shape_image | Text/Kısayol: 'Clear reference shape' |

## 🛠 Arayüz Adı: **Create map (pixflux)**
**📂 Dosya:** generate-map-flux.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "grass platform with a mountain in the background" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "side" |
| seed | "0" |
| map_tile | true |
| map_zoom | "16x16" |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_map_flux" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Animate with text**
**📂 Dosya:** generate-movement.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| character | "cute dragon" |
| action | "walk" |
| negative_description | "" |
| text_guidance_scale | 8 |
| direction | "east" |
| view | "low top-down" |
| movement_images | {} |
| image_guidance_scale | 1.4 |
| init_images | { "No" } |
| init_image_strength | 300 |
| inpainting_images | { "No" } |
| n_frames | 4 |
| start_frame_index | 0 |
| selected_reference_image | "" |
| color_image | "Current image" |
| seed | "0" |
| model_name | "generate_movement" |
| output_method | "New layer" |
| use_selection | false |
| selection_origin | { 0, 0 } |
| image_size | { width |
| max_size | { 64, 64 } |


## 🛠 Arayüz Adı: **Multi-image edit**
**📂 Dosya:** generate-multi-edit.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| images | {} |
| description | "change facial expression to smiling" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| no_background | true |
| seed | "0" |
| output_method | "New frame" |
| model_name | "generate_multi_edit" |
| n_images | 4 |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| slider | n_images | Label: 'Number of frames:' |
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create S-M image (old)**
**📂 Dosya:** generate-no-style-old.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8, -- 6 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "side" |
| direction | "south" |
| no_background | true |
| inpainting_image | "No" |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| use_selection | false |
| selection_origin | { 0, 0 } |
| max_size | { 64, 64 } |
| output_method | "New frame" |
| model_name | "generate_style_old" |


## 🛠 Arayüz Adı: **Create S-M image**
**📂 Dosya:** generate-no-style.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| inpainting_image | "No" |
| reference_image | "Yes" |
| view | "low top-down" |
| direction | "south" |
| no_background | true |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| coverage_percentage | 0.9 |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Image to image (depth)**
**📂 Dosya:** generate-pixflux-depth.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "full-body view elven warrior" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "none" |
| direction | "none" |
| no_background | true |
| seed | "0" |
| map_tile | false |
| isometric | false |
| oblique_projection | false |
| outline | "single color black outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| depth_image | "No" |
| depth_to_index | 0 |
| output_method | "New frame" |
| model_name | "generate_pixflux_depth" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size180 | Text/Kısayol: '180x180' |
| button | size160 | Text/Kısayol: '160x160' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Animate with skeleton**
**📂 Dosya:** generate-pose-animation.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| points | {} |
| pose_images | {} |
| pose_guidance_scale | 3 |
| selected_reference_image | "" |
| reference_guidance_scale | 1.0 |
| guidance_scale | 3 |
| inpainting_images | { "No" } |
| isometric | false |
| oblique_projection | false |
| view | "low top-down" |
| direction | "south" |
| reference_direction | "automatic" |
| color_image | "Reference image" |
| init_images | { "No" } |
| image_size | { width |
| init_image_strength | 300 |
| size_head_pose | 10 |
| size_height_pose | 10 |
| size_width_pose | 10 |
| size_depth_pose | 10 |
| scroll_preview | 0 |
| x_pose | 0 |
| y_pose | 0 |
| fixed_head | "same_direction" |
| model_name | "generate_pose_animation" |
| output_method | "New layer" |
| seed | "0" |
| use_inpainting | false |
| show_skeleton_preview | false |
| ["3d_controls"] | false |
| tilt_angle | 25 |
| direction_angle | 270 |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-pose.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| text_guidance_scale | 8 |
| view_direction | false |
| view | "low top-down" |
| direction | "none" |
| view_direction_guidance_scale | 4 |
| pixelart_style_guidance_scale | 4 |
| image_size | { width |
| no_background | false |
| no_background_guidance_scale | 4 |
| init_image | "No" |
| init_image_strength | 300 |
| seed | "0" |
| pose_image | "" |
| pose_image_size | { width |
| pose_guidance_scale | 1 |
| color_image | "No" |
| model_name | "generate_pose" |


## 🛠 Arayüz Adı: **Re-pose (skeleton)**
**📂 Dosya:** generate-re-pose.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| points | {} |
| selected_reference_frame | { |
| sprite | nil |
| frame | nil |
| guidance_scale | 3 |
| use_inpainting | false |
| isometric | false |
| oblique_projection | false |
| view | "low top-down" |
| direction | "south" |
| color_image | "Reference image" |
| use_init_image | false |
| image_size | { width |
| init_image_strength | 300 |
| model_name | "generate_re_pose_animation" |
| output_method | "Modify current layer" |
| seed | "0" |


## 🛠 Arayüz Adı: **Rotate to 8 directions (pro)**
**📂 Dosya:** generate-reference-to-8-rotations.lua
**💰 Maliyet:** This tool costs 20-40 generations and creates multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_concept_image | "",                        -- Optional: High-res image defining subject identity (max 1024x1024) |
| display_reference_image | "",                      -- Optional: Pixel art style reference (max 84x84, or 168x168 for tier 2+) |
| description | "cute wizard wearing a purple robe", -- Optional: Text description |
| style_description | "",                            -- Optional: Style description |
| method | "create_with_style",                      -- "create_with_style", "create_from_concept", "rotate_character" |
| view | "low top-down" |
| seed | "0" |
| no_background | true |
| image_size | { width |
| output_method | "New frame" |
| model_name | "reference_to_8_rotations" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| combobox | method | Label: 'Method:', Seçenekler: [{ Create with style] |
| button | set_concept_image | Text/Kısayol: 'Set concept' |
| button | clear_concept_image | Text/Kısayol: 'Clear concept' |
| button | set_reference_image | Text/Kısayol: 'Set reference' |
| button | clear_reference_image | Text/Kısayol: 'Clear reference' |
| button | set_reference_image | Text/Kısayol: 'Set reference' |
| button | clear_reference_image | Text/Kısayol: 'Clear reference' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size48 | Text/Kısayol: '48x48' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size84 | Text/Kısayol: '84x84' |
| button | size48x64 | Text/Kısayol: '48x64 (experimental)' |
| button | size96 | Text/Kısayol: '96x96' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size168 | Text/Kısayol: '168x168 (beta)' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Remove background**
**📂 Dosya:** generate-remove-background.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| image | "" |
| text | "" |
| background_removal_task | "Simple" |
| image_size | { width |
| seed | "0" |
| output_method | "New frame" |
| model_name | "generate_remove_background" |


## 🛠 Arayüz Adı: **Reshape**
**📂 Dosya:** generate-reshape.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| character | "cute dragon" |
| reference_image | "" |
| image_guidance_scale | 2 |
| shape_image | "" |
| shape_guidance_scale | 1.5 |
| view | "low top-down" |
| text_guidance_scale | 4 |
| color_image | "Current image" |
| init_images_amount | 1 |
| init_images | { "No" } |
| init_image_strength | 300 |
| seed | "0" |
| model_name | "generate_reshape" |


## 🛠 Arayüz Adı: **Resize**
**📂 Dosya:** generate-resize.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "human mage" |
| width | 64 |
| height | 64 |
| color_image | "No" |
| selected_reference_image | "" |
| image_size | { width |
| view | "none" |
| direction | "none" |
| isometric | false |
| oblique_projection | false |
| init_image | "No" |
| init_image_strength | 300 |
| no_background | false |
| seed | "0" |
| model_name | "generate_resize" |
| output_method | "New frame" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size192 | Text/Kısayol: '192x192' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | size192 | Text/Kısayol: '192x192' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Rotate**
**📂 Dosya:** generate-rotate-single.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| view_change | "0" |
| direction_change | "45" |
| image_guidance_scale | 3 |
| from_image | "" |
| image_size | { width |
| init_image | "No" |
| init_image_strength | 300 |
| inpainting_image | "No" |
| use_inpainting | false |
| color_image | "Reference image" |
| seed | "0" |
| model_name | "generate_rotate_single" |
| output_method | "New frame" |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-rotations.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| negative_description | "" |
| inpainting_images | { "No" } |
| rotation_images | {} |
| always_visible_display_images_name | { "South", "East", "North", "West" } |
| image_guidance_scale | 2 |
| init_images | { "No" } |
| init_image_strength | 300 |
| view | "low top-down" |
| text_guidance_scale | 8 |
| color_image | "Current image" |
| forced_symmetry | false |
| seed | "0" |
| model_name | "generate_rotations" |
| output_method | "Modify current layer" |


## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-start.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| negative_description | "" |
| size | 20 |
| text_guidance_scale | 8 |
| color_image | "No" |
| color_guidance_scale | 4 |
| forced_colors | false |
| forced_symmetry | false |
| init_image | "No" |
| init_image_strength | 300 |
| view | "low top-down" |
| direction | "south" |
| seed | "0" |
| model_name | "generate_start" |


## 🛠 Arayüz Adı: **Create image from style reference (pro)**
**📂 Dosya:** generate-style-new.lua
**💰 Maliyet:** This tool costs 20-40 generations and creates multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| style_images | {}, -- Will store multiple Image objects |
| style_description | "" |
| description | "" |
| seed | "0" |
| no_background | true |
| output_method | "Modify current layer" |
| output_format | "frames" |
| model_name | "generate_consistent_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | add_style_image | Text/Kısayol: 'Add style image' |
| button | clear_style_images | Text/Kısayol: 'Clear all images' |
| button | size16 | Text/Kısayol: '16x16 (256 frames)' |
| button | size32 | Text/Kısayol: '32x32 (64 frames)' |
| button | size64 | Text/Kısayol: '64x64 (16 frames)' |
| button | size128 | Text/Kısayol: '128x128 (4 frames)' |
| button | size256 | Text/Kısayol: '256x256 (1 frame)' |
| button | size512 | Text/Kısayol: '512x512 (1 frame, beta)' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create S-M image (style**
**📂 Dosya:** generate-style-old.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8, -- 6 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "side" |
| direction | "south" |
| no_background | true |
| style_image | "No" |
| style_image_size | { width |
| style_guidance_scale | 5, --8 |
| inpainting_image | "No" |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| use_selection | false |
| selection_origin | { 0, 0 } |
| max_size | { 64, 64 } |
| output_method | "New frame" |
| model_name | "generate_style_old" |


## 🛠 Arayüz Adı: **Pose to image**
**📂 Dosya:** generate-style-pose.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| inpainting_image | "No" |
| reference_image | "Yes" |
| view | "low top-down" |
| direction | "south" |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| pose_keyframe | true |
| pose_guidance_scale | 1.0 |
| coverage_percentage | 0.9 |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "low detail" |
| output_method | "New frame" |
| model_name | "generate_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create S-M image (style)**
**📂 Dosya:** generate-style.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "beautiful amazon warrior" |
| negative_description | "mixels. amateur. multiple. grainy background" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| inpainting_image | "No" |
| reference_image | "Yes" |
| view | "low top-down" |
| direction | "south" |
| no_background | true |
| style_image | "No" |
| style_image_size | { width |
| style_strength | 0 |
| seed | "0" |
| use_inpainting | false |
| map_tile | false |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| coverage_percentage | 0.9 |
| outline | "selective outline" |
| shading | "basic shading" |
| detail | "medium detail" |
| output_method | "New frame" |
| model_name | "generate_style" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | size64 | Text/Kısayol: '64x64' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size16 | Text/Kısayol: '16x16' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create texture**
**📂 Dosya:** generate-texture.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "ocean" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 150.0 |
| image_size | { width |
| seed | "0" |
| texture_strength | 1.0 |
| shading | "basic shading" |
| detail | "low detail" |
| output_method | "New frame" |
| model_name | "generate_texture" |


## 🛠 Arayüz Adı: **Create tiles (pro)**
**📂 Dosya:** generate-tiles-pro.lua
**💰 Maliyet:** This tool costs 20-40 generations and creates multiple frames.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "1). grass tile 2). dirt tile 3). stone tile 4). ocean tile" |
| method | "description",    -- "description" or "style" |
| tile_type | "isometric" |
| tile_size | "32",          -- string for combobox: "16","16x32","32","32x64","48","48x96","64","64x128","96","128" |
| tile_view_angle | 20,      -- slider 0-90 degrees |
| thickness | 30,            -- slider 0-100, becomes tile_depth_ratio |
| seed | 0 |
| style_images_display | {}, -- display-only (Image objects, not serialized) |
| style_options | { |
| color_palette | true |
| outline | true |
| detail | true |
| shading | true |
| model_name | "generate_tiles_pro" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| combobox | method | Label: 'Method:', Seçenekler: [{ Create from description] |
| button | add_style_image | Text/Kısayol: 'Add style tile (' |
| button | clear_style_images | Text/Kısayol: 'Clear' |
| check | style_color_palette | Text/Kısayol: 'Copy color palette', Checked |
| check | style_outline | Text/Kısayol: 'Copy outline style', Checked |
| check | style_detail | Text/Kısayol: 'Copy level of detail', Checked |
| check | style_shading | Text/Kısayol: 'Copy shading technique', Checked |
| combobox | tile_type | Label: 'Tile shape:', Seçenekler: [tile_type_labels] |
| combobox | tile_size | Label: 'Tile size:', Seçenekler: [{ 16] |
| slider | tile_view_angle | Label: 'View angle:' |
| slider | thickness | Label: 'Thickness:' |
| button | size32 | Text/Kısayol: '32x32' |
| button | size48 | Text/Kısayol: '48x48' |
| button | size64 | Text/Kısayol: '64x64' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** generate-tiles-style.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "forest" |
| negative_description | "mixels. amateur" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| image_size | { width |
| view | "high top-down" |
| style_image | "No" |
| style_image_size | { width |
| style_guidance_scale | 1 |
| inpainting_image | "" |
| seed | "0" |
| map_tile | true |
| force_colors | false |
| isometric | false |
| oblique_projection | false |
| map_zoom | "16x16" |
| use_selection | false |
| selection_origin | { 0, 0 } |
| max_size | { 64, 64 } |
| output_method | "New layer with changes" |
| model_name | "generate_tiles_style" |


## 🛠 Arayüz Adı: **Extend map (old)**
**📂 Dosya:** generate-tiles.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "grass" |
| negative_description | "" |
| reference_image | "Yes" |
| view | "high top-down" |
| text_guidance_scale | 8 |
| color_image | "No" |
| init_image | "No" |
| init_image_strength | 300 |
| inpainting_image | "" |
| seed | "0" |
| model_name | "generate_tiles" |
| use_selection | false |
| selection_origin | { 0, 0 } |
| max_size | { 64, 64 } |
| image_size | { width |
| output_method | "Modify current layer" |


## 🛠 Arayüz Adı: **Create tileset (sidescroller)**
**📂 Dosya:** generate-tileset-sidescroller.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| inner_reference_image | "No" |
| transition_reference_image | "No" |
| inner_description | "rocks" |
| transition_description | "grass" |
| text_guidance_scale | 8 |
| transition_size | 0.5 |
| color_image | "No" |
| reference_image_size | { width |
| image_size | { width |
| seed | "0" |
| tileset_adherence | 100 |
| tileset_adherence_freedom | 500 |
| outline | "" |
| shading | "basic shading" |
| detail | "low detail" |
| output_method | "New frame" |
| model_name | "generate_tileset_sidescroller" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| check | open_in_new_sprite | Text/Kısayol: 'Open as sprite', Checked |
| combobox | target_layout | Label: 'Target Layout:', Seçenekler: [{             Tileset Wang] |
| check | export_for_sprite_fusion | Text/Kısayol: 'Export for Sprite Fusion', Checked |
| button | ok | Text/Kısayol: 'OK' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create tileset (high top-down)**
**📂 Dosya:** generate-tileset.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| inner_reference_image | "No" |
| outer_reference_image | "No" |
| transition_reference_image | "No" |
| transition_description | "rocks" |
| inner_description | "ocean" |
| outer_description | "sand" |
| text_guidance_scale | 8 |
| transition_size | 0.5 |
| color_image | "No" |
| reference_image_size | { width |
| image_size | { width |
| seed | "0" |
| tileset_adherence | 100 |
| tileset_adherence_freedom | 500 |
| tileset_type | "higher_lower" |
| outline | "" |
| shading | "basic shading" |
| detail | "low detail" |
| output_method | "New frame" |
| model_name | "generate_tileset" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| check | open_in_new_sprite | Text/Kısayol: 'Open as sprite', Checked |
| combobox | target_layout | Label: 'Target Layout:', Seçenekler: [{             Tileset Wang] |
| check | export_for_sprite_fusion | Text/Kısayol: 'Export for Sprite Fusion', Checked |
| button | ok | Text/Kısayol: 'OK' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Transfer outfit to animation (pro)**
**📂 Dosya:** generate-transfer-outfit-pro.lua
**💰 Maliyet:** This tool costs 20-40 generations and transfers an outfit.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| display_reference_image | "", -- Outfit image to transfer |
| display_edit_images | {},     -- Animation frames to apply outfit to (2-15 frames) |
| seed | "0" |
| no_background | true |
| output_method | "New frame" |
| output_format | "frames" |
| model_name | "transfer_outfit_pro" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_reference_image | Text/Kısayol: 'Set outfit' |
| button | clear_reference_image | Text/Kısayol: 'Clear outfit' |
| button | add_image | Text/Kısayol: 'Add frame' |
| button | clear_images | Text/Kısayol: 'Clear frames' |

## 🛠 Arayüz Adı: **Try-on image**
**📂 Dosya:** generate-try-on.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| subject_image | "" |
| try_on_image | "" |
| description | "golden helmet" |
| text_guidance_scale | 8 |
| color_image | "No" |
| image_size | { width |
| no_background | true |
| seed | "0" |
| output_method | "New frame" |
| model_name | "generate_try_on" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_subject_image | Text/Kısayol: 'Set' |
| button | clear_subject_image | Text/Kısayol: 'Clear' |
| button | set_try_on_image | Text/Kısayol: 'Set' |
| button | clear_try_on_image | Text/Kısayol: 'Clear' |
| button | size320 | Text/Kısayol: '320x320' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size180 | Text/Kısayol: '180x180' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size48 | Text/Kısayol: '48x48' |
| button | sizeCustom | Text/Kısayol: 'Custom' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Create UI elements (pro)**
**📂 Dosya:** generate-ui.lua
**💰 Maliyet:** This tool costs 20-40 generations and outputs a single image.

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| description | "menu, arrow keys and health bar" |
| seed | 0,                   -- 0 |
| no_background | true,       -- Default to transparent background |
| color_palette | "",         -- Optional: e.g., "blue and silver" |
| concept_image_display | "", -- Single concept image for reference |
| output_method | "New layer" |
| model_name | "generate_ui" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | set_concept_image | Text/Kısayol: 'Set concept' |
| button | clear_concept_image | Text/Kısayol: 'Clear concept' |
| button | size64 | Text/Kısayol: '64x64' |
| button | size128x64 | Text/Kısayol: '128x64 (experimental)' |
| button | size128 | Text/Kısayol: '128x128' |
| button | size256 | Text/Kısayol: '256x256' |
| button | size512x256 | Text/Kısayol: '512x256 (beta)' |
| button | size512 | Text/Kısayol: '512x512 (beta)' |
| button | size688x384 | Text/Kısayol: '688x384 (beta)' |
| button | cancel | Text/Kısayol: 'Cancel' |

## 🛠 Arayüz Adı: **Reduce colors**
**📂 Dosya:** quantize-image.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| image | "" |
| image_size | { width |
| palette_image | "" |
| palette_image_size | { width |
| num_colors | 16 |
| dithering | nil |
| dithering_size | "2x2" |
| strength | 50 |
| quantization_method | "Auto" |
| selected_palette | nil |
| output_method | "New frame" |
| model_name | "quantize_image" |
| process_all_frames | false |
| batch_output_method | "New layer" |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| combobox | quantization_method | Label: 'Quantization Method', Seçenekler: [{ Auto] |
| slider | page_slider | - |
| slider | number_of_colors | Label: 'Number of Colors' |
| combobox | dithering | Label: 'Dithering', Seçenekler: [{ No] |
| combobox | dithering_size | Label: 'Dithering size', Seçenekler: [{ 2x2] |
| slider | strength | Label: 'Dithering Strength' |
| check | process_all_frames | Label: 'Process All Frames', Checked |
| combobox | batch_output_method | Label: 'Output Method', Seçenekler: [{ New layer] |
| button | ok | Text/Kısayol: 'Quantize' |
| button | reset | Text/Kısayol: 'Reset' |
| button | close | Text/Kısayol: 'Close' |

## 🛠 Arayüz Adı: **Unzoom pixel art**
**📂 Dosya:** unzoom-pixelart.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Dinamik Parametreler (Varsayılan Config)
| Parametre (Key) | Varsayılan Değer / Açıklama |
|----|--------------------------------------------|
| image | "" |
| image_size | { width |
| quantize | 0 |
| output_method | "New frame" |
| model_name | "unzoom_pixelart" |
| process_all_frames | false |

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| combobox | quantize_mode | Label: 'Color Reduction', Seçenekler: [{ Auto] |
| slider | quantize_slider | Label: 'Number of Colors' |
| check | process_all_frames | Label: 'Process All Frames', Checked |
| button | ok | Text/Kısayol: 'Unzoom' |
| button | reset | Text/Kısayol: 'Reset' |
| button | close | Text/Kısayol: 'Close' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** websocket-multi-layer.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | cancel | Text/Kısayol: 'Cancel' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | discard | Text/Kısayol: 'Discard' |
| button | expand_full | Text/Kısayol: 'Expand to full size' |

## 🛠 Arayüz Adı: **Alt Bileşen veya Sistem Modülü**
**📂 Dosya:** websocket.lua
**💰 Maliyet:** Ücretsiz / Maliyet Belirtilmemiş

### Arayüz Elementleri (Custom Butonlar / Paneller)
| Arac Tipi | ID | Ek Özellikler |
|----|----|--------------------------------------------|
| button | cancel | Text/Kısayol: 'Cancel' |
| button | cancel | Text/Kısayol: 'Cancel' |
| button | discard | Text/Kısayol: 'Discard' |
| button | expand_full | Text/Kısayol: 'Expand to full size' |

