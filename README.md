<h1>
<img src="https://github.com/code-dispenser.png" align="center" height="60px" alt="code-dispenser icon" /> code-dispenser
</h1>

This repository contains the solution from my YouTube video showing how to build an accessible accordion component following the ARIA authoring practice guidelines for the accordion pattern.

YouTube video: https://youtu.be/iR2QjqWBmRc

Coffees welcomed : https://ko-fi.com/codedispenser

My Visual Studio (VS 2022) environment has the Web Compiler 2022+ extension (by Jason Moore) installed for compiling the Sassy CSS (SCSS) files.

https://marketplace.visualstudio.com/items?itemName=Failwyn.WebCompiler64

https://github.com/failwyn/WebCompiler

**Note:** Fixed typo in the _accordion.scss partial file.

I wrote:
```css
& > h1, h2, h3, h4, h5, h6 {/* ... */}

/* Instead of writing */

& > h1, > h2, > h3, > h4, > h5, > h6 {/* ... */}

```
My intention was to only target h tags that are direct descendants of the accordion not all h tags contained anywhere in the accordion.
The typo does not effect the component, code, functionality or style of the demo content.
