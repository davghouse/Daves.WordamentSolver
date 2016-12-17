wordament-solver
================

Wordament solver that supports an arbitrary number of digram, prefix, suffix, and either/or tiles.

Latest release [here](https://github.com/davghouse/wordament-solver/releases/tag/v1.2.1).

![before](/Screenshots/BeforeSolving.png)
![after](/Screenshots/AfterSolving.png)

Implementation
--------------

The solution uses a trie, which offers the big benefit of short-circuiting the recursion/DFS-ing when a string doesn't appear as a prefix in the dictionary (and therefore adding more tiles to it can't produce words).
As a minor optimization, previous search results are used to skip down into the trie in successive searches (making use of the fact that the strings from the latter are necessarily prefixed by the strings from the former, given how we're traversing the board).

I'm using the Model-View-Presenter pattern but it's a little awkward and I'm not quite sure what to consider a model when nothing is being persisted.
The view knows nothing about the presenter, it just fires events for the presenter to subscribe to and handle. Everything knows about the model (but as little as possible), which seems okay because I guess it's like a model/view-model hybrid right now.

Everything is made to be generalizable, so it would be easy to extend the solution to support new tile types, different board sizes (as long as they're rectangular), and different allowed moves (like only diagonals, or weird jumps).
Any number of special tiles are allowed, and invalid tiles are handled gracefully by simply being ignored.

Limitations
-----------
I'm using the TWL06 dictionary, which doesn't have 16-letter words.

Wordament has something like 170 banned words, and I don't know all of them.
The few I do know I haven't bothered removing from the dictionary.
There are also words Wordament has that my dictionary doesn't, like s'mores spelled as smores, for instance.
I tried using SOWPODS and that was more off than TWL.
As it stands right now, the TWL dictionary is pretty close.
Were the banned words removed, it might be a proper subset of the Wordament dictionary.
I know for sure that SOWPODS is no where near a subset, which seems worse.

All that being said, the dictionary is configurable through the exe.config.
Specify the name of a newline-separated dictionary and put it in the same place as the TWL06 one.

In the game, words have 'common' and 'uncommon' designations.
I don't know how to create those designations.
This matters because at least 'common' digram words get a bonus of 5 points, after length multipliers are applied.
Theme-based words get bonuses too, another case I can't handle, and there could be more.

There are ordering options based on physical path length.
Paths may be non-unique and I take the first valid one found, so the displayed ordering may be one of many.

Tile points can be input manually, or if the board-type allows it or an approximation is acceptable, the following values can be automatically used:

A 2, B 5, C 3, D 3, E 1, F 5, G 4, H 4, I 2, J 10, K 6, L 3, M 4, N 2, O 2, P 4, Q 8, R 2, S 2, T 2, U 4, V 6, W 6, X 9, Y 5, Z 8.

Either/or tiles get a fixed value of 20 points.
That's what I've seen in recent games (as of 2014).
Other multi-letter tiles get their individual values summed + 5.
3 or 4 might be correct more of the time, but we probably want to focus on that tile regardless.

I'm not sure if base tiles always have the same values when not specified otherwise in high-value type boards / letter in the corners type boards.
I'm not sure if I can figure out the exact value of prefix/suffix/digram/either\or tiles.
I've only looked into it a little bit.

To-do
-----------

I want to sort the words in some efficient way where the last letter of this word is the first letter of the next word (or adjacent to it, maybe).
Like find the most efficient way to solve the board, optimizing for finger-movement.
