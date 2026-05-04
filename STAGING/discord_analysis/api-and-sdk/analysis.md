# #api-and-sdk Channel Index

### [2026-04-28] API Endpoint Missing Rotation URLs
- Participants: Proxysetting, Tommy
- Type: text
- Summary: Proxysetting reports that the `v2/objects/{id}` endpoint is not returning `rotation_urls` and `preview_url` for completed base64 object generations (specifically a wolf mask, boat, dice). Discussing if it's an API regression or missing metadata in the new sheet feature.
- Flag for User: [None]
- File: screenshots/capture_001_20260502_191507.png

### [2026-04-24] Rotate Character 8-Directions & New Models
- Participants: clunas, Kaninen, Yazanowa
- Type: text + image
- Summary: Users discussing the `rotate_character` endpoint and whether it requires an upstream call to return the reference image. Kaninen notes they are releasing new models/tools at the moment.
- Flag for User: [None]
- File: screenshots/capture_005_20260502_191522.png

### [2026-04-08] generate-ui-v2 Failures & Animate with text (New) Docs
- Participants: ruge, lmakero, jddjdjd6180, Maven
- Type: text
- Summary: Users reported `generate-ui-v2` endpoint returning immediate "failed" status. Dev (lmakero) pushed a fix. Maven asks for API documentation/endpoint structures for the "Animate with text (New)" feature.
- Flag for User: [None]
- File: screenshots/capture_010_20260502_191538.png

### [2026-03-24 to 2026-03-26] Animation Stalling & 8-Direction Proportion Limits
- Participants: jojowiga, Kaninen, Neo, lmakero
- Type: text
- Summary: jojowiga reports high failure rate on `/animate-with-text-v2` stalling at 50%. Kaninen suggests using v3. Neo reports a 422 error on `POST /v2/create-character-with-8-directions` when using custom proportions. lmakero confirms custom proportions must be strictly in the range of 0.5 to 2.0.
- Flag for User: [None]
- File: screenshots/capture_015_20260502_191559.png

### [2026-03-18] MCP vs v2 API & Zed Configuration
- Participants: Tommy, lmakero
- Type: text + image
- Summary: Discussing the differences between MCP and v2 API. Mentioning `v2/docs` and `v2/llms.txt`. Includes a screenshot of someone configuring the PixelLab MCP Server in Zed (`https://api.pixellab.ai/mcp`).
- Flag for User: [None]
- File: screenshots/capture_020_20260502_191615.png
