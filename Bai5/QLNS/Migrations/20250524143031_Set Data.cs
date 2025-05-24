using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNS.Migrations
{
    /// <inheritdoc />
    public partial class SetData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //them du lieu category
            migrationBuilder.InsertData(
               table: "Categories",
               columns: new[] { "CategoryId", "CategoryName" },
               values: new object[,]
               {
                    { 1, "Công nghệ" },
                    { 2, "Lập trình" },
                    { 3, "Kinh tế" },
                    { 4, "Lịch sử" },

               });

            //them cho book
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Title", "Description", "Author", "Price", "CategoryId", "ImageUrl" },
                values: new object[,]
                {
                    { 1, "Lập trình C#", "Giáo trình C#", "Nguyễn Văn A", 120000m, 1, "SachC#.png" },
                    { 2, "AI Cơ bản", "Nhập môn trí tuệ nhân tạo", "Trần Văn C", 135000m, 1, "SachAI.png" },
                    { 3, "HTML & CSS", "Học lập trình web", "Lê Văn B", 95000m, 1, "SachHTML.png" },
                    { 4, "Truyện Kiều", "Tác phẩm văn học cổ điển", "Nguyễn Du", 80000m, 2, "SachTruyenKieu.png" },
                    { 5, "Bí quyết kinh doanh", "Kinh nghiệm khởi nghiệp", "Trần Văn B", 95000m, 3, "SachKD.png" },
                    { 6, "Lịch sử Việt Nam", "Tổng quan lịch sử Việt Nam", "Lê Minh", 70000m, 4, "SachLichSu.png" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa dữ liệu từ bảng Books
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 });

            // Xóa dữ liệu từ bảng Categories
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValues: new object[] { 1, 2, 3, 4 });

        }
    }
}
