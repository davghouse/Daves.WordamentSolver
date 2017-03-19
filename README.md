Dave's Wordament Solver
================

Wordament solver that handles an arbitrary number of special tiles, finds the many-to-many word-path relationships, and approximates a best path.

Latest release [here](https://github.com/davghouse/Daves.WordamentSolver/releases/tag/v1.4.1).

Solver library available as a [NuGet Package](https://www.nuget.org/packages/Daves.WordamentSolver).

![before](/Screenshots/BeforeSolving.png)
![after](/Screenshots/AfterSolving.png)

Implementation
--------------

The solution uses a trie, which offers the big benefit of short-circuiting the recursion/DFS-ing when a string doesn't appear as a prefix in the dictionary (and therefore adding more tiles to it can't produce words).
As a minor optimization, previous search results are used to skip down into the trie in successive searches (making use of the fact that the strings from the latter are necessarily prefixed by the strings from the former, given how we're traversing the board).

I'm using the Model-View-Presenter pattern.
The view knows nothing about the presenter, it just fires events for the presenter to subscribe to and handle. Everything knows about the model (but as little as possible).

Everything is made to be generalizable, so it would be easy to extend the solution to support new tile types, different board sizes (as long as they're rectangular), different allowed moves (like only diagonals, or weird jumps), or other languages.
Any number of special tiles are allowed, and invalid tiles are handled gracefully by simply being ignored.

The best path is a travelling salesman problem where words are vertices and edge lengths are the distances from a word's last tile to another word's first tile.
Visiting a word adds on a fixed amount of length equal to the word's path length (but you could incorporate this into the edges).
We want to find the shortest path visiting each word exactly once... well, sort of.
It's a little more complicated than that because we don't actually visit words, we visit paths.
A path can yield multiple words (either/or tiles), and it's not just any path producing a word that we visit, the path has to be a best path for a word.
Words can be produced by many paths, the best one is the one that's worth the most points.
For a board full of basic tiles these distinctions don't matter, because paths only yield one word and all paths yielding a word are worth the same amount of points.
I'm just doing a nearest neighbor approximation for now, which seems really good.
For example, on a board where the total path length from the words themselves was 930, the nearest neighbor path length was 1060, and the path length from sorting by anything else was around 1400.

Limitations
-----------
The dictionary file is based off of the TWL06 dictionary, with 16 letter words added from litscape.com.
Wordament has something like 170 banned words, and I don't know all of them.
The few I do know I haven't bothered removing from the dictionary.
There are also words Wordament has that my dictionary doesn't, like s'mores spelled as smores.
I tried using SOWPODS but that was more off than TWL.
As it stands right now, the TWL dictionary is pretty close.
Were the banned words removed, it might be a proper subset of the Wordament dictionary.
I know for sure that SOWPODS is no where near a subset, which seems worse.
Ideally the dictionary gets maintained with new words being added as they're discovered to be missing.

In the game, words have 'common' and 'uncommon' designations.
I don't know how to create those designations.
This matters because at least 'common' digram words get a bonus of 5 points, after length multipliers are applied.
Theme-based words get bonuses too, another case I can't handle, and there could be more.

Either/or tiles get a fixed value of 20 points.
That's what I've seen in recent games (as of 2014).
Other multi-letter tiles get their individual values summed + 5.
3 or 4 might be correct more of the time, but we probably want to focus on that tile regardless.
