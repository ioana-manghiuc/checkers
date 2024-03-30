# checkers
A game of checkers implemented in C# using the MVVM design pattern. 

It will be played on an _8x8_ checkerboard. There will be two types of pieces: 
1. **Man**: an uncrowned piece that can move one step ahead (diagonally), jump, or zigzag ("multiple jumps", this option is not activated by default and the user must specify whether they want to enable successive jumps for their game)
2. **King**: a man that reaches the farthest row forward becomes a king, who can move forward as well as backward.

The game ends when one of the players has no more pieces on the board, thus the other one wins.

The two main menus are:
1. **File** with the submenu: New Game, Save (the current game), Open (a saved game), Allow Multiple Jumps, Statistics.
2. **Help** with the submenu: About (which will have information about the game and its creator).
