﻿using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using X.PagedList;

namespace HR_Project.Presentation.Controllers
{
    public class AdvanceController : BaseController
    {
        private readonly IAPIService _apiService;

        public AdvanceController(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(searchText))
            {

                List<AdvanceVM> advances = await _apiService.GetAsync<List<AdvanceVM>>("advance", HttpContext.Request.Cookies["access-token"]);
                List<AdvanceVM> selectedAdvences = advances.Where(x => x.Reason.ToLower().Contains(searchText.ToLower()) || x.Amount.ToString().Contains(searchText)).ToList();

                return View(selectedAdvences.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<AdvanceVM> advances = await _apiService.GetAsync<List<AdvanceVM>>("advance", HttpContext.Request.Cookies["access-token"]);
                return View(advances.ToPagedList(pageNumber, pageSize));
            }

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvanceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _apiService.PostAsync<CreateAdvanceDTO, CreateAdvanceDTO>("advance", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
        }


        public async Task<IActionResult> Update(string id)
        {
            UpdateAdvanceDTO advance = await _apiService.GetByIdAsync<UpdateAdvanceDTO>("advance/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(advance);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAdvanceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateAdvanceDTO>("advance", model, HttpContext.Request.Cookies["access-token"]);

                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncelleme sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<UpdateAdvanceDTO>($"advance", id, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde silindi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt silme sırasında hata oluştu : {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}
