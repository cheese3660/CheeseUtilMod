# Cheese Util Mod
## Installation
Clone the repository, and copy the ```cheeseutil``` folder into ```<logicworld>/GameData```
## Memory
This mod adds multiple memory components, they have the address lines on top, and the data on bottom
The least significant bits are the smallest pins
On the top of the memory is the control wires, they are in this order from biggest to smallest:
	- Chip Select/Read, Enables the rams output
	- Write, writes the data to the selected address
	- Load Select, enables the ram chip for file loading via the "loadram" command
### Loadram command
Usage: loadram /path/to/file
When run this command looks for all RAM chips with the load select line active and loads the file into those chips (Little Endian)
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
### Text Displays
This is a resizable addressable recolorable text display, when looking from the back the pins should be in the bottom right quadrant to look right
The lines on the back from the bottom to top are, LSB is the smallest pin
	- 6 bits: Column
	- 6 bits: Row
	- 8 bit: ASCII character, the top bit is going to be used to invert the character on the display in a future version
	- 8 control lines from smallest to largest
		- Clear, clears the screen
		- Set Character, sets the character at the specified location
		- Set Cursor Position, sets the cursors position to the specified location (only affects cursor blinking)
		- Enable Cursor Blink, while this is active the cursor can blink on the screen
		- Scroll Up, scrolls each line of the display up by 1
		- Scroll Down, scrolls each line of the display down by 1
		- Scroll Left, scrolls each column of the display left by 1
		- Scroll Right, scrolls each column of the display right by 1
## Utilities
This mod also adds a few utilities
### Binary to BCD converter
These are binary to bcd converters, the inputs have the LSB on the bottom left, and outputs, each column is a digit and the bottom is the LSB