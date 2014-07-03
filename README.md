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

See [Wiki](https://github.com/rcmaniac25/DotBlackBerry/wiki/Home)

Todo
========

1. Get some of the base-level APIs avaliable such as BPS and screen
2. Get OpenTK to work with MonoBerry (the avaliable versions are behind in version and don't support GLES 3.0)
3. NuGet packages APIs
4. Cascades and maybe Xamerin.Forms
5. Try to get additional languages working, such as F#
6. Improve/update Visual Studio plugin, especially reducing or eliminating initial Wizard...
7. Debugging. Not entirly sure how this would work, but it would be very useful to have.
