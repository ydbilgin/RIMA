# PixelLab API v2 Reference

Source URLs: https://api.pixellab.ai/v2/docs, https://api.pixellab.ai/v2/openapi.json, https://api.pixellab.ai/v2/llms.txt
Fetched: 2026-05-18

## Overview

- API title: Pixel Lab API
- API version: dev
- Public base URL: `https://api.pixellab.ai/v2`
- Docs page: `https://api.pixellab.ai/v2/docs`
- OpenAPI JSON: `https://api.pixellab.ai/v2/openapi.json`
- LLM reference: `https://api.pixellab.ai/v2/llms.txt`
- Common request headers: `Authorization: Bearer <PIXELLAB_API_TOKEN>` and `Content-Type: application/json` for JSON bodies.
- Standard response envelope includes `success`, `data`, `error`, and `usage` with credit/generation accounting when available.

## Authentication

- Authentication scheme: HTTP Bearer token.
- Send API keys as `Authorization: Bearer YOUR_API_TOKEN`.
- The LLM docs state tokens are managed at `https://pixellab.ai/account`.
- The OpenAPI spec does not expose an unauthenticated endpoint list except documentation endpoints; generation/status/account endpoints should be treated as authenticated.

## Rate limits / pricing

- The spec documents `402 Insufficient credits` on generation endpoints.
- The spec documents `429 Too many concurrent jobs` on many async generation endpoints.
- Response usage metadata can include `credits_used`, `generations_used`, `remaining_credits`, and `remaining_generations`.
- Exact numeric rate limits and per-endpoint prices were not published in the OpenAPI schema.

## Endpoint Inventory

- Paths discovered: 56
- Operations discovered: 60

### Account

| method | path | summary |
| --- | --- | --- |
| GET | `/balance` | Get balance |

### Animate

| method | path | summary |
| --- | --- | --- |
| POST | `/animate-with-skeleton` | Animate with skeleton |
| POST | `/animate-with-text` | Animate with text |
| POST | `/animate-with-text-v2` | Animate with text (pro) |
| POST | `/animate-with-text-v3` | Animate with text v3 |
| POST | `/edit-animation-v2` | Edit animation (Pro) |
| POST | `/estimate-skeleton` | Estimate skeleton |
| POST | `/interpolation-v2` | Interpolate (Pro) |
| POST | `/transfer-outfit-v2` | Transfer outfit (Pro) |

### Background Jobs

| method | path | summary |
| --- | --- | --- |
| GET | `/background-jobs/{job_id}` | Get background job status |

### Character Management

| method | path | summary |
| --- | --- | --- |
| GET | `/characters` | List user's characters |
| DELETE | `/characters/{character_id}` | Delete a character and all associated data |
| GET | `/characters/{character_id}` | Get character details |
| PATCH | `/characters/{character_id}/tags` | Update character tags |
| GET | `/characters/{character_id}/zip` | Export character as ZIP |

### Character from template

| method | path | summary |
| --- | --- | --- |
| POST | `/animate-character` | Animate character |
| POST | `/characters/animations` | Create Character Animation |
| POST | `/create-character-pro` | Create character with Pro mode (8 directions) |
| POST | `/create-character-v3` | Create character with v3 model (8 rotations) |
| POST | `/create-character-with-4-directions` | Create character with 4 directions |
| POST | `/create-character-with-8-directions` | Create character with 8 directions |

### Characters

| method | path | summary |
| --- | --- | --- |
| POST | `/create-character-state` | Create a state of an existing character |

### Create Image

| method | path | summary |
| --- | --- | --- |
| POST | `/create-image-bitforge` | Create image (bitforge) |
| POST | `/create-image-pixen` | Create image (pixen) |
| POST | `/create-image-pixflux` | Create image (pixflux) |
| POST | `/generate-image-v2` | Generate image (Pro) |
| POST | `/generate-ui-v2` | Generate UI (Pro) |
| POST | `/generate-with-style-v2` | Generate with style (Pro) |

### Create map

| method | path | summary |
| --- | --- | --- |
| POST | `/create-isometric-tile` | Create isometric tile (async processing) |
| POST | `/create-tiles-pro` | Create tiles pro (async processing) |
| POST | `/create-tileset` | Create top-down tileset (async processing) |
| POST | `/create-tileset-sidescroller` | Create sidescroller tileset (async processing) |
| GET | `/isometric-tiles` | List user's isometric tiles |
| GET | `/isometric-tiles/{tile_id}` | Get generated isometric tile by ID |
| GET | `/tiles-pro/{tile_id}` | Get generated tiles pro by ID |
| GET | `/tilesets` | List user's tilesets |
| POST | `/tilesets` | Create a tileset asynchronously |
| POST | `/tilesets-sidescroller` | Create a sidescroller tileset asynchronously |
| GET | `/tilesets/{tileset_id}` | Get generated tileset by ID |

### Documentation

| method | path | summary |
| --- | --- | --- |
| GET | `/llms.txt` | Get LLM-friendly API documentation |

### Edit

| method | path | summary |
| --- | --- | --- |
| POST | `/edit-image` | Edit image |
| POST | `/edit-images-v2` | Edit images (Pro) |

### Image Operations

| method | path | summary |
| --- | --- | --- |
| POST | `/image-to-pixelart` | Convert image to pixel art |
| POST | `/remove-background` | Remove background |
| POST | `/resize` | Resize pixel art image |

### Inpaint

| method | path | summary |
| --- | --- | --- |
| POST | `/inpaint` | Inpaint image |
| POST | `/inpaint-v3` | Inpaint image (Pro) |

### Map Objects

| method | path | summary |
| --- | --- | --- |
| POST | `/map-objects` | Create map object |

### Object Management

| method | path | summary |
| --- | --- | --- |
| GET | `/objects` | List user's objects |
| DELETE | `/objects/{object_id}` | Delete an object and all associated data |
| GET | `/objects/{object_id}` | Get object details |
| PATCH | `/objects/{object_id}/tags` | Update object tags |

### Objects

| method | path | summary |
| --- | --- | --- |
| POST | `/animate-object` | Animate an existing object |
| POST | `/create-object-state` | Create a state of an existing object |
| POST | `/objects` | Create object (1-direction consistent-style or 8-direction) |
| POST | `/objects/{object_id}/dismiss-review` | Dismiss a review object without saving any frames |
| POST | `/objects/{object_id}/select-frames` | Promote selected frames of a review object to completed objects |

### Rotate

| method | path | summary |
| --- | --- | --- |
| POST | `/generate-8-rotations-v2` | Generate 8 rotations (Pro) |
| POST | `/generate-8-rotations-v3` | Generate 8 rotations v3 |
| POST | `/rotate` | Rotate character or object |

## Endpoints

### Account

#### GET `/balance`

- Summary: Get balance
- Description: Returns the current balance for your account, including both USD credits and remaining subscription generations.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")
balance = client.get_balance()
print(f"Credits: ${balance.credits.usd}")
print(f"Generations remaining: {balance.subscription.generations}/{balance.subscription.total}")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved balance | BalanceResponse |
| 401 | Invalid API token |  |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/balance' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

### Animate

#### POST `/animate-with-skeleton`

- Summary: Animate with skeleton
- Description: Creates a pixel art animation based on the provided parameters. Called "Animate with skeleton" in the plugin.

Supported image sizes: 
- 16x16
- 32x32
- 64x64
- 128x128
- 256x256  

Supported features:
- Inpainting
- Init image
- Forced palette

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.animate_with_skeleton(
    view="side",
    direction="south",
    image_size=dict(width=64, height=64),
    reference_image=reference_image,
    inpainting_images=existing_animation_frames,
    mask_images=mask_images,
    skeleton_keypoints=skeleton_keypoints,
)
images = [image.pil_image() for image in response.images]
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image_size | app__endpoints__external__v2__animate_with_skeleton__ImageSize | yes |  |  |
| guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the reference image and skeleton keypoints |
| view | CameraView | no | enum=side, low top-down, high top-down | Camera view angle |
| direction | Direction | no | enum=north, north-east, east, south-east, south, south-west, west, north-west | Subject direction |
| isometric | boolean | no |  | Generate in isometric view |
| oblique_projection | boolean | no |  | Generate in oblique projection |
| init_images | array[Base64Image] / null | no |  | Initial images to start the generation from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| skeleton_keypoints | array[array[Point]] | no |  | Skeleton points |
| reference_image | Base64Image | yes |  | Reference image |
| inpainting_images | array[Base64Image / null] | no |  | Images used for showing the model with connected skeleton |
| mask_images | array[Base64Image / null] | no |  | Inpainting / mask image (black and white image, where the white is where the model should inpaint) |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | AnimateWithSkeletonResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-with-skeleton' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"image_size": {"height": 64, "width": 64}}'
```

#### POST `/animate-with-text`

- Summary: Animate with text
- Description: Creates a pixel art animation based on text description and parameters.

Supported image sizes: 
- 64x64

Supported features:
- Text-guided animation generation
- Inpainting
- Init image
- Forced palette
- Multiple frames

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.animate_with_text(
    description="human mage",
    action="walk",
    view="side",
    direction="south",
    image_size=dict(width=64, height=64),
    reference_image=reference_image,
    n_frames=4
)
images = [image.pil_image() for image in response.images]
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image_size | app__endpoints__external__v2__animate_with_text__ImageSize | yes |  |  |
| description | string | yes |  | Character description |
| negative_description | string / null | no |  | Negative prompt to guide what not to generate |
| action | string | yes |  | Action description |
| text_guidance_scale | number / null | no |  | How closely to follow the text prompts |
| image_guidance_scale | number / null | no |  | How closely to follow the reference image |
| n_frames | integer / null | no |  | Length of full animation (the model will always generate 4 frames) |
| start_frame_index | integer / null | no |  | Starting frame index of the full animation |
| view | CameraView | no | enum=side, low top-down, high top-down | Camera view angle |
| direction | Direction | no | enum=north, north-east, east, south-east, south, south-west, west, north-west | Subject direction |
| init_images | array[Base64Image] / null | no |  | Initial images to start the generation from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| reference_image | Base64Image | yes |  | Reference image |
| inpainting_images | array[Base64Image / null] | no |  | Existing animation frames to guide the generation |
| mask_images | array[Base64Image / null] / null | no |  | Inpainting / mask image (black and white image, where the white is where the model should inpaint) |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed for reproducible results (0 for random) |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated animation | AnimateWithTextResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-with-text' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"action": "walk", "description": "human mage", "direction": "south", "image_size": {"height": 64, "width": 64}, "view": "side"}'
```

#### POST `/animate-with-text-v2`

- Summary: Animate with text (pro)
- Description: Generate pixel art animation from text.

This endpoint creates animations from a reference image and action description.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Text-guided animation generation
- Automatic background removal
- Non-blocking: returns job ID immediately

**Frame Counts by Size:**
- 32x32 pixels: Returns 16 animation frames
- 64x64 pixels: Returns 16 animation frames
- 128x128 pixels: Returns 4 animation frames
- 170x170 pixels: Returns 4 animation frames
- 256x256 pixels: Returns 4 animation frames

**Supported Actions:**
- Simple actions: walk, run, jump, attack
- Complex actions: cast spell, dance, celebrate
- Any action description in natural language

**Camera Views:**
- `none`: No camera hint (default)
- `low top-down`: Classic 3/4 RPG view (~20 degrees from horizontal)
- `high top-down`: Steeper overhead angle (~35 degrees)
- `side`: Side scroller, eye level view

**Directions:**
- `none`: No direction hint (default)
- `south`: Front visible, `north`: Back visible
- `east`: Facing right, `west`: Facing left
- Also: `south-east`, `south-west`, `north-east`, `north-west`

**Image Size Limits:**
- Supported sizes: 32x32 to 256x256 pixels for both reference and output images
- Recommended: 64x64 for best quality/performance balance

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, animation frames are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.animate_with_text_v2(
    reference_image=reference_image,
    reference_image_size=dict(width=64, height=64),
    action="walk",
    image_size=dict(width=64, height=64),
    view="low top-down",
    direction="south",
)

# Access individual frames
for i, frame in enumerate(response.images):
    frame.pil_image().save(f"frame_{i}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| reference_image | Base64Image | yes |  | Reference image (character/object to animate) as base64 PNG/JPEG |
| reference_image_size | app__endpoints__external__v2__animate_with_text_v2__ReferenceImageSize | yes |  | Size of the reference image |
| action | string | yes | minLen=1; maxLen=500 | Action description (e.g., 'walk', 'jump', 'attack') |
| image_size | app__endpoints__external__v2__animate_with_text_v2__ImageSize | yes |  | Size of each animation frame |
| seed | integer / null | no |  | Seed for reproducible generation (0 for random) |
| no_background | boolean / null | no |  | Remove background from generated frames |
| view | string | no | enum=none, low top-down, high top-down, side | Camera perspective angle. ('none', 'low top-down', 'high top-down', 'side') |
| direction | string | no | enum=none, south, east, west, north, south-east, south-west, north-east, north-west | Direction the character faces during the animation. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Animation job accepted and processing | AnimateWithTextV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-with-text-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"action": "walk", "direction": "south", "image_size": {"height": 64, "width": 64}, "no_background": true, "reference_image": {"base64": "data:image/png;base64,..."}, "reference_image_size": {"height": 64, "width": 64}, "seed": 42, "view": "low top-down"}'
```

#### POST `/animate-with-text-v3`

- Summary: Animate with text v3
- Description: Generate an animation from a reference frame and a text action description.

Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to retrieve results when generation completes.

**How it works:**
1. Submit the first frame and action description; receive a `background_job_id` immediately.
2. Poll `GET /v2/background-jobs/{background_job_id}` every 2-5 seconds.
3. When `status` is `completed`, the generated frames are available at `last_response.images`.

**Size Limits:**
- Maximum image dimension: 256x256 pixels
- Total pixel budget: width  height  frame_count  524,288

**Frame Count Guidelines:**
- 4 frames: Simple loops (idle, breathing)
- 8 frames: Standard animations (walk, run)
- 16 frames: Complex animations (attack combos)

Typical generation time: 30-180 seconds.

Using the Python client:
```python
import pixellab, time

client = pixellab.Client(secret="YOUR_API_TOKEN")

job = client.animate_with_text_v3(
    first_frame=first_frame_image,
    action="walking forward",
    frame_count=8,
)

while True:
    result = client.get_background_job(job.background_job_id)
    if result.status == "completed":
        images = [img.pil_image() for img in result.last_response["images"]]
        break
    if result.status == "failed":
        raise RuntimeError(result.last_response["detail"])
    time.sleep(2)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| first_frame | Base64Image | yes |  | First frame to animate (PNG/JPEG base64, max 256x256 pixels) |
