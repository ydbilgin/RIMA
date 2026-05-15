# chongdashu full transcript

Source: https://x.com/chongdashu/status/2055287907011768686
Transcription: local Whisper base model, generated from downloaded full video audio.

```srt
1
00:00:00,000 --> 00:00:07,000
I'm going to show you what we're going to build today so that you get a flavor of what you're going to be able to get to at the end of this tutorial.

2
00:00:07,000 --> 00:00:08,000
Here we go.

3
00:00:08,000 --> 00:00:30,000
By the end of this tutorial you will have what I call an endless survival beat-a-map game.

4
00:00:30,000 --> 00:00:41,000
You play as this pirate and then you will have continuous waves of skeleton enemies that will come and you'll get increasingly more difficult as more and more of them turn up over time.

5
00:00:41,000 --> 00:00:52,000
And while this game may seem quite simple, the key thing here is to enable you to understand the fundamentals such as loading in game art, how to implement collision detection, how to implement movement.

6
00:00:52,000 --> 00:01:03,000
Everything that you learn today will be applicable regardless of what games you're trying to build, whether it's a top-down game, whether it's an isometric game, whether it's an arcade game, because all of the concepts that you learn today will be applicable.

7
00:01:03,000 --> 00:01:12,000
And you will be building this game from scratch, from this starting folder or files, and I will take you from by from all the way from start to finish.

8
00:01:12,000 --> 00:01:16,000
And the goal here is to help you build the muscle memory to be able to build games such as this.

9
00:01:16,000 --> 00:01:20,000
So once you've got your project onto your computer, it's time to get going.

10
00:01:20,000 --> 00:01:25,000
I'm just going to walk you through what's inside the folder so that you know where we're starting off from.

11
00:01:25,000 --> 00:01:34,000
A lot of these files that you see here are just called boilerplate. It's just the basic things that are required for the game to get up and running so that you can play it on your computer in your browser.

12
00:01:34,000 --> 00:01:41,000
There are several things that I would like to call your attention to, and let's just start off with what's inside this public folder.

13
00:01:41,000 --> 00:01:47,000
Now, the public folder is where we will store all of our game assets. And you will see that there are already two folders here.

14
00:01:47,000 --> 00:01:54,000
One is called the pirate, and one is called skeleton. And within it, it already contains all of the assets that you'll be using within your game.

15
00:01:54,000 --> 00:02:01,000
In the animations folder, you have the attack, death, hurt, idle, jump, and walk. And within these are what you call sprite sheets.

16
00:02:01,000 --> 00:02:08,000
A sprite sheet is a combination of all the individual frames such that your character can animate within the game.

17
00:02:08,000 --> 00:02:13,000
So if you look at this death sprite sheet, if you play this in sequence, that you will get an animation.

18
00:02:13,000 --> 00:02:19,000
You can just look at the preview.gif file over here. When it's stitched together, you get a complete animation.

19
00:02:19,000 --> 00:02:24,000
The folders that you have here has all the sprite sheets for the skeleton as well as the pirate character.

20
00:02:24,000 --> 00:02:29,000
So you really have everything that you need to go here and build this game that you see over here.

21
00:02:29,000 --> 00:02:34,000
So the other thing to call attention to is that you will see that you have a dot agents and a dot clock folder.

22
00:02:34,000 --> 00:02:41,000
These two folders contain things called skills. And you will see that the skills that are present already are the phaser and phaser for game death skills.

23
00:02:41,000 --> 00:02:46,000
So in today's tutorial, we'll be building this 2D game using the phaser game engine.

24
00:02:46,000 --> 00:02:56,000
And that's the reason why we have loaded in these phaser skills. It's basically an instruction set as well as a knowledge base that gives your AI superpowers in a related domain.

25
00:02:56,000 --> 00:03:00,000
So in this case, it's going to be an expert in building games using the phaser game engine.

26
00:03:00,000 --> 00:03:09,000
Because the skills are provided for both clock code and any other AI agents, you can use cursor, clock code, open code, Zen code or the codex app.

27
00:03:09,000 --> 00:03:16,000
I highly recommend the codex app because codex app does provide a lot of additional functionality as well as your chat GPD subscription.

28
00:03:16,000 --> 00:03:21,000
For today's tutorial, I'll be using the codex app from open AI in the codex app.

29
00:03:21,000 --> 00:03:27,000
You're going to go to file. I save this as VGD pirate survival beat them up. So I'm going to open this right over here.

30
00:03:27,000 --> 00:03:34,000
And once you've loaded it up, you will see that this is the folder that we're going to be in. You want to be working locally and you want to be in the start branch.

31
00:03:34,000 --> 00:03:41,000
Okay. All right. And in this case, if you know what you're doing, you can go to full access, which means that codex will not prompt you for any permissions.

32
00:03:41,000 --> 00:03:50,000
And it should be fairly safe when you're doing this tutorial. But if you want to be more cautious, you can do author review, which means that codex will only ask you when he thinks that it's doing something more dangerous.

33
00:03:51,000 --> 00:04:02,000
At the time of this tutorial, I would suggest you use 5.5 and you can switch between medium or high. If you have a higher tier of the codex or chat GPD subscription, like pro, you can go to fast, which will make things a lot quicker.

34
00:04:02,000 --> 00:04:08,000
If you have a lower subscription, and you can just use medium, but if you are on a higher tier subscription, you can go ahead using high.

35
00:04:08,000 --> 00:04:15,000
So as you can see, in this case, I'll be using high and fast, feel free to stay on the standard speed in order to proceed.

36
00:04:15,000 --> 00:04:20,000
So the first thing that we're going to do is run this project. This will look like a magic command if you've never used it before.

37
00:04:20,000 --> 00:04:28,000
But let's just go here and do it. If you go over here and click toggle terminal, or you can do command J, this will bring up this little console below over here.

38
00:04:28,000 --> 00:04:35,000
Don't worry about it. All you want to do is you want to do NPM install. Okay. This will just install all the dependencies to run your game.

39
00:04:35,000 --> 00:04:42,000
The next thing you want to do is to run NPM run and death. Okay. And you will see something like this pop up. You will say it's ready.

40
00:04:42,000 --> 00:04:47,000
And then you have something called local and network. What you want to do now is just open this up in your browser.

41
00:04:47,000 --> 00:04:56,000
You can hold on the command button or the control of your windows and then click on this button. And this will now open up the game within this window that you see over here.

42
00:04:56,000 --> 00:05:01,000
Now the problem with this is that while it's great that it shows up over here, it will appear quite small. Okay.

43
00:05:01,000 --> 00:05:06,000
Unless you click on this play button over here. And then now once you're within it, you can do up and down.

44
00:05:06,000 --> 00:05:12,000
You can go to settings, you can mute, and then you can go back to the main menu. And then the other thing that you can do is that you can go through sandbox.

45
00:05:12,000 --> 00:05:20,000
And you will see that this will be what we start with. It's just a very simple square that's walking around that allows you to use the arrow keys to walk around on the screen.

46
00:05:20,000 --> 00:05:31,000
By the end of today's tutorial, you will get from this very simple looking sandbox, which is just a starting point to having this fully flesh complete game that you see over here and that we were discussing at the beginning of the video.

47
00:05:31,000 --> 00:05:44,000
Now one thing that I encourage you to do is because this screen is embedded within your window. And if you're working on a laptop and a small screen like me, what you can do is that instead of just clicking on this here, you can just copy this location.

48
00:05:45,000 --> 00:05:54,000
And then just copy and paste this location onto your browser with you have a much larger screen version of what you see over here, rather than having this collapse within this window over here.

49
00:05:54,000 --> 00:06:04,000
And the reason why you want to do this is that when you're in this screen debug mode, you have this console on the right, which will help you to perform troubleshooting steps in the event that you hit any issues.

50
00:06:04,000 --> 00:06:13,000
So for example, as I'm moving around with this character that you see over here, you will see that the input on the right as well as the location of the pointer is updating in real time.

51
00:06:13,000 --> 00:06:21,000
This is part of today's tutorial. You will see that having debug visualization such as this will help you to troubleshoot if you encounter any errors along the way.

52
00:06:21,000 --> 00:06:33,000
Once you are able to get the stage, you are all set because the game is running your project is loaded up in codex and you have all the files that we discuss over here. So you will be ready to go ahead and begin implementing this game.

53
00:06:33,000 --> 00:06:45,000
So let's get started. So we're going to be in the codex app and you want to make sure that you open up your project folder. In this case, it's VGD Pirate Survival beat them up game beat them up double check that this is the case.

54
00:06:45,000 --> 00:06:55,000
And then you want to be working locally if you clone this one GitHub, you should be on the start branch. Don't worry too much about this because we will not be doing any version control as part of today's tutorial.

55
00:06:55,000 --> 00:07:05,000
If you know about Git, you will know that this is the branch that is the beginning of the project. The first thing that we're going to do is if you recall, we have this nice background art that you see over here.

56
00:07:05,000 --> 00:07:13,000
And I thought that the first thing that we'll like to do is to show you how I actually got hold of this background. And this is the problem that we're going to use today. So I'm going to look through this.

57
00:07:13,000 --> 00:07:20,000
I'm going to take you through this step by step. All right. So the first thing is going to do is that I'm going to ask it to look at all the images that we already have.

58
00:07:20,000 --> 00:07:28,000
And if you recall, we have two folders of animation, sprite sheets, including ones for the pirate across all of the different states that we have.

59
00:07:28,000 --> 00:07:37,000
And then we also have one more for the skeleton character. So what we're saying is that study these assets because this is the style of the game that I'm trying to create.

60
00:07:37,000 --> 00:07:42,000
I want it to create a single level with no scrolling using these two characters.

61
00:07:42,000 --> 00:07:48,000
And I wanted to generate four candidates to make sure that there is sufficient space to walk around both horizontally and vertically.

62
00:07:48,000 --> 00:07:53,000
And there needs to be a limit to the vertical range because this is going to be a backdrop.

63
00:07:53,000 --> 00:07:58,000
Now, one thing I'm going to do here is that you can see that I'm saying I want to generate these for the candidates using image.

64
00:07:58,000 --> 00:08:05,000
And if I click over here, I'm going to use the image generation scale that's built into codex and that piggybacks off your chat GPD subscription.

65
00:08:05,000 --> 00:08:10,000
So this allows you to generate images for your game without needing any additional subscription.

66
00:08:10,000 --> 00:08:15,000
Finally, I'm going to say place these into public assets backgrounds and use parallel sub agents.

67
00:08:15,000 --> 00:08:21,000
All right, so now I'm going to kick this off and I'm going to explain to you what I'm trying to do in the original tutorial related to this project.

68
00:08:21,000 --> 00:08:28,000
I created four different backgrounds before I landed up on the final version that you see over here, which is like on the ship.

69
00:08:28,000 --> 00:08:35,000
I asked the AI to generate four different backgrounds and I got a pirate call for one that's on the docs, one that's on the ship and one that was in the ruins.

70
00:08:35,000 --> 00:08:42,000
The reason for this is that you want to have some room to experiment and to see what suits the style that you want.

71
00:08:42,000 --> 00:08:47,000
If you look over here, codex will look at the two images that you have within your project.

72
00:08:47,000 --> 00:08:55,000
Okay, and let's give it a look at these images. It's going to say, okay, I now understand what the style of game that you're going for, which is really useful.

73
00:08:55,000 --> 00:09:02,000
Okay, if you remember, I said use parallel sub agents, you have seen that these four background agents have spawned.

74
00:09:02,000 --> 00:09:07,000
What's happening here is that you want four different backgrounds rather than doing it sequentially.

75
00:09:07,000 --> 00:09:13,000
You're telling codex just spawn it in parallel so that you get all four backgrounds around the same time.

76
00:09:13,000 --> 00:09:18,000
This is a very powerful tool that you should be making use of when you're using something like codex.

77
00:09:18,000 --> 00:09:24,000
All it takes is just a single line like this and it automatically will spawn all the four parallel threats for you.

78
00:09:24,000 --> 00:09:28,000
As you can see here, these will now start to land and appear within the artifacts.

79
00:09:28,000 --> 00:09:38,000
This is the reason why I encourage people to use the codex app as part of this tutorial because not only does it work on your chat GPD subscription, but you probably already have.

80
00:09:38,000 --> 00:09:44,000
But the overall user interface has gone through a lot of changes that made white coding a lot easier.

81
00:09:44,000 --> 00:09:51,000
We are learning with these four candidates and so codex should be wrapping up pretty soon because we are using the inbuilt image generation.

82
00:09:51,000 --> 00:09:54,000
Codex will actually start this images in a different part of your computer.

83
00:09:54,000 --> 00:10:00,000
So what it's going to do is do this final step that we said over here, which is put it into public assets backgrounds.

84
00:10:00,000 --> 00:10:05,000
It's really created a folder and it's going to put the four backgrounds into this location.

85
00:10:05,000 --> 00:10:07,000
And yeah, just as I said it, it's landed here.

86
00:10:07,000 --> 00:10:16,000
The reason why you want it to be put over here is because our game needs to be able to be self-contained and have access to all of these backgrounds so that we can load it in.

87
00:10:16,000 --> 00:10:18,000
And here he is and it's completely done.

88
00:10:18,000 --> 00:10:22,000
It took about seven minutes to get all of these images and they're all great candidates for us to put into the game.

89
00:10:22,000 --> 00:10:30,000
So now that we have these five images, the next step is teach our game and the AI to load in the backgrounds as well as the two different characters.

90
00:10:30,000 --> 00:10:36,000
So the way that we go about teaching our AI as well as our game to know about these new assets is that we're going to do the following.

91
00:10:36,000 --> 00:10:46,000
Say look at the assets that exists and then assets index dot json that will serve as a canonical index to all available sprites in the game.

92
00:10:46,000 --> 00:10:51,000
This means taking into account single sprite like the backgrounds as well as the spreadsheets used for the animations.

93
00:10:51,000 --> 00:10:59,000
Specifically for the spreadsheets, we need frame size, number of frames, offsets and any other metadata useful for integration into a phase again.

94
00:10:59,000 --> 00:11:02,000
So I'm going to kick this off and explain what we're trying to do.

95
00:11:03,000 --> 00:11:10,000
But effectively, even though we have all of our assets with the different backgrounds that we have in the game, our kind of game still doesn't know about them right right now.

96
00:11:10,000 --> 00:11:17,000
All it has is this blue square moving around in a black screen and it needs to load in these assets.

97
00:11:17,000 --> 00:11:30,000
What we're trying to do here is create something called an index or an asset index and the goal here is to create a single file that will point to all of the different assets that are available for us to integrate into this game.

98
00:11:30,000 --> 00:11:42,000
And the reason why you want to have a single file is so that any time that the AI needs to look up a particular spreadsheet or character or background, it doesn't need to list all the files within the folder anymore.

99
00:11:42,000 --> 00:11:47,000
All you need to do is just check this single file and it will be able to find it immediately.

100
00:11:47,000 --> 00:11:50,000
This will help save on your usage and your tokens.

101
00:11:50,000 --> 00:12:02,000
One reason for having a single file serving as an index is that if you're adding in new animations or backgrounds or sprites, you can tell your AI to update the index file and the game will know where and when to pick things up.

102
00:12:02,000 --> 00:12:13,000
And this is good practice. If you watch any of my other videos, you will know that this is something that I do regardless of any game that I'm building ready to say to the game, whether it's a 3D game, I always have an index Jason file.

103
00:12:13,000 --> 00:12:19,000
And so just jumping back here, he says he has created the index.json, click it over here and you will be able to see the result.

104
00:12:19,000 --> 00:12:26,000
Now let me just walk you through what's been hit over here. So if you look, the index Jason now has a couple of really important things to note.

105
00:12:26,000 --> 00:12:36,000
So it tells you when it was generated, where the assets are stored. And most importantly, now he has this that it has four backgrounds and a lot of information related to what these backgrounds are.

106
00:12:36,000 --> 00:12:41,000
And then the other thing that it has is that he has every single animation that you see over here.

107
00:12:41,000 --> 00:12:46,000
And the best way to show is to you is to actually take up one such as the hurt animation.

108
00:12:46,000 --> 00:12:50,000
Okay, I'm just going to pop this to the side and then keep the index Jason on the left.

109
00:12:50,000 --> 00:12:58,000
So now if we look for hurts, you will see here is telling you that this entire sheet is 1280 by 512. There are five columns and two rows.

110
00:12:58,000 --> 00:13:07,000
There are six frames or one, two, three, four, five, six. And then it has information on the frame rate that you want to run as well as the different endpoints for each of these.

111
00:13:07,000 --> 00:13:13,000
The endpoints are really important for it to be able to play animations such that they're not all jumping around.

112
00:13:13,000 --> 00:13:19,000
You don't really have to go into too much detail about what this is. This is really meant for the AI to be able to understand what assets you have.

113
00:13:19,000 --> 00:13:27,000
Now that we've got all of our assets and we've got this index.json, it is time to move on to the fun part where we actually start to integrate all these assets into a playable game.

114
00:13:28,000 --> 00:13:38,000
So I'm going to use this crumb that you see over here and there are two skills that we're going to make use of included in this project for the now phaser game death is because we're using the phaser game engine.

115
00:13:38,000 --> 00:13:48,000
So this contains a lot of information and knowledge about how to utilize it and phaser for is because it's the latest version of phaser and there's a couple of new things that we want to make use of so that the game runs really well.

116
00:13:48,000 --> 00:13:56,000
And so you can see here we're going to integrate the pirate into game. We will support WSD and arrow keys for movement. We will have Z for attack and it should animate correctly.

117
00:13:56,000 --> 00:14:04,000
And over here what we're going to do is that we're going to say we want to use background to as our chosen background.

118
00:14:04,000 --> 00:14:12,000
And we're just going to kick this off and you can see for my prompt. It doesn't take a lot at this point because it has already enough information about what it needs to do.

119
00:14:12,000 --> 00:14:19,000
Going to be able to just take this very simple prompt and give you something that resembles a working game very quickly.

120
00:14:19,000 --> 00:14:29,000
All right, so you can see here is going to use both the phaser skills right phaser three is driving the implementation and phaser fall will be used to standardize it and make use of some of the newer features.

121
00:14:29,000 --> 00:14:38,000
If you want to look at different skills that you have, you can go into plugins and then onto skills. You will see that codex comes with several inbuilt skills like the image chat, which is the one that we were using.

122
00:14:38,000 --> 00:14:44,000
And because I've included additional skills as part of this project, you have the phaser game there as well as the phaser for one.

123
00:14:44,000 --> 00:14:51,000
All right, well, let's jump back into our game here and you can see that it's looking at what we have in the game already.

124
00:14:51,000 --> 00:14:58,000
And if you recall, the game currently has just a single screen with the blue character moving around. This is called the sandbox scene.

125
00:14:58,000 --> 00:15:04,000
So the AI is not thinking, what should we do now? Should we create a new menu screen? Or should we just replace what's already there?

126
00:15:04,000 --> 00:15:10,000
And it's decided that it's just going to go ahead and replace what the ready that if you wanted it to create a new menu item.

127
00:15:10,000 --> 00:15:17,000
Because of the boilerplate that we already have, you can simply say, I want a new screen and it will do that and retain this sandbox over here.

128
00:15:17,000 --> 00:15:24,000
And then you have it. It's really started to integrate the character into the game. And let's just play around with this WSD works.

129
00:15:24,000 --> 00:15:31,000
All right, so I can move up, left down and I can use the arrow keys as well. If I present, it does the attack. And this is looking great.

130
00:15:31,000 --> 00:15:38,000
It's like already you have something that resembles again. It's done a really good job of making sure that it doesn't step into the water.

131
00:15:38,000 --> 00:15:43,000
But you might notice that if it walks too far to the left, it might end up walking on some of the props over here.

132
00:15:43,000 --> 00:15:49,000
And I'll just show you the reason why you can see that there's this wall bounds that constraints where the character can walk.

133
00:15:49,000 --> 00:15:57,000
And you can see over here, it's correct. But if we walk further to the left, you will notice that it's a little bit too far and we should be making this wall bounds and a bit shorter.

134
00:15:57,000 --> 00:16:04,000
But more on that later, now if you're working products, you will probably notice that sometimes they will launch a separate browser for you.

135
00:16:04,000 --> 00:16:10,000
And this is one of the cool things about using the codex app is that it has something called browser use built in.

136
00:16:10,000 --> 00:16:19,000
As you can see here, it's going to resize the browser and then it's going to look at the files and then it's going to try to take several snapshots of the game as it loads up the browser.

137
00:16:19,000 --> 00:16:26,000
The idea here is that it's trying to do an automated verification about whether it's changes may sense.

138
00:16:26,000 --> 00:16:31,000
In some cases, you will need to say allow over here so that it's allowed to press keys for you.

139
00:16:31,000 --> 00:16:36,000
And you can see automatically pressed the sandbox button so that it could load up the game.

140
00:16:36,000 --> 00:16:41,000
And this is a really powerful tool and you can see it's looking at what's on the page right now.

141
00:16:41,000 --> 00:16:44,000
I'm going to shift this side by side so you can see what's happening.

142
00:16:44,000 --> 00:16:51,000
Effectively, what is done is that it took a snapshot and then it pressed a browser key and then now it's trying to take another screenshot again.

143
00:16:51,000 --> 00:16:55,000
Okay, so what I'm going to do is that I'm going to steer this a little bit.

144
00:16:55,000 --> 00:17:03,000
I'm going to say do not clear or delete any playwright screen so that I can keep track of our progress.

145
00:17:03,000 --> 00:17:07,000
Okay, and I'm just going to press enter here and you can see this called steer.

146
00:17:07,000 --> 00:17:09,000
Okay, and this is a nice thing about code access.

147
00:17:09,000 --> 00:17:15,000
Well, you can either queue this message to happen at the end or you can actually start to steer it.

148
00:17:15,000 --> 00:17:20,000
And as you can see here, I managed to steer it so that it doesn't delete the screenshots because I want to keep track of this as we go on through this tutorial.

149
00:17:20,000 --> 00:17:25,000
I'll leave every playwright screenshot in place because I managed to steer it and so it didn't lose context where I needed to be.

150
00:17:25,000 --> 00:17:29,000
And what I'm going to do now is allow for this chat so that it doesn't keep prompting me.

151
00:17:29,000 --> 00:17:35,000
Another thing that you can do is set this to auto review so that you will not keep prompting you whenever any approval for something like this.

152
00:17:35,000 --> 00:17:38,000
And only when it looks like it's doing something dangerous.

153
00:17:38,000 --> 00:17:44,000
If you jump into the game and allow it to continue its testing over here, you will see that everything is working as we expected it to do.

154
00:17:44,000 --> 00:17:48,000
So we are able to walk around, we're able to attack and this is really starting to take shape.

155
00:17:48,000 --> 00:17:55,000
So what we're going to do next is that we're going to start to make this game a lot more interesting by integrating the enemy and have a bit of challenge into the game.

156
00:17:55,000 --> 00:18:00,000
Unfortunately, I recorded this bit and I accidentally killed the screen recording.

157
00:18:00,000 --> 00:18:04,000
So I lost the raw footage, but I'm just going to give an overview of what I did.

158
00:18:04,000 --> 00:18:10,000
The next thing that we want to do is to make this game more interesting by adding the skeleton character into the game as an enemy.

159
00:18:10,000 --> 00:18:13,000
Now, let's add a skeleton enemy using the skeleton sprite.

160
00:18:13,000 --> 00:18:17,000
The enemy is slower than our character, but can attack our character.

161
00:18:17,000 --> 00:18:20,000
When it attacks, the hurt animation is to play.

162
00:18:20,000 --> 00:18:27,000
There is a knockback of the player in the direction of the hit and the enemy needs to pause a while before continuing to chase the player.

163
00:18:27,000 --> 00:18:29,000
And let me explain to you what is going on here.

164
00:18:29,000 --> 00:18:37,000
So if you see this final version of the game, when the enemy attacks, you will see that the player gets hit backwards away from the enemy.

165
00:18:37,000 --> 00:18:40,000
And the enemy also doesn't give chase straight away. There's a bit of a pause.

166
00:18:40,000 --> 00:18:44,000
Now, what I'm trying to demonstrate to you here is that while bike coding can get you very far,

167
00:18:44,000 --> 00:18:50,000
sometimes you'll find that it's quite difficult to explain the kind of effect that you're trying to get out for your game.

168
00:18:50,000 --> 00:18:54,000
But using terminology such as this, even things like sprite sheets and sprites.

169
00:18:54,000 --> 00:19:00,000
And in this case, using something like knockback and also having some sense of how the game viewers should be like,

170
00:19:00,000 --> 00:19:02,000
which is having this enemy pausing.

171
00:19:02,000 --> 00:19:08,000
These are things that are more important than understanding how to implement it because AI will handle that part for you.

172
00:19:08,000 --> 00:19:17,000
So the main thing when you're bike coding games is that you want to be in a position where you can describe it in the best possible way for the AI to be able to go hit and implement it.

173
00:19:17,000 --> 00:19:21,000
Let me just show you what the end result was at the end of this stage.

174
00:19:21,000 --> 00:19:26,000
So if we jump to the game now, you will see that the skeleton has been integrated correctly.

175
00:19:26,000 --> 00:19:30,000
The skeleton can hit the player and there is the knockback as well.

176
00:19:30,000 --> 00:19:35,000
And you will see that there is always a brief pause before these skeletons starts chasing again.

177
00:19:35,000 --> 00:19:38,000
In the past, if you wanted to implement something like this, it would have taken you quite a while.

178
00:19:38,000 --> 00:19:44,000
This just took a single prompt as long as you know the right kind of terms to use to get the effect that you want.

179
00:19:44,000 --> 00:19:50,000
Now you will see that there is a problem with this attack while everything looks fine at first glance.

180
00:19:50,000 --> 00:19:51,000
There are several things that you will notice.

181
00:19:51,000 --> 00:19:57,000
So the first problem that you will notice is that even before the skeleton hits me, I'm really suffering from a damage.

182
00:19:57,000 --> 00:19:58,000
And that doesn't look right.

183
00:19:58,000 --> 00:20:04,000
Now the other thing is that I don't even need to be in the range of the weapon, but the player is also getting hit.

184
00:20:04,000 --> 00:20:08,000
These are bugs that completely break the immersion in front of your game.

185
00:20:08,000 --> 00:20:13,000
And that's why it's important to learn how to troubleshoot this using debug.

186
00:20:13,000 --> 00:20:17,000
So what I'm going to do right now is I'm going to show you the reason why this is happening in this final game.

187
00:20:17,000 --> 00:20:19,000
I'm going to pause the game for a while so you can see on the visual bounds.

188
00:20:19,000 --> 00:20:26,000
So this is how the characters are placed within the world based on their 256 by 256 frames.

189
00:20:26,000 --> 00:20:30,000
But the main thing that you want to see are the hit boxes and the attack boxes.

190
00:20:30,000 --> 00:20:36,000
So the hit boxes just tells you where exactly the weapon needs to hit in order for you to register.

191
00:20:36,000 --> 00:20:38,000
And you can see it's not the entire frame.

192
00:20:38,000 --> 00:20:43,000
It's just the box and this applies both for the skeleton as well as for the player.

193
00:20:43,000 --> 00:20:46,000
The other thing to highlight is that when we're walking, there are no attack boxes.

194
00:20:46,000 --> 00:20:50,000
It's only when the skeleton starts swinging that you see the yellow attack box.

195
00:20:50,000 --> 00:20:53,000
And only when I start attacking that my orange for that box comes out.

196
00:20:53,000 --> 00:20:57,000
This is what makes the game feel a lot better because when all of these are in place correctly,

197
00:20:58,000 --> 00:21:00,000
they only register the hit at the right frame.

198
00:21:00,000 --> 00:21:04,000
And this makes sure that the game doesn't feel broken as we see over here.

199
00:21:04,000 --> 00:21:09,000
Okay, so what we're going to do now is add the debug drawing so that we can start to visualize what's going on.

200
00:21:09,000 --> 00:21:12,000
So for us to be able to visualize this, I'm going to use this problem over here.

201
00:21:12,000 --> 00:21:16,000
I'm going to say it is worth being able to visualize the visual bounds hit box and the attack boxes,

202
00:21:16,000 --> 00:21:20,000
which should be separate elements for each actor, the pirate and the skeleton.

203
00:21:20,000 --> 00:21:22,000
Add these debug bound toggles to the right hand panel.

204
00:21:22,000 --> 00:21:24,000
So we're going to kick this off.

205
00:21:24,000 --> 00:21:28,000
And so what we have here, you can see that the bounds are now showing over here.

206
00:21:28,000 --> 00:21:32,000
And if we go into the game, I want to turn on the bounce so that we can actually visualize this.

207
00:21:32,000 --> 00:21:34,000
So immediately you can see several problems.

208
00:21:34,000 --> 00:21:39,000
So you can see immediately that the hit boxes fall the character as well as skeleton.

209
00:21:39,000 --> 00:21:40,000
I'm not accurate.

210
00:21:40,000 --> 00:21:45,000
And secondly, the attack boxes off the character and the skeleton are way too big

211
00:21:45,000 --> 00:21:47,000
and are already appearing on the first frame.

212
00:21:47,000 --> 00:21:51,000
So if I'm using this watch this in the moment the skeleton swings back the weapon,

213
00:21:51,000 --> 00:21:53,000
it already registers as a hit.

214
00:21:53,000 --> 00:21:55,000
And that's not the feature that we want.

215
00:21:55,000 --> 00:22:02,000
OK, so hopefully with these changes, you can understand the value of being able to see these debug bounds in the game.

216
00:22:02,000 --> 00:22:08,000
And as you can see here, codex has taken a screenshot of the game just to make sure that the debug balls are showing correctly.

217
00:22:08,000 --> 00:22:12,000
OK, so now that we know what are the problems, let's try to fix this.

218
00:22:12,000 --> 00:22:16,000
So we're going to fix the first problem where the weapon is showing up all the time.

219
00:22:16,000 --> 00:22:18,000
So this is the problem that I'm going to use.

220
00:22:18,000 --> 00:22:24,000
I'm going to say looking at the spreadsheets, the extent of the weapon only happens in frame 4 for the pirate and the skeleton.

221
00:22:24,000 --> 00:22:26,000
So you can see one, two, three, four.

222
00:22:26,000 --> 00:22:29,000
Only at the fourth frame does the weapon actually stick out.

223
00:22:29,000 --> 00:22:32,000
Everything else is just like part of the winding up.

224
00:22:32,000 --> 00:22:34,000
And this is the same thing for the skeleton as well.

225
00:22:34,000 --> 00:22:35,000
And you can see over here.

226
00:22:35,000 --> 00:22:37,000
Yep, only in the fourth frame.

227
00:22:37,000 --> 00:22:38,000
OK, so we should reflect this.

228
00:22:38,000 --> 00:22:40,000
Otherwise the player gets hit even in the windout.

229
00:22:40,000 --> 00:22:45,000
Take this opportunity to do the same for the pirate and implement damaging the skeleton in a similar way with kickback.

230
00:22:45,000 --> 00:22:48,000
So if you recall, we actually have not implemented attack.

231
00:22:48,000 --> 00:22:50,000
So it's very one side of right now.

232
00:22:50,000 --> 00:22:52,000
So the player can attack the skeleton at all.

233
00:22:52,000 --> 00:22:58,000
So we're going to replicate the functionality that we see with the skeleton attacking the pirate to be able to walk the other way as well.

234
00:22:58,000 --> 00:23:00,000
So we're just going to kick this off.

235
00:23:00,000 --> 00:23:10,000
And the goal here is so that we'll be able to have a much better hit detection and also the ability for us to hit the skeleton as well.

236
00:23:10,000 --> 00:23:12,000
Let's take a look at where it's gotten to right now.

237
00:23:12,000 --> 00:23:15,000
I'm going to pass the game for a while and take a look at all these.

238
00:23:15,000 --> 00:23:18,000
So you can see that we have all of the attack boxes now.

239
00:23:18,000 --> 00:23:21,000
And this time only when it's in frame four,

240
00:23:21,000 --> 00:23:30,000
do you see that the yellow and orange attack boxes get highlighted, which means that only when that frame is playing does the hit register.

241
00:23:30,000 --> 00:23:32,000
And this is looking a lot better now.

242
00:23:32,000 --> 00:23:36,000
So when it's wind up, I still have a chance to run away, but the problem still stands.

243
00:23:36,000 --> 00:23:41,000
So in terms of the timing of the frames, it still works, but the hit boxes are still not right.

244
00:23:41,000 --> 00:23:44,000
And the attack boxes are still not right in terms of size.

245
00:23:44,000 --> 00:23:51,000
So now let's fix the sizing of this hit boxes so that we at least have a chance to run away from the enemy.

246
00:23:51,000 --> 00:23:54,000
Otherwise, it makes it a lot difficult.

247
00:23:54,000 --> 00:24:03,000
So just to show you the power of the browser use over here is taking a screenshot just as the attack is coming up just to show that is indeed only highlighted at the correct frame.

248
00:24:03,000 --> 00:24:06,000
And that's how it verifies that it's working correctly.

249
00:24:06,000 --> 00:24:09,000
So what I'm going to do now is I'm going to fix the bounds issue.

250
00:24:09,000 --> 00:24:15,000
So with the bounds, I noticed that both the attack bounds are low and large in height given the weapons thinness.

251
00:24:15,000 --> 00:24:20,000
This should not be so big in height and it should be position higher relative to the bodies.

252
00:24:20,000 --> 00:24:25,000
Alright, so now let's kick this off so that we can end up with a situation where this gets fixed correctly.

253
00:24:25,000 --> 00:24:27,000
So let me just show you the hit boxes here.

254
00:24:27,000 --> 00:24:30,000
So you can see the attack boxes are a lot smaller.

255
00:24:30,000 --> 00:24:35,000
The position correctly relative to the character as well as the skeleton.

256
00:24:35,000 --> 00:24:40,000
So this gives you a chance to evade the attack rather than what we see over here where the hit box is just way too big.

257
00:24:40,000 --> 00:24:45,000
Alright, so you can see it says here that it's going to tighten the attack boxes to match the stick shape better.

258
00:24:45,000 --> 00:24:48,000
And then we'll see whether or not this gets it right.

259
00:24:48,000 --> 00:24:50,000
Sometimes you might get things wrong.

260
00:24:50,000 --> 00:24:52,000
So let's just take a look at how things are right now.

261
00:24:52,000 --> 00:24:55,000
So let's just turn it up and I'm going to pause it.

262
00:24:55,000 --> 00:24:59,000
But you can see while it's gotten the thinness correct, it's still a little bit too high.

263
00:24:59,000 --> 00:25:03,000
So we can pause it and take a screenshot.

264
00:25:03,000 --> 00:25:07,000
Okay, and then I wonder whether it's going to be able to figure things out on its own.

265
00:25:07,000 --> 00:25:15,000
But while it's running this, we can jumpstart and paste the screenshot here and say the thinness of the attack is correct.

266
00:25:15,000 --> 00:25:22,000
But they are still too high relative to the body of the characters.

267
00:25:22,000 --> 00:25:28,000
And you can press escape to interrupt in the current processing rather than scaring it.

268
00:25:28,000 --> 00:25:32,000
You saw the steer button just now, which you can use to let it continue what it's doing.

269
00:25:32,000 --> 00:25:34,000
And then gently steer it towards an outcome.

270
00:25:34,000 --> 00:25:40,000
But in this case, I want to get it to fix this correctly now so that it doesn't waste more time.

271
00:25:40,000 --> 00:25:42,000
So the attack boxes are a lot lower now.

272
00:25:42,000 --> 00:25:46,000
Okay, and this is actually a good size, I think.

273
00:25:46,000 --> 00:25:51,000
So you see I can now evade the attack and you can spend more time to tweak this boundary.

274
00:25:51,000 --> 00:25:57,000
So what you want to do generally is make the collision bounds for your character a little bit smaller than the enemy.

275
00:25:57,000 --> 00:26:01,000
So that it's easier for the player to hit the enemy and harder for the enemy to hit the player.

276
00:26:01,000 --> 00:26:08,000
Same thing with the attack bounds, you can make it longer so that the player is likely to hit the enemy more than the enemy is to hit the player.

277
00:26:08,000 --> 00:26:10,000
You can continue tweaking this as much as you want.

278
00:26:10,000 --> 00:26:15,000
For now, let's move on to the next stage where we're going to start to have a health system.

279
00:26:15,000 --> 00:26:18,000
So this is not just going to be an infinite running game where you just slap each other.

280
00:26:18,000 --> 00:26:22,000
So what we're going to do now is going to use this so we say let's make a health system.

281
00:26:22,000 --> 00:26:24,000
Give them to both the player and the enemies.

282
00:26:24,000 --> 00:26:30,000
The enemies health appears as a health bar and the players health appears in the top left corner in the longer bar.

283
00:26:30,000 --> 00:26:35,000
And we want this to change color as it depletes or green amber rate and it blinks when it's rate.

284
00:26:35,000 --> 00:26:36,000
And the player's health is five.

285
00:26:36,000 --> 00:26:38,000
The skeleton health is three hit them.

286
00:26:38,000 --> 00:26:39,000
He just won't always.

287
00:26:39,000 --> 00:26:46,000
Okay, so we're just going to kick this off and the idea is that you will see something like what we see over here where the health bar is above the enemy.

288
00:26:46,000 --> 00:26:48,000
And the player's health is at the top left corner.

289
00:26:48,000 --> 00:26:50,000
So it's more evident to the player.

290
00:26:50,000 --> 00:26:54,000
And you can see every hit is about one damage.

291
00:26:54,000 --> 00:26:56,000
So this works well.

292
00:26:56,000 --> 00:26:58,000
And I'm going to let this skeleton hit me a couple of times.

293
00:26:58,000 --> 00:27:04,000
And you can see it becomes amber and then it becomes red and blinking when it's only one health left.

294
00:27:04,000 --> 00:27:10,000
Okay, and that's the end result that we're looking for because otherwise this game is just an endless battle of the smacking each other silly.

295
00:27:10,000 --> 00:27:13,000
So it's now completed the implementation of the health bar and running some checks.

296
00:27:13,000 --> 00:27:15,000
So let's do our own track within the game.

297
00:27:15,000 --> 00:27:17,000
So you can see I've noted up the game here.

298
00:27:17,000 --> 00:27:23,000
And indeed the health bar is showing up above the skeleton and the character also suffers damage as well.

299
00:27:23,000 --> 00:27:26,000
You can see this is very close to the end result they were looking for.

300
00:27:26,000 --> 00:27:30,000
It's just a slight difference in how the health looks but everything looks correct.

301
00:27:30,000 --> 00:27:37,000
It has gone ahead to correctly play the death animation, which is interesting because in my earlier implementation of the game,

302
00:27:37,000 --> 00:27:40,000
it didn't automatically implement the death animation for me.

303
00:27:41,000 --> 00:27:45,000
In some cases, you will need to prompt the AI to implement the death animation,

304
00:27:45,000 --> 00:27:49,000
but in some cases, because it can check things out from the index.json,

305
00:27:49,000 --> 00:27:52,000
it will automatically implement the death animation for you.

306
00:27:52,000 --> 00:27:56,000
But now what we want to do is that we have to say on death of the enemy,

307
00:27:56,000 --> 00:28:00,000
the enemy should stay on the ground for a while before blinking and disappearing after a bit.

308
00:28:00,000 --> 00:28:04,000
If the player has death, a game over screen appears allowing me start of back to menu.

309
00:28:04,000 --> 00:28:07,000
The game should stop movement animations of any enemies in this case.

310
00:28:07,000 --> 00:28:12,000
So that's what we're going to do now and we're just going to have it such that the game just doesn't reach this strange stage

311
00:28:12,000 --> 00:28:15,000
where I can still move my character around and start with us down there.

312
00:28:15,000 --> 00:28:18,000
There's no sense that the game is making any progress.

313
00:28:18,000 --> 00:28:24,000
What you find is that it's really fun getting these new features in and getting animations in and getting things on the screen.

314
00:28:24,000 --> 00:28:27,000
But don't forget to close the loop on the game.

315
00:28:27,000 --> 00:28:28,000
You need to have a game over condition.

316
00:28:28,000 --> 00:28:31,000
And in some cases, you also need a clear win condition as well.

317
00:28:31,000 --> 00:28:34,000
So we're going to let the skeleton hit us a couple of times.

318
00:28:35,000 --> 00:28:37,000
And hopefully we get a game over screen.

319
00:28:37,000 --> 00:28:40,000
So not only a progression, but you need to have challenges as well.

320
00:28:40,000 --> 00:28:41,000
There you go, game over.

321
00:28:41,000 --> 00:28:43,000
So now there's a progression per round.

322
00:28:43,000 --> 00:28:46,000
We want to have an overall sense of progression as we go on with the game.

323
00:28:46,000 --> 00:28:50,000
What we're going to do now is you remember in this game is that each time we defeat the enemies,

324
00:28:50,000 --> 00:28:53,000
we'll clear that round and then we get a next round where more enemies spawn.

325
00:28:53,000 --> 00:28:58,000
And the way that you want to do this is that you want to have waves of enemies.

326
00:28:58,000 --> 00:29:00,000
But you also don't want this to be too random.

327
00:29:00,000 --> 00:29:02,000
So let me just show you how to go about doing it.

328
00:29:03,000 --> 00:29:06,000
So what we're going to do here is that we're going to use this pulling from.

329
00:29:06,000 --> 00:29:09,000
We need to have a game loop with some sense of progression.

330
00:29:09,000 --> 00:29:14,000
We should only have a bunch of rounds and we should progressively increase the number of spawn enemies per round.

331
00:29:14,000 --> 00:29:17,000
Only after all enemies are defeated, that's the round progress.

332
00:29:17,000 --> 00:29:20,000
And we should have this progression set up as a configurable parameter.

333
00:29:20,000 --> 00:29:21,000
And I explain what this means.

334
00:29:21,000 --> 00:29:27,000
But effectively, you want to be able to say, OK, round one has two enemies, round three has five enemies and things like that.

335
00:29:27,000 --> 00:29:31,000
And I say here, using Fibonacci increments, don't bother too much about this one now.

336
00:29:31,000 --> 00:29:38,000
But you want to have some kind of increment that makes the game more challenging over time rather than just doubling or incrementing by one.

337
00:29:38,000 --> 00:29:41,000
So we will keep track of the enemy's skill and time elapsed too.

338
00:29:41,000 --> 00:29:44,000
And this gives a timer and a sense of greater progression.

339
00:29:44,000 --> 00:29:48,000
We want a progressive fun loop that keeps the player coming back easy to learn progressively harder.

340
00:29:48,000 --> 00:29:54,000
So with this prompt, we are going to be able to build a game that has a sense of challenge and progression.

341
00:29:54,000 --> 00:29:57,000
On the topic of Fibonacci, this is what you call the Fibonacci sequence.

342
00:29:57,000 --> 00:30:02,000
If you come across with whenever I walk in games, whenever you're trying to have some sense of progression,

343
00:30:02,000 --> 00:30:08,000
rather than incrementing by one each time, sometimes adding this Fibonacci sequence gives you an automatic sense of progression.

344
00:30:08,000 --> 00:30:14,000
So you have one enemy in first round, then you have two enemies, then you have three, then you have five, then you have eight, then you have thirteen, then you have twenty-one.

345
00:30:14,000 --> 00:30:16,000
So it becomes a cover as it goes on.

346
00:30:16,000 --> 00:30:22,000
It's not the best, but it's useful whenever you need a non-linear sense of progression.

347
00:30:22,000 --> 00:30:25,000
If you don't know about Fibonacci, the way it works is that you do a zero one.

348
00:30:25,000 --> 00:30:31,000
Zero plus one is one, one plus one is two, one plus two is three, two plus three is five, and that's how the sequence comes about.

349
00:30:31,000 --> 00:30:33,000
But don't bother too much about this.

350
00:30:33,000 --> 00:30:38,000
This is going to be a big structural change because this is a core game loop, and this is going to take a while longer.

351
00:30:38,000 --> 00:30:42,000
Once you reach this point, you're going to be very close to having a complete game.

352
00:30:42,000 --> 00:30:47,000
So now let's take a look at whether or not the progression system is running.

353
00:30:47,000 --> 00:30:54,000
When you click the set box, oh, you can see round one has begun, and you can see that the information around the round is in the top left corner.

354
00:30:54,000 --> 00:31:00,000
And let's just defeat this enemy, and then round one is third, round two has begun, and now there are two enemies.

355
00:31:00,000 --> 00:31:06,000
So it's going to be a lot tougher now, and at least able to evade the skeleton.

356
00:31:06,000 --> 00:31:08,000
And let's try this, so you can see these two.

357
00:31:08,000 --> 00:31:09,000
So now you're going to have three.

358
00:31:09,000 --> 00:31:12,000
It's going to get more and more difficult as things go on.

359
00:31:12,000 --> 00:31:13,000
But here you are.

360
00:31:13,000 --> 00:31:15,000
You're going to have a complete game right now, right?

361
00:31:15,000 --> 00:31:20,000
You have a sense of progression, you have a challenge, you have enemies, you have an integrated artwork, you have a timer.

362
00:31:20,000 --> 00:31:21,000
This is looking really good.

363
00:31:21,000 --> 00:31:28,000
So if you've managed to reach to this stage, give yourself a pat on the back because this is further than most people would have gotten.

364
00:31:28,000 --> 00:31:33,000
Because they usually reach a stage where they have a couple of things running on screen, but there's no close loop.

365
00:31:33,000 --> 00:31:38,000
And there's just no sense that this is a complete game closing the loop is the most important thing when you're building a game.

366
00:31:38,000 --> 00:31:40,000
Now once you've reached to this stage, you're going to hit the problem.

367
00:31:40,000 --> 00:31:45,000
And if you start right now, you will see that it doesn't clean up the game properly.

368
00:31:45,000 --> 00:31:51,000
So you can get stuck here, you have us down, none of the enemies move, and it's really really late.

369
00:31:51,000 --> 00:31:54,000
So what you want to do is fix the bug up right now.

370
00:31:54,000 --> 00:31:55,000
So you can just say there's a bug.

371
00:31:55,000 --> 00:31:57,000
Pressing restart does not restart the state.

372
00:31:57,000 --> 00:31:58,000
No enemies are spawned.

373
00:31:58,000 --> 00:31:59,000
Play half is zero.

374
00:31:59,000 --> 00:32:05,000
In fact, in this case, enemies from previous session are left behind.

375
00:32:05,000 --> 00:32:07,000
So now we're going to press OK.

376
00:32:07,000 --> 00:32:12,000
And what we're going to do is press steer this time because we want it to continue off where it left off.

377
00:32:12,000 --> 00:32:13,000
So it's fixed.

378
00:32:13,000 --> 00:32:19,000
All of these things related to having a progression, but if your restart is not working, then that is a bug.

379
00:32:19,000 --> 00:32:26,000
So steer is like a way for it to continue from where it was at to then implement the fix that you're talking about.

380
00:32:26,000 --> 00:32:30,000
Press previously, if you remember, I press escape, that was just to interrupt it.

381
00:32:30,000 --> 00:32:35,000
And that was because I knew for a fact that I wanted it to stop whenever it was doing so that it can fix the bug.

382
00:32:35,000 --> 00:32:39,000
So steering your conversation is just another way for you to be able to get to the stage.

383
00:32:39,000 --> 00:32:43,000
This is a very common issue whenever you're working with phaser scenes.

384
00:32:43,000 --> 00:32:50,000
In 90% of the games that I work with, the restart always encounters issue because there's so much stuff going on within a game session.

385
00:32:50,000 --> 00:32:56,000
That restarting, we're needing it to clean up things and put back your health, all of these things tend to get missed.

386
00:32:56,000 --> 00:32:58,000
So don't be surprised if you encounter it.

387
00:32:58,000 --> 00:32:59,000
And this is the way that you fix it.

388
00:32:59,000 --> 00:33:04,000
Just tell the AI that it needs to clean up and restart the state and to clear all the entities down on the screen.

389
00:33:04,000 --> 00:33:12,000
So you say is that it's found the root cause is going to fix up what's going on over here because it's reusing some existing state.

390
00:33:12,000 --> 00:33:17,000
So what it's going to do is that it's going to do a proper cleanup whenever the game is to restart.

391
00:33:17,000 --> 00:33:20,000
And now they restart bugs should be fixed.

392
00:33:20,000 --> 00:33:24,000
So let's just take a look and get myself beaten up by the skeleton over here.

393
00:33:24,000 --> 00:33:25,000
And there we go.

394
00:33:25,000 --> 00:33:26,000
And now I want to restart.

395
00:33:26,000 --> 00:33:28,000
But now we have things running correctly.

396
00:33:28,000 --> 00:33:34,000
This means that we now have a complete loop and we're able to restart the game when we hit the game over conditions.

397
00:33:34,000 --> 00:33:35,000
Okay.

398
00:33:35,000 --> 00:33:36,000
So let's with it.

399
00:33:36,000 --> 00:33:37,000
Oh, there we go.

400
00:33:37,000 --> 00:33:38,000
And this gives a lot harder now.

401
00:33:38,000 --> 00:33:41,000
But now we can restart and everything.

402
00:33:41,000 --> 00:33:45,000
So now what we have right here is pretty much a complete game.

403
00:33:45,000 --> 00:33:49,000
But one of the things to notice that there's something that's missing, right?

404
00:33:49,000 --> 00:33:53,000
The missing thing here is that there's no sound and there's no music.

405
00:33:53,000 --> 00:33:57,000
Remember when I told you that when you're building a game, try to focus on making it complete.

406
00:33:57,000 --> 00:34:00,000
No game is complete if there's no sound and music.

407
00:34:00,000 --> 00:34:05,000
Let me show you a trick to get all of the sound and music into your game

408
00:34:05,000 --> 00:34:10,000
without you having to spend too much time looking and searching around for sound effects.

409
00:34:10,000 --> 00:34:14,000
So for us to be able to leverage sounds within this game, we use something called 11 labs.

410
00:34:14,000 --> 00:34:17,000
And what we're going to do is install the 11 lab skill.

411
00:34:17,000 --> 00:34:21,000
The 11 lab skill basically allows you to generate sound effects, music,

412
00:34:21,000 --> 00:34:23,000
and many other things using AI.

413
00:34:23,000 --> 00:34:28,000
And the skills allows you to effectively just do everything within your command line or within codex.

414
00:34:28,000 --> 00:34:31,000
And the only thing that you need is a 11 labs API key.

415
00:34:31,000 --> 00:34:33,000
So this is my 11 labs website.

416
00:34:33,000 --> 00:34:37,000
If you go over to the bottom left corner and go to developers and they go to API keys.

417
00:34:38,000 --> 00:34:40,000
You can actually create a key over here.

418
00:34:40,000 --> 00:34:42,000
And then you can just call it whatever you want.

419
00:34:42,000 --> 00:34:45,000
All right, I'm going to call this a pirate meat up.

420
00:34:45,000 --> 00:34:49,000
And what you want to do is have sound effects and music narration.

421
00:34:49,000 --> 00:34:51,000
Only thing you really need.

422
00:34:51,000 --> 00:34:53,000
Everything else you can just leave alone.

423
00:34:53,000 --> 00:34:54,000
You want to copy this key.

424
00:34:54,000 --> 00:34:58,000
And then what you want to do is you want to go into this folder.

425
00:34:58,000 --> 00:35:00,000
Create in dot e and v file.

426
00:35:00,000 --> 00:35:02,000
This is called an environment variable file.

427
00:35:03,000 --> 00:35:07,000
But you want to call it 11 labs API key equals to your key here.

428
00:35:07,000 --> 00:35:09,000
I'm going to do 11 labs API key.

429
00:35:09,000 --> 00:35:13,000
And then I'm going to copy the key and pop it.

430
00:35:13,000 --> 00:35:18,000
Okay, so this is the main thing and this allows you to utilize your 11 labs account

431
00:35:18,000 --> 00:35:20,000
and regenerate the sounds that you need.

432
00:35:20,000 --> 00:35:21,000
So that's the API key.

433
00:35:21,000 --> 00:35:23,000
So the next thing that you want to do is install the skills.

434
00:35:23,000 --> 00:35:26,000
So just copy this command that you have over here.

435
00:35:26,000 --> 00:35:31,000
And what you want to do is open up the terminal within your project and paste it in here.

436
00:35:31,000 --> 00:35:32,000
Just put on instructions over here.

437
00:35:32,000 --> 00:35:34,000
If you install your packages, say yes.

438
00:35:34,000 --> 00:35:35,000
Okay.

439
00:35:35,000 --> 00:35:38,000
And what it's going to say is that it found eight skills for 11 labs.

440
00:35:38,000 --> 00:35:39,000
What do you need?

441
00:35:39,000 --> 00:35:40,000
We just want music.

442
00:35:40,000 --> 00:35:42,000
So press space and sound effects.

443
00:35:42,000 --> 00:35:43,000
Just press enter.

444
00:35:43,000 --> 00:35:46,000
You will already work with codex and cursor and all this stuff.

445
00:35:46,000 --> 00:35:49,000
And usually it will default to having clock as well.

446
00:35:49,000 --> 00:35:53,000
So you just press enter here and then do installation scope to product.

447
00:35:53,000 --> 00:35:55,000
And then you can do sinling recommended.

448
00:35:55,000 --> 00:36:00,000
And what this means is that rather than having copies of the skills for clock code and the others.

449
00:36:01,000 --> 00:36:03,000
It will just link both of them together.

450
00:36:03,000 --> 00:36:05,000
Just click OK and then proceed with the installation.

451
00:36:05,000 --> 00:36:10,000
If you look into your project, you will see that you have a music skill and the sound effect skill.

452
00:36:10,000 --> 00:36:12,000
This will also work for cloud music and sound effects.

453
00:36:12,000 --> 00:36:17,000
If you do dollar sign and then you do music, you will see that it's pulled it in already.

454
00:36:17,000 --> 00:36:20,000
And you can do dollar sign sound effects.

455
00:36:20,000 --> 00:36:25,000
So what you want to do now is use this problem that we have over here.

456
00:36:25,000 --> 00:36:28,000
And it's based on the vibes of our game.

457
00:36:28,000 --> 00:36:31,000
Let's generate bgm or game that matches the theme of pirates on a ship.

458
00:36:31,000 --> 00:36:33,000
I guess in this case, it's on a beach.

459
00:36:33,000 --> 00:36:34,000
So we can change that.

460
00:36:34,000 --> 00:36:40,000
We're going to use the music skill also add in sound effects across the board using the sound effects skill.

461
00:36:40,000 --> 00:36:41,000
So I'm going to sound effects.

462
00:36:41,000 --> 00:36:46,000
This should be placed in public assets bgm and public SS sfx with assets.

463
00:36:46,000 --> 00:36:47,000
Fuggets are updated.

464
00:36:47,000 --> 00:36:50,000
Actually, this should be assets index.

465
00:36:50,000 --> 00:36:53,000
Fuggets not data for music have four candidates switchable within the debug panel.

466
00:36:53,000 --> 00:36:55,000
So this is just to give you some variation.

467
00:36:55,000 --> 00:36:56,000
So you can pick.

468
00:36:56,000 --> 00:36:58,000
I'm going to kick this off right now.

469
00:36:58,000 --> 00:37:01,000
And this, my friends, is like a protein.

470
00:37:01,000 --> 00:37:03,000
If you need music and sound effects, we are game.

471
00:37:03,000 --> 00:37:07,000
This is not perfect, but it will get you quite far along the way.

472
00:37:07,000 --> 00:37:08,000
What you see here.

473
00:37:08,000 --> 00:37:10,000
And I'm going to turn on the music for you now.

474
00:37:10,000 --> 00:37:16,000
Everything that you see in this game has been generated using the exact process using that single trapped.

475
00:37:16,000 --> 00:37:29,000
What you saw that was the exact same prompt that I used for that game.

476
00:37:29,000 --> 00:37:33,000
And it basically just put in the background music for me and all the sound effects.

477
00:37:33,000 --> 00:37:35,000
And I didn't have to do anything more.

478
00:37:35,000 --> 00:37:37,000
So let's just see what happens over here.

479
00:37:37,000 --> 00:37:41,000
And you will see that sometimes it can't find the 11 lapsed API P.

480
00:37:41,000 --> 00:37:44,000
So he says it's going to check the reposed local dot EMP path.

481
00:37:44,000 --> 00:37:49,000
That's exactly why we went ahead and put this in the dot EMP file.

482
00:37:49,000 --> 00:37:51,000
So remember that there needs to be a dot in front of it.

483
00:37:51,000 --> 00:37:54,000
And then it needs to be named exactly this dot EMP.

484
00:37:54,000 --> 00:37:59,000
This is just a convention for the system that we're using for loading environment keys.

485
00:37:59,000 --> 00:38:03,000
And when you do this, remember never, ever to share your keys.

486
00:38:03,000 --> 00:38:08,000
Because if you this key get leaked, somebody can use your account to generate their sounds.

487
00:38:08,000 --> 00:38:09,000
And it will deplete your credits.

488
00:38:09,000 --> 00:38:10,000
Do not share this.

489
00:38:10,000 --> 00:38:13,000
I'm showing it here because I'm going to delete this key once this tutorial is completed.

490
00:38:14,000 --> 00:38:17,000
Whatever you have in this dot EMP file, you should never ever.

491
00:38:17,000 --> 00:38:21,000
If you're using git, make sure that your getting all has the dot EMP.

492
00:38:21,000 --> 00:38:25,000
So that this does not get sent into your repository as well.

493
00:38:25,000 --> 00:38:26,000
All right.

494
00:38:26,000 --> 00:38:27,000
So this is done now.

495
00:38:27,000 --> 00:38:32,000
It's gotten all of the four different tracks as well as the sound effects for attack,

496
00:38:32,000 --> 00:38:34,000
hurt, death, enemy spawn and all the other stuff.

497
00:38:34,000 --> 00:38:36,000
So let's just check it out.

498
00:38:36,000 --> 00:38:39,000
Just make sure to go into your settings to make sure new dissolve.

499
00:38:39,000 --> 00:38:41,000
And let's just see what turns out.

500
00:38:43,000 --> 00:39:06,000
All right, so there you have it.

501
00:39:06,000 --> 00:39:08,000
That's the music and the sound effects are already integrated.

502
00:39:08,000 --> 00:39:10,000
There's some things that you probably want to tweak.

503
00:39:10,000 --> 00:39:14,000
But this gets you very far along the way and it makes the game feel a lot more complete.

504
00:39:14,000 --> 00:39:18,000
I want to finish off this tutorial by adding some level of polish.

505
00:39:18,000 --> 00:39:20,000
If you look at the game that we have over here in this version,

506
00:39:20,000 --> 00:39:25,000
you can see there's a little bit of what I like to call juice that makes the game feel more alive.

507
00:39:25,000 --> 00:39:28,000
So you can see there's the flashing.

508
00:39:28,000 --> 00:39:30,000
There's the screen shake.

509
00:39:30,000 --> 00:39:33,000
And it makes the game feel easier.

510
00:39:33,000 --> 00:39:38,000
One of the key things here is that when there's a contact mate between the player and the skeleton,

511
00:39:38,000 --> 00:39:43,000
the game freezes for a very short period of time and that makes the impact seem a lot more.

512
00:39:43,000 --> 00:39:46,000
I'm going to just continue this conversation that we have over here.

513
00:39:46,000 --> 00:39:55,000
And I'm going to say at debug purgles for for muting sound and BGM in the debug panel.

514
00:39:55,000 --> 00:40:00,000
And so I'm just going to let this continue and implement that because it doesn't have anything else to do with the game.

515
00:40:00,000 --> 00:40:04,000
But let's now finally move on to adding the polish that we want.

516
00:40:04,000 --> 00:40:07,000
Now you can see it's already added the shadows underneath the actor.

517
00:40:07,000 --> 00:40:09,000
So that's great. So we don't need that.

518
00:40:09,000 --> 00:40:12,000
But what we're going to do is that we're going to say we're going to have a noise-based screen shake.

519
00:40:12,000 --> 00:40:14,000
So we don't want to have a random screen shake.

520
00:40:14,000 --> 00:40:17,000
A noise-based screen shake just makes things feel a lot smoother.

521
00:40:17,000 --> 00:40:20,000
Minor and enemy heat, but it shakes more when there's a death.

522
00:40:20,000 --> 00:40:25,000
We want to have freeze time for the enemy hits less when it's just a regular hit and more on death.

523
00:40:25,000 --> 00:40:30,000
And then a white flash when the player gets hurt with a cooldown for invulnerability.

524
00:40:30,000 --> 00:40:32,000
And then I want it to have some perfectly effects.

525
00:40:32,000 --> 00:40:36,000
And that's all you need to do to make the game feel a little bit more polished.

526
00:40:36,000 --> 00:40:40,000
So here we are. And you can see that the game has already implemented the muting.

527
00:40:40,000 --> 00:40:45,000
And that allows me to not have this playing while I'm talking over the video as well.

528
00:40:45,000 --> 00:40:48,000
Okay. So now you can see compared to what we saw earlier.

529
00:40:48,000 --> 00:40:50,000
There's just no screen shake. There's no flashing.

530
00:40:50,000 --> 00:40:53,000
So ultimately the game still feels a little bit flat.

531
00:40:53,000 --> 00:40:57,000
And these little bits of juice will have to make your game feel a lot more complete.

532
00:40:57,000 --> 00:41:00,000
So the polish is completed. So you can see there's just a bit of screen shake.

533
00:41:00,000 --> 00:41:07,000
And when the skeleton actually gets killed, you can see that the screen shake goes to become a lot more evident.

534
00:41:07,000 --> 00:41:10,000
Okay. And if you actually look very closely, I'm going to just make this a bit bigger.

535
00:41:10,000 --> 00:41:16,000
So you can see that there's particular effects now when we hit the skeleton and you saw that.

536
00:41:16,000 --> 00:41:19,000
It feels a lot more impactful, right?

537
00:41:19,000 --> 00:41:25,000
So this are the things that you want to do in order to polish up your game just to make the game feel a lot better.

538
00:41:25,000 --> 00:41:35,000
And we can just turn on the music again. I'm going to switch it to this movie.

539
00:41:35,000 --> 00:41:58,000
And so there you have it. We have completed this entire game from start to finish.

540
00:41:58,000 --> 00:42:01,000
So what started out as a blue square walking around on a black screen.

541
00:42:01,000 --> 00:42:10,000
We have now come all the way to this fully integrated game with sound effects with music with particle effects and with all the different levels of polish.

542
00:42:10,000 --> 00:42:14,000
It has a full progression loop. It has increasing difficulty of challenges.

543
00:42:14,000 --> 00:42:18,000
And all this was done if you recall without writing a single line of code.

544
00:42:18,000 --> 00:42:21,000
We was all prompted within the codex application.

545
00:42:21,000 --> 00:42:26,000
So I really hope you enjoyed today's tutorial. Don't forget to give it a like and also hit that follow button.

546
00:42:26,000 --> 00:42:29,000
So that you always be the first to know when you content back disrupts.

547
00:42:29,000 --> 00:42:31,000
So till next time, I'll see you.


```
