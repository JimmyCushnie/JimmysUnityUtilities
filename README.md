# Jimmy's Unity Utilities
This repository contains a bunch of code that I like to have on hand when working in Unity. Included is:

* `Color24` struct, which is like Unity's `Color32` but without a transparency byte
* `ScriptableObjectSingleton<T>` class, for creating scriptable object singletons
* `ObjectPoolUtility<T>` class for creating object pools
* `FileUtilities` class with helpful functions for working with the file system
* `ImageUtility` class for saving and loading images to disk
* `CoroutineUtility` class for running coroutines from disabled gameobjects or static methods
* `WaitForSecondsPrecise`, a version of Unity's `WaitForSeconds` that doesn't lose accuracy with repeated usage
* `ClipboardAccess` class which allows you to set the contents of the system clipboard
* `SimpleRotation` component for rotating a  `GameObject` and a constant speed
* a crapload of extension methods both for Unity types and C# types
