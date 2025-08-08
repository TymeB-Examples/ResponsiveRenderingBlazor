# ResponsiveRenderingBlazor
An example to show how to render RenderFragment(s) in a responsive way.

This example works with InteractiveServerRending as well as InteractiveServerRending with prerender.
We fall back to the next lower renderfragment or ChildContent if it does not exists. 
This way we always render something that fits within the screen.
