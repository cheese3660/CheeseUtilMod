# Cheese Util Mod
## Installation
Clone the repository, and copy the ```cheeseutil``` folder into ```<logicworld>/GameData```
## Memory
This mod adds multiple memory components, they have the address lines on top, and the data on bottom
The least significant bit is to the left side when looking at the inputs
On top of the RAM components are a chip select and write line from left to write looking at the input side
And on the back is the data out, with each bit being in the same position as the data in
## Displays
This mod also adds a few display components
### Seven Segment Displays
The least significant bit for a seven segment display is at the bottom left looking at its back
Each segment is defined as follows with 1 being the LSB
```
 111
6   2
6   2
6   2
 777
5   3
5   3
5   3
 444
```
These displays can be color picked by pressing x on them
### Hex Displays
These are the same as seven segment displays but instead of taking in 7 individual segments they take in a 4 bit hex number
With the least significant bit being in the same position as the 7 segment
These displays can also be color picked
## Utilities
This mod also adds a few utilities
### Binary to BCD converter
These are binary to bcd converters, the inputs have the LSB on the bottom left, and outputs, each column is a digit and the bottom is the LSB