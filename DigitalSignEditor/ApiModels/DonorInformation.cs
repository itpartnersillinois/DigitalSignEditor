using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.ApiModels {

    public class DonorInformation {

        public DonorInformation() {
        }

        public DonorInformation(Donor donor, bool UseExternal) {
            Name = donor.Name;
            Id = donor.Id;
            PersonName = donor.PersonName;
            Description = UseExternal ? donor.Html : donor.Description;
            ImageUrl = donor.ImageUrl;
            IsActive = donor.IsActive;
        }

        public string Description { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string PersonName { get; set; }
    }
}