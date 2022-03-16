using System;

namespace DigitalSignEditor.Emergency {

    // use this to test the website by injecting this in the startup layer
    public class EmergencyMock {
        private static DateTime triggerEndTime = DateTime.Now.AddMinutes(6);
        private static DateTime triggerStartTime = DateTime.Now.AddMinutes(2);

        public static Alert Emergency() {
            return new Alert {
                Title = "Illini-Alert. Do not cross Wright Street at Daniel or Chalmers while crews address gas leak. Pedestrians may cross Wright Street at Green, John or Armory.",
                Description = "Illini-Alert. Do not cross Wright Street at Daniel or Chalmers while crews address gas leak. Pedestrians may cross Wright Street at Green, John or Armory. Want more ways to be notified by Illini - Alert ? Follow us on Twitter at http://twitter.com/illinialert and facebook at http://www.facebook.com/illinialert."
            };
        }

        public static Alert NoEmergency() {
            return new Alert();
        }

        public static Alert TimedEmergency() {
            return (DateTime.Now > triggerStartTime && DateTime.Now < triggerEndTime) ? Emergency() : NoEmergency();
        }
    }
}