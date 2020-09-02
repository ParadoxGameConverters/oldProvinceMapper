This is helper program to convert a CK3 barony map to a county map.
It was put together very quickly, and the code quality is representative of that.

Here's the instructions for use:

Zeroth: Make sure you have Python installed on your computer.

First, compile merge_baronies.c , it's self-contained, using only the C standard library.
No special flags or settings needed. Tested on GCC, but should work with MSVC++ as well.

Second, make sure you have all the needed files in this exact folder, you need:
00_landed_titles.txt (from game/common/landed_titles)
definition.csv (from game/map_data)
provinces.bmp (from game/map_data, and make sure to convert the format from .png to .bmp)

Make sure the files have these exact names, things will break if they don't.
The conversion to bitmap can be done in many ways, using MS-Paint for example.

Third, run the merge_baronies.py script, this will produce two temporary files, named:
definition.tmp (Which is the definition.csv file, with the comments removed.)
merge_baronies.tmp (Which lists all counties, the number of baronies in them, and the
	numerical IDs of those baronies.
Both of these files are needed by the next step, and will be removed by it.

Fourth, run the program compiled in step 1, this should produce two output files:
output.bmp (This is the map with the counties.)
output.csv (This is the CSV file to go along with it.)

The ID's in the output.csv and the colour used are taken from the *last* barony listed
in landed titles.


