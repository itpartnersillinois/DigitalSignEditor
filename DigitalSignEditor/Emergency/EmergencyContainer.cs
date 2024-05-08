using System;

namespace DigitalSignEditor.Emergency {

    public class EmergencyContainer {
        private readonly Func<Alert> _action;
        private Alert _alert;

        public EmergencyContainer(Func<Alert> action) {
            _action = action;
            _alert = new();
        }

        public Alert Get() {
            if (!_alert.IsSafe || DateTime.Now.Subtract(_alert.LastUpdated).TotalMinutes > 1) {
                _alert = _action();
            }
            return _alert;
        }
    }
}