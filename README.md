# ResponsiveRenderingBlazor
## Overview

This repository demonstrates responsive rendering techniques in Blazor, enabling components to adapt their layout and content dynamically based on the device screen size or rendering context.

Using Blazor concepts like `RenderFragment` and `RenderContext`, this example shows how to build performant, flexible UI components that seamlessly respond to varying viewport sizes.

The current example uses the same breakpoints as [Bootstrap](https://getbootstrap.com/docs/5.0/layout/breakpoints/). But you can extend this to your liking, you can also use an `IList<IBreakPoint> Breakpoints {get; set;}` for example and render it based on those (or however you like).

---

## Features

- Adaptive UI rendering using Blazor’s `RenderFragment` pattern  
- Context-aware rendering based on screen size or device characteristics (Prerender uses CSS and default rendering listens to JS event)
- Fallback strategies for both prerender as well as default rendering.

---
