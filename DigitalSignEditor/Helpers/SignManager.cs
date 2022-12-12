using System;
using System.Linq;
using System.Text;
using DigitalSignEditor.Data;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubExport;
using DigitalSignEditor.GithubModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalSignEditor.Helpers {

    public static class SignManager {

        public static int DeleteSlides(ISignRepository signRepository) {
            var slides = signRepository.Read(s => s.Slides).Where(slide => slide.EndDate < DateTime.Now && slide.IsActive).ToList();
            var returnValue = slides.Count;
            if (slides.Any()) {
                ChangedHelper.SignChanged(signRepository, null, null);
                foreach (var slide in slides) {
                    _ = signRepository.Delete(slide);
                }
            }
            return returnValue;
        }

        public static string RunCommit(IFileCreatorFactory fileCreatorFactory) {
            return fileCreatorFactory.Create().CommitCheck() ? "Commit cleanup successful" : "";
        }

        public static string TransferSlides(ISignRepository signRepository, IFileCreatorFactory fileCreatorFactory) {
            var returnValue = new StringBuilder();
            foreach (SignType signType in Enum.GetValues(typeof(SignType))) {
                var signs = signRepository.Read(sr => sr.Signs.Include(s => s.Slides).Where(s => s.SignType == signType).ToList());
                _ = returnValue.Append(" Slide " + signType.ToString() + ". ");
                foreach (var slide in signs.SelectMany(s => s.Slides).Where(slide => slide.StorageItemId.HasValue)) {
                    var signUrl = signs.First(s => s.Id == slide.SignId).Url;
                    var file = signRepository.Read(sr => sr.StorageItems.FirstOrDefault(s => s.Id == slide.StorageItemId));
                    _ = fileCreatorFactory.Create().CreateImageFiles(signUrl, file.Name, file.Data);
                    slide.AssignUrl(fileCreatorFactory.Create().GetUrl(signUrl, file.Name),
                        fileCreatorFactory.Create().GetFullUrl(signUrl, file.Name),
                        fileCreatorFactory.Create().GetCompressedUrl(signUrl, file.Name));
                    _ = signRepository.Update(slide);
                    _ = signRepository.Delete(file);
                    returnValue.Append(" - " + file.Name);
                }
                _ = fileCreatorFactory.Create().CreateSharedDataFile(signType.ToString(), JsonConvert.SerializeObject(signs.Select(sign => new GithubSign(sign))));
                _ = returnValue.Append(" - Shared file ");
                foreach (var sign in signs) {
                    _ = fileCreatorFactory.Create().CreateDataFile(sign.Url, JsonConvert.SerializeObject(new GithubSign(sign)));
                    _ = returnValue.Append(" - " + sign.Url);
                }
            }
            _ = fileCreatorFactory.Create().Commit();
            _ = returnValue.Append(" - Commit.");
            return returnValue.ToString();
        }
    }
}