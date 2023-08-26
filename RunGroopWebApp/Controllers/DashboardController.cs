using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVm, ImageUploadResult photoResult)
        {
            user.Id = editVm.Id;
            user.Pace = editVm.Pace;
            user.Mileage = editVm.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVm.City;
            user.State = editVm.State;
        }
        public async Task<IActionResult>  Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");

            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVm);
            }

            AppUser user = await _dashboardRepository.GetByIdNoTracking(editVm.Id);

            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = await _photoService.AddPhotoAsync(editVm.Image);
                MapUserEdit(user,editVm,photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVm);
                }

                var photoResult = await _photoService.AddPhotoAsync(editVm.Image);
                MapUserEdit(user, editVm, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }
    }
}
