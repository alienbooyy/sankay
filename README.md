# Sankay - Windows Forms Application

A basic Windows Forms (WinForms) project compatible with Visual Studio 2026.

## Project Structure

- **MainForm.cs**: The main form that acts as the entry point for the application
  - Contains a Label displaying "Welcome to the System!"
  - Contains a Button labeled "Click Me" that shows a MessageBox with "System is working."
  
- **Program.cs**: Contains the Main() method to start and launch MainForm.cs

## Requirements

- Visual Studio 2026 or later
- .NET 10.0 SDK or later
- Windows operating system

## How to Build and Run

1. Open `Sankay.sln` in Visual Studio 2026
2. Press `F5` to build and run the application

Alternatively, you can build from the command line:
```bash
dotnet build
dotnet run --project SankayProject
```

## Project Details

- **Solution Name**: Sankay
- **Project Name**: SankayProject
- **Target Framework**: .NET 10.0 (Windows)
- **Output Type**: Windows Application (WinExe)