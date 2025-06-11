// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sinhvien.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager; // Thêm UserManager
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager, // Inject UserManager
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager; // Gán UserManager
            _logger = logger;
        }

        /// <summary>
        /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Thuộc tính MaSV để đăng nhập
            /// </summary>
            [Required(ErrorMessage = "Mã sinh viên là bắt buộc.")]
            [StringLength(10, MinimumLength = 5, ErrorMessage = "Mã sinh viên phải có từ 5 đến 10 ký tự.")] // Thêm validation tùy ý
            [Display(Name = "Mã sinh viên")] // Thay đổi hiển thị
            public string MaSV { get; set; } // Đổi từ Email sang MaSV

            /// <summary>
            /// Đây là API hỗ trợ cơ sở hạ tầng giao diện người dùng mặc định của ASP.NET Core Identity và không nhằm mục đích sử dụng trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các bản phát hành tương lai.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Xóa cookie bên ngoài hiện có để đảm bảo quá trình đăng nhập sạch sẽ
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Sử dụng MaSV làm UserName
                var user = await _userManager.FindByNameAsync(Input.MaSV);

                if (user == null)
                {
                    // Nếu user chưa tồn tại trong Identity, tạo mới
                    // KHÔNG có mật khẩu, chỉ dựa vào MaSV
                    user = new IdentityUser { UserName = Input.MaSV, Email = Input.MaSV + "@example.com" }; // Gán Email placeholder
                    var createResult = await _userManager.CreateAsync(user);

                    if (!createResult.Succeeded)
                    {
                        // Xử lý lỗi nếu không thể tạo user (ví dụ: validation thất bại)
                        foreach (var error in createResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        _logger.LogError($"Failed to create IdentityUser for MaSV: {Input.MaSV}. Errors: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        return Page();
                    }
                    _logger.LogInformation($"IdentityUser created for MaSV: {Input.MaSV}");
                }

                // Đăng nhập người dùng mà không cần mật khẩu
                // Đây là điểm cốt lõi của việc bỏ qua xác thực mật khẩu.
                // Lưu ý: Succeeded trong trường hợp này luôn là true nếu user tồn tại.
                await _signInManager.SignInAsync(user, isPersistent: Input.RememberMe);

                _logger.LogInformation($"User '{Input.MaSV}' logged in successfully using MaSV.");
                return LocalRedirect(returnUrl);
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form
            return Page();
        }
    }
}