| last_frame | Base64Image / null | no |  | Optional last frame to guide the animation endpoint (PNG/JPEG base64, max 256x256 pixels) |
| action | string | yes | minLen=1; maxLen=500 | Action description (e.g., 'walking', 'jumping', 'attacking') |
| frame_count | integer | no | min=4.0; max=16.0 | Number of animation frames (4-16, must be even) |
| seed | integer / null | no |  | Seed for reproducible generation (0 for random) |
| no_background | boolean / null | no |  | Remove background from generated frames |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Background job accepted | AnimateWithTextV3Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent background jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-with-text-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"action": "walking forward", "first_frame": {"base64": "data:image/png;base64,..."}, "frame_count": 8, "no_background": true, "seed": 42}'
```

#### POST `/edit-animation-v2`

- Summary: Edit animation (Pro)
- Description: Edit multiple animation frames with a text description.

This endpoint applies a consistent edit across all animation frames,
maintaining temporal coherence while transforming the animation.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Apply text-based edits to animation sequences
- Maintains frame-to-frame consistency
- Supports 2-16 frames per request
- Optional background removal
- Non-blocking: returns job ID immediately

**Example Edits:**
- "add a red cape"
- "make it glow blue"
- "add armor plating"
- "change to ice theme"

**Supported Sizes:**
- Frame sizes from 16x16 to 256x256 pixels (model supports up to 256x256)

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, edited frames are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Load your animation frames
frames = [...]  # List of FrameImage objects

response = client.edit_animation_v2(
    description="add a glowing sword",
    frames=frames,
    image_size=dict(width=64, height=64)
)

# Save edited frames
for i, image in enumerate(response.images):
    image.pil_image().save(f"frame_{i}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the edit to apply (e.g., 'add a red cape', 'make it glow blue') |
| frames | array[app__endpoints__external__v2__edit_animation_v2__FrameImage] | yes | minItems=2; maxItems=16 | Animation frames to edit (2-16 frames) |
| frames[].image | Base64Image | yes |  | Frame image as base64 PNG/JPEG |
| frames[].size | app__endpoints__external__v2__edit_animation_v2__FrameImageSize | yes |  | Size of the frame image |
| image_size | app__endpoints__external__v2__edit_animation_v2__ImageSize | yes |  | Size of the output frames |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from edited frames |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Animation edit job accepted and processing | EditAnimationV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/edit-animation-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "add a glowing red aura", "frames": [{"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}], "image_size": {"height": 64, "width": 64}, "no_background": false, "seed": 42}'
```

#### POST `/estimate-skeleton`

- Summary: Estimate skeleton
- Description: Estimates the skeleton of a character, returning a list of keypoints to use with the skeleton animation tool.

Supported image sizes: 
- 16x16
- 32x32
- 64x64
- 128x128
- 256x256

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.estimate_skeleton(
    image=image_of_the_character_on_a_transparent_background,
)
response.keypoints
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image | Base64Image | no |  | Image for which to estimate the skeleton |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | EstimateSkeletonResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/estimate-skeleton' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"image": {"base64": "<base64_png>"}}'
```

#### POST `/interpolation-v2`

- Summary: Interpolate (Pro)
- Description: Generate intermediate animation frames between two keyframe images.

This endpoint creates smooth transitions between a start and end image,
generating intermediate frames that animate the transformation.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Generates intermediate frames between two keyframes
- Text-guided interpolation with action descriptions
- Maintains visual consistency across frames
- Optional background removal
- Non-blocking: returns job ID immediately

**Example Actions:**
- "morphing"
- "transforming into a werewolf"
- "powering up with energy"
- "dissolving into particles"
- "walking forward"

**Output:**
- Returns multiple interpolated frames (typically 4-8 frames)

**Supported Sizes:**
- Frame sizes from 16x16 to 128x128 pixels

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, interpolated frames are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.interpolation_v2(
    start_image=start_keyframe,
    end_image=end_keyframe,
    action="transforming",
    image_size=dict(width=64, height=64)
)

# Save interpolated frames
for i, image in enumerate(response.images):
    image.pil_image().save(f"frame_{i}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| start_image | KeyframeImage | yes |  | Starting keyframe image |
| end_image | KeyframeImage | yes |  | Ending keyframe image |
| action | string | yes | minLen=1; maxLen=500 | Description of the transition (e.g., 'morphing', 'transforming', 'powering up') |
| image_size | app__endpoints__external__v2__interpolation_v2__ImageSize | yes |  | Size of the output frames |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from output frames |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Interpolation job accepted and processing | InterpolationV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/interpolation-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"action": "transforming into a werewolf", "end_image": {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, "image_size": {"height": 64, "width": 64}, "no_background": true, "seed": 42, "start_image": {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}}'
```

#### POST `/transfer-outfit-v2`

- Summary: Transfer outfit (Pro)
- Description: Transfer an outfit/appearance from a reference image to animation frames.

This endpoint takes a reference image containing a desired outfit or appearance
and applies it consistently across multiple animation frames.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Transfer outfit/appearance from reference to animation
- Maintains animation motion while changing appearance
- Supports 2-16 frames per request
- Optional background removal
- Non-blocking: returns job ID immediately

**Use Cases:**
- Apply armor/clothing to walking animation
- Change character skin/color across animation
- Transfer weapon or equipment to action sequence
- Consistent reskin of sprite animations

**Supported Sizes:**
- Frame sizes from 32x32 to 256x256 pixels

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, frames are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.transfer_outfit_v2(
    reference_image=outfit_reference,  # Image with desired outfit
    frames=animation_frames,           # Animation to apply outfit to
    image_size=dict(width=64, height=64)
)

