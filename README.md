wordament-solver
================

Wordament solver that handles digrams, prefixes, suffixes, and either/or tiles.

Implementation
--------------

Storing the dictionary in a trie wasn't necessary for this, so I'm just using a list of list strings, where each list string (each group) contains words sharing the same 3-letter prefix. The algorithm goes from the dictionary to the board, checking first for the existence of a group's prefix, and if finding it, checking the entire group. Words are checked by looking recursively (in adjacent tiles) for the word's (remaining) letters.

The dictionary file 3PrefixedTWL.txt has the words already arranged into their prefix groups.

Limitations
-----------

Wordament has something like 170 banned words, and I don't know all of them. The few I do know I haven't bothered removing from the dictionary. There are also words Wordament has that my dictionary doesn't, like s'mores spelled as smores, for instance. I tried using SOWPODS and that was more off than TWL. As it stands right now, the TWL dictionary is pretty close. Were the banned words removed, it might be a proper subset of the Wordament dictionary. I know for sure that SOWPODS is no where near a subset, which is way worse.

In the game, words have 'common' and 'uncommon' designations. I don't know how to create those designations. This matters because at least 'common' digram words get a bonus 5 points, after length multipliers are applied. There might be other situations where words get extra points, like themed words, another case I can't handle. Right now, only length multipliers are in use.

There are ordering options based on physical path length. Paths are non-unique and I take the first valid one found, so these orderings will only be approximate.




