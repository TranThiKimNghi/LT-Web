using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sinhvien.Models; // Đảm bảo bạn có các models và DbContext ở đây

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Session (giữ nguyên, có vẻ không phải vấn đề chính)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm dịch vụ cho MVC Controllers với Views
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Dòng này thường không cần thiết và có thể gây xung đột nếu bạn không có cấu trúc view đặc biệt.
        // Tôi sẽ giữ lại nó như bạn đã có, nhưng hãy lưu ý.
        // options.ViewLocationFormats.Add("/{0}.cshtml");
    });


// Cấu hình DbContext (Đảm bảo chuỗi kết nối DefaultConnection trong appsettings.json là đúng)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity (SỬA LỖI Ở ĐÂY: Tham số generic phải là kiểu User, không phải DbContext)
// Giả sử bạn đang dùng IdentityUser mặc định. Nếu bạn có ApplicationUser riêng, hãy thay thế.
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Tùy chỉnh mật khẩu, khóa, v.v nếu muốn ở đây
})
.AddRoles<IdentityRole>() // Thêm hỗ trợ Role nếu bạn đang sử dụng vai trò
.AddEntityFrameworkStores<ApplicationDbContext>();

// Cấu hình cookie của ứng dụng (giữ nguyên)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Đăng ký tất cả các Repositories (Đã THÊM các repositories còn thiếu)
// Đảm bảo bạn có các interface và implementation tương ứng trong thư mục Models hoặc Data
builder.Services.AddScoped<ISinhVienRepository, EFSinhVienRepository>();
builder.Services.AddScoped<IHocPhanRepository, EFHocPhanRepository>();

var app = builder.Build();

// Cấu hình HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Cho phép truy cập các file tĩnh (CSS, JS, images)
app.UseSession();     // Sử dụng Session
app.UseRouting();     // Cho phép định tuyến

app.UseAuthentication(); // Bật xác thực
app.UseAuthorization();  // Bật phân quyền

app.MapRazorPages(); // Kích hoạt Razor Pages (cho Identity UI)

// Cấu hình định tuyến cho MVC Controllers
// Giữ nguyên cách bạn đã cấu hình default route cho SinhViensController
app.UseEndpoints(endpoints =>
{
    // Đây là route mặc định của bạn. Nó sẽ chuyển hướng đến SinhViensController/Index
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=SinhViens}/{action=Index}/{id?}");

    // Nếu bạn có khu vực (Area) "Admin" và muốn quản lý người dùng ở đó, bạn sẽ cần uncomment và cấu hình route cho Admin Area
    // endpoints.MapControllerRoute(
    //     name: "Admin",
    //     pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");
});

app.Run();
