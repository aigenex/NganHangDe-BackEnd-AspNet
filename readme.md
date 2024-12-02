# Giới thiệu
- Đây là phần backend của Dự Án phát triển ngân hàng đề
- Project Sử dụng [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) + [MongoDb](https://www.mongodb.com/try/download/community)

# Lưu ý trước khi cài đặt
- Bạn cần phải chạy project project ở frontend [#](https://github.com/Hieumih/NganHangDe-FrontEnd.git) 
- Xem câu hỏi ở: http://localhost:5083/question
- Xem mẫu và in ở: http://localhost:5083/exam-preview
- Bạn có thể in bằng cách dùng `Ctrl + P` ở Trình duyệt Chrome

# Tương thích trình duyệt 
- Project có thể tương thích với trình duyệt sau:
  - Chrome > 51
  - Edge > 15
  - Safari > 10
  - FireFox > 54
  - Opera > 38
- Không tương thích với trình duyệt sau
  - Internet Explorer 1 ~ 11 


# Cách chạy
- Mở Visual Studio 2022 và chạy project
- Chỉnh sửa [appsettings.json](NganHangDe-Backend\appsettings.json)
  ```json
  "ExamDb": {
    "ConnectionString": "ĐỊA CHỈ MONGODB",
    "DatabaseName": "ExamBank",
    "ExamCollectionName": "Exams",
    "QuestionCollectionName": "Questions",
    "SubjectCollectionName": "Subjects",
    "UserCollectionName": "Users"
  },
  "Gemini": {
    "ApiKey": "Chìa khóa Gemnini của bạn",
  }
  ```

- Ấn vào nút play để chạy ứng dụng

# Bổ sung
- Trang lấy API https://aistudio.google.com/app/apikey
"# NganHangDe-BackEnd-AspNet" 
