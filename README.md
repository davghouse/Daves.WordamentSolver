wordament-solver
================

(English) Wordament solver that handles digrams, prefixes, suffixes, and either/or tiles.

Implementation
--------------

Storing the dictionary in a trie wasn't necessary for this, so I'm just using a list of list strings, where each list string (each group) contains words sharing the same 3-letter prefix. The algorithm goes from the dictionary to the board, checking first for the existence of a group's prefix, and if finding it, checking the entire group. Words are checked by looking recursively (in adjacent tiles) for the word's (remaining) letters.

The dictionary file 3PrefixedTWL.txt has the words already arranged into their prefix groups.

Limitations
-----------

Wordament has something like 170 banned words, and I don't know all of them. The few I do know I haven't bothered removing from the dictionary. There are also words Wordament has that my dictionary doesn't, like s'mores spelled as smores, for instance. I tried using SOWPODS and that was more off than TWL. As it stands right now, the TWL dictionary is pretty close. Were the banned words removed, it might be a proper subset of the Wordament dictionary. I know for sure that SOWPODS is no where near a subset, which is way worse.

In the game, words have 'common' and 'uncommon' designations. I don't know how to create those designations. This matters because at least 'common' digram words get a bonus of 5 points, after length multipliers are applied. Theme-based words get bonuses too, another case I can't handle, and there could be more.

There are ordering options based on physical path length. Paths are non-unique and I take the first valid one found, so the displayed ordering will be one of many.

Tile points can be input manually, or if the board-type allows it or an approximation is good enough, the following values can be automatically used:

A 2, B 5, C 3, D 3, E 1, F 5, G 4, H 4, I 2, J 10, K 6, L 3, M 4, N 2, O 2, P 4, Q 8, R 2, S 2, T 2, U 4, V 6, W 6, X 9, Y 5, Z 8.

Multi-letter tiles get individual values summed + 3. 

I'm not sure if base tiles always have the same values when not specified otherwise in high-value type boards / letter in the corners type boards. I'm not sure if I can figure out the exact value of prefix/suffix/digram/either\or tiles. I've only looked into it a little bit.