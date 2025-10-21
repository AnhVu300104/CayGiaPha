Bước 1: Cài đặt công cụ
1.    Visual Studio 2022 (Vào Visual Studio Installer => Individual components sau đó cài .NET 6.0)
2.    SQL Server 2014
Bước 2: Clone hoặc tải source code
git clone: https://github.com/AnhVu300104/CayGiaPha
Hoặc tải ZIP từ GitHub → giải nén.
Bước 3: Chuẩn bị database
1.    Mở SQL Server 2014
2.    Tạo database mới, ví dụ: CVDB.
3.    Mở file database/FamilyTreeDb.sql → nhấn Execute để tạo bảng và dữ liệu mẫu.
Bước 4: Cấu hình kết nối database
- Tiếp theo mở Visual Studio 2022 -> Open a local folder và chọn file project vừa clone hoặc giải nén để mở dự án
- Kích đúp vào file CayGiaPha.sln
- Chọn project CayGiaPha → Set as Startup Project
- Vào file appsettings.json để cấu hình tìm đến:
 "AllowedHosts": "*",
  "ConnectionStrings": {
	"DefaultConnection": "Server=DESKTOP-9M157Q2\\SQLEXPRESS;Database=CVDB;Trusted_Connection=True;Connect Timeout=30;"
  }
+  Thay Database bằng tên database bạn vừa tạo trong SQL Server, ví dụ: FamilyTreeDb.
+  Thay DESKTOP-9M157Q2\\SQLEXPRESS bằng tên máy chủ SQL của bạn.
+  Nếu dùng SQL Server Authentication, thêm thông tin User Id và Password vào chuỗi kết nối, ví dụ:
"DefaultConnection": "Server=YOUR_SERVER;Database=FamilyTreeDb;User Id=sa;Password=yourpassword;Connect Timeout=30;"
- Nếu bạn muốn cập nhật database bằng EF Core, có thể thêm hướng dẫn:
 Add-Migration DBUpdate(Tên khác cũng được)   # Tạo migration mới từ các thay đổi model
Update-Database          # Áp dụng migration vào database
Bước 5: Chạy dự án
1.    Nhấn F5 để chạy (IIS Express) hoặc Ctrl + F5.
2.    Truy cập trình duyệt: http://localhost:5062 hoặc http://localhost:{port} (thường là tự động mở)
Bước 6: Deploy (Tùy chọn)
Deploy lên IIS, Azure App Service, hoặc hosting cá nhân:
1.    Build project → Publish → chọn folder hoặc server.
2.    Cập nhật connection string trên server nếu cần.
Các gói đã cài đặt:
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore
Các thao tác Git
• Clone repo:
git clone https://github.com/AnhVu300104/CayGiaPha.git
# Tải toàn bộ source code từ GitHub về máy tính
• Commit thay đổi:
git add .
# Thêm tất cả các file thay đổi vào staging area
git commit -m "Mô tả thay đổi"
# Lưu các thay đổi vào local repository với thông điệp mô tả
git push origin main
# Đẩy các commit từ local repository lên nhánh main trên GitHub
 
