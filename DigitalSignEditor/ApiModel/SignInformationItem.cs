using System;

namespace DigitalSignEditor.ApiModel {

    public class SignInformationItem {
        public string Data { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string SignOption { get; set; }
        public DateTime? StartDate { get; set; }
    }
}