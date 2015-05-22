I wanted a simple console app that I could set to run on windows user login/unlock (using windows task scheduler) which would capture a timestamped image from the webcam and exit.

---

Download compiled version here, ready to run (includes all necessary libraries):

https://camcap.googlecode.com/svn/deploy/camcap_20141205.zip

(Updated 12/5, initial 12/4 build was against wrong source code version. Thanks for the heads-up, reddit.)

basic setup instructions here:

https://code.google.com/p/camcap/wiki/SetupTutorial


---

Uses the AForge image libraries found at: https://code.google.com/p/aforge/

Credit for a lot of code goes to various posts and approaches in: http://www.codeproject.com/Questions/456777/Capturing-image-from-a-webcam


---

Usage:

_camcap_
Captures image to file with current date/time: 20130830234730.jpg

Options:

[-f filename] _output to specified filename.jpg, or if filename is a directory, with default name in that directory_

[-l logfile] _output a log file of problems and results_

[-s WIDTHxHEIGHT] _specify width/height of output_

[-d milliseconds] _delay for specified number of milliseconds when initializing camera_


---

Example Execution
```
camcap.exe -f C:\cap\
```