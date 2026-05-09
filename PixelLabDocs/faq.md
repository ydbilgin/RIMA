Source: https://www.pixellab.ai/docs/faq

[PixelLab](/)[Pricing](/#checkout)[Docs](/docs)[API](/pixellab-api)[Vibe coding](/mcp)[Enterprise](/enterprise)[Sign in](/signin)

[Documentation](/docs)[Community](https://discord.gg/pBeyTBF8T7)[API Documentation](https://api.pixellab.ai/v1/docs)[AI Agent Toolkit](/mcp)

Ask AI...

## Introduction

[Introduction](/docs)[Ways to use PixelLab](/docs/ways-to-use-pixellab)[Installation (Aseprite)](/docs/installation)[Introduction to Pixelorama](/docs/introduction-pixelorama)[FAQ](/docs/faq)

## Guides

[Init images and inpainting](/docs/getting-started)[Creating maps](/docs/guides/map-tiles)[Rotating a character](/docs/guides/rotating-a-character)

## Create image

[Create images from style references (Pro)](/docs/tools/consistent-style)[Create S-L image (Pro)](/docs/tools/create-sl-image-pro)[Create M-XL image (new)](/docs/tools/create-image-flux)[Image to image (depth)](/docs/tools/image-to-image-depth)[Create S-M image](/docs/tools/style)[Create S-M image (old)](/docs/tools/style_old)[Pose to image](/docs/tools/pose-to-image)[Image to pixel art](/docs/tools/image-to-pixel-art)

## Edit image

[Edit image](/docs/tools/edit-image)[Edit image (Pro)](/docs/tools/edit-image-pro)[Remove background](/docs/tools/remove-background)[Resize](/docs/tools/resize)[Unzoom pixel art](/docs/tools/unzoom-pixelart)

## Rotate

[Rotate](/docs/tools/rotate)[Create 8-directional sprite (Pro)](/docs/tools/create-8-rotations-pro)

## Animate

[Animate with text (New)](/docs/tools/animate-with-text-new)[Animate with text (Pro)](/docs/tools/animate-with-text-pro)[Create animated object/character (Pro)](/docs/tools/text2animation)[Animation to animation](/docs/tools/animation-to-animation)[Animate with skeleton](/docs/tools/animate-with-skeleton)[Edit animation (Pro)](/docs/tools/edit-animation-pro)[Transfer outfit (Pro)](/docs/tools/transfer-outfit-pro)[Re-pose](/docs/tools/re-pose)[Animate with text (old)](/docs/tools/animation)[Interpolate (old)](/docs/tools/interpolation)[Create animations (automatic)](/docs/tools/create-animations-automatic)

## Map

[Create map (pixflux)](/docs/tools/create-map)[Extend map (v2)](/docs/tools/extend-map-v2)[Extend map](/docs/tools/extend-map)[Extend map (old)](/docs/tools/extend-map-old)[Create texture](/docs/tools/create-texture)[Create tileset](/docs/tools/create-tileset)[Create isometric tile](/docs/tools/create-isometric-tile)[Create tiles (Pro)](/docs/tools/create-tiles-pro)

## Inpaint

[Inpaint](/docs/tools/inpaint)[Inpaint v3](/docs/tools/inpaint-v3)[Inpaint M-L (pixpatch v2)](/docs/tools/inpaint-pixpatch-v2)

## Reduce colors

[Reduce colors](/docs/tools/reduce-colors)

## Experimental tools

[Create walking character](/docs/tools/create-instant-character)[Try on](/docs/tools/try-on)[Multi image](/docs/tools/multi-image)

## Extra tools

[Create S-M image (style)](/docs/tools/style)[Create S-M image (style, old)](/docs/tools/style_old)[Reshape](/docs/tools/reshape)[Create UI elements](/docs/tools/create-ui-elements)[Create UI elements (Pro)](/docs/tools/create-ui-elements-pro)

## Tool options

[General](/docs/options/general)[Init image](/docs/options/init-image)[Inpainting](/docs/options/inpainting)[Guidance](/docs/options/guidance)[Character](/docs/options/character)[Colors](/docs/options/color)[Camera](/docs/options/camera)[Projection](/docs/options/projection)

docs

Frequently asked questions

# Frequently asked questions

## Free trial

## Where is the code for the free trial?

The code is at the end of the video explaining how the free trial tools work. You can find it [here](https://www.youtube.com/watch?v=Tbmfh4pBPeo).

## Why are some of the tools not available in the free trial?

Since the free trial includes only a limited number of generations, only the
tools that are the easiest to use are included.

## General

## I would like to use the Aseprite extension. How can I get it?

See instructions in the [installation](/docs/installation#get-aseprite) section.

## Does PixelLab use my inputs or generated content for training?

No, we do not use any user inputs or generated content to train our models.

## Does PixelLab work on my OS?

Yes, it works on Windows, Mac, and Linux.

## Do I need a high-performance computer?

No, the models run on cloud GPUs so you don't need a strong graphics card yourself.

## Is there a way to see how many generations I've used in total?

No. We don't want to incentivize using the tools as little as possible. The
generation limits are not enforced unless it becomes a problem.

## Can I use AI assets on Steam?

Yes, you are allowed to use AI assets in your games.

"[...] we are updating the Content Survey that developers fill out when submitting to Steam. The survey now includes a new AI disclosure section, where you'll need to describe how you are using AI in the development and execution of your game. It separates AI usage in games into two broad categories:

> Pre-Generated: Any kind of content (art/code/sound/etc) created with the help of AI tools during development. Under the Steam Distribution Agreement, you promise Valve that your game will not include illegal or infringing content, and that your game will be consistent with your marketing materials. In our pre-release review, we will evaluate the output of AI-generated content in your game the same way we evaluate all non-AI content - including a check that your game meets those promises.

> Live-Generated: Any kind of content created with the help of AI tools while the game is running. In addition to following the same rules as Pre-Generated AI content, this comes with an additional requirement: in the Content Survey, you'll need to tell us what kind of guardrails you're putting on your AI to ensure it's not generating illegal content."

[Link to full article](https://steamcommunity.com/groups/steamworks/announcements/detail/3862463747997849619)

## Can I use the generated images commercially?

Yes, we only ask that you do not train new models with the images.

## Why do you have a subscription model?

A lot of people have asked for a one-time payment instead and, while this is something we are open to in the future, we think
a subscription is the most consumer-friendly method for PixelLab given that:

1. It doesn't make sense to pay a large cost upfront for a technology that may be deprecated in a few months.
2. We are paying for cloud GPUs each month so we would need to make everything run locally for this to make sense. We would need to spend a lot of time making everything work on each customer's computer
   instead of spending time on making better tools. Several of the tools require good GPUs and some customers would not be able to use them or we would need to spend even more time trying to optimize memory and performance.
3. Paying monthly is low risk for the customer, you can stop using the product any time you want if you don't like it.
4. A monthly payment is a good incentive for us to keep making better and better tools for you.

That being said, we know a lot of people are allergic to subscriptions and running locally may still happen at some point when we have the bandwidth available to make it happen.

## Can I upgrade during the month?

Yes, you will only pay for the remaining days.

## The plugin never gets past "Connecting..." when I generate?

This seems to happen for some users and can be because the websocket requests get blocked or the connection is lossy. To debug what the issue might be you can:

* Try another connection (e.g. via your phone)
* Use a VPN (there are some free alternatives you can use just to test)
* Test with another computer

Sadly we cannot do much to improve handling of lossy connections because Aseprite has very limited networking functionality.

## How can I turn my photo into pixel art?

You can downsize your photo and then use it as an init image with, for
example, "Create image (style, old)" or "Generate large image".

## The tools

## Can I find the settings of my previous generations?

If you're using the Aseprite extension, you can find the settings and output
of your previous generations. Toggle "advanced options" to show the "Load
previous settings" option.

## On this page

[Free trial](#free-trial)[General](#general)[The tools](#the-tools)