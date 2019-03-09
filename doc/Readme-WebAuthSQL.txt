移除 Microsoft.AspNet.Identity.EntityFramework 改用 SQL 語法實作 ASP.NET Identity.

步驟備忘:

1. 建立網站 WebAuth1 專案
   以 Visual studio 2015, Update 3, 選擇 ASP.NET 4.6 Templates: Web Forms, Authentication: Individual User Account 樣式建立.
     圖 0010-SelectTemplate.jpg
   或 從 https://github.com/github-honda/VS2015TemplateOriginal 取得已建好的專案=WebAuth1.

2. 將 Solution 及 Project 專案名稱改為 WebAuthSQL, 避免跟原專案名稱混淆.

3. 解除安裝 WebAuthSQL 專案的 Microsoft.AspNet.Identity.EntityFramework 元件.
  圖 0010-UninstallEntityFramework1.jpg
  圖 0010-UninstallEntityFramework2.jpg
  
4. 移除參考 EntityFramework 及 EntityFramework.SQLServer.
  圖 0020-RemoveReference.jpg
  
4. 加入並參考 (專案 LibMySQL 及 ZLib)

5. 將專案 WebAuthSQL 中, 所有的 using Microsoft.AspNet.Identity.EntityFramework; 都改成 using LibSQL.DIdentity;
   修改的程式清單如下:
     App_Start\IdentityConfig.cs
     App_Start\Startup.Auth.cs
     Models\IdentityModel.cs
     Account\Manage.aspx.cs
     
6 將 IdentityModel.cs 中, ApplicationUser類別改為繼承 LibSQL.DIdentity.User., ApplicationDbContext 類別改為繼承使用 ZLib.DLib.ZSqlClient 公用程式, 並傳入ConnectionString
  圖 0220-IdentityModel.jpg
  
  
7 IdentifyConfig.cs 中, ApplicationUserManager 類別需要的資料庫改為由 ZLib.DLib.ZSqlClient 提供.
  圖 0230-IdentifyConfig.jpg


8. 將 ConnnectionString 改到新的資料庫位置
    <add name="DefaultConnection" connectionString="Data Source=127.0.01;Initial Catalog=DBAspNet1;Integrated Security=True" providerName="System.Data.SqlClient" />

   原為:	
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-WebBase1Auth-20180924123716.mdf;Initial Catalog=aspnet-WebBase1Auth-20180924123716;Integrated Security=True" providerName="System.Data.SqlClient" />

9. 建立新資料庫, 移除舊資料庫.
  參考 CreateDBAspNet-SqlServer.sql 建立 SQL Server 新資料庫 DBAspNet1
  移除舊資料庫 LocalDB
    App_Data\*.MDF
    App_Data\*.LDF

10. 編譯執行測試.
