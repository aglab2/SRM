# SRM Source
This tutorial covers managing the SRM source [MEGA](https://mega.nz/folder/2DYz2IIa#E9QMa2IlnM5XrPQF-6J2sg) files and describes how to create MEGA documents for a particular marathon.

The folders without "-source" suffix are files that can be used directly without any procedures as they do not require any scripting. Other files require processing to be useful.

## Flags
TODO: Describe how to use GIMP+BIMP to convert files

## Info
TODO: Describe the fields in info file

## Layouts
While 'layouts-source' folder exists, just using 'layouts' being recolored should work for most applications. If a particular new layout is needed, layout-source contains the GIMP projects that can be edited.

## Music
TODO: Describe foobar2000 conversion

## Hack images
'slideshow-source' folder contains all the source images and logos for the images in 'slideshow' folder. The script 'build.sh' is using [imagemagic](https://imagemagick.org/index.php) to merge those together and output the results.
If a new hack needs to be added, follow the steps. GIMP is required.
* Create a hack logo
* * Take screenshot of the logo.
* * Remove the black parts around the logo using 'Fuzzy Select Tool'.
* * Scale down the logo so width is roughly 350-450 units, it heavily depends on the details logo have.
* * Add the shadow to the logo using 'Filters > Light and Shadow > Drop Shadow...'. Set X & Y to 0.
* * Save the file to 'hack.xcf' in '\_LOGOS' folder.
* * Export 'xcf' to 'png' (Export As...) in 'logo' folder in the SRM-Source root.
* Take screenshots of the gameplay
* * All screenshots must not of size 1280x624. 'build.sh' will validate this.
* * Take with any 16:9 resolution and apply Gaussian Blur (Filters > Blur > Gaussian Blur). Save it to 'gamepic' folder in the SRM-Source root.
* Execute 'build.sh' using 'cmder' which will merge the logos and screenshots.

## TODO
* Opensource 'ImageSlideshow2.exe' & 'TitleUpdaterConsole.exe'
