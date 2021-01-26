# Jimmy's Unity Utilities
This repository contains a bunch of code that I like to have on hand when working in Unity, although large chunks of the code aren't Unity-specific and can be used in any .NET application.

I've released everything in here as public domain, so feel free to rip out anything you find useful and use it in your own projects.

## Installation

You can install JUU using the Unity Package Manager. In the top left of the UPM window, hit the "plus" button and click "add package from git URL". Then, enter `https://github.com/JimmyCushnie/JimmysUnityUtilities.git`.

You can also just download/clone the repo and stick it in your project's `Assets` or `Packages` folder.

Note: I update JUU frequently and I'm not shy about breaking APIs, so if you want a stable version you should probably fork it.

## What's included?

By the time you're reading this list, it's probably outdated, but here's what's in JUU at time of writing:

* Hundreds of helpful extension methods for both Unity types and .NET types

* `Color24` struct, which is like Unity's `Color32` but without a transparency byte
* `ObjectPoolUtility<T>` and `TrackedObjectPoolUtility<T>` classes for creating object pools
* `ScriptableObjectSingleton<T>` class, for creating scriptable object singletons
* `NetworkPinger` class for testing connectivity to servers. [Documentation here](https://github.com/JimmyCushnie/JimmysUnityUtilities/wiki/NetworkPinger)
* `LockedList`  and `LockedHashSet`, thread-safe wrappers for `List<T>` and `HashSet<T>` 
* `AudioLoadingUtilities` for streaming or loading audio files on disk
* `ImageUtility` class for saving and loading images on disk
* `FileUtilities` class with various helpful functions for working with the file system
* `CoroutineUtility` class for running coroutines from places they're usually not allowed: disabled gameobjects, static methods, code not on the main thread
* `CryptographyUtility` for hashing strings
* `SceneUtilities` class for working with multi-scene projects
* `WaitForSecondsPrecise`, a version of Unity's `WaitForSeconds` that doesn't lose accuracy with repeated usage
* `ClipboardAccess` class which allows you to set the contents of the system clipboard
* `SimpleRotation` component for rotating a  `GameObject` and a constant speed
* `CryptographyUtility` class to quickly hash strings
* `CustomFixedUpdate` -- like Unity's FixedUpdate, but you can have many of them, and they can all have different tickrates independant from the physics simulation
* `Dispatcher` for calling code on the main Unity thread from code on other threads
* `JRandom` class; like `System.Random` but with many more methods for getting random values
* `NetworkUtilities` class; can be used to get an available network port, and to parse IP endpoints provided by users in string form
* `TerrainTextureDetector` component for detecting the dominant texture at a position on a Unity Terrain object
* `VisibilityDetector` component for detecting when an object is in view of a Camera
* `PhysicsUtilities` class to get collision mask of a physics layer
* `ResourceUtilities` class to a text asset in `Resources` as a string

