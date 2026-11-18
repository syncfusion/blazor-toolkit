namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfChip
    {
        #region LifeCycle Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            WarnNavigation();
            SyncRegistration();
        }

        private void SyncRegistration()
        {
            if (Owner is null)
            {
                return;
            }

            Owner.Register(new DeclarativeChip
            {
                Id = RootId,
                Key = Value ?? Text ?? RootId,
                Value = Value ?? Text,
                Text = Text,
                Disabled = Disabled,
                Removable = Removable,
                Chip = this,
            });
            _registered = true;
        }

        /// <inheritdoc />
        protected override async ValueTask DisposeAsyncCore()
        {
            if (_registered && Owner is not null)
            {
                Owner.Unregister(RootId);
            }

            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion
    }
}
