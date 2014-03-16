wordament-solver
================

(English) Wordament solver that handles digrams, prefixes, suffixes, and either/or tiles.

/Screenshots/1.PNG
/Screenshots/2.PNG

Implementation
--------------

Storing the dictionary in a trie wasn't necessary for this, so I'm just using a list of list strings, where each list string (each group) contains words sharing the same 3-letter prefix. The algorithm goes from the dictionary to the board, checking first for the existence of a group's prefix, and if finding it, checking the entire group. Words are checked by looking recursively (in adjacent tiles) for the word's (remaining) letters.

The dictionary file 3PrefixedTWL.txt has the words already arranged into their prefix groups.

Limitations
-----------

Wordament has something like 170 banned words, and I don't know all of them. The few I do know I haven't bothered removing from the dictionary. There are also words Wordament has that my dictionary doesn't, like s'mores spelled as smores, for instance. I tried using SOWPODS and that was more off than TWL. As it stands right now, the TWL dictionary is pretty close. Were the banned words removed, it might be a proper subset of the Wordament dictionary. I know for sure that SOWPODS is no where near a subset, which is way worse.

In the game, words have 'common' and 'uncommon' designations. I don't know how to create those designations. This matters because at least 'common' digram words get a bonus of 5 points, after length multipliers are applied. Theme-based words get bonuses too, another case I can't handle, and there could be more.

There are ordering options based on physical path length. Paths are non-unique and I take the first valid one found, so the displayed ordering will be one of many.

Tile points can be input manually, or if the board-type allows it or an approximation is good enough (which is almost always), the following values can be automatically used:

A 2, B 5, C 3, D 3, E 1, F 5, G 4, H 4, I 2, J 10, K 6, L 3, M 4, N 2, O 2, P 4, Q 8, R 2, S 2, T 2, U 4, V 6, W 6, X 9, Y 5, Z 8.

Either/Or tiles get a fixed value of 20 points. That's what I've seen in recent games.
Other multi-letter tiles get their individual values summed + 5. 3 or 4 might be correct more of the time, but we probably want to focus on that tile regardless.
 

I'm not sure if base tiles always have the same values when not specified otherwise in high-value type boards / letter in the corners type boards, but I expect they do. I'm not sure if I can figure out the exact value of prefix/suffix/digram/either\or tiles. I've only looked into it a little bit.

Other Stuff
-----------

I want to sort the words in some efficient way where the last letter of this word is the first letter of the next word. And a sort, other than alphabetical, that gives me high-value words and then all of their extensions in order. Like GUARD: ED, ING, S all in a row.

Banned words I've observed that weren't obvious:

arse, lez

