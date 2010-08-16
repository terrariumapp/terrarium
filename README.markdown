Terrarium Project
=================

This is the README file for the Terrarium Project hosted on [CodePlex][codeplex_project_site].

Project Structure
-----------------

The project is layed out in a series of folders and supports both the Visual Studio 2008 (.NET 2.0)
and Visual Studio 2010 (.NET 4.0) versions. 

The major differences between the VS2008 and VS2010 versions are the client being WinForms vs. WPF 
based and the server being WebForms vs. MVC based.

C:.
+---Client					# old VS2008/.NET 2.0 client
¦   +---AsmCheck
¦   +---Configuration
¦   +---Controls
¦   +---Controls2
¦   +---ControlsWPF			# abandoned WPF client for .NET 2.0
¦   +---DXVBLib
¦   +---Game
¦   +---Glass
¦   +---Graphics
¦   +---HttpListener
¦   +---OrganismBase
¦   +---Renderer
¦   +---Services
¦   +---Setup
¦   +---Terrarium			# original WinForms 2.0 client
¦   +---Terrarium2			# abandoned client project for .NET 4.0
¦   +---TerrariumWPF		# abandoned WPF client for .NET 2.0
+---ClientWPF				# new .NET 4.0 clients
¦   +---TerrariumClient		# net .NET 4.0 WPF client
+---Keys
+---Samples
+---SDK
+---SDK2
+---Server					# original WebForms server and services
+---ServerMVC				# new .NET 4.0 MVC server
¦   +---TerrariumServer
¦   +---TerrariumServer.Tests
+---Tools
    +---ServerConfig
    +---StyleEditor

[codeplex_project_site]: http://terrarium2.codeplex.com/