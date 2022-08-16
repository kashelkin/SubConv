# SubConv
**Subconv** is **ASS** to **SRT** subtitle format converter that merges overlapping subtitles.

# Installation
You can download the latest version from the [releases page](https://github.com/kashelkin/SubConv/releases).
# Usage
## Quick start
Let's say we have the following subtitle file `movie.ass`:
```
...
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
...
Dialogue: 0,0:00:59.04,0:01:02.71,Eng,,0,0,0,,{\blur4.5}{\fad(489,0)}I've still got a lot of years ahead of me
Dialogue: 0,0:00:59.04,0:01:02.71,Roma,,0,0,0,,{\blur4.5}{\fad(489,0)}Kono omoi wo keshiteshimau niwa
...
Dialogue: 0,0:05:26.01,0:05:28.51,Default,Roy,0,0,0,,But you claimed that you would be able to-
Dialogue: 0,0:05:27.47,0:05:31.06,Default-alt,Ed,0,0,0,,Yeah, yeah, I get it. My mistake.
...
Dialogue: 1,0:08:38.57,0:08:42.53,Stock,,0,0,0,,{\blur0.6\pos(960,875)\fad(700,0)}Central Prison
Dialogue: 0,0:08:41.91,0:08:45.71,Default,Kimb,0,0,0,,That's the funniest joke I've heard in a while.
...
```
It contains styles:
- `Default` - default speech
- `Default-alt` - when someone interrupts
- `Stock` - titles
- `Eng` - English translation of song lyrics
- `Roma` - karaoke

**SRT** format does not have style information so we need to distinguish non-speech subtitles. Let's  wrap `Eng` and `Roma` styles in `{ }` and wrap `Stock` in `[ ]`.

Also we need to specify vertical layout of merged subtitle entry:

1. `Default`, `Default-alt`
2. `Eng`
3. `Roma`
4. `Stock`

We want titles to be on the lowermost level. Song lyrics should be located above it and karaoke should always be under English lyrics.
Speech should be on the upper level.

To achieve all these requirements we can run the following command:
```
subconv movie.ass -t w:Eng,Roma;{;} w:Stock;[;] m:Default,Default-alt;Eng;Roma;Stock
```
After -t option goes list of subtitle transforms:
- `w:Eng,Roma;{;}` - wrap styles `Eng` and `Roma` in `{ }`
- `w:Stock;[;]` - wrap style `Stock` in `[ ]`
- `m:Default,Default-alt;Eng;Roma;Stock` - merge overlapping subtitles with vertical layout: `Default`, `Default-alt` / `Eng` / `Roma` / `Stock`. `Default,Default-alt` can be replaced with `*` which means 'all other styles': `m:*;Eng;Roma;Stock`.

It will generate `movie.srt` file:
```
...
16
00:00:59,040 --> 00:01:02,710
{I've still got a lot of years ahead of me}
{Kono omoi wo keshiteshimau niwa}

...

86
00:05:26,010 --> 00:05:27,470
But you claimed that you would be able to-

87
00:05:27,470 --> 00:05:28,510
But you claimed that you would be able to-
Yeah, yeah, I get it. My mistake.

88
00:05:28,510 --> 00:05:31,060
Yeah, yeah, I get it. My mistake.

...

149
00:08:38,570 --> 00:08:41,910
[Central Prison]

150
00:08:41,910 --> 00:08:42,530
That's the funniest joke I've heard in a while.
[Central Prison]

151
00:08:42,530 --> 00:08:45,710
That's the funniest joke I've heard in a while.
...
```
