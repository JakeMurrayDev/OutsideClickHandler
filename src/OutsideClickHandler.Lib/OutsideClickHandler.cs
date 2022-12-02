using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace OutsideClickHandler.Component
{
    public sealed class OutsideClickHandler : ComponentBase, IAsyncDisposable
    {
        bool _initialized = false;
        DotNetObjectReference<OutsideClickHandler> _objectReference = default!;
        int _objectIndex = -1;

        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// The content to which the value should be provided.
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Accepts void function to be called on outside click.
        /// </summary>
        [Parameter] public Action? OnOutsideClick { get; set; }

        /// <summary>
        /// Accepts awaitable function to be called on outside click.
        /// </summary>
        [Parameter] public Func<Task>? OnOutsideClickTask { get; set; }

        /// <summary>
        /// Stops <c>OnOutsideClick</c> and <c>OnOutsideClickTask</c> from executing when <see langword="true"/>.
        /// </summary>
        [Parameter] public bool StopPropagation { get; set; }

        /// <summary>
        /// Gets or sets the associated <see cref="ElementReference"/>.
        /// <para>
        /// May be <see langword="null"/> if accessed before the component is rendered.
        /// </para>
        /// </summary>
        [DisallowNull] public ElementReference? Element { get; private set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddElementReferenceCapture(2, __element => Element = __element);
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _objectReference = DotNetObjectReference.Create(this);
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!_initialized)
                {
                    _initialized = true;
                    await AddOutsideClick();
                }
            }
        }

        /// <summary>
        /// Function to be invoked in javascript.
        /// </summary>
        [JSInvokable]
        public async Task InvokeOutsideClick()
        {
            if (StopPropagation)
            {
                _initialized = false;
                await RemoveOutsideClick();
                return;
            }

            if (OnOutsideClick != null)
            {
                OnOutsideClick.Invoke();
            }

            if (OnOutsideClickTask != null)
            {
                await OnOutsideClickTask.Invoke();
            }
        }

        private async Task AddOutsideClick()
        {
            _objectIndex = await JSRuntime.InvokeAsync<int>("addOutsideClickEvent", Element!, _objectReference);
        }

        private async Task RemoveOutsideClick()
        {
            await JSRuntime.InvokeVoidAsync("removeOutsideClickEvent", _objectIndex);
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await RemoveOutsideClick();
            _objectReference.Dispose();
        }
    }
}