# Save frames with transferred outfit
for i, image in enumerate(response.images):
    image.pil_image().save(f"frame_{i}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| reference_image | app__endpoints__external__v2__transfer_outfit_v2__ReferenceImage | yes |  | Reference image containing the outfit/appearance to transfer |
| frames | array[app__endpoints__external__v2__transfer_outfit_v2__FrameImage] | yes | minItems=2; maxItems=16 | Animation frames to apply the outfit to (2-16 frames) |
| frames[].image | Base64Image | yes |  | Frame image as base64 PNG/JPEG |
| frames[].size | app__endpoints__external__v2__transfer_outfit_v2__FrameImageSize | yes |  | Size of the frame image |
| image_size | app__endpoints__external__v2__transfer_outfit_v2__ImageSize | yes |  | Size of the output frames |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from output frames |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Outfit transfer job accepted and processing | TransferOutfitV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/transfer-outfit-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"frames": [{"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}], "image_size": {"height": 64, "width": 64}, "no_background": false, "reference_image": {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, "seed": 42}'
```

### Background Jobs

#### GET `/background-jobs/{job_id}`

- Summary: Get background job status
- Description: Check the status and results of a background job.

This endpoint allows you to monitor the progress of background operations like
character creation and animation generation. Background jobs are used for 
expensive operations that take time to complete.

**Job Statuses:**
- `processing` - Job is currently running
- `completed` - Job finished successfully 
- `failed` - Job encountered an error

**Usage Pattern:**
1. Create a character or animation (returns `background_job_id`)
2. Poll this endpoint periodically to check status
3. When status is `completed`, access results in `last_response`

**Response Data:**
- For character creation: `character_id`, `directions_count`, character info
- For animations: `animation_id`, `frame_count`, animation details
- Storage information and file organization details

**Authentication:**
Requires a valid API token. You can only access jobs you created.

**Error Handling:**
- 404: Job not found or doesn't belong to you
- Jobs are automatically cleaned up after completion

**Polling Recommendations:**
- Poll every 5-10 seconds while status is `processing`
- Stop polling once status is `completed` or `failed`
- Character creation typically takes 30-60 seconds
- Animations may take longer depending on frame count and directions

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| job_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved job status | BackgroundJobResponse |
| 401 | Invalid API token |  |
| 404 | Job not found or doesn't belong to user |  |
| 429 | Too many requests |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/background-jobs/{job_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

### Character Management

#### GET `/characters`

- Summary: List user's characters
- Description: List all characters created by the authenticated user.

This endpoint returns a paginated list of all characters you've created using the 
create-character-with-4-directions or create-character-with-8-directions endpoints.

**Features:**
- Pagination support with limit and offset parameters
- Animation count for each character
- Preview URLs for quick character identification
- Complete character metadata

**Authentication:**
Requires a valid API token in the Authorization header.

**Response includes:**
- Character basic info (name, prompt, size, directions)
- Creation timestamp and template used
- Number of animations created for each character
- Preview URL for the south-facing rotation

**Pagination:**
- Use `limit` to control how many characters to return (1-100)
- Use `offset` to skip characters for pagination
- Total count is included in response for pagination UI

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| limit | query | no | integer | Maximum number of characters to return |
| offset | query | no | integer | Number of characters to skip |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved character list | CharactersListResponse |
| 401 | Invalid API token |  |
| 422 | Invalid pagination parameters |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/characters' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### DELETE `/characters/{character_id}`

- Summary: Delete a character and all associated data
- Description: Delete a character (v2 API for external customers).

Uses the same internal logic as JWT and MCP endpoints, providing
fast storage deletion by using service_role internally (avoiding
the slow storage.search_legacy_v1 function).

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| character_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successful Response | DeleteCharacterResponse |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X DELETE 'https://api.pixellab.ai/v2/characters/{character_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### GET `/characters/{character_id}`

- Summary: Get character details
- Description: Get detailed information about a specific character.

This endpoint returns complete character information including all rotation image URLs,
generation settings, and metadata.

**Features:**
- Complete character information and settings
- URLs for all rotation images (4 or 8 directions)
- Animation count and template information
- Generation parameters used during creation

**Authentication:**
Requires a valid API token. You can only access characters you created.

**Response includes:**
- Basic character info (name, prompt, size, directions)
- All rotation image URLs (publicly accessible)
- Style settings and generation parameters
- Template information and view settings
- Animation count for this character

**URL Format:**
All rotation URLs follow the pattern:
`https://supabase.pixellab.ai/storage/v1/object/public/pixellab-characters/{user_id}/{character_id}/rotations/{direction}.png`

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| character_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved character details | CharacterDetail |
| 401 | Invalid API token |  |
| 403 | Character belongs to another user |  |
| 404 | Character not found |  |
| 429 | Too many requests |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/characters/{character_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### PATCH `/characters/{character_id}/tags`

- Summary: Update character tags
- Description: Update the tags for a specific character.

This endpoint replaces all tags for a character with the provided list.
Tags are used for filtering and organizing your characters.

**Features:**
- Replace all tags at once (set operation)
- Automatic normalization (trim whitespace)
- Case-insensitive duplicate detection
- Maximum 20 tags per character
- Maximum 50 characters per tag

**Tag Validation:**
- Empty strings are ignored
- Duplicate tags (case-insensitive) are removed
- Leading/trailing whitespace is trimmed
- Tags longer than 50 characters are rejected

**Common Use Cases:**
- Organize characters by game genre: ["rpg", "fantasy"]
- Mark character types: ["npc", "enemy", "boss"]
- Track creation status: ["finished", "needs-animation"]
- Group by visual style: ["cute", "pixel-art", "8-bit"]

**Authentication:**
Requires a valid API token. You can only update tags for characters you created.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| character_id | path | yes | string |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| tags | array[string] | yes | maxItems=20 | List of tags to assign to the character |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Tags updated successfully | UpdateTagsResponse |
| 400 | Invalid tag format or validation error |  |
| 401 | Invalid API token |  |
| 403 | Character belongs to another user |  |
| 404 | Character not found |  |
| 429 | Too many requests |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X PATCH 'https://api.pixellab.ai/v2/characters/{character_id}/tags' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"tags": ["wizard", "magic", "fire", "rpg"]}'
```

#### GET `/characters/{character_id}/zip`

- Summary: Export character as ZIP
- Description: Download a character with all animations as a ZIP file.

This endpoint creates a ZIP file containing all rotation images, animation frames,
and metadata for a character. Perfect for using characters in external tools,
game engines, or archiving your creations.

**ZIP Contents:**
- `rotations/` - All character rotation images (4 or 8 directions)
- `animations/` - All animation frames organized by animation type and direction  
- `metadata.json` - Complete character information with keypoints for all frames

**Collision Detection**: Includes keypoints for all frames + PNG transparency for pixel-perfect collision detection.

**File Structure:**
```
character_name.zip
 rotations/
    south.png
    west.png
    east.png
    north.png
    [8-direction files if applicable]
 animations/
    {animation_type}/
        {direction}/
            frame_000.png
            frame_001.png
            ...
 metadata.json
```

**Metadata Structure:**
The metadata.json includes:
- Character information (name, prompt, size, template)
- File organization structure
- Keypoints data for template-based characters
- Export version and timestamp

**Keypoints Data:**
For characters created with templates, keypoints are included with:
- x,y coordinates for each body part
- Labels (nose, left_arm, etc.)
- Scaled to character's actual size
- Available for all rotations and animation frames

**Authentication:**
No authentication required - the random character ID serves as the access key.

**File Size:**
ZIP files are uncompressed for faster generation and compatibility.
File size depends on character image size and number of animations.

**Status Codes:**
- 200: ZIP file ready for download
- 423: Character or animations still being generated (check status later)
- 404: Character not found

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| character_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | ZIP file download containing character data |  |
| 423 | Character or animations still being generated |  |
| 404 | Character not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/characters/{character_id}/zip' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

### Character from template

#### POST `/animate-character`

- Summary: Animate character
- Description: Animate an existing character with multiple frames showing movement or action.

This endpoint creates animation sequences for characters that were previously created using
the create-character-with-4-directions or create-character-with-8-directions endpoints.

**Key Features:**
- Animate existing characters by character_id
- Support for multiple directions (or all character directions)
- Flexible frame count (2-12 frames)
- Template-based animations for consistent motion
- Asynchronous processing for multiple directions
- Automatic storage and organization

**Character Requirements:**
- Character must exist and belong to the authenticated user
- Character must have been created with 4 or 8 directions
- Animation will use the same template and settings as the character

**Direction Handling:**
- Multiple directions per request (specify via directions field)
- If directions field is None/empty, animates all available directions
- Each direction creates a separate background job
- Returns list of job IDs (one per direction)

**AI Freedom Parameter:**
- ai_freedom controls how closely the AI follows the template (0=strict, 1000=creative)
- Lower values produce more consistent animations
- Higher values allow more creative variations

**Style Settings:**
- Uses the same style settings (outline, shading, detail) as the original character by default
- Can override individual style settings in the request

**Frame Count:**
- Determined by the animation template (not configurable in request)
- Typically 4-6 frames for most animations

**Image Size:**
- Uses the same image size as the original character
- All frames have consistent dimensions
- Stored in organized folder structure

**Pricing:**
- Template mode: 1 generation per direction
- Custom mode: 20-40 generations per direction (depending on character size)

**V3 Mode (default when no template):**
Custom animation. Provide `action_description` and optionally `frame_count` (4-16, default 8).
- One job per direction, directions independent
- Directions default to south only if not specified
- Best for: single-direction animations, frame count control

**Pro Mode:**
Custom animation with sequential direction generation. Set `mode="pro"` with `action_description`.
- Generates directions one-by-one, using completed sides as reference
- 20-40 generations per direction depending on character size
- Directions default to south only if not specified

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| character_id | string | yes |  | ID of existing character to animate |
| animation_name | string / null | no |  | Name for this animation (defaults to action_description if not provided) |
| description | string / null | no |  | Description of the character or object to animate (uses character's original if not specified) |
| action_description | string / null | no |  | Action description (e.g., 'walking', 'running', 'jumping'). Required for custom mode (when template_animation_id is omitted). For template mode, defaults to a description based on the template. |
| async_mode | boolean / null | no |  | Process in background (always true - no foreground processing yet) |
| mode | string / null | no |  | Animation mode. "template": skeleton-based from template_animation_id (1 gen/direction). "v3": custom animation from action_description with frame_count control. "pro": custom animation that generates directions sequentially, using completed sides as reference (20-40 gen/direction). Auto-detected: template if template_animation_id is provided, v3 otherwise. |
| template_animation_id | string / null | no |  | Animation template ID. Required for template mode. Available: `angry`, `attack`, `attack-back`, `attack-left`, `attack-right`, `backflip`, `bark`, `breathing-idle`, `cross-punch`, `crouched-walking`, ... |
| frame_count | integer / null | no |  | Number of animation frames (4-16, must be even). Only used in v3 mode. |
| text_guidance_scale | number / null | no |  | How closely to follow the text description (higher = more faithful). Template mode only. |
| outline | string / null | no |  | Outline style (uses character's original if not specified). Template mode only. |
| shading | string / null | no |  | Shading style (uses character's original if not specified). Template mode only. |
| detail | string / null | no |  | Detail level (uses character's original if not specified). Template mode only. |
| directions | array[string] / null | no |  | List of directions to animate (south, north, east, west, etc.). Template mode: defaults to all character directions. Custom mode: defaults to south only. |
| isometric | boolean / null | no |  | Generate in isometric view |
| color_image | Base64Image / null | no |  | Color palette reference image |
| force_colors | boolean / null | no |  | Force the use of colors from color_image |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully started character animation in background | CreateCharacterAnimationResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 404 | Character not found |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-character' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"character_id": "123e4567-e89b-12d3-a456-426614174000", "directions": ["south", "north"], "template_animation_id": "walking-4-frames"}'
```

#### POST `/characters/animations`

- Summary: Create Character Animation
- Description: Animate an existing character (background processing).

Three modes:
- **template**: Provide template_animation_id for skeleton-based animation (1 gen/direction).
- **v3** (default when no template): Custom animation from action text. Supports frame_count (4-16). One job per direction.
- **pro**: Custom animation that generates directions sequentially, using completed sides as reference (20-40 gen/direction).

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| character_id | string | yes |  | ID of existing character to animate |
| animation_name | string / null | no |  | Name for this animation (defaults to action_description if not provided) |
| description | string / null | no |  | Description of the character or object to animate (uses character's original if not specified) |
| action_description | string / null | no |  | Action description (e.g., 'walking', 'running', 'jumping'). Required for custom mode (when template_animation_id is omitted). For template mode, defaults to a description based on the template. |
| async_mode | boolean / null | no |  | Process in background (always true - no foreground processing yet) |
| mode | string / null | no |  | Animation mode. "template": skeleton-based from template_animation_id (1 gen/direction). "v3": custom animation from action_description with frame_count control. "pro": custom animation that generates directions sequentially, using completed sides as reference (20-40 gen/direction). Auto-detected: template if template_animation_id is provided, v3 otherwise. |
| template_animation_id | string / null | no |  | Animation template ID. Required for template mode. Available: `angry`, `attack`, `attack-back`, `attack-left`, `attack-right`, `backflip`, `bark`, `breathing-idle`, `cross-punch`, `crouched-walking`, ... |
| frame_count | integer / null | no |  | Number of animation frames (4-16, must be even). Only used in v3 mode. |
| text_guidance_scale | number / null | no |  | How closely to follow the text description (higher = more faithful). Template mode only. |
| outline | string / null | no |  | Outline style (uses character's original if not specified). Template mode only. |
| shading | string / null | no |  | Shading style (uses character's original if not specified). Template mode only. |
| detail | string / null | no |  | Detail level (uses character's original if not specified). Template mode only. |
| directions | array[string] / null | no |  | List of directions to animate (south, north, east, west, etc.). Template mode: defaults to all character directions. Custom mode: defaults to south only. |
| isometric | boolean / null | no |  | Generate in isometric view |
| color_image | Base64Image / null | no |  | Color palette reference image |
| force_colors | boolean / null | no |  | Force the use of colors from color_image |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successful Response | CreateCharacterAnimationResponse |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/characters/animations' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"character_id": "123e4567-e89b-12d3-a456-426614174000", "directions": ["south", "north"], "template_animation_id": "walking-4-frames"}'
```

#### POST `/create-character-pro`

- Summary: Create character with Pro mode (8 directions)
- Description: Create a character with 8 directional rotations using Pro mode.

Pro mode uses a reference-based generator for higher quality and finer style control than
template-based standard mode. The result is persisted as a
character - same system as `/v2/create-character-with-8-directions` - so it can be
animated, downloaded, and listed alongside template-created characters.

**Three methods** (controlled by the `method` field):
- `create_with_style` (default): text + optional style reference image.
- `create_from_concept`: text + concept image (e.g. a sketch/mood board) + optional style reference.
- `rotate_character`: rotate an existing character image into 8 directions.

**Sizes:**
- `image_size`: 32-168 pixels (output frame). Canvas is padded ~2x for animation room.
- `reference_image`: max 168x168.
- `concept_image`: max 1024x1024.

**Cost:** dynamic - typically 20-40 generations depending on output size.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Style-guided creation
response = client.create_character_pro(
    description="cyberpunk samurai with red coat",
    image_size=dict(width=96, height=96),
    method="create_with_style",
)

# Rotate an existing character
response = client.create_character_pro(
    description="cyberpunk samurai",
    image_size=dict(width=96, height=96),
    method="rotate_character",
    reference_image=dict(base64=existing_character_b64),
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the character or object to generate. |
| image_size | ProImageSize | yes |  | Output frame size for each of the 8 rotations. The persisted character canvas is padded to ~2x for animation room. |
| method | string | no | enum=create_with_style, create_from_concept, rotate_character | How the reference inputs are used: - `create_with_style`: text-driven generation; `reference_image` (if provided) is treated as a style reference. If omitted, a default style for the chosen `view` and template body type is used. - `create_from_concept`: `concept_image` (required) seeds the design; `reference_image` (optional) provides additional style guidance. - `rotate_character`: `reference_image` (required) is an existing character to rotate into 8 directions. `description` is still used as guidance. |
| view | string | no | enum=low top-down, high top-down, side | Camera view angle. |
| template_id | string | no |  | Body type for skeleton reconstruction. Picks the 3D template the skeleton estimator fits to the generated frames so the character can be animated. Use `mannequin` for bipedal subjects or one of `bear`/`cat`/`dog`/`horse`/`lion` for quadrupeds. Quadruped templates also append ", on all fours" to the description so generated frames match the chosen skeleton. |
| concept_image | Base64Image / null | no |  | Optional concept image (max 1024x1024). Used with `method=create_from_concept`. |
| reference_image | Base64Image / null | no |  | Optional reference image (max 168x168). Used as style reference for `create_with_style` / `create_from_concept`, or as the character to rotate for `rotate_character`. |
| style_description | string / null | no |  | Free-text style hint to layer on top of the description. |
| seed | integer / null | no |  | Seed for reproducible generation. |
| no_background | boolean / null | no |  | Generate with transparent background. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Generation job submitted | CreateCharacterProResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error (bad dimensions, missing required image) |  |
| 429 | Concurrency limit reached |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-pro' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "wizard with blue robes and silver staff", "image_size": {"height": 96, "width": 96}, "method": "create_with_style", "template_id": "mannequin", "view": "low top-down"}'
```

#### POST `/create-character-v3`

- Summary: Create character with v3 model (8 rotations)
- Description: Create a character with 8 directional rotations using the v3 model.

Currently takes a south-facing `reference_image` and rotates it into 8 directional views.
From-scratch generation (no reference image) is planned and will be added to this same
endpoint so it covers the full v3 character-creation surface.

The result is persisted as a character - same system as
`/v2/create-character-with-8-directions` - so it can be animated, downloaded, and listed
alongside template-created characters.

**Reference image must be south-facing for best results.** The frontend Character Creator
enforces this and the v3 model is trained around a south-facing input. Other orientations
work but produce noticeably worse results.

**How it works:**
1. Submit a south-facing `reference_image`.
2. The v3 model generates 8 frames in the canonical
   `south, south-east, east, ..., south-west` order.
3. Frames are padded for animation, uploaded to storage, a 3D skeleton is estimated, and
   the character row is marked `completed`.

**Sizes:**
- `reference_image`: max 256x256 pixels.
- `image_size`: advisory; final canvas is determined by the v3 model output + pad-for-animation
  (capped at 256).

**Cost:** 1 generation (charged in the inner v3 job).

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.create_character_v3(
    description="cyberpunk samurai",
    reference_image=dict(base64=south_facing_image_b64),
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the character (used as prompt + display name). |
| reference_image | Base64Image | yes |  | Reference image of the character (PNG/JPEG base64). Must be **south-facing** (character viewed from the front) and max 256x256 pixels. |
| image_size | V3OutputImageSize / null | no |  | Advisory output frame size. The model picks its own output size; this is the initial canvas size recorded on the character row. The final canvas is padded ~2x by the persistence step for animation room (capped at 256). |
| view | string | no | enum=low top-down, high top-down, side | Camera view angle. Used by skeleton reconstruction. |
| template_id | string | no |  | Body type for skeleton reconstruction. Picks the 3D template the skeleton estimator fits to the generated frames so the character can be animated. Use `mannequin` for bipedal subjects or one of `bear`/`cat`/`dog`/`horse`/`lion` for quadrupeds. Must match the body type in `reference_image`. |
| name | string / null | no |  | Display name. Defaults to first 50 chars of `description`. |
| seed | integer / null | no |  | Seed for reproducible generation. |
| no_background | boolean / null | no |  | Generate frames with transparent background. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Generation job submitted | CreateCharacterV3Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error (bad dimensions, invalid image) |  |
| 429 | Concurrency limit reached |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "knight in golden armor", "reference_image": {"base64": "data:image/png;base64,..."}, "template_id": "mannequin", "view": "low top-down"}'
```

#### POST `/create-character-with-4-directions`

- Summary: Create character with 4 directions
- Description: Generate a character or object facing 4 cardinal directions (south, west, east, north).

This endpoint creates 4 separate rotation images plus a combined spritesheet in a 4x1 layout.
Perfect for game development where you need character sprites facing all directions.

**Key Features:**
- Fixed 4-rotation layout (south, west, east, north)
- Individual rotation images + combined spritesheet
- Style customization (outline, shading, detail)
- Color palette support
- Character proportions customization
- Optional reference images per direction (upload some or all)
- Optimized for game sprites and character assets

**Character Proportions:**
- Use preset proportions: chibi, cartoon, stylized, realistic_male, realistic_female, heroic
- Or customize individual body proportions: head size, arm/leg length, shoulder/hip width
- All characters use the advanced mannequin template with bone scaling

**Reference Images (optional `directions` field):**
- Provide existing sprites for some or all of south/east/north/west.
- Missing directions are AI-generated; provided ones are used as-is (frozen).
- Each image's dimensions must match `image_size` exactly (else 422).
- 'south' is required when any reference is provided (bipedal). Quadruped templates
  (bear/cat/dog/horse/lion) additionally require 'east'. Oblique view requires all
  4 cardinals.
- When provided, `proportions` / bone scaling is ignored - the reference images
  drive the pose.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Example: With preset proportions
response = client.generate_4_rotations(
    description="futuristic robot warrior",
    image_size=dict(width=96, height=96),
    view="low_top_down",
    proportions=dict(
        type="preset",
        name="heroic"
    )
)

# Access individual images by direction
south_facing = response.images["south"]
west_facing = response.images["west"]
east_facing = response.images["east"]
north_facing = response.images["north"]

# Example: Provide existing sprites for some directions; generate the rest
import base64
def to_b64(path):
    return base64.b64encode(open(path, "rb").read()).decode()

response = client.generate_4_rotations(
    description="brave knight",
    image_size=dict(width=32, height=32),
    directions={
        "south": {"base64": to_b64("knight_south.png")},
    },
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the character or object to generate |
| image_size | app__endpoints__external__v2__create_character_with_4_directions__ImageSize | yes |  | Size of each rotation image |
| async_mode | boolean / null | no |  | Process asynchronously (always true for character creation) |
| text_guidance_scale | number / null | no |  | How closely to follow the text description (higher = more faithful) |
| outline | string / null | no |  | Outline style (thin, medium, thick, none) |
| shading | string / null | no |  | Shading style (soft, hard, flat, none) |
| detail | string / null | no |  | Detail level (low, medium, high) |
| view | string / null | no |  | Camera view angle (side, low top-down, high top-down, perspective) |
| isometric | boolean / null | no |  | Generate in isometric view |
| color_image | Base64Image / null | no |  | Color palette reference image |
| force_colors | boolean / null | no |  | Force the use of colors from color_image |
| proportions | CharacterProportionsPreset / CharacterProportions / null | no |  | Character body proportions (preset or custom values). Only applies to humanoid characters. |
| template_id | string / null | no |  | Template ID to use (e.g., 'mannequin' for humanoid, 'bear'/'cat'/'dog'/'horse'/'lion' for quadrupeds). Defaults to 'mannequin'. |
| seed | integer / null | no |  | Seed for reproducible generation |
| directions | object / null | no |  | Optional reference images per direction. Allowed keys: 'south', 'east', 'north', 'west'. Missing directions are AI-generated; provided ones are used as-is. Each image's dimensions must match image_size. Bipedal templates require 'south' if any are provided; quadrupeds require both 'south' and 'east'; oblique view requires all 4 cardinals. |
| directions | object | yes |  |  |
| directions | null | yes |  |  |
| output_type | string / null | no |  | Output format (always dict for external API) |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated 4-rotation images | CreateCharacterWith4DirectionsResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-with-4-directions' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute pixel art wizard with blue robes", "image_size": {"height": 64, "width": 64}}'
```

#### POST `/create-character-with-8-directions`

- Summary: Create character with 8 directions
- Description: Generate a character or object facing 8 directions (all cardinal and diagonal directions).

This endpoint creates 8 rotation images in a dictionary format for easy access by direction name.
Perfect for detailed movement systems in games where smooth directional changes are important.

**Two Generation Modes:**
- **standard** (default): Template-based skeleton generation. Costs 1 generation. Uses all style parameters.
- **pro**: AI reference-based generation for higher quality. Costs 20-40 generations depending on size. Ignores outline, shading, detail, proportions, and text_guidance_scale.

**The 8 Directions:**
- south (facing down)
- south-east (diagonal down-right)
- east (facing right)
- north-east (diagonal up-right)
- north (facing up)
- north-west (diagonal up-left)
- west (facing left)
- south-west (diagonal down-left)

**Key Features:**
- Fixed 8-rotation layout in clockwise order starting from south
- Returns dictionary of images by direction name
- Style customization (outline, shading, detail) - standard mode only
- Color palette support
- Character proportions customization - standard mode only
- Optional reference images per direction (standard mode only)
- Optimized for games requiring smooth directional movement

**Reference Images (optional `directions` field, standard mode only):**
- Provide existing sprites for some or all of the 8 directions.
- Missing directions are AI-generated; provided ones are used as-is (frozen).
- Each image's dimensions must match `image_size` exactly (else 422).
- 'south' is required when any reference is provided (bipedal). Quadruped templates
  (bear/cat/dog/horse/lion) additionally require 'east'.
- When provided, `proportions` / bone scaling is ignored - the reference images
  drive the pose.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Standard mode (default)
response = client.create_character_with_8_directions(
    description="futuristic robot warrior",
    image_size=dict(width=96, height=96),
    view="low top-down",
    proportions=dict(type="preset", name="heroic")
)

# Pro mode (higher quality, costs more)
response = client.create_character_with_8_directions(
    description="futuristic robot warrior",
    image_size=dict(width=48, height=48),
    view="low top-down",
    mode="pro"
)

# Provide existing sprites for some directions; generate the rest (standard only)
import base64
def to_b64(path):
    return base64.b64encode(open(path, "rb").read()).decode()

response = client.create_character_with_8_directions(
    description="brave knight",
    image_size=dict(width=32, height=32),
    directions={
        "south": {"base64": to_b64("knight_south.png")},
        "east":  {"base64": to_b64("knight_east.png")},
    },
)

# Access individual images by direction
south_facing = response.images["south"]
east_facing = response.images["east"]
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the character or object to generate |
| image_size | app__endpoints__external__v2__create_character_with_8_directions__ImageSize | yes |  | Size of each rotation image |
| mode | string / null | no |  | Generation mode. "standard" uses template-based skeleton generation (1 generation). "pro" uses AI reference-based generation for higher quality (costs 20-40 generations depending on size). Pro mode ignores outline, shading, detail, proportions, and text_guidance_scale. |
| async_mode | boolean / null | no |  | Process asynchronously (always true - no synchronous processing yet) |
| text_guidance_scale | number / null | no |  | How closely to follow the text description (higher = more faithful) |
| outline | string / null | no |  | Outline style (thin, medium, thick, none) |
| shading | string / null | no |  | Shading style (soft, hard, flat, none) |
| detail | string / null | no |  | Detail level (low, medium, high) |
| view | string / null | no |  | Camera view angle (side, low top-down, high top-down, perspective) |
| isometric | boolean / null | no |  | Generate in isometric view |
| color_image | Base64Image / null | no |  | Color palette reference image |
| force_colors | boolean / null | no |  | Force the use of colors from color_image |
| proportions | CharacterProportionsPreset / CharacterProportions / null | no |  | Character body proportions (preset or custom values). Only applies to humanoid characters. |
| template_id | string / null | no |  | Template ID to use (e.g., 'mannequin' for humanoid, 'bear'/'cat'/'dog'/'horse'/'lion' for quadrupeds). Defaults to 'mannequin'. |
| seed | integer / null | no |  | Seed for reproducible generation |
| directions | object / null | no |  | Optional reference images per direction. Allowed keys: 'south', 'south-east', 'east', 'north-east', 'north', 'north-west', 'west', 'south-west'. Missing directions are AI-generated; provided ones are used as-is. Each image's dimensions must match image_size. Bipedal templates require 'south' if any are provided; quadrupeds require both 'south' and 'east'. |
| directions | object | yes |  |  |
| directions | null | yes |  |  |
| output_type | string / null | no |  | Output format (always dict for external API) |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated 8-rotation images | CreateCharacterWith8DirectionsResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-with-8-directions' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute pixel art wizard with blue robes", "image_size": {"height": 64, "width": 64}}'
```

### Characters

#### POST `/create-character-state`

- Summary: Create a state of an existing character
- Description: Queues a generation job that applies a text edit to an existing character's rotations and saves the result as a new character grouped with the source via group_id. The same edit is applied consistently across all 4 or 8 directions.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| character_id | string | yes |  | ID of the source character |
| edit_description | string | yes | minLen=1; maxLen=1000 |  |
| no_background | boolean | no |  |  |
| seed | integer / null | no |  |  |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | State queued | CreateCharacterStateResponse |
| 400 | Source character is not completed |  |
| 401 | Invalid API token |  |
| 402 | Insufficient generations |  |
| 404 | Source character not found |  |
| 429 | Concurrent job limit reached |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-state' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"character_id": "456e7890-e89b-12d3-a456-426614174001", "edit_description": "wearing red armor"}'
```

### Create Image

#### POST `/create-image-bitforge`

- Summary: Create image (bitforge)
- Description: Generates a pixel art image based on the provided parameters. Called "Create S-M image" in the plugin.

Supported image size: 
- Maximum area 200x200  

Supported features:
- Style image
- Inpainting
- Init image
- Forced palette
- Transparent background

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_image_bitforge(
    description="cute dragon",
    image_size=dict(width=128, height=128),
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the image to generate |
| negative_description | string | no |  | Text description of what to avoid in the generated image |
| image_size | app__endpoints__external__v2__create_image_bitforge__ImageSize | yes |  |  |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text description |
| extra_guidance_scale | number | no | min=0.0; max=20.0 | (Deprecated) |
| style_strength | number | no | min=0.0; max=100.0 | Strength of the style transfer (0-100) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| view | CameraView / null | no |  | Camera view angle |
| direction | Direction / null | no |  | Subject direction |
| isometric | boolean | no |  | Generate in isometric view |
| oblique_projection | boolean | no |  | Generate in oblique projection |
| no_background | boolean | no |  | Generate with transparent background |
| coverage_percentage | number / null | no |  | Percentage of the canvas to cover |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| style_image | Base64Image / null | no |  | Reference image for style transfer |
| inpainting_image | Base64Image / null | no |  | Reference image which is inpainted |
| mask_image | Base64Image / null | no |  | Inpainting / mask image (black and white image, where the white is where the model should inpaint) |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| skeleton_guidance_scale | number | no | min=0.0; max=5.0 | How closely to follow the skeleton keypoints |
| skeleton_keypoints | array[Point] / null | no |  | Skeleton points. Warning! Sizes that are not 16x16, 32x32 and 64x64 can cause the generations to be lower quality |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | CreateImageBitforgeResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-image-bitforge' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute dragon", "image_size": {"height": 128, "width": 128}, "no_background": true, "style_guidance_scale": 3.0, "style_strength": 20.0, "text_guidance_scale": 3.0}'
```

#### POST `/create-image-pixen`

- Summary: Create image (pixen)
- Description: Generates a pixel art image using the Pixen model.

Supported image size:
- Minimum area 32x32 and maximum area 512x512
- Width and height must be divisible by 4

Supported features:
- Transparent background
- Outline and detail style controls
- View and direction

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_image_pixen(
    description="cute wizard",
    image_size=dict(width=64, height=64),
    no_background=True,
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the image to generate |
| image_size | app__endpoints__external__v2__create_image_pixen__ImageSize | yes |  |  |
| outline | Outline / null | no |  | Outline style |
| detail | Detail / null | no |  | Detail level (default: highly detailed) |
| view | CameraView / null | no |  | Camera view angle |
| direction | Direction / null | no |  | Subject direction |
| no_background | boolean | no |  | Generate with transparent background |
| background_removal_task | string | no | enum=remove_simple_background, remove_complex_background | Background removal complexity. 'remove_simple_background' is faster, 'remove_complex_background' handles complex edges better |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | CreateImagePixenResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-image-pixen' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute wizard", "detail": "highly detailed", "image_size": {"height": 64, "width": 64}, "no_background": true}'
```

#### POST `/create-image-pixflux`

- Summary: Create image (pixflux)
- Description: Creates a pixel art image based on the provided parameters. Called "Create image (new)" in the plugin.

Supported image size: 
- Minimum area 32x32 and maximum area 400x400  

Supported features:
- Init image
- Forced palette
- Transparent background

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_image_pixflux(
    description="cute dragon",
    image_size=dict(width=128, height=128),
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the image to generate |
| negative_description | string | no |  | (Deprecated) |
| image_size | app__endpoints__external__v2__create_image_pixflux__ImageSize | yes |  |  |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text description |
| outline | Outline / null | no |  | Outline style reference (weakly guiding) |
| shading | Shading / null | no |  | Shading style reference (weakly guiding) |
| detail | Detail / null | no |  | Detail style reference (weakly guiding) |
| view | CameraView / null | no |  | Camera view angle (weakly guiding) |
| direction | Direction / null | no |  | Subject direction (weakly guiding) |
| isometric | boolean | no |  | Generate in isometric view (weakly guiding) |
| no_background | boolean | no |  | Generate with transparent background, (blank background over 200x200 area) |
| background_removal_task | string | no | enum=remove_simple_background, remove_complex_background | Background removal complexity. 'remove_simple_background' is faster, 'remove_complex_background' handles complex edges better |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | CreateImagePixfluxResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-image-pixflux' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute dragon", "image_size": {"height": 128, "width": 128}, "no_background": true}'
```

#### POST `/generate-image-v2`

- Summary: Generate image (Pro)
- Description: Generate pixel art images from text description.

This endpoint creates multiple pixel art images based on a text description,
with optional reference images for style and subject guidance.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Text-to-image pixel art generation
- Optional reference images for subject guidance (up to 4)
- Optional style image for consistent pixel art style
- Automatic background removal
- Non-blocking: returns job ID immediately

**Output Counts by Size (by max dimension):**
- Up to 42px: 64 images (8x8 grid)
- 43-85px: 16 images (4x4 grid)
- 86-170px: 4 images (2x2 grid)
- Above 170px: 1 image

**Supported Sizes:**
- Minimum 16x16. Maximum depends on aspect ratio (e.g. 512x512 for square, 688x384 for 16:9).

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, images are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_image_v2(
    description="cute wizard character",
    image_size=dict(width=64, height=64)
)

# Access generated images
for i, image in enumerate(response.images):
    image.pil_image().save(f"image_{i}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the image to generate |
| image_size | app__endpoints__external__v2__generate_image_v2__ImageSize | yes |  | Size of the output image |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from generated images |
| reference_images | array[app__endpoints__external__v2__generate_image_v2__ReferenceImage] / null | no |  | Optional reference images for subject guidance (up to 4) |
| style_image | app__endpoints__external__v2__generate_image_v2__ReferenceImage / null | no |  | Optional style image for pixel size and style reference |
| style_options | StyleOptions | no |  | Options for what to copy from the style image |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Image generation job accepted and processing | GenerateImageV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-image-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "a cute wizard character", "image_size": {"height": 64, "width": 64}, "no_background": true, "seed": 42}'
```

#### POST `/generate-ui-v2`

- Summary: Generate UI (Pro)
- Description: Generate pixel art UI elements from text description.

This endpoint creates pixel art UI elements such as buttons, health bars,
inventory slots, dialogue boxes, and other game interface components.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Text-to-image UI generation
- Optional concept image for design guidance
- Optional color palette specification
- Automatic background removal
- Optimized for game UI assets
- Non-blocking: returns job ID immediately

**Supported Sizes:**
- Minimum 16x16. Maximum depends on aspect ratio (e.g. 512x512 for square, 688x384 for 16:9).

**Example Descriptions:**
- "medieval stone button with gold trim"
- "sci-fi health bar with neon glow"
- "wooden inventory slot with metal corners"
- "pixel art dialogue box with decorative border"

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, images are in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_ui_v2(
    description="medieval stone button",
    image_size=dict(width=256, height=256),
    color_palette="brown and gold"
)

response.images[0].pil_image().save("button.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of the UI element to generate (e.g., 'medieval stone button', 'sci-fi health bar') |
| image_size | app__endpoints__external__v2__generate_ui_v2__ImageSize | no |  | Output image size (16 to aspect-ratio max, e.g. 512x512 square) |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from generated UI element |
| concept_image | ConceptImage / null | no |  | Optional concept image to guide the UI design |
| color_palette | string / null | no |  | Optional color palette specification (e.g., 'brown and gold', 'blue and silver') |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | UI generation job accepted and processing | GenerateUIV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-ui-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"color_palette": "brown and gold", "description": "medieval stone button with gold trim", "image_size": {"height": 256, "width": 256}, "no_background": true, "seed": 42}'
```

#### POST `/generate-with-style-v2`

- Summary: Generate with style (Pro)
- Description: Generate new pixel art images that match the style of reference images.

This endpoint creates new pixel art based on a text description while matching
the visual style (colors, shading, detail level) of provided style reference images.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- Style-consistent generation from reference images
- Multiple style references supported (1-4 images)
- Optional style description for fine-tuning
- Automatic background removal
- Non-blocking: returns job ID immediately

**Output Counts by Size:**
- 16 pixels: Returns 64 images (8x8 grid)
- 17-32 pixels: Returns 64 images (8x8 grid)
- 33-64 pixels: Returns 16 images (4x4 grid)
- 65-128 pixels: Returns 4 images (2x2 grid)
- 129-512 pixels: Returns 1 image

**Supported Sizes:**
- Any square size from 16x16 to 512x512 pixels
- Non-standard sizes are internally padded to the nearest fitting size (16, 32, 64, 128, 256, 512)
- Output images match your requested size

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, images are in `last_response`

**Example:**
```python
response = client.generate_with_style_v2(
    style_images=[{"image": style_img, "width": 64, "height": 64}],
    description="a wizard casting a spell",
    style_description="16-bit RPG style with bright colors",
    image_size={"width": 64, "height": 64}
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| style_images | array[StyleImage] | yes | minItems=1; maxItems=4 | Style reference images (1-4 images) |
| style_images[].image | Base64Image | yes |  | Style image as base64 PNG/JPEG |
| style_images[].width | integer | yes | min=1.0; max=512.0 | Image width in pixels (max 512, matches model) |
| style_images[].height | integer | yes | min=1.0; max=512.0 | Image height in pixels (max 512, matches model) |
| description | string | yes | minLen=1; maxLen=2000 | Description of what to generate |
| image_size | app__endpoints__external__v2__generate_with_style_v2__ImageSize | yes |  | Size of the output images |
| style_description | string / null | no |  | Description of the style to match |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from generated images |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Style generation job accepted and processing | GenerateWithStyleV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-with-style-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "a warrior with a sword", "image_size": {"height": 64, "width": 64}, "no_background": true, "seed": 42, "style_description": "16-bit RPG style", "style_images": [{"height": 64, "image": {"base64": "data:image/png;base64,..."}, "width": 64}]}'
```

### Create map

#### POST `/create-isometric-tile`

- Summary: Create isometric tile (async processing)
- Description: Creates a isometric tile based on the provided parameters.

Supported image size: 
- Minimum area 16x16 and maximum area 64x64
- Sizes above 24x24 often produce better quality results

Supported features:
- Init image  
- Forced palette

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.generate_isometric_tile(
    description="grass on top of dirt",
    image_size=dict(width=32, height=32),
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the image to generate |
| image_size | app__endpoints__external__v2__create_isometric_tile__ImageSize | yes |  |  |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text description |
| outline | string / null | no |  | Outline style for the tile |
| shading | string / null | no |  | Shading complexity |
| detail | string / null | no |  | Level of detail in the tile |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| isometric_tile_size | integer / null | no |  | Size of the isometric tile. Recommended sizes: 16, 32. Can be omitted for default. |
| isometric_tile_shape | string | no | enum=thick tile, thin tile, block | Tile thickness. Thicker tiles allow more height variation in game maps. thin tile: ~15% canvas height, thick tile: ~25% height, block: ~50% height |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Successful Response | CreateIsometricTileBackgroundResponse |
| 200 | Successfully generated image |  |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-isometric-tile' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "grass on top of dirt", "image_size": {"height": 32, "width": 32}}'
```

#### POST `/create-tiles-pro`

- Summary: Create tiles pro (async processing)
- Description: Creates pixel art tiles based on the provided parameters.

Generates multiple tile variations by drawing tile shape outlines and having
AI fill them with pixel art. Supports hexagonal, isometric, and square
top-down tile types.

Supported tile types:
- **hex**: Flat-top hexagonal tiles
- **hex_pointy**: Pointy-top hexagonal tiles
- **isometric**: Diamond/rhombus tiles
- **octagon**: 8-sided polygon tiles
- **square_topdown**: Square tiles at angle

Supported tile sizes: 16-128px (32px recommended)

Generation time: ~15-30 seconds (async processing)

**Prompting tip:** For best control over each tile variation, number each tile in the description:
`"1). grass tile 2). dirt tile 3). stone tile 4). water tile"` - the number of tiles is auto-computed based on tile size.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.create_tiles_pro(
    description="1). grass tile 2). dirt tile 3). stone tile 4). water tile 5). sand tile 6). lava tile",
    tile_type="isometric",
    tile_size=32,
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the tiles. For best control, number each tile variation: '1). grass tile 2). stone tile 3). lava tile'. |
| tile_type | string | no | enum=hex, hex_pointy, isometric, octagon, square_topdown | Shape of the tiles. hex: flat-top hexagonal, hex_pointy: pointy-top hexagonal, isometric: diamond/rhombus, octagon: 8-sided polygon, square_topdown: square at angle. |
| tile_size | integer | no | min=16.0; max=256.0 | Tile size in pixels (16-256). 32px is recommended for most use cases. |
| tile_height | integer / null | no |  | Tile height in pixels for non-square tiles (e.g., 128 for 64x128 tiles). When omitted, height is computed from tile_type geometry and view angle. |
| tile_view | string | no | enum=top-down, high top-down, low top-down, side | View angle controlling tile depth. top-down: no depth, high top-down: ~15%, low top-down: ~30%, side: ~50%. |
| tile_view_angle | number / null | no |  | Continuous view angle in degrees (0-90). Overrides tile_view when provided. 0=side, 90=top-down. |
| tile_depth_ratio | number / null | no |  | Tile depth/thickness ratio (0.0-1.0). Controls how much vertical depth the tile has. Overrides the default computed from tile_view. |
| seed | integer / null | no |  | Seed for reproducible generation |
| style_images | array[TilesProStyleImage] / null | no |  | Style reference tiles. When provided, generated tiles will match these tiles' style and dimensions. The tile_type, tile_size, tile_view, tile_view_angle, and tile_depth_ratio are ignored - the style tiles define the shape. |
| style_options | TilesProStyleOptions / null | no |  | Options for what to copy from the style images. Only used when style_images is provided. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Generation started successfully | CreateTilesProBackgroundResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-tiles-pro' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "1). grass tile 2). dirt tile 3). stone tile 4). water tile 5). sand tile 6). lava tile", "tile_size": 32, "tile_type": "isometric", "tile_view": "low top-down"}'
```

#### POST `/create-tileset`

- Summary: Create top-down tileset (async processing)
- Description: Creates a complete tileset for game development with seamlessly connecting tiles.

A tileset is a collection of individual tiles (16 for standard, 23 for transition_size=1.0) that can be combined
to create larger maps and environments. This endpoint generates tiles representing two terrain levels - "lower"
and "upper" - that connect seamlessly when placed adjacent to each other.

**Understanding Lower and Upper Terrain:**
- **Lower terrain**: The base level terrain (e.g., water, grass, lava)
- **Upper terrain**: The elevated terrain level (e.g., beach, dirt path, rock)
- **Transition size**: Controls the visual height difference between levels. Larger transitions (0.25-0.5) 
  create a more pronounced elevation effect, making it appear as if the upper terrain is on a higher plane

Features:
- Returns individual tiles with unique IDs (16 for standard, 23 for transition_size=1.0)
- Corner-based terrain classification (NW, NE, SW, SE)
- Pre-calculated connection compatibility between tiles
- Lower and upper terrain levels for elevation variety
- Tile sizes: 16x16 or 32x32 pixels
- Seamless tile connections for map creation
- Vertical transitions support with transition_size=1.0
- Style control via outline, shading, and detail parameters
- Reference images for style guidance
- Color palette control

Response format:
- Each tile includes: UUID, name, description, base64 image, corner data, connections
- Corner data specifies "lower", "upper", or "transition" terrain for each corner (NW, NE, SW, SE)
- Connection data lists UUIDs of compatible adjacent tiles in each direction
- Metadata includes terrain prompts and creation timestamp

Common use cases:
- **Beach environments**: water/sand with wet sand transitions
- **Forest paths**: grass/dirt with muddy transitions
- **Dungeon floors**: stone floor/walls with cracked stone transitions
- **Snow landscapes**: snow/rock with icy transitions
- **Desert oases**: sand/water with muddy bank transitions
- **Lava caves**: rock/lava with molten rock transitions

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Generate a beach/ocean tileset
response = client.generate_tileset(
    lower_description="deep blue ocean water with gentle waves",
    upper_description="golden sandy beach",
    transition_description="wet sand with foam",
    tile_size=dict(width=16, height=16),
    transition_size=0.5,
    view="high top-down"
)

# Access individual tiles
for tile in response.tileset.tiles:
    print(f"Tile {tile.name}: {tile.description}")
    print(f"  Corners: NW={tile.corners.NW}, NE={tile.corners.NE}, SW={tile.corners.SW}, SE={tile.corners.SE}")
    print(f"  Can connect to {len(tile.connections)} other tiles")
    
    # Save individual tile images
    import base64
    with open(f"tile_{tile.id[:8]}.png", "wb") as f:
        f.write(base64.b64decode(tile.image.base64))
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| lower_description | string | yes | minLen=1 | Description of the lower/base terrain level (e.g., 'ocean', 'grass', 'lava') |
| upper_description | string | yes | minLen=1 | Description of the upper/elevated terrain level (e.g., 'sand', 'stone', 'snow') |
| transition_description | string | no |  | Optional description of transition area between lower and upper |
| lower_base_tile_id | string / null | no |  | Optional ID to identify the lower base tile in metadata |
| upper_base_tile_id | string / null | no |  | Optional ID to identify the upper base tile in metadata |
| tile_size | TileSize | no |  | Size of individual tiles within the tileset |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text descriptions (default: 8.0) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| view | TilesetCameraView | no | enum=low top-down, high top-down | Camera view angle for tileset |
| tile_strength | number | no | min=0.1; max=2.0 | Strength of tile pattern adherence |
| tileset_adherence_freedom | number | no | min=0.0; max=900.0 | How flexible it will be when following tileset structure, higher values means more flexibility |
| tileset_adherence | number | no | min=0.0; max=500.0 | How much it will follow the reference/texture image and follow tileset structure |
| transition_size | number | no | enum=0.0, 0.25, 0.5, 1.0 | Size of transition area (0 = no transition, 0.25 = quarter tile, 0.5 = half tile, 1.0 = full tile) |
| lower_reference_image | Base64Image / null | no |  | Reference image for lower terrain style |
| upper_reference_image | Base64Image / null | no |  | Reference image for upper terrain style |
| transition_reference_image | Base64Image / null | no |  | Reference image for transition area style |
| color_image | Base64Image / null | no |  | Reference image for color palette |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Successful Response | CreateTilesetBackgroundResponse |
| 200 | Successfully generated tileset |  |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-tileset' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"lower_description": "deep blue ocean water with gentle waves", "tile_size": {"height": 16, "width": 16}, "transition_description": "wet sand with foam", "transition_size": 0.5, "upper_description": "golden sandy beach", "view": "high top-down"}'
```

#### POST `/create-tileset-sidescroller`

- Summary: Create sidescroller tileset (async processing)
- Description: Creates a complete sidescroller tileset for 2D platformer game development.

A sidescroller tileset is a collection of individual tiles designed for side-view platformer games.
The tiles represent floating platforms with transparent backgrounds, allowing them to be placed
over any game background.

**Key differences from top-down tilesets:**
- **View**: Always "side" perspective (fixed, cannot be changed)
- **Background**: Always transparent (no background terrain)
- **Use case**: 2D platformer games, side-scrolling adventures
- **No slopes**: Only flat horizontal platform surfaces

**Understanding the layers:**
- **Lower terrain**: The main platform/ground material (e.g., stone, grass, metal)
- **Transition**: Optional decorative layer on top of the platform (e.g., moss, snow, rust)
- **Background**: Automatically transparent (not configurable)

Features:
- Returns individual tiles with unique IDs
- Transparent background for easy overlay on game scenes
- Tile sizes: 16x16 or 32x32 pixels
- Seamless tile connections for platform creation
- Optional transition layer for surface details
- Style control via outline, shading, and detail parameters
- Reference images for style guidance
- Color palette control

**Retrieving results:**
After submission, use `GET /tilesets/{tileset_id}` to retrieve the completed tileset.
The same endpoint is used for both top-down and sidescroller tilesets.

Common use cases:
- **Stone platforms**: stone bricks with moss transitions
- **Grass ground**: green grass with flower decorations
- **Metal grating**: industrial platforms with rust details
- **Ice platforms**: frozen surfaces with snow cover
- **Wood platforms**: wooden planks with vine overgrowth
- **Candy platforms**: colorful candy blocks with frosting

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Generate a stone platform tileset
response = client.generate_tileset_sidescroller(
    lower_description="stone brick platform with carved details",
    transition_description="moss and small green plants",
    tile_size=dict(width=16, height=16),
    transition_size=0.25,
)

# Retrieve completed tileset using the tileset_id
tileset = client.get_tileset(response.tileset_id)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| lower_description | string | yes | minLen=1 | Description of the main terrain/platform material (e.g., 'stone bricks', 'grass ground', 'metal grating') |
| transition_description | string | no |  | Optional description of decorative layer on top of platform (e.g., 'moss and vines', 'snow cover', 'rust stains') |
| lower_base_tile_id | string / null | no |  | Optional ID to identify the lower base tile in metadata (for connected tilesets) |
| tile_size | SidescrollerTileSize | no |  | Size of individual tiles within the tileset |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text descriptions (default: 8.0) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| tile_strength | number | no | min=0.1; max=2.0 | Strength of tile pattern adherence |
| tileset_adherence_freedom | number | no | min=0.0; max=900.0 | How flexible it will be when following tileset structure, higher values means more flexibility |
| tileset_adherence | number | no | min=0.0; max=500.0 | How much it will follow the reference/texture image and follow tileset structure |
| transition_size | number | no | enum=0.0, 0.25, 0.5, 1.0 | Size of transition area (0 = no transition, 0.25 = quarter tile, 0.5 = half tile, 1.0 = full tile) |
| lower_reference_image | Base64Image / null | no |  | Reference image for platform terrain style |
| transition_reference_image | Base64Image / null | no |  | Reference image for transition area style |
| color_image | Base64Image / null | no |  | Reference image for color palette |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Tileset creation started, returns job ID | CreateTilesetBackgroundResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-tileset-sidescroller' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"lower_description": "stone brick platform with carved details", "tile_size": {"height": 16, "width": 16}, "transition_description": "moss and small green plants", "transition_size": 0.25}'
```

#### GET `/isometric-tiles`

- Summary: List user's isometric tiles
- Description: List all isometric tiles created by the authenticated user.

This endpoint returns a paginated list of all isometric tiles you've created.

**Pagination:**
- Use `limit` to control how many tiles to return (1-100)
- Use `offset` to skip tiles for pagination
- Total count is included in response for pagination UI

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| limit | query | no | integer | Maximum number of tiles to return |
| offset | query | no | integer | Number of tiles to skip |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved isometric tile list | IsometricTilesListResponse |
| 401 | Invalid API token |  |
| 422 | Invalid pagination parameters |  |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/isometric-tiles' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### GET `/isometric-tiles/{tile_id}`

- Summary: Get generated isometric tile by ID
- Description: Retrieve a completed isometric tile by its UUID.

This endpoint returns the isometric tile image after background processing completes.
Use this after the tile generation is finished.

The tile ID is returned immediately when you submit a tile generation request.
Check the background job status first to ensure generation is complete.

Response includes:
- Base64 PNG image with transparent background
- Usage information

Example usage:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Get completed tile
tile = client.get_isometric_tile("f47ac10b-58cc-4372-a567-0e02b2c3d479")

# Save tile image
tile.image.pil_image().save("tile.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| tile_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved tile | CreateIsometricTileResponse |
| 404 | Tile not found |  |
| 401 | Invalid API token |  |
| 423 | Tile still processing |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/isometric-tiles/{tile_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### GET `/tiles-pro/{tile_id}`

- Summary: Get generated tiles pro by ID
- Description: Retrieve completed tiles pro by their UUID.

This endpoint returns the tile images after background processing completes.
Use this after the tile generation is finished.

The tile ID is returned immediately when you submit a tile generation request.

Response includes:
- List of base64 PNG tile variation images
- Usage information

Example usage:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Get completed tiles
tiles = client.get_tiles_pro("f47ac10b-58cc-4372-a567-0e02b2c3d479")

# Save tile images
for tile in tiles.tiles:
    save_base64_image(tile.base64, f"tile_{tile.index}.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| tile_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved tiles | GetTilesProResponse |
| 404 | Tiles not found |  |
| 401 | Invalid API token |  |
| 423 | Tiles still processing |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/tiles-pro/{tile_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### GET `/tilesets`

- Summary: List user's tilesets
- Description: List all tilesets (top-down and sidescroller) created by the authenticated user.

This endpoint returns a paginated list of all tilesets you've created.

**Pagination:**
- Use `limit` to control how many tilesets to return (1-100)
- Use `offset` to skip tilesets for pagination
- Total count is included in response for pagination UI

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| limit | query | no | integer | Maximum number of tilesets to return |
| offset | query | no | integer | Number of tilesets to skip |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved tileset list | TilesetsListResponse |
| 401 | Invalid API token |  |
| 422 | Invalid pagination parameters |  |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/tilesets' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### POST `/tilesets`

- Summary: Create a tileset asynchronously
- Description: Creates a Wang tileset (16 tiles for standard, 23 for transition_size=1.0) in the background and returns immediately with job ID

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| lower_description | string | yes | minLen=1 | Description of the lower/base terrain level (e.g., 'ocean', 'grass', 'lava') |
| upper_description | string | yes | minLen=1 | Description of the upper/elevated terrain level (e.g., 'sand', 'stone', 'snow') |
| transition_description | string | no |  | Optional description of transition area between lower and upper |
| lower_base_tile_id | string / null | no |  | Optional ID to identify the lower base tile in metadata |
| upper_base_tile_id | string / null | no |  | Optional ID to identify the upper base tile in metadata |
| tile_size | TileSize | no |  | Size of individual tiles within the tileset |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text descriptions (default: 8.0) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| view | TilesetCameraView | no | enum=low top-down, high top-down | Camera view angle for tileset |
| tile_strength | number | no | min=0.1; max=2.0 | Strength of tile pattern adherence |
| tileset_adherence_freedom | number | no | min=0.0; max=900.0 | How flexible it will be when following tileset structure, higher values means more flexibility |
| tileset_adherence | number | no | min=0.0; max=500.0 | How much it will follow the reference/texture image and follow tileset structure |
| transition_size | number | no | enum=0.0, 0.25, 0.5, 1.0 | Size of transition area (0 = no transition, 0.25 = quarter tile, 0.5 = half tile, 1.0 = full tile) |
| lower_reference_image | Base64Image / null | no |  | Reference image for lower terrain style |
| upper_reference_image | Base64Image / null | no |  | Reference image for upper terrain style |
| transition_reference_image | Base64Image / null | no |  | Reference image for transition area style |
| color_image | Base64Image / null | no |  | Reference image for color palette |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Tileset creation started, returns job ID | CreateTilesetBackgroundResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/tilesets' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"lower_description": "deep blue ocean water with gentle waves", "tile_size": {"height": 16, "width": 16}, "transition_description": "wet sand with foam", "transition_size": 0.5, "upper_description": "golden sandy beach", "view": "high top-down"}'
```

#### POST `/tilesets-sidescroller`

- Summary: Create a sidescroller tileset asynchronously
- Description: Creates a sidescroller platform tileset in the background and returns immediately with job ID. Retrieve results with GET /tilesets/{tileset_id}.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| lower_description | string | yes | minLen=1 | Description of the main terrain/platform material (e.g., 'stone bricks', 'grass ground', 'metal grating') |
| transition_description | string | no |  | Optional description of decorative layer on top of platform (e.g., 'moss and vines', 'snow cover', 'rust stains') |
| lower_base_tile_id | string / null | no |  | Optional ID to identify the lower base tile in metadata (for connected tilesets) |
| tile_size | SidescrollerTileSize | no |  | Size of individual tiles within the tileset |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the text descriptions (default: 8.0) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| tile_strength | number | no | min=0.1; max=2.0 | Strength of tile pattern adherence |
| tileset_adherence_freedom | number | no | min=0.0; max=900.0 | How flexible it will be when following tileset structure, higher values means more flexibility |
| tileset_adherence | number | no | min=0.0; max=500.0 | How much it will follow the reference/texture image and follow tileset structure |
| transition_size | number | no | enum=0.0, 0.25, 0.5, 1.0 | Size of transition area (0 = no transition, 0.25 = quarter tile, 0.5 = half tile, 1.0 = full tile) |
| lower_reference_image | Base64Image / null | no |  | Reference image for platform terrain style |
| transition_reference_image | Base64Image / null | no |  | Reference image for transition area style |
| color_image | Base64Image / null | no |  | Reference image for color palette |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Tileset creation started, returns job ID | CreateTilesetBackgroundResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/tilesets-sidescroller' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"lower_description": "stone brick platform with carved details", "tile_size": {"height": 16, "width": 16}, "transition_description": "moss and small green plants", "transition_size": 0.25}'
```

#### GET `/tilesets/{tileset_id}`

- Summary: Get generated tileset by ID
- Description: Retrieve a completed tileset by its UUID.

This endpoint returns the complete tileset data including all tiles (16 for standard, 23 for transition_size=1.0)
with their images, corner data, connections, and metadata. Use this after background processing completes.

The tileset ID is returned immediately when you submit a tileset generation request.
Check the background job status first to ensure generation is complete.

Response includes:
- All tiles with base64 PNG images (16 for standard, 23 for transition_size=1.0)
- Corner classifications (NW, NE, SW, SE) for each tile
- Connection compatibility data for seamless placement
- Generation metadata and parameters
- Terrain descriptions and style settings

Example usage:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

# Get completed tileset
tileset = client.get_tileset("f47ac10b-58cc-4372-a567-0e02b2c3d479")

# Access tiles
for tile in tileset.tileset.tiles:
    print(f"Tile {tile.name}: {tile.description}")
    # Save tile image
    with open(f"tile_{tile.id[:8]}.png", "wb") as f:
        f.write(base64.b64decode(tile.image.base64))
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| tileset_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved tileset | CreateTilesetResponse |
| 423 | Tileset is still being generated |  |
| 404 | Tileset not found |  |
| 401 | Invalid API token |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/tilesets/{tileset_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

### Documentation

#### GET `/llms.txt`

- Summary: Get LLM-friendly API documentation
- Description: Returns API documentation formatted for Large Language Models (LLMs).

    This endpoint provides a text-based overview of all v2 API endpoints,
    formatted in a way that's easily parseable by AI assistants like Claude,
    GPT-4, and other LLMs.

    ## Usage

    You can reference this documentation in AI prompts:
    - `@api.pixellab.ai/v2/llms.txt` in Claude
    - Direct URL access for other tools

    ## Format

    The documentation includes:
    - Endpoint paths and methods
    - Required and optional parameters
    - Authentication requirements
    - Response formats
    - Usage examples

    ## Auto-Generation

    This documentation is auto-generated from the OpenAPI specification,
    ensuring it stays in sync with the actual API implementation.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | LLM-friendly API documentation | string |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/llms.txt' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

### Edit

#### POST `/edit-image`

- Summary: Edit image
- Description: Edit an existing pixel art image based on a text description.

Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

### Supported Image Sizes
- **Reference image**: 16x16 to 400x400 pixels (minimum 16x16 area)
- **Target canvas**: 16x16 to 400x400 pixels (minimum 16x16 area)
- **Free tier limit**: Maximum 200x200 pixels for target canvas

### Output
Returns a single edited image matching the target canvas dimensions.

### Usage Pattern
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, edited image is in `last_response`

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image | Base64Image | yes |  | Reference image to edit as base64 PNG/JPEG |
| image_size | app__endpoints__external__v2__edit_image__ImageSize | yes |  | Size of the reference image |
| description | string | yes | minLen=1; maxLen=500 | Text description of the edit to apply |
| width | integer | yes | min=16.0; max=400.0 | Target canvas width in pixels (16-400) |
| height | integer | yes | min=16.0; max=400.0 | Target canvas height in pixels (16-400) |
| seed | integer / null | no |  | Seed for reproducible generation (0 for random) |
| no_background | boolean / null | no |  | Generate with transparent background |
| text_guidance_scale | number / null | no |  | How closely to follow the text description (1.0-10.0) |
| color_image | Base64Image / null | no |  | Color reference image for style guidance |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Edit image job accepted and processing | EditImageResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/edit-image' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "make it more colorful", "height": 128, "image": {"base64": "data:image/png;base64,..."}, "image_size": {"height": 64, "width": 64}, "no_background": true, "seed": 42, "text_guidance_scale": 8.0, "width": 128}'
```

#### POST `/edit-images-v2`

- Summary: Edit images (Pro)
- Description: Edit pixel art images using text or reference image.

This endpoint supports two editing methods:
1. **edit_with_text**: Apply edits based on a text description
2. **edit_with_reference**: Match the style of a reference image

Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Output sizes (image_size):**
- **Range:** 32x32 to 512x512 pixels. Each returned image has the dimensions you set in `image_size`.
- Input images (`edit_images`, `reference_image`) must be at most 256x256 each.

**Key Features:**
- Edit multiple images consistently
- Text-guided or reference-guided editing
- Preserves original structure and poses
- Optional background removal
- Non-blocking: returns job ID immediately

**Frame limits by output size (image_size):**
- 32-64px: Up to 16 frames (4x4 grid), 15 with reference
- 65-80px: Up to 9 frames (3x3 grid), 8 with reference
- 81-128px: Up to 4 frames (2x2 grid), 3 with reference
- 129-512px: 1 frame only (text method only)

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, edited images are in `last_response`

**edit_with_text Example:**
```python
response = client.edit_images_v2(
    method="edit_with_text",
    edit_images=[{"image": img, "width": 64, "height": 64}],
    image_size={"width": 64, "height": 64},
    description="add a wizard hat"
)
```

**edit_with_reference Example:**
```python
response = client.edit_images_v2(
    method="edit_with_reference",
    edit_images=[{"image": img, "width": 64, "height": 64}],
    image_size={"width": 64, "height": 64},
    reference_image={"image": ref_img, "width": 64, "height": 64}
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| method | string | no | enum=edit_with_text, edit_with_reference | Edit method: 'edit_with_text' or 'edit_with_reference' |
| edit_images | array[EditImage] | yes | minItems=1; maxItems=16 | Images to edit (1-16 images depending on size) |
| edit_images[].image | Base64Image | yes |  | Image to edit as base64 PNG/JPEG |
| edit_images[].width | integer | yes | min=1.0; max=256.0 | Image width in pixels |
| edit_images[].height | integer | yes | min=1.0; max=256.0 | Image height in pixels |
| image_size | app__endpoints__external__v2__edit_images_v2__ImageSize | yes |  | Size of output images |
| description | string / null | no |  | Edit description (required for edit_with_text method) |
| reference_image | app__endpoints__external__v2__edit_images_v2__ReferenceImage / null | no |  | Reference image (required for edit_with_reference method) |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from edited images |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Image edit job accepted and processing | EditImagesV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/edit-images-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "add a red hat", "edit_images": [{"height": 64, "image": {"base64": "data:image/png;base64,..."}, "width": 64}], "image_size": {"height": 64, "width": 64}, "method": "edit_with_text", "no_background": false, "seed": 42}'
```

### Image Operations

#### POST `/image-to-pixelart`

- Summary: Convert image to pixel art
- Description: Convert regular images to pixel art style.

Supported image sizes:
- Input: Minimum 16x16, maximum 1280x1280
- Output: Minimum 16x16, maximum 320x320

**Best practices:**
- Recommended output sizes is 1/4 of the input size
- Keep the same aspect ratio as the input image

Using the Python client:
```python
import pixellab
from PIL import Image

client = pixellab.Client(secret="YOUR_API_TOKEN")

source_img = Image.open("photo.png")

result = client.image_to_pixelart(
    image=source_img,
    image_size=dict(width=256, height=256),
    output_size=dict(width=64, height=64),
)
result.image.pil_image().save("pixelart.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image | Base64Image | yes |  | Image to convert to pixel art |
| image_size | app__endpoints__external__v2__image_to_pixelart__ImageSize | yes |  | Size of the input image |
| output_size | OutputSize | yes |  | Desired output size |
| text_guidance_scale | number / null | no |  | How closely to follow pixel art style |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully converted image | ImageToPixelartResponse |
| 400 | Invalid image size constraints |  |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/image-to-pixelart' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"image": {"base64": "iVBORw0KGgoAAAANS..."}, "image_size": {"height": 256, "width": 256}, "output_size": {"height": 64, "width": 64}}'
```

#### POST `/remove-background`

- Summary: Remove background
- Description: Remove the background from a pixel art image, producing a transparent PNG.

Supported image size:
- Maximum area 400x400

**Background Removal Tasks:**
- `remove_simple_background` (default) - Faster, works well for simple/solid backgrounds
- `remove_complex_background` - Slower, better for complex edges and detailed backgrounds

**Optional text hint:** Provide a description of the foreground object to improve accuracy.

Using the Python client:
```python
import pixellab
from PIL import Image

client = pixellab.Client(secret="YOUR_API_TOKEN")

source_img = Image.open("character.png")

result = client.remove_background(
    image=source_img,
    image_size=dict(width=64, height=64),
)
result.image.pil_image().save("character_no_bg.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image | Base64Image | yes |  | The image to remove the background from (PNG or JPEG base64) |
| image_size | app__endpoints__external__v2__remove_background__ImageSize | yes |  | Size of the input image |
| background_removal_task | string | no | enum=remove_simple_background, remove_complex_background | Background removal complexity. 'remove_simple_background' is faster, 'remove_complex_background' handles complex edges better |
| text | string / null | no |  | Optional description of the foreground object to help with removal |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully removed background | RemoveBackgroundResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/remove-background' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"background_removal_task": "remove_simple_background", "image": {"base64": "iVBORw0KGgoAAAANS..."}, "image_size": {"height": 64, "width": 64}}'
```

#### POST `/resize`

- Summary: Resize pixel art image
- Description: Intelligently resize pixel art images while maintaining pixel art aesthetics.

Supported image sizes:
- Minimum area 16x16 and maximum area 200x200 (both source and target)

Supported features:
- Init image
- Forced palette
- Transparent background

**Best practices:**
- For best results, resize iteratively in small steps
- Recommended: At most 50% decrease or 2x increase per resize
- Example: 32x32 -> 64x64 (2x) is good, 32x32 -> 128x128 (4x) should be done in two steps

Using the Python client:
```python
import pixellab
from PIL import Image

client = pixellab.Client(secret="YOUR_API_TOKEN")

source_img = Image.open("character_32x32.png")

result = client.resize(
    description="cute wizard with blue robe",
    reference_image=source_img,
    reference_image_size=dict(width=32, height=32),
    target_size=dict(width=64, height=64),
)
result.image.pil_image().save("character_64x64.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of your character |
| reference_image | Base64Image | yes |  | Image to resize |
| reference_image_size | app__endpoints__external__v2__resize__ImageSize | yes |  | Original size of the reference image |
| target_size | app__endpoints__external__v2__resize__ImageSize | yes |  | Desired output size |
| view | CameraView / null | no |  | Camera view angle |
| direction | Direction / null | no |  | Directional view |
| isometric | boolean / null | no |  | Isometric perspective |
| oblique_projection | boolean / null | no |  | Oblique projection (beta) |
| no_background | boolean / null | no |  | Remove background |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | number / null | no |  | Strength of initial image influence |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully resized image | ResizeResponse |
| 400 | Invalid image size constraints |  |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/resize' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute wizard with blue robe", "reference_image": {"base64": "iVBORw0KGgoAAAANS..."}, "reference_image_size": {"height": 32, "width": 32}, "target_size": {"height": 64, "width": 64}}'
```

### Inpaint

#### POST `/inpaint`

- Summary: Inpaint image
- Description: Creates a pixel art image based on the provided parameters. Called "Inpaint" in the plugin.

Supported image size: 
- Maximum area 200x200

Supported features:
- Inpainting
- Init image
- Forced palette
- Transparent background

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.inpaint(
    description="boy with wings",
    image_size=dict(width=16, height=16),
    inpainting_image=image_of_boy_without_wings,
    mask_image=mask_image,
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes |  | Text description of the image to generate |
| negative_description | string | no |  | Text description of what to avoid in the generated image |
| image_size | app__endpoints__external__v2__inpaint__ImageSize | yes |  |  |
| text_guidance_scale | number | no | min=1.0; max=10.0 | How closely to follow the text description |
| extra_guidance_scale | number | no | min=0.0; max=20.0 | (Deprecated) |
| outline | Outline / null | no |  | Outline style reference |
| shading | Shading / null | no |  | Shading style reference |
| detail | Detail / null | no |  | Detail style reference |
| view | CameraView / null | no |  | Camera view angle |
| direction | Direction / null | no |  | Subject direction |
| isometric | boolean | no |  | Generate in isometric view |
| oblique_projection | boolean | no |  | Generate in oblique projection |
| no_background | boolean | no |  | Generate with transparent background |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| inpainting_image | Base64Image | yes |  | Reference image which is inpainted |
| mask_image | Base64Image | yes |  | Inpainting / mask image. (black and white image, where the white is where the model should inpaint). |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | InpaintResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/inpaint' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute dragon", "image_size": {"height": 128, "width": 128}, "no_background": true}'
```

#### POST `/inpaint-v3`

- Summary: Inpaint image (Pro)
- Description: Inpaint/edit pixel art images using AI.

This endpoint uses AI-powered inpainting for high-quality
results. It allows you to edit specific areas of an image based on a
text description.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Key Features:**
- AI-powered inpainting with text descriptions
- Optional context image for style guidance
- Bounding box support for precise editing
- Background removal option
- Mask-based editing (white = generate, black = preserve)
- Non-blocking: returns job ID immediately

**Image Sizes:**
- Inpainting image: 32x32 to 512x512 pixels
- Context image: up to 1024x1024 pixels (optional)

**Mask Format:**
- White pixels = areas to generate/replace
- Black pixels = areas to preserve

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, inpainted image is in `last_response`

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.inpaint_v3(
    description="add a glowing sword",
    inpainting_image=character_image,
    mask_image=sword_area_mask,
)

response.images[0].pil_image().save("edited.png")
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Description of what to generate in the masked area |
| inpainting_image | InpaintImage | yes |  | Image to inpaint (32x32 to 512x512) |
| mask_image | InpaintImage | yes |  | Mask for the image (same dimensions as inpainting_image). White = generate, Black = preserve |
| context_image | InpaintImage / null | no |  | Context image (deprecated) |
| bounding_box | BoundingBox / null | no |  | Bounding box (deprecated) |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from generated content |
| crop_to_mask | boolean / null | no |  | Whether to crop generated content to mask boundary (ensures clean edges) |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | Inpainting job accepted and processing | InpaintV3Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/inpaint-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "add a glowing sword", "inpainting_image": {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, "mask_image": {"image": {"base64": "..."}, "size": {"height": 64, "width": 64}}, "no_background": false, "seed": 42}'
```

### Map Objects

#### POST `/map-objects`

- Summary: Create map object
- Description: Creates a pixel art object with transparent background for game maps.

Returns immediately with job ID. Processing takes ~15-30 seconds.

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.create_map_object(
    description="wooden treasure chest",
    image_size={"width": 128, "height": 128}
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 | Object description (e.g., 'wooden barrel', 'stone fountain') |
| image_size | app__endpoints__external__v2__create_map_object__ImageSize | no |  | Object dimensions |
| view | string | no | enum=low top-down, high top-down, side | Camera angle |
| outline | string / null | no |  | Outline style |
| shading | string / null | no |  | Shading complexity |
| detail | string / null | no |  | Level of detail |
| text_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the description |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of initial image influence |
| color_image | Base64Image / null | no |  | Image containing colors for forced palette |
| background_image | Base64Image / null | no |  | Background/map image for style matching. Required when using inpainting. |
| inpainting | MaskInpainting / OvalInpainting / RectangleInpainting / null | no |  | Inpainting configuration for style matching. Options: mask (custom), oval (auto-generated), rectangle (auto-generated) |
| seed | integer / null | no |  | Seed for reproducible generation |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Object generation queued | CreateMapObjectResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/map-objects' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "wooden treasure chest", "detail": "medium detail", "image_size": {"height": 128, "width": 128}, "outline": "single color outline", "shading": "medium shading", "view": "high top-down"}'
```

### Object Management

#### GET `/objects`

- Summary: List user's objects
- Description: List all objects created by the authenticated user.

This endpoint returns a paginated list of all objects you've created.

**Features:**
- Pagination support with limit and offset parameters
- Preview URLs for quick object identification
- Complete object metadata

**Authentication:**
Requires a valid API token in the Authorization header.

**Pagination:**
- Use `limit` to control how many objects to return (1-100)
- Use `offset` to skip objects for pagination
- Total count is included in response for pagination UI

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| limit | query | no | integer | Maximum number of objects to return |
| offset | query | no | integer | Number of objects to skip |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved object list | ObjectsListResponse |
| 401 | Invalid API token |  |
| 422 | Invalid pagination parameters |  |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/objects' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### DELETE `/objects/{object_id}`

- Summary: Delete an object and all associated data
- Description: Delete an object and all its rotation images.

This permanently deletes:
- The object record from the database
- All rotation images from storage
- All associated tags

**Authentication:**
Requires a valid API token. You can only delete objects you created.

**Warning:** This action cannot be undone.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| object_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Object deleted successfully | DeleteObjectResponse |
| 401 | Invalid API token |  |
| 403 | Object belongs to another user |  |
| 404 | Object not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X DELETE 'https://api.pixellab.ai/v2/objects/{object_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### GET `/objects/{object_id}`

- Summary: Get object details
- Description: Get detailed information about a specific object.

This endpoint returns complete object information including all rotation image URLs
and metadata.

**Features:**
- Complete object information and settings
- URLs for all rotation images (4 directions)
- Generation parameters used during creation

**Authentication:**
Requires a valid API token. You can only access objects you created.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| object_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully retrieved object details | ObjectDetail |
| 401 | Invalid API token |  |
| 403 | Object belongs to another user |  |
| 404 | Object not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X GET 'https://api.pixellab.ai/v2/objects/{object_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### PATCH `/objects/{object_id}/tags`

- Summary: Update object tags
- Description: Update the tags for a specific object.

This endpoint replaces all tags for an object with the provided list.
Tags are used for filtering and organizing your objects.

**Features:**
- Replace all tags at once (set operation)
- Automatic normalization (trim whitespace)
- Case-insensitive duplicate detection
- Maximum 20 tags per object
- Maximum 50 characters per tag

**Authentication:**
Requires a valid API token. You can only update tags for objects you created.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| object_id | path | yes | string |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| tags | array[string] | yes | maxItems=20 | List of tags to assign to the object |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Tags updated successfully | UpdateObjectTagsResponse |
| 400 | Invalid tag format or validation error |  |
| 401 | Invalid API token |  |
| 403 | Object belongs to another user |  |
| 404 | Object not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X PATCH 'https://api.pixellab.ai/v2/objects/{object_id}/tags' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"tags": ["item", "treasure", "rpg"]}'
```

### Objects

#### POST `/animate-object`

- Summary: Animate an existing object
- Description: Queues an animation generation job. Returns immediately with a background_job_id and animation_id. If wait_for_source is True (default), polls up to 30s for the source object to complete generation before queueing.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| object_id | string | yes |  | ID of a completed object to animate |
| direction | string | yes | enum=south, south-west, west, north-west, north, north-east, east, south-east, unknown | Direction to animate |
| animation_description | string | yes | minLen=1; maxLen=1000 |  |
| frame_count | integer | no | min=4.0; max=16.0 | Even number 4-16 |
| no_background | boolean | no |  |  |
| animation_name | string / null | no |  |  |
| wait_for_source | boolean | no |  | When True, the API polls up to 30s waiting for the source object to reach status='completed'. When False, returns 400 immediately if the source object is not yet completed. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Animation queued | AnimateObjectResponse |
| 400 | Source object not completed |  |
| 401 | Invalid API token |  |
| 402 | Insufficient generations |  |
| 404 | Source object not found |  |
| 429 | Concurrent job limit reached |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-object' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"animation_description": "floating gently", "direction": "south", "frame_count": 8, "object_id": "456e7890-e89b-12d3-a456-426614174001"}'
```

#### POST `/create-object-state`

- Summary: Create a state of an existing object
- Description: Queues a generation job that applies a text edit to an existing object's image(s) and saves the result as a new object grouped with the source via group_id.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| object_id | string | yes |  | ID of the source object |
| edit_description | string | yes | minLen=1; maxLen=1000 |  |
| no_background | boolean | no |  |  |
| seed | integer / null | no |  |  |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | State queued | CreateObjectStateResponse |
| 400 | Source object is not completed |  |
| 401 | Invalid API token |  |
| 402 | Insufficient generations |  |
| 404 | Source object not found |  |
| 429 | Concurrent job limit reached |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-object-state' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"edit_description": "add moss and lichen", "object_id": "456e7890-e89b-12d3-a456-426614174001"}'
```

#### POST `/objects`

- Summary: Create object (1-direction consistent-style or 8-direction)
- Description: Queues an object generation job. Returns immediately with a background_job_id and object_id. Poll GET /v2/objects/{object_id} for status. 1-direction mode uses the consistent-style pipeline. 8-direction mode uses the rotations pipeline. The legacy pipeline lives at POST /v2/map-objects.

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| description | string | yes | minLen=1; maxLen=2000 |  |
| directions | integer | no | enum=1, 8 | Number of directional views. 1 routes to the consistent-style pipeline (useful for static map/decoration objects). 8 routes to the rotations pipeline (useful for items the player rotates around). |
| image_size | ObjectImageSize | no |  | Square or rectangular image size for object generation. |
| view | string | no | enum=low top-down, high top-down, side |  |
| n_frames | integer | no |  | Only used when directions=1. Number of candidate frames to generate. Must be one of {1, 4, 16, 64}; the natural value depends on image_size (42px -> 64, 85px -> 16, 170px -> 4, else 1). n_frames=1 returns a completed single-direction object directly. n_frames>1 returns an object with status='review' for the caller to select frames via POST /v2/objects/{id}/select-frames. |
| style_images | array[StyleReferenceImage] | no |  | Style reference images (consistent-style mode only, max 8). When empty, the pipeline uses default style references for the chosen object_view. |
| style_images[].base64 | string | yes |  | Base64-encoded raw RGBA bytes (rgba_bytes format) |
| style_images[].width | integer | yes | min=16.0; max=512.0 | Image width in pixels |
| style_images[].height | integer | yes | min=16.0; max=512.0 | Image height in pixels |
| object_view | string / null | no |  | Default-style category for consistent-style mode when style_images is empty. |
| item_descriptions | array[string] / null | no |  | Per-frame descriptions for consistent-style multi-frame packs. |
| reference_image | Base64Image / null | no |  | Reference image for 8-direction generation. Used as the south view. |
| no_background | boolean | no |  |  |
| seed | integer / null | no |  |  |
| state_of | string / null | no |  | Object ID this is a state of (groups them). |
| group_id | string / null | no |  | Group ID for related objects. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Object generation queued | CreateObjectResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient generations or credits |  |
| 422 | Validation error |  |
| 429 | Concurrent job limit reached |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/objects' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "pixel art fantasy character, transparent background"}'
```

#### POST `/objects/{object_id}/dismiss-review`

- Summary: Dismiss a review object without saving any frames

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| object_id | path | yes | string |  |

Request body schema:
No request body documented.

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Review dismissed | DismissReviewResponse |
| 400 | Object not in review status |  |
| 401 | Invalid API token |  |
| 404 | Object not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/objects/{object_id}/dismiss-review' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

#### POST `/objects/{object_id}/select-frames`

- Summary: Promote selected frames of a review object to completed objects

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
| object_id | path | yes | string |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| indices | array[integer] | yes |  | Frame indices (0-based) to keep as completed individual objects. |
| common_tag | string / null | no |  | Optional tag applied to every newly-created object. |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Frames promoted | SelectObjectFramesResponse |
| 400 | Object not in review status / invalid indices |  |
| 401 | Invalid API token |  |
| 404 | Object not found |  |
| 422 | Validation Error | HTTPValidationError |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/objects/{object_id}/select-frames' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"common_tag": "forest-pack", "indices": [0, 3, 7]}'
```

### Rotate

#### POST `/generate-8-rotations-v2`

- Summary: Generate 8 rotations (Pro)
- Description: Generate 8 rotational views of a character or object.

This endpoint creates 8 directional views from a reference image, useful for
game sprites that need to face multiple directions.
Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to check status and retrieve results.

**Methods:**
- **rotate_character**: Rotate existing sprite to 8 directions (`reference_image`)
- **create_with_style**: Create new character from description (requires `description`)
- **create_from_concept**: Create rotations from concept art (requires `concept_image`)

**Output:**
Returns 8 images in order: South, South-West, West, North-West, North,
North-East, East, South-East

**View Angles:**
- `low top-down`: ~20 degree angle (most common for RPGs)
- `high top-down`: ~35 degree angle
- `side`: Side-scroller eye level

**Supported Sizes:**
- image_size: 32x32 to 168x168 (matches reference_to_8_rotations). reference_image max 168x168, concept_image max 1024x1024.

**Usage Pattern:**
1. POST to this endpoint (returns `background_job_id`)
2. Poll `GET /v2/background-jobs/{background_job_id}` every 5-10 seconds
3. When `status` is `completed`, rotations are in `last_response`

**rotate_character Example:**
```python
response = client.generate_8_rotations_v2(
    method="rotate_character",
    reference_image={"image": char_img, "width": 64, "height": 64},
    image_size={"width": 64, "height": 64},
    view="low top-down"
)
```

**create_with_style Example:**
```python
response = client.generate_8_rotations_v2(
    method="create_with_style",
    description="a knight in armor",
    image_size={"width": 64, "height": 64}
)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| method | string | no | enum=rotate_character, create_with_style, create_from_concept | Generation method: 'rotate_character' rotates an existing character, 'create_with_style' creates new character matching style, 'create_from_concept' creates from concept art |
| image_size | app__endpoints__external__v2__generate_8_rotations_v2__ImageSize | yes |  | Size of the output images |
| reference_image | app__endpoints__external__v2__generate_8_rotations_v2__ReferenceImage / null | no |  | Image to rotate (rotate_character) or style reference |
| concept_image | app__endpoints__external__v2__generate_8_rotations_v2__ReferenceImage / null | no |  | Concept art image (only for create_from_concept method) |
| description | string / null | no |  | Description of the character/item |
| style_description | string / null | no |  | Description of the visual style |
| view | string | no | enum=low top-down, high top-down, side | Camera perspective angle |
| seed | integer / null | no |  | Seed for reproducible generation |
| no_background | boolean / null | no |  | Remove background from generated images |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 202 | 8 rotations job accepted and processing | Generate8RotationsV2Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-8-rotations-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"image_size": {"height": 64, "width": 64}, "method": "rotate_character", "no_background": true, "reference_image": {"height": 64, "image": {"base64": "data:image/png;base64,..."}, "width": 64}, "seed": 42, "view": "low top-down"}'
```

#### POST `/generate-8-rotations-v3`

- Summary: Generate 8 rotations v3
- Description: Generate 8 directional rotations from a reference frame.

Returns immediately with a background job ID. Poll `GET /v2/background-jobs/{job_id}`
to retrieve results when generation completes.

**How it works:**
1. Submit the reference frame; receive a `background_job_id` immediately.
2. Poll `GET /v2/background-jobs/{background_job_id}` every 2-5 seconds.
3. When `status` is `completed`, the 8 rotation frames are available at `last_response.images`.

**Size Limits:**
- Maximum image dimension: 256x256 pixels

Typical generation time: 30-180 seconds.

Using the Python client:
```python
import pixellab, time

client = pixellab.Client(secret="YOUR_API_TOKEN")

job = client.generate_8_rotations_v3(
    first_frame=reference_image,
)

while True:
    result = client.get_background_job(job.background_job_id)
    if result.status == "completed":
        images = [img.pil_image() for img in result.last_response["images"]]
        break
    if result.status == "failed":
        raise RuntimeError(result.last_response["detail"])
    time.sleep(2)
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| first_frame | Base64Image | yes |  | Reference frame to generate 8 rotations from (PNG/JPEG base64, max 256x256 pixels) |
| no_background | boolean / null | no |  | Remove background from generated frames |
| seed | integer / null | no |  | Seed for reproducible generation (0 for random) |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Background job accepted | Generate8RotationsV3Response |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many concurrent background jobs |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-8-rotations-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"first_frame": {"base64": "data:image/png;base64,..."}, "no_background": true, "seed": 42}'
```

#### POST `/rotate`

- Summary: Rotate character or object
- Description: Rotates a pixel art image based on the provided parameters. Called "Rotate" in the plugin.

Supported image sizes: 
- 16x16
- 32x32
- 64x64
- 128x128    

Supported features:
- Init image
- Forced palette

Using the Python client:
```python
import pixellab

client = pixellab.Client(secret="YOUR_API_TOKEN")

response = client.rotate(
    from_view="side",
    to_view="side",
    from_direction="south",
    to_direction="east",
    image_size=dict(width=16, height=16),
    from_image=image_of_subject_facing_south,
)
response.image.pil_image()
```

Parameters:
| name | in | required | type | description |
| --- | --- | --- | --- | --- |
|  |  |  |  |  |

Request body schema:
| field | type | required | constraints | description |
| --- | --- | --- | --- | --- |
| image_size | app__endpoints__external__v2__rotate__ImageSize | yes |  |  |
| image_guidance_scale | number | no | min=1.0; max=20.0 | How closely to follow the reference image |
| view_change | integer / null | no |  | How many degrees to tilt the subject |
| direction_change | integer / null | no |  | How many degrees to rotate the subject |
| from_view | CameraView / null | no |  | From camera view angle |
| to_view | CameraView / null | no |  | To camera view angle |
| from_direction | Direction / null | no |  | From subject direction |
| to_direction | Direction / null | no |  | From subject direction |
| isometric | boolean | no |  | Generate in isometric view |
| oblique_projection | boolean | no |  | Generate in oblique projection |
| init_image | Base64Image / null | no |  | Initial image to start from |
| init_image_strength | integer | no | min=1.0; max=999.0 | Strength of the initial image influence |
| mask_image | Base64Image / null | no |  | Inpainting / mask image. Requires init image! (black and white image, where the white is where the model should inpaint) |
| from_image | Base64Image | yes |  | Reference image to rotate |
| color_image | Base64Image / null | no |  | Forced color palette, image containing colors used for palette |
| seed | integer / null | no |  | Seed decides the starting noise |

Response schema:
| code | description | schema |
| --- | --- | --- |
| 200 | Successfully generated image | RotateResponse |
| 401 | Invalid API token |  |
| 402 | Insufficient credits |  |
| 422 | Validation error |  |
| 429 | Too many requests |  |
| 529 | Rate limit exceeded |  |

Example:
```bash
curl -X POST 'https://api.pixellab.ai/v2/rotate' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "cute dragon", "from_direction": "south", "from_view": "side", "image_guidance_scale": 7.5, "image_size": {"height": 128, "width": 128}, "to_direction": "east", "to_view": "side"}'
```

## RIMA-relevant endpoints

These are the endpoints most directly useful for RIMA art production, character/object pipelines, and async status polling.

### POST `/animate-with-text-v3`

- Resource group: Animate
- Use: Animate with text v3
- Key fields: first_frame, last_frame, action, frame_count, seed, no_background
- Response codes: 200, 401, 402, 422, 429

### GET `/background-jobs/{job_id}`

- Resource group: Background Jobs
- Use: Get background job status
- Response codes: 200, 401, 404, 429, 422

### DELETE `/characters/{character_id}`

- Resource group: Character Management
- Use: Delete a character and all associated data
- Response codes: 200, 422

### GET `/characters/{character_id}`

- Resource group: Character Management
- Use: Get character details
- Response codes: 200, 401, 403, 404, 429, 422

### POST `/animate-character`

- Resource group: Character from template
- Use: Animate character
- Key fields: character_id, animation_name, description, action_description, async_mode, mode, template_animation_id, frame_count, text_guidance_scale, outline
- Response codes: 200, 401, 402, 404, 422, 429

### POST `/characters/animations`

- Resource group: Character from template
- Use: Create Character Animation
- Key fields: character_id, animation_name, description, action_description, async_mode, mode, template_animation_id, frame_count, text_guidance_scale, outline
- Response codes: 200, 422

### POST `/create-character-pro`

- Resource group: Character from template
- Use: Create character with Pro mode (8 directions)
- Key fields: description, image_size, method, view, template_id, concept_image, reference_image, style_description, seed, no_background
- Response codes: 200, 401, 402, 422, 429

### POST `/create-character-v3`

- Resource group: Character from template
- Use: Create character with v3 model (8 rotations)
- Key fields: description, reference_image, image_size, view, template_id, name, seed, no_background
- Response codes: 200, 401, 402, 422, 429

### POST `/generate-image-v2`

- Resource group: Create Image
- Use: Generate image (Pro)
- Key fields: description, image_size, seed, no_background, reference_images, style_image, style_options
- Response codes: 202, 401, 402, 422, 429

### POST `/generate-with-style-v2`

- Resource group: Create Image
- Use: Generate with style (Pro)
- Key fields: style_images, style_images[].image, style_images[].width, style_images[].height, description, image_size, style_description, seed, no_background
- Response codes: 202, 401, 402, 422, 429

### POST `/map-objects`

- Resource group: Map Objects
- Use: Create map object
- Key fields: description, image_size, view, outline, shading, detail, text_guidance_scale, init_image, init_image_strength, color_image
- Response codes: 200, 401, 402, 422, 429

### GET `/objects`

- Resource group: Object Management
- Use: List user's objects
- Response codes: 200, 401, 422

### DELETE `/objects/{object_id}`

- Resource group: Object Management
- Use: Delete an object and all associated data
- Response codes: 200, 401, 403, 404, 422

### GET `/objects/{object_id}`

- Resource group: Object Management
- Use: Get object details
- Response codes: 200, 401, 403, 404, 422

### POST `/animate-object`

- Resource group: Objects
- Use: Animate an existing object
- Key fields: object_id, direction, animation_description, frame_count, no_background, animation_name, wait_for_source
- Response codes: 200, 400, 401, 402, 404, 429, 422

### POST `/objects`

- Resource group: Objects
- Use: Create object (1-direction consistent-style or 8-direction)
- Key fields: description, directions, image_size, view, n_frames, style_images, style_images[].base64, style_images[].width, style_images[].height, object_view
- Response codes: 200, 401, 402, 422, 429

## Quick examples

Set the token once:

```bash
export PIXELLAB_API_TOKEN="YOUR_API_TOKEN"
```

### Generate image (Pro)

Curl:
```bash
curl -X POST 'https://api.pixellab.ai/v2/generate-image-v2' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "a cute wizard character", "image_size": {"height": 64, "width": 64}, "no_background": true, "seed": 42}'
```

Python:
```python
import os
import requests

url = "https://api.pixellab.ai/v2/generate-image-v2"
headers = {"Authorization": f"Bearer {os.environ['PIXELLAB_API_TOKEN']}"}
payload = {
  "description": "a cute wizard character",
  "image_size": {
    "height": 64,
    "width": 64
  },
  "no_background": true,
  "seed": 42
}
response = requests.request("POST", url, headers=headers, json=payload, timeout=120)
response.raise_for_status()
print(response.json())
```

### Animate with text v3

Curl:
```bash
curl -X POST 'https://api.pixellab.ai/v2/animate-with-text-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"action": "walking forward", "first_frame": {"base64": "data:image/png;base64,..."}, "frame_count": 8, "no_background": true, "seed": 42}'
```

Python:
```python
import os
import requests

url = "https://api.pixellab.ai/v2/animate-with-text-v3"
headers = {"Authorization": f"Bearer {os.environ['PIXELLAB_API_TOKEN']}"}
payload = {
  "action": "walking forward",
  "first_frame": {
    "base64": "data:image/png;base64,..."
  },
  "frame_count": 8,
  "no_background": true,
  "seed": 42
}
response = requests.request("POST", url, headers=headers, json=payload, timeout=120)
response.raise_for_status()
print(response.json())
```

### Create character with v3 model (8 rotations)

Curl:
```bash
curl -X POST 'https://api.pixellab.ai/v2/create-character-v3' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN' -H 'Content-Type: application/json' -d '{"description": "knight in golden armor", "reference_image": {"base64": "data:image/png;base64,..."}, "template_id": "mannequin", "view": "low top-down"}'
```

Python:
```python
import os
import requests

url = "https://api.pixellab.ai/v2/create-character-v3"
headers = {"Authorization": f"Bearer {os.environ['PIXELLAB_API_TOKEN']}"}
payload = {
  "description": "knight in golden armor",
  "reference_image": {
    "base64": "data:image/png;base64,..."
  },
  "template_id": "mannequin",
  "view": "low top-down"
}
response = requests.request("POST", url, headers=headers, json=payload, timeout=120)
response.raise_for_status()
print(response.json())
```

### Delete a character and all associated data

Curl:
```bash
curl -X DELETE 'https://api.pixellab.ai/v2/characters/{character_id}' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

Python:
```python
import os
import requests

url = "https://api.pixellab.ai/v2/characters/{character_id}"
headers = {"Authorization": f"Bearer {os.environ['PIXELLAB_API_TOKEN']}"}
response = requests.request("DELETE", url, headers=headers, timeout=120)
response.raise_for_status()
print(response.json())
```

### List user's objects

Curl:
```bash
curl -X GET 'https://api.pixellab.ai/v2/objects' -H 'Authorization: Bearer $PIXELLAB_API_TOKEN'
```

Python:
```python
import os
import requests

url = "https://api.pixellab.ai/v2/objects"
headers = {"Authorization": f"Bearer {os.environ['PIXELLAB_API_TOKEN']}"}
response = requests.request("GET", url, headers=headers, timeout=120)
response.raise_for_status()
print(response.json())
```
