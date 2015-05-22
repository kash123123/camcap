# Requirements #

To run this you will need .NET framework, version 4.0. If you're up to date on Windows Updates, you probably have it already.

You can download the ready-to-run binary package from here:

https://camcap.googlecode.com/svn/deploy/camcap_20141205.zip

(UPDATED 12/5, initial release was built on wrong source code).

This deployment contains the libs necessary to run the application.

OR, for anyone who doesn't trust the code (why wouldn't you trust a program that secretly captures from your webcam?) you can pull the code from svn in the "Source" tab link above. If you are going to compile from source, get the AForge libraries from the project linked on the main page.


# Details #

1) Extract files, or compile source, and put somewhere on your machine.

2) Find your "Task Scheduler" in Windows (sometimes listed as "Scheduled Tasks") it could be under your Control Panel, or Control Panel\Administrative tools. If you're on Windows 7+, just press the start key and start typing "schedule" and you'll see it.

2) Right click the root library (or create your own folder) and select "Create Basic Task" or "Create Task". The look of the screens varies with operating system, but you basically want it to execute:

camcap.exe -f c:\somefolder\

as a command line.

for trigger events, you can use Windows Logon, or similar.

3) After creating your task, you scan test it by right clicking it and running it. You should see the webcam light come on, and an image appear in the directory you specified.


# Common Problems #
1) Program seems to be overwriting the same file, when I wanted a directory.

> - Make sure there is a backslash at the end of the -f switch when specifying a directory.


2) Running as scheduled task doesn't seem to be writing the photo anywhere.

- If you are not specifying a -f command, it will write the photo wherever the "initial" directory is for the task. Under the Action it is executing, below the arguments to the .exe, there is a "Start in (optional):" to specify the starting directory. If you put a directory in here, with no -f switch in the arguments directory, it will write to that working folder.

- In the task scheduler properties for your task, there are some options for who the task runs as, and with what privileges. Mine is set to run as "SYSTEM" with "Run whether user is logged on or not", and it has "Run with the highest privileges" checked. I also have the "Hidden" checkbox checked. (I am using Windows 8, this may vary with different versions of Windows)