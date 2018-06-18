# SonarWiper
You can opt-out collecting usage statistics of every Adobe product (for example Adobe Photoshop) using SonarWiper.

## What we know (about Sonar folder):

- It is located at path `%AppData%\Adobe\Sonar`.
- Sonar folder contains XML file for each installed Adobe product that is runned at least once.
- Inside each XML file is stored timestamp when the first start of application was made by user and frequency of use (every each start and end of the application + number of sessions performed in application).

## About wipe modes
SonarWiper have two modes of getting rid of Adobe's product usage statistics:

1. Normal mode
2. Automated mode

### Normal mode
Normal mode is executed when you run `sonarwiper.exe` without any arguments. SonarWiper just recursively deletes your Sonar folder inside `%AppData%\Adobe\` folder and that is all.

This mode is useful, if you are making more advanced application for managing Adobe's files and apps or you just want a one-time wipe tool that you can run on demand.

### Automated mode
Automated mode is more advanced and it is executed when you run `sonarwiper.exe` with `/stay` (or `-stay`) argument. After the start, SonarWiper recoursively deletes Sonar folder and run a FileSystemWatcher to detect every changes made inside `%AppData%\Adobe\` folder. Whenever the Sonar folder and its subcontent is created or changed, SonarWiper sets new Timer that recursively removes Sonar folder when 50 ms was elapsed.

This mode is useful, if you want to opt-out Adobe's product usage statistics for a long time using AutoRun or Sheduled Tasks.

## How to opt-out in a 3 steps

- [ ] **1a)** Download the latest relese.
- [ ] **1b)** Clone this repo and compile C# code located inside `project` folder (using Visual Studio or alternative .NET IDE or using only the C# compiler).
- [ ] **2)** Run downloaded/compiled executable called `sonarwiper.exe` with `/stay` argument.
- [ ] **3)** Now SonarWiper is watching Sonar folder and each time statistics are created and written, the program will delete Sonar folder immediately (after short 50 ms period of inactivity of course).

### Alternatively...
You can also create a sheduled task, that will run `sonarwiper.exe` with `/stay` argument every time a certain user is logged in. It is more permanent way to opt-out.
