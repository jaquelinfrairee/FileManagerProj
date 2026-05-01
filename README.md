# File Manager P3

## Author
Jaquelin Fraire

## Description
This project is a simple File Manager application built using C# Windows Forms. It demonstrates how to perform file system operations using the System.IO library through a graphical user interface.

## Features
- Create new files
- Read and display file contents
- Update file contents
- Delete files and folders (with confirmation)
- Rename files and folders
- Copy files to another folder
- Move files between folders
- Create new folders
- Navigate directories (forward and back)
- Display file metadata (size and last modified time)
- Error handling for invalid actions

## Project Structure
- Form1.cs: Handles the GUI and user interactions
- FileOperations.cs: Handles all file system operations (logic)

## Technologies Used
- C#
- Windows Forms
- System.IO

## How to Run
1. Open the project in Visual Studio
2. Build and run the application
3. The program will create a working directory on your Desktop named:
   FileManagerTest
4. Use the buttons to perform file operations

## Notes
- The application handles errors such as missing files, permission issues, and files in use
- Deleting files or folders requires confirmation to prevent accidental loss
- Navigation is done by double-clicking folders and using the Back button

## Purpose
This project was created for CS 3502 to demonstrate:
- File system operations
- GUI development
- Separation of concerns (UI vs logic)
- Error handling
