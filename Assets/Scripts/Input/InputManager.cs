namespace WizardParty.Input
{
    public static class InputManager
    {
        #region Properties
        public static WizardPartyControls Controls => _controls;
        #endregion
        #region Class Data
        private static readonly WizardPartyControls _controls = new();
        #endregion
        #region Constructors
        static InputManager()
        {
            _controls.Enable();
        }
        #endregion
    }
}
