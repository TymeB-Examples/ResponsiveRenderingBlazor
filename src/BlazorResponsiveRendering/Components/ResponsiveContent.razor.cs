using BlazorResponsiveRendering.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorResponsiveRendering.Components
{
    public partial class ResponsiveContent
    {
        private bool _jsIsAvailable;
        private bool _failedJsOnFirstRender;
        private Delegate? _currentFragment;
        private BreakPoint _currentBreakPoint;
        private readonly DotNetObjectReference<ResponsiveContent> _dotNetRef;
        private readonly Dictionary<BreakPoint, Delegate?> _breakpoints = [];

        [Parameter] public RenderFragment? Xs { get; set; }
        [Parameter] public RenderFragment? Sm { get; set; }
        [Parameter] public RenderFragment? Md { get; set; }
        [Parameter] public RenderFragment? Lg { get; set; }
        [Parameter] public RenderFragment? Xl { get; set; }
        [Parameter] public RenderFragment? Xxl { get; set; }
        [Parameter] public RenderFragment<BreakPoint> ChildContent { get; set; } = default!;

        public ResponsiveContent()
        {
            _dotNetRef = DotNetObjectReference.Create(this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            InitializeBreakpoints();
            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Still not a better way to check if it is a pre-render or not ...
            if (firstRender || _failedJsOnFirstRender)
            {
                try
                {
                    //Invoke js runtime below
                    await InitializeResponsiveContent();
                    _jsIsAvailable = true;
                }
                catch (Exception)
                {

                    _failedJsOnFirstRender = firstRender;
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task InitializeResponsiveContent()
        {
            await JS.InvokeVoidAsync("window.ResponsiveFragment.registerResizeCallback", _dotNetRef);
            InitializeBreakpoints();

            var width = await JS.InvokeAsync<int>("window.ResponsiveFragment.getWindowWidth");
            UpdateFragment(width);
        }

        private void InitializeBreakpoints()
        {
            _breakpoints.Clear();
            _breakpoints[BreakPoint.Xs] = (Delegate?)Xs ?? (Delegate?)ChildContent;
            _breakpoints[BreakPoint.Sm] = (Delegate?)Sm ?? (Delegate?)ChildContent;
            _breakpoints[BreakPoint.Md] = (Delegate?)Md ?? (Delegate?)ChildContent;
            _breakpoints[BreakPoint.Lg] = (Delegate?)Lg ?? (Delegate?)ChildContent;
            _breakpoints[BreakPoint.Xl] = (Delegate?)Xl ?? (Delegate?)ChildContent;
            _breakpoints[BreakPoint.Xxl] = (Delegate?)Xxl ?? (Delegate?)ChildContent;
        }

        [JSInvokable]
        public void UpdateFragment(int width)
        {
            var candidates = _breakpoints.Keys.Where(bp => (int)bp <= width).OrderDescending();
            foreach (var bp in candidates)
            {
                if (_breakpoints[bp] != null)
                {
                    ReRenderIfNeeded(_breakpoints[bp], bp);
                    break;
                }
            }
        }

        private void ReRenderIfNeeded(Delegate? renderFragment, BreakPoint breakPoint)
        {
            if (ReferenceEquals(_currentFragment, renderFragment) && _currentBreakPoint == breakPoint)
                return;

            _currentBreakPoint = breakPoint;
            _currentFragment = renderFragment;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await JS.InvokeVoidAsync("window.ResponsiveFragment.dispose");

            _dotNetRef.Dispose();
        }
    }
}