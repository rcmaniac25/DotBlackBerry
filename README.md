.BlackBerry
========

BlackBerry 10's API, in an easy to access .Net/Mono form.

Background
========

BlackBerry 10 is a great development platform, as there are few restrictions on HOW you can develop, making it easier to make great applications.

Ok, maybe one big one, there is no support for .Net/Mono.

* C/C++: [Check](http://developer.blackberry.com/native/)
* Java (Android version): [Check](http://developer.blackberry.com/android/)
* HTML5: [Check](http://developer.blackberry.com/html5/)
* Adobe Air: [Check](http://developer.blackberry.com/air/)*
* Python: [Check](http://blackberry-py.microcode.ca/)**  

.Net/Mono: Missing

Luckily, a project by the name of [MonoBerry](https://github.com/roblillack/monoberry) came around that got the Mono runtime working on BlackBerry 10. As of this writing, the 0.2.0 release of MonoBerry includes support for Mono 3.2.4 which supports .Net 4.5. But it too has a problem, it doesn't work well with the .Net/Mono workflow, and expects you are on a *Nix based system. Luckily, @gatm50 produced NuGet packages and actual [Visual Studio addins](https://github.com/gatm50/MonoberryToolsForVisualStudio) to make it a little more [seamless](https://github.com/roblillack/monoberry/releases). But there is still one more issue... both developers have an interop library to access .Net/Mono features, but it barely covers BPS, let alone the many additional libraries and APIs that BlackBerry 10 supports.

That's where this comes in. Just as MonoBerry covers the Mono runtime, and gatm50 made it easier to interface with Visual Studio, this is designed to be the proper interface for a .Net/Mono application to access BlackBerry 10's internals. Will it be perfect? Probably not. Mono, OpenTK, and many others started small with barely any big-time users, but grew with time.

In addition, given that MonoBerry isn't locked to the BlackBerry 10 OS so much as it's underlying QNX kernel, MonoBerry apps should run on [QNX Car Platform](http://www.qnx.com/products/qnxcar/index.html) and maybe even BlackBerry [Project Ion](http://el.blackberry.com/project-ion) (pending actual release of API).

*Though lack of usage means they are removing support for it in 10.3.1  
**Not offically supported, but offically maintained and included along with actual interfaces to use it

Usage
========

Right now, usage isn't the best.

Requirements:
* Mono (as you need it's runtime libs). Keep the version close to 3.2.4 to make sure the runtime isn't out of date with the BCL, potentially causing issues later on.
* Download the latest [Release](https://github.com/roblillack/monoberry/releases) from MonoBerry
* Install [MonoBerry tools for Visual Studio](http://visualstudiogallery.msdn.microsoft.com/b4803586-b446-4df1-8254-978f00ceb52d) (Visual Studio 2012 only right now).

Steps:  
1. [Create a MonoBerry project](http://cup-coffe.blogspot.com/2013/06/hello-world-with-monoberry.html), steps 1 and 2, though you don't need to setup the Simulator. Change the Target Framework to .Net 4.5.
2. Get .BlackBerry. Right now, that means building the libs. Later it would mean using `Install-Package`
3. Modify monoberry-descriptor.xml to point to the proper libraries. (see below)
4. Follow [step 5](http://cup-coffe.blogspot.com/2013/06/hello-world-with-monoberry.html)

monoberry-descriptor changes
--------

It is expected that you have Mono installed and have downloaded MonoBerry.

Section #2: Check the executable/libraries that you have created, specifically it's dependencies. List each runtime dependency (as opposed to downloaded packages):
* For mscorlib, link to {MonoBerry}/target/lib/mscorlib.dll.
* For all other libs, link to the Mono lib (for .Net 4.5) for the source.
* Change all link references from 4.0 to 4.5 (since the build should be for .Net 4.5)  

Section #3: Change the runtime links:
* mono should point to {MonoBerry}/target/target/armle-v7/bin/mono
* libgdiplus.so.1 should point to {MonoBerry}/target/target/armle-v7/lib/libgdiplus.so.1  

Section #4: Change resources and binaries. While it currently points to your application's Debug directory, if you are missing additional dependencies, you may need to add additional asset copies.

Todo
========

1. Get some of the base-level APIs avaliable such as BPS and screen
2. Get OpenTK to work with MonoBerry (the avaliable versions are behind in version and don't support GLES 3.0)
3. NuGet packages APIs
4. Cascades and maybe Xamerin.Forms
5. Try to get additional languages working, such as F#
6. Improve/update Visual Studio plugin, especially reducing or eliminating initial Wizard...
7. Debugging. Not entirly sure how this would work, but it would be very useful to have.
