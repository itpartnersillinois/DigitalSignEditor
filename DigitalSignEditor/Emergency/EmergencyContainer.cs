using System;

namespace DigitalSignEditor.Emergency {

    public class EmergencyContainer {
        private Func<Alert> action;

        public EmergencyContainer(Func<Alert> action) {
            this.action = action;
        }

        public Alert Get() {
            return this.action();
        }
    }
}