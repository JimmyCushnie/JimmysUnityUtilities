# Jimmy's Unity Utilities
This repository contains a bunch of code that I like to have on hand when working in Unity. Included is:

* `Color24` struct, which is like Unity's `Color32` but without a transparency byte
* `ScriptableObjectSingleton<T>` class, for creating scriptable object singletons
* `ObjectPoolUtility<T>` class for creating object pools
* `FileUtilities` class with helpful functions for working with the file system
* `ImageUtility` class for saving and loading images to disk
* `CoroutineUtility` class for running coroutines from disabled gameobjects or static methods
* `SceneUtilities` class for working with multi-scene projects
* `WaitForSecondsPrecise`, a version of Unity's `WaitForSeconds` that doesn't lose accuracy with repeated usage
* `ClipboardAccess` class which allows you to set the contents of the system clipboard
* `SimpleRotation` component for rotating a  `GameObject` and a constant speed
* `CryptographyUtility` class to quickly hash strings
* `CustomFixedUpdate` -- like Unity's FixedUpdate, but you can have many of them, and they can all have different tickrates independant from the physics simulation
* `Dispatcher` for ease of writing threaded code
* `JRandom` class; like `System.Random` but with many more methods for getting random values
* `NetworkUtilities` class (currently only to get an available network port)
* `PhysicsUtilities` class (currently only to get collision mask of a physics layer)
* `ResourceUtilities` class (currently only for getting a text asset in `Resources` as a string)
* about a billion extension methods both for Unity types and C# types
